using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Item;

namespace Walmart.Sgp.Domain.Reabastecimento.CompraCasada
{
    /// <summary>
    /// Representa uma PesquisaCompraCasadaFiltro.
    /// </summary>
    public class PesquisaCompraCasadaFiltro
    {
        /// <summary>
        /// Obtém ou define ItemPaiSelecionado.
        /// </summary>
        public ItemDetalhe ItemPaiSelecionado { get; set; }

        /// <summary>
        /// Obtém ou define Itens.
        /// </summary>
        public IEnumerable<ItemDetalhe> Itens { get; set; }

        /// <summary>
        /// Obtém ou define idDepartamento.
        /// </summary>
        public int? idDepartamento { get; set; }

        /// <summary>
        /// Obtém ou define cdSistema.
        /// </summary>
        public int? cdSistema { get; set; }

        /// <summary>
        /// Obtém ou define idFornecedorParametro.
        /// </summary>
        public int? idFornecedorParametro { get; set; }

        /// <summary>
        /// Obtém ou define idItemDetalheSaida.
        /// </summary>
        public int? idItemDetalheSaida { get; set; }

        /// <summary>
        /// Obtém ou define blPossuiCadastro.
        /// </summary>
        public bool? blPossuiCadastro { get; set; }
    }
}
