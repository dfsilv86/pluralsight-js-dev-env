using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Walmart.Sgp.Domain.Gerenciamento;

namespace Walmart.Sgp.Domain.UnitTests.Gerenciamento
{
    [TestFixture]
    [Category("Domain")]
    public class CodigoVendor9DigitosTest
    {
        [Test]
        public void Constructor_Zero_ZeroAll()
        {
            var target = new CodigoVendor9Digitos(0);
            Assert.AreEqual(0, target.CdV9D);
            Assert.AreEqual(0, target.CdFornecedor);
            Assert.AreEqual(0, target.CdDepartamento);            
            Assert.AreEqual(0, target.CdSequencia);
        }

        [Test]
        public void Constructor_1234_Sequencia4Departamento23Fornecedor1()
        {
            var target = new CodigoVendor9Digitos(1234);
            Assert.AreEqual(1234, target.CdV9D);
            Assert.AreEqual(1, target.CdFornecedor);
            Assert.AreEqual(23, target.CdDepartamento);
            Assert.AreEqual(4, target.CdSequencia);
        }

        [Test]
        public void Constructor_12345_Sequencia5Departamento34Fornecedor12()
        {
            var target = new CodigoVendor9Digitos(12345);
            Assert.AreEqual(12345, target.CdV9D);
            Assert.AreEqual(12, target.CdFornecedor);
            Assert.AreEqual(34, target.CdDepartamento);
            Assert.AreEqual(5, target.CdSequencia);
        }

        [Test]
        public void Constructor_123456_Sequencia6Departamento45Fornecedor123()
        {
            var target = new CodigoVendor9Digitos(123456);
            Assert.AreEqual(123456, target.CdV9D);
            Assert.AreEqual(123, target.CdFornecedor);
            Assert.AreEqual(45, target.CdDepartamento);
            Assert.AreEqual(6, target.CdSequencia);
        }

        [Test]
        public void Constructor_1234567_Sequencia7Departamento56Fornecedor1234()
        {
            var target = new CodigoVendor9Digitos(1234567);
            Assert.AreEqual(1234567, target.CdV9D);
            Assert.AreEqual(1234, target.CdFornecedor);
            Assert.AreEqual(56, target.CdDepartamento);
            Assert.AreEqual(7, target.CdSequencia);
        }

        [Test]
        public void Constructor_12345678_Sequencia8Departamento67Fornecedor12345()
        {
            var target = new CodigoVendor9Digitos(12345678);
            Assert.AreEqual(12345678, target.CdV9D);
            Assert.AreEqual(12345, target.CdFornecedor);
            Assert.AreEqual(67, target.CdDepartamento);
            Assert.AreEqual(8, target.CdSequencia);
        }

        [Test]
        public void Constructor_123456789_Sequencia9Departamento78Fornecedor123456()
        {                                         
            var target = new CodigoVendor9Digitos(123456789);
            Assert.AreEqual(123456789, target.CdV9D);
            Assert.AreEqual(123456, target.CdFornecedor);
            Assert.AreEqual(78, target.CdDepartamento);
            Assert.AreEqual(9, target.CdSequencia);
        }

        [Test]
        public void Constructor_1234567891_Exception()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                new CodigoVendor9Digitos(1234567891);
            });            
        }
    }
}
