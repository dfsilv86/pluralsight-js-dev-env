using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Framework.Processing
{
    /// <summary>
    /// Define a interface de um table data gateway para ProcessOrderService.
    /// </summary>
    public interface IProcessOrderServiceGateway : IDataGateway<ProcessOrderService>
    {
    }
}
