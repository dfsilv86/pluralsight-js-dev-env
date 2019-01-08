using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Processing;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Processing
{
    [Category("Framework")]
    [Category("Processing")]
    [TestFixture]
    public class ProcessingContextTest
    {
        [Test]
        public void Current_Default_IsDummy()
        {
            Assert.AreEqual(ProcessingContext.Current, ProcessingContext.Dummy);
        }

        [Test]
        public void Current_MemoryProcessingContext_Ok()
        {
            bool hasUpdate = false;

            MemoryProcessingContext mpc = new MemoryProcessingContext((a, b, c) =>
            {
                hasUpdate = true;
            });

            try
            {
                ProcessingContext.Current = mpc;

                Assert.AreEqual(ProcessingContext.Current, mpc);

                ProcessingContext.Current.SetProgress(1, 2, "3");
            }
            finally
            {
                ProcessingContext.Current = ProcessingContext.Dummy;
            }
            Assert.IsTrue(hasUpdate);
        }
    }
}
