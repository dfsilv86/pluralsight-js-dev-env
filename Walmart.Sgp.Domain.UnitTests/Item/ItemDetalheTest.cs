using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Walmart.Sgp.Domain.Item;

namespace Walmart.Sgp.Domain.UnitTests.Inventarios
{
    [TestFixture]
    [Category("Domain")]
    public class ItemDetalheTest
    {
        [Test]
        public void PossuiCompraCasada_qtItensCadastradosCompraCasadaZero_False()
        {
            var target = new ItemDetalhe();
            target.qtItensCadastradosCompraCasada = 0;
            Assert.IsFalse(target.PossuiCadastroCompraCasada);
        }

        [Test]
        public void PossuiCompraCasada_qtItensCadastradosCompraCasadaMaiorQueZero_True()
        {
            var target = new ItemDetalhe();
            target.qtItensCadastradosCompraCasada = 24;
            Assert.IsTrue(target.PossuiCadastroCompraCasada);
        }

        [Test]
        public void PesadoCaixa_BlPesadoCaixaNull_False()
        {
            var target = new ItemDetalhe { BlPesadoCaixa = null };
            Assert.IsFalse(target.PesadoCaixa);
        }

        [Test]
        public void PesadoCaixa_BlPesadoCaixaNoNull_SameValue()
        {
            var target = new ItemDetalhe { BlPesadoCaixa = false };
            Assert.IsFalse(target.PesadoCaixa);
            target.BlPesadoCaixa = true;
            Assert.IsTrue(target.PesadoCaixa);
        }

        [Test]
        public void PesadoRetaguarda_BlPesadoRetaguardaNull_False()
        {
            var target = new ItemDetalhe { BlPesadoRetaguarda = null };
            Assert.IsFalse(target.PesadoRetaguarda);
        }

        [Test]
        public void PesadoRetaguarda_BlPesadoRetaguardaNoNull_SameValue()
        {
            var target = new ItemDetalhe { BlPesadoRetaguarda = false };
            Assert.IsFalse(target.PesadoRetaguarda);
            target.BlPesadoRetaguarda = true;
            Assert.IsTrue(target.PesadoRetaguarda);
        }

        [Test]
        public void TipoItem_PesadoCaixaOrPesadoRetaguarda_PesoVariavel()
        {
            var target = new ItemDetalhe();
            target.BlPesadoCaixa = true;
            Assert.AreEqual(TipoItem.PesoVariavel, target.TipoItem);

            target.BlPesadoRetaguarda = true;
            Assert.AreEqual(TipoItem.PesoVariavel, target.TipoItem);

            target.BlPesadoCaixa = false;
            target.BlPesadoRetaguarda = true;
            Assert.AreEqual(TipoItem.PesoVariavel, target.TipoItem);
        }

        [Test]
        public void TipoItem_NotPesadoCaixaOrPesadoRetaguarda_PesoFixo()
        {
            var target = new ItemDetalhe();
            target.BlPesadoCaixa = false;
            target.BlPesadoRetaguarda = false;
            Assert.AreEqual(TipoItem.PesoFixo, target.TipoItem);
        }

        [Test]
        public void IsXDock_ValorTipoReabastecimento3_True()
        {
            var target = new ItemDetalhe
            {
                VlTipoReabastecimento = ValorTipoReabastecimento.CrossDocking3
            };

            Assert.IsTrue(target.IsXDock);
        }

        [Test]
        public void IsXDock_ValorTipoReabastecimento33_True()
        {
            var target = new ItemDetalhe
            {
                VlTipoReabastecimento = ValorTipoReabastecimento.CrossDocking33
            };

            Assert.IsTrue(target.IsXDock);
        }

        [Test]
        public void IsXDock_ValorTipoReabastecimento94_True()
        {
            var target = new ItemDetalhe
            {
                VlTipoReabastecimento = ValorTipoReabastecimento.CrossDocking94
            };

            Assert.IsTrue(target.IsXDock);
        }

        [Test]
        public void IsDSD_ValorTipoReabastecimento7_True()
        {
            var target = new ItemDetalhe
            {
                VlTipoReabastecimento = ValorTipoReabastecimento.Dsd7
            };

            Assert.IsTrue(target.IsDSD);
        }

        [Test]
        public void IsDSD_ValorTipoReabastecimento37_True()
        {
            var target = new ItemDetalhe
            {
                VlTipoReabastecimento = ValorTipoReabastecimento.Dsd37
            };

            Assert.IsTrue(target.IsDSD);
        }

        [Test]
        public void IsDSD_ValorTipoReabastecimento97_True()
        {
            var target = new ItemDetalhe
            {
                VlTipoReabastecimento = ValorTipoReabastecimento.Dsd97
            };

            Assert.IsTrue(target.IsDSD);
        }

        [Test]
        public void IsStaple_ValorTipoReabastecimento20_True()
        {
            var target = new ItemDetalhe
            {
                VlTipoReabastecimento = ValorTipoReabastecimento.StapleStock20
            };

            Assert.IsTrue(target.IsStaple);
        }

        [Test]
        public void IsStaple_ValorTipoReabastecimento22_True()
        {
            var target = new ItemDetalhe
            {
                VlTipoReabastecimento = ValorTipoReabastecimento.StapleStock22
            };

            Assert.IsTrue(target.IsStaple);
        }

        [Test]
        public void IsStaple_ValorTipoReabastecimento40_True()
        {
            var target = new ItemDetalhe
            {
                VlTipoReabastecimento = ValorTipoReabastecimento.StapleStock40
            };

            Assert.IsTrue(target.IsStaple);
        }

        [Test]
        public void IsStaple_ValorTipoReabastecimento42_True()
        {
            var target = new ItemDetalhe
            {
                VlTipoReabastecimento = ValorTipoReabastecimento.StapleStock42
            };

            Assert.IsTrue(target.IsStaple);
        }

        [Test]
        public void IsStaple_ValorTipoReabastecimento43_True()
        {
            var target = new ItemDetalhe
            {
                VlTipoReabastecimento = ValorTipoReabastecimento.StapleStock43
            };

            Assert.IsTrue(target.IsStaple);
        }

        [Test]
        public void IsStaple_ValorTipoReabastecimento81_True()
        {
            var target = new ItemDetalhe
            {
                VlTipoReabastecimento = ValorTipoReabastecimento.StapleStock81
            };

            Assert.IsTrue(target.IsStaple);
        }
    }
}