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
    /// Especificação referente a se um relacionamento de item pode ser removido.
    /// </summary>
    public class RelacionamentoItemPodeSerRemovidoSpec : SpecBase<RelacionamentoItemPrincipal>
    {
        private Func<RelacionamentoItemPrincipal, bool> m_verificarCadMultisourcingExistente;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="RelacionamentoItemPodeSerRemovidoSpec" />
        /// </summary>
        /// <param name="verificarCadMultisourcingExistente">Delegate para verificar se existe multisourcing.</param>
        public RelacionamentoItemPodeSerRemovidoSpec(Func<RelacionamentoItemPrincipal, bool> verificarCadMultisourcingExistente)
        {
            m_verificarCadMultisourcingExistente = verificarCadMultisourcingExistente;
        }

        /// <summary>
        /// Verifica se o alvo informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O alvo.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo alvo.
        /// </returns>
        public override SpecResult IsSatisfiedBy(RelacionamentoItemPrincipal target)
        {            
            if (target.TipoRelacionamento == TipoRelacionamento.Vinculado)
            {
                var item = target.ItemDetalhe;

                if (item.TpReceituario == TipoReceituario.Insumo || item.TpManipulado == TipoManipulado.Pai)
                {
                    return NotSatisfied(Texts.CannotDeleteWhenParentManipulatedOrInputRecipe);
                }
            }

            if (m_verificarCadMultisourcingExistente(target))
            {
                return NotSatisfied(Texts.CantDeleteBecauseItemHaveMultisourcing);
            }

            return Satisfied();
        }
    }
}
