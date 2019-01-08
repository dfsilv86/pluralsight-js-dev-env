using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Movimentacao;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.Memory
{
    /// <summary>
    /// Implementação de um table data gateway para MotivoMovimentacao em memória.
    /// </summary>
    /// <remarks>
    /// Essa implementação deve ser utilizada apenas para fins de testes unitários.
    /// </remarks>
    public class MemoryMotivoMovimentacaoGateway : MemoryDataGateway<MotivoMovimentacao>, IMotivoMovimentacaoGateway
    {
    }
}