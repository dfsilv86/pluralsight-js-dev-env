using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.Memory
{
    /// <summary>
    /// Implementação de um table data gateway para CD em memória.
    /// </summary>
    /// <remarks>
    /// Essa implementação deve ser utilizada apenas para fins de testes unitários.
    /// </remarks>
    public class MemoryCDGateway : MemoryDataGateway<CD>, ICDGateway
    {
        /// <summary>
        /// Obtém um CD pelo seu id.
        /// </summary>
        /// <param name="id">O id.</param>
        /// <returns>A entidade CD.</returns>
        public CD Obter(long id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém o ID de um CD pelo seu Código.
        /// </summary>
        /// <param name="cdCD">O código do CD.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>ID do CD.</returns>
        public int ObterIdCDPorCodigo(int cdCD, int cdSistema)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém todos os CDs convertidos ativos.
        /// </summary>
        /// <returns>Retorna a lista com todos os CDs convertidos ativos.</returns>
        public IEnumerable<CD> ObterTodosConvertidosAtivos()
        {
            throw new NotImplementedException();
        }
    }
}