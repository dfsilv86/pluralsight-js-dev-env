using System;
using System.Collections.Generic;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.MultisourcingVendor.Specs
{
    /// <summary>
    /// Classe base para especificações validam listas de multisourcing.
    /// </summary>
    /// <typeparam name="TKey">O tipo de chave para o agrupamento.</typeparam>
    public abstract class MultisourcingSpecBase<TKey> : SpecBase<IEnumerable<Multisourcing>>
    {
        /// <summary>
        /// Obtém a chave para agrupamento.
        /// </summary>
        protected abstract Func<Multisourcing, TKey> Key { get; }

        /// <summary>
        /// Obtém objeto que agrupa todas as colunas gerando um registro único.
        /// </summary>
        protected Func<Multisourcing, object> GroupAll
        {
            get
            {
                return (m) => new { m.CdItemDetalheSaida, m.CdItemDetalheEntrada, m.Vendor9Digitos, m.CD, m.vlPercentual };
            }
        }

        /// <summary>
        /// A mensagem usada caso o grupo não seja válido.
        /// </summary>
        protected abstract string NotSatisfiedReason { get; }

        /// <summary>
        /// Agrupa uma lista de multisourcings por uma chave e verifica os grupos.
        /// </summary>
        /// <param name="target">A lista de multisourcings.</param>
        /// <returns>
        /// Se a lista é válida.
        /// </returns>
        public override SpecResult IsSatisfiedBy(IEnumerable<Multisourcing> target)
        {
            var grouped = target.GroupBy(this.Key);
            var results = new List<SpecResult>();

            foreach (var group in grouped)
            {
                var isSatisfied = this.IsSatisfiedByGroup(group);

                var groupResult = new SpecResult(isSatisfied, this.NotSatisfiedReason);
                results.Add(groupResult);

                if (isSatisfied)
                {
                    continue;
                }
                
                foreach (var item in group)
                {
                    item.NotSatisfiedSpecReasons.Add(groupResult.Reason);
                }
            }

            return new AggregatedSpecResult(results);
        }

        /// <summary>
        /// Valida se um grupo de multisourcing é válido.
        /// </summary>
        /// <param name="group">O grupo.</param>
        /// <returns>True caso válido, false caso contrário.</returns>
        protected abstract bool IsSatisfiedByGroup(IGrouping<TKey, Multisourcing> group);
    }
}