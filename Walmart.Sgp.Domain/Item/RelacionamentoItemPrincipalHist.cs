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
    /// Representa um histórico de relacionamento de item principal
    /// </summary>
    [DebuggerDisplay("Id: {Id}, IDItemDetalhe: {IDItemDetalhe}, TpAcao: {TpAcao}")]
    public class RelacionamentoItemPrincipalHist : EntityBase, IAggregateRoot
    {
        #region Properties
        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public override int Id
        {
            get
            {
                return IDRelacionamentoItemPrincipalHist;
            }

            set
            {
                IDRelacionamentoItemPrincipalHist = value;
            }
        }

        /// <summary>
        /// Obtém ou define IDRelacionamentoItemPrincipalHist.
        /// </summary>
        public int IDRelacionamentoItemPrincipalHist { get; set; }

        /// <summary>
        /// Obtém ou define IDRelacionamentoItemPrincipal.
        /// </summary>
        public int IDRelacionamentoItemPrincipal { get; set; }

        /// <summary>
        /// Obtém ou define IDTipoRelacionamento.
        /// </summary>
        public int IDTipoRelacionamento { get; set; }

        /// <summary>
        /// Obtém ou define o tipo relacionamento.
        /// </summary>
        public TipoRelacionamento TipoRelacionamento
        {
            get
            {
                return (TipoRelacionamento)IDTipoRelacionamento;
            }

            set
            {
                IDTipoRelacionamento = value.Value;
            }
        }

        /// <summary>
        /// Obtém ou define IDItemDetalhe.
        /// </summary>
        public long IDItemDetalhe { get; set; }

        /// <summary>
        /// Obtém ou define qtProdutoBruto.
        /// </summary>
        public decimal? QtProdutoBruto { get; set; }

        /// <summary>
        /// Obtém ou define pcRendimentoReceita.
        /// </summary>
        public float? PcRendimentoReceita { get; set; }

        /// <summary>
        /// Obtém ou define qtProdutoAcabado.
        /// </summary>
        public decimal? QtProdutoAcabado { get; set; }

        /// <summary>
        /// Obtém ou define pcQuebra.
        /// </summary>
        public float? PcQuebra { get; set; }

        /// <summary>
        /// Obtém ou define psUnitario.
        /// </summary>
        public decimal? PsUnitario { get; set; }

        /// <summary>
        /// Obtém ou define tpAcao.
        /// </summary>
        public TipoAcao TpAcao { get; set; }

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
        /// Cria uma instância de um histórico de relacionamento de item principal.
        /// </summary>
        /// <param name="principal">O relacionamento de item principal.</param>
        /// <param name="tipoAcao">O tipo da ação registrada no histórico.</param>
        /// <returns>A instância do histórico.</returns>
        public static RelacionamentoItemPrincipalHist Create(RelacionamentoItemPrincipal principal, TipoAcao tipoAcao)
        {
            return new RelacionamentoItemPrincipalHist
            {
                CdUsuarioCriacao = RuntimeContext.Current.User.Id,
                DhCriacao = DateTime.Now,
                IDItemDetalhe = principal.ItemDetalhe.Id,
                IDRelacionamentoItemPrincipal = principal.IDRelacionamentoItemPrincipal,
                QtProdutoBruto = principal.QtProdutoBruto,
                PcRendimentoReceita = principal.PcRendimentoReceita,
                QtProdutoAcabado = principal.QtProdutoAcabado,
                PcQuebra = principal.PcQuebra,
                PsUnitario = principal.PsUnitario,
                TpAcao = tipoAcao,
                TipoRelacionamento = principal.TipoRelacionamento
            };
        }
        #endregion
    }
}
