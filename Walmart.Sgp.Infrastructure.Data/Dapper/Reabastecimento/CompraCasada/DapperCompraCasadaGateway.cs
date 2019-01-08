using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.MultisourcingVendor;
using Walmart.Sgp.Domain.Reabastecimento.CompraCasada;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para CompraCasada utilizando o Dapper.
    /// </summary>
    public class DapperCompraCasadaGateway : EntityDapperDataGatewayBase<CompraCasada>, ICompraCasadaGateway
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperCompraCasadaGateway" />.
        /// </summary>
        /// <param name="databases">O registro de bases de dados da aplicação.</param>
        public DapperCompraCasadaGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "CompraCasada", "IDCompraCasada")
        {
        }

        /// <summary>
        /// Obtém o nome das colunas que devem ser consideradas nas operações de SELECT, INSERT e UPDATE.
        /// </summary>
        protected override IEnumerable<string> ColumnsName
        {
            get
            {
                return new string[] { "idItemDetalheSaida", "idFornecedorParametro", "idItemDetalheEntrada", "blItemPai", "blAtivo" };
            }
        }

        /// <summary>
        /// Obtem o codigo do item pai pelo codigo de um item filho.
        /// </summary>
        /// <param name="cdItemFilho">O codigo do item filho.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <returns>Retorna zero se nao encontrar pai ou o codigo do item pai se encontrar.</returns>
        public int ObterCodItemPaiPorCodItemFilho(long cdItemFilho, long cdSistema)
        {
            var result = this.Resource.ExecuteScalar<int?>(Sql.CompraCasada.ObterCdPaiPorCdFilho, new { cdItemFilho, cdSistema });
            if (result.HasValue && result.Value > 0)
            {
                return result.Value;
            }

            return 0;
        }

        /// <summary>
        /// Verifica se a compra casada possui cadastro.
        /// </summary>
        /// <param name="filtros">Os filtros da compra casada.</param>
        /// <returns>Se existe ou nao cadastro.</returns>
        public bool PossuiCadastro(PesquisaCompraCasadaFiltro filtros)
        {
            var result = this.Resource.ExecuteScalar<int>(Sql.CompraCasada.PesquisarItensEntrada_Count, filtros);
            return result > 0;
        }

        /// <summary>
        /// Pesquisar Itens de Entrada para Cadastro de Compra Casada.
        /// </summary>
        /// <param name="filtro">O filtro da pesquisa.</param>
        /// <param name="paging">A paginação da pesquisa.</param>
        /// <returns>Um IEnumerable de ItemDetalhe populados a partir dos filtros informados.</returns>
        public IEnumerable<ItemDetalhe> PesquisarItensEntrada(PesquisaCompraCasadaFiltro filtro, Paging paging)
        {
            var result = this.Resource.Query<Departamento, dynamic, FornecedorParametro, ItemDetalhe, Multisourcing, ItemDetalhe>(
                Sql.CompraCasada.PesquisarItensEntrada,
                filtro,
                MapItensEntrada,
                "CdItemSaida,SplitOn2,SplitOn3,SplitOn4");

            return paging == null ? result : result.AsPaging(paging);
        }

        /// <summary>
        /// Pesquisar Itens para Compra Casada.
        /// </summary>
        /// <param name="filtro">O filtro da pesquisa.</param>
        /// <param name="paging">A paginação da pesquisa.</param>
        /// <returns>Um IEnumerable de ItemDetalhe populados a partir dos filtros informados.</returns>
        public IEnumerable<ItemDetalhe> PesquisarItensCompraCasada(PesquisaCompraCasadaFiltro filtro, Paging paging)
        {
            return this.Resource.Query<Departamento, FornecedorParametro, ItemDetalhe, ItemDetalhe>(
                Sql.CompraCasada.Pesquisar,
                filtro,
                MapItensCompraCasada,
                "SplitOn1,SplitOn2")
             .AsPaging(paging);
        }

        private static ItemDetalhe MapItensCompraCasada(Departamento departamento, FornecedorParametro fornecedorParametro, ItemDetalhe itemDetalhe)
        {
            itemDetalhe.Departamento = departamento;
            itemDetalhe.FornecedorParametro = fornecedorParametro;
            return itemDetalhe;
        }

        private static ItemDetalhe MapItensEntrada(Departamento departamento, dynamic itemSaida, FornecedorParametro fornecedorParametro, ItemDetalhe itemEntrada, Multisourcing multisourcing)
        {
            itemEntrada.Departamento = departamento;
            itemEntrada.FornecedorParametro = fornecedorParametro;
            itemEntrada.ItemSaida = PopulaItem(itemSaida);
            itemEntrada.Multisourcing = multisourcing;

            return itemEntrada;
        }

        private static ItemDetalhe PopulaItem(dynamic itemSaida)
        {
            var item = new ItemDetalhe()
            {
                DsItem = itemSaida.DsItemSaida,
                CdItem = itemSaida.CdItemSaida,
                IDItemDetalhe = (int)itemSaida.IdItemDetalheSaida
            };

            return item;
        }
    }
}