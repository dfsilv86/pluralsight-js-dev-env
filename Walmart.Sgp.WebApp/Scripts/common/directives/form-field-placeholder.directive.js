(function () {
    'use strict';

    angular
        .module('common')
        .directive('formFieldPlaceholder', FormFieldPlaceholder);

    function FormFieldPlaceholder() {
        return {
            restrict: 'E',
            template: '<div class="col-md-6"><div class="row"><div class="form-control" style="visibility:hidden"></div></div></div>',
            replace: true
        };
    }
})();