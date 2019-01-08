using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Acessos
{
    /// <summary>
    /// Representa uma PermissaoLoja.
    /// </summary>
    [DebuggerDisplay("{Id} - IDPermissao: {IDPermissao}, IDLoja: {IDLoja}")]
    public class PermissaoLoja : EntityBase
    {
        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public override int Id
        {
            get
            {
                return IDPermissaoLoja;
            }

            set
            {
                IDPermissaoLoja = value;
            }
        }

        /// <summary>
        /// Obtém ou define IDPermissaoLoja.
        /// </summary>
        public int IDPermissaoLoja { get; set; }

        /// <summary>
        /// Obtém ou define IDPermissao.
        /// </summary>
        public int IDPermissao { get; set; }

        /// <summary>
        /// Obtém ou define IDLoja.
        /// </summary>
        public int IDLoja { get; set; }

        /// <summary>
        /// Obtém ou define Loja.
        /// </summary>
        public Loja Loja { get; set; }
    }
}
