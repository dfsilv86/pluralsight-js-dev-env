using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Reabastecimento.Specs
{
    /// <summary>
    /// Especificação referente a se valor estoque ser valido.
    /// </summary>
    public class ValorEstoqueInformadoDeveSerValidoSpec : SpecBase<SugestaoPedidoModel>
    {
        #region Methods
        /// <summary>
        /// Verifica se o sugestao pedido model informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O sugestao pedido model.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo sugestao pedido model.
        /// </returns>
        public override SpecResult IsSatisfiedBy(SugestaoPedidoModel target)
        {
            if (target.vlEstoque == target.Original_vlEstoque || target.cdOrigemCalculo == TipoOrigemCalculo.Manual)
            {
                return Satisfied();
            }

            var temp = SugestaoPedidoModel.Recalcular(target);
            var validarAlcada = new AlcadaDevePermitirSugestaoSpec();
            var resultadoValidacaoAlcada = validarAlcada.IsSatisfiedBy(temp);

            if (!resultadoValidacaoAlcada)
            {
                return resultadoValidacaoAlcada;
            }

            return Satisfied();
        }
        #endregion
    }
}
