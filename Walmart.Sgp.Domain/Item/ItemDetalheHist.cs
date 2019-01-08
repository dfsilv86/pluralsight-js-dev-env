using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Domain.Item
{
    /// <summary>
    /// Representa uma ItemDetalheHist.
    /// </summary>
    [DebuggerDisplay("Id: {Id}, IDItemDetalhe: {IDItemDetalhe}, TpVinculado: {TpVinculado}, TpReceituario: {TpReceituario}, TpManipulado: {TpManipulado}")]
    public class ItemDetalheHist : EntityBase, IAggregateRoot
    {
        #region Properties
        /// <summary>
        /// Obtém ou define id.
        /// </summary>
        public override int Id
        {
            get
            {
                return base.Id;
            }

            set
            {
                base.Id = value;
            }
        }

        /// <summary>
        /// Obtém ou define IDItemDetalheHist.
        /// </summary>
        public long IDItemDetalheHist { get; set; }

        /// <summary>
        /// Obtém ou define IDItemDetalhe.
        /// </summary>
        public long IDItemDetalhe { get; set; }

        /// <summary>
        /// Obtém ou define o tipo de vinculado.
        /// </summary>
        public TipoVinculado TpVinculado { get; set; }

        /// <summary>
        /// Obtém ou define o tipo de receituário.
        /// </summary>
        public TipoReceituario TpReceituario { get; set; }

        /// <summary>
        /// Obtém ou define tpManipulado.
        /// </summary>
        public TipoManipulado TpManipulado { get; set; }

        /// <summary>
        /// Obtém ou define vlFatorConversao.
        /// </summary>
        public float? VlFatorConversao { get; set; }

        /// <summary>
        /// Obtém ou define tpUnidadeMedida.
        /// </summary>
        public TipoUnidadeMedida TpUnidadeMedida { get; set; }

        /// <summary>
        /// Obtém ou define blItemTransferencia.
        /// </summary>
        public bool? BlItemTransferencia { get; set; }

        /// <summary>
        /// Obtém ou define dhCriacao.
        /// </summary>
        public DateTime DhCriacao { get; set; }

        /// <summary>
        /// Obtém ou define cdUsuarioCriacao.
        /// </summary>
        public int CdUsuarioCriacao { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Cria uma instância de um histórico de item detalhe.
        /// </summary>
        /// <param name="entidade">O item detalhe para o qual será criado o histórico.</param>
        /// <returns>A instância do histórico.</returns>
        public static ItemDetalheHist Create(ItemDetalhe entidade)
        {
            return new ItemDetalheHist
            {
                IDItemDetalhe = entidade.Id,
                TpVinculado = entidade.TpVinculado,
                TpReceituario = entidade.TpReceituario,
                TpManipulado = entidade.TpManipulado,
                VlFatorConversao = entidade.VlFatorConversao,
                TpUnidadeMedida = entidade.TpUnidadeMedida,
                BlItemTransferencia = entidade.BlItemTransferencia,
                CdUsuarioCriacao = RuntimeContext.Current.User.Id,
                DhCriacao = DateTime.Now
            };
        }
        #endregion
    }
}
