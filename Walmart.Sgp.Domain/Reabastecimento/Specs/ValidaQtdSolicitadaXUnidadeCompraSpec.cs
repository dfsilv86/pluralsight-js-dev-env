using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Reabastecimento.Specs
{
    /// <summary>
    /// Spec ValidaQtdSolicitadaXUnidadeCompraSpec 
    /// </summary>
    public class ValidaQtdSolicitadaXUnidadeCompraSpec : SpecBase<SugestaoReturnSheet>
    {
        private bool m_isRA;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ValidaQtdSolicitadaXUnidadeCompraSpec" />
        /// </summary>
        /// <param name="isRA">Se a validação deve ser executada no escopo RA.</param>
        public ValidaQtdSolicitadaXUnidadeCompraSpec(bool isRA)
        {
            m_isRA = isRA;
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
            if (target.ItemLoja.ItemPrincipal.ItemDetalhe.TpCaixaFornecedor == TipoCaixaFornecedor.Caixa)
            {
                return m_isRA ? ValidarTipoCaixaRA(target) : ValidarTipoCaixa(target);
            }

            return m_isRA ? ValidarTipoKgOuUnidadeRA(target) : ValidarTipoKgOuUnidade(target);
        }

        private static SpecResult ValidarTipoCaixa(SugestaoReturnSheet target)
        {
            return target.QtdLoja >= 1 ? 
                Satisfied() :
                NotSatisfied(Texts.QtdInformedMustBeGreaterThanZero);
        }

        private static SpecResult ValidarTipoCaixaRA(SugestaoReturnSheet target)
        {
            return target.QtdRA >= 1 ?
                Satisfied() :
                NotSatisfied(Texts.QtdInformedMustBeGreaterThanZero);
        }

        private static SpecResult ValidarTipoKgOuUnidade(SugestaoReturnSheet target)
        {
            return target.QtdLoja >= 1 && (target.QtdLoja % target.vlPesoLiquidoItemCompra == 0) ?
                Satisfied() :
                NotSatisfied(Texts.QtdMultipleWeight);
        }

        private static SpecResult ValidarTipoKgOuUnidadeRA(SugestaoReturnSheet target)
        {
            return target.QtdRA >= 1 && (target.QtdRA % target.vlPesoLiquidoItemCompra == 0) ?
                Satisfied() :
                NotSatisfied(Texts.QtdMultipleWeight);
        }
    }
}