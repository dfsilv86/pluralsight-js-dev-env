using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.MultisourcingVendor
{
    /// <summary>
    /// Interface de Serviço de log de Multisourcing.
    /// </summary>
    public interface ILogMultiSourcingService
    {
        /// <summary>
        /// Logar evento de multisourcing
        /// </summary>
        /// <param name="acao">A ação (Inserir/Excluir/Alterar).</param>
        /// <param name="msAnterior">O multisourcing original.</param>
        /// <param name="msPosterior">O multisourcing alterado.</param>
        /// <param name="observacao">Uma observação.</param>
        void Logar(TpOperacao acao, Multisourcing msAnterior, Multisourcing msPosterior, string observacao);
    }
}
