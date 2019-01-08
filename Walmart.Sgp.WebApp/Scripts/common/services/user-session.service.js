(function () {
    'use strict';

    angular
        .module('common')
        .service('UserSessionService', UserSessionService);

    UserSessionService.$inject = ['$rootScope', 'StorageService', '$q', '$log', '$interval', 'SpaConfig'];

    function UserSessionService($rootScope, storageService, $q, $log, $interval, spaConfig) {

        var self = this;

        var lockInterval = null;

        if (!spaConfig.activityTimeout) {
            spaConfig.activityTimeout = 20;
        }

        function stopLockInterval() {
            if (!!lockInterval) {
                //$log.debug("StopLockInterval");
                $interval.cancel(lockInterval);
                lockInterval = null;
            }
            $(window).off('mousemove.activityTimeout').off('keydown.activityTimeout');
        }
        var ust = 0;
        function startLockInterval() {
            try {
                stopLockInterval();
            } catch(e) {
                $log.error(e);
            }
            //$log.debug("StartLockInterval");
            lockInterval = $interval(function () {
                // Verifica se houve atividade.
                if (!!window._hasActivity) {
                    self._info._lastUserActivity = new Date();
                    self._setInfo(self._info);
                    window._hasActivity = false;
                    //$log.debug("Teve atividade");
                }
                var sharedInfo = self._getInfo();
                // Verifica se possui atividade armazenada no localstorage - que é compartilhado entre abas
                // Se houver, e for mais recente, sincroniza aqui com a outra aba
                if (!!sharedInfo._lastUserActivity && (!self._info._lastUserActivity || sharedInfo._lastUserActivity > self._info._lastUserActivity)) {
                    self._info._lastUserActivity = sharedInfo._lastUserActivity;
                    self._setInfo(self._info);
                    //$log.debug("Teve atividade em outra aba");
                }
                // Se não tem indício de atividade
                if (!self._info._lastUserActivity) {
                    self.lock();
                    //$log.debug("Lock");
                }
                // se o último indício de atividade foi há mais tempo que o limite, então bloqueia.
                var lastActivityTimestamp = (new Date() - self._info._lastUserActivity);
                var currentTimeout = (spaConfig.activityTimeout * 60 * 1000);
                if (lastActivityTimestamp > currentTimeout) {
                    self.lock();
                    //$log.debug("Lock");
                } else {
                    var tmp = Math.floor((currentTimeout - lastActivityTimestamp) / 1000);
                    if (tmp !== ust){
                        //$log.debug("Faltam {0} s para bloquear".format(tmp));
                        ust = tmp;
                    }
                }
            }, 1000, false);
            $(window).on('mousemove.activityTimeout', resetActivityTimeout).on('keydown.activityTimeout', resetActivityTimeout);
            function resetActivityTimeout(event) {
                window._hasActivity = true;
            }
        }

        this.initialize = function () {

            if (!this._info || !this._info._lastUserActivity || (new Date() - this._info._lastUserActivity) > (spaConfig.activityTimeout * 60 * 1000)) {
                this._info = null;
                this._setInfo(this._info);
            } else {
                startLockInterval();
            }
        };

        this.reset = function () {
            this._info = null;
            this._setInfo(this._info);
        };

        this.start = function (info) {
            this._info = info;
            this._info._lastUserActivity = new Date();
            this._setInfo(this._info);

            var restriction = this.getRestricaoLoja();

            var deferred = $q.defer();

            deferred.promise.finally(function () {
                $rootScope.$broadcast('session-start');
                startLockInterval();
            });

            if (!!restriction) {

                if (!!restriction.idLoja) {

                    // Here be dragons
                    // TODO: arrumar as dependencias
                    var lojaService = $('body').injector().get('LojaService');

                    lojaService.obterEstruturadoPorId(restriction.idLoja).then(function (loja) {
                        self._loja = loja;
                        self._setLoja(self._loja);
                        deferred.resolve();
                    });

                } else if (!!restriction.restrict) {

                    $log.error('Informações do usuário indicam restrição por loja, porém nenhum idLoja foi encontrado.');
                    deferred.resolve();

                } else {

                    self._loja = null;
                    self._setLoja(null);
                    deferred.resolve();
                }
                // Se for necessário adicionar outra situação aqui, lembre-se de utilizar o deferred.resolve() para que o finally da promisse aconteça
                // e o broadcast do 'session-start' ocorra.
            } else {

                deferred.resolve();
            }
        };

        this.end = function () {
            stopLockInterval();
            this.reset();
            $rootScope.$broadcast('session-end');
        };

        this.lock = function (isManual) {

            stopLockInterval();

            var theUser = this.getCurrentUser();

            this._info = { user: { userName: theUser.userName, idLoja: theUser.idLoja, idExternoPapel: theUser.idExternoPapel, fullName: theUser.fullName } };
            this._setInfo(this._info);

            $rootScope.$broadcast('lock-screen', { isManual: isManual || false });
        };

        this.changePassword = function (userName) {

            $rootScope.$broadcast('change-password', { userName: userName });
        };

        this.hasStarted = function () {
            return this._info !== null;
        };

        this.getCurrentUser = function () {
            return (this._info || {}).user;
        };

        this.getRestricaoLoja = function () {
            var tmp = (this._info || {}).user || {};

            return {
                idLoja: tmp.idLoja,
                restrict: tmp.hasAccessToSingleStore,
                loja: this._loja
            };
        };

        this.canAccessMenu = function (routeTemplate, route) {

            var user = this.getCurrentUser();

            if (user != null) {
                var menus = user.menus;

                for (var r in menus) {
                    var testRoute = menus[r].route;

                    // Se for algo como /item/relacionamento/manipulado ou /item/:id/custos (apenas id é aceito).
                    if (route.contains(testRoute) || (routeTemplate.contains(':id') && routeTemplate.contains(testRoute))) {
                        return true;
                    }
                }
            }

            return false;
        };

        this.canAccessAction = function (actionId) {

            var user = this.getCurrentUser();

            if (user != null) {
                var actions = user.actions;

                for (var a in actions) {
                    var testAction = actions[a];

                    if (typeof testAction === 'object' && actionId.toUpperCase() == testAction.id.toUpperCase()) {
                        return true;
                    }
                }
            }

            return false;
        };

        this.getToken = function () {
            return (this._info || {}).token;
        };

        this.pushSessionToResume = function (stateName, stateParams) {
            this._stateName = stateName;
            this._stateParams = stateParams;
        };

        this.popSessionToResume = function () {

            if ((this._stateName || null) === null)
                return null;

            var result = {
                name: this._stateName,
                params: this._stateParams
            };

            this._stateName = this._stateParams = null;

            return result;
        };

        this.readUserPreference = function (category, elementKey, propertyName, defaultValue) {

            var userId = (this.getCurrentUser() || {}).Id || 0;

            var userKey = 'user-preferences[' + (userId).toString() + ']';

            var prefs = {};

            try {
                prefs = angular.fromJson(storageService.get(userKey)) || {};
            } catch (e) {
                $log.warn('Não foi possível ler as preferências de usuário: ' + (e || '(nenhum motivo informado)'));
            }

            var gridPrefs = prefs[category] || {};

            var thisGridPrefs = gridPrefs[elementKey] || {};

            var storedValue = thisGridPrefs[propertyName] || defaultValue || null;

            return storedValue;
        };

        this.saveUserPreference = function (category, elementKey, propertyName, value) {

            var userId = (this.getCurrentUser() || {}).Id || 0;

            var userKey = 'user-preferences[' + userId.toString() + ']';

            var prefs = {};

            try {
                prefs = angular.fromJson(storageService.get(userKey)) || {};
            } catch (e) {
                $log.warn('Não foi possível ler as preferências de usuário: ' + (e || '(nenhum motivo informado)'));
            }

            var gridPrefs = prefs[category] || {};

            var thisGridPrefs = gridPrefs[elementKey] || {};

            if (value === null || angular.isUndefined(value)) {
                delete thisGridPrefs[propertyName];
            } else {
                thisGridPrefs[propertyName] = value;
            }

            gridPrefs[elementKey] = thisGridPrefs;

            prefs[category] = gridPrefs;

            storageService.set(userKey, angular.toJson(prefs));
        };

        this._setInfo = function (info) {
            storageService.set('_info', JSON.stringify(info));
        };

        this._getInfo = function () {
            var info = storageService.get('_info') || null;

            if (typeof info === 'string') {
                return JSON.parse(info);
            }

            return info;
        };

        this._setLoja = function (loja) {
            storageService.set('_loja', JSON.stringify(loja));
        };

        this._getLoja = function () {
            var loja = storageService.get('_loja') || null;

            if (typeof loja === 'string') {
                return JSON.parse(loja);
            }

            return loja;
        };

        this._info = this._getInfo();
        this._loja = this._getLoja();

        $rootScope.$on('$destroy', stopLockInterval);
    }
})();