using System;

namespace Walmart.Sgp.WebApi.Models
{
    /// <summary>
    /// Representa uma model de extrato produto.
    /// </summary>
    public class ExtratoProdutoModel
    {
        /// <summary>
        /// Obtém ou define o NmLoja.
        /// </summary>
        public string NmLoja { get; set; }

        /// <summary>
        /// Obtém ou define o CdLoja.
        /// </summary>
        public int CdLoja { get; set; }

        /// <summary>
        /// Obtém ou define o CdItem.
        /// </summary>
        public int CdItem { get; set; }

        /// <summary>
        /// Obtém ou define o DsItem.
        /// </summary>
        public string DsItem { get; set; }

        /// <summary>
        /// Obtém ou define o EstoqueInicial.
        /// </summary>
        public decimal? EstoqueInicial { get; set; }

        /// <summary>
        /// Obtém ou define o QtdMovimentacao.
        /// </summary>
        public decimal? QtdMovimentacao { get; set; }

        /// <summary>
        /// Obtém ou define o IDLoja.
        /// </summary>
        public int IDLoja { get; set; }

        /// <summary>
        /// Obtém ou define a DtIni.
        /// </summary>
        public DateTime DtIni { get; set; }

        /// <summary>
        /// Obtém ou define a DtFim.
        /// </summary>
        public DateTime DtFim { get; set; }

        /// <summary>
        /// Obtém ou define o IDItem.
        /// </summary>
        public int IDItem { get; set; }
    }
}