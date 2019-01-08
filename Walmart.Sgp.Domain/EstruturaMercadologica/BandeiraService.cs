using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.Gerenciamento.Specs;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Serviço de domínio relacionado a cwi domain.
    /// </summary>
    public class BandeiraService : EntityDomainServiceBase<Bandeira, IBandeiraGateway>, IBandeiraService
    {
        #region Fields
        private readonly IPermissaoService m_permissaoService;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="BandeiraService"/>.
        /// </summary>
        /// <param name="mainGateway">O table data gateway para bandeira.</param>
        /// <param name="permissaoService">O serviço de permissões.</param>
        public BandeiraService(IBandeiraGateway mainGateway, IPermissaoService permissaoService)
            : base(mainGateway)
        {
            m_permissaoService = permissaoService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Conta o número de bandeiras por usuário.
        /// </summary>
        /// <param name="idUsuario">O id de usuário.</param>
        /// <returns>
        /// O número de bandeira.
        /// </returns>
        public long ContarPorUsuario(int idUsuario)
        {
            return MainGateway.Count("IdUsuario = @idUsuario", new { idUsuario });
        }

        /// <summary>
        /// Obtém as bandeiras associadas ao sistema informado, que o usuário informado tem acesso.
        /// </summary>
        /// <param name="idUsuario">O id do usuário.</param>
        /// <param name="idSistema">O id do sistema.</param>
        /// <param name="idFormato">O id do formato.</param>
        /// <returns>A lista de bandeiras ativas do sistema informado, que o usuário tem acesso.</returns>
        public IEnumerable<BandeiraResumo> ObterPorUsuarioESistema(int idUsuario, int? idSistema, int? idFormato)
        {
            Assert(new { User = idUsuario }, new AllMustBeInformedSpec());

            return this.MainGateway.ObterPorUsuarioESistema(idUsuario, idSistema, idFormato, null);
        }

        /// <summary>
        /// Obtém as bandeiras associadas ao sistema informado, que o usuário informado tem acesso.
        /// </summary>
        /// <param name="idUsuario">O id do usuário.</param>
        /// <param name="idSistema">O id do sistema.</param>
        /// <param name="idRegiaoAdministrativa">O id da região administrativa.</param>
        /// <returns>A lista de bandeiras ativas do sistema informado, que o usuário tem acesso.</returns>
        public IEnumerable<BandeiraResumo> ObterPorUsuarioERegiaoAdministrativa(int idUsuario, int idSistema, int? idRegiaoAdministrativa)
        {
            Assert(new { User = idUsuario }, new AllMustBeInformedSpec());

            return this.MainGateway.ObterPorUsuarioESistema(idUsuario, idSistema, null, idRegiaoAdministrativa);
        }

        /// <summary>
        /// Pesquisa bandeiras pelo filtro informado.
        /// </summary>
        /// <param name="filtro">O filtro de bandeira.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>As bandeiras.</returns>
        public IEnumerable<Bandeira> PesquisarPorFiltros(BandeiraFiltro filtro, Paging paging)
        {
            Assert(new { MarketingStructure = filtro.CdSistema }, new AllMustBeInformedSpec());

            return MainGateway.PesquisarPorFiltros(filtro, paging);
        }

        /// <summary>
        /// Obtém a bandeira estruturada pelo id.
        /// </summary>
        /// <param name="id">O id da bandeira desejada.</param>
        /// <returns>A bandeira.</returns>
        public Bandeira ObterEstruturadoPorId(int id)
        {
            return MainGateway.ObterEstruturadoPorId(id);
        }

        /// <summary>
        /// Salva a entidade informada
        /// </summary>
        /// <param name="entidade">A entidade a ser salva.</param>
        public override void Salvar(Bandeira entidade)
        {
            Assert(new { dsBandeira = entidade.DsBandeira, sgBandeira = entidade.SgBandeira, format = entidade.Formato }, new AllMustBeInformedSpec());
            Assert(entidade.Regioes, new RegiaoDeveTerDistritosSpec());

            if (entidade.Formato != null)
            {
                entidade.CdSistema = entidade.Formato.cdSistema;
            }

            if (entidade.IsNew)
            {
                entidade.StampInsert();

                // Tem que carimbar os campos de update toda vez, pois o legado da erro no login se esses campos não estiverem preenchido.
                // TODO: remover quando o legado for desativado.
                entidade.StampUpdate();

                MainGateway.Insert(entidade);
                m_permissaoService.InserirPermissaoBandeira(RuntimeContext.Current.User.Id, entidade.IDBandeira);
            }
            else
            {
                entidade.StampUpdate();
                MainGateway.Update(entidade);
            }
        }

        /// <summary>
        /// Remove a bandeira com o id informado e as permissões associadas.
        /// </summary>
        /// <param name="id">O id da entidade a ser removida.</param>
        public override void Remover(int id)
        {
            m_permissaoService.RemoverPermissoesBandeira(id);
            base.Remover(id);
        }
        #endregion
    }
}
