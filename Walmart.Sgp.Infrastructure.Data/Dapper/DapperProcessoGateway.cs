using System.Collections.Generic;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.Processos;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para processo utilizando o Dapper.
    /// </summary>
    public class DapperProcessoGateway : EntityDapperDataGatewayBase<Processo>, IProcessoGateway
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperProcessoGateway"/>.
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        public DapperProcessoGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "LogTipoProcesso", "IdProcesso")
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém o nome das colunas que devem ser consideradas nas operações de SELECT, INSERT e UPDATE.
        /// </summary>
        protected override IEnumerable<string> ColumnsName
        {
            get
            {
                return new string[]
                {
                    "Descricao",
                    "DiasProcessar",
                    "IDGrupoEnvioEmail",
                };
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Pesquisa as cargas de processos de lojas que combinam com o filtro informado.
        /// </summary>
        /// <param name="filtro">O filtro de pesquisa.</param>
        /// <param name="paging">A paginação a ser utililizada no resultado.</param>
        /// <returns>As cargas de processos de lojas.</returns>
        public IEnumerable<LojaProcessosCarga> PesquisarCargas(ProcessoCargaFiltro filtro, Paging paging)
        {
            var result = new Dictionary<int, LojaProcessosCarga>();
            filtro.Data = filtro.Data.Date;

            return Resource.Query<Bandeira, Loja, ProcessoCarga, ProcessoCargaErro, LojaProcessosCarga>(
                Sql.Processo.PesquisarCargas_Paging,
                filtro,
                (b, l, c, e) =>
                {
                    if (!result.ContainsKey(l.cdLoja))
                    {
                        result.Add(l.cdLoja, new LojaProcessosCarga(filtro.Data, b, l));
                    }

                    if (!string.IsNullOrEmpty(e.Descricao))
                    {
                        c.Erro = e;
                    }

                    var lpc = result[l.cdLoja];                    
                    lpc.Cargas.Add(c);

                    return lpc;
                },
                "SplitOn1,SplitOn2,SplitOn3")
                .AsPaging(paging, Sql.Processo.PesquisarCargas_Paging, Sql.Processo.PesquisarCargas_Count)
                .AsMemoryResult(result.Values);
        }

        /// <summary>
        /// Pesquisa processos de execução que combinam com o filtro informado.
        /// </summary>
        /// <param name="filtro">O filtro de pesquisa.</param>
        /// <param name="paging">A paginação a ser utililizada no resultado.</param>
        /// <returns>Os processos de execução.</returns>
        public IEnumerable<ProcessoExecucao> PesquisarProcessosExecucao(ProcessoExecucaoFiltro filtro, Paging paging)
        {
            var args = new
            {
                logDataInicio = filtro.Data.StartValue.Value,
                logDataFim = filtro.Data.EndValue.Value,
                idProcesso = filtro.IdProcesso,
                cdSistema = filtro.CdSistema,
                idBandeira = filtro.IdBandeira,
                idLoja = filtro.IdLoja,
                idItemDetalhe = filtro.IdItemDetalhe
            };

            return Resource.Query<ProcessoExecucao, ItemDetalhe, Loja, ProcessoTipoExcecao, dynamic, ProcessoExecucao>(
                Sql.Processo.PesquisarProcessosExecucao,
                args,
                (pe, id, lj, pte, p) =>
                {
                    pe.ItemDetalhe = id;
                    pe.Loja = lj;
                    pe.ProcessoTipoExcecao = pte;
                    pe.Processo = new Processo { Descricao = p.DescricaoProcesso };

                    return pe;
                },
                "SplitOn1,SplitOn2,SplitOn3,DescricaoProcesso").AsPaging(paging);
        }
        #endregion
    }
}
