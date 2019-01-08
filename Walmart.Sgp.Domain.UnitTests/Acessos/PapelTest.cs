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
    public class PapelTest
    {      
        [Test]
        public void IdApplication_Get_Default()
        {
            var target = new Papel();
            Assert.AreEqual(1, target.IdApplication);
        }
    }
}
