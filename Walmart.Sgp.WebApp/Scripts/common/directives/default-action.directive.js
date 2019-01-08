(function () {

    angular
        .module('common')
        .directive('defaultAction', ButtonSubmitPreventionDirective);

    function ButtonSubmitPreventionDirective() {
        return {
            restrict: 'AC',
            scope: false,
            link: ButtonLink
        };

        function ButtonLink($scope, elem, attr) {

            if (!attr.defaultAction) return;

            if (elem[0].nodeName === 'INPUT') {
                elem.on('keyup', function (e) {
                    if (e.keyCode === 13 && !e.shiftKey && !e.metaKey) {
                        with ($scope) {
                            eval(attr.defaultAction);
                        }
                    }
                });
            }
        }
    }
})();