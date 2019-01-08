using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Define a interface de uma query ao Dapper.
    /// </summary>
    public interface IDapperQuery
    {
        /// <summary>
        /// Obtém o comando SQL.
        /// </summary>
        string Sql { get; }

        /// <summary>
        /// Obtém os argumentos do comando SQL.
        /// </summary>
        object Args { get; }
    }
}
