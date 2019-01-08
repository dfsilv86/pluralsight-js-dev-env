using System;
using System.Collections.Generic;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Item.Specs
{
    /// <summary>
    /// Especificação referente a se os pesos de um relacionamento de item do tipo receituário podem ser salvos.
    /// </summary>
    public class RelacionamentoItemReceituarioPesoPodeSerSalvoSpec : SpecBase<RelacionamentoItemPrincipal>
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
            if (target.TipoRelacionamento == TipoRelacionamento.Receituario && target.QtProdutoAcabado > target.QtProdutoBruto)
            {
                return NotSatisfied(Texts.FinishedProductWeightMustBeLessOrEqualToWeightGrossProduct);
            }

            return Satisfied();
        }
    }
}
