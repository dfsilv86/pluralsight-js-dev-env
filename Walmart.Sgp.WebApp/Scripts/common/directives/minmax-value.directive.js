(function () {
    'use strict';

    angular
        .module('common')
        .directive('minValue', MinValue)
        .directive('maxValue', MaxValue);

    function read(x) {
        if (angular.isUndefined(x) || x == null) return x;
        return parseFloat(x.toString().replace(/[^0-9,]/gi, '').replace(',', '.'), 10);
    }

    function MinValue() {

        return {
            restrict: 'A',
            require: 'ngModel',
            link: function ($scope, elm, attrs, ngModel) {
                ngModel.$validators.minValue = function (modelVal, viewVal) {
                    if (angular.isUndefined(modelVal) || null == modelVal || '' == modelVal) {
                        return true;
                    }
                    if (angular.isUndefined(attrs.minValue) || null == attrs.minValue || '' == attrs.minValue) {
                        return true;
                    }

                    return read(viewVal) >= read(attrs.minValue);
                }
            }
        }
    }

    function MaxValue() {

        return {
            restrict: 'A',
            require: 'ngModel',
            link: function ($scope, elm, attrs, ngModel) {
                ngModel.$validators.maxValue = function (modelVal, viewVal) {
                    if (angular.isUndefined(modelVal) || null == modelVal || '' == modelVal) {
                        return true;
                    }
                    if (angular.isUndefined(attrs.maxValue) || null == attrs.maxValue || '' == attrs.maxValue) {
                        return true;
                    }

                    return read(viewVal) <= read(attrs.maxValue);
                }
            }
        }
    }
})();