using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Inventarios.Specs
{
    /// <summary>
    /// Especificação referente a se inventario nao possui itens vinculados de entrada.
    /// </summary>
    public class InventarioNaoPossuiItensVinculadosDeEntradaSpec : SpecBase<Inventario>
    {
        private readonly IInventarioGateway m_inventarioGateway;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="InventarioNaoPossuiItensVinculadosDeEntradaSpec"/>.
        /// </summary>
        /// <param name="inventarioGateway">O table data gateway para inventario.</param>
        public InventarioNaoPossuiItensVinculadosDeEntradaSpec(IInventarioGateway inventarioGateway)
        {
            m_inventarioGateway = inventarioGateway;
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
            return m_inventarioGateway.PossuiItemVinculadoEntrada(target.IDInventario)
                ? NotSatisfied(Texts.InventarioNaoPossuiItensVinculadosDeEntradaSpecReason)
                : Satisfied();
        }
    }
}