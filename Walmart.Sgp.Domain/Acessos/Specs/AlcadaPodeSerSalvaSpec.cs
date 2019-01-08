using System;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Acessos.Specs
{
    /// <summary>
    /// Especificação de persistencia da alçada.
    /// </summary>
    public class AlcadaPodeSerSalvaSpec : SpecBase<Alcada>
    {
        /// <summary>
        /// Verifica se o alvo informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O alvo.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo alvo.
        /// </returns>
        public override SpecResult IsSatisfiedBy(Alcada target)
        {
            return And(target, ValidarIdPerfil, ValidarCamposObrigatorios, ValidarPercentualAlterado);
        }

        private static SpecResult And(Alcada target, params Func<Alcada, SpecResult>[] specMethods)
        {
            var result = Satisfied();
            foreach (var method in specMethods)
            {
                result = method(target);
                if (!result.Satisfied)
                {
                    break;
                }
            }

            return result;
        }

        private SpecResult ValidarIdPerfil(Alcada target)
        {
            return new AllMustBeInformedSpec().IsSatisfiedBy(new
            {
                target.IDPerfil
            });
        }

        private SpecResult ValidarCamposObrigatorios(Alcada target)
        {
            if (!target.blAlterarPercentual)
            {
                return Satisfied();
            }

            return new AllMustBeInformedSpec().IsSatisfiedBy(new
            {
                target.vlPercentualAlterado                
            });
        }

        private SpecResult ValidarPercentualAlterado(Alcada target)
        {
            return new AllMustBeGraterThanSpec(0.0m).IsSatisfiedBy(new
            {
                target.vlPercentualAlterado
            });
        }
    }
}