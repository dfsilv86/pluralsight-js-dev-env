using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.Data.Databases
{
    /// <summary>
    /// Representa as bases de dados utilizadas pela aplicação.
    /// </summary>
    public enum Database
    {
        /// <summary>
        /// A base de dados principal da aplicação SGP.
        /// </summary>
        Wlmslp = 0
    }

    /// <summary>
    /// Contém informações sobre as bases de dados utilizadas pela aplicação dentro de uma requisição.
    /// </summary>
    public class ApplicationDatabases : IDisposable
    {
        private bool disposedValue = false; // To detect redundant calls
        private Dictionary<Database, DatabaseContext> m_databaseContexts;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ApplicationDatabases"/>
        /// </summary>
        public ApplicationDatabases()
        {
            m_databaseContexts = new Dictionary<Database, DatabaseContext>();
        }

        /// <summary>
        /// Finaliza uma instância da classe <see cref="ApplicationDatabases" />.
        /// </summary>
        ~ApplicationDatabases()
        {
            Dispose(false);
        }

        /// <summary>
        /// Obtém o <see cref="DatabaseContext"/> para a base de dados do SGP.
        /// </summary>
        /// <value>
        /// O <see cref="DatabaseContext"/> da base SGP.
        /// </value>
        public DatabaseContext Wlmslp
        {
            get
            {
                return this[Database.Wlmslp];
            }
        }

        /// <summary>
        /// Obtém o <see cref="DatabaseContext"/> para a base de dados especificada.
        /// </summary>
        /// <param name="database">A base de dados.</param>
        /// <returns>O <see cref="DatabaseContext"/>.</returns>
        public DatabaseContext this[Database database]
        {
            get
            {
                if (!m_databaseContexts.ContainsKey(database))
                {
                    m_databaseContexts[database] = new DatabaseContext(database.ToString());
                }

                return m_databaseContexts[database];
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
                    foreach (DatabaseContext context in m_databaseContexts.Values)
                    {
                        context.Dispose();
                    }
                }

                disposedValue = true;
            }
        }        
        #endregion
    }
}
