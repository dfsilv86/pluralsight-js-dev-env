using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Representa uma Revisao Custo
    /// </summary>
    public class RevisaoCusto : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public override int Id
        {
            get
            {
                return this.IDRevisaoCusto;
            }

            set
            {
                this.IDRevisaoCusto = value;
            }
        }

        /// <summary>
        /// Obtém ou define IDRevisaoCusto.
        /// </summary>
        public int IDRevisaoCusto { get; set; }

        /// <summary>
        /// Obtém ou define IDLoja.
        /// </summary>
        public int IDLoja { get; set; }

        /// <summary>
        /// Obtém ou define IDItemDetalhe.
        /// </summary>
        public int IDItemDetalhe { get; set; }

        /// <summary>
        /// Obtém ou define IDStatusRevisaoCusto.
        /// </summary>
        public int IDStatusRevisaoCusto { get; set; }

        /// <summary>
        /// Obtém ou define IDMotivoRevisaoCusto.
        /// </summary>
        public int IDMotivoRevisaoCusto { get; set; }

        /// <summary>
        /// Obtém ou define IDUsuarioSolicitante.
        /// </summary>
        public int IDUsuarioSolicitante { get; set; }

        /// <summary>
        /// Obtém ou define dtSolicitacao.
        /// </summary>
        public DateTime? dtSolicitacao { get; set; }

        /// <summary>
        /// Obtém ou define vlCustoSolicitado.
        /// </summary>
        public decimal? vlCustoSolicitado { get; set; }

        /// <summary>
        /// Obtém ou define dsMotivo.
        /// </summary>
        public string dsMotivo { get; set; }

        /// <summary>
        /// Obtém ou define IDUsuarioRevisor.
        /// </summary>
        public int? IDUsuarioRevisor { get; set; }

        /// <summary>
        /// Obtém ou define dtCustoRevisado.
        /// </summary>
        public DateTime? dtCustoRevisado { get; set; }

        /// <summary>
        /// Obtém ou define vlCustoRevisado.
        /// </summary>
        public decimal? vlCustoRevisado { get; set; }

        /// <summary>
        /// Obtém ou define dsRevisor.
        /// </summary>
        public string dsRevisor { get; set; }

        /// <summary>
        /// Obtém ou define dtCriacao.
        /// </summary>
        public DateTime dtCriacao { get; set; }

        /// <summary>
        /// Obtém ou define dtRevisado.
        /// </summary>
        public DateTime? dtRevisado { get; set; }

        /// <summary>
        /// Obtém ou define a loja.
        /// </summary>
        public Loja Loja { get; set; }

        /// <summary>
        /// Obtém ou define o item.
        /// </summary>
        public ItemDetalhe ItemDetalhe { get; set; }

        /// <summary>
        /// Obtém ou define o status.
        /// </summary>
        public StatusRevisaoCusto StatusRevisaoCusto { get; set; }

        /// <summary>
        /// Obtém ou define o motivo.
        /// </summary>
        public MotivoRevisaoCusto MotivoRevisaoCusto { get; set; }

        /// <summary>
        /// Obtém ou define o usuario.
        /// </summary>
        public Usuario UsuarioSolicitante { get; set; }
    }
}
