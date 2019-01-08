using NUnit.Framework;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Domain.Reabastecimento.Specs;

namespace Walmart.Sgp.Domain.UnitTests.Reabastecimento.Specs
{
    [TestFixture]
    [Category("Domain"), Category("Reabastecimento")]
    public class PrecoVendaDeveSerInformadoSpecTest
    {
        [Test]
        public void IsSatisfiedBy_TodasLojasSelecionadasPossuemPrecoVenda_True()
        {
            var lojas = new []
            {
                new ReturnSheetItemLoja 
                {
                    selecionado = true,
                    PrecoVenda = 100.00M
                },

                new ReturnSheetItemLoja 
                {
                    selecionado = true,
                    PrecoVenda = 200.00M
                },
            };

            var target = new PrecoVendaDeveSerInformadoSpec();
            var actual = target.IsSatisfiedBy(lojas);

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_NemTodasLojasSelecionadasPossuemPrecoVenda1_False()
        {
            var lojas = new[]
            {
                new ReturnSheetItemLoja 
                {
                    selecionado = true,
                    PrecoVenda = 100.00M
                },

                new ReturnSheetItemLoja 
                {
                    selecionado = true,
                    PrecoVenda = null
                }
            };

            var target = new PrecoVendaDeveSerInformadoSpec();
            var actual = target.IsSatisfiedBy(lojas);

            Assert.IsFalse(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_NemTodasLojasSelecionadasPossuemPrecoVenda2_False()
        {
            var lojas = new[]
            {
                new ReturnSheetItemLoja 
                {
                    selecionado = true,
                    PrecoVenda = 100.00M
                },

                new ReturnSheetItemLoja 
                {
                    selecionado = true,
                    PrecoVenda = 0
                }
            };

            var target = new PrecoVendaDeveSerInformadoSpec();
            var actual = target.IsSatisfiedBy(lojas);

            Assert.IsFalse(actual.Satisfied);
        }
    }
}