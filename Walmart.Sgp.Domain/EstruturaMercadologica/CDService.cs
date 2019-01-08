using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Serviço de domínio relacionado a CD.
    /// </summary>
    public class CDService : EntityDomainServiceBase<CD, ICDGateway>, ICDService
    {
        #region Constructor
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="CDService"/>.
        /// </summary>
        /// <param name="mainGateway">O table data gateway para CD.</param>
        public CDService(ICDGateway mainGateway)
            : base(mainGateway)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Realiza a atualização do nome.
        /// </summary>
        /// <param name="idCD">O id do CD.</param>
        /// <param name="nome">O novo nome.</param>
        public void AtualizarNomeCD(int idCD, string nome)
        {
            MainGateway.Update("nmNome = @nome", "IDCD = @idCD", new { idCD, nome });
        }

        /// <summary>
        /// Obtém um CD pelo seu id.
        /// </summary>
        /// <param name="id">O id.</param>
        /// <returns>A entidade CD.</returns>
        public CD Obter(long id)
        {
            return this.MainGateway.Obter(id);
        }

        /// <summary>
        /// Obtém o ID de um CD pelo seu Código.
        /// </summary>
        /// <param name="cdCD">O código do CD.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>ID do CD.</returns>
        public int ObterIdCDPorCodigo(int cdCD, int cdSistema)
        {
            return this.MainGateway.ObterIdCDPorCodigo(cdCD, cdSistema);
        }

        /// <summary>
        /// Obtém todos os CDs convertidos ativos.
        /// </summary>
        /// <returns>Retorna a lista com todos os CDs convertidos ativos.</returns>
        public IEnumerable<CD> ObterTodosConvertidosAtivos()
        {
            return this.MainGateway.ObterTodosConvertidosAtivos();
        }
        #endregion
    }
}
