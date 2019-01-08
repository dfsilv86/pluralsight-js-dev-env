using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using Rhino.Mocks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Infrastructure.Framework.Commons;
using System.Reflection.Emit;
using Walmart.Sgp.Domain.Gerenciamento;

namespace Walmart.Sgp.Domain.UnitTests
{
    /// <summary>
    /// Testes utilizando reflection para dar cobertura de código em construtors padrões de serviços de domínio.
    /// </summary>
    [TestFixture]
    [Category("Domain")]
    [Category("Auto")]
    public class DomainServiceAutoTest
    {
        private List<Type> m_domainServiceTypes = new List<Type>();

        [SetUp]
        public void InitializeTest()
        {
            var assemblies = new Assembly[] { typeof(Usuario).Assembly };
            m_domainServiceTypes = new List<Type>();

            foreach (var assembly in assemblies)
            {
                m_domainServiceTypes.AddRange(assembly.GetTypes().Where(t => !t.IsInterface && t.Name.EndsWith("Service")));
            }
        }

        /// <summary>
        /// Realiza teste nos constructores.
        /// </summary>
        [Test]
        public void Constructors_DataGateway_Instance()
        {
            foreach (var serviceType in m_domainServiceTypes)
            {
                var constructor = serviceType.GetConstructors().FirstOrDefault(c => c.GetParameters().Count() == 1);

                if (constructor != null)
                {
                    var testName = "{0}_Constructor_AutoTest".With(serviceType.Name);
                    TeamCityService.ReportTestStarted(testName);

                    var instance = constructor.Invoke(new object[] { null });
                    Assert.IsNotNull(instance);

                    TeamCityService.ReportTestFinished(testName);
                }
            }
        }

        /// <summary>
        /// Realiza teste em métodos de serviço que apenas fazem um "by pass" para o método do table data gateway.
        /// </summary>
        [Test]
        public void Methods_NameEqualDataGatewayMethodName_Call()
        {
            RunForAllServiceMethods(
            "NameEqualDataGatewayMethodName",
            (ctx) => 
            {
                var gatewayMethods = ctx.ConstructorParameters.First().ParameterType.GetMethods();

                // Se o table data gateway tiver o mesmo nome de método e exatamente os mesmos paramêtros, então é apenas um bypass de domain service
                // para table data gateway e pode ser testado automaticamente.
                var gatewayMethod = gatewayMethods.FirstOrDefault(f => f.Name.Equals(ctx.Method.Name) && f.GetParameters().Select(p => p.ParameterType).SequenceEqual(ctx.MethodParameters.Select(p => p.ParameterType)));

                return gatewayMethod != null;
            },
            (ctx) => 
            {               
                // Apenas chama o método, não deve dar exception.
                ctx.Method.Invoke(ctx.ServiceInstance, ctx.MethodParametersValues);                
            });
        }

        /// <summary>
        /// Realiza teste em métodos que estão apenas lançando NotImplemetedException.
        /// <remarks>
        /// Esses métodos serão implementados e esse teste evita que a IC quebre por cobertura por NotImplemetedException.
        /// </remarks>
        /// </summary>
        [Test]
        public void Methods_NotImplementedException_Throw()
        {
            RunForAllServiceMethods(
            "NotImplementedException",
            (ctx) => 
            {
                if (ctx.Method.GetMethodBody() != null)
                {
                    var instructions = Mono.Reflection.Disassembler.GetInstructions(ctx.Method);

                    return instructions.Count == 3 && instructions[1].OpCode.Name.Equals("newobj") && instructions[2].OpCode.Name.Equals("throw");
                }

                return false;
            },
            (ctx) => 
            {
                Assert.Catch<NotImplementedException>(
                () =>
                {
                    try
                    {
                        ctx.Method.Invoke(ctx.ServiceInstance, ctx.MethodParametersValues);       
                    }
                    catch (TargetInvocationException ex)
                    {
                        throw ex.GetBaseException();
                    }
                });
            });
        }

        private void RunForAllServiceMethods(string testType, Func<MethodCallContext, bool> canCall, Action<MethodCallContext> callMethod)
        {
            foreach (var serviceType in m_domainServiceTypes)
            {
                var constructor = serviceType.GetConstructors().FirstOrDefault();

                if (constructor == null)
                {
                    continue;
                }

                var constructorParameters = constructor.GetParameters();
                var constructorArgs = new object[constructorParameters.Length];

                for (int i = 0; i < constructorParameters.Length; i++)
                {
                    constructorArgs[i] = MockRepository.GenerateMock(constructorParameters[i].ParameterType, new Type[0]);
                }

                var instance = constructor.Invoke(constructorArgs);
                var serviceMethods = FilterIgnoredServiceMethods(serviceType.GetMethods());                

                // Use para verificar um teste específico.
#if !CI
                //serviceMethods = serviceMethods.Where(m => m.Name.Equals("Salvar") && m.DeclaringType == typeof(FornecedorParametroService)).ToArray();
#endif
                foreach (var m in serviceMethods)
                {
                    var methodParameters = m.GetParameters();

                    var callContext = new MethodCallContext {
                         ConstructorParameters = constructorParameters,
                          Method = m,
                           MethodParameters = methodParameters,
                            MethodParametersValues = null,
                             ServiceInstance = instance
                    };
                    

                    if(!canCall(callContext))
                    {
                        continue;
                    }

                    var testName = "{0}_{1}_{2}_AutoTest".With(serviceType.Name, m, testType);
                    TeamCityService.ReportTestStarted(testName);

                    try
                    {
                        callContext.MethodParametersValues = methodParameters.Select(p => GetParameterValue(p.ParameterType)).ToArray();
                        callMethod(callContext);
                        TeamCityService.ReportTestFinished(testName);
                    }
                    catch (Exception ex)
                    {
                        var baseEx = ex.GetBaseException();

                        if (baseEx is NotImplementedException)
                        {
                            TeamCityService.ReportTestFinished(testName);
                        }
                        else
                        {
                            var msg = baseEx.Message;
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

        /// <summary>
        /// Adicione aqui os métodos que devem ser ignorados.
        /// </summary>
        /// <param name="methodInfos">Os métodos.</param>
        /// <returns></returns>
        private IEnumerable<MethodInfo> FilterIgnoredServiceMethods(MethodInfo[] methodInfos)
        {
            return methodInfos.Where(m => (m.DeclaringType != typeof(SugestaoPedidoService) || m.Name.Equals("PesquisarPorFiltro"))
                                       && (m.DeclaringType != typeof(ItemDetalheService) || !m.Name.Equals("PesquisarItensEntradaPorSaidaCD")));
        }

        private object GetParameterValue(Type parameterType)
        {
            var nullableType = Nullable.GetUnderlyingType(parameterType);

            if (nullableType != null)
            {
                parameterType = nullableType;
            }

            if (parameterType == typeof(Int16))
            {
                return (Int16)1;
            }
            else if (parameterType == typeof(Int32))
            {
                return 1;
            }
            else if (parameterType == typeof(Int64))
            {
                return 1L;
            }
            else if (parameterType == typeof(float))
            {
                return 1f;
            }
            else if (parameterType == typeof(string))
            {
                return "1";
            }
            else if (parameterType == typeof(char))
            {
                return '1';
            }
            else if (parameterType == typeof(DateTime))
            {
                return DateTime.Now;
            }
            else if (parameterType == typeof(decimal))
            {
                return 1M;
            }
            else if (parameterType == typeof(bool))
            {
                return false;
            }
            else if (parameterType == typeof(byte))
            {
                return (byte)1;
            }
            else if (parameterType == typeof(Paging))
            {
                return new Paging();
            }
            else if (parameterType.BaseType == typeof(FixedValuesBase<int>) || parameterType.BaseType == typeof(FixedValuesBase<string>))
            {
                return parameterType.GetFields().First(f => f.IsStatic && f.IsInitOnly).GetValue(null);
            }
            else if (parameterType == typeof(Walmart.Sgp.Domain.Item.ItemDetalhe))
            {
                return new Walmart.Sgp.Domain.Item.ItemDetalhe()
                {
                    IDItemDetalhe = -1,
                    VlFatorConversao = 1,
                    TpUnidadeMedida = TipoUnidadeMedida.Unidade
                };
            }
            else if (parameterType == typeof(Walmart.Sgp.Domain.Reabastecimento.SugestaoPedidoFiltro))
            {
                return new Walmart.Sgp.Domain.Reabastecimento.SugestaoPedidoFiltro()
                {
                    IDUsuario = 1,
                    cdLoja = 1,
                    dtPedido = DateTime.Today,
                    cdSistema = 1,
                    cdDepartamento = 1
                };
            }
            else if (parameterType == typeof(RangeValue<DateTime>))
            {
                return new RangeValue<DateTime>();
            }
            else if (parameterType.GenericTypeArguments.Length > 0 && parameterType.IsInterface)
            {
                var genericArgs = parameterType.GetGenericArguments();
                var concreteType = typeof(List<>).MakeGenericType(genericArgs);
                var value = Activator.CreateInstance(concreteType);

                return value;
            }
            else
            {
                var value = parameterType.GetConstructors().First().Invoke(new object[0]);
                var valueProperties = parameterType.GetProperties().Where(p => p.CanWrite);

                foreach (var p in valueProperties)
                {
                    p.SetValue(value, GetParameterValue(p.PropertyType));
                }

                return value;
            }
        }

        class MethodCallContext
        {
            public ParameterInfo[] ConstructorParameters { get; set; }
            public object ServiceInstance { get; set; }
            public MethodInfo Method { get; set; }
            public ParameterInfo[] MethodParameters { get; set; }
            public object[] MethodParametersValues { get; set; }
        }
    }
}
