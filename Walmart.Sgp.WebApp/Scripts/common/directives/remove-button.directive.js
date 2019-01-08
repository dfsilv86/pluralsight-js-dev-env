(function() {
	'use strict';

	angular
		.module('common')
		.directive('removeButton', RemoveButton);

	function RemoveButton() {
		return {
			restrict: 'E',
			replace: true,
			transclude: true,
			scope: false,
			template: '<button type="button" title="{{::\'doRemove\'|translate|capitalize}}"><i class="glyphicon glyphicon-remove"></i><span ng-transclude></span></button>',
			link: function ($scope, elem, attrs) {
			    if (!attrs.title) {
			        $(elem).attr('title', window.globalization.texts.doRemove);
			    }
			}
		};
	}
})();