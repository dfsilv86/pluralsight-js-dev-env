namespace Walmart.Sgp.Infrastructure.Data.Dtos
{
    /// <summary>
    /// Representa as informações de usuário do inventário.
    /// </summary>
    public class InventarioUsuariosDto
    {
        /// <summary>
        /// Obtém ou define o id do usuario abertura loja.
        /// </summary>
        public int? UsuarioAberturaLojaId { get; set; }

        /// <summary>
        /// Obtém ou define o usuario abertura loja username.
        /// </summary>
        public string UsuarioAberturaLojaUserName { get; set; }

        /// <summary>
        /// Obtém ou define o usuario importacao identifier.
        /// </summary>
        public int? UsuarioImportacaoId { get; set; }

        /// <summary>
        /// Obtém ou define o usuario importacao username.
        /// </summary>
        public string UsuarioImportacaoUserName { get; set; }

        /// <summary>
        /// Obtém ou define o usuario finalizacao loja identifier.
        /// </summary>
        public int? UsuarioFinalizacaoLojaId { get; set; }

        /// <summary>
        /// Obtém ou define o usuario finalizacao loja username.
        /// </summary>
        public string UsuarioFinalizacaoLojaUserName { get; set; }

        /// <summary>
        /// Obtém ou define o usuario aprovacao loja identifier.
        /// </summary>
        public int? UsuarioAprovacaoLojaId { get; set; }

        /// <summary>
        /// Obtém ou define o usuario aprovacao loja username.
        /// </summary>
        public string UsuarioAprovacaoLojaUserName { get; set; }
    }
}
