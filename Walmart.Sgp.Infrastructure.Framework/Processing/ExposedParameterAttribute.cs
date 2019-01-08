using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.Framework.Processing
{
    /// <summary>
    /// Atributo para sinalizar que um argumento ou propriedade pode ser visualizado ao obter informações sobre a execução de um processo.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class ExposedParameterAttribute : Attribute
    {
        #region Fields
        /// <summary>
        /// O valor padrão caso este atributo não seja especificado.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly ExposedParameterAttribute Default = new ExposedParameterAttribute(false);
        #endregion

        #region Constructor

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ExposedParameterAttribute"/>.
        /// </summary>
        public ExposedParameterAttribute()
            : this(true)
        {
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ExposedParameterAttribute"/>.
        /// </summary>
        /// <param name="isExposed">True se o argumento ou propriedade pode ser visualizado. Falso caso contrário.</param>
        public ExposedParameterAttribute(bool isExposed)
        {
            this.IsExposed = isExposed;
        }
        #endregion

        /// <summary>
        /// Obtém um valor que indica se o valor do argumento ou propriedade marcado com este atributo pode ser visualizado.
        /// </summary>
        public bool IsExposed { get; private set; }
    }
}
