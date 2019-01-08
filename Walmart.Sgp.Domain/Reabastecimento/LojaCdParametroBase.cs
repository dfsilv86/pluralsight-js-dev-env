using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Classe base para entidades que representam  parâmetro de loja/CD.
    /// </summary>
    [DebuggerDisplay("{Loja.cdLoja}/{CD.cdCD}")]
    public abstract class LojaCdParametroBase : EntityBase, IAggregateRoot, IStampContainer
    {
        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public override int Id
        {
            get
            {
                return IDLojaCDParametro;
            }

            set
            {
                IDLojaCDParametro = value;
            }
        }

        /// <summary>
        /// Obtém ou define IDLojaCDParametro.
        /// </summary>
        public int IDLojaCDParametro { get; set; }

        /// <summary>
        /// Obtém ou define IDLoja.
        /// </summary>
        public int IDLoja { get; set; }

        /// <summary>
        /// Obtém ou define a loja.
        /// </summary>
        public Loja Loja { get; set; }

        /// <summary>
        /// Obtém ou define IDCD.
        /// </summary>
        public int IDCD { get; set; }

        /// <summary>
        /// Obtém ou define o CD.
        /// </summary>
        public CD CD { get; set; }

        /// <summary>
        /// Obtém ou define blAtivo.
        /// </summary>
        public bool? blAtivo { get; set; }

        /// <summary>
        /// Obtém ou define cdSistema.
        /// </summary>
        public int cdSistema { get; set; }

        /// <summary>
        /// Obtém ou define dhCriacao.
        /// </summary>
        public DateTime DhCriacao { get; set; }

        /// <summary>
        /// Obtém ou define dhAtualizacao.
        /// </summary>
        public DateTime? DhAtualizacao { get; set; }

        /// <summary>
        /// Obtém ou define cdUsuarioCriacao.
        /// </summary>
        public int? CdUsuarioCriacao { get; set; }

        /// <summary>
        /// Obtém ou define cdUsuarioAtualizacao.
        /// </summary>
        public int? CdUsuarioAtualizacao { get; set; }

        /// <summary>
        /// Obtém ou define vlLeadTime.
        /// </summary>
        public int? vlLeadTime { get; set; }

        /// <summary>
        /// Obtém ou define vlFillRate.
        /// </summary>
        public decimal? vlFillRate { get; set; }

        /// <summary>
        /// Obtém ou define tpPedidoMinimo.
        /// </summary>
        public TipoPedidoMinimo tpPedidoMinimo { get; set; }

        /// <summary>
        /// Obtém ou define vlValorMinimo.
        /// </summary>
        public decimal? vlValorMinimo { get; set; }

        /// <summary>
        /// Obtém ou define tpWeek.
        /// </summary>
        public TipoSemana tpWeek { get; set; }

        /// <summary>
        /// Obtém ou define tpInterval.
        /// </summary>
        public TipoIntervalo tpInterval { get; set; }

        /// <summary>
        /// Obtém ou define tpProduto.
        /// </summary>
        public TipoCD tpProduto { get; set; }
    }

}
