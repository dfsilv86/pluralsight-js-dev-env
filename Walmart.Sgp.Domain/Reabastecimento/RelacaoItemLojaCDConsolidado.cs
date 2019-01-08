using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Representa uma RelacaoItemLojaCDConsolidado.
    /// </summary>
    public class RelacaoItemLojaCDConsolidado : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// Obtém ou define RelacaoItemLojaCD.
        /// </summary>
        public RelacaoItemLojaCD RelacaoItemLojaCD { get; set; }

        /// <summary>
        /// Obtém ou define cdLoja.
        /// </summary>
        public int cdLoja { get; set; }

        /// <summary>
        /// Obtém ou define o nome da loja (nmLoja)
        /// </summary>
        public string nmLoja { get; set; }

        /// <summary>
        /// Obtém ou define dsEstado.
        /// </summary>
        public string dsEstado { get; set; }

        /// <summary>
        /// Obtém ou define dsBandeira.
        /// </summary>
        public string dsBandeira { get; set; }

        /// <summary>
        /// Obtém ou define cdCD.
        /// </summary>
        public int cdCD { get; set; }

        /// <summary>
        /// Obtem ou define o id do CD
        /// </summary>
        public int idCD { get; set; }

        /// <summary>
        /// Obtém ou define dsRegiaoCompra.
        /// </summary>
        public string dsRegiaoCompra { get; set; }

        /// <summary>
        /// Obtem ou define ItemEntrada
        /// </summary>
        public ItemDetalhe ItemEntrada { get; set; }

        /// <summary>
        /// Lista de itens de entrada disponiveis
        /// </summary>
        public IEnumerable<ItemDetalhe> ItensDisponiveis { get; set; }

        /// <summary>
        /// Obtém ou define o cdItemPESS
        /// </summary>
        public int cdItemPess { get; set; }

        #region Propriedades Calculadas
        /// <summary>
        /// Obtem blPossuiItensDisponiveisXDock
        /// </summary>
        public bool blPossuiItensXDockDisponiveis
        {
            get
            {
                if (ItensDisponiveis == null)
                { 
                    return false;
                }
                
                return ItensDisponiveis.Any(i => i.IsXDock && i.IDItemDetalhe != (RelacaoItemLojaCD.IdItemEntrada ?? 0));
            }
        }

        /// <summary>
        /// Obtem blPossuiItensDSDDisponiveis
        /// </summary>
        public bool blPossuiItensDSDDisponiveis
        {
            get
            {
                if (ItensDisponiveis == null)
                {
                    return false;
                }

                return ItensDisponiveis.Any(i => i.IsDSD && i.IDItemDetalhe != (RelacaoItemLojaCD.IdItemEntrada ?? 0));
            }
        }

        /// <summary>
        /// Obtem blPossuiItensStapleDisponiveis
        /// </summary>
        public bool blPossuiItensStapleDisponiveis
        {
            get
            {
                if (ItensDisponiveis == null)
                {
                    return false;
                }

                return ItensDisponiveis.Any(i => i.IsStaple && i.IDItemDetalhe != (RelacaoItemLojaCD.IdItemEntrada ?? 0));
            }
        }
        #endregion
    }
}
