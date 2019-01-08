using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Gerenciamento
{
    /// <summary>
    /// Serviço de domínio relacionado a Fornecedor.
    /// </summary>
    public class FornecedorService : EntityDomainServiceBase<Fornecedor, IFornecedorGateway>, IFornecedorService
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="FornecedorService"/>.
        /// </summary>
        /// <param name="mainGateway">O table data gateway para fineline.</param>
        public FornecedorService(IFornecedorGateway mainGateway)
            : base(mainGateway)
        {
        }
        #endregion

        /// <summary>
        /// Obtém um fornecedor pelo seu id.
        /// </summary>
        /// <param name="id">O id.</param>
        /// <returns>O fornecedor.</returns>
        public Fornecedor Obter(long id)
        {
            return this.MainGateway.Obter(id);
        }

        /// <summary>
        /// Obtém um Fornecedor por cdSistema e cdFornecedor.
        /// </summary>
        /// <param name="cdSistema">O sistema.</param>
        /// <param name="cdFornecedor">O codigo do fornecedor.</param>
        /// <returns>A entidade Fornecedor.</returns>
        public Fornecedor ObterPorSistemaCodigo(short cdSistema, int cdFornecedor)
        {
            Assert(new { MarketingStructure = cdSistema, SupplierCode = cdFornecedor }, new AllMustBeInformedSpec());

            return this.MainGateway.ObterPorSistemaCodigo(cdSistema, cdFornecedor);
        }

        /// <summary>
        /// Obtém uma lista de Fornecedor por cdSistema e cdFornecedor.
        /// </summary>
        /// <param name="cdSistema">O sistema.</param>
        /// <param name="cdFornecedor">O codigo do fornecedor.</param>
        /// <param name="nmFornecedor">O nome do fornecedor.</param>
        /// <param name="paging">Paginação do resultado.</param>
        /// <returns>A lista de entidade Fornecedor.</returns>
        public IEnumerable<Fornecedor> ObterListaPorSistemaCodigoNome(short cdSistema, int? cdFornecedor, string nmFornecedor, Paging paging)
        {
            Assert(new { MarketingStructure = cdSistema }, new AllMustBeInformedSpec());

            return this.MainGateway.ObterListaPorSistemaCodigoNome(cdSistema, cdFornecedor, nmFornecedor, paging);
        }

        /// <summary>
        /// Obtém o fornecedor ativo com base no código e estrutura mercadológica.
        /// </summary>
        /// <param name="cdV9D">O código do fornecedor.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>Retorna o fornecedor juntamente com os parâmetros ativos.</returns>
        public Fornecedor ObterAtivoPorCodigoESistemaComProjecao(long cdV9D, int cdSistema)
        {
            Assert(new { cdV9D = cdV9D, cdSistema = cdSistema }, new AllMustBeInformedSpec());

            return this.MainGateway.ObterAtivoPorCodigoESistemaComProjecao(cdV9D, cdSistema);
        }

        /// <summary>
        /// Retorna true se o cdFornecedor informado é do tipo Walmart.
        /// </summary>
        /// <param name="cdFornecedor">O codigo do fornecedor.</param>
        /// <returns>Se o fornecedor é Walmart ou não.</returns>
        public bool VerificaVendorWalmart(long cdFornecedor)
        {
            Assert(new { SupplierCode = cdFornecedor }, new AllMustBeInformedSpec());

            return this.MainGateway.VerificaVendorWalmart(cdFornecedor);
        }
    }
}