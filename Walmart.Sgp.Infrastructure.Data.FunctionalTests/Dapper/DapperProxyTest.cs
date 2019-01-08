using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Data.Dapper;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.FunctionalTests.Dapper
{
    [TestFixture]
    [Category("Dapper")]
    public class DapperProxyTest
    {
        [Test]
        public void Execute_AsPaging_PagedResults()
        {
            this.RunTransaction(appDbs =>
            {
                var usuarioGateway = new DapperUsuarioGateway(appDbs);
                var usuarioCount = usuarioGateway.Count("Username is not null", new {});
                var password = Guid.NewGuid().ToString();

                for (int i = 0; i < 200; i++)
                {
                    usuarioGateway.Insert(new Usuario { UserName = "u" + (usuarioCount + i), Email = "e" + (i + 1), Passwd = password });
                }

                for (int i = 200; i < 210; i++)
                {
                    usuarioGateway.Insert(new Usuario { UserName = "u" + (usuarioCount + i), Email = "e" + (i + 1), Passwd = "TESTE" });
                }

                var target = new DapperProxy(appDbs.Wlmslp, CommandType.Text);
                

                // Select 1.
                var queryResult = target
                .Query<Usuario>("SELECT Id, Username, email FROM CWIUSER where passwd = @password", new { password })
                .AsPaging(new Paging(0, 100, "Id"));

                var actual = queryResult.ToList();

                Assert.AreEqual(100, actual.Count);
                Assert.AreEqual("e1", actual[0].Email);
                Assert.AreEqual("e100", actual[99].Email);
                var paginated = queryResult as IPaginated;
                Assert.AreEqual(200, paginated.TotalCount);
                Assert.AreEqual(0, paginated.Paging.Offset);
                Assert.AreEqual(100, paginated.Paging.Limit);
                Assert.AreEqual("Id", paginated.Paging.OrderBy);

                // Select 2.
                queryResult = target
                .Query<Usuario>("SELECT Id, Username, email FROM CWIUSER where passwd = @password", new { password })
                .AsPaging(new Paging(1, 10, "Id"));

                actual = queryResult.ToList();

                Assert.AreEqual(10, actual.Count);
                Assert.AreEqual("e2", actual[0].Email);
                Assert.AreEqual("e11", actual[9].Email);
                paginated = queryResult as IPaginated;
                Assert.AreEqual(200, paginated.TotalCount);
                Assert.AreEqual(1, paginated.Paging.Offset);
                Assert.AreEqual(10, paginated.Paging.Limit);
                Assert.AreEqual("Id", paginated.Paging.OrderBy);

                // Select 3.
                queryResult = target
               .Query<Usuario>("SELECT Id, Username, email FROM CWIUSER where passwd = @password", new { password })
               .AsPaging(new Paging(1, 20));

                actual = queryResult.ToList();

                Assert.AreEqual(20, actual.Count);
                paginated = queryResult as IPaginated;
                Assert.AreEqual(200, paginated.TotalCount);
                Assert.AreEqual(1, paginated.Paging.Offset);
                Assert.AreEqual(20, paginated.Paging.Limit);
                Assert.AreEqual("(select 1)", paginated.Paging.OrderBy);
            });
        }
    }
}
