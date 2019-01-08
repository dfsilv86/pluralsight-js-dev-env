using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.UnitTests
{
    /// <summary>
    /// Testes utilizando reflection para dar cobertura de código em auto-properties.
    /// </summary>
    [TestFixture]
    [Category("Domain")]
    [Category("Auto")]
    public class AutoPropertiesAutoTest
    {
        [Test]
        public void Entities_AutoProperites_GetSet()
        {
            var assemblies = new Assembly[] { typeof(Usuario).Assembly, typeof(EntityBase).Assembly };
            var entityTypes = new List<Type>();

            foreach (var assembly in assemblies)
            {
                entityTypes.AddRange(assembly.GetTypes().Where(t => !t.ContainsGenericParameters && t.GetConstructor(new Type[0]) != null));
            }

            var propertiesCount = 0;

            foreach (var entityType in entityTypes)
            {
                var testName = "{0}_Properties_AutoTest".With(entityType.Name);

                TeamCityService.ReportTestStarted(testName);
                var instance = Activator.CreateInstance(entityType);
                var properties = entityType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(p => p.CanWrite && p.SetMethod.IsDefined(typeof(CompilerGeneratedAttribute)));

                foreach (var p in properties)
                {
                    propertiesCount++;
                    var expected = p.GetValue(instance);
                    p.SetValue(instance, expected);
                    var actual = p.GetValue(instance);
                    Assert.AreEqual(expected, actual);                    
                }

                TeamCityService.ReportTestFinished(testName);
            }

            Assert.AreNotEqual(0, propertiesCount);
        }
    }
}
