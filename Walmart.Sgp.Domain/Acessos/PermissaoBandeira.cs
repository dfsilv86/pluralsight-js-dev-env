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
    /// Representa uma permissaobandeira.
    /// </summary>
    [DebuggerDisplay("{Id} - IDPermissao: {IDPermissao}, IDBandeira: {IDBandeira}")]
    public class PermissaoBandeira : EntityBase
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="PermissaoBandeira"/>
        /// </summary>
        public PermissaoBandeira()
        {
        }     
        #endregion

        #region Properties
        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public override int Id
        {
            get
            {
                return IDPermissaoBandeira;
            }

            set
            {
                IDPermissaoBandeira = value;
            }
        }
       
        /// <summary>
        /// Obtém ou define IDPermissaoBandeira.
        /// </summary>
        public int IDPermissaoBandeira { get; set; }

        /// <summary>
        /// Obtém ou define IDBandeira.
        /// </summary>
        public int IDBandeira { get; set; }

        /// <summary>
        /// Obtém ou define IDPermissao.
        /// </summary>
        public int IDPermissao { get; set; }

        /// <summary>
        /// Obtém ou define Bandeira.
        /// </summary>
        public Bandeira Bandeira { get; set; }   
        #endregion
    }
}
