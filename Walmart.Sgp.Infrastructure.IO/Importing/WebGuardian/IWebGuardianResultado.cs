namespace Walmart.Sgp.Infrastructure.IO.Importing.WebGuardian
{
    /// <summary>
    /// Define a interface de um resultado do WebGuardian.
    /// </summary>
    public interface IWebGuardianResultado
    {
        /// <summary>
        /// Obtém o status.
        /// </summary>
        Status Status { get; }
    }

    /// <summary>
    /// Define a interface de um resultado do WebGuardian.
    /// </summary>
    /// <typeparam name="TDado">O tipo de dado do resultado.</typeparam>
    public interface IWebGuardianResultado<TDado> : IWebGuardianResultado
    {        
        /// <summary>
        /// Obtém o dado.
        /// </summary>
        TDado Dado { get; }
    }
}
