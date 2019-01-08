using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Inventarios
{
    /// <summary>
    /// Representa os possíveis valores para os tipos de filtro para itens de inventário.
    /// </summary>
    public class TipoFiltroItemInventario : FixedValuesBase<int>
    {
        #region Fields

        /// <summary>
        /// Os tipos de filtro para itens de inventário de todos (0).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoFiltroItemInventario Nenhum = new TipoFiltroItemInventario(0);

        /// <summary>
        /// Os tipos de filtro para itens de inventário de itenssemcusto (1).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoFiltroItemInventario ItensSemCusto = new TipoFiltroItemInventario(1);

        /// <summary>
        /// Os tipos de filtro para itens de inventário de itenscustocadastro (2).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoFiltroItemInventario ItensCustoCadastro = new TipoFiltroItemInventario(2);

        /// <summary>
        /// Os tipos de filtro para itens de inventário de modificadosaposimportacao (3).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoFiltroItemInventario ModificadosAposImportacao = new TipoFiltroItemInventario(3);

        /// <summary>
        /// Os tipos de filtro para itens de inventário de inativosdeletados (4).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoFiltroItemInventario InativosDeletados = new TipoFiltroItemInventario(4);

        /// <summary>
        /// Os tipos de filtro para itens de inventário de vinculadosentrada (5).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoFiltroItemInventario VinculadosEntrada = new TipoFiltroItemInventario(5);

        /// <summary>
        /// Os tipos de filtro para itens de inventário de sortimentoinvalido (6).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TipoFiltroItemInventario SortimentoInvalido = new TipoFiltroItemInventario(6);

        /// <summary>
        /// Todos os os tipos de filtro para itens de inventário.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly")]
        public static readonly TipoFiltroItemInventario[] Todos = new TipoFiltroItemInventario[] { TipoFiltroItemInventario.Nenhum, TipoFiltroItemInventario.ItensSemCusto, TipoFiltroItemInventario.ItensCustoCadastro, TipoFiltroItemInventario.ModificadosAposImportacao, TipoFiltroItemInventario.InativosDeletados, TipoFiltroItemInventario.VinculadosEntrada, TipoFiltroItemInventario.SortimentoInvalido, };
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="TipoFiltroItemInventario"/>.
        /// </summary>
        /// <param name="value">O valor.</param>
        private TipoFiltroItemInventario(int value)
            : base(value)
        {
        }
        #endregion

        #region Operators
        /// <summary>
        /// Performs an implicit conversion from <see cref="int"/> to <see cref="TipoFiltroItemInventario"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static implicit operator TipoFiltroItemInventario(int value)
        {
            try
            {
                return TipoFiltroItemInventario.Todos.Single(t => t.Value == value);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(
                    Texts.InvalidFixedValue.With(value, typeof(TipoFiltroItemInventario).Name, Todos.Select(i => i.Value).JoinWords()),
                    ex);
            }
        }
        #endregion
    }
}