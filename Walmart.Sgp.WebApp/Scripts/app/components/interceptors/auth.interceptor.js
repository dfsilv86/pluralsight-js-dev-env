(function () {
    'use strict';

    angular.module('SGP')
      .factory('AuthInterceptor', ['UserSessionService', function (userSession) {
          return {
              'request': function (config) {

                  if (userSession.hasStarted()) {
                      config.headers['Authorization'] = 'Bearer ' + userSession.getToken(); // jshint ignore:line
                  }

                  return config;
              }
          };
      }]);
})();