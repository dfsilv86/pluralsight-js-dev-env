using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Interface do gateway do ReturnSheet
    /// </summary>
    public interface IReturnSheetGateway : IDataGateway<ReturnSheet>
    {
        /// <summary>
        /// Obtém um ReturnSheet pelo seu id.
        /// </summary>
        /// <param name="id">O id.</param>
        /// <returns>O ReturnSheet.</returns>
        ReturnSheet Obter(long id);

        /// <summary>
        /// Pesquisar ReturnSheet
        /// </summary>
        /// <param name="dtInicioReturn">Data inicio do ReturnSheet</param>
        /// <param name="dtFinalReturn">Data final do ReturnSheet</param>
        /// <param name="evento">Descricao do evento (ReturnSheet)</param>
        /// <param name="idDepartamento">Id do departamento</param>
        /// <param name="filtroAtivos">Filtrar somente ativos (0 = Somente Inativos, 1 = Somente Ativos, 2 = Todos)</param>
        /// <param name="idRegiaoCompra">Id da regiao de compra.</param>
        /// <param name="paging">A paginacao</param>
        /// <returns>Uma lista de ReturnSheet</returns>
        IEnumerable<ReturnSheet> Pesquisar(DateTime? dtInicioReturn, DateTime? dtFinalReturn, string evento, int? idDepartamento, int filtroAtivos, int? idRegiaoCompra, Paging paging);

        /// <summary>
        /// Verifica se uma ReturnSheet possui itens exportados
        /// </summary>
        /// <param name="idReturnSheet">Id da returnSheet.</param>
        /// <returns>Se a ReturnSheet possui itens exportados.</returns>
        bool PossuiExportacao(int idReturnSheet);

        /// <summary>
        /// Verifica se uma ReturnSheet possui itens autorizados
        /// </summary>
        /// <param name="idReturnSheet">Id da returnSheet.</param>
        /// <returns>Se a ReturnSheet possui itens autorizados.</returns>
        bool PossuiAutorizacao(int idReturnSheet);
    }
}
