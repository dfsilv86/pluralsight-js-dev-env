namespace Walmart.Sgp.Infrastructure.IO.Importing.WebGuardian
{
    /// <summary>
    /// Resultado de usuário do WebGuardian.
    /// </summary>
    public partial class UsuarioResultado : IWebGuardianResultado<UsuarioTO>
    {
        /// <summary>
        /// Obtém o dado.
        /// </summary>
        public UsuarioTO Dado
        {
            get { return Usuario; }
        }
    }
}
