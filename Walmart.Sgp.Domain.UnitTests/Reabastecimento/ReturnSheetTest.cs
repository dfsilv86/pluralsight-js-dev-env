using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Reabastecimento;

namespace Walmart.Sgp.Domain.UnitTests.Reabastecimento
{
    [TestFixture]
    [Category("Domain"), Category("ReturnSheet")]
    public class ReturnSheetTest
    {
        [Test]
        public void Periodo_ReturnSheet_RetornaCorretamente()
        {
            var inicio = new DateTime(1989, 08, 21).Date;
            var final = inicio.AddDays(4).AddHours(10).AddMinutes(30);

            var rs = new ReturnSheet()
            {
                DhInicioReturn = inicio,
                DhFinalReturn = final
            };

            Assert.AreEqual(@"21/08/1989 - 25/08/1989 10:30:00", rs.Periodo);
        }
    }
}
