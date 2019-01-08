/// <reference path="../../sgp.fixed-values.js" />
(function () {
    'use strict';

    function FormFieldDepartamentoCategoria() {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'Scripts/app/components/directives/' +
                'form-field-departamento-categoria.template.html',
            scope: {
                sistema: '=sistema',                
                departamento: '=departamento',
                categoria: '=categoria',
                divisao: '=?divisao',
                departamentoItem: '=',
                categoriaItem: '=',
                descriptionWidth: '@descriptionWidth',
                lineWidth: '@lineWidth',
                gridWidth: '@gridWidth',
                ngRequired: '=ngRequired'
            },
            link: function ($scope) {
                $scope.titleKey = '';
                $scope.sistemas = {
                    supercenter: sgpFixedValues.tipoSistema.supercenter,
                    samsClub: sgpFixedValues.tipoSistema.samsClub
                };

                $scope.vm = {
                    departamento: null,
                    departamentoItem: null,
                    categoria: null,
                    categoriaItem: null
                };

                $scope.$watch('sistema', function (newVal, oldVal) {
                    if (newVal != oldVal) {
                        if (newVal == $scope.sistemas.supercenter) {
                            $scope.vm.categoria = null;
                        }
                        else if (newVal == $scope.sistemas.samsClub) {
                            $scope.vm.departamento = null;
                        }
                    }
                });

                $scope.$watch('vm.departamento', function (newVal, oldVal) {
                    if (newVal != oldVal) {
                        $scope.departamento = newVal;
                    }
                });

                $scope.$watch('departamento', function (newVal, oldVal) {
                    if (newVal != $scope.vm.departamento) {
                        $scope.vm.departamento = newVal;
                    }
                });

                $scope.$watch('categoria', function (newVal, oldVal) {
                    if (newVal != $scope.vm.categoria) {
                        $scope.vm.categoria = newVal;
                    }
                });

                $scope.$watch('vm.departamentoItem', function (newVal, oldVal) {
                    if (newVal != oldVal) {
                        $scope.departamentoItem = newVal;
                    }
                });

                $scope.$watch('vm.categoria', function (newVal, oldVal) {
                    if (newVal != oldVal) {
                        $scope.categoria = newVal;
                    }
                });

                $scope.$watch('vm.categoriaItem', function (newVal, oldVal) {
                    if (newVal != oldVal) {
                        $scope.categoriaItem = newVal;
                    }
                });
            }
        };
    }

    angular.module('SGP')
        .directive('formFieldDepartamentoCategoria',
            FormFieldDepartamentoCategoria);
}());