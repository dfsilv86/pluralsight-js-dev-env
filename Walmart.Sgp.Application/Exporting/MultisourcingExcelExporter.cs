using System.Collections.Generic;
using System.IO;
using System.Linq;
using Walmart.Sgp.Domain.MultisourcingVendor;
using Walmart.Sgp.Infrastructure.Framework.Specs;
using Walmart.Sgp.Infrastructure.IO.Excel;

namespace Walmart.Sgp.Application.Exporting
{
    /// <summary>
    /// Responsável pela exportação de multisourcing.
    /// </summary>
    public class MultisourcingExcelExporter
    {
        #region Fields
        private static ColumnMetadata[] s_configuracaoColunasExportacao = new[] 
        { 
            new ColumnMetadata { Index = 1, Name = "ITEM CONTROLE ESTOQUE" },
            new ColumnMetadata { Index = 2, Name = "DESC" },
            new ColumnMetadata { Index = 3, Name = "ITEM DE ENTRADA" },
            new ColumnMetadata { Index = 4, Name = "DESC" },
            new ColumnMetadata { Index = 5, Name = "VENDOR 9 DIGITOS" },
            new ColumnMetadata { Index = 6, Name = "CANAL" },
            new ColumnMetadata { Index = 7, Name = "NOME DO VENDOR" },
            new ColumnMetadata { Index = 8, Name = "CD" },
            new ColumnMetadata { Index = 9, Name = "VENDOR PACK" },
            new ColumnMetadata { Index = 10, Name = "PESO LÍQUIDO" },
            new ColumnMetadata { Index = 11, Name = "PERCENTUAL" }
        };

        private readonly IMultisourcingGateway m_multisourcingGateway;
        private readonly IExcelWriter m_excelWriter;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="MultisourcingExcelExporter"/>.
        /// </summary>
        /// <param name="multisourcingGateway">O data table gateway de multisourcing.</param>
        /// <param name="excelWriter">O escritor de excel.</param>
        public MultisourcingExcelExporter(IMultisourcingGateway multisourcingGateway, IExcelWriter excelWriter)
        {
            m_multisourcingGateway = multisourcingGateway;
            m_excelWriter = excelWriter;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Realiza a exportação dos registros de multisourcing.
        /// </summary>
        /// <param name="idItemDetalhe">O id do item detalhe.</param>
        /// <param name="idDepartamento">O id do departamento.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <param name="idCD">O id do CD.</param>
        /// <param name="filtroMS">Se deve incluir itens possível multisourcing (1 - Sim, 0 - Não, 2 - Todos).</param>
        /// <param name="filtroCadastro">Incluir itens que possuem cadastro (1 - Sim, 0 - Não, 2 - Todos).</param>
        /// <returns>O stream com o XLSX.</returns>
        public Stream Exportar(long? idItemDetalhe, int? idDepartamento, int? cdSistema, int? idCD, int filtroMS, int filtroCadastro)
        {
            SpecService.Assert(new { idDepartamento, cdSistema }, new AllMustBeInformedSpec());

            var registros = m_multisourcingGateway.ObtemItemDetalheSaidaEntradaMultisourcing(idItemDetalhe, idDepartamento.Value, cdSistema.Value, idCD, filtroMS, filtroCadastro);
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
                    new Column { Metadata = s_configuracaoColunasExportacao[10] }
                }
            };
        }

        private Stream CriarArquivoExportacao(IEnumerable<Multisourcing> multisourcings)
        {
            var index = 2;
            IList<Row> rows = new List<Row>();

            if (multisourcings.Count() == 0)
            {
                rows.Add(CreateHeaderRow());
            }

            foreach (var multisourcing in multisourcings)
            {
                var row = new Row
                {
                    Index = index,
                    Columns = new[] 
                    { 
                        new Column { Metadata = s_configuracaoColunasExportacao[0], Value = multisourcing.ItemDetalheSaida.CdItem },
                        new Column { Metadata = s_configuracaoColunasExportacao[1], Value = multisourcing.ItemDetalheSaida.DsItem },
                        new Column { Metadata = s_configuracaoColunasExportacao[2], Value = multisourcing.ItemDetalheEntrada.CdItem },
                        new Column { Metadata = s_configuracaoColunasExportacao[3], Value = multisourcing.ItemDetalheEntrada.DsItem },
                        new Column { Metadata = s_configuracaoColunasExportacao[4], Value = multisourcing.Fornecedor.Parametros[0].cdV9D },
                        new Column { Metadata = s_configuracaoColunasExportacao[5], Value = multisourcing.Fornecedor.Parametros[0].cdTipo },
                        new Column { Metadata = s_configuracaoColunasExportacao[6], Value = multisourcing.Fornecedor.nmFornecedor },
                        new Column { Metadata = s_configuracaoColunasExportacao[7], Value = multisourcing.CD },
                        new Column { Metadata = s_configuracaoColunasExportacao[8], Value = multisourcing.ItemDetalheEntrada.QtVendorPackage },
                        new Column { Metadata = s_configuracaoColunasExportacao[9], Value = multisourcing.ItemDetalheEntrada.VlPesoLiquido },
                        new Column { Metadata = s_configuracaoColunasExportacao[10], Value = multisourcing.vlPercentual }
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