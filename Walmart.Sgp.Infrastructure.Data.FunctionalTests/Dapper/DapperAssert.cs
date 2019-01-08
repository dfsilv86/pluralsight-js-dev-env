using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Walmart.Sgp.Infrastructure.Data.Dapper;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.FunctionalTests.Dapper
{
    public static class DapperAssert
    {
        public static DapperPermissaoBandeiraGateway PermissaoBandeira(this ApplicationDatabases appDb)
        {
            return new DapperPermissaoBandeiraGateway(appDb);
        }

        public static DapperPermissaoLojaGateway PermissaoLoja(this ApplicationDatabases appDb)
        {
            return new DapperPermissaoLojaGateway(appDb);
        }

        public static void IsCount<TEntity>(this EntityDapperDataGatewayBase<TEntity> gateway, int expected, string filter, object filterArgs)
            where TEntity : IEntity
        {
            Assert.AreEqual(expected, gateway.Count(filter, filterArgs));
        }        
    }
}
