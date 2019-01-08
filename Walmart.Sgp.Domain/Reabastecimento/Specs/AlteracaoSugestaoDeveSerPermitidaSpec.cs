using System;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Reabastecimento.Specs
{
    /// <summary>
    /// Especificação referente a se alteração em um valor da sugestão de pedido é permitida e válida conforme cálculos baseados na alçada ou no estoque informado.
    /// </summary>
    public class AlteracaoSugestaoDeveSerPermitidaSpec : SpecBase<SugestaoPedidoModel>
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
            if (target.dtPedido.Date != DateTime.Today)
            {
                return NotSatisfied(Texts.MaySaveOnlySuggestionsFromCurrentDay);
            }

            if (DeveValidarEstoque(target))
            {
                var result = new ValorEstoqueInformadoDeveSerValidoSpec().IsSatisfiedBy(target);

                if (!result)
                {
                    return result;
                }
            }
            else if (target.qtdPackCompra != target.Original_qtdPackCompra)
            {
                var result = new AlcadaDevePermitirSugestaoSpec().IsSatisfiedBy(target);

                if (!result)
                {
                    return result;
                }
            }

            return Satisfied();
        }

        private static bool DeveValidarEstoque(SugestaoPedidoModel target)
        {
            return target.vlEstoque != target.Original_vlEstoque && !target.qtdPackCompraAlterado;
        }
    }
}
