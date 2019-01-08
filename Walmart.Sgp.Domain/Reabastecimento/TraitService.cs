using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Serviço de domínio relacionado a Trait.
    /// </summary>
    public class TraitService : EntityDomainServiceBase<Trait, ITraitGateway>, ITraitService
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="TraitService"/>.
        /// </summary>
        /// <param name="mainGateway">O table data gateway para Trait.</param>
        public TraitService(ITraitGateway mainGateway)
            : base(mainGateway)
        {
        }

        /// <summary>
        /// Verifica se existe Trait para uma loja/item.
        /// </summary>
        /// <param name="cdItem">O codigo do item.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <param name="cdLoja">codigo da loja.</param>
        /// <returns>True se possui trait.</returns>
        public bool PossuiTrait(long cdItem, long cdSistema, long cdLoja)
        {
            Assert(new { Item = cdItem, System = cdSistema, Store = cdLoja }, new AllMustBeInformedSpec());

            return this.MainGateway.PossuiTrait(cdItem, cdSistema, cdLoja);
        }
        #endregion
    }
}