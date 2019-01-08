using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Infrastructure.Data.Common
{
    /// <summary>
    /// Leitor de recursos utilizado nos comandos SQL.
    /// </summary>
    public static class SqlResourceReader
    {
        #region Fields
        private static ConcurrentDictionary<string, string> s_cache = new ConcurrentDictionary<string, string>();
        private static Assembly s_assembly = typeof(SqlResourceReader).Assembly;
        #endregion

        #region Methods        
        /// <summary>
        /// Realiza a leitura do comando no recurso informado.
        /// </summary>
        /// <remarks>
        /// O arquivo SQL deve estar na pasta Sql do projeto Walmart.Sgp.Infrastructure.Data e deve sequir o padrão de nomve {resourceName}.{commandName}.sql.
        /// </remarks>
        /// <param name="resourceName">O nome do recurso.</param>
        /// <param name="commandName">O nome do comando.</param>
        /// <returns>O comando.</returns>
        public static string Read(string resourceName, string commandName)
        {
            var key = "Walmart.Sgp.Infrastructure.Data.Sql.{0}.{1}.sql".With(resourceName, commandName);

            if (!s_cache.ContainsKey(key))
            {
                if (!s_assembly.GetManifestResourceNames().Contains(key))
                {
                    throw new InvalidOperationException(Texts.CouldNotFindSqlResource.With(key));
                }

                using (var stream = s_assembly.GetManifestResourceStream(key))
                using (var reader = new StreamReader(stream))
                {
                    s_cache.TryAdd(key, reader.ReadToEnd());
                }
            }

            return s_cache[key];
        }

        /// <summary>
        /// Realiza a leitura do comando no recurso informado.
        /// </summary>
        /// <remarks>
        /// O arquivo SQL deve estar na pasta Sql do projeto Walmart.Sgp.Infrastructure.Data e deve sequir o padrão de nomve {resourceName}.{commandName}.sql.
        /// </remarks>
        /// <param name="command">As informações do comando.</param>
        /// <returns>O comando.</returns>
        public static string Read(CommandInfo command)
        {
            return Read(command.ResourceName, command.Name);
        }
        #endregion
    }
}
