using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Item;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Representa uma SugestaoReturnSheetConsolidado.
    /// </summary>
    public class SugestaoReturnSheetConsolidado
    {
        /// <summary>
        /// Obtém ou define IdSugestaoReturnSheet.
        /// </summary>
        public int IdSugestaoReturnSheet { get; set; }

        /// <summary>
        /// Obtém ou define BlExportado.
        /// </summary>
        public bool BlExportado { get; set; }

        /// <summary>
        /// Obtém ou define UsuarioAutorizacao.
        /// </summary>
        public string UsuarioAutorizacao { get; set; }

        /// <summary>
        /// Obtém ou define DhExportacao.
        /// </summary>
        public DateTime? DhExportacao { get; set; }

        /// <summary>
        /// Obtém ou define Descricao.
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Obtém ou define DhInicioEvento.
        /// </summary>
        public DateTime DhInicioEvento { get; set; }

        /// <summary>
        /// Obtém ou define DhFinalEvento.
        /// </summary>
        public DateTime DhFinalEvento { get; set; }

        /// <summary>
        /// Obtém ou define DhInicioReturn.
        /// </summary>
        public DateTime DhInicioReturn { get; set; }

        /// <summary>
        /// Obtém ou define DhFinalReturn.
        /// </summary>
        public DateTime DhFinalReturn { get; set; }

        /// <summary>
        /// Obtém ou define cdItemDetalheSaida.
        /// </summary>
        public int cdItemDetalheSaida { get; set; }

        /// <summary>
        /// Obtém ou define dsItemDetalheSaida.
        /// </summary>
        public string dsItemDetalheSaida { get; set; }

        /// <summary>
        /// Obtém ou define cdV9D.
        /// </summary>
        public long cdV9D { get; set; }

        /// <summary>
        /// Obtém ou define cdTipo.
        /// </summary>
        public TipoCodigoReabastecimento cdTipo { get; set; }

        /// <summary>
        /// Obtém ou define nmFornecedor.
        /// </summary>
        public string nmFornecedor { get; set; }

        /// <summary>
        /// Obtém ou define cdItemDetalheEntrada.
        /// </summary>
        public int cdItemDetalheEntrada { get; set; }

        /// <summary>
        /// Obtém ou define dsItemDetalheEntrada.
        /// </summary>
        public string dsItemDetalheEntrada { get; set; }

        /// <summary>
        /// Obtém ou define vlCustoContabilAtual.
        /// </summary>
        public decimal? vlCustoContabilAtual { get; set; }

        /// <summary>
        /// Obtém ou define qtEstoqueFisico.
        /// </summary>
        public decimal qtEstoqueFisico { get; set; }

        /// <summary>
        /// Obtém ou define cdLoja.
        /// </summary>
        public int cdLoja { get; set; }

        /// <summary>
        /// Obtém ou define qtVendorPackageItemCompra.
        /// </summary>
        public int? qtVendorPackageItemCompra { get; set; }

        /// <summary>
        /// Obtém ou define vlPesoLiquidoItemCompra.
        /// </summary>
        public int? vlPesoLiquidoItemCompra { get; set; }

        /// <summary>
        /// Obtém ou define TipoRA.
        /// </summary>
        public ValorTipoReabastecimento TipoRA { get; set; }

        /// <summary>
        /// Obtém ou define QtdLoja.
        /// </summary>
        public int? QtdLoja { get; set; }

        /// <summary>
        /// Obtém ou define qtdRA.
        /// </summary>
        public int? qtdRA { get; set; }

        /// <summary>
        /// Obtém ou define tpCaixaFornecedor.
        /// </summary>
        public TipoCaixaFornecedor tpCaixaFornecedor { get; set; }

        /// <summary>
        /// Obtém ou define Subtotal.
        /// </summary>
        public int? Subtotal { get; set; }

        /// <summary>
        /// Obtém ou define DhAtualizacao.
        /// </summary>
        public DateTime? DhAtualizacao { get; set; }

        /// <summary>
        /// Obtém ou define UsuarioAtualizacao.
        /// </summary>
        public string UsuarioAtualizacao { get; set; }

        /// <summary>
        /// Obtém ou define vlCustoContabilItemVenda.
        /// </summary>
        public decimal? vlCustoContabilItemVenda { get; set; }
        
        /// <summary>
        /// Obtém ou define EstoqueItemVenda.
        /// </summary>
        public decimal? EstoqueItemVenda { get; set; }

        /// <summary>
        /// Obtém ou define BlAtivo.
        /// </summary>
        public bool BlAtivo { get; set; }

        /// <summary>
        /// Obtém ou define SRSBlAtivo.
        /// </summary>
        public bool SRSBlAtivo { get; set; }

        /// <summary>
        /// Obtém ou define BlAutorizado.
        /// </summary>
        public bool BlAutorizado { get; set; }

        /// <summary>
        /// Obtém ou define DhAutorizacao
        /// </summary>
        public DateTime? DhAutorizacao { get; set; }

        /// <summary>
        /// Obtém ou define PrecoVenda.
        /// </summary>
        public decimal? PrecoVenda { get; set; }
    }
}
