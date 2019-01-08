using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Item
{
    /// <summary>
    /// Define a interface de um table data gateway para item detalhe.
    /// </summary>
    public interface IItemDetalheHistGateway : IDataGateway<ItemDetalheHist>
    {
    }
}
