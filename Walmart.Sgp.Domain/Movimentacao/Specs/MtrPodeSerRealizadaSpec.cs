using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Movimentacao.Specs
{
    /// <summary>
    /// Especificação referente a se pode realizar uma MTR.
    /// </summary>
    public class MtrPodeSerRealizadaSpec : SpecBase<MovimentacaoMtr>
    {
        #region Fields
        private readonly IItemDetalheService m_itemDetalheService;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="MtrPodeSerRealizadaSpec" />.
        /// </summary>
        /// <param name="itemDetalheService">O serviço de item detalhe.</param>
        public MtrPodeSerRealizadaSpec(IItemDetalheService itemDetalheService)
        {
            m_itemDetalheService = itemDetalheService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Verifica se o alvo informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O alvo.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo alvo.
        /// </returns>
        public override SpecResult IsSatisfiedBy(MovimentacaoMtr target)
        {
            if (target.Quantidade == 0)
            {
                return NotSatisfied(Texts.NeedToConfirmQuantity);
            }

            if (target.IdItemOrigem == target.IdItemDestino)
            {
                return NotSatisfied(Texts.SourceItemAndDestItemShouldBeDiff);
            }

            if (AreItemsFromSameDepartment(target))
            {
                return NotSatisfied(Texts.SourceItemAndDestItemShouldBeFromDiffDepartament);
            }

            var itemDest = this.m_itemDetalheService.ObterPorId(target.IdItemDestino);
            if (itemDest.TpManipulado == TipoManipulado.Derivado || itemDest.TpReceituario == TipoReceituario.Transformado || itemDest.TpVinculado == TipoVinculado.Entrada)
            {
                return NotSatisfied(Texts.DestinationItemTypeInvalid);
            }

            return Satisfied();
        }

        private bool AreItemsFromSameDepartment(MovimentacaoMtr target)
        {
             return m_itemDetalheService.ObterPorIds(target.IdItemOrigem, target.IdItemDestino)
                .Select(i => i.IDDepartamento)
                .Distinct()
                .Count() == 1;
        }
        #endregion
    }
}
