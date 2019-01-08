using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Reabastecimento.CompraCasada
{
    /// <summary>
    /// Define a interface para serviço de cadastro de CompraCasada.
    /// </summary>
    public interface ICompraCasadaService : IDomainService<CompraCasada>
    {
        /// <summary>
        /// Obtem o codigo do item pai pelo codigo de um item filho.
        /// </summary>
        /// <param name="cdItemFilho">O codigo do item filho.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <returns>Retorna zero se nao encontrar pai ou o codigo do item pai se encontrar.</returns>
        int ObterCodItemPaiPorCodItemFilho(long cdItemFilho, long cdSistema);

        /// <summary>
        /// Faz o merge dos itens que estão no frontend com os itens que estão na base.
        /// </summary>
        /// <param name="filtros">Filtro para pesquisa e os itens do frontend.</param>
        /// <returns>Itens da base + os itens do front.</returns>
        IEnumerable<ItemDetalhe> MergeItens(PesquisaCompraCasadaFiltro filtros);

        /// <summary>
        /// Verifica se um cadastro de compra casada possui pai.
        /// </summary>
        /// <param name="filtros">Os filtros e os itens em memoria.</param>
        /// <returns>Se tem ou nao pai.</returns>
        ItemDetalhe VerificaPossuiPai(PesquisaCompraCasadaFiltro filtros);

        /// <summary>
        /// Verifica se a compra casada possui cadastro.
        /// </summary>
        /// <param name="filtros">Os filtros da compra casada.</param>
        /// <returns>Se existe ou nao cadastro.</returns>
        bool VerificaPossuiCadastro(PesquisaCompraCasadaFiltro filtros);

        /// <summary>
        /// Salvar compra casada.
        /// </summary>
        /// <param name="filtros">Os filtros da compra casada e os itens alterados.</param>
        /// <returns>Uma lista de SpecResult se houver problemas, caso contrário NULL.</returns>
        IEnumerable<SpecResult> SalvarItensCompraCasada(PesquisaCompraCasadaFiltro filtros);

        /// <summary>
        /// Validações utilizadas nos itens filhos no front-end.
        /// </summary>
        /// <param name="filtros">Filtros compra casada e itens alterados.</param>
        /// <returns>Uma lista de SpecResult se houver problemas, caso contrário NULL.</returns>
        IEnumerable<SpecResult> ValidarItemFilhoMarcado(PesquisaCompraCasadaFiltro filtros);

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
        /// Excluir a compra casada.
        /// </summary>
        /// <param name="filtros">Os filtros da compra casada.</param>
        void ExcluirItensCompraCasada(PesquisaCompraCasadaFiltro filtros);

        /// <summary>
        /// Remove os vinculos de um item pai.
        /// </summary>
        /// <param name="item">O itemDetalhe pai.</param>
        void RemoverVinculoItemPai(ItemDetalhe item);
    }
}
