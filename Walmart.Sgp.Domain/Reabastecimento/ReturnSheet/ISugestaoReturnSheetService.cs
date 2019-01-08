using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Interface do SugestaoReturnSheetService
    /// </summary>
    public interface ISugestaoReturnSheetService : IDomainService<SugestaoReturnSheet>
    {
        /// <summary>
        /// Autoriza a exportação dos registros pesquisados.
        /// </summary>
        /// <param name="dtInicioReturn">A data de início da return sheet.</param>
        /// <param name="dtFinalReturn">A data final da return sheet.</param>
        /// <param name="cdV9D">O código de 9 dígitos do vendor.</param>
        /// <param name="evento">O nome do evento.</param>
        /// <param name="cdItemDetalhe">O código do item detalhe de entrada.</param>
        /// <param name="cdDepartamento">O código do departamento.</param>
        /// <param name="cdLoja">O código da loja.</param>
        /// <param name="idRegiaoCompra">O identificador da região de compra.</param>
        /// <param name="blExportado">O flag indicando se a sugestão foi exportada.</param>
        /// <param name="blAutorizado">O flag indicando se a sugestão foi autorizada.</param>
        void AutorizarExportarPlanilhas(DateTime? dtInicioReturn, DateTime? dtFinalReturn, long? cdV9D, string evento, int? cdItemDetalhe, int? cdDepartamento, int? cdLoja, int? idRegiaoCompra, bool? blExportado, bool? blAutorizado);

        /// <summary>
        /// Obtém um SugestaoReturnSheet pelo seu id.
        /// </summary>
        /// <param name="id">O id.</param>
        /// <returns>O SugestaoReturnSheet.</returns>
        SugestaoReturnSheet Obter(long id);

        /// <summary>
        /// Buscar por IdReturnSheetItemLoja.
        /// </summary>
        /// <param name="idReturnSheetItemLoja">O idReturnSheetItemLoja.</param>
        /// <returns>Lista de SugestaoReturnSheet.</returns>
        IEnumerable<SugestaoReturnSheet> ObterPorIdReturnSheetItemLoja(int idReturnSheetItemLoja);

        /// <summary>
        /// Obtem Sugestoes para visualização na tela de Consulta Loja.
        /// </summary>
        /// <param name="idDepartamento">O Id do departamento</param>
        /// <param name="idLoja">O ID da loja.</param>
        /// <param name="dataSolicitacao">A data da solicitacao</param>
        /// <param name="evento">Nome do evento</param>
        /// <param name="vendor9D">Codigo vendor 9 digitos</param>
        /// <param name="idItemDetalhe">Id de um Item</param>
        /// <param name="paging">Dados de paginacao</param>
        /// <returns>Uma lista de SugestaoReturnSheet para consulta.</returns>
        IEnumerable<SugestaoReturnSheet> ConsultaReturnSheetLoja(int idDepartamento, long idLoja, DateTime dataSolicitacao, string evento, long vendor9D, int idItemDetalhe, Paging paging);

        /// <summary>
        /// Obtem Sugestões para visualização na tela de Consulta Return Sheet RA.
        /// </summary>
        /// <param name="dtInicioReturn">A data de início da return sheet.</param>
        /// <param name="dtFinalReturn">A data final da return sheet.</param>
        /// <param name="cdV9D">O código de 9 dígitos do vendor.</param>
        /// <param name="evento">O nome do evento.</param>
        /// <param name="cdItemDetalhe">O código do item detalhe de entrada.</param>
        /// <param name="cdDepartamento">O código do departamento.</param>
        /// <param name="cdLoja">O código da loja.</param>
        /// <param name="idRegiaoCompra">O identificador da região de compra.</param>
        /// <param name="blExportado">O flag indicando se a sugestão foi exportada.</param>
        /// <param name="blAutorizado">O flag indicando se a sugestão foi autorizada.</param>
        /// <param name="paging">Dados de paginação.</param>
        /// <returns>Retorna Sugestões para visualização na tela de Consulta Return Sheet RA.</returns>
        IEnumerable<SugestaoReturnSheet> ConsultaReturnSheetLojaRA(DateTime? dtInicioReturn, DateTime? dtFinalReturn, long? cdV9D, string evento, int? cdItemDetalhe, int? cdDepartamento, int? cdLoja, int? idRegiaoCompra, bool? blExportado, bool? blAutorizado, Paging paging);

        /// <summary>
        /// Salva sugestões alteradas pela loja.
        /// </summary>
        /// <param name="sugestoes">Lista de sugestões que serão atualizadas.</param>
        void SalvarSugestoesLoja(IEnumerable<SugestaoReturnSheet> sugestoes);

        /// <summary>
        /// Salva sugestões alteradas pelo RA.
        /// </summary>
        /// <param name="sugestoes">Lista de sugestões que serão atualizadas.</param>
        void SalvarSugestoesRA(IEnumerable<SugestaoReturnSheet> sugestoes);

        /// <summary>
        /// Verifica se existem return sheets vigentes que ainda não foram solicitadas quantidades considerando o papel do usuário e lojas que está logado.
        /// </summary>
        /// <returns>Retorna true caso existam return sheets vigentes, do contrário retorna false.</returns>
        bool PossuiReturnsVigentesQuantidadesVazias();

        /// <summary>
        /// Registra log indicando que usuário visualizou o aviso de que existem return sheets vigentes que ainda não foram solicitadas quantidades considerando o papel do usuário e loja que está logado.
        /// </summary>
        void RegistrarLogAvisoReturnSheetsVigentes();
    }
}
