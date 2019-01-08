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
    public class ExposedParameterAttributeTest
    {
        [Test]
        public void ExposedParameterAttribute_Default_HasDefault()
        {
            var target = ExposedParameterAttribute.Default;

            Assert.IsNotNull(target);
        }

        [Test]
        public void Constructor_Values_ExposedParameterAttribute()
        {
            var target = new ExposedParameterAttribute(true);

            Assert.IsTrue(target.IsExposed);
        }
    }
}
