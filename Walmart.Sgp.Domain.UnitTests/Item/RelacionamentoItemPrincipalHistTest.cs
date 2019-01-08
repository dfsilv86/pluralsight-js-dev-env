using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Walmart.Sgp.Domain.Item;

namespace Walmart.Sgp.Domain.UnitTests.Item
{
    [TestFixture]
    [Category("Domain")]
    public class RelacionamentoItemPrincipalHistTest
    {
        [Test]
        public void TipoRelacionamento_Tipo_IDTipoRelacionamentoEstahIgual()
        {
            var target = new RelacionamentoItemPrincipalHist();
            target.TipoRelacionamento = TipoRelacionamento.Manipulado;
            Assert.AreEqual(TipoRelacionamento.Manipulado.Value, target.IDTipoRelacionamento);

            target.TipoRelacionamento = TipoRelacionamento.Receituario;
            Assert.AreEqual(TipoRelacionamento.Receituario.Value, target.IDTipoRelacionamento);

            target.TipoRelacionamento = TipoRelacionamento.Vinculado;
            Assert.AreEqual(TipoRelacionamento.Vinculado.Value, target.IDTipoRelacionamento);
        }

        [Test]
        public void IDTipoRelacionamento_Tipo_TipoRelacionamentoEstahIgual()
        {
            var target = new RelacionamentoItemPrincipalHist();
            target.IDTipoRelacionamento = TipoRelacionamento.Manipulado.Value;
            Assert.AreEqual(TipoRelacionamento.Manipulado, target.TipoRelacionamento);

            target.IDTipoRelacionamento = TipoRelacionamento.Receituario.Value;
            Assert.AreEqual(TipoRelacionamento.Receituario, target.TipoRelacionamento);

            target.IDTipoRelacionamento = TipoRelacionamento.Vinculado.Value;
            Assert.AreEqual(TipoRelacionamento.Vinculado, target.TipoRelacionamento);
        }
    }
}
