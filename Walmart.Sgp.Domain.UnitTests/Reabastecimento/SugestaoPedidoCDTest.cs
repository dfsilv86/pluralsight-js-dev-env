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
    [Category("Domain")]
    public class SugestaoPedidoCDTest
    {
        [Test]
        public void ForecastMedio_SugestaoPedidoCDComDados_MediaDoForecastE2()
        {
            var model = new SugestaoPedidoCD();
            model.dtInicioForecast = new DateTime(2016, 01, 01);
            model.dtFimForecast = new DateTime(2016, 01, 05);
            model.qtdForecast = 10;

            Assert.AreEqual(2, model.ForecastMedio);
        }

        [Test]
        public void ProximoReviewDate_SugestaoPedidoCDComCdReviewDate35Em26042016_ProximoReviewDateEEm28042016()
        {
            var today = new DateTime(2016, 4, 26);
            
            var model = new SugestaoPedidoCD(today)
            {
                cdReviewDate = 35
            };

            var expected = new DateTime(2016, 4, 28);

            Assert.AreEqual(expected, model.ProximoReviewDate);
        }

        [Test]
        public void ProximoReviewDate_SugestaoPedidoCDComCdReviewDate2Em26042016_ProximoReviewDateEEm02052016()
        {
            var today = new DateTime(2016, 4, 26);

            var model = new SugestaoPedidoCD(today)
            {
                cdReviewDate = 2
            };

            var expected = new DateTime(2016, 5, 2);

            Assert.AreEqual(expected, model.ProximoReviewDate);
        }

        [Test]
        public void ProximoReviewDate_SugestaoPedidoCDComCdReviewDate123Em28042016_ProximoReviewDateEEm01052016()
        {
            var today = new DateTime(2016, 4, 28);

            var model = new SugestaoPedidoCD(today)
            {
                cdReviewDate = 123
            };

            var expected = new DateTime(2016, 5, 1);

            Assert.AreEqual(expected, model.ProximoReviewDate);
        }

        [Test]
        public void ProximoReviewDate_SugestaoPedidoCDComCdReviewDate4Em27042016_ProximoReviewDateEEm04052016()
        {
            var today = new DateTime(2016, 4, 27);

            var model = new SugestaoPedidoCD(today)
            {
                cdReviewDate = 4
            };

            var expected = new DateTime(2016, 5, 4);

            Assert.AreEqual(expected, model.ProximoReviewDate);
        }

        [Test]
        public void ProximoReviewDate_SugestaoPedidoCDComCdReviewDate0_NãoTemProximoReviewDate()
        {
            var today = new DateTime(2016, 4, 27);

            var model = new SugestaoPedidoCD(today)
            {
                cdReviewDate = 0
            };

            Assert.AreEqual(null, model.ProximoReviewDate);
        }
    }
}