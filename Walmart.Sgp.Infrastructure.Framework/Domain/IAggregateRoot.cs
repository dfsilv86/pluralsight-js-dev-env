using System.Diagnostics.CodeAnalysis;

namespace Walmart.Sgp.Infrastructure.Framework.Domain
{
    /// <summary>
    /// Define a interface de marcação para um aggregate root.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1040:AvoidEmptyInterfaces")]
    public interface IAggregateRoot : IEntity
    {
    }
}