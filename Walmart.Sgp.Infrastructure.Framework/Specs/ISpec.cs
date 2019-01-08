namespace Walmart.Sgp.Infrastructure.Framework.Specs
{
    /// <summary>
    /// Define a interface de uma especificação.    
    /// </summary>
    /// <remarks>Specification Pattern: http://en.wikipedia.org/wiki/Specification_pattern.</remarks>
    /// <typeparam name="TTarget">O tipo de objeto que deve satisfazer a especificação.</typeparam>
    public interface ISpec<TTarget>
    {
        #region Methods                
        /// <summary>
        /// Verifica se o alvo informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo alvo.
        /// </returns>
        SpecResult IsSatisfiedBy(TTarget target);
        #endregion
    }
}
