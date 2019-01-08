(function() {
	'use strict';

	angular
		.module('common')
		.directive('detailButton', DetailButton);

	function DetailButton() {
		return {
			restrict: 'E',
			replace: true,
			transclude: true,
			scope: false,
			template: '<button type="button"><i class="glyphicon glyphicon-eye-open"></i><span ng-transclude></span></button>',
			link: function ($scope, elem, attrs) {
			    if (!attrs.title) {
			        $(elem).attr('title', window.globalization.texts.doDetail);
			    }
			}
		};
	}
})();