(function () {
    'use strict';

    angular
        .module('common')
        .directive('formField', FormField);

    function makeTitle(value) {
        return !!value ? value + ':' : '';
    }

    // Gridwidth: o col-md-xx dentro do conteudo do form-field (12 = 100% do espaço disponivel do lado do label)
    // Sugestoes:
    // Para lookups: 12
    // Para inputs normais: 10
    // Para dropdowns de dominios de descrição curta: 2-4
    function makeGridWidth(value) {
        return parseInt(value, 10) || 6;
    }

    // Linewidth: o col-md-xx no nível do <form><div.row>
    // Para forms com duas colunas de campos: 6
    // Para forms com um campo ocupando uma linha inteira: 12
    function makeLineWidth(value) {
        return (parseInt(value, 10) || 12) - 2;
    }

    function FormField() {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'Scripts/common/directives/form-field.template.html',
            transclude: true,
            scope: true,
            link: function ($scope, elem, attr) {
                $scope.formField = {
                    title: makeTitle(attr.title),
                    lineWidth: makeLineWidth(attr.lineWidth),
                    gridWidth: makeGridWidth(attr.gridWidth)
                };
                attr.$observe('title', function (value) {
                    $scope.formField.title = makeTitle(value);
                });
                attr.$observe('gridWidth', function (value) {
                    $scope.formField.gridWidth = makeGridWidth(value);
                });
                attr.$observe('lineWidth', function (value) {
                    $scope.formField.lineWidth = makeLineWidth(value);
                });
            }
        };
    }
})();