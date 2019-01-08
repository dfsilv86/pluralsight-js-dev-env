(function () {
    'use strict';

    angular
		.module('common')
		.directive('scrollTopButton', ScrollTopButton);

    ScrollTopButton.$inject = ['$window', '$timeout'];
    function ScrollTopButton($window, $timeout) {
        return {
            restrict: 'E',
            replace: true,
            scope: false,
            template: '<a class="scroll-to-top" title="{{::\'doScrollTop\'|translate|capitalize}}"><span class="glyphicon glyphicon-chevron-up"></span>&nbsp;{{::\'doScrollTop\'|translate}}</a>',
            link: function ($scope, elem, attrs) {

                var theWindow = $($window);

                if (!attrs.title) {
                    elem.attr('title', window.globalization.texts.doScrollTop);
                }

                if (attrs.type === 'link') {
                    elem.removeClass('btn').removeClass('btn-default');
                } else if (attrs.type === 'button') {
                    elem.addClass('btn').addClass('btn-default');
                }

                elem.on('click.scrollToTop', function () {
                    window.scrollTo(0, 0);
                });

                function check() {
                    checkTimeout = null;
                    if (theWindow.scrollTop() > 10) {
                        elem.show();
                    } else {
                        elem.hide();
                    }
                }

                var checkTimeout = null;
                theWindow.on('scroll.scrollToTop', function (evt) {
                    if (!checkTimeout) {
                        checkTimeout = $timeout(check, 100, false); // suppress-validator
                    }
                });

                $scope.$on("$destroy", function () {
                    if (checkTimeout) $timeout.cancel(checkTimeout); // suppress-validator
                    elem.off('click.scrollToTop');
                    theWindow.off('scroll.scrollToTop');
                });

                check();
            }
        };
    }
})();