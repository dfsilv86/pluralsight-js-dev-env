using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.UnitTests;
using Walmart.Sgp.Infrastructure.Data.Dapper;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Data.FunctionalTests.Dapper;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Walmart.Sgp.Infrastructure.Data.FunctionalTests
{
    [TestFixture]
    [Category("Data")]
    public class FixedValuesTest
    {        
        [Test]
        public void FixedValues_Fields_SameAsDatabaseValues()
        {
            var convertToColumnNameRegex = new Regex("^Tipo", RegexOptions.Compiled);

            this.RunTransaction((appDb) =>
            {
                var proxy = new DapperProxy(appDb.Wlmslp, CommandType.Text);
                var propertiesUsingFixedValue = typeof(Usuario).Assembly.GetTypes()
                    .Where(t => !t.IsAbstract && typeof(IEntity).IsAssignableFrom(t))
                    .SelectMany(t => t.GetProperties().Where(p => IsFixedValue(p)));
                var propertiesVerified = 0;
                var propertiesNotVerified = 0;

                foreach (var p in propertiesUsingFixedValue)
                {
                    try
                    {
                        var columnName = convertToColumnNameRegex.Replace(p.Name, "tp");
                        var result = proxy.Query<dynamic>("SELECT {0} as Value FROM {1} group by {0}".With(columnName, p.ReflectedType.Name), null);
                        var conversionOperator = FixedValuesHelper.GetConversionOperator(p.PropertyType);

                        foreach(var row in result)
                        {
                            conversionOperator.Invoke(null, new object[] { row.Value });
                        }

                        propertiesVerified++;
                    }                    
                    catch (SqlException)
                    {
                        propertiesNotVerified++;

                        // Não foi possível inferir corretamente nome de coluna e tabela.
                        continue;
                    }
                    catch (Exception ex)
                    {
                        Assert.Fail("Existe um valor no banco de dados que não está mapeado no FixedValue: {0}", ex.InnerException.Message);
                    }
                }

                Assert.AreNotEqual(0, propertiesVerified);
                Assert.AreNotEqual(0, propertiesNotVerified);
            });
        }

        private bool IsFixedValue(PropertyInfo p)
        {
            return typeof(FixedValuesBase<string>).IsAssignableFrom(p.PropertyType)
                || typeof(FixedValuesBase<char>).IsAssignableFrom(p.PropertyType)
                || typeof(FixedValuesBase<char?>).IsAssignableFrom(p.PropertyType)
                || typeof(FixedValuesBase<int>).IsAssignableFrom(p.PropertyType)
                || typeof(FixedValuesBase<int?>).IsAssignableFrom(p.PropertyType);
        }
    }
}
