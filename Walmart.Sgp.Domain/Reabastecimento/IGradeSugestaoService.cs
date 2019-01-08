using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Define a interface de um serviço de grade sugestao.
    /// </summary>
    public interface IGradeSugestaoService : IDomainService<GradeSugestao>
    {
        /// <summary>
        /// Verifica se a grade de sugestões para os parâmetros informados está aberta.
        /// </summary>
        /// <param name="cdSistema">O código de sistema.</param>
        /// <param name="idBandeira">O id de bandeira.</param>
        /// <param name="idDepartamento">O id de departamento.</param>
        /// <param name="idLoja">O id de loja.</param>
        /// <param name="vlHoraLimite">O valor de hora-minuto (HHMM)</param>
        /// <returns>True se a grade de sugestões estiver aberta.</returns>
        bool ExisteGradeSugestaoAberta(int cdSistema, int idBandeira, int idDepartamento, int idLoja, int vlHoraLimite);

        /// <summary>
        /// Pesquisa estruturado por filtro.
        /// </summary>
        /// <param name="cdSistema">O código de sistema.</param>
        /// <param name="idBandeira">O id de bandeira.</param>
        /// <param name="cdDepartamento">O código de departamento.</param>
        /// <param name="cdLoja">O código de loja.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>Os registros que satisfasem o filtro.</returns>
        IEnumerable<GradeSugestao> PesquisarEstruturadoPorFiltro(int cdSistema, int? idBandeira, int? cdDepartamento, int? cdLoja, Paging paging);

        /// <summary>
        /// Obtém a grade de sugestão junto com seus relacionamentos.
        /// </summary>
        /// <param name="id">O id da grade de sugestão.</param>
        /// <returns>A grade de sugestão.</returns>
        GradeSugestao ObterEstruturadoPorId(int id);

        /// <summary>
        /// Atualiza a grade de sugestão especificada.
        /// </summary>
        /// <param name="gradeSugestao">A grade de sugestão.</param>
        void Atualizar(GradeSugestao gradeSugestao);   

        /// <summary>
        /// Salva as novas sugestões.
        /// </summary>
        /// <param name="sugestoes">As sugestões.</param>
        void SalvarNovas(IEnumerable<GradeSugestao> sugestoes);

        /// <summary>
        /// Conta quantas sugestoes existem com a mesma configuração.
        /// </summary>
        /// <param name="cdSistema">O código do sistema.</param>
        /// <param name="idBandeira">O id da bandeira.</param>
        /// <param name="idLoja">O id da loja.</param>
        /// <param name="idDepartamento">O id do departamento</param>
        /// <returns>A quantidade de sugestoes que existem com a mesma configuração.</returns>
        long ContarExistentes(int cdSistema, int idBandeira, int idLoja, int idDepartamento);
    }
}
