using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Inventarios.Specs
{
    /// <summary>
    /// Especificação referente a se item deve possuir mesmo departamento do inventario.
    /// </summary>
    public class ItemDevePossuirMesmoDepartamentoDoInventarioSpec : SpecBase<ItemDetalhe>
    {
        private readonly int m_idDepartamentoInventario;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ItemDevePossuirMesmoDepartamentoDoInventarioSpec"/>.
        /// </summary>
        /// <param name="idDepartamentoInventario">O id de departamento inventario.</param>
        public ItemDevePossuirMesmoDepartamentoDoInventarioSpec(int idDepartamentoInventario)
        {
            m_idDepartamentoInventario = idDepartamentoInventario;
        }

        /// <summary>
        /// Verifica se o item detalhe informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O item detalhe.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo item detalhe.
        /// </returns>
        public override SpecResult IsSatisfiedBy(ItemDetalhe target)
        {
            return target.IDDepartamento == m_idDepartamentoInventario
                ? Satisfied()
                : NotSatisfied(Texts.ItemDevePossuirMesmoDepartamentoDoInventarioSpecReason);
        }
    }
}