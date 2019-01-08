using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Inventarios.Specs
{
    /// <summary>
    /// Especificação referente a se sortimento é valido.
    /// </summary>
    public class SortimentoDeveSerValidoSpec : SpecBase<Inventario>
    {
        private readonly IInventarioGateway m_gateway;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="SortimentoDeveSerValidoSpec"/>.
        /// </summary>
        /// <param name="gateway">O gateway de inventario.</param>
        public SortimentoDeveSerValidoSpec(IInventarioGateway gateway)
        {
            m_gateway = gateway;
        }

        /// <summary>
        /// Verifica se o int32 informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O int32.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo int32.
        /// </returns>
        public override SpecResult IsSatisfiedBy(Inventario target)
        {
            return m_gateway.PossuiSortimentoInvalido(target.IDInventario)
                ? NotSatisfied(Texts.ThereAreItemsOutOfStoreBuyingSorting)
                : Satisfied();
        }
    }
}