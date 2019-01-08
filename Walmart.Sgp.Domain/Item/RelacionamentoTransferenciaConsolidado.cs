using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Item
{
    /// <summary>
    /// Representa uma RelacionamentoTransferenciaConsolidado.
    /// </summary>
    public class RelacionamentoTransferenciaConsolidado : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// Obtém ou define IDRelacionamentoTransferencia.
        /// </summary>
        public int idRelacionamento { get; set; }
        
        /// <summary>
        /// Obtém ou define Bandeira.
        /// </summary>
        public string dsBandeira { get; set; }
        
        /// <summary>
        /// Obtém ou define Loja.
        /// </summary>
        public string dsLoja { get; set; }
        
        /// <summary>
        /// Obtém ou define DepartamentoItemDestino.
        /// </summary>
        public string dsDepartamentoItemDestino { get; set; }
        
        /// <summary>
        /// Obtém ou define cdItemDestino.
        /// </summary>
        public long cdItemDestino { get; set; }
        
        /// <summary>
        /// Obtém ou define dsItemDestino.
        /// </summary>
        public string dsItemDestino { get; set; }
        
        /// <summary>
        /// Obtém ou define fatorConversaoItemDestino.
        /// </summary>
        public decimal fatorConversaoItemDestino { get; set; }
        
        /// <summary>
        /// Obtém ou define unidadeMedidaItemDestino.
        /// </summary>
        public string unidadeMedidaItemDestino { get; set; }
        
        /// <summary>
        /// Obtém ou define DepartamentoItemOrigem.
        /// </summary>
        public string dsDepartamentoItemOrigem { get; set; }
        
        /// <summary>
        /// Obtém ou define cdItemOrigem.
        /// </summary>
        public long cdItemOrigem { get; set; }
        
        /// <summary>
        /// Obtém ou define dsItemOrigem.
        /// </summary>
        public string dsItemOrigem { get; set; }
        
        /// <summary>
        /// Obtém ou define fatorConversaoItemOrigem.
        /// </summary>
        public string fatorConversaoItemOrigem { get; set; }
        
        /// <summary>
        /// Obtém ou define unidadeMedidaItemOrigem.
        /// </summary>
        public string unidadeMedidaItemOrigem { get; set; }
    }
}
