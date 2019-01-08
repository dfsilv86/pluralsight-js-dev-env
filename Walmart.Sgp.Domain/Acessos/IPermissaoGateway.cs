using System.Collections.Generic;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Acessos
{
    /// <summary>
    /// Define a interface de um table data gateway para permissão.
    /// </summary>
    public interface IPermissaoGateway : IDataGateway<Permissao>
    {
        /// <summary>
        /// Pesquisa permissões utilizando os filtros informados.
        /// </summary>
        /// <param name="idUsuario">O id de usuário.</param>
        /// <param name="idBandeira">O id de bandeira.</param>
        /// <param name="idLoja">O id de loja.</param>
        /// <returns>As permissões.</returns>
        IEnumerable<Permissao> Pesquisar(int? idUsuario, int? idBandeira, int? idLoja);

        /// <summary>
        /// Obtém as permissões do usuário informado.
        /// </summary>
        /// <param name="idUsuario">O id do usuário.</param>
        /// <returns>As permissões do usuário.</returns>
        UsuarioPermissoes ObterPermissoesDoUsuario(int idUsuario);

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
        /// Obtém a permissão pelo seu id.
        /// </summary>
        /// <param name="idPermissao">O id da permissão.</param>
        /// <returns>A permissão.</returns>
        Permissao ObterPorId(int idPermissao);

        /// <summary>
        /// Verifica se o usuário possui permissão para uma determinada loja.
        /// </summary>
        /// <param name="idUsuario">O id de usuário.</param>
        /// <param name="idLoja">O id da loja.</param>
        /// <returns>True se o usuário possui permissão para a loja, através de permissão específica na loja (PermissaoLoja) ou através da bandeira da loja (PermissaoBandeira).</returns>
        bool PossuiPermissaoLoja(int idUsuario, int idLoja);
    }
}
