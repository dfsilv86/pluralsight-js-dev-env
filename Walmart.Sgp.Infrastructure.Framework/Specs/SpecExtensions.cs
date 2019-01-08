using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.Framework.Specs
{
    /// <summary>
    /// Extension methods para spec.
    /// </summary>
    public static class SpecExtensions
    {
        /// <summary>
        /// Cria uma nova especificação que nega o resultado da especificação e utiliza a razão informada.
        /// </summary>
        /// <typeparam name="TTarget">O tipo do alvo da especificação.</typeparam>
        /// <param name="spec">A especificação que será negada.</param>
        /// <param name="reason">The razão.</param>
        /// <returns>A especificação de negação.</returns>
        public static ISpec<TTarget> Not<TTarget>(this ISpec<TTarget> spec, string reason)
        {
            return new NotSpec<TTarget>(spec, reason);
        }

        /// <summary>
        /// Filtra uma lista de alvos conforme a avaliação da especificação.
        /// </summary>
        /// <typeparam name="TTarget">O tipo do alvo da especificação.</typeparam>
        /// <param name="targets">A lista de alvos.</param>
        /// <param name="spec">A especificação.</param>
        /// <returns>
        /// Lista de valores que atendem à especificação.
        /// </returns>
        public static IEnumerable<TTarget> IsSatisfiedBy<TTarget>(this IEnumerable<TTarget> targets, params ISpec<TTarget>[] spec) 
        {
            return targets.Where(t => spec.All(s => s.IsSatisfiedBy(t)));
        }

        /// <summary>
        /// Filtra uma lista de alvos conforme a negação da avaliação da especificação.
        /// </summary>
        /// <typeparam name="TTarget">O tipo do alvo da especificação.</typeparam>
        /// <param name="targets">A lista de alvos.</param>
        /// <param name="spec">A especificação.</param>
        /// <returns>
        /// Lista de valores que não atendem à especificação.
        /// </returns>
        public static IEnumerable<TTarget> IsNotSatisfiedBy<TTarget>(this IEnumerable<TTarget> targets, params ISpec<TTarget>[] spec)
        {
            return targets.Where(t => spec.Any(s => !s.IsSatisfiedBy(t)));
        }
    }
}
