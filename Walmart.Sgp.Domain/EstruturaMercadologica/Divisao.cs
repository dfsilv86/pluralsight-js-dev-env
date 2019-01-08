using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Representa uma Divisao.
    /// </summary>
    public class Divisao : EntityBase
    {
        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public override int Id
        {
            get
            {
                return IDDivisao;
            }

            set
            {
                IDDivisao = value;
            }
        }

        /// <summary>
        /// Obtém ou define IDDivisao.
        /// </summary>
        public int IDDivisao { get; set; }

        /// <summary>
        /// Obtém ou define cdSistema.
        /// </summary>
        public short cdSistema { get; set; }

        /// <summary>
        /// Obtém ou define cdDivisao.
        /// </summary>
        public int cdDivisao { get; set; }

        /// <summary>
        /// Obtém ou define dsDivisao.
        /// </summary>
        public string dsDivisao { get; set; }

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
    }
}
