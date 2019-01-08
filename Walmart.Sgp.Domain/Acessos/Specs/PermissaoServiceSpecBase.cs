using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Acessos.Specs
{
    /// <summary>
    /// Classe base para especificações que utilizam o serviço de permissão.
    /// </summary>
    /// <typeparam name="TTarget">O tipo do alvo.</typeparam>
    public abstract class PermissaoServiceSpecBase<TTarget> : SpecBase<TTarget>
    {
        #region Constructors        
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="PermissaoServiceSpecBase{TTarget}"/>.
        /// </summary>
        /// <param name="permissaoService">O serviço de permissão.</param>
        protected PermissaoServiceSpecBase(IPermissaoService permissaoService)
        {
            PermissaoService = permissaoService;
        }
        #endregion

        #region Properties        
        /// <summary>
        /// Obtém o serviço de permissão.
        /// </summary>
        protected IPermissaoService PermissaoService { get; private set; }
        #endregion
    }
}
