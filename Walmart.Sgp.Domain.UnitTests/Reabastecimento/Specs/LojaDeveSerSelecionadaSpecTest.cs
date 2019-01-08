using NUnit.Framework;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Domain.Reabastecimento.Specs;

namespace Walmart.Sgp.Domain.UnitTests.Reabastecimento.Specs
{
    [TestFixture]
    [Category("Domain"), Category("Reabastecimento")]
    public class LojaDeveSerSelecionadaSpecTest
    {
        [Test]
        public void IsSatisfiedBy_AlgumaLojaSelecionada_True()
        {
            var lojasAlteradas = new[]
            {
                new ReturnSheetItemLoja 
                {
                    IdItemDetalheEntrada = 1,
                    IdLoja = 1,
                    selecionado = true
                }
            };

            var lojasPersistidas = new[]
            {
                new ReturnSheetItemLoja 
                {
                    IdItemDetalheEntrada = 2,
                    IdLoja = 1,
                    blAtivo = true
                }
            };

            var target = new LojaDeveSerSelecionadaSpec();
            var actual = target.IsSatisfiedBy(new ReturnSheetItemLojaSpecParameter
            {
                LojasAlteradas = lojasAlteradas,
                LojasPersistidas = lojasPersistidas
            });

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_NenhumaLojaSelecionada_False()
        {
            var lojasAlteradas = new[]
            {
                new ReturnSheetItemLoja 
                {
                    IdItemDetalheEntrada = 1,
                    IdLoja = 1,
                    selecionado = false
                }
            };

            var lojasPersistidas = new[]
            {
                new ReturnSheetItemLoja 
                {
                    IdItemDetalheEntrada = 2,
                    IdLoja = 1,
                    blAtivo = false
                }
            };

            var target = new LojaDeveSerSelecionadaSpec();
            var actual = target.IsSatisfiedBy(new ReturnSheetItemLojaSpecParameter
            {
                LojasAlteradas = lojasAlteradas,
                LojasPersistidas = lojasPersistidas
            });

            Assert.IsFalse(actual.Satisfied);
        }
    }
}
