namespace Walmart.Sgp.WebApi.Models
{
    /// <summary>
    /// Representa uma model de item relacionamento.
    /// </summary>
    public class ItemRelacionamentoModel
    {
        /// <summary>
        /// Obtém ou define o CdSistema.
        /// </summary>
        public byte CdSistema { get; set; }

        /// <summary>
        /// Obtém ou define o TipoRelacionamento.
        /// </summary>
        public int TipoRelacionamento { get; set; }

        /// <summary>
        /// Obtém ou define o CdItem.
        /// </summary>
        public long? CdItem { get; set; }

        /// <summary>
        /// Obtém ou define o IDDepartamento.
        /// </summary>
        public int? IDDepartamento { get; set; }

        /// <summary>
        /// Obtém ou define o IDCategoria.
        /// </summary>
        public int? IDCategoria { get; set; }

        /// <summary>
        /// Obtém ou define o IDSubcategoria.
        /// </summary>
        public int? IDSubcategoria { get; set; }

        /// <summary>
        /// Obtém ou define o IDFineline.
        /// </summary>
        public int? IDFineline { get; set; }

        /// <summary>
        /// Obtém ou define o IDRegiaoCompra.
        /// </summary>
        public int? IDRegiaoCompra { get; set; }

        /// <summary>
        /// Obtém ou define o DsItem.
        /// </summary>
        public string DsItem { get; set; }
    }
}