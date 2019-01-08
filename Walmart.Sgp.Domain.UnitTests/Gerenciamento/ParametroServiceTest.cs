using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Infrastructure.Data.Memory;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.UnitTests.Gerenciamento
{
    [TestFixture]
    [Category("Domain")]
    public class ParametroServiceTest
    {
        [Test]
        public void Obter_ExisteParametro_Parametro()
        {
            var gateway = new MemoryParametroGateway();
            gateway.Insert(new Parametro { cdUsuarioAdministrador = 22 });
            var target = new ParametroService(gateway);

            var actual = target.Obter();
            Assert.AreEqual(22, actual.cdUsuarioAdministrador);
        }

        [Test]
        public void Salvar_PropriedadesRequeridasNaoInformadas_Exception()
        {
            var gateway = new MemoryParametroGateway();
            var target = new ParametroService(gateway);
            var parametro = new Parametro();

            Assert.Catch<NotSatisfiedSpecException>(() => target.Salvar(parametro));

            parametro.cdUsuarioAdministrador = 1;
            Assert.Catch<NotSatisfiedSpecException>(() => target.Salvar(parametro));

            parametro.pcDivergenciaCustoCompra = 1;
            Assert.Catch<NotSatisfiedSpecException>(() => target.Salvar(parametro));

            parametro.PercentualAuditoria = 1;
            Assert.Catch<NotSatisfiedSpecException>(() => target.Salvar(parametro));

            parametro.dsServidorSmartEndereco = "a";
            Assert.Catch<NotSatisfiedSpecException>(() => target.Salvar(parametro));

            parametro.dsServidorSmartDiretorio = "a";
            Assert.Catch<NotSatisfiedSpecException>(() => target.Salvar(parametro));

            parametro.TpArquivoInventario = TipoFormatoArquivoInventario.Pipe;
            Assert.Catch<NotSatisfiedSpecException>(() => target.Salvar(parametro));

            parametro.qtdDiasArquivoInventarioVarejo = 1;
            Assert.Catch<NotSatisfiedSpecException>(() => target.Salvar(parametro));

            parametro.qtdDiasArquivoInventarioAtacado = 1;

            target.Salvar(parametro);
        }

        [Test]
        public void Salvar_NaoExisteParametro_Insere()
        {
            var gateway = new MemoryParametroGateway();
            var target = new ParametroService(gateway);
            var parametro = new Parametro
            {
                cdUsuarioAdministrador = 22,
                pcDivergenciaCustoCompra = 1,
                PercentualAuditoria = 1,
                dsServidorSmartEndereco = "a",
                dsServidorSmartDiretorio = "a",
                TpArquivoInventario = TipoFormatoArquivoInventario.Pipe,
                qtdDiasArquivoInventarioVarejo = 1,
                qtdDiasArquivoInventarioAtacado = 1
            };

            target.Salvar(parametro);

            var actual = gateway.FindAll().ToList();
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(22, actual[0].cdUsuarioAdministrador);
        }

        [Test]
        public void Salvar_ExisteParametro_Atualiza()
        {
            var gateway = new MemoryParametroGateway();

            var parametro = new Parametro
            {
                cdUsuarioAdministrador = 22,
                pcDivergenciaCustoCompra = 1,
                PercentualAuditoria = 1,
                dsServidorSmartEndereco = "a",
                dsServidorSmartDiretorio = "a",
                TpArquivoInventario = TipoFormatoArquivoInventario.Pipe,
                qtdDiasArquivoInventarioVarejo = 1,
                qtdDiasArquivoInventarioAtacado = 1
            };
            gateway.Insert(parametro);

            var target = new ParametroService(gateway);

            parametro = new Parametro
            {
                Id = 10,
                cdUsuarioAdministrador = 23,
                pcDivergenciaCustoCompra = 1,
                PercentualAuditoria = 1,
                dsServidorSmartEndereco = "a",
                dsServidorSmartDiretorio = "a",
                TpArquivoInventario = TipoFormatoArquivoInventario.Pipe,
                qtdDiasArquivoInventarioVarejo = 1,
                qtdDiasArquivoInventarioAtacado = 1
            };

            target.Salvar(parametro);

            var actual = gateway.FindAll().ToList();
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(23, actual[0].cdUsuarioAdministrador);
        }
    }
}
