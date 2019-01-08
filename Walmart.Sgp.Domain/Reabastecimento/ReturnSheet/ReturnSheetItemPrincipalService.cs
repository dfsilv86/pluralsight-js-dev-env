using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Reabastecimento.Specs;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Serviço de domínio relacionado a ReturnSheetItemPrincipal.
    /// </summary>
    public class ReturnSheetItemPrincipalService : EntityDomainServiceBase<ReturnSheetItemPrincipal, IReturnSheetItemPrincipalGateway>, IReturnSheetItemPrincipalService
    {
        private static String[] s_returnSheetItemPrincipalAuditProperties = new String[] { "blAtivo" };
        private static String[] s_returnSheetItemLojaAuditProperties = new String[] { "PrecoVenda", "blAtivo" };
        private readonly IReturnSheetItemLojaService m_returnSheetItemLojaService;
        private readonly IReturnSheetItemLojaGateway m_returnSheetItemLojaGateway;
        private readonly IAuditService m_auditService;

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ReturnSheetItemPrincipalService"/>.
        /// </summary>
        /// <param name="mainGateway">O table data gateway para ReturnSheetItemPrincipal.</param>
        /// <param name="returnSheetItemLojaService">O service do returnSheetItemLoja.</param>
        /// <param name="returnSheetItemLojaGateway">O gateway do returnSheetItemLoja.</param>
        /// <param name="auditService">O serviço de auditoria.</param>
        public ReturnSheetItemPrincipalService(IReturnSheetItemPrincipalGateway mainGateway, IReturnSheetItemLojaService returnSheetItemLojaService, IReturnSheetItemLojaGateway returnSheetItemLojaGateway, IAuditService auditService)
            : base(mainGateway)
        {
            this.m_returnSheetItemLojaService = returnSheetItemLojaService;
            this.m_returnSheetItemLojaGateway = returnSheetItemLojaGateway;
            this.m_auditService = auditService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obtem uma lista de ReturnSheetItemPrincipal por IdReturnSheet
        /// </summary>
        /// <param name="idReturnSheet">O IdReturnSheet</param>
        /// <returns>Uma lista de ReturnSheetItemPrincipal.</returns>
        public IEnumerable<ReturnSheetItemPrincipal> ObterPorIdReturnSheet(int idReturnSheet)
        {
            return this.MainGateway.ObterPorIdReturnSheet(idReturnSheet);
        }

        /// <summary>
        /// Pesquisa ReturnSheetItemPrincipal por um IdReturnSheet.
        /// </summary>
        /// <param name="idReturnSheet">O ID do return sheet.</param>
        /// <param name="paging">O controle de paginação.</param>
        /// <returns>Uma lista de ReturnSheetItemPrincipal.</returns>
        public IEnumerable<ReturnSheetItemPrincipal> PesquisarPorIdReturnSheet(int idReturnSheet, Paging paging)
        {
            Assert(new { IdReturnSheet = idReturnSheet }, new AllMustBeInformedSpec());

            return this.MainGateway.PesquisarPorIdReturnSheet(idReturnSheet, paging);
        }

        /// <summary>
        /// Exclusao logica de ReturnSheetItemPrincipal
        /// </summary>
        /// <param name="id">O IdReturnSheetItemPrincipal.</param>
        public override void Remover(int id)
        {
            var entidade = this.MainGateway.FindById(id);
            if (entidade != null)
            {
                RemoverReturnSheetItemLoja(id);

                entidade.blAtivo = false;
                this.m_auditService.LogDelete(entidade, s_returnSheetItemPrincipalAuditProperties);
                this.MainGateway.Update(entidade);
            }
        }

        /// <summary>
        /// Realiza o vínculo entre as lojas e os itens de uma ReturnSheet.
        /// </summary>
        /// <param name="lojasAlteradas">A lista de lojas que foram alteradas.</param>
        /// <param name="idReturnSheet">O identificador da ReturnSheet.</param>
        /// <param name="precoVenda">O preço de venda a ser aplicado para todas as lojas.</param>
        public void SalvarLojas(IEnumerable<ReturnSheetItemLoja> lojasAlteradas, int idReturnSheet, decimal? precoVenda)
        {
            Assert(new { idReturnSheet }, new AllMustBeInformedSpec());

            var idItemDetalheSaida = lojasAlteradas.ElementAt(0).IdItemDetalheSaida;
            var returnSheetItemPrincipal = MainGateway.Insert(idReturnSheet, idItemDetalheSaida);
            var lojasPersistidas = m_returnSheetItemLojaGateway.ObterLojasPorReturnSheetEItemDetalheSaida(idReturnSheet, idItemDetalheSaida);

            ValidarLojas(lojasAlteradas, lojasPersistidas, idReturnSheet);

            foreach (var loja in lojasAlteradas)
            {
                var lojaPersistida = lojasPersistidas.Where(lp => lp.IdItemDetalhe == loja.IdItemDetalheEntrada && lp.IdLoja == loja.IdLoja).SingleOrDefault();

                if (lojaPersistida == null)
                {
                    InserirLoja(returnSheetItemPrincipal, loja);
                }
                else
                {
                    AtualizarLoja(loja, lojaPersistida);
                }
            }

            AtualizarPrecoVendaLojasPersistidas(lojasAlteradas, precoVenda, lojasPersistidas);
            ReativarReturnSheetItemPrincipal(returnSheetItemPrincipal);
        }

        /// <summary>
        /// Exclusão lógica de ReturnSheetItemPrincipal.
        /// </summary>
        /// <param name="idReturnSheet">O identificador da return sheet.</param>
        /// <param name="cdItem">O código do item detalhe de saída.</param>
        public void Remover(int idReturnSheet, int cdItem)
        {
            var idReturnSheetItemPrincipal = MainGateway.ObterPorReturnSheetEItemDetalheSaida(idReturnSheet, cdItem);
            Remover(idReturnSheetItemPrincipal);
        }

        private void ValidarLojas(IEnumerable<ReturnSheetItemLoja> lojasAlteradas, IEnumerable<ReturnSheetItemLoja> lojasPersistidas, int idReturnSheet)
        {
            Assert(lojasAlteradas, new PrecoVendaDeveSerInformadoSpec());
            Assert(new ReturnSheetItemLojaSpecParameter { LojasAlteradas = lojasAlteradas, LojasPersistidas = lojasPersistidas }, new LojaDeveSerSelecionadaSpec());
        }

        private void ReativarReturnSheetItemPrincipal(ReturnSheetItemPrincipal returnSheetItemPrincipal)
        {
            if (returnSheetItemPrincipal.blAtivo)
            {
                return;
            }

            returnSheetItemPrincipal.blAtivo = true;
            this.MainGateway.Update(returnSheetItemPrincipal);
            this.m_auditService.LogUpdate(returnSheetItemPrincipal, s_returnSheetItemPrincipalAuditProperties);
        }

        private void AtualizarPrecoVendaLojasPersistidas(IEnumerable<ReturnSheetItemLoja> lojasAlteradas, decimal? precoVenda, IEnumerable<ReturnSheetItemLoja> lojasPersistidas)
        {
            if (precoVenda.HasValue)
            {
                var lojas = lojasPersistidas.Except(lojasAlteradas, new ReturnSheetItemLojaEqualityComparer());

                foreach (var loja in lojas)
                {
                    loja.PrecoVenda = precoVenda.Value;
                    m_returnSheetItemLojaGateway.Update(loja);
                    this.m_auditService.LogUpdate(loja, s_returnSheetItemLojaAuditProperties);
                }
            }
        }

        private void AtualizarLoja(ReturnSheetItemLoja loja, ReturnSheetItemLoja lojaPersistida)
        {
            if (!loja.selecionado && !lojaPersistida.blAtivo)
            {
                return;
            }

            lojaPersistida.blAtivo = loja.selecionado;
            lojaPersistida.PrecoVenda = loja.PrecoVenda.HasValue ? loja.PrecoVenda : 0;
            m_returnSheetItemLojaGateway.Update(lojaPersistida);
            this.m_auditService.LogUpdate(lojaPersistida, s_returnSheetItemLojaAuditProperties);
        }

        private void InserirLoja(ReturnSheetItemPrincipal returnSheetItemPrincipal, ReturnSheetItemLoja loja)
        {
            if (loja.selecionado)
            {
                loja.IdReturnSheetItemPrincipal = returnSheetItemPrincipal.IdReturnSheetItemPrincipal;
                loja.blAtivo = loja.selecionado;
                loja.IdItemDetalhe = loja.IdItemDetalheEntrada;
                m_returnSheetItemLojaGateway.Insert(loja);
                this.m_auditService.LogInsert(loja, s_returnSheetItemLojaAuditProperties);
            }
        }

        private void RemoverReturnSheetItemLoja(int idReturnSheetItemPrincipal)
        {
            var entidadesFilhas = m_returnSheetItemLojaService.ObterPorIdReturnSheetItemPrincipal(idReturnSheetItemPrincipal);
            if (entidadesFilhas != null)
            {
                foreach (var e in entidadesFilhas)
                {
                    this.m_returnSheetItemLojaService.Remover(e.Id);
                }
            }
        }
        #endregion
    }
}