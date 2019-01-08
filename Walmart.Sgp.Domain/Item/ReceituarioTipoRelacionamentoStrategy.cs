using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Domain.Item
{
    /// <summary>
    /// Estratégia que será utilizada quando o tipo de relacionamento for de Receituário.
    /// </summary>
    public class ReceituarioTipoRelacionamentoStrategy : ITipoRelacionamentoStrategy
    {
        #region Fields
        private readonly IItemDetalheService m_itemDetalheService;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ReceituarioTipoRelacionamentoStrategy"/>.
        /// </summary>
        /// <param name="itemDetalheService">O serviço de item detalhe.</param>
        public ReceituarioTipoRelacionamentoStrategy(IItemDetalheService itemDetalheService)
        {
            m_itemDetalheService = itemDetalheService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Marca o item detalhe como principal no relacionamento de itens.
        /// </summary>
        /// <param name="idItemDetalhe">O id do item detalhe.</param>
        public void MarcarItemDetalheComoPrincipal(int idItemDetalhe)
        {
            m_itemDetalheService.AlterarReceituario(idItemDetalhe, TipoReceituario.Transformado);
        }

        /// <summary>
        /// Desmarca o item detalhe como principal no relacionamento de itens.
        /// </summary>
        /// <param name="idItemDetalhe">O id do item detalhe.</param>
        public void DesmarcarItemDetalheComoPrincipal(int idItemDetalhe)
        {
            m_itemDetalheService.AlterarReceituario(idItemDetalhe, TipoReceituario.NaoDefinido);
        }

        /// <summary>
        /// Marca o item detalhe como secundario no relacionamento de itens.
        /// </summary>
        /// <param name="idItemDetalhe">O id do item detalhe.</param>
        public void MarcarItemDetalheComoSecundario(int idItemDetalhe)
        {
            m_itemDetalheService.AlterarReceituario(idItemDetalhe, TipoReceituario.Insumo);
        }

        /// <summary>
        /// Desmarca o item detalhe como secundario no relacionamento de itens.
        /// </summary>
        /// <param name="idItemDetalhe">O id do item detalhe.</param>
        public void DesmarcarItemDetalheComoSecundario(int idItemDetalhe)
        {
            m_itemDetalheService.AlterarReceituario(idItemDetalhe, TipoReceituario.NaoDefinido);
        }
        #endregion
    }
}
