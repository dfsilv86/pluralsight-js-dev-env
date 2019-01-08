using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Domain.Movimentacao
{
    /// <summary>
    /// Define a interface para um serviço de domínio para tipo de movimentação.
    /// </summary>
    public interface ITipoMovimentacaoService
    {
        /// <summary>
        /// Obtém tipos de movimentação da categoria informada.
        /// </summary>
        /// <param name="categoria">A categoria.</param>
        /// <returns>Os tipos de movimentação.</returns>
        IEnumerable<TipoMovimentacao> ObterPorCategoria(CategoriaTipoMovimentacao categoria);
    }
}
