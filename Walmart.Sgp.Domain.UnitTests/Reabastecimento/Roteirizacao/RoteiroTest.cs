using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Reabastecimento;

namespace Walmart.Sgp.Domain.UnitTests.Reabastecimento.Roteirizacao
{
    [TestFixture]
    [Category("Domain")]
    public class RoteiroTest
    {
        [Test]
        public void CdUsuarioAtualizacaoCdUsuarioCriacao_RoteiroComUsuarioDeCriacaoEAtualizacao_TemUmUsuarioAtualizacaoEUmUsuarioCriacao()
        {
            var r = new Roteiro();
            r.CdUsuarioCriacao = 1;
            r.CdUsuarioAtualizacao = 1;

            Assert.AreEqual(1, r.CdUsuarioAtualizacao);
            Assert.AreEqual(1, r.CdUsuarioCriacao);
        }
    }
}
