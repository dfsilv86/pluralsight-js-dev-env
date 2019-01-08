namespace Walmart.Sgp.Infrastructure.Framework.Domain
{
    /// <summary>
    /// Define a interface de configuração de uma paginação.
    /// </summary>
    public interface IPaging
    {
        /// <summary>
        /// Obtém o índice de início da paginação.
        /// </summary>
        /// <remarks>
        /// A primeira linha de resultado é 0
        /// </remarks>
        int Offset { get; }

        /// <summary>
        /// Obtém a quantidade de linhas retornadas.
        /// </summary>
        int Limit { get; }        

        /// <summary>
        /// Obtém a ordenação.
        /// </summary>
        string OrderBy { get; }
    }
}