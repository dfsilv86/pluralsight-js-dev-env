(function () {
    'use strict';


    // Configuração da controller
    angular
        .module('SGP')
        .config(CustosItensRelacionadosRoute)
        .controller('CustosItensRelacionados', CustosItensRelacionados);


    // Implementação da controller
    CustosItensRelacionados.$inject = ['$scope', '$q', 'custos' ];
    function CustosItensRelacionados($scope, $q, custos) {

        $scope.data = {
            custosPrincipal: [],
            custosSecundario: []
        };

        $q.when(custos)
            .then(function (result) {
                for (var i = 0; i < result.length; i++) {
                    if (result[i].isPrincipal) {
                        $scope.data.custosPrincipal.push(result[i]);
                    } else {
                        $scope.data.custosSecundario.push(result[i]);
                    }
                }
            });

        $scope.back = back;

        function back() {
            alert("TODO: voltar");
        }
    }


    // Configuração do estado
    CustosItensRelacionadosRoute.$inject = ['$stateProvider'];
    function CustosItensRelacionadosRoute($stateProvider) {

        $stateProvider
            .state('custosItensRelacionados', {
                url: '/item/:id/custos/loja/:loja/custos-relacionados',
                templateUrl: 'Scripts/app/item/custos-itens-relacionados.view.html',
                controller: 'CustosItensRelacionados',
                resolve: {
                    custos: ['$stateParams', 'ItemDetalheService', function ($stateParams, service) {
                        return service.obterUltimoCustoDeItensRelacionadosNaLoja($stateParams.id, $stateParams.loja);
                    }]
                }
            });
    }

})();