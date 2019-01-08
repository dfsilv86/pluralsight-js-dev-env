(function () {
    'use strict';

    angular
          .module('SGP')
          .controller('ModalDetalheConsultaReturnSheetRAController', ModalDetalheConsultaReturnSheetRAController);

    ModalDetalheConsultaReturnSheetRAController.$inject = ['$scope', 'UserSessionService', 'sugestaoReturnSheet'];
    function ModalDetalheConsultaReturnSheetRAController($scope, userSessionService, sugestaoReturnSheet) {
        $scope.filters = {};
        $scope.filters.bandeira = userSessionService.getCurrentUser().bandeiraId;

        setVendor(sugestaoReturnSheet);

        $scope.sugestaoReturnSheet = sugestaoReturnSheet;

        function setVendor(sugestaoReturnSheet) {
            var vendor = {
                cdSistema: 1,
                cdV9D: sugestaoReturnSheet.fornecedorParametro.cdV9D,
                fornecedor: {
                    nmFornecedor: sugestaoReturnSheet.fornecedor.nmFornecedor,
                    cdFornecedor: sugestaoReturnSheet.fornecedor.cdFornecedor
                }
            };

            $scope.filters.vendor = vendor;
            $scope.filters.cdV9D = vendor.cdV9D;
        }
    }
})();