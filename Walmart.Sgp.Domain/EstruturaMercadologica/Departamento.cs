using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Departamento da estrutura mercadológica.
    /// </summary>
    [DebuggerDisplay("{cdDepartamento} - {dsDepartamento}")]
    public class Departamento : EntityBase, IAggregateRoot, IStampContainer, ILojaSecao
    {
        /// <summary>
        /// Obtém ou define the id.
        /// </summary>
        public override int Id
        {
            get
            {
                return IDDepartamento;
            }

            set
            {
                IDDepartamento = value;
            }
        }

        /// <summary>
        /// Obtém ou define IDDepartamento.
        /// </summary>
        public int IDDepartamento { get; set; }

        /// <summary>
        /// Obtém ou define IDDivisao.
        /// </summary>
        public int IDDivisao { get; set; }

        /// <summary>
        /// Obtém ou define cdSistema.
        /// </summary>
        public int cdSistema { get; set; }

        /// <summary>
        /// Obtém ou define cdDepartamento.
        /// </summary>
        public int cdDepartamento { get; set; }

        /// <summary>
        /// Obtém o código da categoria.
        /// </summary>
        /// <remarks>     
        /// "Here be dragons"
        /// Existe um relacionamento entre Departamento.cdDepartamento = Categoria.cdCategoria 
        /// quando TipoSistema.SamsClub, pois o Sam's Club não possui departamento.
        /// </remarks>
        public int cdCategoria
        {
            get
            {
                // TODO: essa estrutura e relacionamento de departamento/categoria para o Sam's Club precisa ser revista e remodelada.
                return cdSistema == TipoSistema.SamsClub.Value ? cdDepartamento : 0;
            }
        }

        /// <summary>
        /// Obtém ou define dsDepartamento.
        /// </summary>
        public string dsDepartamento { get; set; }

        /// <summary>
        /// Obtém ou define blPerecivel.
        /// </summary>
        /// <remarks>S ou ..?</remarks>
        public string blPerecivel { get; set; }

        /// <summary>
        /// Obtém ou define blAtivo.
        /// </summary>
        public bool blAtivo { get; set; }

        /// <summary>
        /// Obtém ou define dhCriacao.
        /// </summary>
        /// <remarks>
        /// No banco está (datetime, null) porém não existe registros nulos
        /// e os novos sempre serão inseridos com data de criação. Por isso
        /// foi removido o nullable desta propriedade e também para que
        /// satisfaça a interface <see cref="IStampContainer"/>.
        /// </remarks>
        public DateTime DhCriacao { get; set; }

        /// <summary>
        /// Obtém ou define dhAtualizacao.
        /// </summary>
        public DateTime? DhAtualizacao { get; set; }

        /// <summary>
        /// Obtém ou define cdUsuarioCriacao.
        /// </summary>
        public int? CdUsuarioCriacao { get; set; }

        /// <summary>
        /// Obtém ou define cdUsuarioAtualizacao.
        /// </summary>
        public int? CdUsuarioAtualizacao { get; set; }

        /// <summary>
        /// Obtém ou define pcDivergenciaNF.
        /// </summary>
        public decimal? pcDivergenciaNF { get; set; }

        /// <summary>
        /// Obtém ou define a divisão.
        /// </summary>
        /// <value>
        /// A divisão.
        /// </value>
        public Divisao Divisao { get; set; }

        /// <summary>
        /// Obtém o código.
        /// </summary>
        public int Codigo
        {
            get { return cdDepartamento; }
        }

        /// <summary>
        /// Obtém a descrição.
        /// </summary>
        public string Descricao
        {
            get { return dsDepartamento; }
        }
    }
}
