using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.IO.Importing;

namespace Walmart.Sgp.Domain.Acessos
{
    /// <summary>
    /// Importador de usuário.
    /// </summary>
    public class UsuarioImporter : IImporter<UsuarioAcessoInfo>
    {
        #region Fields
        private readonly IUsuarioService m_usuarioService;
        private readonly IPapelService m_papelService;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="UsuarioImporter"/>.
        /// </summary>
        /// <param name="usuarioService">O serviço de usuário.</param>
        /// <param name="papelService">O serviço de papel.</param>
        public UsuarioImporter(IUsuarioService usuarioService, IPapelService papelService)
        {
            m_usuarioService = usuarioService;
            m_papelService = papelService;                  
        }
        #endregion

        #region Methods
        /// <summary>
        /// Importa o usuário utilizando as informações de acesso.
        /// </summary>
        /// <param name="input">A entrada para a importação.</param>        
        public void Import(UsuarioAcessoInfo input)
        {
            Validate(input);
            ImportarUsuario(input);
            ImportarPapel(input);
        }

        private static void Validate(UsuarioAcessoInfo input)
        {
            if (input.Usuario == null)
            {
                throw new ArgumentNullException("input", Texts.UserIsRequired);
            }  
        }      

        private void ImportarUsuario(UsuarioAcessoInfo input)
        {
            var usuarioParaSalvar = input.Usuario;
            var usuarioExistente = m_usuarioService.ObterPorUserName(usuarioParaSalvar.UserName);

            if (usuarioExistente != null)
            {
                usuarioParaSalvar.Id = usuarioExistente.Id;
            }

            m_usuarioService.Salvar(usuarioParaSalvar);
        }

        private void ImportarPapel(UsuarioAcessoInfo input)
        {            
            var papelParaSalvar = input.Papel;

            if (papelParaSalvar != null)
            {
                var papelExistente = m_papelService.ObterPorNome(papelParaSalvar.Name);

                // Se o papel ainda não existe cria, se já existir não precisa atualizar.
                if (papelExistente == null)
                {
                    m_papelService.Salvar(papelParaSalvar);
                }
                else
                {
                    papelParaSalvar.Id = papelExistente.Id;
                }
            }
        }                
        #endregion
    }
}
