namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Representa uma LogRelacaoItemLojaCD.
    /// </summary>
    public class LogRelacaoItemLojaCD
    {
        /// <summary>
        /// Obtém ou define IDCD
        /// </summary>
        public int IDCD { get; set; }

        /// <summary>
        /// Obtém ou define IDLoja
        /// </summary>
        public int IDLoja { get; set; }

        /// <summary>
        /// Obtém ou define IDItemDetalheSaida
        /// </summary>
        public long IDItemDetalheSaida { get; set; }

        /// <summary>
        /// Obtém ou define IDItemDetalheEntradaAnterior
        /// </summary>
        public long? IDItemDetalheEntradaAnterior { get; set; }

        /// <summary>
        /// Obtém ou define IDItemDetalheEntradaNovo
        /// </summary>
        public long? IDItemDetalheEntradaNovo { get; set; }

        /// <summary>
        /// Obtém ou define IDLogTipoProcesso
        /// </summary>
        public int IDLogTipoProcesso { get; set; }

        /// <summary>
        /// Obtém ou define cdCrossRefAnterior
        /// </summary>
        public int? cdCrossRefAnterior { get; set; }

        /// <summary>
        /// Obtém ou define cdCrossRefNovo
        /// </summary>
        public int? cdCrossRefNovo { get; set; }

        /// <summary>
        /// Obtém ou define IdAuditUser
        /// </summary>
        public int IdAuditUser { get; set; }

        /// <summary>
        /// Obtém ou define vlTipoReabastecimento
        /// </summary>
        public int? vlTipoReabastecimento { get; set; }

        /// <summary>
        /// Obtém ou define Observacao
        /// </summary>
        public string Observacao { get; set; }
    }
}
