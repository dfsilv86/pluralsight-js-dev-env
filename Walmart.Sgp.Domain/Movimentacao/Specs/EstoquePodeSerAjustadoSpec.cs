using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Movimentacao.Specs
{
    /// <summary>
    /// Especificação referente a se um estoque pode ser ajustado.
    /// </summary>
    public class EstoquePodeSerAjustadoSpec : SpecBase<Estoque>
    {
        #region Fields
        private readonly INotaFiscalService m_notaFiscalService;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="EstoquePodeSerAjustadoSpec"/>.
        /// </summary>
        /// <param name="notaFiscalService">O serviço de nota fiscal.</param>
        public EstoquePodeSerAjustadoSpec(INotaFiscalService notaFiscalService)
        {
            m_notaFiscalService = notaFiscalService;
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
        public override SpecResult IsSatisfiedBy(Estoque target)
        {
            var itemNaUltimaNotaRecebida = m_notaFiscalService.ObterItemNaUltimaNotaRecebidaDaLoja(target.IDItemDetalhe, target.IDLoja);

            if (itemNaUltimaNotaRecebida != null && itemNaUltimaNotaRecebida.IdNotaFiscalItemStatus == NotaFiscalItemStatus.IdPendente)
            {
                return NotSatisfied(Texts.StockCannotBeAdjustedThereIsPendingInvoice);
            }

            return Satisfied();
        }
        #endregion
    }
}
