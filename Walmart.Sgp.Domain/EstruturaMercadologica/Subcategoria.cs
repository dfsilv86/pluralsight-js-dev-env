using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Representa uma Subcategoria.
    /// </summary>
    public class Subcategoria : EntityBase
    {
        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public override int Id
        {
            get
            {
                return IDSubcategoria;
            }

            set
            {
                IDSubcategoria = value;
            }
        }

        /// <summary>
        /// Obtém ou define IDSubcategoria.
        /// </summary>
        public int IDSubcategoria { get; set; }

        /// <summary>
        /// Obtém ou define IDCategoria.
        /// </summary>
        public int IDCategoria { get; set; }

        /// <summary>
        /// Obtém ou define IDDepartamento.
        /// </summary>
        public int IDDepartamento { get; set; }

        /// <summary>
        /// Obtém ou define cdSistema.
        /// </summary>
        public byte cdSistema { get; set; }

        /// <summary>
        /// Obtém ou define cdSubcategoria.
        /// </summary>
        public long cdSubcategoria { get; set; }

        /// <summary>
        /// Obtém ou define dsSubcategoria.
        /// </summary>
        public string dsSubcategoria { get; set; }

        /// <summary>
        /// Obtém ou define blAtivo.
        /// </summary>
        public bool blAtivo { get; set; }

        /// <summary>
        /// Obtém ou define dhCriacao.
        /// </summary>
        public DateTime? dhCriacao { get; set; }

        /// <summary>
        /// Obtém ou define dhAlteracao.
        /// </summary>
        public DateTime? dhAlteracao { get; set; }

        /// <summary>
        /// Obtém ou define cdUsuarioCriacao.
        /// </summary>
        public int? cdUsuarioCriacao { get; set; }

        /// <summary>
        /// Obtém ou define cdUsuarioAlteracao.
        /// </summary>
        public int? cdUsuarioAlteracao { get; set; }

        /// <summary>
        /// Obtém ou define a categoria.
        /// </summary>
        public Categoria Categoria { get; set; }
    }
}
