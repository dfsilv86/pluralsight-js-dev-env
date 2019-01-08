using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Inventarios
{
    /// <summary>
    /// Serviço de domínio relacionado a ReabastecimentoItemFornecedorCD.
    /// </summary>
    public class ReabastecimentoItemFornecedorCDService : EntityDomainServiceBase<ReabastecimentoItemFornecedorCD, IReabastecimentoItemFornecedorCDGateway>, IReabastecimentoItemFornecedorCDService
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ReabastecimentoItemFornecedorCDService"/>.
        /// </summary>
        /// <param name="mainGateway">O table data gateway para ReabastecimentoItemFornecedorCD.</param>
        public ReabastecimentoItemFornecedorCDService(IReabastecimentoItemFornecedorCDGateway mainGateway)
            : base(mainGateway)
        {
        }
        #endregion
    }
}