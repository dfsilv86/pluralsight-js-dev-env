using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Movimentacao;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Movimentacao
{
    /// <summary>
    /// Define a interface para serviço de cadastro de item de nota fiscal.
    /// </summary>
    public interface INotaFiscalItemService : IDomainService<NotaFiscalItem>
    {
    }
}
