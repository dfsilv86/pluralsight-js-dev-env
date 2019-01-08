using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.MultisourcingVendor;

namespace Walmart.Sgp.Domain.UnitTests.MultisourcingVendor
{
    [TestFixture]
    [Category("Domain"), Category("MultiSourcing")]
    public class MultisourcingEqualityComparerTest
    {
        [Test]
        public void Equals_MultisourcingsIguais_True()
        {
            var multisourcingX = new Multisourcing 
            { 
                IDRelacionamentoItemSecundario = 10,
                IDCD = 100
            };

            var multisourcingY = new Multisourcing
            {
                IDRelacionamentoItemSecundario = 10,
                IDCD = 100
            };

            var target = new MultisourcingEqualityComparer();
            var actual = target.Equals(multisourcingX, multisourcingY);

            Assert.IsTrue(actual);
        }

        [Test]
        public void Equals_MultisourcingsCDDiferente_False()
        {
            var multisourcingX = new Multisourcing
            {
                IDRelacionamentoItemSecundario = 10,
                IDCD = 100
            };

            var multisourcingY = new Multisourcing
            {
                IDRelacionamentoItemSecundario = 10,
                IDCD = 99
            };

            var target = new MultisourcingEqualityComparer();
            var actual = target.Equals(multisourcingX, multisourcingY);

            Assert.IsFalse(actual);
        }

        [Test]
        public void Equals_MultisourcingsRelacionamentoItemSecundarioDiferente_False()
        {
            var multisourcingX = new Multisourcing
            {
                IDRelacionamentoItemSecundario = 10,
                IDCD = 100
            };

            var multisourcingY = new Multisourcing
            {
                IDRelacionamentoItemSecundario = 11,
                IDCD = 100
            };

            var target = new MultisourcingEqualityComparer();
            var actual = target.Equals(multisourcingX, multisourcingY);

            Assert.IsFalse(actual);
        }
    }
}
