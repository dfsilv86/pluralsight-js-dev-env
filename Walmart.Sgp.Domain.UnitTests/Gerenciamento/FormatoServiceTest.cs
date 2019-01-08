using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Gerenciamento;

namespace Walmart.Sgp.Domain.UnitTests.Gerenciamento
{
    [TestFixture]
    [Category("Domain")]
    public class FormatoServiceTest
    {
        [Test]
        public void ObterPorSistema_CodigoSistema_Formatos()
        {
            Formato[] formatos = new Formato[] { new Formato { Id = 1 }, new Formato { Id = 2 } };

            var gateway = MockRepository.GenerateMock<IFormatoGateway>();
            gateway.Expect(fs => fs.Find(null, null)).IgnoreArguments().Return(formatos);

            var target = new FormatoService(gateway);

            var result = target.ObterPorSistema(1);

            Assert.AreEqual(formatos, result);
            gateway.VerifyAllExpectations();
        }
    }
}
