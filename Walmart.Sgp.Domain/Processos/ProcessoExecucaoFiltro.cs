using System;
using Walmart.Sgp.Infrastructure.Framework.Commons;

namespace Walmart.Sgp.Domain.Processos
{
    /// <summary>
    /// Filtro para execução de processo.
    /// </summary>
    public class ProcessoExecucaoFiltro
    {
        /// <summary>
        /// Obtém ou define o código do sistema.
        /// </summary>
        public RangeValue<DateTime> Data { get; set; }

        /// <summary>
        /// Obtém ou define o id do processo.
        /// </summary>
        public int? IdProcesso { get; set; }

        /// <summary>
        /// Obtém ou define o código do sistema.
        /// </summary>
        public int CdSistema { get; set; }

        /// <summary>
        /// Obtém ou define o código da bandeira.
        /// </summary>
        public int IdBandeira { get; set; }

        /// <summary>
        /// Obtém ou define o id da loja.
        /// </summary>
        public int? IdLoja { get; set; }

        /// <summary>
        /// Obtém ou define a id item detalhe.
        /// </summary>
        public int? IdItemDetalhe { get; set; }
    }
}
