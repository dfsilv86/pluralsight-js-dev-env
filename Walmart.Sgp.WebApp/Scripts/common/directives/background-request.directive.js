(function () {
    'use strict';

    // <element background-request="request do escopo" background-request-filter="variavel do escopo">

    // onde request do escopo é um objeto criado pelo BackgroundRequestFactory
    // onde variavel do escopo é alguma coisa que foi atribuida para a propriedade subject do bgrequest antes de chamar .perform()
    // a intenção do subject/filter é diferenciar qual item em uma grid está realizando a requisicao
    // (vide sugestao de pedido)

    angular
        .module('common')
        .directive('backgroundRequest', BackgroundRequestDirective);
   

    BackgroundRequestDirective.$inject = ['$interpolate'];
    function BackgroundRequestDirective($interpolate) {
        return {
            restrict: 'A',
            scope: false,
            link: function ($scope, elem, attrs) {
                var interpolateFn = angular.noop, cancelWatch = angular.noop;
                attrs.$observe('backgroundRequest', function (newVal, oldVal) {
                    if (newVal === oldVal) {
                        return;
                    }
                    cancelWatch();

                    // TODO: subject deveria ser um array?
                    var filter = (!!attrs.backgroundRequestFilter) ? '&& {0}.subject == {1}'.format(attrs.backgroundRequest, attrs.backgroundRequestFilter) : '';

                    interpolateFn = !!newVal ? $interpolate("{{!!{0} && {0}.isPerforming {1} }}".format(attrs.backgroundRequest, filter), true) : angular.noop;

                    if (interpolateFn) {
                        cancelWatch = $scope.$watch(interpolateFn, function (newVal) {
                            elem.toggleClass('loading', newVal === 'true');
                        });
                    }
                });
            }
        };
    }

})();