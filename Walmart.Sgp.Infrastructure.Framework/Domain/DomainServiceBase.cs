using System.Collections.Generic;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Infrastructure.Framework.Domain
{
    /// <summary>
    /// Classe base para serviços de domínio.
    /// </summary>
    /// <typeparam name="TMainDataGateway">O gateway de acesso a dados principal.</typeparam>
    public abstract class DomainServiceBase<TMainDataGateway>
    {
        #region Constructors       
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DomainServiceBase{TMainDataGateway}"/>.
        /// </summary>
        /// <param name="mainGateway">O table data gateway para main data.</param>
        protected DomainServiceBase(TMainDataGateway mainGateway)
        {
            MainGateway = mainGateway;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém o table data gateway principal.
        /// </summary>
        protected TMainDataGateway MainGateway { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Verifica se todas as especificações são satisfeitas. Se uma única não for, será lançada um SpecificationNotSatisfiedException.
        /// </summary>
        /// <typeparam name="TTarget">O tipo do alvo da especificações.</typeparam>
        /// <param name="target">O alvo da especificações.</param>
        /// <param name="specifications">As especificações.</param>
        protected static void Assert<TTarget>(TTarget target, params ISpec<TTarget>[] specifications)
        {
            SpecService.Assert(target, specifications);
        }

        /// <summary>
        /// Verifica se todas as especificações são satisfeitas. Se uma única não for, será lançada um SpecificationNotSatisfiedException.
        /// </summary>
        /// <typeparam name="TTarget">O tipo do alvo da especificações.</typeparam>
        /// <param name="targets">Os alvos das especificações.</param>
        /// <param name="specifications">As especificações.</param>
        protected static void Assert<TTarget>(IEnumerable<TTarget> targets, params ISpec<TTarget>[] specifications)
        {
            SpecService.Assert(targets, specifications);
        }

        /// <summary>
        /// Verifica se todas as especificações são satisfeitas. A final se pelo menos uma única não for, será lançada um SpecificationNotSatisfiedException com a razão de todas que não foram satisfietas.
        /// </summary>
        /// <typeparam name="TTarget">O tipo do alvo da especificações.</typeparam>
        /// <param name="targets">Os alvos das especificações.</param>
        /// <param name="specifications">As especificações.</param>
        protected static void AssertAll<TTarget>(IEnumerable<TTarget> targets, params ISpec<TTarget>[] specifications)
        {
            SpecService.AssertAll(targets, specifications);
        }
        #endregion
    }
}
