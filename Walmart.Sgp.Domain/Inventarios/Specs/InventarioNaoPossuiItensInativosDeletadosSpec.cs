using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Inventarios.Specs
{
    /// <summary>
    /// Especificação referente a se inventario nao possui itens inativos deletados.
    /// </summary>
    public class InventarioNaoPossuiItensInativosDeletadosSpec : SpecBase<Inventario>
    {
        private readonly IInventarioGateway m_gateway;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="InventarioNaoPossuiItensInativosDeletadosSpec"/>.
        /// </summary>
        /// <param name="gateway">O gateway de inventário.</param>
        public InventarioNaoPossuiItensInativosDeletadosSpec(IInventarioGateway gateway)
        {
            m_gateway = gateway;
        }

        /// <summary>
        /// Verifica se o inventário informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O id do inventário.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo inventario.
        /// </returns>
        public override SpecResult IsSatisfiedBy(Inventario target)
        {
            return m_gateway.PossuiItemInativoDeletado(target.IDInventario)
                ? NotSatisfied(Texts.ThereAreInactiveOrRemovedItems)
                : Satisfied();
        }
    }
}