namespace Walmart.Sgp.Infrastructure.Framework.Runtime
{
    using System.Collections.Generic;
    using Walmart.Sgp.Infrastructure.Framework.Domain;
    using Walmart.Sgp.Infrastructure.Framework.Domain.Acessos;

    #region Enums
    /// <summary>
    /// Tipos de usuários.
    /// </summary>
    public enum UserKind
    {
        /// <summary>
        /// Usuário convidado.
        /// </summary>
        Guest = 0,

        /// <summary>
        /// Usuário autenticado.
        /// </summary>
        AuthenticatedUser = 1
    }
    #endregion

    /// <summary>
    /// Define a interface de usuário que pode executar a aplicação.
    /// </summary>
    public interface IRuntimeUser
    {
        #region Properties
        /// <summary>
        /// Obtém o Id.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Obtém o username.
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// Obtém o nome completo.
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// Obtém o e-mail.
        /// </summary> 
        string Email { get; }

        /// <summary>
        /// Obtém um valor que indica se está autenticado.
        /// </summary>
        bool IsAuthenticated { get; }

        /// <summary>
        /// Obtém o id do papel.
        /// </summary>
        int RoleId { get; }

        /// <summary>
        /// Obtém o nome do papel.
        /// </summary> 
        string RoleName { get; }

        /// <summary>
        /// Obtém ou define id da loja.
        /// </summary>
        int? StoreId { get; set; }

        /// <summary>
        /// Obtém ou define um valor que indica se o usuário está associado a apenas uma única loja.
        /// </summary>
        bool HasAccessToSingleStore { get; set; }

        /// <summary>
        /// Obtém ou define id da bandeira.
        /// </summary>
        int? BandeiraId { get; set; }

        /// <summary>
        /// Obtém ou define as ações que o usuário  tem acesso.
        /// </summary> 
        IEnumerable<UserActionInfo> Actions { get; set; }

        /// <summary>
        /// Obtém um valor que indica se o usuário é de um papel de administrador.
        /// </summary>
        /// <remarks>Utilizado ao salvar sugestão pedido.</remarks>
        bool IsAdministrator { get; }

        /// <summary>
        /// Obtém ou define um valor que indica se o usuário é de um papel de GA.
        /// </summary>
        /// <remarks>Utilizado para validar se é possivel editar um item de inventário.</remarks>
        bool IsGa { get; set; }

        /// <summary>
        /// Obtém ou define um valor que indica se o usuário é de um papel de HO.
        /// </summary>
        /// <remarks>Utilizado para validar se é possível incluir uma loja para determinada bandeira.</remarks>
        bool IsHo { get; set; }

        /// <summary>
        /// Obtém ou define o tipo de permissão do usuário.
        /// </summary>
        TipoPermissao TipoPermissao { get; set; }

        #endregion

        #region Methods
        /// <summary>
        /// Verifica se o usuário possui qualquer um dos papéis informado.
        /// </summary>
        /// <param name="roleIds">Os ids dos papéis.</param>
        /// <returns>True se possui.</returns>
        bool HasAnyRole(IEnumerable<int> roleIds);

        /// <summary>
        /// Verifica se o usuário possui a permissão informada.
        /// </summary>
        /// <param name="permissionName">O nome da permissão.</param>
        /// <returns><c>true</c> se possui a permissão; caso contrário <c>false</c>.</returns>
        bool HasPermission(string permissionName);

        #endregion
    }
}
