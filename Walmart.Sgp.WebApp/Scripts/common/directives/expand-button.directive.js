(function() {
	'use strict';

	angular
		.module('common')
		.directive('expandButton', DetailButton);

	function DetailButton() {
		return {
			restrict: 'E',
            scope: {
                ngModel: '=',
			    expand: '&',
			    collapse: '&',
			    isExpanded: '&',
			    canExpand: '&?'                
			},
            template: '<button type="button" ng-if="canExpand(ngModel)" ng-click="toggle()" ng-attr="{title:(isExpanded(ngModel)?\'collapse\':\'expand\')}"><i class="glyphicon" ng-class="{\'glyphicon-minus\':isExpanded(ngModel),\'glyphicon-plus\':!isExpanded(ngModel)}"></i></button>',
            controller: ['$scope', function DetailButtonController($scope) {
                $scope.toggle = function () {
                    if ($scope.isExpanded($scope.ngModel)) {
                        $scope.collapse($scope.ngModel);
                    } else {
                        $scope.expand($scope.ngModel);
                    }
                };
            }]
		};
	}
})();