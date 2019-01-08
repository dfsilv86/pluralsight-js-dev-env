(function () {
    'use strict';

    angular
        .module('common')
        .service('WebApiService', WebApiService);

    WebApiService.$inject = ['StorageService', '$injector', 'ApiEndpoints'];

    function WebApiService(storageService, $injector, apiEndpoints) {
        this.setLocalVersion = function(version) {
            storageService.set('_localVersion', version);
            this._localVersion = version;
        };

        this.getLocalVersion = function() {
            this._localVersion = this._localVersion || storageService.get('_localVersion');
            return this._localVersion;
        };
        
        this.getRemoteVersion = function () {
            return $injector.get('$http')
                .get(apiEndpoints.sgp.login + "WebApiVersion")
                .then(function (response) {
                    return response.data;
                });
        };

        this.getServerName = function () {
            return $injector.get('$http')
                .get(apiEndpoints.sgp.login + "ServerName")
                .then(function (response) {
                    return response.data;
                });
        };
    }
})();