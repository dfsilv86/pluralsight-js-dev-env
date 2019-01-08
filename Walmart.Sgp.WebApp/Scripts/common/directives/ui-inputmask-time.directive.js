(function () {
    'use strict';

    angular
        .module('common')
        .directive('uiInputmaskTime', UiInputMask);

    function UiInputMask() {        
        return {
            require: 'ngModel',            
            scope: false,
            link: function ($scope, element, attrs, ngModel) {
                ngModel.$formatters.push(formatter);
                ngModel.$parsers.push(parser);                
                function formatter(fromModelValue) {
                    var textVal = fromModelValue;                    
                    
                    element.val(textVal);
                    return element.inputmask('hh:mm', { placeholder: 'HH:MM', showMaskOnFocus: false, showMaskOnHover: false, clearIncomplete: true }).val();
                }
                function parser(fromViewValue) {
                    return fromViewValue;
                }                
            }
        };        
    }

})();