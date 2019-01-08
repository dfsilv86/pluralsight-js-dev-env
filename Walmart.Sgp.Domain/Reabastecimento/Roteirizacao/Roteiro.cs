using System;
using System.Collections.Generic;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Representa uma Roteiro.
    /// </summary>
    public class Roteiro : EntityBase, IAggregateRoot, IStampContainer
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="Roteiro"/>.
        /// </summary>
        public Roteiro()
        {
            Lojas = new List<RoteiroLoja>();
        }

        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public override int Id
        {
            get
            {
                return IDRoteiro;
            }

            set
            {
                IDRoteiro = value;
            }
        }

        /// <summary>
        /// Obtém ou define idRoteiro.
        /// </summary>
        public int IDRoteiro { get; set; }

        /// <summary>
        /// Obtém ou define Descricao.
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Obtém ou define vlCargaMinima.
        /// </summary>
        public int vlCargaMinima { get; set; }

        /// <summary>
        /// Obtém ou define blKgCx.
        /// </summary>
        public bool blKgCx { get; set; }

        /// <summary>
        /// Obtém ou define idUsuarioCriacao.
        /// </summary>
        public int idUsuarioCriacao { get; set; }

        /// <summary>
        /// Obtém ou define idUsuarioAtualizacao.
        /// </summary>
        public int? idUsuarioAtualizacao { get; set; }

        /// <summary>
        /// Obtém ou define blAtivo.
        /// </summary>
        public bool blAtivo { get; set; }

        /// <summary>
        /// Obtém ou define cdV9D.
        /// </summary>
        public long cdV9D { get; set; }

        #region Propriedades Calculadas
        /// <summary>
        /// Obtém ou define Fornecedor.
        /// </summary>
        public Fornecedor Fornecedor { get; set; }

        /// <summary>
        /// Obtém ou define as Lojas.
        /// </summary>
        public IList<RoteiroLoja> Lojas { get; set; }
        #endregion

        #region Stamp Properties
        /// <summary>
        /// Obtém ou define DhCriacao.
        /// </summary>
        public DateTime DhCriacao { get; set; }

        /// <summary>
        /// Obtém ou define DhAtualizacao.
        /// </summary>
        public DateTime? DhAtualizacao { get; set; }

        /// <summary>
        /// Obtém ou define CdUsuarioCriacao.
        /// </summary>
        public int? CdUsuarioCriacao
        {
            get
            {
                return idUsuarioCriacao;
            }

            set
            {
                idUsuarioCriacao = value ?? 0;
            }
        }

        /// <summary>
        /// Obtém ou define CdUsuarioAtualizacao.
        /// </summary>
        public int? CdUsuarioAtualizacao
        {
            get
            {
                return idUsuarioAtualizacao;
            }

            set
            {
                idUsuarioAtualizacao = value;
            }
        }
        #endregion
    }
}
