using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Data.Common;

namespace Walmart.Sgp.Infrastructure.Data
{
    /// <summary>
    /// Informações sobre um comando SQL.
    /// </summary>
    public class CommandInfo
    {
        #region Constructors        
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="CommandInfo"/>.
        /// </summary>
        /// <param name="name">O nome do comando.</param>
        /// <param name="resourceName">O nome do recurso onde está agrupado o comando.</param>
        /// <param name="fileName">O nome do arquivo onde está o conteúdo do comando.</param>
        public CommandInfo(string name, string resourceName, string fileName)
        {
            Name = name;
            ResourceName = resourceName;
            FileName = fileName;
        }
        #endregion

        #region Properties        
        /// <summary>
        /// Obtém o nome do comando.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Obtém o nome do recurso onde está agrupado o comando.
        /// </summary>
        public string ResourceName { get; private set; }

        /// <summary>
        /// Obtém o nome do arquivo onde está o conteúdo do comando.
        /// </summary> 
        public string FileName { get; private set; }
        #endregion

        #region Operators        
        /// <summary>
        /// Performs an implicit conversion from <see cref="CommandInfo"/> to <see cref="String"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator String(CommandInfo value)
        {
            return value.Name;
        }
        #endregion

        #region Methods        
        /// <summary>
        /// Realiza a leitura o conteúdo do comando.
        /// </summary>
        /// <returns>O conteúdo do comando.</returns>
        public string Read()
        {
            return SqlResourceReader.Read(this);
        }
        #endregion       
    }
}
