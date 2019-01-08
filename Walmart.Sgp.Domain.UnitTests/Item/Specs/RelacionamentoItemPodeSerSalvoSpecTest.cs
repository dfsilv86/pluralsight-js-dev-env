using System;
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

namespace Walmart.Sgp.Domain.UnitTests.Item.Specs
{
    [TestFixture]
    [Category("Domain")]
    public class RelacionamentoItemPodeSerSalvoSpecTest
    {
        [Test]
        public void IsSatisfiedBy_SemItensSecundarios_False()
        {
            var target = new RelacionamentoItemPodeSerSalvoSpec();
            var actual = target.IsSatisfiedBy(new RelacionamentoItemPrincipal
            {
                RelacionamentoSecundario = null,
                TipoRelacionamento = TipoRelacionamento.Manipulado
            });
            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.DefineAtLeastOneExitItem.With(Texts.ExitItem), actual.Reason);

            actual = target.IsSatisfiedBy(new RelacionamentoItemPrincipal
            {
                RelacionamentoSecundario = new List<RelacionamentoItemSecundario>(),
                TipoRelacionamento = TipoRelacionamento.Receituario
            });
            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.DefineAtLeastOneExitItem.With(Texts.InputItem), actual.Reason);
        }

        [Test]
        public void IsSatisfiedBy_EmProcessamento_False()
        {
            var target = new RelacionamentoItemPodeSerSalvoSpec();
            var actual = target.IsSatisfiedBy(new RelacionamentoItemPrincipal
            {
                StatusReprocessamentoCusto = StatusReprocessamentoCusto.Processando,
                RelacionamentoSecundario = new RelacionamentoItemSecundario[] { 
                    new RelacionamentoItemSecundario()
                }
            });
            
            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.RelacionamentoItemPrincipalCanBeSavedIsProcessing, actual.Reason);
        }

        [Test]
        public void IsSatisfiedBy_TemItensSecundariosENaoEstaEmProcessamento_True()
        {
            var target = new RelacionamentoItemPodeSerSalvoSpec();
            var actual = target.IsSatisfiedBy(new RelacionamentoItemPrincipal
            {
                StatusReprocessamentoCusto = StatusReprocessamentoCusto.Concluido,
                RelacionamentoSecundario = new RelacionamentoItemSecundario[] { 
                    new RelacionamentoItemSecundario()
                }
            });
            Assert.IsTrue(actual.Satisfied);
        }
    }
}
