using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Movimentacao
{
    /// <summary>
    /// Define a interface de um table data gateway para tipo movimentação.
    /// </summary>
    public interface ITipoMovimentacaoGateway : IDataGateway<TipoMovimentacao>
    {
    }
}
