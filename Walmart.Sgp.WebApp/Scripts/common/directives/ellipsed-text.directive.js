(function () {
    'use strict';

    // <td ellipsed-text="{{width}}"> {{contents}} </td>

    angular
        .module('common')
        .directive('ellipsedText', EllipsedText);

    EllipsedText.$inject = ['$interpolate'];
    function EllipsedText($interpolate) {
        return {
            restrict: 'A',
            scope: false,
            priority: 2,
            link: function ($scope, elem, attrs) {
                var elemTemplate = elem.html();
                var interpolateFn = $interpolate(elemTemplate, true);
                elem.html('<div class="unbreakable" style="margin-right: -12px;"></div>');
                var $contentElem = elem.find('.unbreakable');
                elem.css('padding-right', 0);
                $scope.$watch(interpolateFn, function (value) {
                    $contentElem.attr('title', value).html(value);
                });
                attrs.$observe('ellipsedText', function (newVal) {
                    $contentElem.css('width', newVal);
                    if (elem[0].nodeName === 'TD' || elem[0].nodeName === 'TH') {
                        elem.css('width', newVal);
                    }
                });
            }
        };
    }

})();