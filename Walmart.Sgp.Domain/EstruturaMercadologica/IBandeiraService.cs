using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Interface para serviços de CWIDomain.
    /// </summary>
    public interface IBandeiraService : IDomainService<Bandeira>
    {
        /// <summary>
        /// Obter por usuário e sistema.
        /// </summary>
        /// <param name="idUsuario">O id de usuário.</param>
        /// <param name="idSistema">O id de sistema.</param>
        /// <param name="idFormato">O id do formato.</param>
        /// <returns>As bandeiras (resumo).</returns>
        IEnumerable<BandeiraResumo> ObterPorUsuarioESistema(int idUsuario, int? idSistema, int? idFormato);

        /// <summary>
        /// Obter por usuário e região administrativa.
        /// </summary>
        /// <param name="idUsuario">O id de usuário.</param>
        /// <param name="idSistema">O id de sistema.</param>
        /// <param name="idRegiaoAdministrativa">O id da região administrativa.</param>
        /// <returns>As bandeiras (resumo).</returns>
        IEnumerable<BandeiraResumo> ObterPorUsuarioERegiaoAdministrativa(int idUsuario, int idSistema, int? idRegiaoAdministrativa);

        /// <summary>
        /// Conta o número de bandeiras por usuário.
        /// </summary>
        /// <param name="idUsuario">O id de usuário.</param>
        /// <returns>O número de bandeira.</returns>
        long ContarPorUsuario(int idUsuario);

        /// <summary>
        /// Pesquisa bandeiras pelo filtro informado.
        /// </summary>
        /// <param name="filtro">O filtro de bandeira.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>As bandeiras.</returns>
        IEnumerable<Bandeira> PesquisarPorFiltros(BandeiraFiltro filtro, Paging paging);

        /// <summary>
        /// Obtém a bandeira estruturada pelo id.
        /// </summary>
        /// <param name="id">O id da bandeira desejada.</param>
        /// <returns>A bandeira.</returns>
        Bandeira ObterEstruturadoPorId(int id);
    }
}
