using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Commons;
using Walmart.Sgp.Infrastructure.Framework.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Helpers;

namespace Walmart.Sgp.Domain.Acessos
{
    /// <summary>
    /// Representas as permissões de um usuário.
    /// </summary>
    public sealed class UsuarioPermissoes
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="UsuarioPermissoes"/>.
        /// </summary>
        /// <param name="idUsuario">O id do usuário.</param>
        /// <param name="bandeiras">As bandeiras que o usuário tem acesso.</param>
        /// <param name="lojas">As lojas que o usuário tem acesso.</param>
        public UsuarioPermissoes(int idUsuario, IEnumerable<Bandeira> bandeiras, IEnumerable<Loja> lojas)
        {            
            if (lojas.Any())
            {
                TipoPermissao = TipoPermissao.PorLoja;                
            }
            else if (bandeiras.Any())
            {
                TipoPermissao = TipoPermissao.PorBandeira;
            }
            else
            {
                throw new UserInvalidOperationException(Texts.PermissionsDoNotDefinedForUser);
            }

            IDUsuario = idUsuario;
            Bandeiras = bandeiras;
            Lojas = lojas;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém o id do usuário.
        /// </summary>
        public int IDUsuario { get; private set; }

        /// <summary>
        /// Obtém o tipo de permissão.
        /// </summary>
        public TipoPermissao TipoPermissao { get; private set; }

        /// <summary>
        /// Obtém as banderias que o usuário tem acesso.
        /// </summary>
        public IEnumerable<Bandeira> Bandeiras { get; private set; }

        /// <summary>
        /// Obtém as lojas que o usuário tem acesso.
        /// </summary>
        public IEnumerable<Loja> Lojas { get; private set; }
        #endregion
    }
}
