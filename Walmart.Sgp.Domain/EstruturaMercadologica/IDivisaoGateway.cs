using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Define a interface de um table data gateway para divisao.
    /// </summary>
    public interface IDivisaoGateway
    {
        /// <summary>
        /// Obtém a divisão pelo código da divisão e código da estrutura mercadológica.
        /// </summary>
        /// <param name="cdDivisao">O código de divisao.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>A divisão.</returns>
        Divisao ObterPorDivisaoESistema(int cdDivisao, byte cdSistema);

        /// <summary>
        /// Pesquisa divisões filtrando pelo código de divisão, descrição da divisão e/ou código da estrutura mercadológica.
        /// </summary>
        /// <param name="cdDivisao">O código de divisao.</param>
        /// <param name="dsDivisao">A descrição da divisão.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>A divisão.</returns>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <remarks>Não valida o usuário que está efetuando a pesquisa. O filtro da descrição é Contains.</remarks>
        IEnumerable<Divisao> PesquisarPorSistema(int? cdDivisao, string dsDivisao, byte cdSistema, Paging paging);
    }
}
