using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Movimentacao
{
    /// <summary>
    /// Define a interface para um serviço de domínio para motivo de movimentação.
    /// </summary>
    public interface IMotivoMovimentacaoService : IDomainService<MotivoMovimentacao>
    {
        /// <summary>
        /// Obtém os motivos de movimentação que são visíveis ao usuário.
        /// </summary>
        /// <returns>Os motivos de movimentação.</returns>
        IEnumerable<MotivoMovimentacao> ObterVisiveis();
    }
}
