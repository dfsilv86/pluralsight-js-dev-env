using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Domain
{
    [TestFixture]
    [Category("Framework")]
    public class MemoryDataGatewayTest
    {
        #region Fields
        private MemoryDataGateway<Usuario> m_target;
        #endregion

        #region Initialize
        [SetUp]
        public void InitializeTest()
        {
            m_target = new MemoryDataGateway<Usuario>();
            m_target.Insert(new Usuario { UserName = "Usuario 1", Email = "usuario1@cwi.com.br" });
            m_target.Insert(new Usuario { UserName = "Usuario 2", Email = "usuario2@cwi.com.br" });
            m_target.Insert(new Usuario { UserName = "Usuario 3", Email = "usuario3@cwi.com.br" });
            m_target.Insert(new Usuario { UserName = "Usuario 11", Email = "usuario11@cwi.com.br" });
        }
        #endregion

        #region Methods
        [Test]
        public void Count_Filter_FilteredCount()
        {
            Assert.AreEqual(1, m_target.Count("Id = @Id", new { Id = 1 }));
            Assert.AreEqual(1, m_target.Count("Id = @ID", new { Id = 2 }));
            Assert.AreEqual(1, m_target.Count("Id = @Id", new { Id = 3 }));
            Assert.AreEqual(3, m_target.Count("Id <> @Id", new { Id = 3 }));
            Assert.AreEqual(0, m_target.Count("Username =  @Username", new { Username = (string)null }));
            Assert.AreEqual(2, m_target.Count("UserName LIKE @Username", new { Username = "Usuario 1" }));
        }

        [Test]
        public void Count_Filters_FilteredCounts()
        {
            var actual = m_target.Count(new { Id = 0, Id1 = 1, Id2 = 2, Id3 = 3 }, "Id = @Id1", "Id = @Id1 OR Id = @Id2", "Id <> @Id3").ToArray();
            Assert.AreEqual(3, actual.Length);
            Assert.AreEqual(1, actual[0]);
            Assert.AreEqual(2, actual[1]);
            Assert.AreEqual(3, actual[2]);
        }

        [Test]
        public void Delete_Id_Deleted()
        {
            m_target.Delete(2);
            Assert.AreEqual(3, m_target.Count("Id <> 2", new { }));
            Assert.AreEqual(0, m_target.Count("Id = 2", new { }));
        }

        [Test]
        public void Delete_Filter_FilteredDeleted()
        {
            m_target.Delete("Id <> @Id", new { Id = 2 });
            Assert.AreEqual(0, m_target.Count("Id <> 2", new { }));
            Assert.AreEqual(1, m_target.Count("Id = 2", new { }));
        }

        [Test]
        public void FindByIds_Filter_Filtered()
        {            
            Assert.AreEqual(1, m_target.FindByIds(1, 10).Count());
            Assert.AreEqual(2, m_target.FindByIds(1, 2).Count());
            Assert.AreEqual(3, m_target.FindByIds(1, 2, 3).Count());
        }

        [Test]
        public void FindByIds_Ids_Filtered()
        {            
            Assert.AreEqual(3, m_target.Find("Id <> @Id", new { Id = 2 }).Count());
            Assert.AreEqual(3, m_target.Find("Id, Email", "Id <> @Id AND Email LIKE @Email", new { Id = 2, Email = "cwi.com.br" }).Count());
            Assert.AreEqual(1, m_target.Find("Id = @Id", new { Id = 2 }).Count());
        }

        [Test]
        public void Find_TModel_Filtered()
        {
            var actual = m_target.Find<Permissao>("Id", "Id = @Id", new { Id = 2 }).First();
            Assert.IsInstanceOf<Permissao>(actual);

            Assert.AreEqual(2, actual.Id);

            actual = m_target.Find<Permissao>("Id", "UserName = @username", new { userName = "Usuario 1" }).First();
            Assert.IsInstanceOf<Permissao>(actual);
            Assert.AreEqual(1, actual.Id);

            actual = m_target.Find<Permissao>("Id", "UserName = @username", new { userName = "Usuario 1" }, new Paging(0, 10, "Id")).First();
            Assert.IsInstanceOf<Permissao>(actual);
            Assert.AreEqual(1, actual.Id);
        }

        [Test]
        public void Find_TEntity_Filtered()
        {
            var actual = m_target.Find("UserName = @username", new { userName = "Usuario 1" }, new Paging(0, 10, "Id"));
            Assert.AreEqual(1, actual.Count());
            Assert.AreEqual(1, actual.First().Id);

            actual = m_target.Find("UserName = @username", new { userName = "Usuario 1" }, new Paging(1, 10, "Id"));
            Assert.AreEqual(0, actual.Count());
        }

        [Test]
        public void FindAll_NoArgs_AllEntities()
        {
            Assert.AreEqual(4, m_target.FindAll().Count());
        }

        [Test]
        public void FindAll_Paging_PaginatedEntities()
        {
            var paging = new Paging
            {
                 Limit = 1,
                 Offset = 0,
                 OrderBy = "UserName"
            };
            
            var actual = m_target.FindAll(paging).ToArray();
            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual("Usuario 1", actual[0].UserName);

            paging.OrderBy = "UserName DESC";
            actual = m_target.FindAll(paging).ToArray();
            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual("Usuario 3", actual[0].UserName);

            paging.OrderBy = "UserName ASC";
            paging.Limit = 2;
            paging.Offset = 1;
            actual = m_target.FindAll(paging).ToArray();
            Assert.AreEqual(2, actual.Length);
            Assert.AreEqual("Usuario 11", actual[0].UserName);
            Assert.AreEqual("Usuario 2", actual[1].UserName);
        }

        [Test]
        public void Insert_Entities_Inserted()
        {
            m_target.Insert(new Usuario[] { new Usuario(), new Usuario() });
            Assert.AreEqual(6, m_target.Count("Id <> 0", new { }));
        }

        [Test]
        public void Insert_WithChilrenEntities_Inserted()
        {
            var target = new MemoryDataGateway<Permissao>();
            target.Insert(new Permissao
            {
                IDUsuario = 1,                                
                Bandeiras = new PermissaoBandeira[] { new PermissaoBandeira() { IDBandeira = 11 }, new PermissaoBandeira() { IDBandeira = 111 } },
                Lojas = new PermissaoLoja[] { new PermissaoLoja() { IDLoja = 22 }, new PermissaoLoja() { IDLoja = 222 } }
            });

            var actual = target.Find("IdUsuario = 1", null).Single();
            Assert.AreNotEqual(0, actual.Id);
            Assert.AreNotEqual(0, actual.Bandeiras.First().Id);
            Assert.AreNotEqual(0, actual.Bandeiras.Last().Id);

            Assert.AreNotEqual(0, actual.Lojas.First().Id);
            Assert.AreNotEqual(0, actual.Lojas.Last().Id);
        }

        [Test]
        public void Insert_WithChilrenEntitiesNoAggregateRoot_Inserted()
        {
            var target = new MemoryDataGateway<EntityStub>();
            target.Insert(new EntityStub
            {
                 Value = 1,
                 Child = new EntityNoAggregateRootStub {  Value = 2 }
            });

            var actual = target.Find("Value = 1", null).Single();
            Assert.AreNotEqual(0, actual.Id);
            Assert.AreEqual(1, actual.Value);
            Assert.AreEqual(2, actual.Child.Value);
        }

        [Test]
        public void Update_ExistingEntity_Updated()
        {
            m_target.Update(new Usuario { Id = 2, UserName = "Usuario 2.updated", Email = "usuario1@cwi.com.br" } );
            Assert.AreEqual(4, m_target.Count("Id <> 0", new { }));

            var actual = m_target.Find("Id = 2", null).Single();
            Assert.AreEqual("Usuario 2.updated", actual.UserName);
        }

        [Test]
        public void Update_ExistingEntityWithChild_Updated()
        {
            var inserted = new EntityStub
            {
                 Value = 1,
                 Child = new EntityNoAggregateRootStub { Value = 2 }
            };

            var target = new MemoryDataGateway<EntityStub>();
            target.Insert(inserted);
            target.Update(new EntityStub { Id = inserted.Id, Value = 11, Child = new EntityNoAggregateRootStub { Value = 22 } });
            
            var actual = target.Find("Value = 11", null).Single();
            Assert.AreEqual(inserted.Id, actual.Id);
            Assert.AreEqual(11, actual.Value);
            Assert.AreEqual(22, actual.Child.Value);
        }
      
        [Test]
        public void Update_Model_Updated()
        {
            m_target.Update("Username = @Username", new Usuario { Id = 2, UserName = "Usuario 2.updated", Email = "usuario1@cwi.com.br" });
            Assert.AreEqual(4, m_target.Count("Id <> 0", new { }));

            var actual = m_target.Find("Id = 2", null).Single();
            Assert.AreEqual("Usuario 2.updated", actual.UserName);
        }

        [Test]
        public void Update_Sets_Updated()
        {
            m_target.Update("Username = @Username, Comment = @Comment", "Email LIKE @Email", new Usuario { Id = 2, UserName = "Usuario 1*.updated", Email = "usuario1", Comment = "teste atualizado" });
            Assert.AreEqual(4, m_target.Count("Id <> 0", new { }));

            var actual = m_target.Find("Email LIKE @Email", new { Email = "usuario1" }).ToList();

            Assert.AreEqual("Usuario 1*.updated", actual[0].UserName);
            Assert.AreEqual("Usuario 1*.updated", actual[1].UserName);
            Assert.AreEqual("teste atualizado", actual[0].Comment);
            Assert.AreEqual("teste atualizado", actual[1].Comment);
        }

        [Test]
        public void Find_InvalidProperty_Exception()
        {
            Assert.Catch<InvalidOperationException>(() =>
            {
                m_target.Find("IdUsuario = @Wrong", new { IdUsuario = 1 });
            }, Texts.PropertyNotFoundOnFilterArgs, "Wrong", "IdUsuario = @Wrong");
        }
        #endregion
    }
}