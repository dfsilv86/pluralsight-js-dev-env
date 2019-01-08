using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Data.FunctionalTests.Dapper;
using NUnit.Framework;
using Walmart.Sgp.Infrastructure.Data.Dapper;
using System.Diagnostics;
using NUnit.Framework.Internal;
using System.Runtime.CompilerServices;

namespace Walmart.Sgp.Infrastructure.Data.FunctionalTests
{
    public abstract class DataGatewayTestBase<TDataGateway>
    {
#if CI
        public const int TimesToRunEachGateway = 1;
#else
        public const int TimesToRunEachGateway = 10;
#endif

        private Type[] m_gatewayTypes;

        protected DataGatewayTestBase()
        {
            var gatewayInterfaceType = typeof(TDataGateway);

            m_gatewayTypes = typeof(DapperUsuarioGateway).Assembly.GetTypes()
                .Where(t => gatewayInterfaceType.IsAssignableFrom(t) && !t.Name.StartsWith("Memory"))
                .OrderBy(t => t.Name).ToArray();

            // m_gatewayTypes = new Type[] { m_gatewayTypes.Last() };
            Assert.AreNotEqual(0, m_gatewayTypes.Length, "Não foram localizadas implementações de {0}", gatewayInterfaceType.Name);
        }

        protected void RunForEachGateway(Action<ApplicationDatabases, TDataGateway> action, [CallerMemberName] string testName = "", bool rollback = true)
        {
            foreach(var gatewayType in m_gatewayTypes)
            {
                try
                {
                    var sw = new Stopwatch();
                    sw.Start();

                    for (int i = 0; i < TimesToRunEachGateway; i++)
                    {
                        this.RunTransaction((appDbs) =>
                        {
                            var gateway = (TDataGateway)Activator.CreateInstance(gatewayType, appDbs);
                            action(appDbs, gateway);
                        }, rollback);
                    }

                    sw.Stop();
                    
                    TestContext.WriteLine(
                        "{0}.{1} runs {2} times in {3} milliseconds. Average: {4}", 
                        gatewayType.Name, 
                        testName, 
                        TimesToRunEachGateway,
                        sw.ElapsedMilliseconds,
                        sw.ElapsedMilliseconds / TimesToRunEachGateway);
                }
                catch(Exception ex)
                {
                    throw new AssertionException("{0}: {1}".With(gatewayType.Name, ex.Message), ex);
                }
            }
        }
    }
}
