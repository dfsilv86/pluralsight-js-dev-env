using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Representa uma AutorizaPedido.
    /// </summary>
    public class AutorizaPedido : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public override int Id
        {
            get
            {
                return this.IdAutorizaPedido;
            }

            set
            {
                this.IdAutorizaPedido = value;
            }
        }

        /// <summary>
        /// Obtém ou define IdAutorizaPedido.
        /// </summary>
        public int IdAutorizaPedido { get; set; }

        /// <summary>
        /// Obtém ou define IdLoja.
        /// </summary>
        public int IdLoja { get; set; }

        /// <summary>
        /// Obtém ou define a loja.
        /// </summary>
        public Loja Loja { get; set; }

        /// <summary>
        /// Obtém ou define IdDepartamento.
        /// </summary>
        public int IdDepartamento { get; set; }

        /// <summary>
        /// Obtém ou define o departamento.
        /// </summary>
        public Departamento Departamento { get; set; }

        /// <summary>
        /// Obtém ou define dtPedido.
        /// </summary>
        public DateTime dtPedido { get; set; }

        /// <summary>
        /// Obtém ou define dtAutorizacao.
        /// </summary>
        public DateTime dtAutorizacao { get; set; }

        /// <summary>
        /// Obtém ou define IdUser.
        /// </summary>
        public int IdUser { get; set; }

        /// <summary>
        /// Obtém ou define o usuário.
        /// </summary>
        public Usuario User { get; set; }
    }
}
