(function () {
    'use strict';

    angular.module('common')
      .factory('WebApiVersionInterceptor', WebApiVersionInterceptor);

    WebApiVersionInterceptor.$inject = ['$q', 'WebApiService', '$injector',  'ToastService', '$window', '$log'];

    function WebApiVersionInterceptor($q, webApi, $injector, toast, $window, $log) {
        return {
            'response': function (response) {
                if (response.config.url.indexOf('/WebApiVersion') != -1) {
                    return response;
                }

                var localVersion = webApi.getLocalVersion();                
                var remoteVersion = response.headers('x-web-api-version');

                if (remoteVersion !== null) {
                    if (localVersion == null) {
                        $log.debug("First access, setting WebApi local version to:{0}".format(remoteVersion));
                        webApi.setLocalVersion(remoteVersion);
                    }
                    else if (localVersion != remoteVersion) {
                        $log.debug("WebApi local version: {0} and remote version:{1}".format(localVersion, remoteVersion));

                        webApi.setLocalVersion(remoteVersion);
                        var confirm = $injector.get('ConfirmService');
                        confirm.open(
                        {
                            message: globalization.texts.youAreUsingAnOldAppVersion,
                            yes: function () {
                                toast.warning(globalization.texts.newAppVersionBeenLoaded);
                                $window.location.reload();
                            }
                        });
                    }
                }

                return response;
            }
        };
    }
})();