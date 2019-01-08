(function () {
    'use strict';

    angular
        .module('common')
        .directive('timePicker', TimePicker)
        .directive('greaterThan', GreaterThan)
        .directive('lesserThan', LesserThan);

    function read(x) {
        if (angular.isUndefined(x) || x == null) return x;
        return parseInt(x.toString().replace(/[^0-9]/gi, ''), 10);
            }

    function TimePicker() {
        return {
            restrict: 'E',
            require: 'ngModel',
            template: '<input type="text" ui-inputmask-time class="military-time-picker form-control input-width-small" />',
            replace: true,
            link: function ($scope, elm, attrs, ngModel) {
                ngModel.$parsers.push(read);
                ngModel.$formatters.push(window.formatMilitaryTime);
        }
            }
    }

    function GreaterThan() {

        return {
            restrict: 'A',
            require: 'ngModel',
            link: function ($scope, elm, attrs, ngModel) {
                ngModel.$validators.greaterThan = function (modelVal, viewVal) {
                    if (angular.isUndefined(modelVal) || null == modelVal || '' === modelVal) {
                        return true;
        }
                    if (angular.isUndefined(attrs.greaterThan) || null == attrs.greaterThan || '' == attrs.greaterThan) {
                        return true;
                    }

                    return read(viewVal) > read(attrs.greaterThan);
            }
            }
        }
    }

    function LesserThan() {

        return {
            restrict: 'A',
            require: 'ngModel',
            link: function ($scope, elm, attrs, ngModel) {
                ngModel.$validators.lesserThan = function (modelVal, viewVal) {
                    if (angular.isUndefined(modelVal) || null == modelVal || '' === modelVal) {
                        return true;
        }
                    if (angular.isUndefined(attrs.lesserThan) || null == attrs.lesserThan || '' == attrs.lesserThan) {
                        return true;
        }

                    return read(viewVal) < read(attrs.lesserThan);
        }
                }
                    }
            }
})();