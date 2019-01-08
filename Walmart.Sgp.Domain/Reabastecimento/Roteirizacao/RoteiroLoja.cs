using System;
using Walmart.Sgp.Domain.Specs;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Representa uma RoteiroLoja.
    /// </summary>
    public class RoteiroLoja : EntityBase, IAggregateRoot, IRegistroSelecionavel
    {
        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public override int Id
        {
            get
            {
                return IDRoteiroLoja;
            }

            set
            {
                IDRoteiroLoja = value;
            }
        }

        /// <summary>
        /// Obtém ou define idRoteiroLoja.
        /// </summary>
        public int IDRoteiroLoja { get; set; }

        /// <summary>
        /// Obtém ou define idRoteiro.
        /// </summary>
        public int idRoteiro { get; set; }

        /// <summary>
        /// Obtém ou define idloja.
        /// </summary>
        public int idloja { get; set; }

        /// <summary>
        /// Obtém ou define blativo.
        /// </summary>
        public bool? blativo { get; set; }

        #region Propriedades Calculadas
        /// <summary>
        /// Obtém ou define dsEstado.
        /// </summary>
        public string dsEstado { get; set; }

        /// <summary>
        /// Obtém ou define nmLoja.
        /// </summary>
        public string nmLoja { get; set; }

        /// <summary>
        /// Obtém ou define cdLoja.
        /// </summary>
        public int cdLoja { get; set; }

        /// <summary>
        /// Obtém ou define blDomingo.
        /// </summary>
        public bool blDomingo { get; set; }

        /// <summary>
        /// Obtém ou define blSegunda.
        /// </summary>
        public bool blSegunda { get; set; }

        /// <summary>
        /// Obtém ou define blTerca.
        /// </summary>
        public bool blTerca { get; set; }

        /// <summary>
        /// Obtém ou define blQuarta.
        /// </summary>
        public bool blQuarta { get; set; }

        /// <summary>
        /// Obtém ou define blQuinta.
        /// </summary>
        public bool blQuinta { get; set; }

        /// <summary>
        /// Obtém ou define blSexta.
        /// </summary>
        public bool blSexta { get; set; }

        /// <summary>
        /// Obtém ou define blSabado.
        /// </summary>
        public bool blSabado { get; set; }
        #endregion

        /// <summary>
        /// Obtém Selecionado.
        /// </summary>
        public bool Selecionado
        {
            get
            {
                return blativo ?? false;
            }
        }
    }
}
