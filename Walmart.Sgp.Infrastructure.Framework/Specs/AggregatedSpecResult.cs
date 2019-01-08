using System.Collections.Generic;
using System.Linq;

namespace Walmart.Sgp.Infrastructure.Framework.Specs
{
    /// <summary>
    /// Resultado agregado de spec, baseado em uma lista de resultados.
    /// </summary>
    public class AggregatedSpecResult : SpecResult
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="AggregatedSpecResult"/>.
        /// </summary>
        /// <param name="results">Uma lista de resultados agregados.</param>
        /// <remarks>Achata as listas de resultados e retorna um único resultado agregado formado por todos resultados individuais.</remarks>
        public AggregatedSpecResult(IEnumerable<AggregatedSpecResult> results)
            : this(results.SelectMany(r => r.Results))
        {
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="AggregatedSpecResult"/>.
        /// </summary>
        /// <param name="results">Uma lista de resultados.</param>
        /// <remarks>Retorna um resultado agregado composto por todos resultados individuais, onde o resultado agregado só é satisfeito caso todos os resultados individuais estejam satisfeitos.</remarks>
        public AggregatedSpecResult(IEnumerable<SpecResult> results)
            : base(results.All(r => r.Satisfied), string.Join(", ", results.Where(r => !string.IsNullOrWhiteSpace(r.Reason)).Select(r => r.Reason).Distinct().ToArray()))
        {
            this.Results = results;
        }

        /// <summary>
        /// Obtém a lista de resultados individuais.
        /// </summary>
        public IEnumerable<SpecResult> Results { get; private set; }
    }
}
