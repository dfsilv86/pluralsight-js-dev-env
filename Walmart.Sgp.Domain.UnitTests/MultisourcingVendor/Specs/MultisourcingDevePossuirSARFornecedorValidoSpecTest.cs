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
    public class MultisourcingDevePossuirSARFornecedorValidoSpecTest
    {
        [Test]
        public void IsSatisfiedBy_SARFornecedorValido1_True()
        {
            var multisourcing = new Multisourcing
            {
                Fornecedor = new Fornecedor
                {
                    Parametros = new List<FornecedorParametro> 
                    {  
                        new FornecedorParametro
                        {
                            tpStoreApprovalRequired = TipoSAR.Yes
                        }
                    }
                }
            };

            var target = new MultisourcingDevePossuirSARFornecedorValidoSpec();
            var actual = target.IsSatisfiedBy(new[] { multisourcing });

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_SARFornecedorValido2_True()
        {
            var multisourcing = new Multisourcing
            {
                Fornecedor = new Fornecedor
                {
                    Parametros = new List<FornecedorParametro> 
                    {  
                        new FornecedorParametro
                        {
                            tpStoreApprovalRequired = TipoSAR.Required
                        }
                    }
                }
            };

            var target = new MultisourcingDevePossuirSARFornecedorValidoSpec();
            var actual = target.IsSatisfiedBy(new[] { multisourcing });

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_SARFornecedorNaoEValido_False()
        {
            var multisourcing = new Multisourcing
            {
                Fornecedor = new Fornecedor
                {
                    Parametros = new List<FornecedorParametro> 
                    {  
                        new FornecedorParametro
                        {
                            tpStoreApprovalRequired = TipoSAR.Manual
                        }
                    }
                }
            };

            var target = new MultisourcingDevePossuirSARFornecedorValidoSpec();
            var actual = target.IsSatisfiedBy(new[] { multisourcing });

            Assert.False(actual.Satisfied);
        }
    }
}
