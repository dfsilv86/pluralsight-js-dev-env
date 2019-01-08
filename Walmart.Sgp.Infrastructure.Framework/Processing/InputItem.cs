using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.Framework.Processing
{
    /// <summary>
    /// Representa um argumento de um processamento durante a fase de deserialização, validação e preparação dos parâmetros.
    /// </summary>
    internal class SerializedArgumentInformation
    {
        /// <summary>
        /// Obtém ou define o nome do argumento.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Obtém ou define o valor serializado.
        /// </summary>
        public string SerializedValue { get; set; }

        /// <summary>
        /// Obtém ou define o nome padronizado.
        /// </summary>
        public string StandardizedName { get; set; }

        /// <summary>
        /// Obtém ou define as partes do nome.
        /// </summary>
        public IReadOnlyList<string> Tokens { get; set; }

        /// <summary>
        /// Obtém ou define o nível dentro da árvore.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Obtém ou define o tipo esperado do valor deserializado.
        /// </summary>
        public Type ParameterType { get; set; }

        /// <summary>
        /// Obtém ou define um valor que indica se o tipo esperado pode ser deserializado diretamente.
        /// </summary>
        public bool? IsSupportedDeserializableType { get; set; }

        /// <summary>
        /// Obtém ou define o valor deserializado.
        /// </summary>
        public object ParameterValue { get; set; }

        /// <summary>
        /// Obtém ou define a propriedade na instância de objeto pai onde este valor será setado.
        /// </summary>
        public PropertyInfo ParentPropertyInfo { get; set; }
    }
}
