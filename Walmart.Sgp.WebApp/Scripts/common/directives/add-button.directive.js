(function() {
	'use strict';

	angular
		.module('common')
		.directive('addButton', AddButton);

	function AddButton() {
		return {
			restrict: 'E',
			replace: true,
			transclude: true,
			scope: false,
			template: '<button type="button"><i class="glyphicon glyphicon-plus"></i><span ng-transclude></span></button>',
			link: function ($scope, elem, attrs) {
			    if (!attrs.title) {
			        $(elem).attr('title', window.globalization.texts.doAdd);
			    }
			}
		};
	}
})();