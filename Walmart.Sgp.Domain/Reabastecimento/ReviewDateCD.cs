using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Representa uma data de revisão do CD.
    /// </summary>
    [DebuggerDisplay("{cdReviewDate}")]
    public class ReviewDateCD : EntityBase
    {
        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public override int Id
        {
            get
            {
                return IDReviewDateCD;
            }

            set
            {
                IDReviewDateCD = value;
            }
        }

        /// <summary>
        /// Obtém ou define IDReviewDateCD.
        /// </summary>
        public int IDReviewDateCD { get; set; }

        /// <summary>
        /// Obtém ou define IDLojaCDParametro.
        /// </summary>
        public int IDLojaCDParametro { get; set; }

        /// <summary>
        /// Obtém ou define IDDepartamento.
        /// </summary>
        public int IDDepartamento { get; set; }

        /// <summary>
        /// Obtém ou define o departamento.
        /// </summary>
        public Departamento Departamento { get; set; }

        /// <summary>
        /// Obtém ou define cdSistema.
        /// </summary>
        public int cdSistema { get; set; }

        /// <summary>
        /// Obtém ou define dhCriacao.
        /// </summary>
        public DateTime dhCriacao { get; set; }

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
        /// Obtém ou define blAtivo.
        /// </summary>
        public bool blAtivo { get; set; }

        /// <summary>
        /// Obtém ou define cdReviewDate.
        /// </summary>
        public int? cdReviewDate { get; set; }

        /// <summary>
        /// Obtém ou define tpReabastecimento.
        /// </summary>
        public TipoReabastecimento tpReabastecimento { get; set; }

    }

}
