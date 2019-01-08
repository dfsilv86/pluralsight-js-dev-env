using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Walmart.Sgp.Domain.Inventarios
{
    /// <summary>
    /// Classe que representa um ItemDetalhe com seu CD vinculado e outras informações de fornecedor / multisourcing
    /// </summary>
    public class ItemDetalheCD
    {
        /// <summary>
        /// Obtem ou define valor para IDItemDetalhe
        /// </summary>
        public int IDItemDetalhe { get; set; }

        /// <summary>
        /// Obtem ou define valor para IDRelacionamentoItemSecundario
        /// </summary>
        public int IDRelacionamentoItemSecundario { get; set; }

        /// <summary>
        /// Obtem ou define valor para cdItem
        /// </summary>
        public string dsItem { get; set; }

        /// <summary>
        /// Obtem ou define valor para cdItem
        /// </summary>
        public int cdItem { get; set; }

        /// <summary>
        /// Obtem ou define valor para IDCD
        /// </summary>
        public int IDCD { get; set; }

        /// <summary>
        /// Obtem ou define valor para cdCD
        /// </summary>
        public int cdCD { get; set; }

        /// <summary>
        /// Obtem ou define valor para QtdItensEntrada
        /// </summary>
        public int QtdItensEntrada { get; set; }

        /// <summary>
        /// Obtem ou define valor para QtdMultisourcing
        /// </summary>
        public int QtdMultisourcing { get; set; }

        /// <summary>
        /// Obtem ou define valor para PossuiCadastro
        /// </summary>
        public bool PossuiCadastro
        {
            get
            {
                return this.QtdMultisourcing > 0;
            }
        }

        /// <summary>
        /// Obtem ou define valor para Multivendor
        /// </summary>
        public bool Multivendor 
        { 
            get 
            { 
                return this.QtdItensEntrada > 2;
            }
        }

        /// <summary>
        /// Obtem ou define valor para nmFornecedor
        /// </summary>
        public string nmFornecedor { get; set; }

        /// <summary>
        /// Obtem ou define valor para cdV9D
        /// </summary>
        public long cdV9D { get; set; }

        /// <summary>
        /// Obtem ou define valor para cdTipo
        /// </summary>
        public char cdTipo { get; set; }

        /// <summary>
        /// Obtem ou define valor para cdItemSaida
        /// </summary>
        public long cdItemSaida { get; set; }

        /// <summary>
        /// Obtem ou define valor para IDMultisourcing
        /// </summary>
        public long? IDMultisourcing { get; set; }

        /// <summary>
        /// Obtem ou define valor para vlPercentual
        /// </summary>
        public decimal? vlPercentual { get; set; }

        /// <summary>
        /// Obtem ou define valor para qtVendorPackage
        /// </summary>
        public decimal? qtVendorPackage { get; set; }

        /// <summary>
        /// Obtem ou define valor para vlPesoLiquido
        /// </summary>
        public decimal? vlPesoLiquido { get; set; }

        /// <summary>
        /// Obtem ou define valor para idCompraCasada
        /// </summary>
        public decimal? idCompraCasada { get; set; }
    }
}