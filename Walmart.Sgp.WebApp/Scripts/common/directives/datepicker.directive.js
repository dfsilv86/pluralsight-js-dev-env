(function () {
  'use strict';

  angular.module('common')
    .directive('datepicker', ['$filter', '$locale', function ($filter, $locale) {
      return {
        restrict: 'E',
        templateUrl: 'Scripts/common/directives/datepicker.directive.html',
        requires: 'ngModel',
        replace: true,
        scope: {
          ngModel: '=ngModel',
          controll: '=?controll',
          ngDisabled: '=ngDisabled',
          ngStyle: '=ngStyle',
          ngStyleButton: '=ngStyleButton',
          blur: '&ngBlur',
          ngRequired: '=?'
        },
        link: function (scope, element, attrs) {            
          scope.datepickerName = attrs.name || 'data';
          scope.isRequired = angular.isDefined(attrs.$attr.required);

          var filter = $filter('translate');

          /**
           * Determina se uma data é válida
           */
          var isValidDate = function (date) {

            // É uma String?
            if (typeof date === 'string') {

              // Tenta converter para Date
              var timestamp = Date.parse(date);
              return isFinite(timestamp) === true;
            }

            // É uma data?
            if (date instanceof Date) {

              // Então está tudo bem
              return true;
            }

            return false;
          };

          /**
           * Seta o valor de uma data dentro do scope
           */
          var setDate = function (dateName, newDate) {              
            var date = null;

            if (isValidDate(newDate) === true || newDate === null) {
              date = newDate !== null ? new Date(newDate) : null;
              scope[dateName] = date;
            }
          };

          /**
           * Seta o valor de maxDate
           */
          var setMaxDate = function (maxDate) {
            if (angular.isDefined(maxDate)) {
              setDate('maxDate', maxDate);
            }
          };

          /**
           * Seta o valor de minDate
           */
          var setMinDate = function (minDate) {
            if (angular.isDefined(minDate)) {
              setDate('minDate', minDate);
            }
          };

          scope.init = function () {
            scope.controll = scope.controll || {};
            scope.controll.setMaxDate = setMaxDate;
            scope.controll.setMinDate = setMinDate;

            scope.options = {
              format: attrs.format || 'dd/MM/yyyy',
              currentText: filter('today'),
              clearText: filter('clear'),
              closeText: filter('close'),
              formatPlaceholder: attrs.placeholder || 'DD/MM/AAAA'
            };

            scope.vm = {
              value: null,
              isOpen: false
            };
          };

          /**
            * Habilita/desabilita o calendário
            */
          scope.toggle = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();

            scope.vm.isOpen = !scope.vm.isOpen;
          };

          scope.$watch('vm.value', function (newValue, oldValue) {
            if (newValue === oldValue) {
              return;
            }

            if (angular.isDefined(scope.vm.value) && (isValidDate(scope.vm.value) || scope.vm.value === null)) {
              if (scope.vm.value instanceof Date) {
                  scope.ngModel = scope.vm.value; //.toISOString();
              } else if (scope.vm.value === null || typeof scope.vm.value === 'string') {
                scope.ngModel = scope.vm.value;
              }
            }

            if (angular.isUndefined(scope.vm.value)) {
                scope.ngModel = null;
            }

          }, true);

          scope.$watch('ngModel', function (newVal, oldVal) {
              scope.vm.value = newVal;            
          });

          scope.isDisabled = function () {
             if (typeof scope.ngDisabled !== 'boolean') {
               scope.ngDisabled = null;
             }
             else {
               scope.ngDisabled = "disabled";
             }
          };

          scope.init();
        }
      };
    }]);
})();
