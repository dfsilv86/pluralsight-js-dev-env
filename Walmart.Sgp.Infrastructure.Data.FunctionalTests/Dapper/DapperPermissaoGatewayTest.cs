using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Data.Dapper;

namespace Walmart.Sgp.Infrastructure.Data.FunctionalTests.Dapper
{
    [TestFixture]
    [Category("Dapper")]
    public class DapperPermissaoGatewayTest
    {
        [Test]
        public void Insert_Permissao_Inserted()
        {
            this.RunTransaction(appDbs =>
            {
                var target = new DapperPermissaoGateway(appDbs);
                var expected = new Permissao
                {
                    Bandeiras = new PermissaoBandeira[] { new PermissaoBandeira { IDBandeira = 1 }, new PermissaoBandeira { IDBandeira = 2 } },
                    Lojas = new PermissaoLoja[] { new PermissaoLoja { IDLoja = 1 } }
                };

                target.Insert(expected);
                Assert.AreNotEqual(0, expected.Id);

                var actual = target.Find("IdPermissao = @Id", expected).FirstOrDefault();
                Assert.AreEqual(expected.Id, actual.Id);

                var actualBandeiras = actual.Bandeiras.ToList();
                Assert.AreEqual(2, actualBandeiras.Count);
                Assert.AreEqual(1, actualBandeiras[0].IDBandeira);
                Assert.AreEqual(2, actualBandeiras[1].IDBandeira);

                var actualLojas = actual.Lojas.ToList();
                Assert.AreEqual(1, actualLojas.Count);
                Assert.AreEqual(1, actualLojas[0].IDLoja);                
            });
        }

        [Test]
        public void Update_Permissao_Updated()
        {
            this.RunTransaction(appDbs =>
            {
                var target = new DapperPermissaoGateway(appDbs);
                var expected = new Permissao
                {
                    Bandeiras = new PermissaoBandeira[] { new PermissaoBandeira { IDBandeira = 1 }, new PermissaoBandeira { IDBandeira = 2 } },
                    Lojas = new PermissaoLoja[] { new PermissaoLoja { IDLoja = 1 }, new PermissaoLoja { IDLoja = 2 } }
                };
                
                target.Insert(expected);
                
                // Removendo uma bandeira, alterado uma existente e adicionando duas outras.
                var newBandeiras = expected.Bandeiras.Take(1).ToList();
                newBandeiras[0].IDBandeira = 3;
                newBandeiras.Add(new PermissaoBandeira { IDBandeira = 4 });
                newBandeiras.Add(new PermissaoBandeira { IDBandeira = 5 });
                expected.Bandeiras = newBandeiras;

                var newLojas = expected.Lojas.Take(1).ToList();
                newLojas[0].IDLoja = 3;
                newLojas.Add(new PermissaoLoja { IDLoja = 4 });
                newLojas.Add(new PermissaoLoja { IDLoja = 5 });
                expected.Lojas = newLojas;

                target.Update(expected);

                var actual = target.Find("IdPermissao = @Id", expected).FirstOrDefault();
                Assert.AreEqual(expected.Id, actual.Id);

                var actualBandeiras = actual.Bandeiras.ToList();
                Assert.AreEqual(3, actualBandeiras.Count);
                Assert.AreEqual(3, actualBandeiras[0].IDBandeira);
                Assert.AreEqual(4, actualBandeiras[1].IDBandeira);
                Assert.AreEqual(5, actualBandeiras[2].IDBandeira);

                var actualLojas = actual.Lojas.ToList();
                Assert.AreEqual(3, actualLojas.Count);
                Assert.AreEqual(3, actualLojas[0].IDLoja);
                Assert.AreEqual(4, actualLojas[1].IDLoja);
                Assert.AreEqual(5, actualLojas[2].IDLoja);
            });
        }

        [Test]
        public void Delete_Permissao_Deleted()
        {
            this.RunTransaction(appDbs =>
            {
                var target = new DapperPermissaoGateway(appDbs);
                var expected = new Permissao
                {
                    Bandeiras = new PermissaoBandeira[] { new PermissaoBandeira { IDBandeira = 1 }, new PermissaoBandeira { IDBandeira = 2 } },
                    Lojas = new PermissaoLoja[] { new PermissaoLoja { IDLoja = 1 } }
                };

                target.Insert(expected);
                Assert.AreNotEqual(0, expected.Id);

                var inserted = target.Find("IdPermissao = @Id", expected).SingleOrDefault();
                Assert.AreEqual(expected.Id, inserted.Id);

                appDbs.PermissaoBandeira().IsCount(2, "IdPermissao = @IdPermissao", inserted);
                appDbs.PermissaoLoja().IsCount(1, "IdPermissao = @IdPermissao", inserted);

                target.Delete(expected.Id);
                var actual = target.Find("IdPermissao = @Id", expected).SingleOrDefault();
                Assert.IsNull(actual);

                appDbs.PermissaoBandeira().IsCount(0, "IdPermissao = @IdPermissao", inserted);
                appDbs.PermissaoLoja().IsCount(0, "IdPermissao = @IdPermissao", inserted);
            });
        }

        [Test]
        public void Pesquisar_Filtros_Permissoes()
        {
            this.RunTransaction(appDbs =>
            {
                var usuarioGateway = new DapperUsuarioGateway(appDbs);
                var u1 = new Usuario
                {
                    UserName = "DapperUsuarioGateway1",
                    Passwd = "pwd",
                    Email = "email"
                };
                usuarioGateway.Insert(u1);

                var u2 = new Usuario
                {
                    UserName = "DapperUsuarioGateway2",
                    Passwd = "pwd",
                    Email = "email"
                };
                usuarioGateway.Insert(u2);

                var target = new DapperPermissaoGateway(appDbs);
                foreach(var p in target.FindAll())
                {
                    target.Delete(p.Id);
                }                

                var p1 = new Permissao
                {
                    IDUsuario = u1.Id,
                    Bandeiras = new PermissaoBandeira[] { new PermissaoBandeira { IDBandeira = 1 }, new PermissaoBandeira { IDBandeira = 2 } },
                    Lojas = new PermissaoLoja[] { new PermissaoLoja { IDLoja = 1 } }
                };
                target.Insert(p1);

                var p2 = new Permissao
                {
                    IDUsuario = u2.Id,
                    Bandeiras = new PermissaoBandeira[] { new PermissaoBandeira { IDBandeira = 2 } },
                    Lojas = new PermissaoLoja[] { new PermissaoLoja { IDLoja = 2 } }
                };
                target.Insert(p2);

                var actual = target.Pesquisar(null, null, null).ToList();
                Assert.AreEqual(2, actual.Count);
                Assert.AreEqual(p1.Id, actual[0].Id);
                Assert.AreEqual(2, actual[0].Bandeiras.Count);
                Assert.AreEqual(1, actual[0].Lojas.Count);

                Assert.AreEqual(p2.Id, actual[1].Id);
                Assert.AreEqual(1, actual[1].Bandeiras.Count);
                Assert.AreEqual(1, actual[1].Lojas.Count);

                actual = target.Pesquisar(u1.Id, null, null).ToList();
                Assert.AreEqual(1, actual.Count);
                Assert.AreEqual(2, actual[0].Bandeiras.Count);
                Assert.AreEqual(1, actual[0].Lojas.Count);
                Assert.AreEqual(p1.Id, actual[0].Id);

                actual = target.Pesquisar(u1.Id, 1, null).ToList();
                Assert.AreEqual(1, actual.Count);
                Assert.AreEqual(p1.Id, actual[0].Id);

                actual = target.Pesquisar(u1.Id, 1, 1).ToList();
                Assert.AreEqual(1, actual.Count);
                Assert.AreEqual(p1.Id, actual[0].Id);

                actual = target.Pesquisar(u1.Id, 2, 1).ToList();
                Assert.AreEqual(1, actual.Count);
                Assert.AreEqual(p1.Id, actual[0].Id);

                actual = target.Pesquisar(u1.Id, 2, 1).ToList();
                Assert.AreEqual(1, actual.Count);
                Assert.AreEqual(p1.Id, actual[0].Id);

                actual = target.Pesquisar(u1.Id, 3, 1).ToList();
                Assert.AreEqual(0, actual.Count);

                actual = target.Pesquisar(u1.Id, 2, 3).ToList();
                Assert.AreEqual(0, actual.Count);
            });
        }
    }
}
