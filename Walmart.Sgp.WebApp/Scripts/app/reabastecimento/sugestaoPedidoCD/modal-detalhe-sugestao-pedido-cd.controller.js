(function () {
    'use strict';

    angular
          .module('SGP')
          .controller('ModalDetalheSugestaoPedidoCDController', ModalDetalheSugestaoPedidoCDController);

    ModalDetalheSugestaoPedidoCDController.$inject = ['$scope', '$q', 'UserSessionService', 'sugestaoPedidoCD'];
    function ModalDetalheSugestaoPedidoCDController($scope, $q, userSessionService, sugestaoPedidoCD) {

        $scope.item = sugestaoPedidoCD;
        $scope.filters = {};
        setVendor(sugestaoPedidoCD);

        function setVendor(sugestaoPedidoCD) {
            var vendor = {
                cdSistema: 1,
                cdV9D: sugestaoPedidoCD.fornecedorParametro.cdV9D,
                fornecedor: {
                    nmFornecedor: sugestaoPedidoCD.fornecedorParametro.nmFornecedor
                }
            };

            $scope.filters.vendor = vendor;
            $scope.filters.cdV9D = vendor.cdV9D;
        }
    }
})();