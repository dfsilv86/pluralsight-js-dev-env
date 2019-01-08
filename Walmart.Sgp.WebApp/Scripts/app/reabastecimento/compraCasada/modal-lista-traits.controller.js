(function () {
    'use strict';

    angular
        .module('SGP')
        .controller('ModalListaTraitsController', ModalListaTraitsController);

    ModalListaTraitsController.$inject = ['$scope', '$q', 'PagingService', 'ItemDetalheService', 'itemDetalhe'];

    function ModalListaTraitsController($scope, $q, pagingService, itemDetalheService, itemDetalhe) {

        $scope.itemDetalheEntrada = itemDetalhe;
        $scope.data = {};
        $scope.data.paging = { offset: 0, limit: 10, orderBy: 'cdLoja' };
        $scope.orderBy = orderBy;
        $scope.search = search;

        search();

        function orderBy(field) {
            if ($scope.data.values != null && $scope.data.values.length > 0) {
                $scope.data.paging.orderBy = ($scope.data.paging.orderBy || '').indexOf(field + ' asc') >= 0 ?
                    field + ' desc' :
                    field + ' asc';
                search();
            }
        }

        function search(pageNumber) {
            pagingService.calculateOffset($scope.data.paging, pageNumber);

            var idItemDetalhe = $scope.itemDetalheEntrada.idItemDetalhe;
            var cdSistema = 1;

            var deferred = $q
                 .when(itemDetalheService.obterTraitsPorItem(idItemDetalhe, cdSistema, $scope.data.paging))
                 .then(applyValue);
        }

        function applyValue(data) {
            $scope.data.values = data;

            pagingService.acceptPagingResults($scope.data.paging, data);
        }
    }

})();