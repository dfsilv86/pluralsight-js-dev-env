(function() {
	'use strict';

	angular
		.module('common')
		.directive('editButton', EditButton);

	function EditButton() {
		return {
			restrict: 'E',
			replace: true,
			transclude: true,
			scope: false,
			template: '<button type="button" title="{{::\'doEdit\'|translate|capitalize}}"><i class="glyphicon glyphicon-pencil"></i><span ng-transclude></span></button>',
			link: function ($scope, elem, attrs) {
			    if (!attrs.title) {
			        $(elem).attr('title', window.globalization.texts.doEdit);
			    }
			}
		};
	}
})();