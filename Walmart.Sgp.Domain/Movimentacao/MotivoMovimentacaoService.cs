using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Movimentacao
{
    /// <summary>
    /// Serviço de domínio para motivo de movimentação.
    /// </summary>
    public class MotivoMovimentacaoService : EntityDomainServiceBase<MotivoMovimentacao, IMotivoMovimentacaoGateway>, IMotivoMovimentacaoService
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="MotivoMovimentacaoService"/>.
        /// </summary>
        /// <param name="mainGateway">O table data gateway para motivo de movimentação.</param>
        public MotivoMovimentacaoService(IMotivoMovimentacaoGateway mainGateway)
            : base(mainGateway)
        {
        }

        #endregion

        #region Methods
        /// <summary>
        /// Obtém os motivos de movimentação que são visíveis ao usuário.
        /// </summary>
        /// <returns>Os motivos de movimentação.</returns>
        public IEnumerable<MotivoMovimentacao> ObterVisiveis()
        {
            return MainGateway.Find("blExibir = @blExibir AND  blAtivo = @blAtivo", new { blExibir = true, blAtivo = true });
        }
        #endregion
    }
}
