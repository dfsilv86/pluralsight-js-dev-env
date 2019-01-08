using System.Collections.Generic;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Domain.Acessos
{
    /// <summary>
    /// Define a interface de um serviço de permissão.
    /// </summary>
    public interface IPermissaoService : IDomainService<Permissao>
    {
        /// <summary>
        /// Conta as permissões por usuário.
        /// </summary>
        /// <param name="idUsuario">O id de usuário.</param>
        /// <returns>O número de permissões do usuário.</returns>
        long ContarPermissoesPorUsuario(int idUsuario);
   
        /// <summary>
        /// Insere a permissão para o usuário na bandeira informada.
        /// </summary>
        /// <param name="idUsuario">O id do usuário.</param>
        /// <param name="idBandeira">O id da bandeira.</param>
        /// <returns>A permissão de bandeira inserida.</returns>
        PermissaoBandeira InserirPermissaoBandeira(int idUsuario, int idBandeira);

        /// <summary>
        /// Pesquisa permissões utilizando os filtros informados.
        /// </summary>
        /// <param name="idUsuario">O id de usuário.</param>
        /// <param name="idBandeira">O id de bandeira.</param>
        /// <param name="idLoja">O id de loja.</param>
        /// <returns>As permissõe.</returns>
        IEnumerable<Permissao> Pesquisar(int? idUsuario, int? idBandeira, int? idLoja);

        /// <summary>
        /// Obtém as permissões do usuário informado.
        /// </summary>
        /// <param name="idUsuario">O id do usuário.</param>
        /// <returns>As permissões do usuário.</returns>
        UsuarioPermissoes ObterPermissoesDoUsuario(int idUsuario);

        /// <summary>
        /// Remove todas as permissões associadas a bandeira informada.
        /// </summary>
        /// <param name="idBandeira">O id da bandeira.</param>
        void RemoverPermissoesBandeira(int idBandeira);

        /// <summary>
        /// Pesquisa permissões utilizando os filtros informados retornando objetos Usuario, Bandeira e Loja preenchidos.
        /// </summary>
        /// <param name="idUsuario">O id de usuário.</param>
        /// <param name="idBandeira">O id de bandeira.</param>
        /// <param name="idLoja">O id de loja.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>As permissões.</returns>
        IEnumerable<Permissao> PesquisarComFilhos(int? idUsuario, int? idBandeira, int? idLoja, Paging paging);
     
        /// <summary>
        /// Verifica se a bandeira está válida para inclusão na permissão.
        /// </summary>
        /// <param name="idBandeira">O id da bandeira.</param>
        void ValidarInclusaoBandeira(int idBandeira);

        /// <summary>
        /// Verifica se a loja está válida para inclusão na permissão.
        /// </summary>
        /// <param name="usuario">O usuário logado.</param>
        /// <param name="idLoja">O id da loja.</param>
        void ValidarInclusaoLoja(IRuntimeUser usuario, int idLoja);

        /// <summary>
        /// Verifica se o usuário logado possui permissão para manutenção da permissão.
        /// </summary>
        /// <param name="usuario">O usuário logado.</param>
        /// <returns>Retorna true caso o usuário possua permissão, do contrário retorna false.</returns>
        bool PossuiPermissaoManutencao(IRuntimeUser usuario);

        /// <summary>
        /// Verifica se o usuário possui permissão para uma determinada loja.
        /// </summary>
        /// <param name="idUsuario">O id de usuário.</param>
        /// <param name="idLoja">O id da loja.</param>
        /// <returns>True se o usuário possui permissão para a loja, através de permissão específica na loja (PermissaoLoja) ou através da bandeira da loja (PermissaoBandeira).</returns>
        bool PossuiPermissaoLoja(int idUsuario, int idLoja);

        /// <summary>
        /// Obtém um valor que indica se o usuário informado tem acesso de administrador master.
        /// </summary>
        /// <remarks>
        /// Essa permissão é diferente do Papel.IsAdmin, pois essa permissão é referente a um único usuário que é o admin master do sistema.
        /// </remarks>
        /// <param name="idUsuario">O id de usuário.</param>
        /// <returns>True se tem acesso de administrador, false no contrário.</returns>
        bool TemAcessoAdminMaster(int idUsuario);
    }
}
