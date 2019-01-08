using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.Memory
{
    /// <summary>
    /// Implementação de um table data gateway para loja de inventário em memória.
    /// </summary>
    /// <remarks>
    /// Essa implementação deve ser utilizada apenas para fins de testes unitários.
    /// </remarks>
    public class MemoryLojaGateway : MemoryDataGateway<Loja>, ILojaGateway
    {
        /// <summary>
        /// Obter por usuário e sistema.
        /// </summary>
        /// <param name="idUsuario">O id de usuário.</param>
        /// <param name="idBandeira">O id da bandeira.</param>
        /// <param name="cdLoja">O código da loja.</param>
        /// <returns>As lojas.</returns>
        /// <remarks>Não valida o tipo de permissão do usuário.</remarks>
        public Loja ObterPorLojaUsuarioEBandeira(int idUsuario, short? idBandeira, int cdLoja)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Pesquisa lojas com os parâmetros especificados.
        /// </summary>
        /// <param name="idUsuario">O id do usuário.</param>
        /// <param name="tipoPermissao">O tipo de permissão do usuário.</param>
        /// <param name="cdSistema">O código do sistema.</param>
        /// <param name="idBandeira">O id da bandeira.</param>
        /// <param name="cdLoja">O código da loja.</param>
        /// <param name="nmLoja">O nome da loja.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>Lista de lojas que atendem aos parâmetros especificados.</returns>
        public IEnumerable<Loja> Pesquisar(int idUsuario, Framework.Domain.Acessos.TipoPermissao tipoPermissao, int cdSistema, int? idBandeira, int? cdLoja, string nmLoja, Paging paging)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Pesquisa lojas por item destino e origem
        /// </summary>
        /// <param name="filtro">Os filtros</param>
        /// <param name="paging">A paginacao</param>
        /// <returns>As lojas</returns>
        public IEnumerable<Loja> PesquisarLojasPorItemDestinoItemOrigem(LojaFiltro filtro, Paging paging)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtem as informações da loja e entidades associadas.
        /// </summary>
        /// <param name="idLoja">O id da loja.</param>
        /// <returns>
        /// A loja.
        /// </returns>
        public Loja ObterEstruturadoPorId(int idLoja)
        {
            throw new NotImplementedException();
        }
    }
}