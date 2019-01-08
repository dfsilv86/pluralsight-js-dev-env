using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.UnitTests.Reabastecimento
{
    [TestFixture]
    [Category("Domain")]
    public class AlterarSugestaoResponseExtensionsTest
    {
        [Test]
        public void Summarize_IgnorarMensagemPedidoMinimoTrue_Summarized()
        {
            List<AlterarSugestaoResponse> data = new List<AlterarSugestaoResponse>();

            data.Add(new AlterarSugestaoResponse(1) { Inexistente = true });
            data.Add(new AlterarSugestaoResponse(2) { Sucesso = true });
            data.Add(new AlterarSugestaoResponse(3) { NaoSalvaGradeSugestao = true });
            data.Add(new AlterarSugestaoResponse(5) { NaoSalvaPercentualAlteracao = true });
            data.Add(new AlterarSugestaoResponse(6) { Sucesso = true });
            data.Add(new AlterarSugestaoResponse(8) { Sucesso = true });

            AlterarSugestoesResponse summarized = data.Summarize();

            Assert.IsNotNull(summarized);
            Assert.AreEqual(6, summarized.Total);
            Assert.AreEqual(1, summarized.Inexistentes);
            Assert.AreEqual(3, summarized.Sucesso);
            Assert.AreEqual(1, summarized.NaoSalvaPercentualAlteracao);
            Assert.AreEqual(1, summarized.NaoSalvaGradeSugestao);

            var actualMsg = summarized.Mensagem.ToUpperInvariant();
            StringAssert.DoesNotContain(Texts.MinimumRequest.ToUpperInvariant(), actualMsg);
        }
    }
}
