using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Dapper;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.Movimentacao;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Classe base para table data gateways utilizando o Dapper.
    /// </summary>
    /// <typeparam name="T">O tipo da classe associada ao data gatway.</typeparam>
    public abstract class DapperDataGatewayBase<T>
    {
        #region Constructors     
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperDataGatewayBase{T}"/>.
        /// </summary>
        /// <param name="dbContext">O contexto do banco de dados que será utilizado.</param>
        protected DapperDataGatewayBase(DatabaseContext dbContext)            
        {
            Command = new DapperProxy(dbContext, CommandType.Text);
            StoredProcedure = new DapperProxy(dbContext, CommandType.StoredProcedure);
            Resource = new ResourceDapperProxy<T>(dbContext);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém os comandos SQL.
        /// </summary>
        protected DapperProxy Command { get; private set; }

        /// <summary>
        /// Obtém os comandos de stored procedure.
        /// </summary>
        protected DapperProxy StoredProcedure { get; private set; }

        /// <summary>
        /// Obtém os comandos de SQL de arquivos de recursos.
        /// </summary>
        protected ResourceDapperProxy<T> Resource { get; private set; }
        #endregion  
    }
}
