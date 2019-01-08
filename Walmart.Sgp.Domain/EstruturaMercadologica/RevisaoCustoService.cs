using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Serviço de domínio para revisao custo.
    /// </summary>
    public class RevisaoCustoService : EntityDomainServiceBase<RevisaoCusto, IRevisaoCustoGateway>, IRevisaoCustoService
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="RevisaoCustoService"/>.
        /// </summary>
        /// <param name="mainGateway">O table data gateway para revisao custo.</param>
        public RevisaoCustoService(IRevisaoCustoGateway mainGateway)
            : base(mainGateway)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obtém uma revisao de custo pelo seu id e retorna a rebisao de custo com informações das entidades associadas.
        /// </summary>
        /// <param name="idRevisaoCusto">O id da revisao de custo.</param>
        /// <returns>A RevisaoCusto com informações de Loja, ItemDetalhe, StatusRevisaoCusto, MotivoRevisaoCusto, Departamento.</returns>
        public RevisaoCusto ObterEstruturadoPorId(int idRevisaoCusto)
        {
            return MainGateway.ObterEstruturadoPorId(idRevisaoCusto);
        }
     
        /// <summary>
        /// Pesquisa detalhe de revisoes de custo pelos filtros informados.
        /// </summary>
        /// <param name="filtro">O filtro</param>
        /// <param name="paging">A paginação</param>
        /// <returns>As revisoes de custos</returns>
        public IEnumerable<RevisaoCusto> PesquisarPorFiltros(RevisaoCustoFiltro filtro, Paging paging)
        {
            return MainGateway.PesquisarPorFiltros(filtro, paging);
        }

        /// <summary>
        /// Salva a entidade informada
        /// </summary>
        /// <param name="entidade">A entidade a ser salva.</param>
        public override void Salvar(RevisaoCusto entidade)
        {
            if (entidade.IsNew)
            {
                entidade.dtCriacao = DateTime.Now;
                entidade.IDUsuarioSolicitante = RuntimeContext.Current.User.Id;
            }

            Assert(new { entidade.IDLoja, ItemCode = entidade.IDItemDetalhe, entidade.IDStatusRevisaoCusto, entidade.IDMotivoRevisaoCusto, entidade.IDUsuarioSolicitante, entidade.dtCriacao }, new AllMustBeInformedSpec());
            Assert(new { entidade.vlCustoSolicitado }, new AllMustBeInformedSpec(true));

            base.Salvar(entidade);
        }
        #endregion        
    }
}
