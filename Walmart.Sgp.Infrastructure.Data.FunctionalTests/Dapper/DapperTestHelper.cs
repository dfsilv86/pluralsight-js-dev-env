using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Data.Databases;

namespace Walmart.Sgp.Infrastructure.Data.FunctionalTests.Dapper
{
    public static class DapperTestHelper
    {
        public static void RunTransaction(this object test, Action<ApplicationDatabases> action, bool rollback = true)
        {
            using (var appDatabases = new ApplicationDatabases())
            {
                action(appDatabases);

                var trn = appDatabases.Wlmslp.Transaction;

                if (rollback && trn.Connection != null)
                {
                    trn.Rollback();
                }
            }
        }
    }
}
