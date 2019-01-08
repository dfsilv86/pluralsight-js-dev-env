using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Movimentacao
{
    /// <summary>
    /// Representa uma MotivoMovimentacao.
    /// </summary>
    public class MotivoMovimentacao : EntityBase, IAggregateRoot
    {
        #region Constants
        /// <summary>
        /// O id do registro de erro de quebra PDV.
        /// </summary>
        //// TODO: alterar a tabela de MotivoMovimentacao para que seja possível descobrir essa informação pelo registro.
        public const int IDErroDeQuebraPDV = 14;

        /// <summary>
        /// O id do registro de entrada acerto.
        /// </summary>
        //// TODO: alterar a tabela de MotivoMovimentacao para que seja possível descobrir essa informação pelo registro.
        public const int IDEntradaAcerto = 15;

        /// <summary>
        /// O id do registro de saída acerto.
        /// </summary>
        //// TODO: alterar a tabela de MotivoMovimentacao para que seja possível descobrir essa informação pelo registro.
        public const int IDSaidaAcerto = 16;
        #endregion

        #region Properties
        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public override int Id
        {
            get
            {
                return IDMotivo;
            }

            set
            {
                IDMotivo = value;
            }
        }

        /// <summary>
        /// Obtém ou define IDMotivo.
        /// </summary>
        public int IDMotivo { get; set; }

        /// <summary>
        /// Obtém ou define dsMotivo.
        /// </summary>
        public string dsMotivo { get; set; }

        /// <summary>
        /// Obtém ou define blAtivo.
        /// </summary>
        public bool? blAtivo { get; set; }

        /// <summary>
        /// Obtém ou define blExibir.
        /// </summary>
        public bool? blExibir { get; set; }
        #endregion
    }

}
