using System;
using System.Collections.Generic;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Movimentacao
{
    /// <summary>
    /// Representa uma NotaFiscal.
    /// </summary>
    public class NotaFiscal : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="NotaFiscal"/>.
        /// </summary>
        public NotaFiscal()
        {
            this.Itens = new List<NotaFiscalItem>();
        }

        /// <summary>
        /// Obtém ou define IDNotaFiscal.
        /// </summary>
        public long IDNotaFiscal { get; set; }

        /// <summary>
        /// Obtém ou define IDConcentrador.
        /// </summary>
        public long? IDConcentrador { get; set; }

        /// <summary>
        /// Obtém ou define IDLoja.
        /// </summary>
        public int? IDLoja { get; set; }

        /// <summary>
        /// Obtém ou define IDBandeira.
        /// </summary>
        public int? IDBandeira { get; set; }

        /// <summary>
        /// Obtém ou define IDFornecedor.
        /// </summary>
        public long? IDFornecedor { get; set; }

        /// <summary>
        /// Obtém ou define nrNotaFiscal.
        /// </summary>
        public long? nrNotaFiscal { get; set; }

        /// <summary>
        /// Obtém ou define srNotaFiscal.
        /// </summary>
        public string srNotaFiscal { get; set; }

        /// <summary>
        /// Obtém ou define dtEmissao.
        /// </summary>
        public DateTime? dtEmissao { get; set; }

        /// <summary>
        /// Obtém ou define dtRecebimento.
        /// </summary>
        public DateTime? dtRecebimento { get; set; }

        /// <summary>
        /// Obtém ou define dtCadastroLivro.
        /// </summary>
        public DateTime? dtCadastroLivro { get; set; }

        /// <summary>
        /// Obtém ou define dtCadastroConcentrador.
        /// </summary>
        public DateTime? dtCadastroConcentrador { get; set; }

        /// <summary>
        /// Obtém ou define dtAtualizacaoConcentrador.
        /// </summary>
        public DateTime? dtAtualizacaoConcentrador { get; set; }

        /// <summary>
        /// Obtém ou define dtInclusaoHistorico.
        /// </summary>
        public DateTime? dtInclusaoHistorico { get; set; }

        /// <summary>
        /// Obtém ou define dtAlteracaoHistorico.
        /// </summary>
        public DateTime? dtAlteracaoHistorico { get; set; }

        /// <summary>
        /// Obtém ou define blDivergente.
        /// </summary>
        public bool? blDivergente { get; set; }

        /// <summary>
        /// Obtém ou define dtLiberacao.
        /// </summary>
        public DateTime? dtLiberacao { get; set; }

        /// <summary>
        /// Obtém ou define IDTipoNota.
        /// </summary>
        public byte? IDTipoNota { get; set; }

        /// <summary>
        /// Obtém ou define Visivel.
        /// </summary>
        public bool? Visivel { get; set; }

        /// <summary>
        /// Obtém ou define tpOperacao.
        /// </summary>
        public string tpOperacao { get; set; }

        /// <summary>
        /// Obtém ou define cdCfop.
        /// </summary>
        public int? cdCfop { get; set; }

        /// <summary>
        /// Obtém ou define DhCriacao.
        /// </summary>
        public DateTime? DhCriacao { get; set; }

        /// <summary>
        /// Obtém ou define os itens da nota fiscal.
        /// </summary>
        public IList<NotaFiscalItem> Itens { get; set; }

        /// <summary>
        /// Obtém ou define a loja.
        /// </summary>
        public Loja Loja { get; set; }

        /// <summary>
        /// Obtém ou define o fornecedor.
        /// </summary>
        public Fornecedor Fornecedor { get; set; }

        /// <summary>
        /// Obtém ou define a bandeira.
        /// </summary>
        public Bandeira Bandeira { get; set; }
    }
}
