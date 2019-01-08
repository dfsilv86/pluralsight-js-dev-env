using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Infrastructure.Data.Ado
{
    /// <summary>
    /// Implementação de um table data gateway para data gateway utilizando o ADO .NET.
    /// </summary>
    /// <typeparam name="T">O tipo de objeto trabalhdo pelo data gateway.</typeparam>
    public abstract class AdoDataGatewayBase<T>
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="AdoDataGatewayBase{T}"/>.
        /// </summary>
        /// <param name="transaction">A transação.</param>
        protected AdoDataGatewayBase(SqlTransaction transaction)
        {
            Transaction = transaction;
            Connection = transaction.Connection;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém a transação.
        /// </summary>
        protected SqlTransaction Transaction { get; private set; }

        /// <summary>
        /// Obtém a conexão.
        /// </summary>
        protected SqlConnection Connection { get; private set; }
        #endregion

        #region Helpers
        /// <summary>
        /// Realiza o mapeamento do resultado do SQLCommand informado para a lista de TModel.
        /// </summary>
        /// <typeparam name="TModel">O tipo de modelo onde o resultado será mapeado.</typeparam>
        /// <param name="cmd">O comando.</param>
        /// <param name="projection">A projeção de colunas.</param>
        /// <param name="afterMap">Action chamada após o mapeamento de cada linha.</param>
        /// <returns>As linhas mapeadas.</returns>
        protected static IEnumerable<TModel> Map<TModel>(SqlCommand cmd, string projection = "*", Action<TModel, SqlDataReader> afterMap = null)
        {
            var result = new List<TModel>();

            if (afterMap == null)
            {
                afterMap = (m, r) => { };
            }

            using (var dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    var model = Map<TModel>(dr, projection);

                    afterMap(model, dr);
                    result.Add(model);
                }

                dr.Close();
            }

            return result;
        }

        /// <summary>
        /// Realiza o mapeamento da linha corrente do SqlDataReader nformado para um TModel.
        /// </summary>
        /// <typeparam name="TModel">O tipo de modelo onde o resultado será mapeado.</typeparam>
        /// <param name="dr">O SqlDataReader.</param>
        /// <param name="projection">A projeção de colunas.</param>
        /// <returns>O objeto mapeado com os valores da linha atual.</returns>
        protected static TModel Map<TModel>(SqlDataReader dr, string projection = "*")
        {
            var properties = GetPropertiesInProjection<TModel>(projection);

            var model = Activator.CreateInstance<TModel>();

            for (int i = 0; i < dr.FieldCount; i++)
            {
                var columnName = dr.GetName(i);
                var property = properties.FirstOrDefault(p => p.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase));

                if (property != null)
                {
                    SetPropertyValue(model, property, dr.GetValue(i));
                }
            }

            return model;
        }       

        /// <summary>
        /// Cria parâmetros no SqlCommand informado utilizando os valores das propriedades de obj.
        /// </summary>
        /// <param name="cmd">O SqlCommand.</param>
        /// <param name="parametersSource">O objeto de onde serão tirados os nomes e valores dos parâmetros.</param>
        protected static void CreateParameters(SqlCommand cmd, object parametersSource)
        {
            var objType = parametersSource.GetType();
            var properties = GetProperties(objType);

            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(parametersSource);

                if (propertyValue == null)
                {
                    cmd.Parameters.Add(new SqlParameter("@{0}".With(property.Name), DBNull.Value));
                }
                else
                {
                    cmd.Parameters.Add(new SqlParameter("@{0}".With(property.Name), propertyValue));
                }
            }
        }

        /// <summary>
        /// Cria um SqlCommand.
        /// </summary>
        /// <returns>O comando.</returns>
        protected SqlCommand CreateCommand()
        {
            var cmd = Connection.CreateCommand();
            cmd.CommandTimeout = 300;
            cmd.Transaction = Transaction;

            return cmd;
        }

        private static IEnumerable<PropertyInfo> GetProperties(Type objType)
        {
            return objType
                .GetProperties(BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        }

        private static IEnumerable<PropertyInfo> GetPropertiesInProjection<TModel>(string projection)
        {
            var projectionColumns = projection.ToUpperInvariant().Split(new string[] { " , ", ", ", " ,", "," }, StringSplitOptions.RemoveEmptyEntries);
            var properties = GetProperties(typeof(TModel));

            if (!projection.Equals("*", StringComparison.OrdinalIgnoreCase))
            {
                properties = properties.Where(p => projectionColumns.Contains(p.Name.ToUpperInvariant())).ToArray();
            }

            return properties;
        }

        private static void SetPropertyValue<TModel>(TModel model, PropertyInfo property, object value)
        {
            if (value == DBNull.Value)
            {
                property.SetValue(model, null);
            }
            else
            {
                if (value.GetType() == typeof(Int64))
                {
                    property.SetValue(model, Convert.ToInt32(value, CultureInfo.InvariantCulture));
                }
                else
                {
                    property.SetValue(model, value);
                }
            }
        }
        #endregion
    }
}
