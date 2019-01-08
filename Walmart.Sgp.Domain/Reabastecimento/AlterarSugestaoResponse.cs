using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Resultado de uma operação de alterar sugestão pedido.
    /// </summary>
    public class AlterarSugestaoResponse
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="AlterarSugestaoResponse"/>.
        /// </summary>
        /// <param name="idSugestaoPedido">O id de sugestao pedido.</param>
        public AlterarSugestaoResponse(int idSugestaoPedido)
        {
            this.IDSugestaoPedido = idSugestaoPedido;
        }

        /// <summary>
        /// Obtém ou define o id de sugestão pedido.
        /// </summary>
        public int IDSugestaoPedido { get; set; }

        /// <summary>
        /// Obtém ou define um valor que indica que a sugestão pedido não foi localizada no banco de dados.
        /// </summary>
        public bool Inexistente { get; set; }

        /// <summary>
        /// Obtém um valor que indica se a alteração foi realizada com sucesso.
        /// </summary>
        public bool Sucesso { get; set; }

        /// <summary>
        /// Obtém ou define um valor que indica que a sugestão não pode ser salva porque a alteração foi maior que o percentual de alteração permitido.
        /// </summary>
        public bool NaoSalvaPercentualAlteracao { get; set; }

        /// <summary>
        /// Obtém ou define um valor que indica que a sugestão não foi salva por causa da grade de sugestão.
        /// </summary>
        public bool NaoSalvaGradeSugestao { get; set; }
    }
}
