using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Define a interface do serviço de domínio relacionado a CD.
    /// </summary>
    public interface ICDService : IDomainService<CD>
    {
        /// <summary>
        /// Realiza a atualização do nome.
        /// </summary>
        /// <param name="idCD">O id do CD.</param>
        /// <param name="nome">O novo nome.</param>
        void AtualizarNomeCD(int idCD, string nome);

        /// <summary>
        /// Obtém um CD pelo seu id.
        /// </summary>
        /// <param name="id">O id.</param>
        /// <returns>A entidade CD.</returns>
        CD Obter(long id);

        /// <summary>
        /// Obtém o ID de um CD pelo seu Código.
        /// </summary>
        /// <param name="cdCD">O código do CD.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>ID do CD.</returns>
        int ObterIdCDPorCodigo(int cdCD, int cdSistema);

        /// <summary>
        /// Obtém todos os CDs convertidos ativos.
        /// </summary>
        /// <returns>Retorna a lista com todos os CDs convertidos ativos.</returns>
        IEnumerable<CD> ObterTodosConvertidosAtivos();
    }
}
