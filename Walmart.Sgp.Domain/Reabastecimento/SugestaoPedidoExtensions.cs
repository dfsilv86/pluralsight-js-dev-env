using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Item;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Extension methods para sugestão pedido.
    /// </summary>
    public static class SugestaoPedidoExtensions
    {
        //// Quando
        //// qtVendorPackage    DSD    blCDConvertido    tpCaixaFornecedor     Então
        ////        1            N            0                  F              Valida multiplo de peso bruto
        ////        1            N            0                  V              Valida multiplo de peso bruto
        ////        1            N            1                  F              Nao valida multiplo
        ////        1            N            1                  V              Valida multiplo de peso liquido
        ////        1            S            0                  F              Nao valida multiplo
        ////        1            S            0                  V              Valida multiplo de peso liquido
        ////        1            S            1                  F              Nao valida multiplo
        ////        1            S            1                  V              Valida multiplo de peso liquido
        ////       >1            N            0                  F              Nao valida multiplo
        ////       >1            N            0                  V              Nao valida multiplo
        ////       >1            N            1                  F              Nao valida multiplo
        ////       >1            N            1                  V              Valida multiplo de peso liquido
        ////       >1            S            0                  F              Nao valida multiplo
        ////       >1            S            0                  V              Valida multiplo de peso liquido
        ////       >1            S            1                  F              Nao valida multiplo
        ////       >1            S            1                  V              Valida multiplo de peso liquido
        
        /// <summary>
        /// Obtém o peso líquido ou bruto conforme tipo de sugestão.
        /// </summary>
        /// <param name="sugestaoPedido">A sugestão de pedido.</param>
        /// <returns>O peso líquido ou bruto.</returns>
        public static decimal ObterPeso(this SugestaoPedido sugestaoPedido)
        {
            if (sugestaoPedido.IsPesoLiquido())
            {
                return sugestaoPedido.vlPesoLiquido ?? 0;
            }

            // Se não for peso líquido, retorna o peso bruto.
            return sugestaoPedido.vlModulo;
        }

        /// <summary>
        /// Obtém o pack conforme tipo de sugestão.
        /// </summary>
        /// <param name="sugestaoPedido">A sugestão de pedido.</param>
        /// <returns>O pack.</returns>
        public static decimal ObterPack(this SugestaoPedido sugestaoPedido)
        {
            if (sugestaoPedido.IsPesoLiquido())
            {
                return sugestaoPedido.vlPesoLiquido ?? sugestaoPedido.qtVendorPackage;
            }
            else if (sugestaoPedido.IsPesoBruto())
            {
                return sugestaoPedido.vlModulo;
            }

            return sugestaoPedido.qtVendorPackage;
        }

        /// <summary>
        /// Determina se a sugestão pedido é de um item onde o reabastecimento é via DSD.
        /// </summary>
        /// <param name="sugestaoPedido">A sugestão de pedido.</param>
        /// <returns>True se for DSD (tipos reabastecimento 7, 37, 97), false caso contrário.</returns>
        public static bool IsDsd(this SugestaoPedido sugestaoPedido)
        {
            return sugestaoPedido.vlTipoReabastecimento == ValorTipoReabastecimento.Dsd37 || sugestaoPedido.vlTipoReabastecimento == ValorTipoReabastecimento.Dsd7 || sugestaoPedido.vlTipoReabastecimento == ValorTipoReabastecimento.Dsd97;
        }

        /// <summary>
        /// Determina se a sugestão pedido do item deve usar peso líquido.
        /// </summary>
        /// <param name="sugestaoPedido">A sugestão de pedido.</param>
        /// <returns>True se for DSD (tipos reabastecimento 7, 37, 97) ou se for CD convertido, false caso contrário.</returns>
        public static bool IsDsdOuConvertido(this SugestaoPedido sugestaoPedido)
        {
            return sugestaoPedido.IsDsd() || sugestaoPedido.blCDConvertido;
        }

        /// <summary>
        /// Determina se a sugestão pedido deve usar o peso liquido.
        /// </summary>
        /// <param name="sugestaoPedido">A sugestão de pedido.</param>
        /// <returns>True caso deva usar peso liquido (se for tipo caixa fornecedor Variavel E (seja DSD OU CD convertido))</returns>
        public static bool IsPesoLiquido(this SugestaoPedido sugestaoPedido)
        {
            return sugestaoPedido.IsDsdOuConvertido() && sugestaoPedido.TpCaixaFornecedor == TipoCaixaFornecedor.KgOuUnidade;
        }

        /// <summary>
        /// Determina se a sugestão pedido deve usar o peso liquido.
        /// </summary>
        /// <param name="sugestaoPedido">A sugestão de pedido.</param>
        /// <returns>True caso deva usar peso bruto (qtdVendorPackage = 1 E não for DSD nem CD convertido)</returns>
        public static bool IsPesoBruto(this SugestaoPedido sugestaoPedido)
        {
            return sugestaoPedido.qtVendorPackage == 1 && !sugestaoPedido.IsDsdOuConvertido();
        }
    }
}
