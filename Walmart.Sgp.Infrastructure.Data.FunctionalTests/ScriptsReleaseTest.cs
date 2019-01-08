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

namespace Walmart.Sgp.Infrastructure.Data.FunctionalTests
{
    [TestFixture]
    [Category("ScriptsRelease")]
    public class ScriptsReleaseTest
    {       
        [Test]
        public void DataGateways_Table_ShouldHaveTableCreated()
        {            
            this.RunTransaction((appDb) =>
            {
                var proxy = new DapperProxy(appDb.Wlmslp, CommandType.Text);
                var gateways = typeof(DapperProxy).Assembly.GetTypes()
                    .Where(t => t.BaseType != null && t.BaseType.Name.Equals("EntityDapperDataGatewayBase`1"))
                    .ToArray();

                foreach (var gateway in gateways)
                {
                    var instance = Activator.CreateInstance(gateway, new object[] { new ApplicationDatabases() });
                    var tableModel = EntityTableModelRegistry.GetTableModelForEntity(gateway.BaseType.GenericTypeArguments[0]);
                    var tableName = tableModel.Item1;

                    try
                    {
                        proxy.Execute("SELECT TOP 1 * FROM {0}".With(tableName), null);
                    }
                    catch (Exception)
                    {
                        Assert.Fail("Parece que a tabela '{0}' utilizada pelo table data gateway '{1}' não foi criada no banco. Faltou adicionar script em SQL\\ScriptsRelease?", tableName, gateway.Name);
                    }
                }
            });
        }

        [Test]
        public void Services_UsingAuditService_MainEntityShouldHaveTableLogCreated()
        {
            this.RunTransaction((appDb) =>
            {
                var proxy = new DapperProxy(appDb.Wlmslp, CommandType.Text);
                var services = typeof(Usuario).Assembly.GetTypes()
                    .Where(t => t.Name.EndsWith("Service") && t.GetConstructors().Any(c => c.GetParameters().Any(p => typeof(IAuditService) == p.ParameterType)))
                    .ToArray();

                foreach (var service in services)
                {
                    var entityName = service.Name.Replace("Service", "");

                    try
                    {
                        proxy.Execute("SELECT TOP 1 * FROM {0}Log".With(entityName), null);
                    }
                    catch (Exception)
                    {
                        Assert.Fail("Parece que o serviço '{0}Service' utiliza o IAuditService, entretanto não existe a tabela de log '{0}Log' criada no banco. Faltou adicionar script em SQL\\ScriptsRelease? (http://git.cwi.com.br/walmart/walmart-sgp-reescrita/wikis/logs#auditoria-de-inclus%C3%A3o-altera%C3%A7%C3%A3o-e-exclus%C3%A3o-de-registros)", entityName);
                    }
                }
            });
        }        
    }
}
