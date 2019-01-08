using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.MultisourcingVendor;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;
using Walmart.Sgp.Infrastructure.IO.Excel;

namespace Walmart.Sgp.Application.Exporting
{
    /// <summary>
    /// Responsável pela exportação de SugestaoReturnSheet.
    /// </summary>
    public class SugestaoReturnSheetRAExporter
    {
        #region Fields
        private static ColumnMetadata[] s_configuracaoColunasExportacao = new[] 
        { 
            new ColumnMetadata { Index = 1, Name = Texts.Exported.ToUpperInvariant() },
            new ColumnMetadata { Index = 2, Name = Texts.GeneratedReport.ToUpperInvariant() },
            new ColumnMetadata { Index = 3, Name = Texts.Event.ToUpperInvariant() },
            new ColumnMetadata { Index = 4, Name = Texts.eventPeriod.ToUpperInvariant() },
            new ColumnMetadata { Index = 5, Name = Texts.startDateReturnSheet.ToUpperInvariant() },
            new ColumnMetadata { Index = 6, Name = Texts.endingDateTimeReturnSheet.ToUpperInvariant() },
            new ColumnMetadata { Index = 7, Name = Texts.VendorNineDigitsCode.ToUpperInvariant() },
            new ColumnMetadata { Index = 8, Name = Texts.vendorName.ToUpperInvariant() },
            new ColumnMetadata { Index = 9, Name = Texts.InputItemCode.ToUpperInvariant() },
            new ColumnMetadata { Index = 10, Name = Texts.Description.ToUpperInvariant() },
            new ColumnMetadata { Index = 11, Name = Texts.PriceSale.ToUpperInvariant() },
            new ColumnMetadata { Index = 12, Name = Texts.Store.ToUpperInvariant() },
            new ColumnMetadata { Index = 13, Name = Texts.Stock.ToUpperInvariant() },
            new ColumnMetadata { Index = 14, Name = Texts.Pack.ToUpperInvariant() },
            new ColumnMetadata { Index = 15, Name = Texts.Weight.ToUpperInvariant() },
            new ColumnMetadata { Index = 16, Name = Texts.raType.ToUpperInvariant() },
            new ColumnMetadata { Index = 17, Name = Texts.Cost.ToUpperInvariant() },
            new ColumnMetadata { Index = 18, Name = Texts.storeQtd.ToUpperInvariant() },
            new ColumnMetadata { Index = 19, Name = Texts.raQtd.ToUpperInvariant() },
            new ColumnMetadata { Index = 20, Name = Texts.ItemSubtotalInBoxes.ToUpperInvariant() },
        };

        private readonly ISugestaoReturnSheetGateway m_sugestaoReturnSheetGateway;
        private readonly IExcelWriter m_excelWriter;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="SugestaoReturnSheetRAExporter"/>.
        /// </summary>
        /// <param name="sugestaoReturnSheetGateway">O data table gateway de SugestaoReturnSheet.</param>
        /// <param name="excelWriter">O escritor de excel.</param>
        public SugestaoReturnSheetRAExporter(ISugestaoReturnSheetGateway sugestaoReturnSheetGateway, IExcelWriter excelWriter)
        {
            m_sugestaoReturnSheetGateway = sugestaoReturnSheetGateway;
            m_excelWriter = excelWriter;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Realiza a exportação dos registros de sugestão return sheet.
        /// </summary>
        /// <param name="dtInicioReturn">A data de início da return sheet.</param>
        /// <param name="dtFinalReturn">A data final da return sheet.</param>
        /// <param name="cdV9D">O código de 9 dígitos do vendor.</param>
        /// <param name="evento">O nome do evento.</param>
        /// <param name="cdItemDetalhe">O código do item detalhe de entrada.</param>
        /// <param name="cdDepartamento">O código do departamento.</param>
        /// <param name="cdLoja">O código da loja.</param>
        /// <param name="idRegiaoCompra">O identificador da região de compra.</param>
        /// <param name="blExportado">O flag indicando se a sugestão foi exportada.</param>
        /// <param name="blAutorizado">O flag indicando se a sugestão foi autorizada.</param>
        /// <param name="paging">Dados de paginação.</param>
        /// <returns>Retorna planilha contendo sugestões.</returns>
        public Stream Exportar(DateTime? dtInicioReturn, DateTime? dtFinalReturn, long? cdV9D, string evento, int? cdItemDetalhe, int? cdDepartamento, int? cdLoja, int? idRegiaoCompra, bool? blExportado, bool? blAutorizado, Paging paging)
        {
            SpecService.Assert(new { Department = cdDepartamento, startDateReturnSheet = dtInicioReturn, endDateReturnSheet = dtFinalReturn }, new AllMustBeInformedSpec());

            paging.Offset = 0;
            paging.Limit = int.MaxValue;

            var registros = m_sugestaoReturnSheetGateway.ConsultaReturnSheetLojaRA(dtInicioReturn, dtFinalReturn, cdV9D, evento, cdItemDetalhe, cdDepartamento, cdLoja, idRegiaoCompra, blExportado, blAutorizado, paging);
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
                }
            };
        }

        private static string FormatarAutorizado(SugestaoReturnSheet sugestao)
        {
            if (sugestao.BlAutorizado.HasValue)
            {
                return sugestao.BlAutorizado.Value ?
                    "{0} - {1} - {2:dd/MM/yyyy // HH:mm}".With(Texts.Yes, sugestao.UsuarioAutorizacao.FullName, sugestao.DhAutorizacao) :
                    Texts.No;
            }

            return null;
        }

        private static string FormatarAtualizado(SugestaoReturnSheet sugestao)
        {
            if (sugestao.BlExportado.HasValue)
            {
                return sugestao.BlExportado.Value ?
                    "{0} - {1} - {2:dd/MM/yyyy // HH:mm}".With(Texts.Yes, sugestao.UsuarioAtualizacao.FullName, sugestao.DhExportacao) :
                    Texts.No;
            }

            return null;
        }

        private static string FormatarPeriodo(SugestaoReturnSheet sugestao)
        {
            return "{0:dd/MM/yyyy} - {1:dd/MM/yyyy}".With(sugestao.ReturnSheet.DhInicioEvento, sugestao.ReturnSheet.DhFinalEvento);
        }

        private Stream CriarArquivoExportacao(IEnumerable<SugestaoReturnSheet> sugestoes)
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
                        new Column { Metadata = s_configuracaoColunasExportacao[0], Value = FormatarAutorizado(sugestao) },
                        new Column { Metadata = s_configuracaoColunasExportacao[1], Value = FormatarAtualizado(sugestao) },
                        new Column { Metadata = s_configuracaoColunasExportacao[2], Value = sugestao.ReturnSheet.Descricao },
                        new Column { Metadata = s_configuracaoColunasExportacao[3], Value = FormatarPeriodo(sugestao) },
                        new Column { Metadata = s_configuracaoColunasExportacao[4], Value = sugestao.ReturnSheet.DhInicioReturn.ToString("dd/MM/yyyy", RuntimeContext.Current.Culture) },
                        new Column { Metadata = s_configuracaoColunasExportacao[5], Value = sugestao.ReturnSheet.DhFinalReturn.ToString("dd/MM/yyyy // HH:mm", RuntimeContext.Current.Culture) },
                        new Column { Metadata = s_configuracaoColunasExportacao[6], Value = sugestao.FornecedorParametro.cdV9D },
                        new Column { Metadata = s_configuracaoColunasExportacao[7], Value = sugestao.Fornecedor.nmFornecedor },
                        new Column { Metadata = s_configuracaoColunasExportacao[8], Value = sugestao.ItemDetalheEntrada.CdItem },
                        new Column { Metadata = s_configuracaoColunasExportacao[9], Value = sugestao.ItemDetalheEntrada.DsItem },
                        new Column { Metadata = s_configuracaoColunasExportacao[10], Value = sugestao.PrecoVenda.HasValue ? sugestao.PrecoVenda.Value.ToString("N2", RuntimeContext.Current.Culture) : null },
                        new Column { Metadata = s_configuracaoColunasExportacao[11], Value = sugestao.Loja.cdLoja },
                        new Column { Metadata = s_configuracaoColunasExportacao[12], Value = sugestao.EstoqueItemVenda.HasValue ? sugestao.EstoqueItemVenda.Value.ToString("N2", RuntimeContext.Current.Culture) : "0,00" },
                        new Column { Metadata = s_configuracaoColunasExportacao[13], Value = sugestao.qtVendorPackageItemCompra },
                        new Column { Metadata = s_configuracaoColunasExportacao[14], Value = sugestao.vlPesoLiquidoItemCompra },
                        new Column { Metadata = s_configuracaoColunasExportacao[15], Value = sugestao.ItemDetalheEntrada.VlTipoReabastecimento.ToString(RuntimeContext.Current.Culture) },
                        new Column { Metadata = s_configuracaoColunasExportacao[16], Value = sugestao.vlCustoContabilItemVenda.HasValue ? sugestao.vlCustoContabilItemVenda.Value.ToString("N2", RuntimeContext.Current.Culture) : null },
                        new Column { Metadata = s_configuracaoColunasExportacao[17], Value = sugestao.QtdLoja },
                        new Column { Metadata = s_configuracaoColunasExportacao[18], Value = sugestao.QtdRA },
                        new Column { Metadata = s_configuracaoColunasExportacao[19], Value = sugestao.Subtotal },
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