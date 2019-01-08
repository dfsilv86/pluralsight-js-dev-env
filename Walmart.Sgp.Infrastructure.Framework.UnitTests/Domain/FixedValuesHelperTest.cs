using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Domain
{
    [TestFixture]
    [Category("Framework")]
    public class FixedValuesHelperTest
    {
        [Test]
        public void GetConversionOperator_Type_MethodInfo()
        {
            var actual = FixedValuesHelper.GetConversionOperator(typeof(int));
            Assert.IsNull(actual);

            actual = FixedValuesHelper.GetConversionOperator(typeof(TipoReabastecimento));
            Assert.AreEqual(TipoReabastecimento.Cross, actual.Invoke(null, new object[] { "C" }));

            actual = FixedValuesHelper.GetConversionOperator(typeof(TipoItemEntrada));
            Assert.AreEqual(TipoItemEntrada.Embalagem, actual.Invoke(null, new object[] { 1 }));
        }
    }
}
