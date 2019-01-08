using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Extensions;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Extensions
{
    [Category("Framework")]
    [TestFixture]
    public class StringExtensionsTest
    {
        [Test]
        public void LimitTo_Null_Null()
        {
            Assert.IsNull(((string)null).LimitTo(10));
        }

        [Test]
        public void LimitTo_LimiteMaiorQueMensagem_Mensagem()
        {
            var target = "mensagem mensagem";

            var result = target.LimitTo(target.Length + 10);

            Assert.AreEqual(target, result);
        }

        [Test]
        public void LimitTo_LimiteMenorQueMensagemMaiorQue6_MensagemTruncadaComReticencias()
        {
            var target = "mensagem mensagem";

            var result = target.LimitTo(10);

            Assert.AreEqual("mensage...", result);
        }

        [Test]
        public void LimitTo_LimiteMenorQueMensagemMenorQue6_MensagemTruncadaSemReticencias()
        {
            var target = "mensagem mensagem";

            var result = target.LimitTo(5);

            Assert.AreEqual("mensa", result);
        }
    }
}
