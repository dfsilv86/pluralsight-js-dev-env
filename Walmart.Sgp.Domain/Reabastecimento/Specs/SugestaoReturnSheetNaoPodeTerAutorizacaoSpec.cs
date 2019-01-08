using System;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Reabastecimento.Specs
{
    /// <summary>
    /// Spec para SugestaoReturnSheet Nao Pode Ter Autorização
    /// </summary>
    public class SugestaoReturnSheetNaoPodeTerAutorizacaoSpec : SpecBase<SugestaoReturnSheet>
    {
        private Func<int, bool> m_possuiAutorizacao;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="SugestaoReturnSheetNaoPodeTerAutorizacaoSpec"/>.
        /// </summary>
        /// <param name="possuiAutorizacao">O delegate que verifica se um returnsheet tem autorização.</param>
        public SugestaoReturnSheetNaoPodeTerAutorizacaoSpec(Func<int, bool> possuiAutorizacao)
        {
            this.m_possuiAutorizacao = possuiAutorizacao;
        }

        /// <summary>
        /// Verifica se o objeto informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O objeto a ser validado.</param>
        /// <returns>
        /// Se a especificação foi satisfeita.
        /// </returns>
        public override SpecResult IsSatisfiedBy(SugestaoReturnSheet target)
        {
            var idReturnSheet = target.ItemLoja.ItemPrincipal.ReturnSheet.Id;

            if (m_possuiAutorizacao(idReturnSheet))
            {
                return NotSatisfied(Texts.CantChangeAuthorizedReturnSheet);
            }

            return Satisfied();
        }
    }
}
