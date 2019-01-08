using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Item
{
    /// <summary>
    /// Representa os possíveis valores de tipos de reabastecimento de um item.
    /// </summary>
    public class ValorTipoReabastecimento : FixedValuesBase<short?>
    {
        #region Fields

        /// <summary>
        /// Valor tipo reabastecimento Cross Docking (3).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly ValorTipoReabastecimento CrossDocking3 = new ValorTipoReabastecimento(3);

        /// <summary>
        /// Valor tipo reabastecimento Cross Docking (33).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly ValorTipoReabastecimento CrossDocking33 = new ValorTipoReabastecimento(33);

        /// <summary>
        /// Valor tipo reabastecimento Cross Docking (94).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly ValorTipoReabastecimento CrossDocking94 = new ValorTipoReabastecimento(94);

        /// <summary>
        /// Valor tipo reabastecimento DSD (7).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly ValorTipoReabastecimento Dsd7 = new ValorTipoReabastecimento(7);

        /// <summary>
        /// Valor tipo reabastecimento DSD (37).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly ValorTipoReabastecimento Dsd37 = new ValorTipoReabastecimento(37);

        /// <summary>
        /// Valor tipo reabastecimento DSD (97).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly ValorTipoReabastecimento Dsd97 = new ValorTipoReabastecimento(97);

        /// <summary>
        /// Valor tipo reabastecimento Staple Stock (20).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly ValorTipoReabastecimento StapleStock20 = new ValorTipoReabastecimento(20);

        /// <summary>
        /// Valor tipo reabastecimento Staple Stock (22).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly ValorTipoReabastecimento StapleStock22 = new ValorTipoReabastecimento(22);

        /// <summary>
        /// Valor tipo reabastecimento Staple Stock (40).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly ValorTipoReabastecimento StapleStock40 = new ValorTipoReabastecimento(40);

        /// <summary>
        /// Valor tipo reabastecimento Staple Stock (42).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly ValorTipoReabastecimento StapleStock42 = new ValorTipoReabastecimento(42);

        /// <summary>
        /// Valor tipo reabastecimento Staple Stock (43).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly ValorTipoReabastecimento StapleStock43 = new ValorTipoReabastecimento(43);

        /// <summary>
        /// Valor tipo reabastecimento Staple Stock (81).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly ValorTipoReabastecimento StapleStock81 = new ValorTipoReabastecimento(81);

        /// <summary>
        /// Valor tipo reabastecimento nenhum
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly ValorTipoReabastecimento Nenhum = new ValorTipoReabastecimento(null);

        /// <summary>
        /// Todos os tipos de reabastecimento.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly")]
        public static readonly ValorTipoReabastecimento[] Todos = new[]
        {
            ValorTipoReabastecimento.CrossDocking3, 
            ValorTipoReabastecimento.CrossDocking33, 
            ValorTipoReabastecimento.CrossDocking94,
            ValorTipoReabastecimento.Dsd7, 
            ValorTipoReabastecimento.Dsd37, 
            ValorTipoReabastecimento.Dsd97,
            ValorTipoReabastecimento.StapleStock20,
            ValorTipoReabastecimento.StapleStock22,
            ValorTipoReabastecimento.StapleStock40,
            ValorTipoReabastecimento.StapleStock42,
            ValorTipoReabastecimento.StapleStock43,
            ValorTipoReabastecimento.StapleStock81,
            ValorTipoReabastecimento.Nenhum
        };

        /// <summary>
        /// Todos os tipos XDock.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly")]
        public static readonly ValorTipoReabastecimento[] TodosXDock = new[] 
        { 
            ValorTipoReabastecimento.CrossDocking3, 
            ValorTipoReabastecimento.CrossDocking33, 
            ValorTipoReabastecimento.CrossDocking94
        };

        /// <summary>
        /// Todos os tipos DSD.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly")]
        public static readonly ValorTipoReabastecimento[] TodosDSD = new[] 
        { 
            ValorTipoReabastecimento.Dsd7, 
            ValorTipoReabastecimento.Dsd37, 
            ValorTipoReabastecimento.Dsd97
        };

        /// <summary>
        /// Todos os tipos Staple.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly")]
        public static readonly ValorTipoReabastecimento[] TodosStaple = new[] 
        { 
            ValorTipoReabastecimento.StapleStock20,
            ValorTipoReabastecimento.StapleStock22,
            ValorTipoReabastecimento.StapleStock40,
            ValorTipoReabastecimento.StapleStock42,
            ValorTipoReabastecimento.StapleStock43,
            ValorTipoReabastecimento.StapleStock81
        };

        #endregion

        #region Constructors

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ValorTipoReabastecimento"/>.
        /// </summary>
        /// <param name="value">O valor.</param>
        private ValorTipoReabastecimento(short? value)
            : base(value)
        {
        }

        #endregion

        #region Operators
        /// <summary>
        /// Performs an implicit conversion from <see cref="short"/> to <see cref="ValorTipoReabastecimento"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static implicit operator ValorTipoReabastecimento(short? value)
        {
            try
            {
                if (!value.HasValue || value.Value == 0)
                {
                    return ValorTipoReabastecimento.Nenhum;
                }

                return ValorTipoReabastecimento.Todos.Single(t => t.Value == value);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(
                    Texts.InvalidFixedValue.With(value, typeof(ValorTipoReabastecimento).Name, Todos.Where(i => i.Value.HasValue).Select(i => i.Value.Value.ToString(CultureInfo.InvariantCulture)).JoinWords()),
                    ex);
            }
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="short"/> to <see cref="ValorTipoReabastecimento"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static implicit operator ValorTipoReabastecimento(short value)
        {
            return (ValorTipoReabastecimento)(new short?(value));
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ValorTipoReabastecimento"/> to <see cref="short"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static explicit operator short?(ValorTipoReabastecimento value)
        {
            return value == null ? new short?() : value.Value;
        }

        /// <summary>
        /// Converte para string o ValorTipoReabastecimento (Formato: Código - Descrição).
        /// </summary>
        /// <returns>Retorna representação em string.</returns>
        public override string ToString()
        {
            return Texts.ResourceManager.GetString(
                typeof(ValorTipoReabastecimento).Name + "FixedValue" + (this == ValorTipoReabastecimento.Nenhum ? string.Empty : this.Value.ToString()));
        }

        #endregion
    }
}
