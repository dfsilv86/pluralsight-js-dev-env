using System.Collections.Generic;
using System.IO;
using System.Linq;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.IO.Excel;

namespace Walmart.Sgp.Application.Exporting
{
    /// <summary>
    /// Responsável pela exportação de relacao item loja.
    /// </summary>
    public class RelacaoItemLojaCDExcelExporter
    {
        #region Fields
        private static ColumnMetadata[] s_configuracaoColunasExportacao = new[]
        {
            new ColumnMetadata { Index = 1, Name = "CD" },
            new ColumnMetadata { Index = 2, Name = "LOJA" },
            new ColumnMetadata { Index = 3, Name = "NOME LOJA" },
            new ColumnMetadata { Index = 4, Name = "DEPARTAMENTO" },
            new ColumnMetadata { Index = 5, Name = "ITEM CONTROLE DE ESTOQUE" },
            new ColumnMetadata { Index = 6, Name = "DESCRIÇÃO ITEM CONTROLE ESTOQUE" },
            new ColumnMetadata { Index = 7, Name = "ITEM DE ENTRADA" },
            new ColumnMetadata { Index = 8, Name = "DESCRIÇÃO ITEM ENTRADA" },
            new ColumnMetadata { Index = 9, Name = "VENDOR 9 DIGITOS" },
            new ColumnMetadata { Index = 10, Name = "STATUS" },
        };

        private readonly IRelacaoItemLojaCDGateway m_relacaoItemLojaCDGateway;
        private readonly IExcelWriter m_excelWriter;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="RelacaoItemLojaCDExcelExporter"/>.
        /// </summary>
        /// <param name="relacaoItemLojaCDGateway">O data table gateway de relação item loja cd.</param>
        /// <param name="excelWriter">O escritor do excel.</param>
        public RelacaoItemLojaCDExcelExporter(IRelacaoItemLojaCDGateway relacaoItemLojaCDGateway, IExcelWriter excelWriter)
        {
            m_relacaoItemLojaCDGateway = relacaoItemLojaCDGateway;
            m_excelWriter = excelWriter;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Realiza a exportação dos registros de relação item loja cd
        /// </summary>
        /// <param name="filtro">O filtro.</param>
        /// <returns>O stream com o XLSX.</returns>
        public Stream Exportar(RelacaoItemLojaCDFiltro filtro)
        {
            var registros = m_relacaoItemLojaCDGateway.ObterDadosExportacaoCadastro(filtro);
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
                }
            };
        }

        private Stream CriarArquivoExportacao(IEnumerable<RelacaoItemLojaCDConsolidado> lista)
        {
            var index = 2;
            IList<Row> rows = new List<Row>();

            if (lista.Count() == 0)
            {
                rows.Add(CreateHeaderRow());
            }

            foreach (var item in lista)
            {
                var departamento = item.RelacaoItemLojaCD.Item.Departamento.cdDepartamento + " - " + item.RelacaoItemLojaCD.Item.Departamento.dsDepartamento;
                var fornecedor = item.ItemEntrada.CdItem != 0 ? item.ItemEntrada.FornecedorParametro.cdV9D + " - " + item.ItemEntrada.Fornecedor.nmFornecedor : null;

                var row = new Row
                {
                    Index = index,
                    Columns = new[]
                    {
                        new Column { Metadata = s_configuracaoColunasExportacao[0], Value = item.cdCD },
                        new Column { Metadata = s_configuracaoColunasExportacao[1], Value = item.cdLoja },
                        new Column { Metadata = s_configuracaoColunasExportacao[2], Value = item.nmLoja },
                        new Column { Metadata = s_configuracaoColunasExportacao[3], Value = departamento },
                        new Column { Metadata = s_configuracaoColunasExportacao[4], Value = item.RelacaoItemLojaCD.Item.CdItem },
                        new Column { Metadata = s_configuracaoColunasExportacao[5], Value = item.RelacaoItemLojaCD.Item.DsItem },
                        new Column { Metadata = s_configuracaoColunasExportacao[6], Value = item.ItemEntrada.CdItem == 0 ? null : (int?)item.ItemEntrada.CdItem },
                        new Column { Metadata = s_configuracaoColunasExportacao[7], Value = item.ItemEntrada.DsItem },
                        new Column { Metadata = s_configuracaoColunasExportacao[8], Value = fornecedor },
                        new Column { Metadata = s_configuracaoColunasExportacao[9], Value = item.cdItemPess == 0 ? null : string.Format(RuntimeContext.Current.Culture, Texts.CheckNeedToUpdateInputItemForNewItemPESS, (int?)item.cdItemPess) },
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
