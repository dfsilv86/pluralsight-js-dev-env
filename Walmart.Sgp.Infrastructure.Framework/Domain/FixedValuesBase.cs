using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Infrastructure.Framework.Domain
{
    /// <summary>
    /// Classe base para implementações de valores fixo.
    /// </summary>
    /// <remarks>
    /// Normalmente eram enumerações no antigo sistema, mas por razão de compatibilidade com o Dapper, foi criada essa classe base.
    /// </remarks>
    /// <typeparam name="TUnderlyingValue">O tipo do valor trabalhado, normalmente string ou int.</typeparam>
    [DebuggerDisplay("{Description} ({Value})")]
    public abstract class FixedValuesBase<TUnderlyingValue> : IFixedValue, IConvertible, IEquatable<FixedValuesBase<TUnderlyingValue>>
    {
        #region Fields
        private readonly Type m_thisType;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="FixedValuesBase{TUnderlyingValue}"/>.
        /// </summary>
        /// <param name="value">O valor.</param>
        protected FixedValuesBase(TUnderlyingValue value)
        {
            Value = value;
            Description = GlobalizationHelper.GetText("{0}FixedValue{1}".With(GetType().Name, value));

            m_thisType = this.GetType();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém o valor.
        /// </summary>
        public TUnderlyingValue Value { get; private set; }

        /// <summary>
        /// Obtém ou define a descrição.
        /// </summary>
        public string Description { get; protected set; }

        /// <summary>
        /// Obtém o valor.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        object IFixedValue.ValueAsObject
        {
            get
            {
                return Value;
            }
        }   
        #endregion

        #region Methods
        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Value == null ? null : Value.ToString();
        }

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            var other = obj as FixedValuesBase<TUnderlyingValue>;

            return
                (other != null && GetType() == other.GetType()) // O outro objto comparado nãod eve ser nulo e deve ser do mesmo tipo.
                && ((Value == null && other.Value == null) // Se ambos os valores forem nulos
                || (Value != null && Value.Equals(other.Value))); // Ou se o valores forem iguais.
        }

        /// <summary>
        /// Determines whether the specified <see cref="FixedValuesBase{TUnderlyingValue}" />, is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="FixedValuesBase{TUnderlyingValue}" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="FixedValuesBase{TUnderlyingValue}" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(FixedValuesBase<TUnderlyingValue> other)
        {
            return Equals((object)other);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <summary>
        /// Gets the type code.
        /// </summary>
        /// <returns>O tipo do código.</returns>
        public TypeCode GetTypeCode()
        {
            var underlyingType = typeof(TUnderlyingValue);

            if (underlyingType == typeof(int))
            {
                return TypeCode.Int32;
            }

            return TypeCode.String;
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public string ToString(IFormatProvider provider)
        {
            return Value == null ? null : Convert.ToString(Value, provider);
        }

        /// <summary>
        /// Returns a <see cref="int" /> that represents this instance.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <returns>
        /// A <see cref="int" /> that represents this instance.
        /// </returns>
        public int ToInt32(IFormatProvider provider)
        {
            return Convert.ToInt32(Value, provider);
        }

        /// <summary>
        /// Returns a <see cref="short" /> that represents this instance.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <returns>
        /// A <see cref="short" /> that represents this instance.
        /// </returns>
        public short ToInt16(IFormatProvider provider)
        {
            return Convert.ToInt16(Value, provider);
        }

        /// <summary>
        /// Converts the value of this instance to an <see cref="object"/> of the specified <see cref="System.Type"/> that has an equivalent value, using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="conversionType">The <see cref="System.Type"/> to which the value of this instance is converted.</param>
        /// <param name="provider">An <see cref="System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>An <see cref="object"/> instance of type conversionType whose value is equivalent to the value of this instance.</returns>
        /// <remarks>Utilizado pela serialização para JSON.</remarks>
        public object ToType(Type conversionType, IFormatProvider provider)
        {
            if (conversionType == m_thisType)
            {
                return this;
            }

            // Utilizado pela serialização para JSON.
            return this.ToString(provider);
        }

        #endregion

        #region Not implemented
        /// <summary>
        /// Não implementado.
        /// </summary>
        /// <param name="provider">O provider.</param>
        /// <returns>Não foi implementado.</returns>
        public bool ToBoolean(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Não implementado.
        /// </summary>
        /// <param name="provider">O provider.</param>
        /// <returns>Não foi implementado.</returns>
        public char ToChar(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Não implementado.
        /// </summary>
        /// <param name="provider">O provider.</param>
        /// <returns>Não foi implementado.</returns>
        public sbyte ToSByte(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Não implementado.
        /// </summary>
        /// <param name="provider">O provider.</param>
        /// <returns>Não foi implementado.</returns>
        public byte ToByte(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Não implementado.
        /// </summary>
        /// <param name="provider">O provider.</param>
        /// <returns>Não foi implementado.</returns>
        public ushort ToUInt16(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Não implementado.
        /// </summary>
        /// <param name="provider">O provider.</param>
        /// <returns>Não foi implementado.</returns>
        public uint ToUInt32(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Não implementado.
        /// </summary>
        /// <param name="provider">O provider.</param>
        /// <returns>Não foi implementado.</returns>
        public long ToInt64(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Não implementado.
        /// </summary>
        /// <param name="provider">O provider.</param>
        /// <returns>Não foi implementado.</returns>
        public ulong ToUInt64(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Não implementado.
        /// </summary>
        /// <param name="provider">O provider.</param>
        /// <returns>Não foi implementado.</returns>
        public float ToSingle(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Não implementado.
        /// </summary>
        /// <param name="provider">O provider.</param>
        /// <returns>Não foi implementado.</returns>
        public double ToDouble(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Não implementado.
        /// </summary>
        /// <param name="provider">O provider.</param>
        /// <returns>Não foi implementado.</returns>
        public decimal ToDecimal(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Não implementado.
        /// </summary>
        /// <param name="provider">O provider.</param>
        /// <returns>Não foi implementado.</returns>
        public DateTime ToDateTime(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        #endregion                   
    }
}
