(function () {
    'use strict';

    angular
        .module('common')
        .factory('BackgroundRequestFactory', BackgroundRequestFactory)

    window.$$backgroundRequestFactoryStack = [];

    // Factory para requisições GET em background.
    //   Atua como um wrapper em torno do método do serviço Js.
    //   Retorna um objeto com duas propriedades - value e isPerforming - e dois métodos - perform() e cancel().
    //   Seta value para null antes de iniciar a requisição, e para o resultado caso requisição concluída com sucesso.
    //
    // Forma de uso, na controller da tela:
    //
    // $scope.valorDeFoo = backgroundRequestFactory(fooService.obterValorFoo);
    //
    // No(s) campo(s) em tela:
    //   (marca o elemento com a classe loading enquanto a requisição estiver sendo executada)
    //
    // <campo background-request="valorDeFoo">{{valorDeFoo.value}}</campo>
    //
    // Para iniciar a requisição:
    //
    // $scope.valorDeFoo.perform(parametros do servico);
    //
    // Para cancelar a requisição (no Limpar, por ex. - não cancela a requisição mas evita que um valor inválido apareça na tela)
    //
    // $scope.valorDeFoo.cancel();
    //
    
    BackgroundRequestFactory.$inject = ['$q', '$log'];
    function BackgroundRequestFactory($q, $log) {
        return function (performFn, clearFn) {

            // TODO: subject deveria ser um array?
            var state = { deferred: $q.defer(), subject: null };

            var backgroundRequest = new BackgroundRequest();

            backgroundRequest.perform = function () {

                backgroundRequest.cancel();

                backgroundRequest.value = null;

                state.deferred = $q.defer();

                var thisRequestPromise = $q.defer();

                state.deferred.promise.then(function (value) {
                    backgroundRequest.value = value;
                    thisRequestPromise.resolve(value);
                    $log.debug('Requisição background: concluída.');
                }, function (reason) {
                    backgroundRequest.value = null;
                    thisRequestPromise.reject(reason);
                    $log.debug('Requisição background: rejeitada (' + (reason||'nenhum motivo informado') + ').');
                }).finally(function () {
                    backgroundRequest.isPerforming = false;
                });

                window.$$backgroundRequestFactoryStack.push(true);

                try {
                    backgroundRequest.isPerforming = true;

                    $log.debug('Requisição background: iniciando.');

                    return performFn.apply(null, arguments).then(function (value) {
                        state.deferred.resolve(value);
                    }, function (reason) {
                        state.deferred.reject(reason);
                    });
                } catch (e) {
                    state.deferred.reject(e);
                } finally {
                    window.$$backgroundRequestFactoryStack.pop();
                }

                // retorna uma promise separada para ser usada por quem chamar o perform()
                return thisRequestPromise.promise;
            };

            backgroundRequest.cancel = function (reason) {
                state.deferred.reject(reason);
            };

            return backgroundRequest;
        }
    }

    function BackgroundRequest() {
        this.value = null;
        this.isPerforming = false;
    }
})();