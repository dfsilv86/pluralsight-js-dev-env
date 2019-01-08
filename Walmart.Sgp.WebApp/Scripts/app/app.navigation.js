(function () {

    var nodes = {
        'pesquisaPermissao': {
            'cadastroPermissaoEdit': null,
            'cadastroPermissaoNew': null
        },
        'importacaoUsuario': null,
        'manutencaoParametroSistema': null,
        'pesquisaBandeira': {
            'cadastroBandeiraEdit': null,
            'cadastroBandeiraNew': null
        },
        'pesquisaDivisao': null,
        'pesquisaDepartamento': {
            'cadastroDepartamento': null
        },
        'pesquisaCategoria': null,
        'pesquisaSubcategoria': null,
        'pesquisaFineline': null,
        'pesquisaFornecedor': null,
        'manutencaoLoja': {
            'manutencaoLojaEdit': null
        },
        'pesquisaDistrito': null, // tem uma modal que talvez devesse ser uma tela
        'custosItem': null,
        'pesquisaProdutoVinculado': {
            'manutencaoRelacionamentoVinculadoNew': null,
            'manutencaoRelacionamentoVinculadoEdit': {
                'custosItemVinculado': null
            },
        },
        'pesquisaProdutoReceituario': {
            'manutencaoRelacionamentoReceituarioNew': null,
            'manutencaoRelacionamentoReceituarioEdit': {
                'custosItemReceituario': null
            },
        },
        'pesquisaProdutoManipulado': {
            'manutencaoRelacionamentoManipuladoNew': null,
            'manutencaoRelacionamentoManipuladoEdit': {
                'custosItemManipulado': null
            },
        },
        'pesquisaRelacionamentoTransferencia': {
            'cadastroRelacionamentoTransferencia': null
        },
        'pesquisaAlcada': {
            'manutencaoAlcada': null
        },
        'cadastroRelacaoItemLoja': {
            'impVincularItemLojaCD': null,
            'impDesvincularItemLojaCD': null,
            'impVincularItemLojaCDResults': null,
            'impDesvincularItemLojaCDResults': null,
        },
        'pesquisaGradeSugestao': {
            'cadastroGradeSugestaoEdit': null,
            'cadastroGradeSugestaoNew': null
        },
        'pesquisaLojaCdParametro': {
            'detalheLojaCdParametro': null
        },
        'pesquisaParametroProduto': {
            'detalheParametroProduto': null
        },
        'pesquisaParametroFornecedor': {
            'detalheParametroFornecedor': null
        },
        'pesquisaSugestaoPedido': {
            'cadastroSugestaoPedido': {
                'pesquisaSugestaoPedidoLog': {
                    'pesquisaAutorizaPedido': null
                }
            }
        },
        'pesquisaItem': {
            'manutencaoItem': {
                'manutencaoItemRelacionamento': null
            },
            'manutencaoItemPrecosCusto': null
        },
        'estoqueAjuste': null,
        'extratoProduto': null,
        'transferenciaMtr': null,
        'correcaoNotaFiscal': null,
        'pesquisaNotaFiscal': {
            'notaFiscalEdit': null
        },
        'pesquisaInventarioAgendamento': {
            'cadastroInventarioAgendamentoNew': null,
            'cadastroInventarioAgendamentoEdit': null,
            'cadastroInventarioAgendamentoManyEdit': null
        },
        'importacaoAutomatica': null,
        'pesquisaInventarioGeracao': {
            'detalheInventario': null
        },
        'preparacaoInventario': null,
        'pesquisaInventarioCritica': null,
        'importacaoInventario': null,
        'pesquisaLogsExecucao': null,
        'solicitacaoAjusteCusto': null,
        'revisaoAjusteCusto': {
            'revisaoAjusteCustoEdit': null
        },
        'pesquisaMonitorCarga': {
            'detalheMonitorCarga': null
        },
        'pesquisaReturnSheet': {
            'cadastroReturnSheetNew': null,
            'cadastroReturnSheetEdit': {
                'pesquisaItensReturnSheetEdit': {
                    'pesquisaItensLojasValidasItemEdit': null
                },
                'lojasValidasItemEdit': null
            }
        },
        'multiSourcing': {
            'cadMultiSourcingEdit': null
        },
        'pedidosRoteirizados': {
            'autorizarPedidoEdit': {
                'detalhePedidoEdit': null
            }
        },
        'cadastroRoteiroEntrega': {
            'cadastroRoteiroNew': null,
            'cadastroRoteiroEdit': null
        }
    };

    angular
        .module('common')
        .config(['StateHierarchyConfig', function (hierarchy) {
            hierarchy.addNodes(nodes);
        }]);
})();