using System;
using Walmart.Sgp.Infrastructure.Framework.Commons;

namespace Walmart.Sgp.Domain.Item
{
    /// <summary>
    /// Filtro de nota fiscal.
    /// </summary>
    public class NotaFiscalFiltro
    {
        /// <summary>
        /// Obtém ou define o código do sistema.
        /// </summary>
        public int? CdSistema { get; set; }

        /// <summary>
        /// Obtém ou define o id da bandeira.
        /// </summary>
        public int? IdBandeira { get; set; }

        /// <summary>
        /// Obtém ou define o id da loja.
        /// </summary>
        public int? IdLoja { get; set; }

        /// <summary>
        /// Obtém ou define o código da loja.
        /// </summary>
        public int? CdLoja { get; set; }

        /// <summary>
        /// Obtém ou define o id do fornecedor.
        /// </summary>
        public int? IdFornecedor { get; set; }

        /// <summary>
        /// Obtém ou define o código do fornecedor.
        /// </summary>
        public int? CdFornecedor { get; set; }

        /// <summary>
        /// Obtém ou define o número da nota fiscal.
        /// </summary>
        public long? NrNotaFiscal { get; set; }

        /// <summary>
        /// Obtém ou define o id de item detalhe.
        /// </summary>
        public int? IdItemDetalhe { get; set; }

        /// <summary>
        /// Obtém ou define o código do item.
        /// </summary>
        public long? CdItem { get; set; }

        /// <summary>
        /// Obtém ou define o id do departamento.
        /// </summary>
        public int? IdDepartamento { get; set; }

        /// <summary>
        /// Obtém ou define o id do item status de nota fiscal.
        /// </summary>
        public int? IdNotaFiscalItemStatus { get; set; }

        /// <summary>
        /// Obtém ou define o intervalo de datas de recebimento.
        /// </summary>
        public RangeValue<DateTime> DtRecebimento { get; set; }

        /// <summary>
        /// Obtém ou define o intervalo de datas de cadastro concentrador.
        /// </summary>
        public RangeValue<DateTime> DtCadastroConcentrador { get; set; }

        /// <summary>
        /// Obtém ou define o intervalo de datas de atualização concentrador.
        /// </summary>
        public RangeValue<DateTime> DtAtualizacaoConcentrador { get; set; }
    }
}
