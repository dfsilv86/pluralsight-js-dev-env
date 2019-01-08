using System;
using System.Data;
using System.Data.SqlClient;

namespace Walmart.Sgp.Infrastructure.Data.Databases
{
    /// <summary>
    /// Informações relacionadas à conexão com a base de dados utilizada durante uma requisição.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class DatabaseContext : IDisposable
    {
        private bool disposedValue = false; // To detect redundant calls
        private SqlConnection m_connection = null;
        private SqlTransaction m_transaction = null;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DatabaseContext"/>.
        /// </summary>
        /// <param name="connectionString">A string de conexão.</param>
        public DatabaseContext(string connectionString)
        {
            this.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings[connectionString].ConnectionString;
        }

        /// <summary>
        /// Finaliza uma instância da classe <see cref="DatabaseContext" />.
        /// </summary>
        ~DatabaseContext()
        {
            Dispose(false);
        }

        /// <summary>
        /// Obtém ou define a string de conexão.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Obtém a conexão com a base de dados (criada caso ainda não exista).
        /// </summary>
        public SqlConnection Connection
        {
            get
            {
                if (null == m_connection)
                {
                    m_connection = new SqlConnection(this.ConnectionString);
                }

                return m_connection;
            }
        }

        /// <summary>
        /// Obtém a transação usada pela conexão nesta requisição (criada caso não exista).
        /// </summary>
        public SqlTransaction Transaction
        {
            get
            {
                if (null == m_transaction)
                {
                    var connection = this.Connection;

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    m_transaction = this.Connection.BeginTransaction();
                }

                return m_transaction;
            }
        }

        #region IDisposable Support

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (null != m_transaction)
                    {
                        m_transaction.Dispose();
                        m_transaction = null;
                    }

                    if (null != m_connection)
                    {
                        if (m_connection.State != ConnectionState.Closed)
                        {
                            m_connection.Close();
                        }

                        m_connection.Dispose();
                        m_connection = null;
                    }
                }

                disposedValue = true;
            }
        }
        #endregion
    }
}
