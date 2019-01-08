using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Commons;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Serviço de domínio relacionado a autoriza pedido.
    /// </summary>
    public class AutorizaPedidoService : EntityDomainServiceBase<AutorizaPedido, IAutorizaPedidoGateway>, IAutorizaPedidoService
    {
        #region Constructor
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="AutorizaPedidoService"/>.
        /// </summary>
        /// <param name="mainGateway">O table data gateway principal.</param>
        public AutorizaPedidoService(IAutorizaPedidoGateway mainGateway)
            : base(mainGateway)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Verifica se existe autorização de pedido para a data, loja e departamento informada.
        /// </summary>
        /// <param name="dtPedido">A data.</param>
        /// <param name="idLoja">O id de loja.</param>
        /// <param name="idDepartamento">O id de departamento.</param>
        /// <returns>
        /// True caso exista autorização, false caso contrário.
        /// </returns>
        public bool ExisteAutorizacaoPedido(DateTime dtPedido, int idLoja, int idDepartamento)
        {
            Assert(new { dtPedido, idLoja, idDepartamento }, new AllMustBeInformedSpec());

            return this.MainGateway.Find("DtPedido=@dtPedido AND IDLoja=@idLoja AND IDDepartamento=@idDepartamento", new { dtPedido, idLoja, idDepartamento }).Any();
        }

        /// <summary>
        /// Cria o registro de autoriza pedido.
        /// </summary>
        /// <param name="dtPedido">A data.</param>
        /// <param name="idLoja">O id de loja.</param>
        /// <param name="idDepartamento">O id de departamento.</param>
        public void AutorizarPedido(DateTime dtPedido, int idLoja, int idDepartamento)
        {
            Assert(new { dtPedido, idLoja, idDepartamento }, new AllMustBeInformedSpec());

            dtPedido = dtPedido.Date;

            if (ExisteAutorizacaoPedido(dtPedido, idLoja, idDepartamento))
            {
                throw new UserInvalidOperationException(Texts.OrderAuthorizationAlreadyExists);
            }

            AutorizaPedido autorizaPedido = new AutorizaPedido
            {
                dtAutorizacao = DateTime.Now,
                dtPedido = dtPedido,
                IdDepartamento = idDepartamento,
                IdLoja = idLoja,
                IdUser = RuntimeContext.Current.User.Id
            };

            this.Salvar(autorizaPedido);
        }

        /// <summary>
        /// Obtém as autorizações de pedido pelo id da sugestão de pedido.
        /// </summary>
        /// <param name="idSugestaoPedido">O id da sugestão de pedido.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>As autorizações.</returns>
        public IEnumerable<AutorizaPedido> ObterAutorizacoesPorSugestaoPedido(int idSugestaoPedido, Paging paging)
        {
            return MainGateway.ObterAutorizacoesPorSugestaoPedido(idSugestaoPedido, paging);
        }
        #endregion
    }
}
