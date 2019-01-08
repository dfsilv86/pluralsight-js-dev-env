using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.Framework.Data
{
    /// <summary>
    /// Registro de table names de entidades.
    /// </summary>
    /// <remarks>
    /// Usado para determinar o nome da tabela de uma entidade conforme o registro dos gateways de entidade. Provavelmente pode ser substituído por um método que encontre essas informações dentro das estruturas do Dapper.
    /// </remarks>
    public static class EntityTableModelRegistry
    {
        private static Dictionary<string, Tuple<string, string>> s_registry = new Dictionary<string, Tuple<string, string>>();

        /// <summary>
        /// Registra o nome da tabela de uma entidade.
        /// </summary>
        /// <param name="entityType">O tipo da entidade.</param>
        /// <param name="tableName">O nome da tabela.</param>
        /// <param name="idColumnName">O nome da coluna de chave primária da tabela.</param>
        public static void RegisterEntityTableModel(Type entityType, string tableName, string idColumnName)
        {
            lock (s_registry)
            {
                s_registry[entityType.FullName] = new Tuple<string, string>(tableName, idColumnName);
            }
        }

        /// <summary>
        /// Obtém o nome da tabela de uma entidade.
        /// </summary>
        /// <param name="entityType">O tipo da entidade.</param>
        /// <returns>Tupla com o nome da tabela e da coluna de id.</returns>
        public static Tuple<string, string> GetTableModelForEntity(Type entityType)
        {
            // TODO: tentar buscar isso dentro das estruturas do Dapper.
            lock (s_registry)
            {
                return s_registry[entityType.FullName];
            }
        }

        /// <summary>
        /// Obtém o nome da tabela de uma entidade.
        /// </summary>
        /// <param name="entityFullName">O nome do tipo da entidade.</param>
        /// <returns>Tupla com o nome da tabela e da coluna de id.</returns>
        public static Tuple<string, string> GetTableModelForEntity(string entityFullName)
        {
            // TODO: tentar buscar isso dentro das estruturas do Dapper.
            lock (s_registry)
            {
                return s_registry[entityFullName];
            }
        }
    }
}
