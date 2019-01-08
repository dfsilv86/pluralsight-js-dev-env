using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Processos;
using Walmart.Sgp.Infrastructure.Framework.Commons;

namespace Walmart.Sgp.Domain.UnitTests.Processos
{
    [TestFixture]
    [Category("Domain")]
    public class LojaProcesssosCargaTest
    {
        [Test]
        public void StatusGeral_QualquerCargaComErro_Erro()
        {     
            var target = new LojaProcessosCarga(DateTime.Today, new Bandeira(), new Loja());
            target.Cargas.Add(new ProcessoCarga { DataInicioExecucao = DateTime.Now });
            target.Cargas.Add(new ProcessoCarga { Erro = new ProcessoCargaErro() });

            Assert.AreEqual(ProcessoCargaStatus.Erro, target.StatusGeral);
        }

        [Test]
        public void StatusGeral_TodasCargasNaoIniciado_NaoIniciado()
        {
            var target = new LojaProcessosCarga(DateTime.Today, new Bandeira(), new Loja());
            target.Cargas.Add(new ProcessoCarga());
            target.Cargas.Add(new ProcessoCarga());

            Assert.AreEqual(ProcessoCargaStatus.NaoIniciado, target.StatusGeral);
        }

        [Test]
        public void StatusGeral_AlgumaCargaEmAndamento_EmAndamento()
        {
            var target = new LojaProcessosCarga(DateTime.Today, new Bandeira(), new Loja());
            target.Cargas.Add(new ProcessoCarga { DataInicioExecucao = DateTime.Now });
            target.Cargas.Add(new ProcessoCarga());
            target.Cargas.Add(new ProcessoCarga { DataInicioExecucao = DateTime.Now, DataFimExecucao = DateTime.Now });

            Assert.AreEqual(ProcessoCargaStatus.EmAndamento, target.StatusGeral);
        }

        [Test]
        public void StatusGeral_TodasCargasConcluido_Concluido()
        {
            var target = new LojaProcessosCarga(DateTime.Today, new Bandeira(), new Loja());
            target.Cargas.Add(new ProcessoCarga{ DataInicioExecucao = DateTime.Now, DataFimExecucao = DateTime.Now });
            target.Cargas.Add(new ProcessoCarga{ DataInicioExecucao = DateTime.Now, DataFimExecucao = DateTime.Now });

            Assert.AreEqual(ProcessoCargaStatus.Concluido, target.StatusGeral);
        }    
    }
}
