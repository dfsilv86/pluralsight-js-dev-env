using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Serviço para parâmetro de loja/CD.
    /// </summary>
    public class LojaCdParametroService : EntityDomainServiceBase<LojaCdParametro, ILojaCdParametroGateway>, ILojaCdParametroService
    {
        #region Fields
        private readonly ICDService m_cdService;
        #endregion

        #region Constructor
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="LojaCdParametroService"/>
        /// </summary>
        /// <param name="mainGateway">O table data gateway principal.</param>
        /// <param name="cdService">O serviço de CD.</param>
        public LojaCdParametroService(ILojaCdParametroGateway mainGateway, ICDService cdService)
            : base(mainGateway)
        {
            m_cdService = cdService;
        }
        #endregion

        /// <summary>
        /// Pesquisa parâmetros de loja/CD pelos filtros informados.
        /// </summary>
        /// <param name="filtro">Os filtros.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>Os parâmetros de loja/CD.</returns>
        public IEnumerable<LojaCdParametroPorDepartamento> PesquisarPorFiltros(LojaCdParametroFiltro filtro, Paging paging)
        {
            return MainGateway.PesquisarPorFiltros(filtro, paging);
        }

        /// <summary>
        /// Obtém o LojaCdParametro com o id informado.
        /// </summary>
        /// <param name="idLojaCdParametro">O id.</param>
        /// <param name="tpReabastecimento">O tipo de reabastecimento.</param>
        /// <returns>O LojaCdParametro.</returns>
            public LojaCdParametro ObterEstruturadoPorId(int idLojaCdParametro, TipoReabastecimento tpReabastecimento)
        {
            return MainGateway.ObterEstruturadoPorId(idLojaCdParametro, tpReabastecimento);
        }

        /// <summary>
        /// Salva a entidade informada
        /// </summary>
        /// <param name="entidade">A entidade a ser salva.</param>
        public override void Salvar(LojaCdParametro entidade)
        {
            Assert(
                new { CDName = entidade.CD.nmNome, entidade.tpWeek, entidade.tpInterval },
                new AllMustBeInformedSpec());

            entidade.Stamp();

            if (entidade.IsNew)
            {
                MainGateway.Insert(entidade);
            }
            else
            {
                // Apenas as propriedades que podem ser atualizadas.
                MainGateway.Update(
                    @"tpWeek = @tpWeek, 
                      tpInterval = @tpInterval, 
                      tpPedidoMinimo = @tpPedidoMinimo, 
                      tpProduto = @tpProduto, 
                      vlFillRate = @vlFillRate, 
                      vlValorMinimo = @vlValorMinimo,
                      dhAtualizacao = @DhAtualizacao,
                      cdUsuarioAtualizacao = @CdUsuarioAtualizacao",
                    entidade);
            }
            
            // Atualiza o nome do CD.
            var cd = entidade.CD;
            m_cdService.AtualizarNomeCD(cd.Id, cd.nmNome);

        }
    }
}
