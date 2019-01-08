(function () {
    'use strict';

    angular.module('common')
      .factory('LoaderInterceptor', LoaderInterceptor);

    LoaderInterceptor.$inject = ['$q', 'SpaConfig', 'LoaderService', 'ToastService'];

    function LoaderInterceptor($q, spaConfig, loaderService, toastService) {
        return {
            'request': function (config) {
                // Não fechar os toasts quando for GET.
                // Ao fechar os toasts aqui, a tela de relacionamento de itens não mostra o toast ao salvar a primeira vez
                // pois quando salva, ela sai da rota novo e vai para a rota de edição não mostrando a mensagem que foi
                // salvo com sucesso.
                // TODO: aparentemente podemos tirar isso agora, porém é necessário testar em diversos cenários                
                // keepNotifications informa que a chamada não se importa se houver algum aviso em tela
                if (config.method !== 'GET' && !config.keepNotifications) {
                    toastService.dismissAll();
                }

                if (!config.background) {
                    var url = config.url;
                    // Garante que os templates html da app sejam recarregados 
                    // toda vez que uma nova versão for publicada.
                    if (url.indexOf('Scripts/app') === 0 || url.indexOf('Scripts/common') === 0 || url.indexOf('Content/') === 0) {
                        config.url = url.asVersioned(spaConfig.appVersion);
                    }

                    loaderService.push(config.url);
                }

                return config;
            },

            'requestError': function (rejection) {
                loaderService.pop(rejection.config.url);
                return $q.reject(rejection);
            },

            'response': function (response) {
                if (!response.config.background) {
                    loaderService.pop(response.config.url);
                }

                return response;
            },

            'responseError': function (rejection) {
                loaderService.pop(rejection.config.url);
                return $q.reject(rejection);
            }
        };
    }
})();