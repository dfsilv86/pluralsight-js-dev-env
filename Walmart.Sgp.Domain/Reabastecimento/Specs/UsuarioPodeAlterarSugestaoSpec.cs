using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Reabastecimento.Specs
{
    /// <summary>
    /// Especificação referente a se o usuário pode alterar sugestao de pedido.
    /// </summary>
    public class UsuarioPodeAlterarSugestaoSpec : SpecBase<IRuntimeUser>
    {
        private readonly IAlcadaService m_service;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="UsuarioPodeAlterarSugestaoSpec"/>
        /// </summary>
        /// <param name="service">O serviço de alcada.</param>
        public UsuarioPodeAlterarSugestaoSpec(IAlcadaService service)
        {
            m_service = service;
        }

        /// <summary>
        /// Verifica se o sugestao pedido model informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O sugestao pedido model.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo sugestao pedido model.
        /// </returns>
        public override SpecResult IsSatisfiedBy(IRuntimeUser target)
        {
            var alcada = m_service.ObterPorPerfil(target.RoleId);

            return null == alcada || alcada.blAlterarSugestao ? Satisfied() :
                NotSatisfied(Texts.CannotChangeSuggestion);                
        }
    }
}
