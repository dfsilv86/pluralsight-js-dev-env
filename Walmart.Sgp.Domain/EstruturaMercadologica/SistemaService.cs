using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Serviço de domínio relacionado a sistema.
    /// </summary>
    public class SistemaService : DomainServiceBase<ISistemaGateway>, ISistemaService
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="SistemaService"/>.
        /// </summary>
        /// <param name="mainGateway">O table data gateway para sistema.</param>
        public SistemaService(ISistemaGateway mainGateway)
            : base(mainGateway)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obter por usuário.
        /// </summary>
        /// <param name="idUsuario">O id de usuário.</param>
        /// <param name="cultureCode">O código da cultura.</param>
        /// <returns>Os sistemas.</returns>
        public IEnumerable<Sistema> ObterPorUsuario(int idUsuario, string cultureCode)
        {
            Assert(new { User = idUsuario, CultureCode = cultureCode }, new AllMustBeInformedSpec());

            return MainGateway.ObterPorUsuario(idUsuario, cultureCode);
        }
        #endregion
    }
}
