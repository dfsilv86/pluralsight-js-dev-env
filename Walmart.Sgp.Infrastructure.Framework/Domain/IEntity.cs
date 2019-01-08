namespace Walmart.Sgp.Infrastructure.Framework.Domain
{
    /// <summary>
    /// Define a interface básica de uma entidade.
    /// </summary>
    public interface IEntity
    {
        #region Properties
        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Obtém um valor que indica se é uma nova entidade ou se já existia.
        /// </summary>
        bool IsNew { get;  }        
        #endregion
    }
}