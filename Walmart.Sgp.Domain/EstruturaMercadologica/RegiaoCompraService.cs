using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Serviço de domínio relacionado a regiao compra.
    /// </summary>
    public class RegiaoCompraService : EntityDomainServiceBase<RegiaoCompra, IRegiaoCompraGateway>, IRegiaoCompraService
    {
        #region Constructors

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="RegiaoCompraService"/>.
        /// </summary>
        /// <param name="mainGateway">O table data gateway para região compra.</param>
        public RegiaoCompraService(IRegiaoCompraGateway mainGateway)
            : base(mainGateway)
        {
        }

        #endregion
    }
}