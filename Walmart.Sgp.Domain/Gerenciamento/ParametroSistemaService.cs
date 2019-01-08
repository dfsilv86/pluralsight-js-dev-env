using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Gerenciamento
{
    /// <summary>
    /// Serviço de domínio relacionado a parâmetro sistema.
    /// </summary>
    public class ParametroSistemaService : EntityDomainServiceBase<ParametroSistema, IParametroSistemaGateway>, IParametroSistemaService
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ParametroSistemaService"/>.
        /// </summary>
        /// <param name="mainGateway">O table data gateway para parâmetro sistema.</param>
        public ParametroSistemaService(IParametroSistemaGateway mainGateway)
            : base(mainGateway)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obtém o parâmetro sistema por nome
        /// </summary>
        /// <param name="nome">O nome.</param>
        /// <returns>O parâmetro.</returns>
        public ParametroSistema ObterPorNome(string nome)
        {
            return MainGateway.Find("nmParametroSistema = @nome", new { nome }).SingleOrDefault();
        }
        #endregion
    }
}
