using System;
using System.Collections.Generic;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Reabastecimento.Specs;
using Walmart.Sgp.Domain.Specs;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Helpers;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Serviço de domínio relacionado a Roteiro.
    /// </summary>
    public class RoteiroService : EntityDomainServiceBase<Roteiro, IRoteiroGateway>, IRoteiroService
    {
        private static String[] s_auditProperties = new String[] { "vlEstoque", "qtdPackCompra", "qtdSugestaoRoteiroRA" };
        private readonly IRoteiroLojaService m_roteiroLojaService;
        private readonly ISugestaoPedidoGateway m_sugestaoPedidoGateway;
        private readonly IAuditService m_auditService;
        private readonly IFornecedorParametroGateway m_fornecedorParametroGateway;

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="RoteiroService"/>.
        /// </summary>
        /// <param name="mainGateway">O table data gateway para Roteiro.</param>
        /// <param name="roteiroLojaService">O table data gateway para RoteiroLoja.</param>
        /// <param name="sugestaoPedidoGateway">O table data gateway para SugestaoPedido.</param>
        /// <param name="auditService">O serviço de auditoria.</param>
        /// <param name="fornecedorParametroGateway">O table data gateway para FornecedorParametro.</param>
        public RoteiroService(IRoteiroGateway mainGateway, IRoteiroLojaService roteiroLojaService, ISugestaoPedidoGateway sugestaoPedidoGateway, IAuditService auditService, IFornecedorParametroGateway fornecedorParametroGateway)
            : base(mainGateway)
        {
            m_roteiroLojaService = roteiroLojaService;
            m_sugestaoPedidoGateway = sugestaoPedidoGateway;
            m_auditService = auditService;
            m_fornecedorParametroGateway = fornecedorParametroGateway;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obtém os roteiros dos fornecedores.
        /// </summary>
        /// <param name="cdV9D">O código 9 dígitos do fornecedor.</param>
        /// <param name="cdDepartamento">O código do departamento.</param>
        /// <param name="cdLoja">O código da loja.</param>
        /// <param name="roteiro">O nome do roteiro.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>A lista contendo os roteiros dos fornecedores.</returns>
        public IEnumerable<Roteiro> ObterRoteirosPorFornecedor(long? cdV9D, int? cdDepartamento, int? cdLoja, string roteiro, Paging paging)
        {
            Assert(new { Vendor9Digits = cdV9D, Department = cdDepartamento, Store = cdLoja, Script = roteiro }, new AtLeastOneMustBeInformedSpec());

            return MainGateway.ObterRoteirosPorFornecedor(cdV9D, cdDepartamento, cdLoja, roteiro, paging);
        }

        /// <summary>
        /// Salva o roteiro informado
        /// </summary>
        /// <param name="entidade">O roteiro a ser salvo.</param>
        public override void Salvar(Roteiro entidade)
        {
            ValidarRoteiro(entidade);
            base.Salvar(entidade);
        }

        /// <summary>
        /// Obtém uma lista de SugestaoPedido com Loja populada
        /// </summary>
        /// <param name="idRoteiro">O id do roteiro.</param>
        /// <param name="dtPedido">A data do pedido.</param>
        /// <param name="idItemDetalhe">O id do item.</param>
        /// <param name="paging">A paginação. (OPCIONAL)</param>
        /// <returns>Uma lista de sugestaoPedido.</returns>
        public IEnumerable<SugestaoPedido> ObterSugestaoPedidoLoja(int idRoteiro, DateTime dtPedido, int idItemDetalhe, Paging paging)
        {
            Assert(new { IdRoteiro = idRoteiro, DtPedido = dtPedido, IdItemDetalhe = idItemDetalhe }, new AllMustBeInformedSpec());

            var result = this.MainGateway.ObterSugestaoPedidoLoja(idRoteiro, dtPedido, idItemDetalhe, paging);

            var roteiro = this.MainGateway.FindById(idRoteiro);

            // Code-review: if desnecessário, pois a lista nunca vem nula do table data gateway.
            if (result != null)
            {
                foreach (var r in result)
                {
                    // Code-review: não utilizar o ternário nessa situação, já que está sendo feita a verificação duas fezes.
                    // Utilizar um if-else.
                    r.ValorEmCaixa = roteiro.blKgCx ? r.QtdPackCompraToCaixa() : r.QtdPackCompraToQuilo();
                    r.ValorEmCaixaRA = roteiro.blKgCx ? r.QtdSugestaoRoteiroRAToCaixa() : r.QtdSugestaoRoteiroRAToQuilo();
                }
            }

            return result;
        }

        /// <summary>
        /// Salva uma lista de SugestaoPedido convertida para exibição em Caixa.
        /// </summary>
        /// <param name="sugestoesConvertidas">As sugestoes convertidas.</param>
        /// <param name="idRoteiro">O id do roteiro.</param>
        public void SalvarSugestaoPedidoConvertidoCaixa(IEnumerable<SugestaoPedido> sugestoesConvertidas, int idRoteiro)
        {
            Assert(new { SugestoesConvertidas = sugestoesConvertidas, IdRoteiro = idRoteiro }, new AllMustBeInformedSpec());

            var roteiro = this.MainGateway.FindById(idRoteiro);

            foreach (var sugestao in sugestoesConvertidas)
            {
                var entidade = this.m_sugestaoPedidoGateway.FindById(sugestao.IDSugestaoPedido);
                entidade.qtdSugestaoRoteiroRA = BuscaValorRA(sugestao, roteiro);

                this.m_sugestaoPedidoGateway.Update("qtdSugestaoRoteiroRA = @qtdSugestaoRoteiroRA", entidade);
                this.m_auditService.LogUpdate(entidade, s_auditProperties);
            }
        }

        /// <summary>
        /// Obtém um Roteiro estruturado pelo seu id.
        /// </summary>
        /// <param name="id">O id.</param>
        /// <returns>A entidade Roteiro.</returns>
        public Roteiro ObterEstruturadoPorId(int id)
        {
            return this.MainGateway.ObterEstruturadoPorId(id);
        }

        private static decimal BuscaValorRA(SugestaoPedido sugestao, Roteiro roteiro)
        {
            var valorNovoRA = Convert.ToDecimal(sugestao.ValorEmCaixaRA, RuntimeContext.Current.Culture);

            if (roteiro.blKgCx)
            {
                if (sugestao.TpCaixaFornecedor == EstruturaMercadologica.TipoCaixaFornecedor.KgOuUnidade)
                {
                    valorNovoRA = sugestao.ValorEmCaixaRA * sugestao.vlPesoLiquido.Value;
                }
            }
            else
            {
                if (sugestao.TpCaixaFornecedor == EstruturaMercadologica.TipoCaixaFornecedor.Caixa)
                {
                    var resultado = (int)CalcHelper.CustomDivision((double)sugestao.ValorEmCaixaRA, (double)sugestao.qtVendorPackage);
                    valorNovoRA = Convert.ToDecimal(resultado, RuntimeContext.Current.Culture);
                }
            }

            return valorNovoRA;
        }

        private void ValidarRoteiro(Roteiro roteiro)
        {
            Assert(new { CdDV9 = roteiro.cdV9D, orderScript = roteiro.Descricao }, new AllMustBeInformedSpec());
            Assert(new { minimalLoad = roteiro.vlCargaMinima }, new AllMustBeGraterThanSpec(0));
            Assert(roteiro.Lojas, new RegistroDeveSerSelecionadoSpec(() => roteiro.IDRoteiro == 0 ? new IRegistroSelecionavel[0] : (IEnumerable<IRegistroSelecionavel>)m_roteiroLojaService.ObterPorIdRoteiro(roteiro.IDRoteiro)));
            Assert(roteiro, new VendorRoteiroDeveConterItensDSDSpec(m_fornecedorParametroGateway.PossuiItensDSD));
        }
        #endregion
    }
}
