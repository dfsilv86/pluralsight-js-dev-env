using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Acessos.Specs
{
    /// <summary>
    /// Representa um parâmetro para a especificação.
    /// </summary>
    public class UsuarioDevePossuirPermissaoNaLojaSpecParameter
    {
        /// <summary>
        /// Obtém ou define Usuario.
        /// </summary>
        public IRuntimeUser Usuario { get; set; }

        /// <summary>
        /// Obtém ou define IdLoja.
        /// </summary>
        public int IdLoja { get; set; }
    }
}
