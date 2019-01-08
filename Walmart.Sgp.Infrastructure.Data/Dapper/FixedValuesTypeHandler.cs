using System;
using System.Data;
using System.Linq;
using System.Reflection;
using Dapper;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de ITypeHandler para FixedValues.
    /// </summary>
    public class FixedValuesTypeHandler : SqlMapper.ITypeHandler
    {
        #region Fields
        private MethodInfo m_fixedValuesConversionOperator;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="FixedValuesTypeHandler"/>.
        /// </summary>
        /// <param name="fixedValuesType">O tipo do FixedValues.</param>
        public FixedValuesTypeHandler(Type fixedValuesType)
        {
            m_fixedValuesConversionOperator = FixedValuesHelper.GetConversionOperator(fixedValuesType);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Realiza o parse do valor do banco para o FixedValue.
        /// </summary>
        /// <param name="destinationType">O tipo do FixedValue.</param>
        /// <param name="value">O valor oriundo do banco de dados.</param>
        /// <returns>A instância do FixedValue.</returns>
        public object Parse(Type destinationType, object value)
        {
            return m_fixedValuesConversionOperator.Invoke(null, new object[] { value });
        }

        /// <summary>
        /// Configura o valor do db parameter.
        /// </summary>
        /// <param name="parameter">O db parameter.</param>
        /// <param name="value">O valor.</param>
        public void SetValue(IDbDataParameter parameter, object value)
        {
            var fixedValue = value as IFixedValue;
            
            if (fixedValue == null || fixedValue.ValueAsObject == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = fixedValue.ValueAsObject;
            }
        }
        #endregion
    }
}
