using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Item;

namespace Walmart.Sgp.Domain.Movimentacao
{
    /// <summary>
    /// Resposta da consulta de custo mais recente de itens relacionados a um item detalhe.
    /// </summary>
    public class CustoItemRelacionadoResponse
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="CustoItemRelacionadoResponse"/>.
        /// </summary>
        /// <param name="isPrincipal">Se é uma informação vinda do relacionamento principal.</param>
        /// <param name="principal">O relacionamento principal.</param>
        /// <param name="secundario">O relacionamento secundário.</param>
        /// <param name="custo">O custo mais recente do item presente no relacionamento secundário.</param>
        public CustoItemRelacionadoResponse(bool isPrincipal, RelacionamentoItemPrincipal principal, RelacionamentoItemSecundario secundario, CustoMaisRecente custo)
        {
            this.IsPrincipal = isPrincipal;
            this.RelacionamentoPrincipal = principal;
            this.RelacionamentoSecundario = secundario;
            this.CustoMaisRecente = custo;
        }

        /// <summary>
        /// Obtém um valor que indica se esta tupla veio de um relacionamento principal.
        /// </summary>
        public bool IsPrincipal { get; private set; }

        /// <summary>
        /// Obtém o relacionamento principal.
        /// </summary>
        public RelacionamentoItemPrincipal RelacionamentoPrincipal { get; private set; }

        /// <summary>
        /// Obtém o relacionamento secundário.
        /// </summary>
        public RelacionamentoItemSecundario RelacionamentoSecundario { get; private set; }

        /// <summary>
        /// Obtém ou define o custo mais recente (nota fiscal e estoque)
        /// </summary>
        public CustoMaisRecente CustoMaisRecente { get; set; }
    }
}
