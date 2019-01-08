using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Gerenciamento
{
    /// <summary>
    /// Serviço de domínio relacionado a parâmetro de fornecedor.
    /// </summary>
    public class FornecedorParametroService : EntityDomainServiceBase<FornecedorParametro, IFornecedorParametroGateway>, IFornecedorParametroService
    {
        private readonly IItemDetalheGateway m_itemDetalheGateway;

        #region Constructor
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="FornecedorParametroService"/>.
        /// </summary>
        /// <param name="mainGateway">O table data gateway para main data.</param>
        /// <param name="itemDetalheGateway">O table data gateway para itemDetalhe.</param>
        public FornecedorParametroService(IFornecedorParametroGateway mainGateway, IItemDetalheGateway itemDetalheGateway)
            : base(mainGateway)
        {
            this.m_itemDetalheGateway = itemDetalheGateway;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Verifica se um item tem FornecedorParametro vinculado.
        /// </summary>
        /// <param name="cdItem">O codigo do item.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <returns>Se o fornecedorParametro do item informado esta vinculado ou não.</returns>
        public bool PossuiVendorVinculado(long cdItem, long cdSistema)
        {
            var item = m_itemDetalheGateway.ObterPorItemESistema((long)cdItem, (byte)cdSistema);
            if (item == null)
            {
                return true;
            }

            var fp = this.MainGateway.FindById(item.IdFornecedorParametro);

            return fp != null;
        }

        /// <summary>
        /// Verifica se um FornecedorParametro esta inativo ou excluido por codigo de item e sistema.
        /// </summary>
        /// <param name="cdItem">O codigo do item.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <returns>Se o fornecedorParametro do item informado esta inativo ou excluido.</returns>
        public bool EstaInativoOuExcluido(long cdItem, long cdSistema)
        {
            var item = m_itemDetalheGateway.ObterPorItemESistema(cdItem, (byte)cdSistema);
            if (item == null)
            {
                return false;
            }

            var fp = this.MainGateway.ObterEstruturadoPorId(item.IdFornecedorParametro);
            if (fp == null)
            {
                return false;
            }

            if (fp.blAtivo == false || fp.cdStatusVendor.Value != "A")
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Pesquisa parametros de fornecedores baseado nos filtros informados.
        /// </summary>
        /// <param name="cdSistema">O código do sistema.</param>
        /// <param name="stFornecedor">O status do fornecedor.</param>
        /// <param name="cdV9D">O código do vendor (9 dígitos).</param>
        /// <param name="nmFornecedor">A razão social.</param>
        /// <param name="paging">A paginação</param>
        /// <returns>Os parâmetros de fornecedores que se encaixam no filtro informado.</returns>
        public IEnumerable<FornecedorParametro> PesquisarPorFiltro(int cdSistema, string stFornecedor, long? cdV9D, string nmFornecedor, Paging paging)
        {
            return MainGateway.PesquisarPorFiltro(cdSistema, stFornecedor, cdV9D, nmFornecedor, paging);
        }

        /// <summary>
        /// Busca o parametro do fornecedor juntamente com o fornecedor.
        /// </summary>
        /// <param name="id">O identificador do parametro do fornecedor.</param>
        /// <returns>O parametro do fornecedor juntamente com o fornecedor.</returns>
        public FornecedorParametro ObterEstruturadoPorId(int id)
        {
            return MainGateway.ObterEstruturadoPorId(id);
        }

        /// <summary>
        /// Busca review dates por detalhe.
        /// </summary>
        /// <param name="idFornecedorParametro">O id do fornecedor parâmetro.</param>
        /// <param name="detalhe">O tipo de detalhamento.</param>
        /// <param name="paging">A paginação</param>
        /// <returns>Os review dates filtrados por detalhe.</returns>
        public IEnumerable<FornecedorParametroReviewDate> ObterReviewDatesPorDetalhe(int idFornecedorParametro, TipoDetalhamentoReviewDate detalhe, Paging paging)
        {
            return MainGateway.ObterReviewDatesPorDetalhe(idFornecedorParametro, detalhe, paging);
        }

        /// <summary>
        /// Pesquisa parametros de fornecedor.
        /// </summary>
        /// <param name="cdSistema">O código da estrutura mercadológica.</param>
        /// <param name="cdTipo">O canal do vendor, se aplicável.</param>
        /// <param name="cdV9D">O código 9 dígitos do vendor.</param>
        /// <param name="nmFornecedor">O nome do fornecedor.</param>
        /// <param name="paging">A paginação</param>
        /// <returns>Os parametros de fornecedor encontrados.</returns>
        public IEnumerable<FornecedorParametro> PesquisarPorSistemaCodigo9DigitosENomeFornecedor(int cdSistema, string cdTipo, long? cdV9D, string nmFornecedor, Paging paging)
        {
            Assert(new { MarketingStructure = cdSistema }, new AllMustBeInformedSpec());
            Assert(new { cdV9D, nmFornecedor }, new AtLeastOneMustBeInformedSpec(true));

            return this.MainGateway.PesquisarPorSistemaCodigo9DigitosENomeFornecedor(cdSistema, cdTipo, cdV9D, nmFornecedor, paging);
        }

        /// <summary>
        /// Localiza um parâmetro de fornecedor pelo código de 9 dígitos e nome do fornecedor.
        /// </summary>
        /// <param name="cdSistema">O código da estrutura mercadológica.</param>
        /// <param name="cdTipo">O canal do vendor, se aplicável.</param>
        /// <param name="cdV9D">O código 9 dígitos do vendor.</param>
        /// <returns>O parâmetro de fornecedor.</returns>
        public FornecedorParametro ObterPorSistemaECodigo9Digitos(int cdSistema, string cdTipo, long? cdV9D)
        {
            Assert(new { MarketingStructure = cdSistema }, new AllMustBeInformedSpec());
            Assert(new { cdV9D }, new AllMustBeInformedSpec(true));

            return this.MainGateway.ObterPorSistemaECodigo9Digitos(cdSistema, cdTipo, cdV9D.Value);
        }

        #endregion
    }
}
