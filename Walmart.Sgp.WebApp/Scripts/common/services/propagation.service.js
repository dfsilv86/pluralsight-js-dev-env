(function () {
	'use strict';

	angular
		.module('common')
		.service('PropagationService', PropagationService);

	// Bug #4301 - lida com as inconsistências do banco em relação ao cadastro de estrutura mercadológica.

	PropagationService.$inject = ['$log']
	function PropagationService($log) {

		var self = this;

		// Prepara o escopo de uma tela para tratar eventos de propagação das lookups.
		// Caso não seja usado, as lookups vão depender do watchGroup normalmente.
		self.prepare = function prepare($scope) {

			if (angular.isUndefined($scope.propagateEnabled)) {

				$scope.$on('propagate-request', propagate);

				$scope.propagateEnabled = true;
			}
		};

		// Prepara o escopo de uma lookup para inibir o refresh() por conta de um watchGroup.
		self.prepareLookup = function prepareLookup($scope) {

			if (angular.isUndefined($scope.propagateEnabled)) {

				$scope.$on('propagate-start', function (event, propagateData) {
					$scope.propagateOwner = propagateData.owner || 'unknown';
				});

				$scope.$on('propagate-end', function (event, propagateData) {
					if ($scope.propagateOwner !== (propagateData.owner || 'unknown')) {
						$log.warn('PropagateService: recebida notificação de término de propagação de outro dono.')
					}
					$scope.propagateOwner = null;
				});

				$scope.isPropagating = function () {
					return !!$scope.propagateOwner;
				};

				// O refresh fica lá para retrocompatibilidade
				$scope.refresh = function () {

					if ($scope.isPropagating()) return;

					originalRefresh();
				};

				$scope.propagateEnabled = true;

				var originalRefresh = $scope.refresh;
			}
		};

		function propagate(event, propagateData) {

			event.currentScope.$broadcast('propagate-start', propagateData);

			event.currentScope.$broadcast('propagate-data', propagateData);

			// Não é necessário aguardar o próximo ciclo de digest aqui porque o evento propagate-data seta internamente o itemOut de cada lookup,
			//   que faz com que o refresh() não pense que houve alteração
			//
			// Caso o refresh() esteja agindo para resetar mesmo depois do propagate-data, então é um bug.
			//
			// Eventualmente podemos mudar a regra no refresh de forma a eliminar o reset por causa da propagação, 
			//   já que isso será feito no propagate-data
			event.currentScope.$broadcast('propagate-end', propagateData);
		}
	}
})();