using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Domain.Item
{
    /// <summary>
    /// Estratégia que será utilizada quando o tipo de relacionamento for de Vinculado.
    /// </summary>
    public class VinculadoTipoRelacionamentoStrategy : ITipoRelacionamentoStrategy
    {
        #region Fields
        private readonly IItemDetalheService m_itemDetalheService;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="VinculadoTipoRelacionamentoStrategy"/>.
        /// </summary>
        /// <param name="itemDetalheService">O serviço de item detalhe.</param>
        public VinculadoTipoRelacionamentoStrategy(IItemDetalheService itemDetalheService)
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
            m_itemDetalheService.AlterarVinculado(idItemDetalhe, TipoVinculado.Saida);
        }

        /// <summary>
        /// Desmarca o item detalhe como principal no relacionamento de itens.
        /// </summary>
        /// <param name="idItemDetalhe">O id do item detalhe.</param>
        public void DesmarcarItemDetalheComoPrincipal(int idItemDetalhe)
        {
            m_itemDetalheService.AlterarVinculado(idItemDetalhe, TipoVinculado.NaoDefinido);
        }

        /// <summary>
        /// Marca o item detalhe como secundario no relacionamento de itens.
        /// </summary>
        /// <param name="idItemDetalhe">O id do item detalhe.</param>
        public void MarcarItemDetalheComoSecundario(int idItemDetalhe)
        {
            m_itemDetalheService.AlterarVinculado(idItemDetalhe, TipoVinculado.Entrada);
        }

        /// <summary>
        /// Desmarca o item detalhe como secundario no relacionamento de itens.
        /// </summary>
        /// <param name="idItemDetalhe">O id do item detalhe.</param>
        public void DesmarcarItemDetalheComoSecundario(int idItemDetalhe)
        {
            m_itemDetalheService.AlterarVinculado(idItemDetalhe, TipoVinculado.NaoDefinido);
        }
        #endregion
    }
}
