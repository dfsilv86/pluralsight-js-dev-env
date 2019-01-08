using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Walmart.Sgp.Domain.Processos;
using Walmart.Sgp.Infrastructure.Framework.Commons;

namespace Walmart.Sgp.Domain.UnitTests.Processos
{
    [TestFixture]
    [Category("Domain")]
    public class ProcessoCargaTest
    {
        [Test]
        public void Status_DataInicioNull_NaoIniciado()
        {
            var actual = new ProcessoCarga();            
            Assert.AreEqual(ProcessoCargaStatus.NaoIniciado, actual.Status);
        }

        [Test]
        public void Status_DataTerminoNull_EmAndamento()
        {
            var actual = new ProcessoCarga { DataInicioExecucao = DateTime.Now };
            Assert.AreEqual(ProcessoCargaStatus.EmAndamento, actual.Status);
        }

        [Test]
        public void Status_ComDataTermino_Concluido()
        {
            var actual = new ProcessoCarga { DataInicioExecucao = DateTime.Now, DataFimExecucao = DateTime.Now };
            Assert.AreEqual(ProcessoCargaStatus.Concluido, actual.Status);
        }

        [Test]
        public void Status_ComMensagemErro_Erro()
        {
            var actual = new ProcessoCarga { DataInicioExecucao = DateTime.Now, DataFimExecucao = DateTime.Now, Erro = new ProcessoCargaErro() };
            Assert.AreEqual(ProcessoCargaStatus.Erro, actual.Status);
        }
    }
}
