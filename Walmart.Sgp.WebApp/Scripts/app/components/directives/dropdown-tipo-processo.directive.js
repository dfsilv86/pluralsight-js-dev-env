(function () {
    'use strict';

    angular
        .module('SGP')
        .directive('dropdownTipoProcesso', DropdownTipoProcesso);

    DropdownTipoProcesso.$inject = ['$q', 'ProcessoService'];

    function DropdownTipoProcesso($q, ProcessoService) {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'Scripts/app/components/directives/dropdown-tipo-processo.template.html',
            scope: {
                ngModel: '='
            },
            link: function ($scope, elem, attr) {
                $scope.dropdownName = attr.name || 'dropdownTipoProcesso';
            },
            controller: ['$scope', function ($scope) {

                $scope.data = { values: [] };

                var deferred = $q
                    .when(ProcessoService.obterTodos())
                    .then(applyValues);

                function applyValues(values) {                    
                    $scope.data.values = values;                                     
                }
            }]
        };
    }
})();