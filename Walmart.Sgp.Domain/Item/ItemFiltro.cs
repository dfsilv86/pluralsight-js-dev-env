namespace Walmart.Sgp.Domain.Item
{
    /// <summary>
    /// Representa o filtro da consulta de itens.
    /// </summary>
    public class ItemFiltro
    {
        /// <summary>
        /// Obtém ou define a estrutura mercadológica.
        /// </summary>
        public byte? cdSistema { get; set; }

        /// <summary>
        /// Obtém ou define o IDBandeira.
        /// </summary>        
        public int? IDBandeira { get; set; }

        /// <summary>
        /// Obtém ou define o cdItem.
        /// </summary>
        public long? cdItem { get; set; }

        /// <summary>
        /// Obtém ou define o dsItem.
        /// </summary>
        public string dsItem { get; set; }

        /// <summary>
        /// Obtém ou define o IdFineline.
        /// </summary>
        public int? IdFineline { get; set; }

        /// <summary>
        /// Obtém ou define o cdPlu.
        /// </summary>
        public long? cdPlu { get; set; }

        /// <summary>
        /// Obtém ou define o IDDepartamento.
        /// </summary>
        public int? IdDepartamento { get; set; }

        /// <summary>
        /// Obtém ou define o status.
        /// </summary>
        public TipoStatusItem status { get; set; }

        /// <summary>
        /// Obtém ou define o idUsuario.
        /// </summary>
        public int idUsuario { get; set; }
    }
}