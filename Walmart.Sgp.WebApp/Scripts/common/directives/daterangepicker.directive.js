(function () {
    'use strict';

    angular.module('common')
        .directive('daterangepicker', [function () {

            return {
                restrict: 'E',
                templateUrl: 'Scripts/common/directives/daterangepicker.directive.html',
                requires: 'ngModel',
                replace: true,
                scope: {
                    startDate: '=dateStart',
                    endDate: '=dateEnd',
                    ngRequired: '=?'
                },
                link: function (scope, element, attrs) {

                    function setMaxDate(val) {
                        if (scope.startDateControll && typeof scope.startDateControll.setMaxDate === 'function') {
                            scope.startDateControll.setMaxDate(val);
                        }
                    }

                    function setMinDate(val) {
                        if (scope.endDateControll && typeof scope.endDateControll.setMinDate === 'function') {
                            scope.endDateControll.setMinDate(val);
                        }
                    }

                    scope.datepickerNameStart = attrs.nameStart || 'data';
                    scope.datepickerNameEnd = attrs.nameEnd || 'data';
                    scope.isRequired = angular.isDefined(attrs.$attr.required);

                    scope.resetDates = function ($event) {
                        if ($($event.currentTarget).children().val() === "") {
                            if ($($event.currentTarget).attr('name') == "receiveIntervalStart") {
                                scope.startDate = null;
                            }
                            else if ($($event.currentTarget).attr('name') == "receiveIntervalEnd") {
                                scope.endDate = null;
                            }
                        }
                    };

                    scope.$watch('startDate', function (newVal, oldVal) {
                        if (!newVal) {
                            setMinDate(null);
                            return;
                        }
                        
                        if (newVal == oldVal || newVal == scope.endDate || !scope.endDate) {
                            return;
                        }

                        if (newVal > scope.endDate) {                            
                            scope.startDate = scope.endDate;
                        }

                        setMinDate(scope.startDate);                        
                    });

                    scope.$watch('endDate', function (newVal, oldVal) {
                        if (!newVal) {
                            setMaxDate(null);
                            return;
                        }
                                                
                        if (newVal.getHours() == 0) {                            
                            var endOfDay = moment(newVal).endOf('day').toDate();                            
                            scope.endDate = endOfDay;
                        }

                        if (newVal == oldVal || newVal == scope.startDate || !scope.startDate) {
                            return;
                        }

                        if (newVal < scope.startDate) {                            
                            scope.endDate = scope.startDate;
                        }

                        setMaxDate(scope.endDate);
                    });
                }
          };
      }]);
})();