(function () {
	'use strict';

	// Configuração da controller
	angular
        .module('SGP')
        .config(PesquisaParametroFornecedorRoute)
        .controller('PesquisaParametroFornecedorController', PesquisaParametroFornecedorController);

	// Implementação da controller
	PesquisaParametroFornecedorController.$inject = ['$scope', '$q', '$timeout', '$stateParams', '$state', 'ValidationService', 'FornecedorParametroService', 'PagingService'];
	function PesquisaParametroFornecedorController($scope, $q, $timeout, $stateParams, $state, $validation, fornecedorParametroService, pagingService) {

		var ordenacaoPadrao = 'cdV9D asc';

		$validation.prepare($scope);

		$scope.filters = $stateParams.filters || { nmFornecedor: null, cdV9D: null, stFornecedor: null, cdSistema: null };

		$scope.data = { values: null };
		$scope.data.paging = $stateParams.paging || { offset: 0, limit: 10, orderBy: ordenacaoPadrao };

		$scope.search = search;
		$scope.clear = clear;
		$scope.detail = detail;
		$scope.orderBy = orderBy;

		if (!!$scope.filters && !!$scope.filters.didSearch) {
		    // paging já foi persistido e carregado com a última configuração, não é necessário usar $timeout.
		    search();
		}

		function clear() {
			escondeFornecedores();

			//Seta o valor do campo status como "Ativo"

			var ativo = sgpFixedValues.tipoStatusItem[0].value;

			$scope.filters.stFornecedor = ativo;

			$scope.filters.cdV9D = $scope.filters.nmFornecedor = null;
		}

		function orderBy(field) {
		    pagingService.toggleSorting($scope.data.paging, field);
			search();
		}

		function exibeFornecedores(data) {
			$scope.data.values = data;

			pagingService.acceptPagingResults($scope.data.paging, data);

			$scope.filters.didSearch = true;
			$validation.accept($scope);
        }

		function escondeFornecedores() {
			$scope.data.values = null;
			$scope.filters.didSearch = false;
        }

		function search(pageNumber) {

			if (!$validation.validate($scope)) return;

			pagingService.calculateOffset($scope.data.paging, pageNumber);

			// Prepara os parâmetros de pesquisa
			var stFornecedor = $scope.filters.stFornecedor;			
			var cdSistema = $scope.filters.cdSistema;
			var cdV9D = $scope.filters.cdV9D;
			var nmFornecedor = $scope.filters.nmFornecedor;

			// Requisição ao serviço
			var deferred = $q
                .when(fornecedorParametroService.pesquisarPorFiltro(cdSistema, stFornecedor, cdV9D, nmFornecedor, $scope.data.paging))
                .then(exibeFornecedores)
                .catch(escondeFornecedores);
		}

		function detail(item) {

		    $state.update({
			    filters: $scope.filters,
                paging: $scope.data.paging
			});

			$state.go('detalheParametroFornecedor', {
			    'id': item.idFornecedorParametro
			});
		}
	}

	// Configuração do estado
	PesquisaParametroFornecedorRoute.$inject = ['$stateProvider'];
	function PesquisaParametroFornecedorRoute($stateProvider) {

		$stateProvider
            .state('pesquisaParametroFornecedor', {
                url: '/vendor/parametro',
                params: {
                    filters: null,
                    paging: null
                },
            	templateUrl: 'Scripts/app/gerenciamento/pesquisa-parametro-fornecedor.view.html',
            	controller: 'PesquisaParametroFornecedorController'
            });
	}
})();