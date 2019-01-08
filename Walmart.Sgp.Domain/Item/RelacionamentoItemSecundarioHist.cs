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
    /// Representa uma RelacionamentoItemSecundarioHist.
    /// </summary>
    [DebuggerDisplay("Id: {Id}, IDItemDetalhe: {IDItemDetalhe}, IDRelacionamentoItemPrincipal: {IDRelacionamentoItemPrincipal}, TpAcao: {TpAcao}")]
    public class RelacionamentoItemSecundarioHist : EntityBase, IAggregateRoot
    {
        #region Properties
        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public override int Id
        {
            get
            {
                return IDRelacionamentoItemSecundarioHist;
            }

            set
            {
                IDRelacionamentoItemSecundarioHist = value;
            }
        }

        /// <summary>
        /// Obtém ou define IDRelacionamentoItemSecundarioHist.
        /// </summary>
        public int IDRelacionamentoItemSecundarioHist { get; set; }

        /// <summary>
        /// Obtém ou define IDRelacionamentoItemPrincipalHist.
        /// </summary>
        public int IDRelacionamentoItemPrincipalHist { get; set; }

        /// <summary>
        /// Obtém ou define IDRelacionamentoItemSecundario.
        /// </summary>
        public int IDRelacionamentoItemSecundario { get; set; }

        /// <summary>
        /// Obtém ou define IDRelacionamentoItemPrincipal.
        /// </summary>
        public int IDRelacionamentoItemPrincipal { get; set; }

        /// <summary>
        /// Obtém ou define psItem.
        /// </summary>
        public decimal psItem { get; set; }

        /// <summary>
        /// Obtém ou define tpItem.
        /// </summary>
        public int tpItem { get; set; }

        /// <summary>
        /// Obtém ou define pcRendimentoDerivado.
        /// </summary>
        public float? pcRendimentoDerivado { get; set; }

        /// <summary>
        /// Obtém ou define IDItemDetalhe.
        /// </summary>
        public long? IDItemDetalhe { get; set; }

        /// <summary>
        /// Obtém ou define qtItemUn.
        /// </summary>
        public decimal? qtItemUn { get; set; }

        /// <summary>
        /// Obtém ou define tpAcao.
        /// </summary>
        public TipoAcao TpAcao { get; set; }

        /// <summary>
        /// Obtém ou define dhCriacao.
        /// </summary>
        public DateTime dhCriacao { get; set; }

        /// <summary>
        /// Obtém ou define cdUsuarioCriacao.
        /// </summary>
        public int cdUsuarioCriacao { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Cria uma instância de um histórico de relacionamento de item secundário.
        /// </summary>
        /// <param name="principalHist">O histórico de relacionamento de item principal.</param>
        /// <param name="secundario">O relacionamento de item secundário.</param>
        /// <param name="tipoAcao">O tipo da ação registrada no histórico.</param>
        /// <returns>A instÂncia do histórico.</returns>
        public static RelacionamentoItemSecundarioHist Create(RelacionamentoItemPrincipalHist principalHist, RelacionamentoItemSecundario secundario, TipoAcao tipoAcao)
        {
            return new RelacionamentoItemSecundarioHist
            {
                cdUsuarioCriacao = RuntimeContext.Current.User.Id,
                dhCriacao = DateTime.Now,
                IDItemDetalhe = secundario.IDItemDetalhe,
                IDRelacionamentoItemPrincipal = secundario.IDRelacionamentoItemPrincipal,
                IDRelacionamentoItemPrincipalHist = principalHist.Id,
                IDRelacionamentoItemSecundario = secundario.Id,
                pcRendimentoDerivado = secundario.pcRendimentoDerivado,
                psItem = secundario.psItem,
                qtItemUn = secundario.qtItemUn,
                TpAcao = tipoAcao,
                tpItem = secundario.TpItem.Value
            };
        }
        #endregion
    }
}
