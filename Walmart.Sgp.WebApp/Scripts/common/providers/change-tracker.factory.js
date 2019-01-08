(function () {
    'use strict';

    angular
        .module('common')
        .factory('ChangeTrackerFactory', ChangeTrackerFactory);

    function ChangeTrackerFactory() {

        var changeTrackerFactory = {

            createChangeTracker: function (identityDelegate, checkDelegate, copyDelegate) {

                // identityDelegate é function(a, b) {} e retorna se a é b (comparando pela PK, ou outra property da entidade)

                // checkDelegate é function (item) {} e retorna se o item foi modificado (comparando valores atuais por valores originais)

                // copyDelegate é function (item, direction) {} e copia os valores relevantes, onde direction é 0 para salvar valores originais e 1 para restaurar valores originais (undo)

                if (!identityDelegate || typeof (identityDelegate) !== 'function') throw "Invalid identity delegate.";

                if (!checkDelegate || typeof (checkDelegate) !== 'function') throw "Invalid check delegate.";

                if (!copyDelegate || typeof (copyDelegate) !== 'function') throw "Invalid copy delegate.";

                return new ChangeTracker(identityDelegate, checkDelegate, copyDelegate);
            },

            createChangeTrackerForProperties: function (propertyList, identityDelegate) {

                // propertyList é um array com nomes de propriedades

                // identityDelegate é function(a, b) {} e retorna se a é b (comparando pela PK, ou outra property da entidade)

                if (!propertyList || !angular.isArray(propertyList)) throw "Invalid property list.";
                if (!identityDelegate || typeof (identityDelegate) !== 'function') throw "Invalid identity delegate.";

                var check = function (item) {
                    var hasChanges = false;
                    angular.forEach(propertyList, function (value) {
                        var values = value.split('.');
                        if (values.length == 1)
                            hasChanges = hasChanges || (item[value] != item['original_' + value]);
                        else
                            hasChanges = hasChanges || (item[values[0]][values[1]] != item['original_' + value]);
                    });
                    return hasChanges;
                };

                var copy = function (item, direction) {
                    if (direction === 1) {
                        // rollback - restaura valores originais
                        angular.forEach(propertyList, function (value) {
                            var values = value.split('.');
                            if (values.length == 1)
                                item[value] = item['original_' + value];
                            else
                                item[values[0]][values[1]] = item['original_' + value];
                        });
                    } else {
                        // begin tran - copia valores originais para outra prop
                        angular.forEach(propertyList, function (value) {
                            var values = value.split('.');
                            if (values.length == 1)
                                item['original_' + value] = item[value];
                            else
                                item['original_' + value] = item[values[0]][values[1]];
                        });
                    }
                };

                return new ChangeTracker(identityDelegate, check, copy);
            }
        };

        return changeTrackerFactory;
    }

    function ChangeTracker(identityDelegate, checkDelegate, copyDelegate) {
        this._identity = identityDelegate;
        this._check = checkDelegate;
        this._copy = copyDelegate;
        this._tracked = [];
    }
    ChangeTracker.prototype._identity = null;
    ChangeTracker.prototype._check = null;
    ChangeTracker.prototype._copy = null;
    ChangeTracker.prototype._tracked = null;

    // Rastreia um item (ou todos itens de uma lista), substituindo o(s) item(s) por aqueles 
    // que já estão na lista de rastreados caso tenham sofrido alguma modificação.
    // O array retornado é o mesmo informado no parametro, e este pode ter sido modificado.
    ChangeTracker.prototype.track = function (itemOrList) {

        this._removeUnmodifiedItems();

        if (!angular.isArray(itemOrList)) {
            return this._trackItem(itemOrList);
        } else {
            return this._trackList(itemOrList);
        }
    };

    ChangeTracker.prototype.inherit = function (itemOrList) {

        this._removeUnmodifiedItems();
        if (!angular.isArray(itemOrList)) {
            this._tracked.push(itemOrList);
        } else {
            this._tracked = this._tracked.concat(itemOrList);
        }
    };

    // Verifica se algum item na lista de objetos rastreados possui alguma alteração.
    ChangeTracker.prototype.hasChanges = function () {

        var result = false;

        for (var i = 0; i < this._tracked.length && !result; i++) {
            result = result || this._check(this._tracked[i]);
        }

        return result;
    };

    // Retorna a lista de objetos modificados.
    ChangeTracker.prototype.getChangedItems = function () {

        var self = this, result = [];

        angular.forEach(this._tracked, function (item) {
            if (self._check(item)) {
                result.push(item);
            }
        });

        return result;
    };

    // Zera a lista de objetos rastreados, sem descartar modificações.
    ChangeTracker.prototype.reset = function () {
        this._tracked.splice(0, this._tracked.length);
    };

    // Sobrescreve os valores originais, zerando a contagem de modificacoes pendentes.
    ChangeTracker.prototype.commitAll = function () {
        var self = this;
        angular.forEach(this._tracked, function (item) {
            self._copy(item, 0);
        });
    };

    // Sobrescreve os valores originais apenas para os objetos onde identityDelegate retorna true.
    ChangeTracker.prototype.commitByIdentity = function (identityDelegate) {

        // identityDelegate é function(item) { return [bool] }

        var self = this;
        angular.forEach(this._tracked, function (item) {
            if (identityDelegate(item)) {
                self._copy(item, 0);
            }
        });
    };

    // Desfaz alterações de todos os itens rastreados
    ChangeTracker.prototype.undoAll = function () {
        var self = this;
        angular.forEach(this._tracked, function (item) {
            self._copy(item, 1);
        });
    };

    // Desfaz as alterações de um item específico
    ChangeTracker.prototype.undo = function (item) {
        this._copy(item, 1);
        return item;
    };

    // Desfaz as alterações apenas para os objetos onde identityDelegate retorna true.
    ChangeTracker.prototype.undoByIdentity = function (identityDelegate) {

        // identityDelegate é function(item) { return [bool] }

        var self = this;
        angular.forEach(this._tracked, function (item) {
            if (identityDelegate(item)) {
                self._copy(item, 1);
            }
        });
    };

    // Obtem da lista de rastreados o item modificado que corresponde ao item informado, caso exista
    ChangeTracker.prototype.getTrackedItem = function (item) {

        for (var i = 0; i < this._tracked.length; i++) {
            if (this._identity(this._tracked[i], item)) {
                return this._tracked[i];
            }
        }

        return null;
    };

    // Rastreia uma lista de itens, retornando os itens em suas versões rastreadas/modificadas, caso existam
    ChangeTracker.prototype._trackList = function (list) {

        for (var i = 0; i < list.length; i++) {
            var theItem = list[i];
            list[i] = this._trackItem(theItem);
        }
        return list;
    };

    // Rastreia um item, retornando o item em sua versão rastreada/modificada, caso exista
    ChangeTracker.prototype._trackItem = function (item) {

        var tracked = null;
        if (tracked = this.getTrackedItem(item)) {
            item = tracked;
        } else {
            this._prepare(item);
        }
        return item;
    };

    // Prepara um item (salva valores originais) e adiciona na lista de objetos rastreados
    ChangeTracker.prototype._prepare = function (item) {

        this._copy(item);

        this._tracked.push(item);
    };

    // Remove da lista de rastreados aqueles objetos que não sofreram modificações
    ChangeTracker.prototype._removeUnmodifiedItems = function () {

        for (var i = this._tracked.length - 1; i >= 0; i--) {

            var trackedItem = this._tracked[i];

            if (!this._check(trackedItem)) {

                this._tracked.splice(i, 1);
            }
        }
    };

})();