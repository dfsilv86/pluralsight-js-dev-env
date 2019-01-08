using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Representa uma FineLine.
    /// </summary>
    public class FineLine : EntityBase
    {
        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public override int Id
        {
            get
            {
                return IDFineLine;
            }

            set
            {
                IDFineLine = value;
            }
        }

        /// <summary>
        /// Obtém ou define IDFineLine.
        /// </summary>
        public int IDFineLine { get; set; }

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
        /// Obtém ou define cdFineLine.
        /// </summary>
        public long cdFineLine { get; set; }

        /// <summary>
        /// Obtém ou define dsFineLine.
        /// </summary>
        public string dsFineLine { get; set; }

        /// <summary>
        /// Obtém ou define blAtivo.
        /// </summary>
        public bool blAtivo { get; set; }

        /// <summary>
        /// Obtém ou define dhCriacao.
        /// </summary>
        public DateTime? dhCriacao { get; set; }

        /// <summary>
        /// Obtém ou define dhAtualizacao.
        /// </summary>
        public DateTime? dhAtualizacao { get; set; }

        /// <summary>
        /// Obtém ou define cdUsuarioCriacao.
        /// </summary>
        public int? cdUsuarioCriacao { get; set; }

        /// <summary>
        /// Obtém ou define cdUsuarioAtualizacao.
        /// </summary>
        public int? cdUsuarioAtualizacao { get; set; }

        /// <summary>
        /// Obtém ou define a subcategoria.
        /// </summary>
        public Subcategoria Subcategoria { get; set; }
    }
}
