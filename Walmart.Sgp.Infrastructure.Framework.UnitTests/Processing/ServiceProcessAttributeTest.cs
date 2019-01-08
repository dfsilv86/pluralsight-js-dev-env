using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Processing;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Processing
{
    [TestFixture]
    [Category("Framework")]
    [Category("Processing")]
    public class ServiceProcessAttributeTest
    {
        [Test]
        public void Constructor_Arguments_Ok()
        {
            ServiceProcessAttribute target = new ServiceProcessAttribute("Xyz") { MaxGlobal = 2, MaxPerUser = 3, EnableQueueing = true };

            Assert.AreEqual("Xyz", target.ProcessName);
            Assert.AreEqual(2, target.MaxGlobal);
            Assert.AreEqual(3, target.MaxPerUser);
            Assert.AreEqual(true, target.EnableQueueing);
        }
    }
}
