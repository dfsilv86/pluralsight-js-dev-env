using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Movimentacao;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Item
{
    /// <summary>
    /// Define a interface da estratégia que será utilizada para um tipo de relacionamento de itens.
    /// </summary>
    public interface ITipoRelacionamentoStrategy
    {
        /// <summary>
        /// Marca o item detalhe como principal no relacionamento de itens.
        /// </summary>
        /// <param name="idItemDetalhe">O id do item detalhe.</param>
        void MarcarItemDetalheComoPrincipal(int idItemDetalhe);

        /// <summary>
        /// Desmarca o item detalhe como principal no relacionamento de itens.
        /// </summary>
        /// <param name="idItemDetalhe">O id do item detalhe.</param>
        void DesmarcarItemDetalheComoPrincipal(int idItemDetalhe);

        /// <summary>
        /// Marca o item detalhe como secundario no relacionamento de itens.
        /// </summary>
        /// <param name="idItemDetalhe">O id do item detalhe.</param>
        void MarcarItemDetalheComoSecundario(int idItemDetalhe);

        /// <summary>
        /// Desmarca o item detalhe como secundario no relacionamento de itens.
        /// </summary>
        /// <param name="idItemDetalhe">O id do item detalhe.</param>
        void DesmarcarItemDetalheComoSecundario(int idItemDetalhe);        
    }
}
