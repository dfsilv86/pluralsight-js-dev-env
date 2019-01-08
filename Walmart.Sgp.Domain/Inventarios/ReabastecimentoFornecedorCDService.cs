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
    /// Serviço de domínio relacionado a ReabastecimentoFornecedorCD.
    /// </summary>
    public class ReabastecimentoFornecedorCDService : EntityDomainServiceBase<ReabastecimentoFornecedorCD, IReabastecimentoFornecedorCDGateway>, IReabastecimentoFornecedorCDService
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ReabastecimentoFornecedorCDService"/>.
        /// </summary>
        /// <param name="mainGateway">O table data gateway para ReabastecimentoFornecedorCD.</param>
        public ReabastecimentoFornecedorCDService(IReabastecimentoFornecedorCDGateway mainGateway)
            : base(mainGateway)
        {
        }
        #endregion
    }
}