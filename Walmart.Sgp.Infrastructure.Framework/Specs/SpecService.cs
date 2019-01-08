using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Walmart.Sgp.Infrastructure.Framework.Specs
{
    /// <summary>
    /// Serviço de infra para especificações.
    /// </summary>
    public static class SpecService
    {
        #region Methods
        /// <summary>
        /// Verifica se todas as especificações são satisfeitas, ao achar uma que não for satisfeita, será lançada um SpecificationNotSatisfiedException.
        /// </summary>
        /// <typeparam name="TTarget">O tipo do alvo da especificações.</typeparam>
        /// <param name="target">O alvo da especificações.</param>
        /// <param name="specifications">As especificações.</param>
        public static void Assert<TTarget>(TTarget target, params ISpec<TTarget>[] specifications)
        {
            foreach (var spec in specifications)
            {
                var result = spec.IsSatisfiedBy(target);

                if (!result.Satisfied)
                {
                    throw new NotSatisfiedSpecException(result.Reason);
                }
            }
        }

        /// <summary>
        /// Verifica se todas as especificações são satisfeitas, ao achar uma que não for satisfeita, será lançada um SpecificationNotSatisfiedException.
        /// </summary>
        /// <typeparam name="TTarget">O tipo do alvo da especificações.</typeparam>
        /// <param name="targets">Os alvos das especificações.</param>
        /// <param name="specifications">As especificações.</param>
        public static void Assert<TTarget>(IEnumerable<TTarget> targets, params ISpec<TTarget>[] specifications)
        {
            foreach (var target in targets)
            {
                Assert(target, specifications);
            }
        }

        /// <summary>
        /// Verifica se todas as especificações são satisfeitas. A final se pelo menos uma única não for, será lançada um SpecificationNotSatisfiedException com a razão de todas que não foram satisfietas.
        /// </summary>
        /// <typeparam name="TTarget">O tipo do alvo da especificações.</typeparam>
        /// <param name="targets">Os alvos das especificações.</param>
        /// <param name="specifications">As especificações.</param>
        public static void AssertAll<TTarget>(IEnumerable<TTarget> targets, params ISpec<TTarget>[] specifications)
        {
            var reasons = new List<string>();

            foreach (var target in targets)
            {
                foreach (var spec in specifications)
                {
                    var result = spec.IsSatisfiedBy(target);

                    if (!result.Satisfied)
                    {
                        reasons.Add(result.Reason);
                    }
                }
            }

            if (reasons.Count > 0)
            {
                throw new NotSatisfiedSpecException(string.Join("\n", reasons));
            }
        }

        /// <summary>
        /// Verifica se o alvo satisfaz todas as especificações. Retorna um <see cref="AggregatedSpecResult"/> com o resultado agregado.
        /// </summary>
        /// <typeparam name="TTarget">O tipo do alvo da especificações.</typeparam>
        /// <param name="target">O alvo da especificações.</param>
        /// <param name="specifications">As especificações.</param>
        /// <returns>AggregatedSpecResult contendo resultado agregado das especificações.</returns>
        /// <remarks>Utilizado pela importação excel de Multisourcing.</remarks>
        public static AggregatedSpecResult IsSatisfiedByAll<TTarget>(TTarget target, params ISpec<TTarget>[] specifications)
        {
            var results = from spec in specifications select spec.IsSatisfiedBy(target);
            var aggregated = results.OfType<AggregatedSpecResult>().ToList();
            var nonAggregated = new AggregatedSpecResult(results.Except(aggregated));

            aggregated.Add(nonAggregated);

            if (aggregated.Count < 2)
            {
                return aggregated.SingleOrDefault();
            }
            else
            {
                return new AggregatedSpecResult(aggregated);
            }
        }

        /// <summary>
        /// Obtém o resultado das especificações não são satifeitas.
        /// </summary>
        /// <typeparam name="TTarget">O tipo do alvo da especificações.</typeparam>
        /// <param name="target">O alvo das especificações.</param>
        /// <param name="specifications">As especificações.</param>
        /// <returns>O resultado das especificações.</returns>
        public static IEnumerable<SpecResult> GetNotSatisfiedBy<TTarget>(TTarget target, params ISpec<TTarget>[] specifications)
        {
            var results = new List<SpecResult>();

            foreach (var spec in specifications)
            {
                var specResult = spec.IsSatisfiedBy(target);

                if (!specResult.Satisfied)
                {
                    results.Add(specResult);
                }
            }

            return results;
        }

        /// <summary>
        /// Obtém o resultado das especificações não são satifeitas.
        /// </summary>
        /// <typeparam name="TTarget">O tipo do alvo da especificações.</typeparam>
        /// <param name="targets">Os alvos das especificações.</param>
        /// <param name="specifications">As especificações.</param>
        /// <returns>O resultado das especificações.</returns>
        public static IEnumerable<SpecResult> GetNotSatisfiedBy<TTarget>(IEnumerable<TTarget> targets, params ISpec<TTarget>[] specifications)
        {
            var results = new List<SpecResult>();

            foreach (var target in targets)
            {
                results.AddRange(GetNotSatisfiedBy(target, specifications));
            }

            return results;
        }
        #endregion
    }
}
