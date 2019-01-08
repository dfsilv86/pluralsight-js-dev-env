using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Reabastecimento.Specs
{
    /// <summary>
    /// Especificação referente a se a grade de sugestão é única.
    /// </summary>
    public class GradeSugestaoDeveSerUnicaSpec : SpecBase<GradeSugestao>
    {
        private readonly IGradeSugestaoGateway m_gateway;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="GradeSugestaoDeveSerUnicaSpec"/>.
        /// </summary>
        /// <param name="gateway">O gateway.</param>
        public GradeSugestaoDeveSerUnicaSpec(IGradeSugestaoGateway gateway)
        {
            m_gateway = gateway;
        }

        /// <summary>
        /// Verifica se o alvo informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O alvo.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo alvo.
        /// </returns>
        public override SpecResult IsSatisfiedBy(GradeSugestao target)
        {
            var filter = "IDGradeSugestao <> @IDGradeSugestao AND IDBandeira = @IDBandeira AND IDLoja = @IDLoja AND IDDepartamento = @IDDepartamento";
            var filterArgs = new
            {
                target.IDGradeSugestao,
                target.IDBandeira,
                target.IDLoja,
                target.IDDepartamento
            };

            var count = m_gateway.Count(
                filter,
                filterArgs);

            if (count > 0)
            {
                return NotSatisfied(Texts.SuggestionGridAlreadyExists);
            }

            return Satisfied();
        }
    }
}