using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Acessos.Specs;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Acessos
{
    /// <summary>
    /// Serviço de domínio relacionado a permissão.
    /// </summary>
    public class PermissaoService : EntityDomainServiceBase<Permissao, IPermissaoGateway>, IPermissaoService
    {
        #region Fields
        private readonly IParametroService m_parametroService;
        private readonly IPermissaoBandeiraGateway m_permissaoBandeiraGateway;
        private readonly IPermissaoLojaGateway m_permissaoLojaGateway;
        private readonly IBandeiraGateway m_bandeiraGateway;
        private readonly ILojaGateway m_lojaGateway;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="PermissaoService" />.
        /// </summary>
        /// <param name="mainGateway">O table data gateway para permissão.</param>
        /// <param name="parametroService">O serviço de parâmetro.</param>
        /// <param name="permissaoBandeiraGateway">O table data gateway para permissão de bandeira.</param>
        /// <param name="permissaoLojaGateway">O table data gateway para permissão de loja.</param>
        /// <param name="bandeiraGateway">O table data gateway para bandeira.</param>
        /// <param name="lojaGateway">O table data gateway para loja.</param>
        public PermissaoService(
            IPermissaoGateway mainGateway,
            IParametroService parametroService,
            IPermissaoBandeiraGateway permissaoBandeiraGateway,
            IPermissaoLojaGateway permissaoLojaGateway,
            IBandeiraGateway bandeiraGateway,
            ILojaGateway lojaGateway)
            : base(mainGateway)
        {
            m_parametroService = parametroService;
            m_permissaoBandeiraGateway = permissaoBandeiraGateway;
            m_permissaoLojaGateway = permissaoLojaGateway;
            m_bandeiraGateway = bandeiraGateway;
            m_lojaGateway = lojaGateway;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obtém a permissão pelo id informado.
        /// </summary>
        /// <param name="id">O id.</param>
        /// <returns>A permissão.</returns>
        public override Permissao ObterPorId(int id)
        {
            return MainGateway.ObterPorId(id);
        }

        /// <summary>
        /// Conta as permissões por usuário.
        /// </summary>
        /// <param name="idUsuario">O id de usuário.</param>
        /// <returns>
        /// O número de permissões do usuário.
        /// </returns>
        public long ContarPermissoesPorUsuario(int idUsuario)
        {
            return MainGateway.Count("IdUsuario = @idUsuario", new { idUsuario });
        }

        /// <summary>
        /// Insere a permissão para o usuário na bandeira informada.
        /// </summary>
        /// <param name="idUsuario">O id do usuário.</param>
        /// <param name="idBandeira">O id da bandeira.</param>
        /// <returns>A permissão de bandeira inserida.</returns>
        public PermissaoBandeira InserirPermissaoBandeira(int idUsuario, int idBandeira)
        {
            var usuario = new Usuario { Id = idUsuario };
            Assert(usuario, new UsuarioDevePossuirPermissoesSpec(this));

            var permissaoUsuario = MainGateway.Find("IdUsuario = @Id", usuario).Single();
            var permissaoBandeira = new PermissaoBandeira
            {
                IDPermissao = permissaoUsuario.IDPermissao,
                IDBandeira = idBandeira
            };

            m_permissaoBandeiraGateway.Insert(permissaoBandeira);

            return permissaoBandeira;
        }

        /// <summary>
        /// Remove todas as permissões associadas a bandeira informada.
        /// </summary>
        /// <param name="idBandeira">O id da bandeira.</param>
        public void RemoverPermissoesBandeira(int idBandeira)
        {
            m_permissaoBandeiraGateway.Delete("IDBandeira = @idBandeira", new { idBandeira });
        }

        /// <summary>
        /// Salva a permissão informada.
        /// </summary>
        /// <param name="entidade">A permissão a ser salva.</param>
        public override void Salvar(Permissao entidade)
        {
            Assert(new { user = entidade.IDUsuario }, new AllMustBeInformedSpec());
            var usuario = new Usuario { Id = entidade.IDUsuario };

            Assert(entidade, new PemissaoDeveSerPorBandeiraOuPorLojaSpec());

            if (entidade.IsNew)
            {
                Assert(usuario, new UsuarioPodeTerPermissoesCriadasSpec(this));
                entidade.IDUsuario = usuario.Id;

                entidade.IDUsuarioCriacao = RuntimeContext.Current.User.Id;
                entidade.dhCriacao = DateTime.Now;
                MainGateway.Insert(entidade);
            }
            else
            {
                Assert(usuario, new UsuarioPodeTerPermissoesCriadasSpec(this).Not(Texts.UserDoesNotHavePermissionsYet));

                var permissao = MainGateway.Find("IdUsuario = @Id", usuario).Single();
                permissao.Bandeiras = entidade.Bandeiras;
                permissao.Lojas = entidade.Lojas;
                permissao.blRecebeNotificaoOperacoes = entidade.blRecebeNotificaoOperacoes;
                permissao.blRecebeNotificaoFinanceiro = entidade.blRecebeNotificaoFinanceiro;

                entidade.IDUsuarioAlteracao = RuntimeContext.Current.User.Id;
                entidade.dhAlteracao = DateTime.Now;

                MainGateway.Update(permissao);
            }
        }

        /// <summary>
        /// Remove a permissão com o id informado.
        /// </summary>
        /// <param name="id">O id da permissão a ser removida.</param>
        public override void Remover(int id)
        {
            Assert(RuntimeContext.Current.User, new UsuarioPodeRemoverPermissoesSpec(this));

            base.Remover(id);
        }

        /// <summary>
        /// Obtém um valor que indica se o usuário informado tem acesso de administrador master.
        /// </summary>
        /// <remarks>
        /// Essa permissão é diferente do Papel.IsAdmin, pois essa permissão é referente a um único usuário que é o admin master do sistema.
        /// </remarks>
        /// <param name="idUsuario">O id de usuário.</param>
        /// <returns>True se tem acesso de administrador, false no contrário.</returns>
        public bool TemAcessoAdminMaster(int idUsuario)
        {
            return m_parametroService.Obter().cdUsuarioAdministrador == idUsuario;
        }

        /// <summary>
        /// Pesquisa permissões utilizando os filtros informados.
        /// </summary>
        /// <param name="idUsuario">O id de usuário.</param>
        /// <param name="idBandeira">O id de bandeira.</param>
        /// <param name="idLoja">O id de loja.</param>
        /// <returns>As permissões.</returns>
        public IEnumerable<Permissao> Pesquisar(int? idUsuario, int? idBandeira, int? idLoja)
        {
            return MainGateway.Pesquisar(idUsuario, idBandeira, idLoja);
        }

        /// <summary>
        /// Obtém as permissões do usuário informado.
        /// </summary>
        /// <param name="idUsuario">O id do usuário.</param>
        /// <returns>As permissões do usuário.</returns>
        public UsuarioPermissoes ObterPermissoesDoUsuario(int idUsuario)
        {
            return MainGateway.ObterPermissoesDoUsuario(idUsuario);
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
            return this.MainGateway.PesquisarComFilhos(idUsuario, idBandeira, idLoja, paging);
        }

        /// <summary>
        /// Verifica se a bandeira está válida para inclusão na permissão.
        /// </summary>
        /// <param name="idBandeira">O id da bandeira.</param>
        public void ValidarInclusaoBandeira(int idBandeira)
        {
            Assert(new Bandeira { IDBandeira = idBandeira }, new BandeiraDeveSerValidaParaInclusaoSpec(m_bandeiraGateway));
        }

        /// <summary>
        /// Verifica se a loja está válida para inclusão na permissão.
        /// </summary>
        /// <param name="usuario">O usuário logado.</param>
        /// <param name="idLoja">O id da loja.</param>
        public void ValidarInclusaoLoja(IRuntimeUser usuario, int idLoja)
        {
            var loja = new Loja { IDLoja = idLoja };

            Assert(loja, new LojaDevePossuirBandeiraDefinidaSpec(m_lojaGateway));
            Assert(loja, new BandeiraDaLojaDeveEstarAtivaSpec(m_bandeiraGateway));

            var usuarioLoja = new UsuarioDevePossuirPermissaoNaLojaSpecParameter
            {
                Usuario = usuario,
                IdLoja = idLoja
            };

            Assert(usuarioLoja, new UsuarioDevePossuirPermissaoNaLojaSpec(m_bandeiraGateway, m_permissaoBandeiraGateway, m_permissaoLojaGateway));
        }

        /// <summary>
        /// Verifica se o usuário logado possui permissão para manutenção da permissão.
        /// </summary>
        /// <param name="usuario">O usuário logado.</param>
        /// <returns>Retorna true caso o usuário possua permissão, do contrário retorna false.</returns>
        public bool PossuiPermissaoManutencao(IRuntimeUser usuario)
        {
            var spec = new UsuarioDevePossuirPermissaoManutencaoSpec(this.MainGateway);
            return spec.IsSatisfiedBy(usuario);
        }

        /// <summary>
        /// Verifica se o usuário possui permissão para uma determinada loja.
        /// </summary>
        /// <param name="idUsuario">O id de usuário.</param>
        /// <param name="idLoja">O id da loja.</param>
        /// <returns>True se o usuário possui permissão para a loja, através de permissão específica na loja (PermissaoLoja) ou através da bandeira da loja (PermissaoBandeira).</returns>
        public bool PossuiPermissaoLoja(int idUsuario, int idLoja)
        {
            Assert(new { idUsuario, idLoja }, new AllMustBeInformedSpec());

            return this.MainGateway.PossuiPermissaoLoja(idUsuario, idLoja);
        }
        #endregion
    }
}
