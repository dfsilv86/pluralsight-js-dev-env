using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Reabastecimento.CompraCasada
{
    /// <summary>
    /// Define a interface de um table data gateway para CompraCasada.
    /// </summary>
    public interface ICompraCasadaGateway : IDataGateway<CompraCasada>
    {
        /// <summary>
        /// Obtem o codigo do item pai pelo codigo de um item filho.
        /// </summary>
        /// <param name="cdItemFilho">O codigo do item filho.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <returns>Retorna zero se nao encontrar pai ou o codigo do item pai se encontrar.</returns>
        int ObterCodItemPaiPorCodItemFilho(long cdItemFilho, long cdSistema);

        /// <summary>
        /// Pesquisar Itens de Entrada para Cadastro de Compra Casada.
        /// </summary>
        /// <param name="filtro">O filtro da pesquisa.</param>
        /// <param name="paging">A paginação da pesquisa.</param>
        /// <returns>Um IEnumerable de ItemDetalhe populados a partir dos filtros informados.</returns>
        IEnumerable<ItemDetalhe> PesquisarItensEntrada(PesquisaCompraCasadaFiltro filtro, Paging paging);

        /// <summary>
        /// Pesquisar Itens para Compra Casada.
        /// </summary>
        /// <param name="filtro">O filtro da pesquisa.</param>
        /// <param name="paging">A paginação da pesquisa.</param>
        /// <returns>Um IEnumerable de ItemDetalhe populados a partir dos filtros informados.</returns>
        IEnumerable<ItemDetalhe> PesquisarItensCompraCasada(PesquisaCompraCasadaFiltro filtro, Paging paging);

        /// <summary>
        /// Verifica se a compra casada possui cadastro.
        /// </summary>
        /// <param name="filtros">Os filtros da compra casada.</param>
        /// <returns>Se existe ou nao cadastro.</returns>
        bool PossuiCadastro(PesquisaCompraCasadaFiltro filtros);
    }
}
