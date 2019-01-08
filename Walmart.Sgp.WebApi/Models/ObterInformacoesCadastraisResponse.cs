using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Item;

namespace Walmart.Sgp.WebApi.Models
{
    /// <summary>
    /// Representa o resultado de /ItemDetalhe/InformacoesCadastrais
    /// </summary>
    public class ObterInformacoesCadastraisResponse
    {
        /// <summary>
        /// Obtém ou define a bandeira.
        /// </summary>
        public Bandeira Bandeira { get; set; }

        /// <summary>
        /// Obtém ou define o itemDetalhe.
        /// </summary>
        public ItemDetalhe ItemDetalhe { get; set; }
    }
}