/*global angular*/
(function () {
    'use strict';

    function DirectRequestService() {
        function dateToString(dateValue) {
            return JSON.stringify(dateValue).replace('"', '').replace('"', '');
        }

        function createInputFor(propName, propVal, form, noInputOnUndefined) {
            if (propVal == undefined) {
                if (noInputOnUndefined) {
                    return;
                }

                propVal = '';
            }

            if (angular.isArray(propVal)) {
                if (propVal.length > 0) {                    
                    angular.forEach(propVal, function (i, item) {
                        createInputFor(propName + '[' + i + ']', item, form, noInputOnUndefined);
                    });
                }                
            }
            else if (angular.isObject(propVal) && !angular.isDate(propVal)) {
                angular.forEach(propVal, function(subPropName, subPropVal) {
                    createInputFor(propName + '.' + subPropName, subPropVal, form, noInputOnUndefined);
                });
            } else {
                var input = angular.element('<input>');
                input.prop('type', 'hidden');
                input.prop('name', propName);
                var val = angular.isDate(propVal) ? dateToString(propVal) : propVal.toString();
                input.val(val);
                form.append(input);
            }
        }

        function createFormFor(obj, noInputOnUndefined) {
            var form = angular.element('<form>');

            angular.forEach(obj, function (propVal, propName) {
                createInputFor(propName, propVal, form, noInputOnUndefined);
            });

            return form;
        }

        function execute(url, params, method) {
            var form = createFormFor(params, false);
            form.prop('method', (method || 'POST').toUpperCase());
            form.prop('action', url);
            form.prop('target', '_blank');
            form[0].submit();
        }

        this.execute = execute;
    }

    angular.module('common').service('DirectRequestService', DirectRequestService);
}());