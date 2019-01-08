using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Representa uma Categoria.
    /// </summary>
    [DebuggerDisplay("{cdCategoria}: {dsCategoria}")]
    public class Categoria : EntityBase, IAggregateRoot, ILojaSecao
    {
        /// <summary>
        /// Obtém ou define id.
        /// </summary>
        public override int Id
        {
            get
            {
                return IDCategoria;
            }

            set
            {
                IDCategoria = value;
            }
        }

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
        public int cdSistema { get; set; }

        /// <summary>
        /// Obtém ou define cdCategoria.
        /// </summary>
        public int cdCategoria { get; set; }

        /// <summary>
        /// Obtém ou define dsCategoria.
        /// </summary>
        public string dsCategoria { get; set; }

        /// <summary>
        /// Obtém ou define blPerecivel.
        /// </summary>
        public string blPerecivel { get; set; }

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
        /// Obtém ou define o departamento.
        /// </summary>
        public Departamento Departamento { get; set; }

        /// <summary>
        /// Obtém o código.
        /// </summary>
        public int Codigo
        {
            get { return cdCategoria; }
        }

        /// <summary>
        /// Obtém a descrição.
        /// </summary>
        public string Descricao
        {
            get { return dsCategoria; }
        }
    }
}
