using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Inventarios.Specs
{
    /// <summary>
    /// Especificação referente a existem datas abertas para importação de inventário.
    /// </summary>
    public class DatasDevemEstarAbertasParaImportacaoInventarioSpec : SpecBase<Inventario>
    {
        #region Fields
        private readonly IInventarioService m_inventarioService;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DatasDevemEstarAbertasParaImportacaoInventarioSpec"/>.
        /// </summary>
        /// <param name="inventarioService">O serviço de inventário.</param>
        public DatasDevemEstarAbertasParaImportacaoInventarioSpec(IInventarioService inventarioService)
        {
            m_inventarioService = inventarioService;
        }
        #endregion

        /// <summary>
        /// Verifica se as datas especificadas satisfazem a especificação.
        /// </summary>
        /// <param name="target">O inventário.</param>
        /// <returns>
        /// Se a especificação foi satisfeita.
        /// </returns>
        public override SpecResult IsSatisfiedBy(Inventario target)
        {
            var inventarios = m_inventarioService.ObterInventariosAbertosParaImportacao(target.IDLoja, null, null, null);

            if (inventarios == null || inventarios.Count() == 0)
            {
                return NotSatisfied(Texts.ThereAreNoOpenInventory);
            }

            return Satisfied();
        }
    }
}
