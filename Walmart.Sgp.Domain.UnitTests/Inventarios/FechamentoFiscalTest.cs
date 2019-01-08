using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Walmart.Sgp.Domain.Inventarios;

namespace Walmart.Sgp.Domain.UnitTests.Inventarios
{
    [TestFixture]
    [Category("Domain")]
    public class FechamentoFiscalTest
    {
        [Test]
        public void ObterMesAberto_NrMesOuNrAnoNulos_Null()
        {
            var target = new FechamentoFiscal();
            
            target.nrAno = null;
            Assert.IsFalse(target.ObterMesAberto().HasValue);

            target.nrAno = 2016;
            Assert.IsFalse(target.ObterMesAberto().HasValue);
        }

        [Test]
        public void ObterMesAberto_NrMesENrAno_UltimoDiaDoMes()
        {
            var target = new FechamentoFiscal();
            target.nrAno = 2016;
            target.nrMes = 5;

            var actual = target.ObterMesAberto().Value;

            Assert.AreEqual(2016, actual.Year);
            Assert.AreEqual(5, actual.Month);
            Assert.AreEqual(31, actual.Day);
            Assert.AreEqual(23, actual.Hour);
            Assert.AreEqual(59, actual.Minute);
            Assert.AreEqual(59, actual.Second);
            Assert.AreEqual(999, actual.Millisecond);
        }
    }
}
