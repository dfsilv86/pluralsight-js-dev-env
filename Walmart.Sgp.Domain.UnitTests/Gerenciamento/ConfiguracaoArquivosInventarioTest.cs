using NUnit.Framework;
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
    public class ConfiguracaoArquivosInventarioTest
    {
        [Test]
        public void ConfiguracaoArquivosInventario_ReadOnlyProperties_Ok()
        {
            var target = new ConfiguracaoArquivosInventario();

            Assert.IsFalse(string.IsNullOrWhiteSpace(target.DiretorioDescompactados));
            Assert.IsFalse(string.IsNullOrWhiteSpace(target.DiretorioBackup));
            Assert.IsFalse(string.IsNullOrWhiteSpace(target.ArquivoPipeVarejo));
            Assert.IsFalse(string.IsNullOrWhiteSpace(target.ArquivoPipeAtacado));
        }
    }
}
