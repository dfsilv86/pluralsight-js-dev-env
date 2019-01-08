using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.MultisourcingVendor;
using Walmart.Sgp.Domain.MultisourcingVendor.Specs;

namespace Walmart.Sgp.Domain.UnitTests.MultisourcingVendor.Specs
{
    [TestFixture]
    [Category("Domain"), Category("Multisourcing")]
    public class MultisourcingPossuiCanalValidoSpecTest
    {
        [Test]
        public void IsSatisfiedBy_CanalFornecedorTipoD_True()
        {
            var multisourcing = new Multisourcing
            {
                Fornecedor = new Fornecedor
                {
                    Parametros = new List<FornecedorParametro> 
                    {  
                        new FornecedorParametro
                        {
                            cdTipo = TipoCodigoReabastecimento.Dao
                        }
                    }
                }
            };

            var target = new MultisourcingPossuiCanalValidoSpec();
            var actual = target.IsSatisfiedBy(new[] { multisourcing });

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_CanalFornecedorTipoL_True()
        {
            var multisourcing = new Multisourcing
            {
                Fornecedor = new Fornecedor
                {
                    Parametros = new List<FornecedorParametro> 
                    {  
                        new FornecedorParametro
                        {
                            cdTipo = TipoCodigoReabastecimento.All
                        }
                    }
                }
            };

            var target = new MultisourcingPossuiCanalValidoSpec();
            var actual = target.IsSatisfiedBy(new[] { multisourcing });

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_CanalFornecedorNaoValido_False()
        {
            var multisourcing = new Multisourcing
            {
                Fornecedor = new Fornecedor
                {
                    Parametros = new List<FornecedorParametro> 
                    {  
                        new FornecedorParametro
                        {
                            cdTipo = TipoCodigoReabastecimento.Wog
                        }
                    }
                }
            };

            var target = new MultisourcingPossuiCanalValidoSpec();
            var actual = target.IsSatisfiedBy(new[] { multisourcing });

            Assert.False(actual.Satisfied);
        }
    }
}
