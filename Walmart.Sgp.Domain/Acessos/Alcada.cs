using System.Collections.Generic;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Acessos
{
    /// <summary>
    /// Representa uma Alcada.
    /// </summary>
    public class Alcada : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="Alcada"/>
        /// </summary>
        public Alcada()
        {
            Detalhe = new List<AlcadaDetalhe>();
        }

        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public override int Id
        {
            get
            {
                return this.IDAlcada;
            }

            set
            {
                this.IDAlcada = value;
            }
        }

        /// <summary>
        /// Obtém ou define IDAlcada.
        /// </summary>
        public int IDAlcada { get; set; }

        /// <summary>
        /// Obtém ou define IDPerfil.
        /// </summary>
        public int IDPerfil { get; set; }

        /// <summary>
        /// Obtém ou define blAlterarSugestao.
        /// </summary>
        public bool blAlterarSugestao { get; set; }

        /// <summary>
        /// Obtém ou define blAlterarInformacaoEstoque.
        /// </summary>
        public bool blAlterarInformacaoEstoque { get; set; }

        /// <summary>
        /// Obtém ou define blAlterarPercentual.
        /// </summary>
        public bool blAlterarPercentual { get; set; }

        /// <summary>
        /// Obtém ou define vlPercentualAlterado.
        /// </summary>
        public decimal? vlPercentualAlterado { get; set; }

        /// <summary>
        /// Obtém ou define blZerarItem.
        /// </summary>
        public bool blZerarItem { get; set; }

        /// <summary>
        /// Obtém ou define o papel da alçada.
        /// </summary>
        public Papel Papel { get; set; }

        /// <summary>
        /// Obtém ou define os relacionamentos de alcada detalhe.
        /// </summary>
        public IList<AlcadaDetalhe> Detalhe { get; set; }

        /// <summary>
        /// Garante a integridade dos valores contidos nesta instância.
        /// </summary>
        public void GarantirIntegridade()
        {
            if (!blAlterarSugestao)
            {
                blAlterarInformacaoEstoque = blAlterarPercentual = false;
            }

            if (!blAlterarPercentual)
            {
                vlPercentualAlterado = null;
            }
        }
    }
}
