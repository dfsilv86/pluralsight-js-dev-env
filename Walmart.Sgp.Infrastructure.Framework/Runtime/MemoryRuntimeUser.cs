namespace Walmart.Sgp.Infrastructure.Framework.Runtime
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Walmart.Sgp.Infrastructure.Framework.Domain;
    using Walmart.Sgp.Infrastructure.Framework.Domain.Acessos;
    using Walmart.Sgp.Infrastructure.Framework.Helpers;

    /// <summary>
    /// Usuário de execução em memória.
    /// <remarks>Utilizado principalmente para questões de teste.</remarks>
    /// </summary>
    public class MemoryRuntimeUser : IRuntimeUser
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="MemoryRuntimeUser"/>.
        /// </summary>
        public MemoryRuntimeUser()
        {            
            Actions = new List<UserActionInfo>();
            TipoPermissao = TipoPermissao.PorBandeira;
            //// TODO: Preencher valor de bandeira
            BandeiraId = 1;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém ou define o Id.
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Obtém ou define o username.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Obtém ou define o nome completo.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Obtém ou define o e-mail.
        /// </summary> 
        public string Email { get; set; }

        /// <summary>
        /// Obtém um valor que indica se está autenticado.
        /// </summary>
        public bool IsAuthenticated
        {
            get
            {
                return Actions.Any();
            }
        }
        
        /// <summary>
        /// Obtém ou define id do papel.
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Obtém ou define o nome do papel.
        /// </summary> 
        public string RoleName { get; set; }

        /// <summary>
        /// Obtém ou define id da loja.
        /// </summary>
        public int? StoreId { get; set; }

        /// <summary>
        /// Obtém ou define um valor que indica se o usuário está associado a apenas uma única loja.
        /// </summary>
        public bool HasAccessToSingleStore { get; set; }

        /// <summary>
        /// Obtém ou define id da bandeira.
        /// </summary>
        public int? BandeiraId { get; set; }

        /// <summary>
        /// Obtém ou define as ações que o usuário não tem acesso.
        /// </summary> 
        public IEnumerable<UserActionInfo> Actions { get; set; }

        /// <summary>
        /// Obtém ou define um valor que indica se o usuário é de um papel de administrador.        
        /// </summary>
        /// <remarks>
        /// Utilizado ao salvar sugestão pedido.
        /// psqSugestaoPedido.aspx.cs linha 386
        /// </remarks>
        public bool IsAdministrator { get; set; }

        /// <summary>
        /// Obtém ou define um valor que indica se o usuário é de um papel de GA.
        /// </summary>
        /// <remarks>
        /// Utilizado para validar se é possivel editar um item de inventário.
        /// </remarks>
        public bool IsGa { get; set; }

        /// <summary>
        /// Obtém ou define um valor que indica se o usuário é de um papel de HO.
        /// </summary>
        /// <remarks>Utilizado para validar se é possível incluir uma loja para determinada bandeira.</remarks>
        public bool IsHo { get; set; }

        /// <summary>
        /// Obtém ou define o tipo de permissão do usuário.
        /// </summary>
        public TipoPermissao TipoPermissao { get; set; }

        #endregion

        #region Methods
        /// <summary>
        /// Verifica se o usuário possui qualquer um dos papéis informado.
        /// </summary>
        /// <param name="roleIds">Os ids dos papéis.</param>
        /// <returns>
        /// True se possui.
        /// </returns>
        public bool HasAnyRole(IEnumerable<int> roleIds)
        {
            return roleIds.Contains(RoleId);
        }

        /// <summary>
        /// Verifica se o usuário possui a permissão informada.
        /// </summary>
        /// <param name="permissionName">O nome da permissão.</param>
        /// <returns>
        ///   <c>true</c> se possui a permissão; caso contrário <c>false</c>.
        /// </returns>
        public bool HasPermission(string permissionName)
        {
            return permissionName != null && Actions.Any(a => permissionName.Equals(a.Id, StringComparison.OrdinalIgnoreCase));
        }

        #endregion
    }
}
