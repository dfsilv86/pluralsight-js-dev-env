using System.Diagnostics.CodeAnalysis;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Acessos
{
    /// <summary>
    /// Define a interface de um table data gateway para usuário.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1040:AvoidEmptyInterfaces")]
    public interface IUsuarioGateway : IDataGateway<Usuario>
    {
    }
}