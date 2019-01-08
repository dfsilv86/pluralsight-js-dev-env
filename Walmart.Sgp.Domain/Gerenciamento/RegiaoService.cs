using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Gerenciamento
{
    /// <summary>
    /// Serviço de domínio relacionado a regiao.
    /// </summary>
    public class RegiaoService : EntityDomainServiceBase<Regiao, IRegiaoGateway>, IRegiaoService
    {
        #region Constructor        
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="RegiaoService"/>
        /// </summary>
        /// <param name="mainGateway">O table data gateway principal.</param>
        public RegiaoService(IRegiaoGateway mainGateway)
            : base(mainGateway)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obtém as regiões cadastradas para uma bandeira.
        /// </summary>
        /// <param name="idBandeira">O id de bandeira.</param>
        /// <returns>
        /// As regiões
        /// </returns>
        public IEnumerable<Regiao> ObterPorBandeira(int idBandeira)
        {
            return this.MainGateway.Find("IDBandeira=@IDBandeira", new { idBandeira });
        }
        #endregion
    }
}
