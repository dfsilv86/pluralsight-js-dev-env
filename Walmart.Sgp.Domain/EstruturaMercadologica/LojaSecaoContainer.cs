using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Classe base para containers de seção de loja.
    /// </summary>
    public abstract class LojaSecaoContainer : EntityBase
    {
        /// <summary>
        /// Obtém ou define o departamento.
        /// </summary>
        public Departamento Departamento { get; set; }

        /// <summary>
        /// Obtém ou define a categoria.
        /// </summary>
        public Categoria Categoria { get; set; }  

        /// <summary>
        /// Obtém a qual seção (Categoria ou Departamento) está associada.
        /// </summary>
        public ILojaSecao Secao
        {
            get
            {
                return Departamento as ILojaSecao ?? Categoria as ILojaSecao;
            }
        }
    }
}
