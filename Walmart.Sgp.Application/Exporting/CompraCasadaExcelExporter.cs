using System.Collections.Generic;
using System.IO;
using System.Linq;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.Reabastecimento.CompraCasada;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;
using Walmart.Sgp.Infrastructure.IO.Excel;

namespace Walmart.Sgp.Application.Exporting
{
    /// <summary>
    /// Responsável pela exportação de compra casada.
    /// </summary>
    public class CompraCasadaExcelExporter
    {
        #region Fields
        private static ColumnMetadata[] s_configuracaoColunasExportacao = new[] 
        { 
            new ColumnMetadata { Index = 1, Name = "DEPARTAMENTO" },
            new ColumnMetadata { Index = 2, Name = "ITEM DE CONTROLE" },
            new ColumnMetadata { Index = 3, Name = "DESCRIÇÃO DO ITEM DE CONTROLE" },
            new ColumnMetadata { Index = 4, Name = "VENDOR 9 DIGITOS" },
            new ColumnMetadata { Index = 5, Name = "NOME DO VENDOR" },
            new ColumnMetadata { Index = 6, Name = "ITEM DE ENTRADA" },
            new ColumnMetadata { Index = 7, Name = "DESCRIÇÃO DO ITEM DE ENTRADA" },
            new ColumnMetadata { Index = 8, Name = "TIPO RA" },
            new ColumnMetadata { Index = 9, Name = "VENDOR PACKAGE" },
            new ColumnMetadata { Index = 10, Name = "CUSTO UNITÁRIO" },
            new ColumnMetadata { Index = 11, Name = "PAI" },
            new ColumnMetadata { Index = 12, Name = "FILHO" }
        };

        private readonly ICompraCasadaGateway m_compraCasadaGateway;
        private readonly IExcelWriter m_excelWriter;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="CompraCasadaExcelExporter"/>.
        /// </summary>
        /// <param name="compraCasadaGateway">O data table gateway de compra casada.</param>
        /// <param name="excelWriter">O escritor de excel.</param>
        public CompraCasadaExcelExporter(ICompraCasadaGateway compraCasadaGateway, IExcelWriter excelWriter)
        {
            m_compraCasadaGateway = compraCasadaGateway;
            m_excelWriter = excelWriter;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Realiza a exportação dos registros de cadastro de compra casada.
        /// </summary>
        /// <param name="filtro">Os filtros da exportação.</param>
        /// <returns>Stream com o XLSX.</returns>
        public Stream Exportar(PesquisaCompraCasadaFiltro filtro)
        {
            SpecService.Assert(new { System = filtro.cdSistema }, new AllMustBeInformedSpec());

            var registros = m_compraCasadaGateway.PesquisarItensEntrada(filtro, null).OrderBy(o => o.ItemSaida.CdItem).ThenBy(o => o.FornecedorParametro.cdV9D);
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
                    new Column { Metadata = s_configuracaoColunasExportacao[11] }
                }
            };
        }

        private static Row PopularLinha(ItemDetalhe item, int index)
        {
            return new Row
            {
                Index = index,
                Columns = new[] 
                    { 
                        new Column { Metadata = s_configuracaoColunasExportacao[0], Value = item.Departamento.cdDepartamento + " - " + item.Departamento.dsDepartamento },
                        new Column { Metadata = s_configuracaoColunasExportacao[1], Value = item.ItemSaida.CdItem },
                        new Column { Metadata = s_configuracaoColunasExportacao[2], Value = item.ItemSaida.DsItem },
                        new Column { Metadata = s_configuracaoColunasExportacao[3], Value = item.FornecedorParametro.cdV9D.ToString(RuntimeContext.Current.Culture) + item.FornecedorParametro.cdTipo },
                        new Column { Metadata = s_configuracaoColunasExportacao[4], Value = item.FornecedorParametro.nmFornecedor },
                        new Column { Metadata = s_configuracaoColunasExportacao[5], Value = item.CdItem },
                        new Column { Metadata = s_configuracaoColunasExportacao[6], Value = item.DsItem },
                        new Column { Metadata = s_configuracaoColunasExportacao[7], Value = item.VlTipoReabastecimento.Description },
                        new Column { Metadata = s_configuracaoColunasExportacao[8], Value = item.QtVendorPackage },
                        new Column { Metadata = s_configuracaoColunasExportacao[9], Value = (item.VlCustoUnitario.HasValue ? item.VlCustoUnitario.Value.ToString("n2", RuntimeContext.Current.Culture) : "0,00").Replace(".", ",") },
                        new Column { Metadata = s_configuracaoColunasExportacao[10], Value = item.PaiCompraCasada.HasValue && item.PaiCompraCasada.Value ? "Sim" : "Não" },
                        new Column { Metadata = s_configuracaoColunasExportacao[11], Value = item.FilhoCompraCasada.HasValue && item.FilhoCompraCasada.Value ? "Sim" : "Não" }
                    }
            };
        }

        private Stream CriarArquivoExportacao(IEnumerable<ItemDetalhe> itens)
        {
            var index = 2;
            IList<Row> rows = new List<Row>();

            if (itens.Count() == 0)
            {
                rows.Add(CreateHeaderRow());
            }

            foreach (var item in itens)
            {
                rows.Add(PopularLinha(item, index));
                index++;
            }

            return m_excelWriter.Write(rows);
        }
        #endregion
    }
}