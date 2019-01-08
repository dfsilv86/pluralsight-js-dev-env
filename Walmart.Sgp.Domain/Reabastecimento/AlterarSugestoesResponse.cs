using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Informações gerais sobre o resultado uma operação de alterar sugestão pedido executada em várias entidades.
    /// </summary>
    public class AlterarSugestoesResponse
    {
        /// <summary>
        /// Obtém ou define o total de sugestões enviadas para salvar.
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Obtém ou define o número de sugestões alteradas com sucesso.
        /// </summary>
        public int Sucesso { get; set; }

        /// <summary>
        /// Obtém ou define o total de sugestões não localizadas no banco de dados.
        /// </summary>
        public int Inexistentes { get; set; }

        /// <summary>
        /// Obtém ou define o número de sugestões que não foram salvas por causa do percentual de modificação da alçada.
        /// </summary>
        public int NaoSalvaPercentualAlteracao { get; set; }

        /// <summary>
        /// Obtém ou define o número de sugestões que não foram salvas por causa da grade de sugestão.
        /// </summary>
        public int NaoSalvaGradeSugestao { get; set; }

        /// <summary>
        /// A mensagem traduzida do resultado.
        /// </summary>
        public string Mensagem { get; set; }

        /// <summary>
        /// A lista de IDs de SugestaoPedido que foram realmente alterados em banco, para o commit() na tela.
        /// </summary>
        public IEnumerable<int> IDSugestaoPedidoAlterados { get; set; }
    }
}
