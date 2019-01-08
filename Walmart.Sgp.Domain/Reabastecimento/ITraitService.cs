using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Define a interface para serviço de cadastro de Trait.
    /// </summary>
    public interface ITraitService : IDomainService<Trait>
    {
        /// <summary>
        /// Verifica se existe Trait para uma loja/item.
        /// </summary>
        /// <param name="cdItem">O codigo do item.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <param name="cdLoja">codigo da loja.</param>
        /// <returns>True se possui trait.</returns>
        bool PossuiTrait(long cdItem, long cdSistema, long cdLoja);
    }
}