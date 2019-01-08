using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Data.Dapper;
using Walmart.Sgp.Infrastructure.Data.FunctionalTests.Commons;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.FunctionalTests
{
    [TestFixture]
    [Category("Ado")]
    [Category("Dapper")]
    public class UsuarioGatewayTest : DataGatewayTestBase<IUsuarioGateway>
    {
        #region Constants
        private const int TotalToInsert = 10000;
        #endregion      

        #region Tests
        [Test]
        public void Count_Filter_Count()
        {
            this.RunForEachGateway((appDbs, target) =>
            {
                var user = new Usuario
                {
                    UserName = Guid.NewGuid().ToString(),
                    Passwd = "teste",
                    Email = "teste@cwi.com.br"
                };

                target.Insert(user);

                var actual = target.Count("Username <> @Username", new { Username = "" });
                Assert.AreNotEqual(0, actual);
            });
        }

        [Test]
        public void Delete_NonExistsId_Exception()
        {
            this.RunForEachGateway((appDbs, target) =>
            {
                Assert.Catch<InvalidOperationException>(() =>
                {
                    target.Delete(int.MaxValue);
                });
            });
        }

        [Test]
        public void Delete_ExistsId_Deleted()
        {
            this.RunForEachGateway((appDbs, target) =>
            {
                var user = new Usuario
                {
                    UserName = Guid.NewGuid().ToString(),
                    Passwd = "teste",
                    Email = "teste@cwi.com.br"
                };

                target.Insert(user);
                target.Delete(user.Id);                
            });
        }

        [Test]
        public void Delete_WithFilter_Deleted()
        {
            this.RunForEachGateway((appDbs, target) =>
            {
                var users = new Usuario[0];
                var email = Guid.NewGuid().ToString() + "_oldemail";

                for (int i = 0; i < users.Length; i++)
                {
                    users[i] = new Usuario
                    {
                        UserName = Guid.NewGuid().ToString() + "_TEST",
                        Passwd = "teste",
                        Email = email,
                        FailedPasswdCount = i
                    };
                }
                
                var filter = "Username LIKE @Username";
                var filterNot = "Username NOT LIKE @Username";
                var filterArgs = new { Username = "%_TEST" };

                var originalCount = target.Count(filterNot, filterArgs);
                
                target.Insert(users);
                Assert.AreEqual(users.Length, target.Count(filter, filterArgs));

                target.Delete(filter, filterArgs);
                Assert.AreEqual(0, target.Count(filter, filterArgs));
                Assert.AreEqual(originalCount, target.Count(filterNot, filterArgs));
            });
        }

        [Test]
        public void Find_ProjectionModel_OnlyProjecteProperties()
        {
            this.RunForEachGateway((appDbs, target) =>
            {
                var user1 = new Usuario
                {
                    UserName = Guid.NewGuid().ToString(),
                    Passwd = "teste1",
                    Email = "teste1@cwi.com.br"
                };

                var user2 = new Usuario
                {
                    UserName = Guid.NewGuid().ToString(),
                    Passwd = "teste2",
                    Email = "teste2@cwi.com.br"
                };

                target.Insert(user1);
                target.Insert(user2);
                
                var actual = target.Find<ModelStub>("Username, Email", "Email LIKE @Email", new { Email = "teste%" });
                Assert.LessOrEqual(2, actual.Count());

                foreach (var a in actual)
                {
                    Assert.IsFalse(String.IsNullOrEmpty(a.Username));
                    Assert.IsFalse(String.IsNullOrEmpty(a.Email));
                }
            });
        }

        [Test]
        public void Find_Projection_OnlyProjectedProperties()
        {
            this.RunForEachGateway((appDbs, target) =>
            {
                var user1 = new Usuario
                {
                    UserName = Guid.NewGuid().ToString(),
                    Passwd = "teste1",
                    Email = "teste1@cwi.com.br"
                };

                var user2 = new Usuario
                {
                    UserName = Guid.NewGuid().ToString(),
                    Passwd = "teste2",
                    Email = "teste2@cwi.com.br"
                };

                target.Insert(user1);
                target.Insert(user2);

                var actual = target.Find("Username, Email", "Email LIKE @Email", new { Email = "teste%" });
                Assert.LessOrEqual(2, actual.Count());

                foreach(var a in actual)
                {
                    Assert.IsFalse(String.IsNullOrEmpty(a.UserName));
                    Assert.IsFalse(String.IsNullOrEmpty(a.Email));
                    Assert.IsTrue(String.IsNullOrEmpty(a.Passwd));
                }
            });
        }

        [Test]
        public void Find_Filter_FilteredEntities()
        {
            this.RunForEachGateway((appDbs, target) =>
            {
                var user1 = new Usuario
                {
                    UserName = Guid.NewGuid().ToString(),
                    Passwd = "teste1",
                    Email = "teste1@cwi.com.br"
                };

                var user2 = new Usuario
                {
                    UserName = Guid.NewGuid().ToString(),
                    Passwd = "teste2",
                    Email = "teste2@cwi.com.br"
                };

                target.Insert(user1);
                target.Insert(user2);

                var actual = target.Find("Username = @Username AND Passwd = @Passwd AND Email = @Email", user1).ToList();
                Assert.AreEqual(1, actual.Count);
                Assert.AreEqual("teste1", actual[0].Passwd);

                actual = target.Find("Username = @Username AND Passwd = @Passwd AND Email = @Email", user1).ToList();
                Assert.AreEqual(1, actual.Count);
                Assert.AreEqual("teste1", actual[0].Passwd);

                actual = target.Find("Email LIKE @Email", new { Email = "%cwi.com.br" }).ToList();
                Assert.LessOrEqual(2, actual.Count);
            });
        }

        [Test]
        public void Insert_New_EntityIdSet()
        {
            this.RunForEachGateway((appDbs, target) =>
            {
                var originalCount = target.Count("Username <> @Username", new { Username = "" });
                var user = new Usuario
                {
                    UserName = Guid.NewGuid().ToString(),
                    Passwd = "teste",
                    Email = "teste@cwi.com.br"
                };

                target.Insert(user);
                var newCount = target.Count("Username <> @Username", new { Username = "" });

                Assert.AreNotEqual(0, user.Id);
                Assert.AreEqual(originalCount + 1, newCount);
            });
        }

#if PARALLEL_TESTS
        [Test]
        public void Insert_Parallel_EntitiesInserted()
        {
            long originalCount = 0;
            var gatewaysCount = 0;

            this.RunForEachGateway((appDbs, target) =>
            {
                originalCount = target.Count("Username <> @Username", new { Username = "" });
                gatewaysCount++;
            });

            Parallel.For(0, TotalToInsert, (i) =>
            {
                this.RunForEachGateway((appDbs, target) =>
                {
                    var user = new Usuario
                    {
                        UserName = Guid.NewGuid().ToString(),
                        Passwd = "teste",
                        Email = "teste@cwi.com.br"
                    };

                    target.Insert(user);
                    Assert.AreNotEqual(0, user.Id);

                    appDbs.Wlmslp.Transaction.Commit();
                });
            });

            this.RunForEachGateway((appDbs, target) =>
            {
                var count = target.Count("Username <> @Username", new { Username = "" });
                Assert.AreEqual(originalCount + TotalToInsert * gatewaysCount, count);
            });

            this.RunForEachGateway((appDbs, target) =>
            {
                target.Delete("Email = @email", new { email = "teste@cwi.com.br" });
                appDbs.Wlmslp.Transaction.Commit();
            });            
        }
#endif
        [Test]
        public void Update_New_Exception()
        {
            this.RunForEachGateway((appDbs, target) =>
            {
                var user = new Usuario
                {
                    UserName = Guid.NewGuid().ToString(),
                    Passwd = "teste",
                    Email = "teste@cwi.com.br"
                };

                Assert.Catch<InvalidOperationException>(() =>
                {
                    target.Update(user);
                });
            });
        }

        [Test]
        public void Update_OldWithModel_Updated()
        {
            this.RunForEachGateway((appDbs, target) =>
            {
                var user = new Usuario
                {
                    UserName = Guid.NewGuid().ToString(),
                    Passwd = "teste",
                    Email = "teste@cwi.com.br"
                };

                target.Insert(user);

                var userModel = new ModelStub
                {
                    Id = user.Id,
                    Email = Guid.NewGuid().ToString()
                };
                
                target.Update("Email = @Email",  userModel);

                var actual = target.Find("Email = @Email", userModel).ToList();
                Assert.AreEqual(1, actual.Count);
            });
        }

        [Test]
        public void Update_Old_Updated()
        {
            this.RunForEachGateway((appDbs, target) =>
            {
                var user = new Usuario
                {
                    UserName = Guid.NewGuid().ToString(),
                    Passwd = "teste",
                    Email = "teste@cwi.com.br"
                };

                target.Insert(user);
                user.Email = Guid.NewGuid().ToString();
                target.Update(user);

                var actual = target.Find("Email = @Email", user).ToList();
                Assert.AreEqual(1, actual.Count);
            });
        }       

        [Test]
        public void Update_WithFilter_Updated()
        {
            this.RunForEachGateway((appDbs, target) =>
            {
                var users = new Usuario[10];
                var email = Guid.NewGuid().ToString() + "_oldemail";

                for (int i = 0; i < 10; i++)
                {
                    users[i] = new Usuario
                    {
                        UserName = Guid.NewGuid().ToString(),
                        Passwd = "teste",
                        Email = email
                    };
                }

                target.Insert(users);

                var newEmail = Guid.NewGuid().ToString() + "_newEmail";

                target.Update(
                    "Email = @NewEmail, Passwd = @NewPasswd", 
                    "Email = @Email", 
                    new { NewEmail = newEmail, NewPasswd = "teste2", Email = email } );

                Assert.AreEqual(0, target.Count("Email = @Email", new { Email = email }));
                Assert.LessOrEqual(10, target.Count("Email = @Email", new { Email = newEmail }));
            });
        }
        #endregion

        #region Benchmark
        [Test]
        [Category("Benchmark")]
        public void Insert_1000UsersBatch_Inserted()
        {
            this.RunForEachGateway((appDbs, target) =>
            {
                target.Delete("Id > @Id", new { Id = 100 });
                var originalCount = target.Count("Username <> @Username", new { Username = "" });
                var users = new Usuario[1000];

                for (int i = 0; i < users.Length; i++)
                {
                    users[i] = new Usuario
                    {
                        UserName = Guid.NewGuid().ToString(),
                        Passwd = "teste",
                        Email = "teste@cwi.com.br"
                    };
                }

                target.Insert(users);
                var newCount = target.Count("Username <> @Username", new { Username = "" });
                Assert.AreEqual(originalCount + users.Length, newCount);
            });
        }

        [Test]
        [Category("Benchmark")]
        public void Insert_1000Users_Inserted()
        {
            this.RunForEachGateway((appDbs, target) =>
            {
                target.Delete("Id > @Id", new { Id = 100 });
                var originalCount = target.Count("Username <> @Username", new { Username = "" });

                for (int i = 0; i < 1000; i++)
                {
                    target.Insert(new Usuario
                    {
                        UserName = Guid.NewGuid().ToString(),
                        Passwd = "teste",
                        Email = "teste@cwi.com.br"
                    });
                }

                var newCount = target.Count("Username <> @Username", new { Username = "" });
                Assert.AreEqual(originalCount + 1000, newCount);
            });
        }

        [Test]
        [Category("Benchmark")]
        public void Update_1000Users_Updated()
        {
            this.RunForEachGateway((appDbs, target) =>
            {
                target.Delete("Id > @Id", new { Id = 100 });
                var newEmail = Guid.NewGuid().ToString();

                for (int i = 0; i < 1000; i++)
                {
                    var user = new Usuario
                    {
                        UserName = Guid.NewGuid().ToString(),
                        Passwd = "teste",
                        Email = "teste@cwi.com.br"
                    };

                    target.Insert(user);
                    user.Email = newEmail;
                    target.Update(user);
                }

                var actual = target.Count("Email = @Email", new { Email = newEmail });
                Assert.AreEqual(1000, actual);
            });
        }


        [Test]
        [Category("Benchmark")]
        public void Delete_1000Users_Deleted()
        {
            this.RunForEachGateway((appDbs, target) =>
            {
                target.Delete("Id > @Id", new { Id = 100 });

                for (int i = 0; i < 1000; i++)
                {
                    var user = new Usuario
                    {
                        UserName = Guid.NewGuid().ToString(),
                        Passwd = "teste",
                        Email = "teste@cwi.com.br"
                    };

                    target.Insert(user);
                    target.Delete(user.Id);

                }

                var actual = target.Count("Email = @Email", new { Email = "teste@cwi.com.br" });
                Assert.AreEqual(0, actual);
            });
        }

        [Test]
        [Category("Benchmark")]
        public void Find_1000Users_Filtered()
        {
            this.RunForEachGateway((appDbs, target) =>
            {
                target.Delete("Id > @Id", new { Id = 100 });

                for (int i = 0; i < 1000; i++)
                {
                    var user = new Usuario
                    {
                        UserName = Guid.NewGuid().ToString(),
                        Passwd = "teste",
                        Email = "teste@cwi.com.br"
                    };

                    target.Insert(user);
                }

                var actual = target.Find("Email = @Email", new { Email = "teste@cwi.com.br" });
                Assert.AreEqual(1000, actual.Count());
            });
        }
        #endregion
    }
}
