(function () {
    'use strict';

    angular
        .module('common')
        .directive('semaphore', Semaphore);

    function Semaphore() {
        return {
            restrict: 'E',
            replace: true,
            template: '<span class="glyphicon semaphore" ng-class="{\'semaphore-green\':!!(ngModel||(value == \'true\')),\'glyphicon-ok-sign\':!!(ngModel||(value == \'true\')),\'semaphore-red\':!(ngModel||(value == \'true\')), \'glyphicon-remove-sign\':!(ngModel||(value == \'true\'))}"></span>',
            scope: {
                ngModel: '=?',
                value: '@?'
            }
        };
    }

})();