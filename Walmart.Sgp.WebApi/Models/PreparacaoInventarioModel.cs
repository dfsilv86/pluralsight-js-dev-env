namespace Walmart.Sgp.WebApi.Models
{
    /// <summary>
    /// Representa uma model de preparação inventário.
    /// </summary>
    public class PreparacaoInventarioModel
    {
        /// <summary>
        /// Obtém ou define o IDLoja.
        /// </summary>
        public int IDLoja { get; set; }

        /// <summary>
        /// Obtém ou define o CdLoja.
        /// </summary>
        public int CdLoja { get; set; }

        /// <summary>
        /// Obtém ou define o NmLoja.
        /// </summary>
        public string NmLoja { get; set; }

        /// <summary>
        /// Obtém ou define o IDDepartamento.
        /// </summary>
        public int? IDDepartamento { get; set; }

        /// <summary>
        /// Obtém ou define o IDTipoRelatorio.
        /// </summary>
        public int IDTipoRelatorio { get; set; }
    }
}