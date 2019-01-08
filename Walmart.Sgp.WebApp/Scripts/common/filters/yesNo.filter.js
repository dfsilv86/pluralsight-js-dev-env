/*global angular*/
(function () {
    angular.module('common').filter('yesNo', ['$translate', function ($translate) {
        return function (input) {
            return input ?
                    $translate.instant('yes') :
                    $translate.instant('no');
        };
    }]);
}());