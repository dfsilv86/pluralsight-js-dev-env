(function () {

    angular
        .module('common')
        .directive('grabFocus', GrabFocusDirective);

    GrabFocusDirective.$inject = ['$interpolate', '$timeout'];
    function GrabFocusDirective($interpolate, $timeout) {
        return {
            restrict: 'A',
            scope: false,
            link: GrabFocusLink
        };

        function GrabFocusLink($scope, elem, attr) {

            if (!attr.grabFocus) return;

            var interpolateFn = $interpolate("{{" + attr.grabFocus + "}}", true);
            var $elem = $(elem);

            var check = function (newVal, oldVal) {
                if (newVal === 'true' && newVal !== oldVal) {
                    $timeout(function () { // suppress-validator

                        if ($elem.is('select[ui-select2]')) {
                            $elem.select2('focus');
                        } else {
                            $elem.focus();
                        }
                    }, 100);
                }
            };

            check(interpolateFn($scope));

            if (interpolateFn) {
                $scope.$watch(interpolateFn, check);
            }
        }
    }
})();