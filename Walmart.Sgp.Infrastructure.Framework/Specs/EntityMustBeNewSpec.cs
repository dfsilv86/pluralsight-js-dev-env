using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Infrastructure.Framework.Specs
{
    /// <summary>
    /// Especificação referente a se a entidade é nova.
    /// </summary>
    public class EntityMustBeNewSpec : SpecBase<IEntity>
    {
        /// <summary>
        /// Verifica se a entidade informada satisfaz a especificação.
        /// </summary>
        /// <param name="target">A entidade.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pela entidade.
        /// </returns>
        public override SpecResult IsSatisfiedBy(IEntity target)
        {
            return target.IsNew ? Satisfied() : NotSatisfied(Texts.EntityMustBeNew);
        }
    }
}
