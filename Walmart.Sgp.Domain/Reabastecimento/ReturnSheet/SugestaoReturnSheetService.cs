using System;
using System.Collections.Generic;
using Walmart.Sgp.Domain.Reabastecimento.Specs;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Servico SugestaoReturnSheet
    /// </summary>
    public class SugestaoReturnSheetService : EntityDomainServiceBase<SugestaoReturnSheet, ISugestaoReturnSheetGateway>, ISugestaoReturnSheetService
    {
        private static String[] s_sugestaoReturnSheetAuditProperties = new String[] { "QtdLoja", "QtdRA", "BlAutorizado", "BlAtivo" };
        private readonly IReturnSheetGateway m_returnSheetGateway;
        private readonly IAuditService m_auditService;
        private readonly ILogMensagemReturnSheetVigenteService m_logMensagemReturnSheetVigenteService;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="SugestaoReturnSheetService"/>.
        /// </summary>
        /// <param name="mainGateway">O gateway.</param>
        /// <param name="returnSheetGateway">O gateway do returnsheet.</param>
        /// <param name="auditService">O serviço de auditoria.</param>
        /// <param name="logMensagemReturnSheetVigenteService">O serviço de log de mensagem de return sheet vigente.</param>
        public SugestaoReturnSheetService(ISugestaoReturnSheetGateway mainGateway, IReturnSheetGateway returnSheetGateway, IAuditService auditService, ILogMensagemReturnSheetVigenteService logMensagemReturnSheetVigenteService)
            : base(mainGateway)
        {
            this.m_returnSheetGateway = returnSheetGateway;
            this.m_auditService = auditService;
            this.m_logMensagemReturnSheetVigenteService = logMensagemReturnSheetVigenteService;
        }

        /// <summary>
        /// Obtém um SugestaoReturnSheet pelo seu id.
        /// </summary>
        /// <param name="id">O id.</param>
        /// <returns>O SugestaoReturnSheet.</returns>
        public SugestaoReturnSheet Obter(long id)
        {
            return this.MainGateway.Obter(id);
        }

        /// <summary>
        /// Exclusão logica de SugestaoReturnSheet
        /// </summary>
        /// <param name="id">o IdSugestaoReturnSheet</param>
        public override void Remover(int id)
        {
            var entidade = this.MainGateway.FindById(id);
            if (entidade != null)
            {
                entidade.BlAtivo = false;
                this.m_auditService.LogDelete(entidade, s_sugestaoReturnSheetAuditProperties);
                this.MainGateway.Update(entidade);
            }
        }

        /// <summary>
        /// Buscar por IdReturnSheetItemLoja.
        /// </summary>
        /// <param name="idReturnSheetItemLoja">O idReturnSheetItemLoja.</param>
        /// <returns>Lista de returnSheetItemLoja.</returns>
        public IEnumerable<SugestaoReturnSheet> ObterPorIdReturnSheetItemLoja(int idReturnSheetItemLoja)
        {
            return this.MainGateway.ObterPorIdReturnSheetItemLoja(idReturnSheetItemLoja);
        }

        /// <summary>
        /// Obtem Sugestoes para visualização na tela de Consulta Loja.
        /// </summary>
        /// <param name="idDepartamento">O Id do departamento</param>
        /// <param name="idLoja">O ID da loja.</param>
        /// <param name="dataSolicitacao">A data da solicitacao</param>
        /// <param name="evento">Nome do evento</param>
        /// <param name="vendor9D">Codigo vendor 9 digitos</param>
        /// <param name="idItemDetalhe">Id de um Item</param>
        /// <param name="paging">Dados de paginacao</param>
        /// <returns>Uma lista de SugestaoReturnSheet para consulta.</returns>
        public IEnumerable<SugestaoReturnSheet> ConsultaReturnSheetLoja(int idDepartamento, long idLoja, DateTime dataSolicitacao, string evento, long vendor9D, int idItemDetalhe, Paging paging)
        {
            Assert(new { Department = idDepartamento, requestDate = dataSolicitacao, Store = idLoja }, new AllMustBeInformedSpec());

            return this.MainGateway.ConsultaReturnSheetLoja(idDepartamento, idLoja, dataSolicitacao, evento, vendor9D, idItemDetalhe, paging);
        }

        /// <summary>
        /// Autoriza a exportação dos registros pesquisados.
        /// </summary>
        /// <param name="dtInicioReturn">A data de início da return sheet.</param>
        /// <param name="dtFinalReturn">A data final da return sheet.</param>
        /// <param name="cdV9D">O código de 9 dígitos do vendor.</param>
        /// <param name="evento">O nome do evento.</param>
        /// <param name="cdItemDetalhe">O código do item detalhe de entrada.</param>
        /// <param name="cdDepartamento">O código do departamento.</param>
        /// <param name="cdLoja">O código da loja.</param>
        /// <param name="idRegiaoCompra">O identificador da região de compra.</param>
        /// <param name="blExportado">O flag indicando se a sugestão foi exportada.</param>
        /// <param name="blAutorizado">O flag indicando se a sugestão foi autorizada.</param>
        public void AutorizarExportarPlanilhas(DateTime? dtInicioReturn, DateTime? dtFinalReturn, long? cdV9D, string evento, int? cdItemDetalhe, int? cdDepartamento, int? cdLoja, int? idRegiaoCompra, bool? blExportado, bool? blAutorizado)
        {
            Assert(new { Department = cdDepartamento, startDateReturnSheet = dtInicioReturn, endDateReturnSheet = dtFinalReturn }, new AllMustBeInformedSpec());

            this.MainGateway.AutorizarExportarPlanilhas(dtInicioReturn, dtFinalReturn, cdV9D, evento, cdItemDetalhe, cdDepartamento, cdLoja, idRegiaoCompra, blExportado, blAutorizado);
        }

        /// <summary>
        /// Salva sugestões alteradas pela loja.
        /// </summary>
        /// <param name="sugestoes">Lista de sugestões que serão atualizadas.</param>
        public void SalvarSugestoesLoja(IEnumerable<SugestaoReturnSheet> sugestoes)
        {
            Assert(sugestoes, new SugestoesReturnSheetNaoPodeSerNullSpec());

            foreach (var sugestao in sugestoes)
            {
                Assert(sugestao, new SugestaoReturnSheetNaoPodeTerAutorizacaoSpec(this.m_returnSheetGateway.PossuiAutorizacao));
                Assert(sugestao, new SugestaoReturnSheetNaoPodeTerExportacaoSpec(this.m_returnSheetGateway.PossuiExportacao));

                var sugestaoOld = MainGateway.Obter(sugestao.IdSugestaoReturnSheet);
                sugestaoOld.QtdLoja = sugestao.QtdLoja;

                SalvarSugestao(sugestaoOld);
            }
        }

        /// <summary>
        /// Salva sugestões alteradas pelo RA.
        /// </summary>
        /// <param name="sugestoes">Lista de sugestões que serão atualizadas.</param>
        public void SalvarSugestoesRA(IEnumerable<SugestaoReturnSheet> sugestoes)
        {
            Assert(sugestoes, new SugestoesReturnSheetNaoPodeSerNullSpec());

            foreach (var sugestao in sugestoes)
            {
                Assert(sugestao, new SugestaoReturnPodeSalvarSpec());
                Assert(sugestao, new SugestaoReturnSheetNaoPodeTerAutorizacaoSpec(this.m_returnSheetGateway.PossuiAutorizacao));
                Assert(sugestao, new SugestaoReturnSheetNaoPodeTerExportacaoSpec(this.m_returnSheetGateway.PossuiExportacao));

                var sugestaoOld = MainGateway.Obter(sugestao.IdSugestaoReturnSheet);
                sugestaoOld.QtdRA = sugestao.QtdRA;

                SalvarSugestao(sugestaoOld);
            }
        }

        /// <summary>
        /// Obtem Sugestões para visualização na tela de Consulta Return Sheet RA.
        /// </summary>
        /// <param name="dtInicioReturn">A data de início da return sheet.</param>
        /// <param name="dtFinalReturn">A data final da return sheet.</param>
        /// <param name="cdV9D">O código de 9 dígitos do vendor.</param>
        /// <param name="evento">O nome do evento.</param>
        /// <param name="cdItemDetalhe">O código do item detalhe de entrada.</param>
        /// <param name="cdDepartamento">O código do departamento.</param>
        /// <param name="cdLoja">O código da loja.</param>
        /// <param name="idRegiaoCompra">O identificador da região de compra.</param>
        /// <param name="blExportado">O flag indicando se a sugestão foi exportada.</param>
        /// <param name="blAutorizado">O flag indicando se a sugestão foi autorizada.</param>
        /// <param name="paging">Dados de paginação.</param>
        /// <returns>Retorna Sugestões para visualização na tela de Consulta Return Sheet RA.</returns>
        public IEnumerable<SugestaoReturnSheet> ConsultaReturnSheetLojaRA(DateTime? dtInicioReturn, DateTime? dtFinalReturn, long? cdV9D, string evento, int? cdItemDetalhe, int? cdDepartamento, int? cdLoja, int? idRegiaoCompra, bool? blExportado, bool? blAutorizado, Paging paging)
        {
            Assert(new { Department = cdDepartamento, startDateReturnSheet = dtInicioReturn, endDateReturnSheet = dtFinalReturn }, new AllMustBeInformedSpec());

            return this.MainGateway.ConsultaReturnSheetLojaRA(dtInicioReturn, dtFinalReturn, cdV9D, evento, cdItemDetalhe, cdDepartamento, cdLoja, idRegiaoCompra, blExportado, blAutorizado, paging);
        }

        /// <summary>
        /// Verifica se existem return sheets vigentes que ainda não foram solicitadas quantidades considerando o papel do usuário e loja que está logado.
        /// </summary>
        /// <returns>Retorna true caso existam return sheets vigentes, do contrário retorna false.</returns>
        public bool PossuiReturnsVigentesQuantidadesVazias()
        {
            var idLoja = RuntimeContext.Current.User.StoreId;

            if (!idLoja.HasValue)
            {
                return false;
            }

            var idUsuario = RuntimeContext.Current.User.Id;
            var idPapel = RuntimeContext.Current.User.RoleId;

            return MainGateway.PossuiReturnsVigentesQuantidadesVazias(idUsuario, idPapel, idLoja.Value);
        }

        /// <summary>
        /// Registra log indicando que usuário visualizou o aviso de que existem return sheets vigentes que ainda não foram solicitadas quantidades considerando o papel do usuário e loja que está logado.
        /// </summary>
        public void RegistrarLogAvisoReturnSheetsVigentes()
        {
            var log = new LogMensagemReturnSheetVigente
            {
                IDUsuario = RuntimeContext.Current.User.Id,
                IDLoja = RuntimeContext.Current.User.StoreId.Value,
                dhCriacao = DateTime.Now
            };

            m_logMensagemReturnSheetVigenteService.Salvar(log);
        }

        private void SalvarSugestao(SugestaoReturnSheet sugestao)
        {
            sugestao.IdUsuarioAtualizacao = RuntimeContext.Current.User.Id;
            sugestao.DhAtualizacao = DateTime.Now;
            this.MainGateway.Update("qtdLoja = @qtdLoja, qtdRA = @qtdRA, IdUsuarioAtualizacao = @IdUsuarioAtualizacao, DhAtualizacao = @DhAtualizacao", sugestao);
            this.m_auditService.LogUpdate(sugestao, s_sugestaoReturnSheetAuditProperties);
        }
    }
}