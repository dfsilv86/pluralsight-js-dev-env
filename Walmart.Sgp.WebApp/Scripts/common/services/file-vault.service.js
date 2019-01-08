(function () {
    'use strict';

    angular
        .module('common')
        .service('FileVaultService', FileVaultService);

    FileVaultService.$inject = ['$q', '$http', 'ApiEndpoints', '$window'];

    function FileVaultService($q, $http, apiEndpoints, $window) {
     
        var self = this;

        self.downloadSerialized = function (serialized) {
            var url = "{0}Ticket/DownloadSerialized?serialized={1}".format(apiEndpoints.sgp.fileVault, serialized);
            $window.location.href = url;
        };

        self.download = function (ticketId, ticketCreatedDate) {
            var url = "{0}Ticket/Download?ticketId={1}&ticketCreatedDate={2}".format(apiEndpoints.sgp.fileVault, ticketId, ticketCreatedDate);            
            $window.location.href = url;
        };
    }
})();