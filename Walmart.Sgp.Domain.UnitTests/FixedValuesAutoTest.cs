using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using Rhino.Mocks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Reabastecimento;

namespace Walmart.Sgp.Domain.UnitTests
{
    /// <summary>
    /// Testes utilizando reflection para dar cobertura de código em construtors padrões de implementações de FixedValuesBase.
    /// </summary>
    [TestFixture]
    [Category("Domain")]
    [Category("Auto")]
    public class FixedValuesAutoTest
    {
        private List<Type> m_fixedValuesTypes = new List<Type>();

        [SetUp]
        public void InitializeTest()
        {
            var assemblies = new Assembly[] { typeof(Usuario).Assembly };
            m_fixedValuesTypes = new List<Type>();
            var baseTypes = new Type[] { typeof(FixedValuesBase<string>), typeof(FixedValuesBase<int>), typeof(FixedValuesBase<short?>) };

            foreach (var assembly in assemblies)
            {
                m_fixedValuesTypes.AddRange(assembly.GetTypes().Where(t => baseTypes.Any(b => b.IsAssignableFrom(t))));
            }

#if !CI
           // m_fixedValuesTypes = m_fixedValuesTypes.Where(t => t == typeof(TipoSemana)).ToList();
#endif
        }
       
        [Test]
        public void Fields_CompareValues_DiffValues()
        {
            foreach (var fixedValueType in m_fixedValuesTypes)
            {
                var fields = fixedValueType.GetFields().Where(f => f.IsStatic && f.IsInitOnly).ToArray();
                var implicitOperator = FixedValuesHelper.GetConversionOperator(fixedValueType);
                var testName = "{0}_Fields_AutoTest".With(fixedValueType.Name);
                TeamCityService.ReportTestStarted(testName);

                try
                {
                    Assert.GreaterOrEqual(fields.Length, 2, "Implementações de FixedValuesBase devem ter, pelo menos, dois campos public static readonly");
                    Assert.IsTrue(fields.Any(f => f.Name.Equals("Todos")), "Implementações de FixedValuesBase devem ter um campo public static readonly chamado Todos");
                    Assert.AreEqual(fields.Length, fields.Distinct().Count(), "Todos os campos devem ter valores diferentes");
                    Assert.NotNull(implicitOperator, "Implementações de FixedValuesBase devem ter um operador de conversão implícito de string ou int.");

                    #region Testa se o operador implicito está levantando exception em caso de valor inválido.
                    object invalidValue = null;

                    Type paramType = implicitOperator.GetParameters()[0].ParameterType;
                    if (paramType == typeof(int))
                    {
                        invalidValue = int.MaxValue;
                    }
                    else if (paramType == typeof(byte))
                    {
                        invalidValue = byte.MaxValue;
                    }
                    else if (paramType == typeof(long))
                    {
                        invalidValue = long.MaxValue;
                    }
                    else if (paramType == typeof(short))
                    {
                        invalidValue = short.MaxValue;
                    }
                    else if (paramType == typeof(string))
                    {
                        invalidValue = DateTime.Now.ToString();
                    }

                    if (fixedValueType != typeof(TipoPedidoMinimo))
                    {
                        Exception invalidCastExp = null;
                        try
                        {
                            var result = implicitOperator.Invoke(null, new object[] { invalidValue });
                        }
                        catch (TargetInvocationException ex)
                        {
                            if (ex.InnerException is InvalidCastException)
                            {
                                invalidCastExp = ex.InnerException;
                            }
                        }
                        Assert.IsNotNull(invalidCastExp);
                    }
                    #endregion

                    foreach (var fieldValue in fields.Select(f => f.GetValue(null)))
                    {
                        if (fieldValue.GetType() == fixedValueType)
                        {
                            var valuePropertyValue = fixedValueType.GetProperty("Value").GetValue(fieldValue);
                            var actualValue = implicitOperator.Invoke(null, new object[] { valuePropertyValue });
                            Assert.AreEqual(fieldValue, actualValue, "Era esperado o valor '{0}' no operador, mas foi '{1}'.", fieldValue, actualValue);
                            Assert.IsFalse(fixedValueType.GetProperty("Description").GetValue(fieldValue).ToString().Contains("[TEXT NOT FOUND]"), "Faltou globalização para o valor '{0}'".With("{0}FixedValue{1}".With(fixedValueType.Name, valuePropertyValue)));
                        }
                    }

                    TeamCityService.ReportTestFinished(testName);
                }
                catch (Exception ex)
                {
                    var msg = ex.GetBaseException().Message;
                    TeamCityService.ReportTestFailed(testName, msg, ex.StackTrace);
                    Console.WriteLine(msg);

#if !CI
                    Assert.Fail("{0}: {1}".With(testName, msg));
#endif
                }                
            }
        }     
    }
}
