using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Domain.Movimentacao
{
    /// <summary>
    /// Informações de custo mais recente de um item.
    /// </summary>
    public class CustoMaisRecente
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="CustoMaisRecente"/>.
        /// </summary>
        /// <param name="notaFiscal">A nota fiscal.</param>
        /// <param name="estoque">O estoque.</param>
        public CustoMaisRecente(NotaFiscal notaFiscal, Estoque estoque)
        {
            this.NotaFiscal = notaFiscal;
            this.Estoque = estoque;
        }

        /// <summary>
        /// Obtém a nota fiscal.
        /// </summary>
        public NotaFiscal NotaFiscal { get; private set; }

        /// <summary>
        /// Obtém o estoque.
        /// </summary>
        public Estoque Estoque { get; private set; }

        /// <summary>
        /// Obtém o número da linha.
        /// </summary>
        public int? NrLinha { get; set; }
    }
}
