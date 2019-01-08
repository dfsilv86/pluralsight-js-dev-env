using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Gerenciamento
{
    /// <summary>
    /// Representa e interpreta um código do vendor de 9 dígitos (cdV9D).
    /// <remarks>
    /// O código do vendor deve ter no máximo 9 dígitos (6 para o cdFornecedor + 2 para cdDepartamento + 1 para CdSequencia).
    /// </remarks>
    /// </summary>
    public sealed class CodigoVendor9Digitos
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="CodigoVendor9Digitos"/>
        /// </summary>
        public CodigoVendor9Digitos(long cdV9D)
        {
            var cdV9Formatted = cdV9D.ToString(CultureInfo.InvariantCulture).PadLeft(9, '0');

            if (cdV9Formatted.Length > 9)
            {
                throw new ArgumentOutOfRangeException("cdV9D", Texts.VendorCodeInvalid);
            }

            CdV9D = cdV9D;
            CdFornecedor = Convert.ToInt32(cdV9Formatted.Substring(0, 6), CultureInfo.InvariantCulture);
            CdDepartamento = Convert.ToInt32(cdV9Formatted.Substring(6, 2), CultureInfo.InvariantCulture);
            CdSequencia = Convert.ToInt32(cdV9Formatted.Substring(8, 1), CultureInfo.InvariantCulture);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém o código do vendor de 9 dígitos.
        /// </summary>
        public long CdV9D { get; private set; }

        /// <summary>
        /// Obtém o código do fornecedor.
        /// </summary>
        public int CdFornecedor { get; private set; }

        /// <summary>
        /// Obtém o código do departamento.
        /// </summary>
        public int CdDepartamento { get; private set; }

        /// <summary>
        /// Obtém o código de sequência.
        /// </summary>
        public int CdSequencia { get; private set; }
        #endregion
    }
}
