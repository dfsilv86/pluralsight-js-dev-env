using Walmart.Sgp.Domain.Item;

namespace Walmart.Sgp.WebApi.Models
{
    /// <summary>
    /// Requisição para validar item do relacionamento principal.
    /// </summary>
    public class ValidarPrincipalRequest
    {
        /// <summary>
        /// Obtém ou define o relacionamento principal.
        /// </summary>
        public RelacionamentoItemPrincipal ItemRelacionamento { get; set; }

        /// <summary>
        /// Obtém ou define o item detalhe.
        /// </summary>
        public ItemDetalhe ItemDetalhe { get; set; }

        /// <summary>
        /// Obtém ou define um valor que indica se o item será usado como saída.
        /// </summary>
        public bool UtilizadoComoSaida { get; set; }
    }
}