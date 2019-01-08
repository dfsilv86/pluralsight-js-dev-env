using System;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Inventarios
{
    /// <summary>
    /// Representa uma FechamentoFiscal.
    /// </summary>
    public class FechamentoFiscal : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public override int Id
        {
            get
            {
                return IDFechamentoFiscal;
            }

            set
            {
                IDFechamentoFiscal = value;
            }
        }

        /// <summary>
        /// Obtém ou define IDFechamentoFiscal.
        /// </summary>
        public int IDFechamentoFiscal { get; set; }

        /// <summary>
        /// Obtém ou define IDLoja.
        /// </summary>
        public int IDLoja { get; set; }

        /// <summary>
        /// Obtém ou define nrAno.
        /// </summary>
        public int? nrAno { get; set; }

        /// <summary>
        /// Obtém ou define nrMes.
        /// </summary>
        public byte? nrMes { get; set; }

        /// <summary>
        /// Obtém ou define dhFechamentoFiscal.
        /// </summary>
        public DateTime? dhFechamentoFiscal { get; set; }

        /// <summary>
        /// Obtém ou define dhContabilizacao.
        /// </summary>
        public DateTime? dhContabilizacao { get; set; }

        /// <summary>
        /// Obtém ou define dhWarehouseInventory.
        /// </summary>
        public DateTime? dhWarehouseInventory { get; set; }

        /// <summary>
        /// Obtém ou define dhInicioFechamentoFiscal.
        /// </summary>
        public DateTime? dhInicioFechamentoFiscal { get; set; }

        /// <summary>
        /// Retorna o ultimo dia e horario do mês e do ano fiscal.
        /// </summary>
        /// <returns>o ultimo dia e hora do mês e do ano fiscal.</returns>
        public DateTime? ObterMesAberto()
        {
            return nrMes.HasValue && nrAno.HasValue 
                ? new DateTime(nrAno.Value, nrMes.Value, 1).ToLastMonthTime()
                : (DateTime?)null;                
        }
    }
}
