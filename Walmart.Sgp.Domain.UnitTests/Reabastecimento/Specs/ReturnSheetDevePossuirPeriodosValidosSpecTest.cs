using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Domain.Reabastecimento.Specs;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.UnitTests.Reabastecimento.Specs
{
    [TestFixture]
    [Category("Domain"), Category("Reabastecimento")]
    public class ReturnSheetDevePossuirPeriodosValidosSpecTest
    {
        [Test]
        public void IsSatisfiedBy_ReturnSheetComInicioAmanha_Satisfied()
        {
            var amanha = DateTime.Now.AddDays(1);

            var target = new ReturnSheetDevePossuirPeriodosValidosSpec();
            var actual = target.IsSatisfiedBy(new ReturnSheet()
            {
                DhInicioReturn = amanha,
                DhFinalReturn = amanha.AddDays(1),
                DhInicioEvento = amanha.AddDays(2),
                DhFinalEvento = amanha.AddDays(4)
            });

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_ReturnSheetComDataInicioPosteriorADataFim_NotSatisfied()
        {
            var amanha = DateTime.Now.AddDays(1);

            var target = new ReturnSheetDevePossuirPeriodosValidosSpec();
            var actual = target.IsSatisfiedBy(new ReturnSheet()
            {
                DhInicioReturn = amanha.AddDays(1),
                DhFinalReturn = amanha,
                DhInicioEvento = amanha.AddDays(2),
                DhFinalEvento = amanha.AddDays(4)
            });

            Assert.IsFalse(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_ReturnSheetComDataInicioPosteriorADataFim_MensagemDoMotivoEstaCorreta()
        {
            var amanha = DateTime.Now.AddDays(1);

            var target = new ReturnSheetDevePossuirPeriodosValidosSpec();
            var actual = target.IsSatisfiedBy(new ReturnSheet()
            {
                DhInicioReturn = amanha.AddDays(1),
                DhFinalReturn = amanha,
                DhInicioEvento = amanha.AddDays(2),
                DhFinalEvento = amanha.AddDays(4)
            });

            Assert.AreEqual(Texts.RSStartDateCantBeBiggerThanFinal, actual.Reason);
        }

        [Test]
        public void IsSatisfiedBy_ReturnSheetComInicioEventoPosteriorAoInicioDaReturn_NotSatisfied()
        {
            var amanha = DateTime.Now.AddDays(1);

            var target = new ReturnSheetDevePossuirPeriodosValidosSpec();
            var actual = target.IsSatisfiedBy(new ReturnSheet()
            {
                DhInicioReturn = amanha,
                DhFinalReturn = amanha.AddDays(1),
                DhInicioEvento = amanha.AddDays(4),
                DhFinalEvento = amanha.AddDays(2)
            });

            Assert.IsFalse(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_ReturnSheetComInicioEventoPosteriorAoInicioDaReturn_MensagemDoMotivoEstaCorreta()
        {
            var amanha = DateTime.Now.AddDays(1);

            var target = new ReturnSheetDevePossuirPeriodosValidosSpec();
            var actual = target.IsSatisfiedBy(new ReturnSheet()
            {
                DhInicioReturn = amanha,
                DhFinalReturn = amanha.AddDays(1),
                DhInicioEvento = amanha.AddDays(4),
                DhFinalEvento = amanha.AddDays(2)
            });

            Assert.AreEqual(Texts.EventStartDateCantBeBiggerThanFinal, actual.Reason);
        }

        [Test]
        public void IsSatisfiedBy_ReturnSheetComFinalPosteriorAoInicioDoEvento_NotSatisfied()
        {
            var amanha = DateTime.Now.AddDays(1);

            var target = new ReturnSheetDevePossuirPeriodosValidosSpec();
            var actual = target.IsSatisfiedBy(new ReturnSheet()
            {
                DhInicioReturn = amanha,
                DhFinalReturn = amanha.AddDays(2),
                DhInicioEvento = amanha.AddDays(1),
                DhFinalEvento = amanha.AddDays(4)
            });

            Assert.IsFalse(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_ReturnSheetComFinalPosteriorAoInicioDoEvento_MensagemDoMotivoEstaCorreta()
        {
            var amanha = DateTime.Now.AddDays(1);

            var target = new ReturnSheetDevePossuirPeriodosValidosSpec();
            var actual = target.IsSatisfiedBy(new ReturnSheet()
            {
                DhInicioReturn = amanha,
                DhFinalReturn = amanha.AddDays(2),
                DhInicioEvento = amanha.AddDays(1),
                DhFinalEvento = amanha.AddDays(4)
            });

            Assert.AreEqual(Texts.RSFinalDateCantBeBiggerThanEventStart, actual.Reason);
        }
    }
}
