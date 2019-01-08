using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;
using Walmart.Sgp.Infrastructure.IO.Excel;

namespace Walmart.Sgp.Application.Exporting
{
    // TODO - Globalizar colunas e informações

    /// <summary>
    /// Responsável pela exportação de SugestaoPedidoCD.
    /// </summary>
    public class SugestaoPedidoCDExcelExporter
    {
        #region Fields
        private static ColumnMetadata[] s_configuracaoColunasExportacao = new[] 
        { 
            new ColumnMetadata { Index = 1, Name = "CÓDIGO DO CD" },
            new ColumnMetadata { Index = 2, Name = "DEPARTAMENTO" },
            new ColumnMetadata { Index = 3, Name = "ITEM CONTROLE DE ESTOQUE" },
            new ColumnMetadata { Index = 4, Name = "CÓDIGO VENDOR 9 DÍGITOS" },
            new ColumnMetadata { Index = 5, Name = "NOME VENDOR" },
            new ColumnMetadata { Index = 6, Name = "CANAL DO VENDOR" },
            new ColumnMetadata { Index = 7, Name = "DESCRIÇÃO ITEM CONTROLE DE ESTOQUE" },
            new ColumnMetadata { Index = 8, Name = "ITEM DE ENTRADA" },
            new ColumnMetadata { Index = 9, Name = "DESCRIÇÃO ITEM ENTRADA" },
            new ColumnMetadata { Index = 10, Name = "TIPO REABASTECIMENTO" },
            new ColumnMetadata { Index = 11, Name = "VENDORPACK" },
            new ColumnMetadata { Index = 12, Name = "CUSTO" },
            new ColumnMetadata { Index = 13, Name = "LEAD TIME" },
            new ColumnMetadata { Index = 14, Name = "ON HAND" },
            new ColumnMetadata { Index = 15, Name = "ON ORDER" },
            new ColumnMetadata { Index = 16, Name = "MÍNIMO DO CD" },
            new ColumnMetadata { Index = 17, Name = "PIPELINE" },
            new ColumnMetadata { Index = 18, Name = "PERÍODO DEMANDA" },
            new ColumnMetadata { Index = 19, Name = "FORECAST MÉDIO" },
            new ColumnMetadata { Index = 20, Name = "VALOR FORECAST" },
            new ColumnMetadata { Index = 21, Name = "DATA PEDIDO" },
            new ColumnMetadata { Index = 22, Name = "QUANTIDADE SUGERIDA" },
            new ColumnMetadata { Index = 23, Name = "QUANTIDADE FINALIZADA (RA)" }
        };

        private readonly ISugestaoPedidoCDGateway m_sugestaoPedidoCDGateway;
        private readonly IExcelWriter m_excelWriter;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="SugestaoPedidoCDExcelExporter"/>.
        /// </summary>
        /// <param name="sugestaoPedidoCDGateway">O data table gateway de SugestaoPedidoCD.</param>
        /// <param name="excelWriter">O escritor de excel.</param>
        public SugestaoPedidoCDExcelExporter(ISugestaoPedidoCDGateway sugestaoPedidoCDGateway, IExcelWriter excelWriter)
        {
            m_sugestaoPedidoCDGateway = sugestaoPedidoCDGateway;
            m_excelWriter = excelWriter;
        } 
        #endregion
        
        #region Methods
        /// <summary>
        /// Realiza a exportação dos registros de multisourcing.
        /// </summary>
        /// <param name="dtSolicitacao">Data de solicitacao do pedido.</param>
        /// <param name="idDepartamento">ID do departamento.</param>
        /// <param name="idCD">ID do CD.</param>
        /// <param name="idItem">ID do item. (Entrada ou Saida)</param>
        /// <param name="idFornecedorParametro">ID do FornecedorParametro.</param>
        /// <param name="statusPedido">Filtrar por status do pedido: 0 - Nao finalizado, 1 - Finalizado, 2 - Todos</param>
        /// <returns>O stream com o XLSX.</returns>
        public Stream Exportar(DateTime? dtSolicitacao, int? idDepartamento, int? idCD, int? idItem, int? idFornecedorParametro, int? statusPedido)
        {
            SpecService.Assert(new { requestDate = dtSolicitacao, IdDepartamento = idDepartamento, cd = idCD }, new AllMustBeInformedSpec());

            var registros = m_sugestaoPedidoCDGateway.ObterSugestoesPorFiltro(dtSolicitacao.Value, idDepartamento, idCD, idItem, idFornecedorParametro, statusPedido);
            return CriarArquivoExportacao(registros);
        }

        private static Row CreateHeaderRow()
        {
            return new Row
            {
                Index = 2,
                Columns = new[] 
                { 
                    new Column { Metadata = s_configuracaoColunasExportacao[0] },
                    new Column { Metadata = s_configuracaoColunasExportacao[1] },
                    new Column { Metadata = s_configuracaoColunasExportacao[2] },
                    new Column { Metadata = s_configuracaoColunasExportacao[3] },
                    new Column { Metadata = s_configuracaoColunasExportacao[4] },
                    new Column { Metadata = s_configuracaoColunasExportacao[5] },
                    new Column { Metadata = s_configuracaoColunasExportacao[6] },
                    new Column { Metadata = s_configuracaoColunasExportacao[7] },
                    new Column { Metadata = s_configuracaoColunasExportacao[8] },
                    new Column { Metadata = s_configuracaoColunasExportacao[9] },
                    new Column { Metadata = s_configuracaoColunasExportacao[10] },
                    new Column { Metadata = s_configuracaoColunasExportacao[11] },
                    new Column { Metadata = s_configuracaoColunasExportacao[12] },
                    new Column { Metadata = s_configuracaoColunasExportacao[13] },
                    new Column { Metadata = s_configuracaoColunasExportacao[14] },
                    new Column { Metadata = s_configuracaoColunasExportacao[15] },
                    new Column { Metadata = s_configuracaoColunasExportacao[16] },
                    new Column { Metadata = s_configuracaoColunasExportacao[17] },
                    new Column { Metadata = s_configuracaoColunasExportacao[18] },
                    new Column { Metadata = s_configuracaoColunasExportacao[19] },
                    new Column { Metadata = s_configuracaoColunasExportacao[20] },
                    new Column { Metadata = s_configuracaoColunasExportacao[21] },
                    new Column { Metadata = s_configuracaoColunasExportacao[22] },
                }
            };
        }

        private Stream CriarArquivoExportacao(IEnumerable<SugestaoPedidoCD> sugestoes)
        {
            var index = 2;
            IList<Row> rows = new List<Row>();

            if (sugestoes.Count() == 0)
            {
                rows.Add(CreateHeaderRow());
            }

            foreach (var sugestao in sugestoes)
            {
                var row = new Row
                {
                    Index = index,
                    Columns = new[] 
                    { 
                        new Column { Metadata = s_configuracaoColunasExportacao[0], Value = sugestao.CD.cdCD },
                        new Column { Metadata = s_configuracaoColunasExportacao[1], Value = sugestao.FornecedorParametro.cdDepartamento + " - " + sugestao.FornecedorParametro.dsDepartamento },
                        new Column { Metadata = s_configuracaoColunasExportacao[2], Value = sugestao.ItemDetalheSugestao.CdItem },
                        new Column { Metadata = s_configuracaoColunasExportacao[3], Value = sugestao.FornecedorParametro.cdV9D },
                        new Column { Metadata = s_configuracaoColunasExportacao[4], Value = sugestao.FornecedorParametro.nmFornecedor },
                        new Column { Metadata = s_configuracaoColunasExportacao[5], Value = sugestao.FornecedorParametro.cdTipo },
                        new Column { Metadata = s_configuracaoColunasExportacao[6], Value = sugestao.ItemDetalheSugestao.DsItem },
                        new Column { Metadata = s_configuracaoColunasExportacao[7], Value = sugestao.ItemDetalhePedido.CdItem },
                        new Column { Metadata = s_configuracaoColunasExportacao[8], Value = sugestao.ItemDetalhePedido.DsItem },
                        new Column { Metadata = s_configuracaoColunasExportacao[9], Value = sugestao.vlTipoReabastecimento + " - Staple" },
                        new Column { Metadata = s_configuracaoColunasExportacao[10], Value = sugestao.qtVendorPackage },
                        new Column { Metadata = s_configuracaoColunasExportacao[11], Value = sugestao.vlCusto.ToString("N2", RuntimeContext.Current.Culture) },
                        new Column { Metadata = s_configuracaoColunasExportacao[12], Value = sugestao.vlLeadTime },
                        new Column { Metadata = s_configuracaoColunasExportacao[13], Value = sugestao.qtdOnHand },
                        new Column { Metadata = s_configuracaoColunasExportacao[14], Value = sugestao.qtdOnOrder },
                        new Column { Metadata = s_configuracaoColunasExportacao[15], Value = sugestao.tempoMinimoCD },
                        new Column { Metadata = s_configuracaoColunasExportacao[16], Value = sugestao.qtdPipeline },
                        new Column { Metadata = s_configuracaoColunasExportacao[17], Value = sugestao.dtInicioForecast.ToString("dd/MM/yyyy", RuntimeContext.Current.Culture) + " à " + sugestao.dtFimForecast.ToString("dd/MM/yyyy", RuntimeContext.Current.Culture) },
                        new Column { Metadata = s_configuracaoColunasExportacao[18], Value = sugestao.ForecastMedio },
                        new Column { Metadata = s_configuracaoColunasExportacao[19], Value = sugestao.qtdForecast },
                        new Column { Metadata = s_configuracaoColunasExportacao[20], Value = sugestao.dtPedido.ToString("dd/MM/yyyy", RuntimeContext.Current.Culture) },
                        new Column { Metadata = s_configuracaoColunasExportacao[21], Value = sugestao.qtdPackCompraOriginal },
                        new Column { Metadata = s_configuracaoColunasExportacao[22], Value = sugestao.qtdPackCompra },
                    }
                };

                rows.Add(row);
                index++;
            }

            return m_excelWriter.Write(rows);
        } 
        #endregion
    }
}