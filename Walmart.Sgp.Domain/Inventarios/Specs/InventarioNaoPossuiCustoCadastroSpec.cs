using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Inventarios.Specs
{
    /// <summary>
    /// Especificação referente a se inventario nao possui custo cadastro.
    /// </summary>
    public class InventarioNaoPossuiCustoCadastroSpec : SpecBase<Inventario>
    {
        private readonly IInventarioGateway m_gateway;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="InventarioNaoPossuiCustoCadastroSpec"/>.
        /// </summary>
        /// <param name="gateway">O gateway de inventário.</param>
        public InventarioNaoPossuiCustoCadastroSpec(IInventarioGateway gateway)
        {
            m_gateway = gateway;
        }

        /// <summary>
        /// Verifica se o inventario informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O inventario.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo inventario.
        /// </returns>
        public override SpecResult IsSatisfiedBy(Inventario target)
        {
            return m_gateway.PossuiItemComCustoDeCadastro(target.IDInventario)
                ? NotSatisfied(Texts.ThereAreItemsWithRegisterCost)
                : Satisfied();
        }
    }
}