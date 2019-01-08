using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Item;

namespace Walmart.Sgp.Domain.UnitTests.Item
{
    [TestFixture]
    [Category("Domain")]
    public class ItemFactoryTest
    {      
        [Test]
        public void CreateTipoRelacionamentoFactory_Tipo_Strategy()
        {
            var actual = ItemFactory.CreateTipoRelacionamentoStrategy(new RelacionamentoItemPrincipal { TipoRelacionamento = TipoRelacionamento.Manipulado }, null);
            Assert.IsInstanceOf<ManipuladoTipoRelacionamentoStrategy>(actual);

            actual = ItemFactory.CreateTipoRelacionamentoStrategy(new RelacionamentoItemPrincipal { TipoRelacionamento = TipoRelacionamento.Receituario }, null);
            Assert.IsInstanceOf<ReceituarioTipoRelacionamentoStrategy>(actual);

            actual = ItemFactory.CreateTipoRelacionamentoStrategy(new RelacionamentoItemPrincipal { TipoRelacionamento = TipoRelacionamento.Vinculado }, null);
            Assert.IsInstanceOf<VinculadoTipoRelacionamentoStrategy>(actual);
        }
    }
}
