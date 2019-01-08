﻿using System;
using NUnit.Framework;
using Walmart.Sgp.Domain.Item.Specs;
using Walmart.Sgp.Infrastructure.Framework.Commons;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.UnitTests.Inventarios.Specs
{
    [TestFixture]
    [Category("Domain")]
    public class IntervaloNaoDeveExcederSessentaDiasSpecTest
    {
        [Test]
        public void IsSatisfiedBy_IntervaloExcedeSessentaDias_False()
        {
            var intervalo = new RangeValue<DateTime>();
            intervalo.StartValue = DateTime.Now;
            intervalo.EndValue = DateTime.Now.AddMonths(5);

            var target = new IntervaloNaoDeveExcederSessentaDiasSpec();
            var actual = target.IsSatisfiedBy(intervalo);

            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.IntervalMustBeLessThanSixtyDays, actual.Reason);
        }

        [Test]
        public void IsSatisfiedBy_IntervaloNaoExcedeSessentaDias_True()
        {
            var intervalo = new RangeValue<DateTime>();
            intervalo.StartValue = DateTime.Now;
            intervalo.EndValue = DateTime.Now.AddMonths(1);

            var target = new IntervaloNaoDeveExcederSessentaDiasSpec();
            var actual = target.IsSatisfiedBy(intervalo);

            Assert.IsTrue(actual.Satisfied);
        }
    }
}