using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.Memory
{
    /// <summary>
    /// Implementação de um table data gateway para bandeira em memória.
    /// </summary>
    /// <remarks>
    /// Essa implementação deve ser utilizada apenas para fins de testes unitários.
    /// </remarks>
    public class MemoryPermissaoGateway : MemoryDataGateway<Permissao>, IPermissaoGateway
    {
        /// <summary>
        /// Pesquisa permissões utilizando os filtros informados.
        /// </summary>
        /// <param name="idUsuario">O id de usuário.</param>
        /// <param name="idBandeira">O id de bandeira.</param>
        /// <param name="idLoja">O id de loja.</param>
        /// <returns>As permissões.</returns>
        public IEnumerable<Permissao> Pesquisar(int? idUsuario, int? idBandeira, int? idLoja)
        {
            return from p in Entities
                   where
                   (!idUsuario.HasValue || p.IDUsuario == idUsuario.Value) &&
                   (!idBandeira.HasValue || p.Bandeiras.Any(b => b.IDBandeira == idBandeira.Value)) &&
                   (!idLoja.HasValue || p.Lojas.Any(b => b.IDLoja == idLoja.Value))
                   select p;
        }

        /// <summary>
        /// Obtém as permissões do usuário informado.
        /// </summary>
        /// <param name="idUsuario">O id do usuário.</param>
        /// <returns>As permissões do usuário.</returns>
        public UsuarioPermissoes ObterPermissoesDoUsuario(int idUsuario)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Pesquisa permissões utilizando os filtros informados retornando objetos Usuario, Bandeira e Loja preenchidos.
        /// </summary>
        /// <param name="idUsuario">O id de usuário.</param>
        /// <param name="idBandeira">O id de bandeira.</param>
        /// <param name="idLoja">O id de loja.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>As permissões.</returns>
        public IEnumerable<Permissao> PesquisarComFilhos(int? idUsuario, int? idBandeira, int? idLoja, Paging paging)
        {
            return Pesquisar(idUsuario, idBandeira, idLoja);
        }

        /// <summary>
        /// Obtém a permissão pelo seu id.
        /// </summary>
        /// <param name="idPermissao">O id da permissão.</param>
        /// <returns>A permissão.</returns>
        public Permissao ObterPorId(int idPermissao)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Verifica se o usuário possui permissão para uma determinada loja.
        /// </summary>
        /// <param name="idUsuario">O id de usuário.</param>
        /// <param name="idLoja">O id da loja.</param>
        /// <returns>True se o usuário possui permissão para a loja, através de permissão específica na loja (PermissaoLoja) ou através da bandeira da loja (PermissaoBandeira).</returns>
        public bool PossuiPermissaoLoja(int idUsuario, int idLoja)
        {
            throw new NotImplementedException();
        }
    }
}
