using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Item;

namespace Walmart.Sgp.Domain.Movimentacao
{
    /// <summary>
    /// Representa uma MTR (Movimentação de Transferência de Retail).
    /// </summary>
    public class MovimentacaoMtr
    {
        /// <summary>
        /// Obtém ou define o id do item detalhe origem.
        /// </summary>
        public int IdItemOrigem { get; set; }

        /// <summary>
        /// Obtém ou define o id do item detalhe destino.
        /// </summary>
        public int IdItemDestino { get; set; }

        /// <summary>
        /// Obtém ou define o id da loja.
        /// </summary>
        public int IdLoja { get; set; }

        /// <summary>
        /// Obtém ou define a quantidade.
        /// </summary>
        public decimal Quantidade { get; set; }
    }
}
