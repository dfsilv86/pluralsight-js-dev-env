using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Serviço de domínio relacionado a grade sugestao.
    /// </summary>
    public class GradeSugestaoService : EntityDomainServiceBase<GradeSugestao, IGradeSugestaoGateway>, IGradeSugestaoService
    {
        #region Constructor
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="GradeSugestaoService"/>
        /// </summary>
        /// <param name="mainGateway">O table data gateway para grade de sugestão.</param>
        public GradeSugestaoService(IGradeSugestaoGateway mainGateway)
            : base(mainGateway)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Verifica se existe uma grade de sugestões para os parâmetros informados aberta.
        /// </summary>
        /// <param name="cdSistema">O código de sistema.</param>
        /// <param name="idBandeira">O id de bandeira.</param>
        /// <param name="idDepartamento">O id de departamento.</param>
        /// <param name="idLoja">O id de loja.</param>
        /// <param name="vlHoraLimite">O valor de hora-minuto (HHMM)</param>
        /// <returns>
        /// True se a grade de sugestões estiver aberta.
        /// </returns>
        public bool ExisteGradeSugestaoAberta(int cdSistema, int idBandeira, int idDepartamento, int idLoja, int vlHoraLimite)
        {
            return this.MainGateway.ExisteGradeSugestaoAberta(cdSistema, idBandeira, idDepartamento, idLoja, vlHoraLimite);
        }

        /// <summary>
        /// Pesquisa estruturado por filtro.
        /// </summary>
        /// <param name="cdSistema">O código de sistema.</param>
        /// <param name="idBandeira">O id de bandeira.</param>
        /// <param name="cdDepartamento">O código de departamento.</param>
        /// <param name="cdLoja">O código de loja.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>Os registros que satisfasem o filtro.</returns>
        public IEnumerable<GradeSugestao> PesquisarEstruturadoPorFiltro(int cdSistema, int? idBandeira, int? cdDepartamento, int? cdLoja, Paging paging)
        {
            return MainGateway.PesquisarEstruturadoPorFiltro(cdSistema, idBandeira, cdDepartamento, cdLoja, paging);
        }

        /// <summary>
        /// Obtém a grade de sugestão junto com seus relacionamentos.
        /// </summary>
        /// <param name="id">O id da grade de sugestão.</param>
        /// <returns>
        /// A grade de sugestão.
        /// </returns>
        public GradeSugestao ObterEstruturadoPorId(int id)
        {
            return MainGateway.ObterEstruturadoPorId(id);
        }

        /// <summary>
        /// Atualiza a grade de sugestão especificada.
        /// </summary>
        /// <param name="gradeSugestao">A grade de sugestão.</param>        
        public void Atualizar(GradeSugestao gradeSugestao)
        {
            Assert(
                new
                {
                    gradeSugestao.vlHoraInicial,
                    gradeSugestao.vlHoraFinal,
              },
              new MustRespectRangeSpec { AllowEquals = false });

            Assert(
                new
                {
                    gradeSugestao.vlHoraFinal,
                    gradeSugestao.cdSistema,
                    gradeSugestao.IDBandeira,
                    gradeSugestao.IDDepartamento,
                    gradeSugestao.IDLoja,
                    gradeSugestao.IDGradeSugestao
                },
                new AllMustBeInformedSpec());

            Assert(gradeSugestao, new Specs.GradeSugestaoDeveSerUnicaSpec(MainGateway));

            MainGateway.Update(
                "vlHoraInicial = @inicial, vlHoraFinal = @final, dhAtualizacao = @dhAtualizacao, cdUsuarioAtualizacao = @cdUsuarioAtualizacao",
                "IDGradeSugestao = @id",
                new
                {
                    inicial = gradeSugestao.vlHoraInicial,
                    final = gradeSugestao.vlHoraFinal,
                    dhAtualizacao = DateTime.Now,
                    cdUsuarioAtualizacao = RuntimeContext.Current.User.Id,
                    id = gradeSugestao.IDGradeSugestao
                });
        }     

        /// <summary>
        /// Salva as novas sugestões.
        /// </summary>
        /// <param name="sugestoes">As sugestões.</param>
        public void SalvarNovas(IEnumerable<GradeSugestao> sugestoes)
        {
            Assert(sugestoes, new EntityMustBeNewSpec());

            Assert(
                sugestoes.Select(
                gradeSugestao =>
                new
                {
                    gradeSugestao.vlHoraInicial,
                    gradeSugestao.vlHoraFinal
                }),
                 new MustRespectRangeSpec { AllowEquals = false });

            Assert(
                sugestoes.Select(
                gradeSugestao =>
                new
                {
                    gradeSugestao.vlHoraFinal,
                    gradeSugestao.cdSistema,
                    gradeSugestao.IDBandeira,
                    gradeSugestao.IDDepartamento,
                    gradeSugestao.IDLoja
                }),
                new AllMustBeInformedSpec());

            Assert<IEnumerable<GradeSugestao>>(sugestoes, new Specs.SugestoesDevemSerUnicasSpec());
            Assert(sugestoes, new Specs.GradeSugestaoDeveSerUnicaSpec(MainGateway));
            var cdUsuarioCriacao = RuntimeContext.Current.User.Id;
            var dhCricacao = DateTime.Now;

            foreach (var sugestao in sugestoes)
            {
                sugestao.CdUsuarioCriacao = cdUsuarioCriacao;
                sugestao.DhCriacao = dhCricacao;

                MainGateway.Insert(sugestao);
            }
        }      

        /// <summary>
        /// Conta quantas sugestoes existem com a mesma configuração.
        /// </summary>
        /// <param name="cdSistema">O código do sistema.</param>
        /// <param name="idBandeira">O id da bandeira.</param>
        /// <param name="idLoja">O id da loja.</param>
        /// <param name="idDepartamento">O id do departamento</param>
        /// <returns>A quantidade de sugestoes que existem com a mesma configuração.</returns>
        public long ContarExistentes(int cdSistema, int idBandeira, int idLoja, int idDepartamento)
        {
            return MainGateway.Count(
                "cdSistema = @cdSistema AND idBandeira = @idBandeira AND idLoja = @idLoja AND idDepartamento = @idDepartamento",
                new
                {
                    cdSistema,
                    idBandeira,
                    idLoja,
                    idDepartamento
                });
        }

        #endregion
    }
}
