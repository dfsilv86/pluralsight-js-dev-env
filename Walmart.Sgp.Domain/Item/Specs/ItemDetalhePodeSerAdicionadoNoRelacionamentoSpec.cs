using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Item.Specs
{
    /// <summary>
    /// Especificação referente a se um item detalhe pode ser adicionado a um relacionamento.
    /// </summary>
    public class ItemDetalhePodeSerAdicionadoNoRelacionamentoSpec : SpecBase<ItemDetalhe>
    {
        #region Fields
        private RelacionamentoItemPrincipal m_principal;
        #endregion

        #region Constructors        
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ItemDetalhePodeSerAdicionadoNoRelacionamentoSpec"/>.
        /// </summary>
        /// <param name="principal">O item principal do relacionamento.</param>
        public ItemDetalhePodeSerAdicionadoNoRelacionamentoSpec(RelacionamentoItemPrincipal principal)
        {
            m_principal = principal;
        }
        #endregion

        /// <summary>
        /// Verifica se o alvo informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O alvo.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo alvo.
        /// </returns>
        public override SpecResult IsSatisfiedBy(ItemDetalhe target)
        {
            // Item principal e item secundário não podem ser o mesmo.
            if (m_principal.IDItemDetalhe == target.IDItemDetalhe)
            {
                return NotSatisfied(Texts.SecondaryItemShouldBeDiffThanMainItem.With(target.CdItem));
            }

            // Apenas Vinculado pode relacionar com itens de departamentos diferentes.
            if (m_principal.TipoRelacionamento != TipoRelacionamento.Vinculado && m_principal.ItemDetalhe.IDDepartamento != target.IDDepartamento)
            {
                return NotSatisfied(Texts.SecondaryItemWithDepartmentDiffThanMainItem);
            }

            // Item já foi adicionado no relacionamento.
            if (ItemJaExisteNoRelacionamento(target))
            {
                return NotSatisfied(Texts.SecondaryItemAlreadyInRelationship.With(target.CdItem));
            }
          
            return Satisfied();
        }

        private bool ItemJaExisteNoRelacionamento(ItemDetalhe item)
        {
            return m_principal.RelacionamentoSecundario.Count(t => !ReferenceEquals(t.ItemDetalhe, item) && t.IDItemDetalhe == item.IDItemDetalhe) > 0;
        }
    }
}
