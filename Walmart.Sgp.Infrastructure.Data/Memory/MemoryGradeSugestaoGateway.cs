using System.Collections.Generic;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.Memory
{
    /// <summary>
    /// Implementação de um table data gateway para grade se sugestão em memória.
    /// </summary>
    /// <remarks>
    /// Essa implementação deve ser utilizada apenas para fins de testes unitários.
    /// </remarks>
    public class MemoryGradeSugestaoGateway : MemoryDataGateway<GradeSugestao>, IGradeSugestaoGateway
    {
        /// <summary>
        /// Verifica se a grade de sugestões para os parâmetros informados está aberta.
        /// </summary>
        /// <param name="cdSistema">O código de sistema.</param>
        /// <param name="idBandeira">O id de bandeira.</param>
        /// <param name="idDepartamento">O id de departamento.</param>
        /// <param name="idLoja">O id de loja.</param>
        /// <param name="vlHoraLimite">O valor de hora-minuto (HHMM)</param>
        /// <returns>
        /// True se a grade de sugestões estiver aberta.
        /// </returns>        
        public bool ExisteGradeSugestaoAberta(int cdSistema, int idBandeira, int idDepartamento, int idLoja, int vlHoraLimite)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Pesquisa estruturado por filtro.
        /// </summary>
        /// <param name="cdSistema">O código de sistema.</param>
        /// <param name="idBandeira">O id de bandeira.</param>
        /// <param name="cdDepartamento">O código de departamento.</param>
        /// <param name="cdLoja">O código de loja.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>
        /// Os registros que satisfasem o filtro.
        /// </returns>        
        public IEnumerable<GradeSugestao> PesquisarEstruturadoPorFiltro(int cdSistema, int? idBandeira, int? cdDepartamento, int? cdLoja, Paging paging)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Obtém a grade de sugestão junto com seus relacionamentos.
        /// </summary>
        /// <param name="id">O id da grade de sugestão.</param>
        /// <returns>
        /// A grade de sugestão.
        /// </returns>        
        public GradeSugestao ObterEstruturadoPorId(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}