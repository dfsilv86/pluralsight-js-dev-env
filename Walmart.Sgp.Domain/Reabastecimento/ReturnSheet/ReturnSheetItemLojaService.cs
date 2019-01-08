using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Serviço de domínio relacionado a ReturnSheetItemLoja.
    /// </summary>
    public class ReturnSheetItemLojaService : EntityDomainServiceBase<ReturnSheetItemLoja, IReturnSheetItemLojaGateway>, IReturnSheetItemLojaService
    {
        private static String[] s_returnSheetItemLojaAuditProperties = new String[] { "PrecoVenda", "blAtivo" };
        private readonly ISugestaoReturnSheetService m_sugestaoReturnSheetService;
        private readonly IAuditService m_auditService;

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ReturnSheetItemLojaService"/>.
        /// </summary>
        /// <param name="mainGateway">O table data gateway para ReturnSheetItemLoja.</param>
        /// <param name="sugestaoReturnSheetService">O service do SugestaoReturnSheet.</param>
        /// <param name="auditService">O serviço de auditoria.</param>
        public ReturnSheetItemLojaService(IReturnSheetItemLojaGateway mainGateway, ISugestaoReturnSheetService sugestaoReturnSheetService, IAuditService auditService)
            : base(mainGateway)
        {
            this.m_sugestaoReturnSheetService = sugestaoReturnSheetService;
            this.m_auditService = auditService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obtem uma lista de ReturnSheetItemLoja por idReturnSheetItemPrincipal
        /// </summary>
        /// <param name="idReturnSheetItemPrincipal">O idReturnSheetItemPrincipal.</param>
        /// <returns>Uma lista de ReturnSheetItemLoja.</returns>
        public IEnumerable<ReturnSheetItemLoja> ObterPorIdReturnSheetItemPrincipal(int idReturnSheetItemPrincipal)
        {
            Assert(new { IdReturnSheetItemPrincipal = idReturnSheetItemPrincipal }, new AllMustBeInformedSpec());

            return this.MainGateway.ObterPorIdReturnSheetItemPrincipal(idReturnSheetItemPrincipal);
        }

        /// <summary>
        /// Exclusão logia de ReturnSheetItemLoja
        /// </summary>
        /// <param name="id">O IdReturnSheetItemLoja.</param>
        public override void Remover(int id)
        {
            var entidade = this.MainGateway.FindById(id);
            if (entidade != null)
            {
                RemoverSugestaoReturnSheet(id);

                entidade.blAtivo = false;
                this.m_auditService.LogDelete(entidade, s_returnSheetItemLojaAuditProperties);
                this.MainGateway.Update(entidade);
            }
        }

        /// <summary>
        /// Obtém lojas válidas para associação com o item da ReturnSheet.
        /// </summary>
        /// <param name="cdItem">O código do item de saída.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <param name="idReturnSheet">O identificador da ReturnSheet.</param>
        /// <param name="dsEstado">UF para filtrar.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>As lojas válidas para associação com item da ReturnSheet.</returns>
        public IEnumerable<ReturnSheetItemLoja> ObterLojasValidasItem(int cdItem, int cdSistema, int idReturnSheet, string dsEstado, Paging paging)
        {
            var args = new
            {
                cdItem,
                idReturnSheet,
                cdSistema
            };

            Assert(args, new AllMustBeInformedSpec());

            return this.MainGateway.ObterLojasValidasItem(cdItem, cdSistema, idReturnSheet, dsEstado, paging);
        }

        /// <summary>
        /// Obtém estados que possuem lojas válidas para associação com o item da ReturnSheet.
        /// </summary>
        /// <param name="cdItem">O código do item de saída.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>Retorna estados que possuem lojas válidas para associação com o item da ReturnSheet.</returns>
        public IEnumerable<string> ObterEstadosLojasValidasItem(int cdItem, int cdSistema)
        {
            Assert(new { cdItem, cdSistema }, new AllMustBeInformedSpec());

            return this.MainGateway.ObterEstadosLojasValidasItem(cdItem, cdSistema);
        }

        private void RemoverSugestaoReturnSheet(int idReturnSheetItemLoja)
        {
            var entidadesFilhas = m_sugestaoReturnSheetService.ObterPorIdReturnSheetItemLoja(idReturnSheetItemLoja);
            if (entidadesFilhas != null)
            {
                foreach (var e in entidadesFilhas)
                {
                    this.m_sugestaoReturnSheetService.Remover(e.Id);
                }
            }
        }
        #endregion
    }
}