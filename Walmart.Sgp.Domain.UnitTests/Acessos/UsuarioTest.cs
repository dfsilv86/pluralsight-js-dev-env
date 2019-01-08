using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Walmart.Sgp.Domain.Acessos;

namespace Walmart.Sgp.Domain.UnitTests.Acessos
{
    [TestFixture]
    [Category("Domain")]
    public class UsuarioTest
    {
        [Test]
        public void Constructor_WithId_IdDefined()
        {
            var target = new Usuario(1);
            Assert.AreEqual(1, target.Id);
        }

        [Test]
        public void DiffOperator_Instances_Compared()
        {
            var user1 = new Usuario(1);
            var user1Clone = new Usuario(1);
            var user2 = new Usuario(2);

            Assert.IsTrue(user1 != user2);
            Assert.IsFalse(user1 != user1Clone);
        }

        [Test]
        public void Equals_NotUser_False()
        {
            var user1 = new Usuario(1);

            Assert.IsFalse(user1.Equals(new Random()));
        }

        [Test]
        public void GetHashCode_DiffIds_DiffResults()
        {
            var user1 = new Usuario(1);
            var user2 = new Usuario(2);

            Assert.AreNotEqual(user1.GetHashCode(), user2.GetHashCode());
        }

        [Test]
        public void IdApplication_Get_Default()
        {
            var target = new Usuario();
            Assert.AreEqual(1, target.IdApplication);
        }
    }
}
