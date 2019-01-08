using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Helpers;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Helpers
{
    [TestFixture]
    [Category("Framework")]
    public class ExceptionHelperTest
    {
        [Test]
        public void ExceptionHelper_Throws()
        {
            Assert.Throws(Is.TypeOf<ArgumentNullException>(), () =>
            {
                ExceptionHelper.ThrowIfNull("test", null);
            });

            Assert.DoesNotThrow(() =>
            {
                ExceptionHelper.ThrowIfNull("test", "value");
            });
        }

        [Test]
        public void AnyNull_AllPropertiesNotNull_NoException()
        {
            ExceptionHelper.ThrowIfAnyNull(new { a = 1, b = "2", c = 3L });
        }

        [Test]
        public void AnyNull_AnyPropertieNull_Exception()
        {
            Assert.Catch<ArgumentNullException>(() =>
            {
                ExceptionHelper.ThrowIfAnyNull(null);
            });

            Assert.Catch<ArgumentNullException>(() =>
            {
                ExceptionHelper.ThrowIfAnyNull(new { a = (string)null, b = "2", c = 3L });
            });

            Assert.Catch<ArgumentNullException>(() =>
            {
                ExceptionHelper.ThrowIfAnyNull(new { a = 1, b = (object)null, c = 3L });
            });

            Assert.Catch<ArgumentNullException>(() =>
            {
                ExceptionHelper.ThrowIfAnyNull(new { a = 1, b = "2", c = (string)null });
            });
        }
    }
}
