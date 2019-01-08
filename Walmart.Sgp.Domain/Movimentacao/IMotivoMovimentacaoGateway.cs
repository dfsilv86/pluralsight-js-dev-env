using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Movimentacao
{
    /// <summary>
    /// Define a interface de um data table gateway para movito de movimentação.
    /// </summary>
    public interface IMotivoMovimentacaoGateway : IDataGateway<MotivoMovimentacao>
    {
    }
}
