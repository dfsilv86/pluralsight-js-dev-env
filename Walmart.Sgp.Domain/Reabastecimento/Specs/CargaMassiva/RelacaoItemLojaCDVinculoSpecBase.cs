using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Reabastecimento.Specs.CargaMassiva
{
    /// <summary>
    /// Classe base para especificações validam listas de RelacaoItemLojaCDVinculo.
    /// </summary>
    /// <typeparam name="TKey">O tipo de chave para o agrupamento.</typeparam>
    public abstract class RelacaoItemLojaCDVinculoSpecBase<TKey> : SpecBase<IEnumerable<RelacaoItemLojaCDVinculo>>
    {
        /// <summary>
        /// Obtém a chave para agrupamento.
        /// </summary>
        protected abstract Func<RelacaoItemLojaCDVinculo, TKey> Key { get; }

        /// <summary>
        /// A mensagem usada caso o grupo não seja válido.
        /// </summary>
        protected abstract string NotSatisfiedReason { get; }

        /// <summary>
        /// Agrupa uma lista de multisourcings por uma chave e verifica os grupos.
        /// </summary>
        /// <param name="target">A lista de RelacaoItemLojaCDVinculo.</param>
        /// <returns>
        /// Se a lista é válida.
        /// </returns>
        public override SpecResult IsSatisfiedBy(IEnumerable<RelacaoItemLojaCDVinculo> target)
        {
            var grouped = target.GroupBy(this.Key);
            var results = new List<SpecResult>();

            foreach (var group in grouped)
            {
                bool isSatisfied;
                SpecResult groupResult;

                isSatisfied = this.IsSatisfiedByGroup(group);

                groupResult = new SpecResult(isSatisfied, this.NotSatisfiedReason);
                results.Add(groupResult);

                if (isSatisfied)
                {
                    continue;
                }

                AddReasons(group, groupResult);
            }

            return new AggregatedSpecResult(results);
        }

        /// <summary>
        /// Valida se um grupo de RelacaoItemLojaCDVinculo é válido.
        /// </summary>
        /// <param name="group">O grupo.</param>
        /// <returns>True caso válido, false caso contrário.</returns>
        protected abstract bool IsSatisfiedByGroup(IGrouping<TKey, RelacaoItemLojaCDVinculo> group);
        
        private static void AddReasons(IGrouping<TKey, RelacaoItemLojaCDVinculo> group, SpecResult groupResult)
        {
            foreach (var item in group)
            {
                if (item.NotSatisfiedSpecReasons == null)
                {
                    item.NotSatisfiedSpecReasons = new List<string>();
                }

                item.NotSatisfiedSpecReasons.Add(groupResult.Reason);
            }
        }
    }
}
