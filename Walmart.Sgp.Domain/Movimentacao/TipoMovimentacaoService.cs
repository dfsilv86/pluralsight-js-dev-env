using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Movimentacao
{
    /// <summary>
    /// Serviço de domínio para tipo de movimentação.
    /// </summary>
    public class TipoMovimentacaoService : DomainServiceBase<ITipoMovimentacaoGateway>, ITipoMovimentacaoService
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="TipoMovimentacaoService"/>.
        /// </summary>
        /// <param name="mainGateway">O table data gateway para tipo movimentação.</param>
        public TipoMovimentacaoService(ITipoMovimentacaoGateway mainGateway)
            : base(mainGateway)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obtém tipos de movimentação da categoria informada.
        /// </summary>
        /// <param name="categoria">A categoria.</param>
        /// <returns>Os tipos de movimentação.</returns>
        public IEnumerable<TipoMovimentacao> ObterPorCategoria(CategoriaTipoMovimentacao categoria)
        {
            var ids = CategoriaTipoMovimentacao.ObterIdsTipoMovimentacao(categoria);
            return MainGateway.Find("IDTipoMovimentacao IN @ids", new { ids = ids });
        }
        #endregion
    }
}
