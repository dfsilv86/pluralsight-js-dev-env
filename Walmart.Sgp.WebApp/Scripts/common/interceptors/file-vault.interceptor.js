(function () {
    'use strict';

    angular.module('common')
      .factory('FileVaultInterceptor', FileVaultInterceptor);

    FileVaultInterceptor.$inject = ['$q', '$injector', '$window', '$log'];

    function FileVaultInterceptor($q, $injector, $window, $log) {
        return {
            'response': function (response) {
                                
                var ticketId = response.headers('x-file-vault-ticket-id');

                if (ticketId !== null) {
                    var ticketCreatedDate = response.headers('x-file-vault-ticket-created-date');
                    $log.debug('File vault ticket: {0} / {1}'.format(ticketId, ticketCreatedDate));

                    var fileVault = $injector.get('FileVaultService');
                    fileVault.download(ticketId, ticketCreatedDate);
                }

                return response;
            }
        };
    }
})();