using NUnit.Framework;
using Walmart.Sgp.Domain.Acessos;

namespace Walmart.Sgp.Domain.UnitTests.Acessos
{
    [TestFixture]
    [Category("Domain")]
    public class AlcadaTest
    {
        [Test]
        public void GarantirIntegridade_AlcadaNaoIntegra_AlcadaIntegra()
        {
            var target = new Alcada
            {
                blAlterarSugestao = true,
                blAlterarInformacaoEstoque = true,
                blAlterarPercentual = true,
                blZerarItem = true,
                vlPercentualAlterado = 20                
            };

            target.blAlterarSugestao = false;
            
            target.GarantirIntegridade();

            Assert.IsFalse(target.blAlterarInformacaoEstoque);
            Assert.IsFalse(target.blAlterarPercentual);
            Assert.IsTrue(target.blZerarItem);
            Assert.AreEqual(null, target.vlPercentualAlterado);
        }
    }
}