using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Representa uma ReturnSheet.
    /// </summary>
    public class ReturnSheet : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// Obtém ou define Id.
        /// </summary>
        public override int Id
        {
            get
            {
                return this.IdReturnSheet;
            }

            set
            {
                this.IdReturnSheet = value;
            }
        }

        /// <summary>
        /// Obtém ou define Departamento.
        /// </summary>
        public Walmart.Sgp.Domain.EstruturaMercadologica.Departamento Departamento { get; set; }

        /// <summary>
        /// Obtém ou define IdReturnSheet.
        /// </summary>
        public int IdReturnSheet { get; set; }

        /// <summary>
        /// Obtém ou define DhInicioReturn.
        /// </summary>
        public DateTime DhInicioReturn { get; set; }

        /// <summary>
        /// Obtém ou define DhFinalReturn.
        /// </summary>
        public DateTime DhFinalReturn { get; set; }

        /// <summary>
        /// Obtém ou define DhInicioEvento.
        /// </summary>
        public DateTime DhInicioEvento { get; set; }

        /// <summary>
        /// Obtém ou define HoraCorte.
        /// </summary>
        public DateTime HoraCorte { get; set; }

        /// <summary>
        /// Obtém ou define DhFinalEvento.
        /// </summary>
        public DateTime DhFinalEvento { get; set; }

        /// <summary>
        /// Obtém ou define IdRegiaoCompra.
        /// </summary>
        public int IdRegiaoCompra { get; set; }

        /// <summary>
        /// Obtém ou define idDepartamento.
        /// </summary>
        public int idDepartamento { get; set; }

        /// <summary>
        /// Obtém ou define Descricao.
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Obtém ou define IdUsuarioCriacao.
        /// </summary>
        public int IdUsuarioCriacao { get; set; }

        /// <summary>
        /// Obtém ou define DhAtualizacao.
        /// </summary>
        public DateTime DhAtualizacao { get; set; }

        /// <summary>
        /// Obtém ou define DhCriacao.
        /// </summary>
        public DateTime DhCriacao { get; set; }

        /// <summary>
        /// Obtém ou define BlAtivo.
        /// </summary>
        public bool BlAtivo { get; set; }

        /// <summary>
        /// Obtém ou define ReturnSheetItemPrincipal.
        /// </summary>
        public ReturnSheetItemPrincipal ItemPrincipal { get; set; }

        /// <summary>
        /// Obtém ou define Usuario.
        /// </summary>
        public Walmart.Sgp.Domain.Acessos.Usuario Usuario { get; set; }

        /// <summary>
        /// Obtém ou define RegiaoCompra.
        /// </summary>
        public EstruturaMercadologica.RegiaoCompra RegiaoCompra { get; set; }

        /// <summary>
        /// Obtém ou define SugestaoReturnSheet
        /// </summary>
        public SugestaoReturnSheet Sugestao { get; set; }
        
        /// <summary>
        /// Obtém ou define Exportada
        /// </summary>
        public bool Exportada { get; set; }

        /// <summary>
        /// Obtém Periodo
        /// </summary>
        public string Periodo
        {
            get
            {
                return DhInicioReturn.ToString("dd/MM/yyyy", RuntimeContext.Current.Culture)
                    + " - "
                    + DhFinalReturn.ToString("dd/MM/yyyy HH:mm:ss", RuntimeContext.Current.Culture);
            }
        }
    }
}
