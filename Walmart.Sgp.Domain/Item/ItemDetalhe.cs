using System;
using System.Linq;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.MultisourcingVendor;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Item
{
    /// <summary>
    /// Tipo de item.
    /// </summary>
    public enum TipoItem
    {
        /// <summary>
        /// Peso fixo.
        /// </summary>
        PesoFixo,

        /// <summary>
        /// Peso variável.
        /// </summary>
        PesoVariavel,
    }

    /// <summary>
    /// Representa uma ItemDetalhe.
    /// </summary>
    public class ItemDetalhe : EntityBase, IAggregateRoot
    {
        #region Fields
        private TipoVinculado m_tpVinculado;
        private TipoManipulado m_tpManipulado;
        private TipoReceituario m_tpReceituario;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ItemDetalhe"/>
        /// </summary>
        public ItemDetalhe()
        {
            TpStatus = TipoStatusItem.Ativo;
            TpVinculado = TipoVinculado.NaoDefinido;
            TpManipulado = TipoManipulado.NaoDefinido;
            TpReceituario = TipoReceituario.NaoDefinido;
            TpCaixaFornecedor = TipoCaixaFornecedor.NaoDefinido;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public override int Id
        {
            get
            {
                return IDItemDetalhe;
            }
            
            set
            {
                IDItemDetalhe = value;
            }
        }

        /// <summary>
        /// Obtém ou define IDItemDetalhe.
        /// </summary>
        public int IDItemDetalhe { get; set; }

        /// <summary>
        /// Obtém ou define IdFornecedorParametro.
        /// </summary>
        public int IdFornecedorParametro { get; set; }

        /// <summary>
        /// Obtém ou define IDFineline.
        /// </summary>
        public long IDFineline { get; set; }

        /// <summary>
        /// Obtém ou define IDCategoria.
        /// </summary>
        public long IDCategoria { get; set; }

        /// <summary>
        /// Obtém ou define IDSubcategoria.
        /// </summary>
        public long IDSubcategoria { get; set; }

        /// <summary>
        /// Obtém ou define IDDepartamento.
        /// </summary>
        public int IDDepartamento { get; set; }

        /// <summary>
        /// Obtém ou define IDFornecedor.
        /// </summary>
        public long? IDFornecedor { get; set; }

        /// <summary>
        /// Obtém ou define cdItem.
        /// </summary>
        public int CdItem { get; set; }

        /// <summary>
        /// Obtém ou define cdOldNumber.
        /// </summary>
        public int CdOldNumber { get; set; }

        /// <summary>
        /// Obtém ou define vlCustoUnitario.
        /// </summary>
        public float? VlCustoUnitario { get; set; }

        /// <summary>
        /// Obtém ou define cdSistema.
        /// </summary>
        public int CdSistema { get; set; }

        /// <summary>
        /// Obtém ou define cdUPC.
        /// </summary>
        public decimal? CdUPC { get; set; }

        /// <summary>
        /// Obtém ou define dsItem.
        /// </summary>
        public string DsItem { get; set; }

        /// <summary>
        /// Obtém ou define dsHostItem.
        /// </summary>
        public string DsHostItem { get; set; }

        /// <summary>
        /// Obtém ou define blAtivo.
        /// </summary>
        public bool? BlAtivo { get; set; }

        /// <summary>
        /// Obtém ou define dhHostCreate.
        /// </summary>
        public DateTime? DhHostCreate { get; set; }

        /// <summary>
        /// Obtém ou define dhHostUpdate.
        /// </summary>
        public DateTime? DhHostUpdate { get; set; }

        /// <summary>
        /// Obtém ou define blPesadoCaixa.
        /// </summary>
        public bool? BlPesadoCaixa { get; set; }

        /// <summary>
        /// Obtém ou define blPesadoRetaguarda.
        /// </summary>
        public bool? BlPesadoRetaguarda { get; set; }

        /// <summary>
        /// Obtém ou define cdPLU.
        /// </summary>
        public long? CdPLU { get; set; }

        /// <summary>
        /// Obtém ou define dsTamanhoItem.
        /// </summary>
        public string DsTamanhoItem { get; set; }

        /// <summary>
        /// Obtém ou define dsCor.
        /// </summary>
        public string DsCor { get; set; }

        /// <summary>
        /// Obtém ou define dhCriacao.
        /// </summary>
        public DateTime? DhCriacao { get; set; }

        /// <summary>
        /// Obtém ou define dhAtualizacao.
        /// </summary>
        public DateTime? DhAtualizacao { get; set; }

        /// <summary>
        /// Obtém ou define o tipo de status do item.
        /// </summary>
        public TipoStatusItem TpStatus { get; set; }

        /// <summary>
        /// Obtém ou define dhAtualizacaoStatus.
        /// </summary>
        public DateTime? DhAtualizacaoStatus { get; set; }

        /// <summary>
        /// Obtém ou define cdUsuarioCriacao.
        /// </summary>
        public int? CdUsuarioCriacao { get; set; }

        /// <summary>
        /// Obtém ou define cdUsuarioAtualizacao.
        /// </summary>
        public int? CdUsuarioAtualizacao { get; set; }

        /// <summary>
        /// Obtém ou define tpIndPesoReal.
        /// </summary>
        public string TpIndPesoReal { get; set; }

        /// <summary>
        /// Obtém ou define tpCaixaFornecedor.
        /// </summary>
        public TipoCaixaFornecedor TpCaixaFornecedor { get; set; }

        /// <summary>
        /// Obtém ou define vlPesoLiquido.
        /// </summary>
        public decimal? VlPesoLiquido { get; set; }

        /// <summary>
        /// Obtém ou define tpAlinhamentoCD.
        /// </summary>
        public string TpAlinhamentoCD { get; set; }

        /// <summary>
        /// Obtem ou define tempoMinimoCD.
        /// </summary>
        public int? TempoMinimoCD { get; set; }

        /// <summary>
        /// Obtém ou define tpVinculado.
        /// </summary>
        public TipoVinculado TpVinculado
        {
            get
            {
                return this.m_tpVinculado;
            }

            set
            {
                m_tpVinculado = value ?? TipoVinculado.NaoDefinido;
            }
        }

        /// <summary>
        /// Obtém ou define tpReceituario.
        /// </summary>
        public TipoReceituario TpReceituario
        {
            get
            {
                return this.m_tpReceituario;
            }

            set
            {
                m_tpReceituario = value ?? TipoReceituario.NaoDefinido;
            }
        }

        /// <summary>
        /// Obtém ou define tpManipulado.
        /// </summary>
        public TipoManipulado TpManipulado
        {
            get
            {
                return this.m_tpManipulado;
            }

            set
            {
                m_tpManipulado = value ?? TipoManipulado.NaoDefinido;
            }
        }

        /// <summary>
        /// Obtém ou define qtVendorPackage.
        /// </summary>
        public int? QtVendorPackage { get; set; }

        /// <summary>
        /// Obtém ou define qtWarehousePackage.
        /// </summary>
        public int? QtWarehousePackage { get; set; }

        /// <summary>
        /// Obtém ou define vlFatorConversao.
        /// </summary>
        public float? VlFatorConversao { get; set; }

        /// <summary>
        /// Obtém ou define tpUnidadeMedida.
        /// </summary>
        public TipoUnidadeMedida TpUnidadeMedida { get; set; }

        /// <summary>
        /// Obtém ou define vlTipoReabastecimento.
        /// </summary>
        public ValorTipoReabastecimento VlTipoReabastecimento { get; set; }

        /// <summary>
        /// Obtém ou define vlShelfLife.
        /// </summary>
        public int? VlShelfLife { get; set; }

        /// <summary>
        /// Obtém ou define blItemTransferencia.
        /// </summary>
        public bool BlItemTransferencia { get; set; }

        /// <summary>
        /// Obtém ou define vlModulo.
        /// </summary>
        public decimal? VlModulo { get; set; }

        /// <summary>
        /// Obtém ou define cdDepartamentoVendor.
        /// </summary>
        public int? CdDepartamentoVendor { get; set; }

        /// <summary>
        /// Obtém ou define cdSequenciaVendor.
        /// </summary>
        public int? CdSequenciaVendor { get; set; }

        /// <summary>
        /// Obtém ou define IdRegiaoCompra
        /// </summary>
        public int? IdRegiaoCompra { get; set; }

        /// <summary>
        /// Obtém ou define o fornecedor.
        /// </summary>
        public Fornecedor Fornecedor { get; set; }

        /// <summary>
        /// Obtém ou define o FornecedorParametro.
        /// </summary>
        public FornecedorParametro FornecedorParametro { get; set; }

        /// <summary>
        /// Obtém ou define o FineLine.
        /// </summary>
        public FineLine FineLine { get; set; }

        /// <summary>
        /// Obtém ou define a subcategoria.
        /// </summary>
        public Subcategoria Subcategoria { get; set; }

        /// <summary>
        /// Obtém ou define a categoria.
        /// </summary>
        public Categoria Categoria { get; set; }

        /// <summary>
        /// Obtém ou define o departamento.
        /// </summary>
        public Departamento Departamento { get; set; }

        /// <summary>
        /// Obtém ou define a RegiaoCompra.
        /// </summary>
        public RegiaoCompra RegiaoCompra { get; set; }

        /// <summary>
        /// Obtém ou define a AreaCD.
        /// </summary>
        public AreaCD AreaCD { get; set; }

        /// <summary>
        /// Obtém ou define Estoque.
        /// </summary>
        public Movimentacao.Estoque Estoque { get; set; }

        /// <summary>
        /// Código da cross ref - obtido da tabela RelacionamentoItemPrime
        /// </summary>
        public int? CdCrossRef { get; set; }

        /// <summary>
        /// Obtém ou define qtItensEntrada.
        /// </summary>
        public int qtItensEntrada { get; set; }

        /// <summary>
        /// Obtém ou define qtItensCadastradosCompraCasada.
        /// </summary>
        public int qtItensCadastradosCompraCasada { get; set; }

        /// <summary>
        /// Obtém ou define FilhoCompraCasada.
        /// </summary>
        public bool? FilhoCompraCasada { get; set; }

        /// <summary>
        /// Obtém ou define PaiCompraCasada.
        /// </summary>
        public bool? PaiCompraCasada { get; set; }

        /// <summary>
        /// Obtém ou define ItemSaida.
        /// </summary>
        public ItemDetalhe ItemSaida { get; set; }

        /// <summary>
        /// Obtém ou define Traits.
        /// </summary>
        public int Traits { get; set; }

        /// <summary>
        /// Obtém ou define IDCompraCasada.
        /// </summary>
        public int? IDCompraCasada { get; set; }
        #endregion

        #region Propriedades calculadas
        
        /// <summary>
        /// Obtém um valor que indica se o item tem cadastro de compra casada.
        /// </summary>
        public bool PossuiCadastroCompraCasada
        {
            get
            {
                return qtItensCadastradosCompraCasada > 0;
            }
        }

        /// <summary>
        /// Obtém um valor que indica se o item é pesado caixa.
        /// </summary>
        public bool PesadoCaixa
        {
            get
            {
                return BlPesadoCaixa ?? false;
            }
        }

        /// <summary>
        /// Obtém um valor que indica se o item é pesado retaguarda.
        /// </summary>
        public bool PesadoRetaguarda
        {
            get
            {
                return BlPesadoRetaguarda ?? false;
            }
        }

        /// <summary>
        /// Obtém o tipo de item.
        /// </summary>
        public TipoItem TipoItem
        {
            get
            {
                if (PesadoCaixa || PesadoRetaguarda)
                {
                    return TipoItem.PesoVariavel;
                }

                return TipoItem.PesoFixo;
            }
        }

        /// <summary>
        /// Obrtém um valor indicando se o item recebe nota.
        /// </summary>
        /// <value>
        ///   <c>true</c> se o item recebe nota; caso contrário, <c>false</c>.
        /// </value>
        public bool RecebeNota
        {
            get
            {
                return TpManipulado != TipoManipulado.Derivado &&
                   TpVinculado != TipoVinculado.Saida &&
                   TpReceituario != TipoReceituario.Transformado;
            }
        }

        /// <summary>
        /// Obtém ou define o relacionamentoSGP.
        /// </summary>
        public bool relacionamentoSGP { get; set; }

        /// <summary>
        /// Obtém ou define o hasLojas.
        /// </summary>
        public bool hasLojas { get; set; }

        /// <summary>
        /// Obtém IsXDock.
        /// </summary>
        public bool IsXDock
        {
            get
            {
                return ValorTipoReabastecimento.TodosXDock.Contains(VlTipoReabastecimento);
            }
        }

        /// <summary>
        /// Obtém IsDSD.
        /// </summary>
        public bool IsDSD
        {
            get
            {
                return ValorTipoReabastecimento.TodosDSD.Contains(VlTipoReabastecimento);
            }
        }

        /// <summary>
        /// Obtém IsStaple.
        /// </summary>
        public bool IsStaple
        {
            get
            {
                return ValorTipoReabastecimento.TodosStaple.Contains(VlTipoReabastecimento);
            }
        }

        /// <summary>
        /// Obtém Multisourcing.
        /// </summary>
        public Multisourcing Multisourcing { get; set; }
        #endregion
    }
}
