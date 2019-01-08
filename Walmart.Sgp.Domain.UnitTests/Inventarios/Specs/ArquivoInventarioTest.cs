using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Inventarios;

namespace Walmart.Sgp.Domain.UnitTests.Inventarios.Specs
{
    [TestFixture]
    public class ArquivoInventarioTest
    {
        [Test]
        [Category("Domain")]
        public void ArquivoInventario_Constructor_ArquivoInventario()
        {
            ArquivoInventario target = new ArquivoInventario(1, "2", new DateTime(2015, 03, 06));

            Assert.AreEqual(1, target.IdLojaImportacao);
            Assert.IsNull(target.CdLoja);
            Assert.AreEqual("2", target.NomeArquivo);
            Assert.AreEqual(new DateTime(2015, 03, 06), target.DataInventario);
            Assert.IsNull(target.DataArquivo);
            Assert.IsNull(target.UltimoCdDepartamentoLido);
            Assert.AreEqual(TipoArquivoInventario.Nenhum, target.TipoArquivo);
            Assert.IsNotNull(target.Itens);
            Assert.AreEqual(0, target.Itens.Count());
        }
    }
}
