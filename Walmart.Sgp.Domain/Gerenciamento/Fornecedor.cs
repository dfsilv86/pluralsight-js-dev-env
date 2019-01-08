using System;
using System.Collections.Generic;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Gerenciamento
{
    /// <summary>
    /// Representa uma Fornecedor.
    /// </summary>
    public class Fornecedor : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="Fornecedor"/>
        /// </summary>
        public Fornecedor()
        {
            Parametros = new List<FornecedorParametro>();
        }

        /// <summary>
        /// Obtém ou define IDFornecedor.
        /// </summary>
        public long IDFornecedor { get; set; }

        /// <summary>
        /// Obtém ou define cdFornecedor.
        /// </summary>
        public long cdFornecedor { get; set; }

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
        /// Obtém ou define o status.
        /// </summary>        
        public FornecedorStatus stFornecedor { get; set; }

        /// <summary>
        /// Obtém ou define cdSistema.
        /// </summary>
        public byte cdSistema { get; set; }

        /// <summary>
        /// Obtém ou define FornecedorParametros.
        /// </summary>
        public IList<FornecedorParametro> Parametros { get; set; }
    }
}
