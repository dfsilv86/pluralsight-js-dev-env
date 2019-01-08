(function () {
    'use strict';

    angular.module('common')
      .factory('ProcessingInterceptor', ProcessingInterceptor);

    ProcessingInterceptor.$inject = ['$q', '$rootScope'];

    function ProcessingInterceptor($q, $rootScope) {
        return {
            'response': function (response) {

                var ticketId = response.headers('x-processing-ticket');

                if (ticketId !== null) {

                    var processOrder = {
                        ticket: response.headers('x-processing-ticket'),
                        state: response.headers('x-processing-state'),
                        currentProgress: response.headers('x-processing-current-progress'),
                        totalProgress: response.headers('x-processing-total-progress'),
                        message: response.headers('x-processing-status-message'),
                        resultIsFile: (response.headers('x-processing-result-type-name') || '').contains('FileVault'),
                        processName: response.headers('x-processing-name'),
                        createdDate: moment(response.headers('x-processing-created-date') || new Date()).toDate(),
                        modifiedDate: moment(response.headers('x-processing-modified-date')).toDate()
                };

                    $rootScope.$broadcast('processing-ticket-notification', processOrder);
                }

                return response;
            }
        };
    }
})();