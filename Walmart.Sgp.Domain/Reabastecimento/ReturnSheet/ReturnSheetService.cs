using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Reabastecimento.Specs;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Specs;
using Runtime = Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Servico do ReturnSheet
    /// </summary>
    public class ReturnSheetService : EntityDomainServiceBase<ReturnSheet, IReturnSheetGateway>, IReturnSheetService
    {
        private static String[] s_returnSheetAuditProperties = new String[] { "DhInicioReturn", "DhFinalReturn", "DhInicioEvento", "DhFinalEvento", "IdRegiaoCompra", "Descricao", "BlAtivo" };
        private readonly IRegiaoCompraGateway m_regiaoCompraGateway;
        private readonly IDepartamentoGateway m_departamentoGateway;
        private readonly IReturnSheetItemPrincipalService m_returnSheetItemPrincipalService;
        private readonly IAuditService m_auditService;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ReturnSheetService"/>.
        /// </summary>
        /// <param name="mainGateway">O gateway.</param>
        /// <param name="regiaoCompraGateway">O gateway regiao compra.</param>
        /// <param name="departamentoGateway">O gateway departamento.</param>
        /// <param name="returnSheetItemPrincipalService">O service do ReturnSheetItemPrincipal</param>
        /// <param name="auditService">O serviço de auditoria.</param>
        public ReturnSheetService(IReturnSheetGateway mainGateway, IRegiaoCompraGateway regiaoCompraGateway, IDepartamentoGateway departamentoGateway, IReturnSheetItemPrincipalService returnSheetItemPrincipalService, IAuditService auditService)
            : base(mainGateway)
        {
            this.m_regiaoCompraGateway = regiaoCompraGateway;
            this.m_departamentoGateway = departamentoGateway;
            this.m_returnSheetItemPrincipalService = returnSheetItemPrincipalService;
            this.m_auditService = auditService;
        }

        /// <summary>
        /// Obtém um ReturnSheet pelo seu id.
        /// </summary>
        /// <param name="id">O id.</param>
        /// <returns>O ReturnSheet.</returns>
        public ReturnSheet Obter(long id)
        {
            var r = this.MainGateway.Obter(id);

            if (r != null)
            {
                r.RegiaoCompra = this.m_regiaoCompraGateway.FindById(r.IdRegiaoCompra);
                r.Departamento = this.m_departamentoGateway.FindById(r.idDepartamento);
            }

            return r;
        }

        /// <summary>
        /// Verifica se uma ReturnSheet pode ser editada.
        /// </summary>
        /// <param name="idReturnSheet">Id da returnSheet.</param>
        /// <returns>Retorna true caso a return sheet possa ser editada, do contrário retorna false.</returns>
        public bool PodeSerEditada(int idReturnSheet)
        {
            Assert(new { IdReturnSheet = idReturnSheet }, new AllMustBeInformedSpec());

            return !this.MainGateway.PossuiExportacao(idReturnSheet) && !this.MainGateway.PossuiAutorizacao(idReturnSheet);
        }

        /// <summary>
        /// Pesquisar ReturnSheet
        /// </summary>
        /// <param name="dtInicioReturn">Data inicio do ReturnSheet</param>
        /// <param name="dtFinalReturn">Data final do ReturnSheet</param>
        /// <param name="evento">Descricao do evento (ReturnSheet)</param>
        /// <param name="idDepartamento">Id do departamento</param>
        /// <param name="filtroAtivos">Filtrar somente ativos (0 = Somente Inativos, 1 = Somente Ativos, 2 = Todos)</param>
        /// <param name="idRegiaoCompra">Id da regiao de compra.</param>
        /// <param name="paging">A paginacao</param>
        /// <returns>Uma lista de ReturnSheet</returns>
        public IEnumerable<ReturnSheet> Pesquisar(DateTime? dtInicioReturn, DateTime? dtFinalReturn, string evento, int? idDepartamento, int filtroAtivos, int? idRegiaoCompra, Paging paging)
        {
            return this.MainGateway.Pesquisar(dtInicioReturn, dtFinalReturn, evento, idDepartamento, filtroAtivos, idRegiaoCompra, paging);
        }

        /// <summary>
        /// Salva a entidade.
        /// </summary>
        /// <param name="entidade">A entidade.</param>
        public override void Salvar(ReturnSheet entidade)
        {
            var camposObrigatorios = new
            {
                DataHoraInicioEvento = entidade.DhInicioEvento,
                DataHoraFinalEvent = entidade.DhFinalEvento,
                DataHoraInicioReturnSheet = entidade.DhInicioReturn,
                DataHoraFinalReturnSheet = entidade.DhFinalReturn,
                Descricao = entidade.Descricao,
                Departamento = entidade.idDepartamento,
                RegiaoCompra = entidade.IdRegiaoCompra
            };

            Assert(camposObrigatorios, new AllMustBeInformedSpec());

            ValidaRS(entidade);

            if (entidade.IsNew)
            {
                Assert(new DateTime[] { entidade.DhInicioReturn, entidade.DhFinalReturn, entidade.DhInicioEvento, entidade.DhFinalEvento }, new AllDatesMustBeGreaterThanNowSpec());

                entidade.DhAtualizacao = entidade.DhCriacao;
                entidade.IdUsuarioCriacao = Runtime.RuntimeContext.Current.User.Id;
                entidade.DhCriacao = DateTime.Now;

                this.MainGateway.Insert(entidade);
                this.m_auditService.LogInsert(entidade, s_returnSheetAuditProperties);
            }
            else
            {
                var entidadeOld = this.Obter(entidade.Id);

                if (entidadeOld != null)
                {
                    ValidarDataAlterada(entidadeOld, entidade);

                    entidade.BlAtivo = entidadeOld.BlAtivo;
                    entidade.DhCriacao = entidadeOld.DhCriacao;
                    entidade.IdUsuarioCriacao = entidadeOld.IdUsuarioCriacao;

                    entidade.DhAtualizacao = DateTime.Now;
                    this.MainGateway.Update(entidade);

                    LogReturnSheet(entidadeOld, entidade);
                }
            }
        }

        /// <summary>
        /// Faz a exclusão logica (BlAtivo = false) de um ReturnSheet e seus SugestaoReturnSheet.
        /// </summary>
        /// <param name="id">O id do ReturnSheet</param>
        public override void Remover(int id)
        {
            var entidade = this.MainGateway.FindById(id);
            if (entidade != null)
            {
                RemoverReturnSheetItemPrincipal(id);

                entidade.BlAtivo = false;
                entidade.DhAtualizacao = DateTime.Now;
                this.m_auditService.LogDelete(entidade, s_returnSheetAuditProperties);
                this.MainGateway.Update(entidade);
            }
        }

        private static void ValidarDataAlterada(ReturnSheet entidadeOld, ReturnSheet entidade)
        {
            if (entidadeOld.DhInicioReturn != entidade.DhInicioReturn || entidadeOld.DhFinalReturn != entidade.DhFinalReturn ||
                entidadeOld.DhInicioEvento != entidade.DhInicioEvento || entidadeOld.DhFinalEvento != entidade.DhFinalEvento)
            {
                Assert(new DateTime[] { entidade.DhInicioReturn, entidade.DhFinalReturn, entidade.DhInicioEvento, entidade.DhFinalEvento }, new AllDatesMustBeGreaterThanNowSpec());
            }
        }

        private static void ValidaRS(ReturnSheet entidade)
        {
            Assert(entidade.Descricao, new MaxLenghtSpec(50));

            Assert(entidade, new ReturnSheetCannotHavePastDatesSpec());

            Assert(entidade, new ReturnSheetDevePossuirPeriodosValidosSpec());
        }

        private void RemoverReturnSheetItemPrincipal(int idReturnSheet)
        {
            var entidadesFilhas = m_returnSheetItemPrincipalService.ObterPorIdReturnSheet(idReturnSheet);
            if (entidadesFilhas != null)
            {
                foreach (var e in entidadesFilhas)
                {
                    this.m_returnSheetItemPrincipalService.Remover(e.Id);
                }
            }
        }

        private void LogReturnSheet(ReturnSheet oldReturnSheet, ReturnSheet newReturnSheet)
        {
            if ((newReturnSheet.DhInicioReturn.Date != oldReturnSheet.DhInicioReturn.Date) ||
               (newReturnSheet.DhFinalReturn.Date != oldReturnSheet.DhFinalReturn.Date) ||
               (newReturnSheet.DhInicioEvento.Date != oldReturnSheet.DhInicioEvento.Date) ||
               (newReturnSheet.DhFinalEvento.Date != oldReturnSheet.DhFinalEvento.Date) ||
               (newReturnSheet.IdRegiaoCompra != oldReturnSheet.IdRegiaoCompra) ||
               (newReturnSheet.Descricao != oldReturnSheet.Descricao))
            {
                this.m_auditService.LogUpdate(newReturnSheet, s_returnSheetAuditProperties);
            }
        }
    }
}
