/// <reference path="../../bower/angular/angular.js" />
(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(ManutencaoRelacionamentoItemRoute)
        .controller('ManutencaoRelacionamentoItemController', ManutencaoRelacionamentoItemController);

    // Implementação da controller
    ManutencaoRelacionamentoItemController.$inject = ['$scope', '$q', '$timeout', '$stateParams', 'ValidationService', '$state', 'ToastService', 'ItemRelacionamentoService', 'itemRelacionamento', 'capitalizeFilter', 'PagingService'];
    function ManutencaoRelacionamentoItemController($scope, $q, $timeout, $stateParams, $validation, $state, toast, service, itemRelacionamento, capitalizeFilter, pagingService) {
        var hasLoaded = !itemRelacionamento.id;        
        $validation.prepare($scope);

        $scope.data = {
            itemRelacionamentoEmEdicao: null,
            itemRelacionamentoEmbalagemEmEdicao: null,
            cdItem: itemRelacionamento.itemDetalhe.cdItem,
            itemRelacionamento: itemRelacionamento,
            itemSecundario: null,            
            itemSecundarioCdItem: null,
            itemSaidaPcRendimentoDerivado: null,
            itemMateriaPrimaPsUnitario: null,
            itemSecundarioEmbalagem: null,
            itemSecundarioEmbalagemCdItem: null,
            itemSecundarioEmbalagemPsItem: null,
            isVinculado: false,
            isManipulado: false,
            isReceituario: false,
            tituloGrid: null,
            tituloItemPrincipal: 'exitItem',
            podeEditarPesoInsumos: itemRelacionamento.id != 0,
            tipoRelacionamento: $stateParams.tipoRelacionamento,
            relacionamentoSecundarioPaginado: [],
            embalagemPaginado: [],
            itemDetalhe: itemRelacionamento.itemDetalhe
        };              

        $scope.data.relacionamentoSecundarioPaginado.offset = 0;
        $scope.data.relacionamentoSecundarioPaginado.limit = 10;
        $scope.data.relacionamentoSecundarioPaginado.totalCount = 0;

        $scope.data.embalagemPaginado.offset = 0;
        $scope.data.embalagemPaginado.limit = 10;
        $scope.data.embalagemPaginado.totalCount = 0;     

        $scope.tipoRelacionamentoCapitalizado = capitalizeFilter($stateParams.tipoRelacionamento);

        $scope.$watchGroup(
            ['data.itemRelacionamento.itemDetalhe'],
            function (newValues, oldValues) {                
                if (newValues[0] !== oldValues[0]) {
                    if (newValues[0] != oldValues[0] && !!newValues[0] && !!newValues[0].id) {
                        if (hasLoaded) {
                            service.validarItem($scope.data.itemRelacionamento, newValues[0], true).catch(function () {                                
                                $scope.data.itemSecundarioCdItem = null;
                            });
                        }
                        else {
                            $scope.data.cdDepartamentoFilter = $scope.data.itemRelacionamento.idTipoRelacionamento != sgpFixedValues.tipoRelacionamento.vinculado
                                ? $scope.data.itemRelacionamento.itemDetalhe.departamento.cdDepartamento
                                : null;

                            hasLoaded = true;
                        }
                    }
                }
            });

        switch ($scope.data.itemRelacionamento.idTipoRelacionamento) {
            case 1:
                $scope.data.titulo = 'attachedPlural';
                $scope.data.isVinculado = true;
                $scope.data.tituloGrid = 'inbound';
                $scope.data.tituloItemPrincipal = 'linkedItem';
                break;
            case 2:
                $scope.data.titulo = 'recipesPlural';
                $scope.data.isReceituario = true;
                $scope.data.tituloGrid = 'rawMaterial';
                break;
            case 3:
                $scope.data.titulo = 'manipulatedPlural';
                $scope.data.isManipulado = true;
                $scope.data.tituloGrid = 'exit';
                $scope.data.tituloItemPrincipal = 'inputItem';
                break;
        }

        $scope.clear = clear;
        $scope.limparSecundario = limparSecundario;
        $scope.limparSecundarioEmbalagem = limparSecundarioEmbalagem;
        $scope.save = save;
        $scope.remove = remove;
        $scope.adicionarItemSecundario = adicionarItemSecundario;
        $scope.adicionarItemSecundarioEmbalagem = adicionarItemSecundarioEmbalagem;
        $scope.removerItemSecundario = removerItemSecundario;
        $scope.removerItemSecundarioEmbalagem = removerItemSecundarioEmbalagem;
        $scope.editarItemSecundario = editarItemSecundario;
        $scope.editarItemSecundarioEmbalagem = editarItemSecundarioEmbalagem;
        $scope.podeRemoverItemSecundario = podeRemoverItemSecundario;
        $scope.podeRemoverItemSecundarioEmbalagem = podeRemoverItemSecundarioEmbalagem;
        $scope.marcarTodos = marcarTodos;
        $scope.marcarTodosEmbalagem = marcarTodosEmbalagem;
        $scope.podeAdicionarItemSecundario = podeAdicionarItemSecundario;
        $scope.podeAdicionarItemSecundarioEmbalagem = podeAdicionarItemSecundarioEmbalagem;
        $scope.back = back;
        $scope.calcularQtProdutoAcabado = calcularQtProdutoAcabado;
        $scope.calcularPcRendimentoReceita = calcularPcRendimentoReceita;
        $scope.custosItem = custosItem;
        $scope.pageRelacionamentoSecundario = pageRelacionamentoSecundario;
        $scope.pageEmbalagem = pageEmbalagem;

        pageRelacionamentoSecundario(1);
        pageEmbalagem(1);

        function calcularQtProdutoAcabado() {
            var rendimentoReceita = $scope.data.itemRelacionamento.pcRendimentoReceita;
            var produtoBruto = $scope.data.itemRelacionamento.qtProdutoBruto;
            var produtoAcabado = ((produtoBruto * rendimentoReceita) / 100);

            $scope.data.itemRelacionamento.qtProdutoAcabado = produtoAcabado;
        }

        function calcularPcRendimentoReceita() {
            var produtoAcabado = $scope.data.itemRelacionamento.qtProdutoAcabado;
            var produtoBruto = $scope.data.itemRelacionamento.qtProdutoBruto;
            var rendimentoReceita = ((produtoAcabado * 100) / produtoBruto);

            $scope.data.itemRelacionamento.pcRendimentoReceita = rendimentoReceita;
        }

        function clear() {
            $state.go('manutencaoRelacionamento' + $scope.tipoRelacionamentoCapitalizado + 'New', null, { reload: true });
        }

        function limparSecundario() {
            var data = $scope.data;
            data.isEditing = false;
            data.itemRelacionamentoEmEdicao = null;
            data.itemSecundario = data.itemSecundarioCdItem = data.itemSaidaPcRendimentoDerivado =
                data.itemMateriaPrimaPsUnitario = data.idRelacionamentoItemPrincipal = data.idRelacionamentoItemSecundario = null;
        }

        function limparSecundarioEmbalagem() {
            var data = $scope.data;
            data.itemRelacionamentoEmbalagemEmEdicao = null;
            data.itemSecundarioEmbalagem = data.itemSecundarioEmbalagemCdItem = data.itemSecundarioEmbalagemPsItem = null;
        }

        function custosItem(id) {
            var params = {
                id: $stateParams.id,
                tipoRelacionamento: $stateParams.tipoRelacionamento,
                idItemDetalhe: id,
            };

            $state.go('custosItem' + capitalizeFilter($stateParams.tipoRelacionamento), params);
        }

        function save() {

            if (!$validation.validate($scope)) return;

            var itemRelacionamento = {};

            angular.copy($scope.data.itemRelacionamento, itemRelacionamento);

            if ($scope.data.isReceituario) {
                itemRelacionamento.relacionamentoSecundario = itemRelacionamento.relacionamentoSecundario.concat(itemRelacionamento.relacionamentoSecundarioEmbalagem);
            }

            service
                .salvar(itemRelacionamento)
                .then(function (data) {                    
                    $state.go(
                    'manutencaoRelacionamento' + $scope.tipoRelacionamentoCapitalizado + 'Edit',
                    {
                        id: data.id,
                    },
                    {
                        reload: true
                    });
                    toast.success(globalization.texts.linkBetweenItemsSavedSuccessfully);
                });
        }

        function remove() {
            service
                .remover($scope.data.itemRelacionamento)
                .then(function () {
                    toast.success(globalization.texts.linkBetweenItemsRemovedSuccessfully);
                    back();
                });
        }

        function back() {
            //$stateParams.tipoRelacionamento = sgpFixedValues.getByValue("tipoRelacionamento", $scope.data.itemRelacionamento.idTipoRelacionamento).normalizedDescription;

            $state.go('pesquisaProduto' + $scope.tipoRelacionamentoCapitalizado);
        }

        function marcarTodos() {
            var relacionamentoSecundario = $scope.data.itemRelacionamento.relacionamentoSecundario;
            relacionamentoSecundario.setAll('marcado', relacionamentoSecundario.marcado);
        }

        function marcarTodosEmbalagem() {
            var relacionamentoSecundarioEmbalagem = $scope.data.itemRelacionamento.relacionamentoSecundarioEmbalagem;
            relacionamentoSecundarioEmbalagem.setAll('marcado', relacionamentoSecundarioEmbalagem.marcado);
        }

        function adicionarItemSecundario() {
            var data = $scope.data;            
            if (!!data.itemRelacionamentoEmEdicao) {
                data.itemRelacionamentoEmEdicao.pcRendimentoDerivado = data.itemSaidaPcRendimentoDerivado;
                data.itemRelacionamentoEmEdicao.psItem = data.itemMateriaPrimaPsUnitario;

                limparSecundario();                

                if (data.isReceituario) {
                    calcularPesoBruto();
                    data.podeEditarPesoInsumos = true;
                }

                $scope.calcularQtProdutoAcabado();
                return;
            }

            var item = data.itemSecundario;
            var psItem = 0;

            if (data.isManipulado) {
                psItem = data.itemSaidaPcRendimentoDerivado;
            }
            else if (data.isReceituario) {
                psItem = data.itemMateriaPrimaPsUnitario;
            }

            data.idRelacionamentoItemSecundario = 0;

            var newItem = {
                idRelacionamentoItemPrincipal: data.itemRelacionamento.idRelacionamentoItemPrincipal,
                idRelacionamentoItemSecundario: data.idRelacionamentoItemSecundario,
                idItemDetalhe: item.idItemDetalhe,
                idDepartamento: item.idDepartamento,
                pcRendimentoDerivado: data.itemSaidaPcRendimentoDerivado,
                psUnitario: data.itemMateriaPrimaPsUnitario,
                dsTamanhoItem: item.dsTamanhoItem,
                tpReceituario: item.tpReceituario,
                tpManipulado: item.tpManipulado,
                tpVinculado: item.tpVinculado,
                tpStatus: item.tpStatus,
                psItem: psItem,
                itemDetalhe: item,
                tpItem: 0,
                cdItem: item.cdItem
            };
            
            data.itemRelacionamento.idItemDetalhe = data.itemRelacionamento.itemDetalhe.idItemDetalhe;

            var itemRelacionamento = {};

            angular.copy($scope.data.itemRelacionamento, itemRelacionamento);

            if ($scope.data.isReceituario) {
                itemRelacionamento.relacionamentoSecundario = itemRelacionamento.relacionamentoSecundario.concat(itemRelacionamento.relacionamentoSecundarioEmbalagem);
            }
            
            var request = { relacionamentoItemPrincipal: itemRelacionamento, itemDetalhe: newItem, isEditing: data.isEditing };

            service.validarAdicaoItemSecundario(request).then(function () {
                limparSecundario();
                data.itemRelacionamento.relacionamentoSecundario.push(newItem);

                if (data.isReceituario) {
                    calcularPesoBruto();
                    data.podeEditarPesoInsumos = true;
                }

                pageRelacionamentoSecundario();

                $scope.calcularQtProdutoAcabado();
            });
        }

        function calcularPesoBruto() {
            $scope.data.itemRelacionamento.qtProdutoBruto = $scope.data.itemRelacionamento.relacionamentoSecundario.sum('psItem');
        }

        function itemEmbalagemJaExiste(idItemDetalhe) {
            var lista = $scope.data.itemRelacionamento.relacionamentoSecundarioEmbalagem;
            return angular.isArray(lista) && lista.some(function (item) {
                return item.itemDetalhe.idItemDetalhe === idItemDetalhe;
            });
        }

        function adicionarItemSecundarioEmbalagem() {
            var data = $scope.data;
            if (!!data.itemRelacionamentoEmbalagemEmEdicao) {                
                data.itemRelacionamentoEmbalagemEmEdicao.psItem = data.itemSecundarioEmbalagemPsItem;
                limparSecundarioEmbalagem();                
                return;
            }
            
            var item = data.itemSecundarioEmbalagem;

            if (itemEmbalagemJaExiste(item.idItemDetalhe)) {
                toast.error(globalization.texts.inputItemAlreadyAddedToOutputItem);
                return;
            }

            var newItem = {
                idItemDetalhe: item.idItemDetalhe,
                idDepartamento: item.idDepartamento,
                psItem: data.itemSecundarioEmbalagemPsItem,
                tpReceituario: item.tpReceituario,
                tpManipulado: item.tpManipulado,
                tpVinculado: item.tpVinculado,
                tpStatus: item.tpStatus,
                itemDetalhe: item,
                tpItem: 1,
                cdItem: item.cdItem
            };

            data.itemRelacionamento.idItemDetalhe = data.itemRelacionamento.itemDetalhe.idItemDetalhe;

            var itemRelacionamento = { relacionamentoItemPrincipal: data.itemRelacionamento, itemDetalhe: newItem, isEditing: data.isEditing };

            service.validarAdicaoItemSecundario(itemRelacionamento).then(function () {
                limparSecundarioEmbalagem();
                data.itemRelacionamento.relacionamentoSecundarioEmbalagem.push(newItem);
                pageEmbalagem();
            });
        }

        function podeAdicionarItemSecundario() {
            var data = $scope.data;

            if (data.isVinculado) {
                return data.itemRelacionamento.itemDetalhe && data.itemSecundario;
            }

            return data.itemRelacionamento.itemDetalhe && data.itemSecundario && ((data.isManipulado && data.itemSaidaPcRendimentoDerivado) || (data.isReceituario && data.itemMateriaPrimaPsUnitario));
        }

        function podeRemoverItemSecundario() {
            var data = $scope.data;

            return !data.itemSecundario && data.itemRelacionamento.relacionamentoSecundario.any('marcado', true);
        }

        function podeAdicionarItemSecundarioEmbalagem() {
            var data = $scope.data;

            return data.itemRelacionamento.itemDetalhe && data.itemSecundarioEmbalagem && data.itemSecundarioEmbalagemPsItem;
        }

        function podeRemoverItemSecundarioEmbalagem() {
            var data = $scope.data;

            return !data.itemSecundarioEmbalagem && data.itemRelacionamento.relacionamentoSecundarioEmbalagem.any('marcado', true);
        }

        function removerItemSecundario() {
            var data = $scope.data;
            var relacionamentoSecundario = data.itemRelacionamento.relacionamentoSecundario;
           
            // Remove os marcados na grid.
            relacionamentoSecundario.removeByProperty('marcado', true);

            if (data.isReceituario) {
                calcularPesoBruto();
            }
            
            pageRelacionamentoSecundario();

            $scope.calcularQtProdutoAcabado();
        }

        function removerItemSecundarioEmbalagem() {
            var data = $scope.data;
            var relacionamentoSecundarioEmbalagem = data.itemRelacionamento.relacionamentoSecundarioEmbalagem;

            // Remove os marcados na grid.
            relacionamentoSecundarioEmbalagem.removeByProperty('marcado', true);            

            pageEmbalagem();
        }

        function editarItemSecundario(secundario) {
            var data = $scope.data;
            data.itemRelacionamentoEmEdicao = secundario;
            data.itemSecundario = secundario.itemDetalhe;
            data.itemSecundarioCdItem = secundario.itemDetalhe.cdItem;
            data.itemSaidaPcRendimentoDerivado = secundario.pcRendimentoDerivado;
            data.itemMateriaPrimaPsUnitario = secundario.psItem;
            data.isEditing = true;
            data.idRelacionamentoItemPrincipal = secundario.idRelacionamentoItemPrincipal;
            data.idRelacionamentoItemSecundario = secundario.idRelacionamentoItemSecundario;            
        }

        function editarItemSecundarioEmbalagem(secundario) {
            var data = $scope.data;
            data.itemRelacionamentoEmbalagemEmEdicao = secundario;
            data.itemSecundarioEmbalagem = secundario.itemDetalhe;
            data.itemSecundarioEmbalagemCdItem = secundario.itemDetalhe.cdItem;
            data.itemSecundarioEmbalagemPsItem = secundario.psItem;
            data.isEditing = true;
        }

        function pageRelacionamentoSecundario(pageNumber) {
            
            var paged = $scope.data.relacionamentoSecundarioPaginado;
            var source = $scope.data.itemRelacionamento.relacionamentoSecundario;
            
            pagingService.inMemoryPaging(pageNumber, source, paged);
        }

        function pageEmbalagem(pageNumber) {

            var paged = $scope.data.embalagemPaginado;
            var source = $scope.data.itemRelacionamento.relacionamentoSecundarioEmbalagem || [];

            pagingService.inMemoryPaging(pageNumber, source, paged);
        }
    }

    // Configuração do estado
    ManutencaoRelacionamentoItemRoute.$inject = ['$stateProvider'];
    function ManutencaoRelacionamentoItemRoute($stateProvider) {

        // VINCULADO
        $stateProvider
            .state('manutencaoRelacionamentoVinculadoNew', {
                url: '/item/relacionamento/vinculado/new',
                params: {
                    tipoRelacionamento: 'vinculado'
                },
                templateUrl: 'Scripts/app/item/manutencao-relacionamento-item.view.html',
                controller: 'ManutencaoRelacionamentoItemController',
                resolve: {
                    itemRelacionamento: ['$stateParams', 'ItemRelacionamentoService', function ($stateParams, service) {
                        return service.obterNovo($stateParams.tipoRelacionamento);
                    }]
                }
            })
            .state('manutencaoRelacionamentoVinculadoEdit', {
                url: '/item/relacionamento/vinculado/edit/:id',
                params: {
                    filters: null,
                    paging: null,
                    tipoRelacionamento: 'vinculado',
                    id: null
                },
                templateUrl: 'Scripts/app/item/manutencao-relacionamento-item.view.html',
                controller: 'ManutencaoRelacionamentoItemController',
                resolve: {
                    itemRelacionamento: ['$stateParams', 'ItemRelacionamentoService', function ($stateParams, service) {
                        return service.obter($stateParams.id);
                    }]
                }
            });

        // RECEITUARIO
        $stateProvider
            .state('manutencaoRelacionamentoReceituarioNew', {
                url: '/item/relacionamento/receituario/new',
                params: {
                    tipoRelacionamento: 'receituario'
                },
                templateUrl: 'Scripts/app/item/manutencao-relacionamento-item.view.html',
                controller: 'ManutencaoRelacionamentoItemController',
                resolve: {
                    itemRelacionamento: ['$stateParams', 'ItemRelacionamentoService', function ($stateParams, service) {
                        return service.obterNovo($stateParams.tipoRelacionamento);
                    }]
                }
            })
            .state('manutencaoRelacionamentoReceituarioEdit', {
                url: '/item/relacionamento/receituario/edit/:id',
                params: {
                    filters: null,
                    paging: null,
                    tipoRelacionamento: 'receituario',
                    id: null
                },
                templateUrl: 'Scripts/app/item/manutencao-relacionamento-item.view.html',
                controller: 'ManutencaoRelacionamentoItemController',
                resolve: {
                    itemRelacionamento: ['$stateParams', 'ItemRelacionamentoService', function ($stateParams, service) {
                        return service.obter($stateParams.id);
                    }]
                }
            });

        // MANIPULADO
        $stateProvider
            .state('manutencaoRelacionamentoManipuladoNew', {
                url: '/item/relacionamento/manipulado/new',
                params: {
                    tipoRelacionamento: 'manipulado'
                },
                templateUrl: 'Scripts/app/item/manutencao-relacionamento-item.view.html',
                controller: 'ManutencaoRelacionamentoItemController',
                resolve: {
                    itemRelacionamento: ['$stateParams', 'ItemRelacionamentoService', function ($stateParams, service) {
                        return service.obterNovo($stateParams.tipoRelacionamento);
                    }]
                }
            })
            .state('manutencaoRelacionamentoManipuladoEdit', {
                url: '/item/relacionamento/manipulado/edit/:id',
                params: {
                    filters: null,
                    paging: null,
                    tipoRelacionamento: 'manipulado',
                    id: null
                },
                templateUrl: 'Scripts/app/item/manutencao-relacionamento-item.view.html',
                controller: 'ManutencaoRelacionamentoItemController',
                resolve: {
                    itemRelacionamento: ['$stateParams', 'ItemRelacionamentoService', function ($stateParams, service) {
                        return service.obter($stateParams.id);
                    }]
                }
            });
    }
})();