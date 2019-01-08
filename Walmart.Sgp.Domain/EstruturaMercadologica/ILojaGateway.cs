﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Domain.Acessos;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Define a interface de um table data gateway para loja.
    /// </summary>
    public interface ILojaGateway : IDataGateway<Loja>
    {
        /// <summary>
        /// Obter por usuário e sistema.
        /// </summary>
        /// <param name="idUsuario">O id de usuário.</param>
        /// <param name="idBandeira">O id da bandeira.</param>
        /// <param name="cdLoja">O código da loja.</param>
        /// <returns>As lojas.</returns>
        /// <remarks>Não valida o tipo de permissão do usuário.</remarks>
        Loja ObterPorLojaUsuarioEBandeira(int idUsuario, short? idBandeira, int cdLoja);

        /// <summary>
        /// Pesquisa lojas com os parâmetros especificados.
        /// </summary>
        /// <param name="idUsuario">O id do usuário.</param>
        /// <param name="tipoPermissao">O tipo de permissão do usuário.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <param name="idBandeira">O id da bandeira.</param>
        /// <param name="cdLoja">O código da loja.</param>
        /// <param name="nmLoja">O nome da loja.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>Lista de lojas que atendem aos parâmetros especificados.</returns>
        IEnumerable<Loja> Pesquisar(int idUsuario, TipoPermissao tipoPermissao, int cdSistema, int? idBandeira, int? cdLoja, string nmLoja, Paging paging);

        /// <summary>
        /// Pesquisa lojas por item destino e origem
        /// </summary>
        /// <param name="filtro">Os filtros</param>
        /// <param name="paging">A paginacao</param>
        /// <returns>As lojas</returns>
        IEnumerable<Loja> PesquisarLojasPorItemDestinoItemOrigem(LojaFiltro filtro, Paging paging);

        /// <summary>
        /// Obtem as informações da loja e entidades associadas.
        /// </summary>
        /// <param name="idLoja">O id da loja.</param>
        /// <returns>A loja.</returns>
        Loja ObterEstruturadoPorId(int idLoja);
    }
}
