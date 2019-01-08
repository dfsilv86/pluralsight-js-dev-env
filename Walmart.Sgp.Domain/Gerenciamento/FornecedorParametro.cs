using System;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Gerenciamento
{
    /// <summary>
    /// Representa uma FornecedorParametro.
    /// </summary>
    public class FornecedorParametro : EntityBase, IAggregateRoot
    {
        private TipoSAR m_tpStoreApprovalRequired;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="FornecedorParametro" />
        /// </summary>
        public FornecedorParametro()
        {
            m_tpStoreApprovalRequired = TipoSAR.NaoDefinido;
        }

        /// <summary>
        /// Obtém ou define o id de parâmetro de fornecedor.
        /// Inicia uma nova instância da classe <see cref="FornecedorParametro"/>.
        /// </summary>
        public override int Id
        {
            get
            {
                return (int)this.IDFornecedorParametro;
            }

            set
            {
                this.IDFornecedorParametro = value;
            }
        }

        /// <summary>
        /// Obtém ou define IDFornecedorParametro.
        /// </summary>
        public long IDFornecedorParametro { get; set; }

        /// <summary>
        /// Obtém ou define IDFornecedor.
        /// </summary>
        public long IDFornecedor { get; set; }

        /// <summary>
        /// Obtém ou define cdV9D.
        /// </summary>
        /// <remarks>
        /// O código é formado por:
        /// Código do fornecedor (6 dígitos) + Código do departamento (2 dígitos) + sequencial do vendor (1 dígitos).
        /// Se precisar extrair esses dados do código utilize a classe CodigoVendor9Digitos.
        /// </remarks>
        public long cdV9D { get; set; }

        /// <summary>
        /// Obtém ou define o cdTipo.
        /// </summary>
        public TipoCodigoReabastecimento cdTipo { get; set; }

        /// <summary>
        /// Obtém ou define o tipo de pedido mínimo.
        /// </summary>
        /// <remarks>
        /// Valores identificados em banco: "$", "W", "C", "O", "V" e string.Empty. 
        /// Foram importados apenas o "$" e o "C" pois os demais valores estão pendentes de revisão com o Analista.
        /// </remarks>
        public TipoPedidoMinimo tpPedidoMinimo { get; set; }        

        /// <summary>
        /// Obtém ou define o cdStatusVendor.
        /// </summary>
        public TipoStatusFornecedorReabastecimento cdStatusVendor { get; set; }

        /// <summary>
        /// Obtém ou define o tipo da semana.
        /// </summary>
        public TipoSemana tpWeek { get; set; }

        /// <summary>
        /// Obtém ou define o tipo do intervalo.
        /// </summary>
        public TipoIntervalo tpInterval { get; set; }

        /// <summary>
        /// Obtém ou define o Lead Time.
        /// </summary>
        public short vlLeadTime { get; set; }

        /// <summary>
        /// Obtém ou define o vlFillRate.
        /// </summary>
        public decimal vlFillRate { get; set; }

        /// <summary>
        /// Obtém ou define o cdReviewDate.
        /// </summary>
        public int? cdReviewDate { get; set; }

        /// <summary>
        /// Obtém ou define o fornecedor.
        /// </summary>
        public Fornecedor Fornecedor { get; set; }

        /// <summary>
        /// Obtém ou define cdSistema.
        /// </summary>
        public byte cdSistema { get; set; }

        /// <summary>
        /// Obtém ou define nmFornecedor.
        /// </summary>
        public string nmFornecedor { get; set; }

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
        /// Obtém ou define vlValorMinimo.
        /// </summary>
        public decimal? vlValorMinimo { get; set; }

        /// <summary>
        /// Obtém ou define tpStoreApprovalRequired.
        /// </summary>
        public TipoSAR tpStoreApprovalRequired 
        {
            get
            {
                return m_tpStoreApprovalRequired;
            }

            set
            {
                m_tpStoreApprovalRequired = value ?? TipoSAR.NaoDefinido;
            }
        }

        #region Propriedades Calculadas
        /// <summary>
        /// Obtém ou define cdDepartamento.
        /// </summary>
        public int cdDepartamento { get; set; }

        /// <summary>
        /// Obtém ou define dsDepartamento.
        /// </summary>
        public string dsDepartamento { get; set; }
        #endregion
    }
}
