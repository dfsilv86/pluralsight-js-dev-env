using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.Movimentacao;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Representa uma SugestaoReturnSheet.
    /// </summary>
    public class SugestaoReturnSheet : EntityBase, IAggregateRoot
    {
        private int? m_qtdLoja;

        private int? m_qtdRA;

        /// <summary>
        /// Obtém ou define Id.
        /// </summary>
        public override int Id
        {
            get
            {
                return this.IdSugestaoReturnSheet;
            }

            set
            {
                this.IdSugestaoReturnSheet = value;
            }
        }

        /// <summary>
        /// Obtém ou define IdSugestaoReturnSheet.
        /// </summary>
        public int IdSugestaoReturnSheet { get; set; }

        /// <summary>
        /// Obtém ou define IdReturnSheetItemLoja.
        /// </summary>
        public int IdReturnSheetItemLoja { get; set; }

        /// <summary>
        /// Obtém ou define EstoqueItemVenda.
        /// </summary>
        public decimal? EstoqueItemVenda { get; set; }

        /// <summary>
        /// Obtém ou define qtVendorPackageItemCompra.
        /// </summary>
        public int? qtVendorPackageItemCompra { get; set; }

        /// <summary>
        /// Obtém ou define PackSugeridoCompra.
        /// </summary>
        public int? PackSugeridoCompra { get; set; }

        /// <summary>
        /// Obtém ou define vlPesoLiquidoItemCompra.
        /// </summary>
        public decimal? vlPesoLiquidoItemCompra { get; set; }

        /// <summary>
        /// Obtém ou define vlTipoAbastetimentoItemCompra.
        /// </summary>
        public int? vlTipoAbastetimentoItemCompra { get; set; }

        /// <summary>
        /// Obtém ou define QtdLoja.
        /// </summary>
        public int? QtdLoja
        {
            get
            {
                return m_qtdLoja;
            }

            set
            {
                m_qtdLoja = value;
            }
        }

        /// <summary>
        /// Obtém ou define QtdRA.
        /// </summary>
        public int? QtdRA
        {
            get
            {
                return m_qtdRA;
            }

            set
            {
                m_qtdRA = value;
            }
        }

        /// <summary>
        /// Obtém ou define IdUsuarioCriacao.
        /// </summary>
        public int? IdUsuarioCriacao { get; set; }

        /// <summary>
        /// Obtém ou define DhCriacao.
        /// </summary>
        public DateTime? DhCriacao { get; set; }

        /// <summary>
        /// Obtém ou define IdUsuarioAtualizacao.
        /// </summary>
        public int? IdUsuarioAtualizacao { get; set; }

        /// <summary>
        /// Obtém ou define DhAtualizacao.
        /// </summary>
        public DateTime? DhAtualizacao { get; set; }

        /// <summary>
        /// Obtém ou define IdUsuarioRA.
        /// </summary>
        public int? IdUsuarioRA { get; set; }

        /// <summary>
        /// Obtém ou define DhAtualizacaoRA.
        /// </summary>
        public DateTime? DhAtualizacaoRA { get; set; }

        /// <summary>
        /// Obtém ou define BlExportado.
        /// </summary>
        public bool? BlExportado { get; set; }

        /// <summary>
        /// Obtém ou define DhExportacao.
        /// </summary>
        public DateTime? DhExportacao { get; set; }

        /// <summary>
        /// Obtém ou define BlAtivo.
        /// </summary>
        public bool? BlAtivo { get; set; }

        /// <summary>
        /// Obtém ou define ItemLoja.
        /// </summary>
        public ReturnSheetItemLoja ItemLoja { get; set; }

        /// <summary>
        /// Obtém ou define vlCustoContabilItemVenda.
        /// </summary>
        public decimal? vlCustoContabilItemVenda { get; set; }

        /// <summary>
        /// Obtém ou define BlAutorizado.
        /// </summary>
        public bool? BlAutorizado { get; set; }

        /// <summary>
        /// Obtém ou define DhAutorizacao.
        /// </summary>
        public DateTime? DhAutorizacao { get; set; }

        /// <summary>
        /// Obtém ou define IdUsuarioAutorizacao.
        /// </summary>
        public int? IdUsuarioAutorizacao { get; set; }

        /// <summary>
        /// Obtém ou define PrecoVenda.
        /// </summary>
        public decimal? PrecoVenda { get; set; }

        #region Propriedades Calculadas
        /// <summary>
        /// Obtém ou define Subtotal.
        /// </summary>
        public int? Subtotal { get; set; }

        /// <summary>
        /// Obtém ou define ReturnSheet.
        /// </summary>
        public ReturnSheet ReturnSheet { get; set; }

        /// <summary>
        /// Obtém ou define Fornecedor.
        /// </summary>
        public Fornecedor Fornecedor { get; set; }

        /// <summary>
        /// Obtém ou define FornecedorParametro.
        /// </summary>
        public FornecedorParametro FornecedorParametro { get; set; }

        /// <summary>
        /// Obtém ou define ItemDetalheEntrada.
        /// </summary>
        public ItemDetalhe ItemDetalheEntrada { get; set; }

        /// <summary>
        /// Obtém ou define ItemDetalheSaida
        /// </summary>
        public ItemDetalhe ItemDetalheSaida { get; set; }

        /// <summary>
        /// Obtém ou define UsuarioAutorizacao
        /// </summary>
        public Usuario UsuarioAutorizacao { get; set; }

        /// <summary>
        /// Obtém ou define UsuarioAtualizacao
        /// </summary>
        public Usuario UsuarioAtualizacao { get; set; }

        /// <summary>
        /// Obtém ou define Loja
        /// </summary>
        public Loja Loja { get; set; }

        /// <summary>
        /// Obtém Finalizada.
        /// </summary>
        public bool Finalizada
        {
            get
            {
                if (this.ItemLoja == null)
                {
                    return false;
                }

                return this.ItemLoja.ItemPrincipal.ReturnSheet.DhFinalReturn < DateTime.Now;
            }
        }
        #endregion
    }
}
