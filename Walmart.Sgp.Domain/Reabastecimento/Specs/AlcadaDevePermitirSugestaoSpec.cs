using System;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Commons;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Reabastecimento.Specs
{
    /// <summary>
    /// Especificação referente a se alcada valida.
    /// </summary>
    public class AlcadaDevePermitirSugestaoSpec : SpecBase<SugestaoPedidoModel>
    {
        /// <summary>
        /// Verifica se o sugestao pedido model informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O sugestao pedido model.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo sugestao pedido model.
        /// </returns>
        public override SpecResult IsSatisfiedBy(SugestaoPedidoModel target)
        {
            if (target.cdOrigemCalculo == TipoOrigemCalculo.Manual)
            {
                // origem manual não valida alçada - aspx linha 248
                return Satisfied();
            }

            if (target.qtdPackCompra == 0)
            {
                // aspx linha 197
                // msg: Campo Qtde. Solicitada deve ser maior que 0
                return target.blZerarItem || target.qtdPackCompraOriginal == 0 ? Satisfied() : NotSatisfied(Texts.CannotZeroQtdPackCompra);
            }

            if (target.blAlterarPercentual)
            {
                // Determina se deve validar múltiplo de peso:
                // Multiplo de peso é validado apenas se peso (liquido ou bruto) for relevante
                // Caso contrário nao valida múltiplo.
                if (target.IsPesoLiquido() || target.IsPesoBruto())
                {
                    // Bug #4866 - Arredonda vlModulo para conciliar com o requisito de que o campo qtdPackCompra seja inteiro (seria truncado), 
                    //             o requisito de que o cálculo seja feito em cima do valor decimal (psqSugestaoPedido.aspx linhas 209 a 213) e o requisito de que o 
                    //             save seja feito em cima do valor truncado (psqSugestaoPedido.aspx linhas 138 ou 143)
                    // PESS: agora diferencia entre peso bruto e peso líquido - ObterPeso()
                    if (target.qtdPackCompra % Math.Max(1, Math.Round(target.ObterPeso(), 0, MidpointRounding.AwayFromZero)) > 0)
                    {
                        // aspx linha 213
                        // mensagem: Valor informado não é multiplo do Peso.
                        return NotSatisfied(Texts.QtdPackCompraIsNotMultipleOfWeight);
                    }
                }

                if (!(target.vlLimiteInferior <= target.qtdPackCompra && target.qtdPackCompra <= target.vlLimiteSuperior))
                {
                    // aspx linha 223
                    // mensagem: Os valores calculados não correspondem a Alçada definida para este perfil.
                    return NotSatisfiedByLimites(target);
                }
            }
            else
            {
                // aspx linha 232 - talvez o campo devesse estar desabilitado?
                // mensagem: Alçada não permite alteração ou não está configurada.
                return NotSatisfied(Texts.NoCompetenceDefined);
            }

            return Satisfied();
        }

        private static SpecResult NotSatisfiedByLimites(SugestaoPedidoModel target)
        {
            var limites = new RangeValue<decimal>();

            if (target.qtdPackCompraOriginal == 0 && target.TpCaixaFornecedor == TipoCaixaFornecedor.Caixa)
            {
                limites.StartValue = 1;
            }
            else
            {
                limites.StartValue = target.vlLimiteInferior;
            }
            
            limites.EndValue = target.vlLimiteSuperior;

            // Development 5127:#15 Ao exibir min/max na mensagem de validação da alçada, acatar regras do multiplo de peso.
            // Multiplo de peso apenas se for peso liquido ou bruto.
            if (target.IsPesoLiquido() || target.IsPesoBruto())
            {
                limites = target.CalcularLimitesDisponiveis();
            }

            return FormatarValoresRangeAlcada(limites);
        }

        private static SpecResult FormatarValoresRangeAlcada(RangeValue<decimal> limites)
        {
            limites.StartValue = (int)Math.Ceiling(limites.StartValue ?? 1);
            limites.EndValue = (int)Math.Floor(limites.EndValue ?? 1);

            // aspx linha 223
            // mensagem: Os valores calculados não correspondem a Alçada definida para este perfil.
            var limiteInferiorText = limites.StartValue.Value.ToString("N0", RuntimeContext.Current.Culture);
            var limiteSuperiorText = limites.EndValue.Value.ToString("N0", RuntimeContext.Current.Culture);

            return NotSatisfied(Texts.QtdPackCompraIsOutsideRange.With(limiteInferiorText, limiteSuperiorText));
        }
    }
}
