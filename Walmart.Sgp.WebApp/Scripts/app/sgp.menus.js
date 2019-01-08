(function () {
    'use strict';

    // TODO: globalizar menus

    window.sgpMenus = [
        // ACESSOS
        {
            'name': 'Acessos',
            'children': [
                { 'name': 'Permissões', route: '/acesso/permissao' },
                { 'name': 'Importação de Usuários', route: '/acessos/usuario/importacao' },
                { 'name': 'Auditoria de Dados' }
            ]
        },

        // CADASTRO
        {
            'name': 'Cadastro',
            'children': [
                { 'name': 'Parâmetros do Sistema', route: '/gerenciamento/parametroSistema' },
                { 'name': 'Bandeiras', route: '/cadastro/bandeira' },
                { 'name': 'Divisões', route: '/cadastro/divisao' },
                { 'name': 'Departamentos', route: '/cadastro/departamento' },
                { 'name': 'Categorias', route: '/cadastro/categoria' },
                { 'name': 'Subcategorias', route: '/cadastro/subcategoria' },
                { 'name': 'Finelines', route: '/cadastro/fineline' },
                { 'name': 'Fornecedores', route: '/cadastro/fornecedor' },
                { 'name': 'Lojas', route: '/gerenciamento/loja' },
                { 'name': 'Distritos', route: '/cadastro/distrito' }
            ]
        },

        // RELACIONAMENTO DE ITENS
        {
            'name': 'Relacionamento de Itens',
            'children': [
                 { 'name': 'Vinculados', 'route': '/item/relacionamento/vinculado' },
                 { 'name': 'Receituários', 'route': '/item/relacionamento/receituario' },
                 { 'name': 'Manipulados', 'route': '/item/relacionamento/manipulado' },
                 //{ 'name': 'Relacionamento de Itens de MTR', 'route': '/item/relacionamento-mtr' },
                 {
                     'name': 'Relatórios',
                     'children': [
                         { 'name': 'Itens Relacionados Deletados e Inativos', route: '/item/relatorios/relatorioItensRelacionadosDeletadosInativos' },
                         { 'name': 'Itens com PLU sem Relacionamento', route: '/item/relatorios/relatorioItensPluSemRelacionamento' },
                         { 'name': 'Itens de Compra Sem Relacionamento', route: '/item/relatorios/relatorioItensDeCompraSemRelacionamento' },
                         { 'name': 'Itens com Variação de Custo', route: '/item/relatorios/relatorioItensComVariacaoCusto' },
                         { 'name': 'Itens com Custo Zero', route: '/item/relatorios/relatorioItensComCustoZero' },
                         { 'name': 'Itens Excluídos do HOST', route: '/item/relatorios/relatorioItensExcluidosDoHost' },
                         { 'name': 'Tipos de Itens', route: '/item/relatorios/relatorioTiposItens' },
                         { 'name': 'Itens com PLU', route: '/item/relatorios/relatorioItensComPLU' },
                         { 'name': 'Log de Alteração do Relacionamento de Itens', route: '/item/relatorios/relatorioDeLogDeAlteracaoDoRelacionamentoDeItens' },
                         { 'name': 'Log de Manutenção do Cadastro de Itens', route: '/item/relatorios/relatorioDeLogDeManutencaoDoCadastroDeItens' }
                     ]
                 }
            ]
        },

        // REABASTECIMENTO
        {
            'name': 'Reabastecimento',
            'children': [
                { 'name': 'Cadastro de Alçada', 'route': '/reabastecimento/alcada' },
                { 'name': 'Cadastro de Compra Casada', 'route': '/reabastecimento/compraCasada' },
                { 'name': 'Cadastro de Reabastecimento Item/Loja', 'route': '/reabastecimento/relacao-item-loja' },
                { 'name': 'Cadastro de Grade de Corte', 'route': '/reabastecimento/grade-sugestao' },
                { 'name': 'Cadastro de Parâmetros Loja/CD', 'route': '/reabastecimento/loja-cd/parametro' },
                { 'name': 'Consulta de Parâmetros do Item', 'route': '/item/parametro' },
                { 'name': 'Consulta de Parâmetros do Vendor', 'route': '/vendor/parametro' },
                { 'name': 'Cadastro de Multi Sourcing', 'route': '/multiSourcing' },
                { 'name': 'Sugestão de Pedido', 'route': '/reabastecimento/sugestao-pedido' },
                { 'name': 'Sugestão Pedido CD', 'route': '/reabastecimento/sugestaoPedidoCD/sugestao-pedido-cd' },
                { 'name': 'Consulta Return Sheet (LOJA)', 'route': '/consultaReturnSheetLoja' },
                { 'name': 'Consulta Return Sheet (RA)', 'route': '/consultaReturnSheetRA' },
                { 'name': 'Cadastro Return Sheet', 'route': '/pesquisaReturnSheet' },
                { 'name': 'Cadastro Roteiro de Entrega', 'route': '/reabastecimento/cadastro-roteiro-entrega' },
                { 'name': 'Pedidos Roteirizados', 'route': '/reabastecimento/pedidos-roteirizados' },
                {
                    'name': 'Relatórios',
                    'children': [
                      { 'name': 'Sugestão de Compra', 'route': '/reabastecimento/relatorios/relatorioSugestaoCompra' },
                      { 'name': 'Gerencial de Aderência', 'route': '/reabastecimento/relatorios/relatorioGerencialDeAderencia' },
                      { 'name': 'Item Visível/Não visível para interferir', 'route': '/reabastecimento/relatorios/relatorioDeItemVisivelNaoVisivelParaInterferir' },
                      { 'name': 'Previsão de Demandas', 'route': '/reabastecimento/relatorios/relatorioPrevisaoDemandas' },
                      { 'name': 'Forecast', 'route': '/reabastecimento/relatorios/relatorioForecast' },
                      { 'name': 'Pedidos Gerados por Return Sheet', 'route': '/reabastecimento/relatorios/relatorioPedidosGeradosPorReturnSheet' },
                      { 'name': 'Referência Cruzada OIF x SGP', 'route': '/reabastecimento/relatorios/relatorioReferenciaCruzada' },
                      { 'name': 'Consistência Vendor 9 Digitos', 'route': '/reabastecimento/relatorios/relatorioConsistenciaVendor9Digitos'},
                      { 'name': 'Log Vendor Primario', 'route': '/reabastecimento/relatorios/relatorioLogVendorPrimario' }
                    ]
                }
            ]
        },

        // INFORMACOES GERENCIAIS
        {
            'name': 'Informações Gerenciais',
            'children': [
                { 'name': 'Consulta de Itens', 'route': '/informacoes-gerenciais/item' },
                {
                    'name': 'Relatórios',
                    'children': [
                       { 'name': 'Itens com Estoque sem Vendas', 'route': '/gerenciamento/relatorios/relatorioItensComEstoqueSemVendas' },
                       { 'name': 'Financeiro por Item', 'route': '/gerenciamento/relatorios/financeiroPorItem' },
                       { 'name': 'Financeiro por Departamento Analítico', 'route': '/gerenciamento/relatorios/relatorioFinanceiroPorDepartamentoAnalitico' },
                       { 'name': 'Financeiro por Departamento Sintético', 'route': '/gerenciamento/relatorios/relatorioFinanceiroPorDepartamentoSintetico' },
                       { 'name': 'Quebras', 'route': '/gerenciamento/relatorios/relatorioQuebras' },
                       { 'name': 'ABC de Vendas', 'route': '/gerenciamento/relatorios/itensAbcVendas' }
                    ]
                }
            ]
        },

        // REPROCESSAMENTO DE ITENS
        {
            'name': 'Reprocessamento de Itens',
            'children': [
                { 'name': 'Reprocessamento de Custos' }
            ]
        },

        // ESTOQUE
        {
            'name': 'Estoque',
            'children': [
                { 'name': 'Ajuste de Estoque', 'route': '/estoque/ajuste' },
                { 'name': 'Extrato do Produto', 'route': '/estoque/extrato' },
                { 'name': 'Transferência Interna - MTR', 'route': '/estoque/transferencia-mtr' },
                {
                    'name': 'Relatórios',
                    'children': [
                        { 'name': 'Posição Estoque', 'route': '/estoque/relatorios/relatorioPosicaoEstoque' },
                        { 'name': 'Estoque Contabilizado', 'route': '/estoque/relatorios/relatorioEstoqueContabilizado' },
                        { 'name': 'Quebras Contabilizado', 'route': '/estoque/relatorios/relatorioQuebrasContabilizado' },
                        { 'name': 'Estoque Inventariado', 'route': '/estoque/relatorios/relatorioEstoqueInventariado' },
                        { 'name': 'Ajuste de Estoque', 'route': '/estoque/relatorios/relatorioAjusteEstoque' },
                        { 'name': 'Extrato de MTR', 'route': '/estoque/relatorios/relatorioExtratoMtr' },
                        { 'name': 'Extrato do Produto', 'route': '/estoque/extrato/relatorio' },
                        { 'name': 'Estoque Negativo', 'route': '/estoque/relatorios/relatorioEstoqueNegativo' },
                        { 'name': 'Valorização e Desvalorização de Estoque', 'route': '/estoque/relatorios/relatorioValorizacaoDesvalorizacaoEstoque' },
                        { 'name': 'Itens Com Compras', 'route': '/estoque/relatorios/relatorioItensComCompra' }
                    ]
                }
            ]
        },

        // NOTA FISCAL
        {
            'name': 'Nota Fiscal',
            'children': [
                { 'name': 'Correção de Custos', 'route': '/notafiscal/correcao' },
                { 'name': 'Consulta de NF', 'route': '/notafiscal/pesquisa' },
                { 'name': 'Log - NF divergente' },
            ]
        },

        // INVENTÁRIOS
        {
            'name': 'Inventários',
            'children': [
            { 'name': 'Loja - Agendamento', 'route': '/inventario/agendamento' },
            { 'name': 'Loja - Importação', route: '/inventario/importacao/automatica' },
            { 'name': 'Loja - Geração de Inventário', route: '/inventario/geracao' },
            { 'name': 'Loja - Relatório de Preparação de Inventário', route: '/inventario/preparacao' },
            {
                'name': 'Suporte HO',
                'children': [
                    { 'name': 'Consulta Geral Inventários' },
                    { 'name': 'Críticas de Importação', route: '/inventario/critica' },
                    { 'name': 'Importação de Inventários (Upload)', route: '/inventario/importacao' },
                ]
            },
            {
                'name': 'Relatórios',
                'children': [
                    { 'name': 'Log de Itens do Inventário', route: '/inventario/relatorios/relatorioLogItensInventario' },
                    { 'name': 'Acuracidade de PI', route: '/inventario/relatorios/relatorioAcuracidadePI' }
                ]
            }
            ]
        },

        // ALERTAS
        {
            'name': 'Alertas',
            'children': [
                { 'name': 'Consulta do Log de Validação' },
                { 'name': 'Consulta dos Logs de Execução', 'route': '/alertas/logsExecucao' },
                //{ 'name': 'Solicitação de Ajuste de Custo', 'route': '/alertas/solicitacaoAjusteCusto' },
                //{ 'name': 'Revisão de Ajuste de Custo', 'route': '/alertas/revisaoAjusteCusto' },
                //{ 'name': 'Relatório de Auditoria', 'route': '/alertas/relatorioRevisaoCusto' },
                {
                    'name': 'Relatórios',
                    'children': [
                        { 'name': 'Log de Execuções', route: '/alertas/relatorios/relatorioExecucao' },
                        { 'name': 'Conciliação de NF', route: '/alertas/relatorios/relatorioDeConciliacaoDeNF'}
                    ]
                }
            ]
        },

        // MONITOR
        {
            'name': 'Monitor',
            'children': [
               { 'name': 'Carga', 'route': '/monitor/carga' },
               { 'name': 'Workflow' }
            ]
        }
    ];
})();
