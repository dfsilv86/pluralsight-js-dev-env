using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Gerenciamento
{
    /// <summary>
    /// Serviço de domínio relacionado a formato.
    /// </summary>
    public class FormatoService : EntityDomainServiceBase<Formato, IFormatoGateway>, IFormatoService
    {
        #region Constructors        
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="FormatoService"/>.
        /// </summary>
        /// <param name="mainGateway">O table data gateway para formato.</param>
        public FormatoService(IFormatoGateway mainGateway) 
            : base(mainGateway)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obtém a lista de formatos associados ao sistema informado.
        /// </summary>
        /// <param name="cdSistema">O código de sistema.</param>
        /// <returns>Os formatos</returns>
        public IEnumerable<Formato> ObterPorSistema(int? cdSistema)
        {
            return this.MainGateway.Find("cdSistema = @cdSistema", new { cdSistema });
        }
        #endregion
    }
}
