namespace Walmart.Sgp.Infrastructure.IO.Importing.WebGuardian
{
    /// <summary>
    /// Resultado de permissões do WebGuardian.
    /// </summary>
    public partial class PermissoesResultado : IWebGuardianResultado<PermissoesTO>
    {
        /// <summary>
        /// Obtém o dado.
        /// </summary>
        public PermissoesTO Dado
        {
            get
            {                                
                return Permissoes;
            }
        }
    }
}
