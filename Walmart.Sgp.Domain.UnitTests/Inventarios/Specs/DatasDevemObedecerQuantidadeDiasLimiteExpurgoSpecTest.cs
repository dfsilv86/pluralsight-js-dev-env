﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Rhino.Mocks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.Acessos.Specs;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Domain.Item.Specs;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.Inventarios.Specs;
using Walmart.Sgp.Infrastructure.Framework.Commons;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Domain.Gerenciamento;

namespace Walmart.Sgp.Domain.UnitTests.Inventarios.Specs
{
    [TestFixture]
    [Category("Domain")]
    public class DatasDevemObedecerQuantidadeDiasLimiteExpurgoSpecTest
    {
        [Test]
        public void IsSatisfiedBy_DatasNaoObedecemQuantidadeDiasLimiteExpurgo_False()
        {
            var datas = new RangeValue<DateTime>();
            datas.StartValue = DateTime.UtcNow.AddMonths(-5);
            datas.EndValue = DateTime.UtcNow.AddMonths(-5);

            var parametroSistemaService = MockRepository.GenerateMock<IParametroSistemaService>();

            parametroSistemaService.Expect(o => o.ObterPorNome(null)).IgnoreArguments().Return(new ParametroSistema
            {
                vlParametroSistema = "3"
            });

            var target = new DatasDevemObedecerQuantidadeDiasLimiteExpurgoSpec(parametroSistemaService);
            var actual = target.IsSatisfiedBy(datas);

            parametroSistemaService.VerifyAllExpectations();

            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.DatesMustObayPurgeLimit, actual.Reason);
        }

        [Test]
        public void IsSatisfiedBy_DatasObedecemQuantidadeDiasLimiteExpurgo_True()
        {
            var datas = new RangeValue<DateTime>();
            datas.StartValue = DateTime.UtcNow;
            datas.EndValue = DateTime.UtcNow;

            var parametroSistemaService = MockRepository.GenerateMock<IParametroSistemaService>();

            parametroSistemaService.Expect(o => o.ObterPorNome(null)).IgnoreArguments().Return(new ParametroSistema
            {
                vlParametroSistema = "3"
            });

            var target = new DatasDevemObedecerQuantidadeDiasLimiteExpurgoSpec(parametroSistemaService);
            var actual = target.IsSatisfiedBy(datas);

            parametroSistemaService.VerifyAllExpectations();

            Assert.IsTrue(actual.Satisfied);
        }
    }
}