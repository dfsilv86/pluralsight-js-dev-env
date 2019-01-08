using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Item
{
    /// <summary>
    /// Factory para componentes relacionados a itens.
    /// </summary>
    public static class ItemFactory
    {
        /// <summary>
        /// Cria uma estratégia para tratar os relacionamentos de intes.
        /// </summary>
        /// <param name="itemPrincipal">O item principal no relacionamento.</param>
        /// <param name="itemDetalheService">O serviço de item detalhe.</param>
        /// <returns>A estratégia.</returns>
        public static ITipoRelacionamentoStrategy CreateTipoRelacionamentoStrategy(RelacionamentoItemPrincipal itemPrincipal, IItemDetalheService itemDetalheService)
        {
            var tipo = itemPrincipal.TipoRelacionamento;

            if (tipo == TipoRelacionamento.Vinculado)
            {
                return new VinculadoTipoRelacionamentoStrategy(itemDetalheService);
            }
            else if (tipo == TipoRelacionamento.Manipulado)
            {
                return new ManipuladoTipoRelacionamentoStrategy(itemDetalheService);
            }
            else 
            {
                return new ReceituarioTipoRelacionamentoStrategy(itemDetalheService);
            }
        }        
    }
}
