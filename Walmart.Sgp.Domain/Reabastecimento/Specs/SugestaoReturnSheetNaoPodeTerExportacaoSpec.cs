using System;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Reabastecimento.Specs
{
    /// <summary>
    /// Spec para SugestaoReturnSheet Nao Pode Ter Exportacao
    /// </summary>
    public class SugestaoReturnSheetNaoPodeTerExportacaoSpec : SpecBase<SugestaoReturnSheet>
    {
        private Func<int, bool> m_possuiExportacao;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="SugestaoReturnSheetNaoPodeTerExportacaoSpec"/>.
        /// </summary>
        /// <param name="possuiExportacao">O delegate que verifica se um returnsheet tem exportacao.</param>
        public SugestaoReturnSheetNaoPodeTerExportacaoSpec(Func<int, bool> possuiExportacao)
        {
            this.m_possuiExportacao = possuiExportacao;
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

            if (m_possuiExportacao(idReturnSheet))
            {
                return NotSatisfied(Texts.CantChangeExportedReturnSheet);
            }

            return Satisfied();
        }
    }
}
