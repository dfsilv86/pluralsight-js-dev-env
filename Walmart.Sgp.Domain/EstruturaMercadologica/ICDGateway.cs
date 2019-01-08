using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Define a interface de um table data gateway para CD.
    /// </summary>
    public interface ICDGateway : IDataGateway<CD>
    {
        /// <summary>
        /// Obtém um CD pelo seu id.
        /// </summary>
        /// <param name="id">O id.</param>
        /// <returns>A entidade CD.</returns>
        CD Obter(long id);

        /// <summary>
        /// Obtém o ID de um CD pelo seu Código.
        /// </summary>
        /// <param name="cdCD">O código do CD.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>ID do CD.</returns>
        int ObterIdCDPorCodigo(int cdCD, int cdSistema);

        /// <summary>
        /// Obtém todos os CDs convertidos ativos.
        /// </summary>
        /// <returns>Retorna a lista com todos os CDs convertidos ativos.</returns>
        IEnumerable<CD> ObterTodosConvertidosAtivos();
    }
}
