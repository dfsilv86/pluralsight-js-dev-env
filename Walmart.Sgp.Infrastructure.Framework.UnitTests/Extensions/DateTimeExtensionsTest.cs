using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Extensions;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Extensions
{
    [TestFixture]
    [Category("Framework")]
    public class DateTimeExtensionsTest
    {
        [Test]
        public void ToMilitaryTime_Horario_Ok()
        {
            DateTime dateTime = new DateTime(2008, 04, 03, 01, 55, 20);

            Assert.AreEqual(155, dateTime.ToMilitaryTime());

            dateTime = new DateTime(2017, 06, 21, 23, 12, 45);

            Assert.AreEqual(2312, dateTime.ToMilitaryTime());
        }

        [Test]
        public void ToLastDayTime_DateTime_LastDayTime()
        {
            var target = new DateTime(2016, 5, 18, 7, 39, 25);
            var actual = target.ToLastDayTime();
            Assert.AreEqual(2016, actual.Year);
            Assert.AreEqual(5, actual.Month);
            Assert.AreEqual(18, actual.Day);
            Assert.AreEqual(23, actual.Hour);
            Assert.AreEqual(59, actual.Minute);
            Assert.AreEqual(59, actual.Second);
            Assert.AreEqual(999, actual.Millisecond);
        }

        [Test]
        public void ToLastMonthTime_DateTime_LastMonthTime()
        {
            var target = new DateTime(2016, 5, 18, 7, 39, 25);
            var actual = target.ToLastMonthTime();
            Assert.AreEqual(2016, actual.Year);
            Assert.AreEqual(5, actual.Month);
            Assert.AreEqual(31, actual.Day);
            Assert.AreEqual(23, actual.Hour);
            Assert.AreEqual(59, actual.Minute);
            Assert.AreEqual(59, actual.Second);
            Assert.AreEqual(999, actual.Millisecond);

            target = new DateTime(2017, 2, 1);
            actual = target.ToLastMonthTime();
            Assert.AreEqual(2017, actual.Year);
            Assert.AreEqual(2, actual.Month);
            Assert.AreEqual(28, actual.Day);
            Assert.AreEqual(23, actual.Hour);
            Assert.AreEqual(59, actual.Minute);
            Assert.AreEqual(59, actual.Second);
            Assert.AreEqual(999, actual.Millisecond);
        }

        [Test]
        public void StringToDate_Ok()
        {
            var str = "1989-08-21";
            var dt = str.ToDate();

            Assert.AreEqual(1989, dt.Year);
            Assert.AreEqual(21, dt.Day);
            Assert.AreEqual(08, dt.Month);
        }

        [Test]
        public void StringToDate_Exception()
        {
            var str = "21-08-1989";

            Assert.Catch(typeof(FormatException), new TestDelegate(() => { var x = str.ToDate(); }));
        }
    }
}
