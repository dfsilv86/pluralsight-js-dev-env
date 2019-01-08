using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Domain
{
    [TestFixture]
    [Category("Audit")]
    public class AuditKindTest
    {
        [Test]
        public void AuditKind_ImplicitOp_Throws()
        {
            Assert.Throws<InvalidCastException>(() =>
            {
                var x = (AuditKind)10000;
            });
        }
    }
}
