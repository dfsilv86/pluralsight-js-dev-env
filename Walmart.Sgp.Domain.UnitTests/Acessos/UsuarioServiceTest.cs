using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Rhino.Mocks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Data.Memory;
using Walmart.Sgp.Infrastructure.Framework.Domain;

using Is = Rhino.Mocks.Constraints.Is;

namespace Walmart.Sgp.Domain.UnitTests.Acessos
{
    [TestFixture]
    [Category("Domain")]
    public class UsuarioServiceTest
    {
        [Test]
        public void Save_NewUser_Created()
        {
            var user = new Usuario();
            var userGateway = MockRepository.GenerateMock<IUsuarioGateway>();
            userGateway.Expect(u => u.Insert(user));

            var target = new UsuarioService(userGateway);
            target.Salvar(user);

            userGateway.VerifyAllExpectations();
        }

        [Test]
        public void Save_OldUser_Updated()
        {
            var user = new Usuario() { Id = 1 };
            var userGateway = MockRepository.GenerateMock<IUsuarioGateway>();
            userGateway.Expect(u => u.Update(user));

            var target = new UsuarioService(userGateway);
            target.Salvar(user);

            userGateway.VerifyAllExpectations();
        }

        [Test]
        public void GetAll_NoFilter_FilteredEntities()
        {
            var userGateway = MockRepository.GenerateMock<IUsuarioGateway>();
            userGateway.Expect(u => u.FindAll()).Return(new Usuario[] { new Usuario(), new Usuario() });

            var target = new UsuarioService(userGateway);
            var actual = target.ObterTodos();
            Assert.AreEqual(2, actual.Count());

            userGateway.VerifyAllExpectations();
        }

        [Test]
        public void GetById_Id_Entity()
        {
            var userGateway = MockRepository.GenerateMock<IUsuarioGateway>();
            userGateway.Expect(u => u.FindById(1)).IgnoreArguments().Return(new Usuario());

            var target = new UsuarioService(userGateway);
            var actual = target.ObterPorId(1);
            Assert.IsNotNull(actual);

            userGateway.VerifyAllExpectations();
        }

        [Test]
        public void ObterPorUserName_UserName_Usuario()
        {
            var userGateway = new MemoryUsuarioGateway();
            userGateway.Insert(new Usuario { UserName = "user1" });
            userGateway.Insert(new Usuario { UserName = "user2" });

            var target = new UsuarioService(userGateway);
            var actual = target.ObterPorUserName("user1");
            Assert.AreEqual("user1", actual.UserName);

            actual = target.ObterPorUserName("user2");
            Assert.AreEqual("user2", actual.UserName);

            actual = target.ObterPorUserName("user3");
            Assert.IsNull(actual);
        }

        [Test]
        public void Remove_Id_Removed()
        {
            var userGateway = MockRepository.GenerateMock<IUsuarioGateway>();
            userGateway.Expect(u => u.Delete(1));

            var target = new UsuarioService(userGateway);
            target.Remover(1);

            userGateway.VerifyAllExpectations();
        }

        [Test]
        public void Pesquisar_Termo_Usuario()
        {
            var usuarios = new Usuario[] { 
                new Usuario { Id = 1 },
                new Usuario { Id = 2 },
            };

            var gateway = MockRepository.GenerateMock<IUsuarioGateway>();
            gateway.Expect(ug => ug.Find(null, (object)null, null)).IgnoreArguments().Return(usuarios);

            var target = new UsuarioService(gateway);

            var result = target.Pesquisar("teste", new Paging());

            Assert.AreEqual(usuarios, result);

            gateway.VerifyAllExpectations();
        }

        [Test]
        public void ObterResumidoPorUserName_UserName_UsuarioResumo()
        {
            var userGateway = new MemoryUsuarioGateway();
            userGateway.Insert(new Usuario { UserName = "user1" });
            userGateway.Insert(new Usuario { UserName = "user2" });

            var target = new UsuarioService(userGateway);

            UsuarioResumo actual = target.ObterResumidoPorUserName("user1");

            Assert.AreEqual("user1", actual.UserName);

            actual = target.ObterResumidoPorUserName("user2");
            Assert.AreEqual("user2", actual.UserName);

            actual = target.ObterResumidoPorUserName("user3");
            Assert.IsNull(actual);
        }

        [Test]
        public void ObterResumidoPorId_Id_UsuarioResumo()
        {
            var userGateway = MockRepository.GenerateMock<IUsuarioGateway>();
            userGateway.Expect(u => u.FindById(1)).Return(new Usuario(1) { FullName = "Foo" });
            userGateway.Expect(u => u.FindById(0)).IgnoreArguments().Constraints(Is.NotEqual(1)).Return(null);

            var target = new UsuarioService(userGateway);

            UsuarioResumo actual = target.ObterResumidoPorId(1);

            Assert.IsNotNull(actual);
            Assert.AreEqual("Foo", actual.FullName);

            actual = target.ObterResumidoPorId(2);
            Assert.IsNull(actual);

            userGateway.VerifyAllExpectations();
        }

        [Test]
        public void PesquisarResumidoPorUsuario_Filtro_UsuarioResumido()
        {
            var usuarios = new UsuarioResumo[] { 
                new UsuarioResumo { Id = 1 },
                new UsuarioResumo { Id = 2 },
            };

            var gateway = MockRepository.GenerateMock<IUsuarioGateway>();
            gateway.Expect(ug => ug.Find<UsuarioResumo>(null, null, (object)null, null)).IgnoreArguments().Return(usuarios);

            var target = new UsuarioService(gateway);

            var result = target.PesquisarResumidoPorUsuario("foo", null, null, null, new Paging());

            Assert.AreEqual(usuarios, result);

            gateway.VerifyAllExpectations();
        }

    }
}
