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
    /// Especificação referente a se um relacionamento de item pode ser salvo.
    /// </summary>
    public class RelacionamentoItemPodeSerSalvoSpec : SpecBase<RelacionamentoItemPrincipal>
    {
        /// <summary>
        /// Verifica se o alvo informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O alvo.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo alvo.
        /// </returns>
        public override SpecResult IsSatisfiedBy(RelacionamentoItemPrincipal target)
        {
            if (!TemSecundario(target))
            {
                return NotSatisfied(Texts.DefineAtLeastOneExitItem.With(target.EhEntrada() ? Texts.ExitItem : Texts.InputItem));
            }

            if (!ReceituarioTemInsumo(target))
            {
                return NotSatisfied(Texts.InformAtLeastOneItemRawMaterial);
            }

            if (target.StatusReprocessamentoCusto == StatusReprocessamentoCusto.Processando)
            {
                return NotSatisfied(Texts.RelacionamentoItemPrincipalCanBeSavedIsProcessing);
            }

            return Satisfied();
        }

        private static bool TemSecundario(RelacionamentoItemPrincipal target)
        {
            return target.RelacionamentoSecundario != null && target.RelacionamentoSecundario.Any();
        }

        private static bool ReceituarioTemInsumo(RelacionamentoItemPrincipal target)
        {
            return (target.TipoRelacionamento == TipoRelacionamento.Receituario && 
                target.RelacionamentoSecundario.Any(t => t.TpItem == TipoItemEntrada.Insumo)) ||
                target.TipoRelacionamento != TipoRelacionamento.Receituario;
        }
    }
}
