(function () {
    'use strict';

    angular
        .module('common')
        .directive('uiInputmaskDate', UiInputMask);

    function UiInputMask() {        
        return {
            require: 'ngModel',            
            scope: false,
            link: function ($scope, element, attrs, ngModel) {
                ngModel.$formatters.push(formatter);
                ngModel.$parsers.push(parser);                
                function formatter(fromModelValue) {
                    var textVal = fromModelValue;
                    if (angular.isDate(fromModelValue)) {
                        textVal = moment(fromModelValue).format($scope.options.format.toUpperCase());
                    }
                    
                    element.val(textVal);
                    return element.inputmask($scope.options.format.toLowerCase(), {
                        placeholder: $scope.options.formatPlaceholder,
                        showMaskOnFocus: false,
                        showMaskOnHover: false,
                        clearIncomplete: true
                    }).val();
                }
                function parser(fromViewValue) {
                    return fromViewValue;
                }                
            }
        };        
    }

})();