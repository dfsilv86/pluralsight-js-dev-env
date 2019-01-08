using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Infrastructure.IO.Importing.Inventario;

namespace Walmart.Sgp.Infrastructure.IO.FunctionalTests.Importing.Inventario
{
    [TestFixture]
    public class LeitorArquivoInventarioTest
    {
        #region Rtl - Supercenter

        [Test]
        [Category("ImportacaoInventario")]
        public void LerArquivo_RtlSupercenter_Ok()
        {
            LogMock log = new LogMock();
            ParametroMock parametro = new ParametroMock(1, 1);

            string root = Path.GetDirectoryName(typeof(LogMock).Assembly.Location);
            string pathRelative = @"Importing\Inventario\ArquivosTeste\Rtl\Supercenter";

            string[] files = new string[] 
            { 
                @"spi28.2517007",
                @"spi28.2815780",
                @"spi28.2826669",
                @"qtdonhand.valido",
                @"semtotaldif.valido",
            };

            var arquivos = files.Select(f => Path.Combine(root, pathRelative, f));

            ILeitorArquivoInventario target = new LeitorArquivoInventario(log, parametro);

            var result = target.LerArquivosRtlSupercenter(TipoProcessoImportacao.Automatico, TipoOrigemImportacao.Loja, 999, arquivos, new DateTime(2016, 03, 28)).ToArray();

            Assert.AreEqual(5, result.Length);

            var result2517007 = result.Single(r => r.NomeArquivo == @"spi28.2517007");
            var result2815780 = result.Single(r => r.NomeArquivo == @"spi28.2815780");
            var result2826669 = result.Single(r => r.NomeArquivo == @"spi28.2826669");
            var resultonhand = result.Single(r => r.NomeArquivo == @"qtdonhand.valido");
            var resultsemtotaldif = result.Single(r => r.NomeArquivo == @"semtotaldif.valido");

            Assert.IsTrue(result2517007.IsArquivoValido);
            Assert.IsTrue(result2815780.IsArquivoValido);
            Assert.IsTrue(result2826669.IsArquivoValido);

            Assert.AreEqual(103, result2826669.CdLoja);
            Assert.IsNotNull(result2826669.DataArquivo);
            Assert.AreEqual(new DateTime(2016, 03, 28), result2826669.DataArquivo.Value.Date);
            Assert.AreEqual(new DateTime(2016, 03, 28), result2826669.DataInventario);
            Assert.AreEqual(999, result2826669.IdLojaImportacao);
            Assert.IsFalse(result2826669.IsMultiDepartamento);
            Assert.AreEqual(123, result2826669.Itens.Count());
            Assert.AreEqual(true, result2826669.LeuCabecalho);
            Assert.AreEqual("spi28.2826669", result2826669.NomeArquivo);
            Assert.AreEqual(TipoArquivoInventario.Final, result2826669.TipoArquivo);
            Assert.AreEqual(0, result2826669.TotalEstoqueFinal);
            Assert.AreEqual(123, result2826669.TotalItensContados);
            Assert.AreEqual(94, result2826669.UltimoCdDepartamentoLido);

            var item = result2826669.Itens.First();

            Assert.AreEqual(94, item.CdDepartamento);
            Assert.AreEqual(500006325, item.CdItem);
            Assert.AreEqual(710530, item.CdUpc);
            Assert.AreEqual("TFARIAS", item.Completo);
            Assert.AreEqual(5m, item.CustoUnitario);
            Assert.AreEqual("CAQUI CORACAO BOI KG", item.DescricaoItem);
            Assert.AreEqual("CAQU COR BOI", item.DescricaoTamanho);
            Assert.IsNull(item.Erro);
            Assert.AreEqual("71053", item.NumeroEstoque);
            Assert.AreEqual(6.98m, item.PrecoUnitarioVarejo);
            Assert.AreEqual(0m, item.QtItem);
            Assert.AreEqual(7.90m, item.QuantidadeDif);
            Assert.AreEqual(0m, item.QuantidadeOnHand);
            Assert.AreEqual("/", item.Tamanho);
            Assert.AreEqual(0m, item.TotalAumentadoContg);
            Assert.AreEqual(39.50m, item.TotalAumentadoDif);
            Assert.AreEqual(0m, item.TotalAumentadoOnHand);
            Assert.AreEqual(new DateTime(2016, 03, 28, 13, 59, 00), item.UltimaContagem);

            item = result2826669.Itens.Last();

            Assert.AreEqual(94, item.CdDepartamento);
            Assert.AreEqual(500673578, item.CdItem);
            Assert.AreEqual(7898935614570, item.CdUpc);
            Assert.AreEqual("TFARIAS", item.Completo);
            Assert.AreEqual(4.41m, item.CustoUnitario);
            Assert.AreEqual("TOMATE SECO SACHE", item.DescricaoItem);
            Assert.AreEqual("TOMATE SECO", item.DescricaoTamanho);
            Assert.IsNull(item.Erro);
            Assert.AreEqual("210", item.NumeroEstoque);
            Assert.AreEqual(5.12m, item.PrecoUnitarioVarejo);
            Assert.AreEqual(0m, item.QtItem);
            Assert.AreEqual(8m, item.QuantidadeDif);
            Assert.AreEqual(0m, item.QuantidadeOnHand);
            Assert.AreEqual("/", item.Tamanho);
            Assert.AreEqual(0m, item.TotalAumentadoContg);
            Assert.AreEqual(35.28m, item.TotalAumentadoDif);
            Assert.AreEqual(0m, item.TotalAumentadoOnHand);
            Assert.AreEqual(new DateTime(2016, 03, 28, 14, 9, 0), item.UltimaContagem);

            Assert.AreEqual(1.45m, resultonhand.Itens[0].QuantidadeOnHand);
            Assert.AreEqual(1.56m, resultonhand.Itens[0].TotalAumentadoOnHand);

            Assert.AreEqual(1.45m, resultsemtotaldif.Itens[0].QuantidadeOnHand);
            Assert.AreEqual(1.56m, resultsemtotaldif.Itens[0].TotalAumentadoOnHand);
            Assert.AreEqual(0m, resultsemtotaldif.Itens[0].QuantidadeDif);
            Assert.AreEqual(0m, resultsemtotaldif.Itens[0].TotalAumentadoDif);

            Assert.AreEqual(0, log.Criticas);
            Assert.AreEqual(0, log.Invalidos);
        }

        [Test]
        [Category("ImportacaoInventario")]
        public void LerArquivo_RtlSupercenter_DataInventarioForaIntervaloQtdDias()
        {
            LogMock log = new LogMock();
            ParametroMock parametro = new ParametroMock(1, 1);

            string root = Path.GetDirectoryName(typeof(LogMock).Assembly.Location);
            string pathRelative = @"Importing\Inventario\ArquivosTeste\Rtl\Supercenter";

            string[] files = new string[] 
            { 
                @"spi28.2517007"
            };

            var arquivos = files.Select(f => Path.Combine(root, pathRelative, f)).Single();

            ILeitorArquivoInventario target = new LeitorArquivoInventario(log, parametro);

            var result = target.LerArquivosRtlSupercenter(TipoProcessoImportacao.Automatico, TipoOrigemImportacao.Loja, 999, arquivos, new DateTime(2016, 03, 28)).ToArray();

            Assert.AreEqual(1, result.Length);

            var result2517007 = result.Single(r => r.NomeArquivo == @"spi28.2517007");

            Assert.AreEqual(140, result2517007.CdLoja);
            Assert.AreEqual(new DateTime(2015, 11, 25, 21, 00, 35), result2517007.DataArquivo);
            Assert.AreEqual(new DateTime(2016, 03, 28), result2517007.DataInventario);
            Assert.AreEqual(999, result2517007.IdLojaImportacao);
            Assert.IsTrue(result2517007.IsArquivoValido);
            Assert.IsNull(result2517007.IsMultiDepartamento);
            Assert.AreEqual(0, result2517007.Itens.Count());
            Assert.IsTrue(result2517007.LeuCabecalho);
            Assert.AreEqual(TipoArquivoInventario.Nenhum, result2517007.TipoArquivo);
            Assert.IsNull(result2517007.TotalEstoqueFinal);
            Assert.IsNull(result2517007.TotalItensContados);
            Assert.IsNull(result2517007.UltimoCdDepartamentoLido);

            Assert.AreEqual(0, log.Criticas);
            Assert.AreEqual(0, log.Invalidos);
        }

        [Test]
        [Category("ImportacaoInventario")]
        public void LerArquivo_RtlSupercenter_CabecalhoFormatoInvalido()
        {
            LogMock log = new LogMock();
            ParametroMock parametro = new ParametroMock(1, 1);

            string root = Path.GetDirectoryName(typeof(LogMock).Assembly.Location);
            string pathRelative = @"Importing\Inventario\ArquivosTeste\Rtl\Supercenter";
            string[] files = new string[] 
            { 
                @"cabecalho.invalido",
                @"cabecalho2.invalido",
                @"cabecalho3.invalido",
                @"cabecalho4.invalido",
                @"cabecalho5.invalido",
            };

            var arquivos = files.Select(f => Path.Combine(root, pathRelative, f));

            ILeitorArquivoInventario target = new LeitorArquivoInventario(log, parametro);

            var result = target.LerArquivosRtlSupercenter(TipoProcessoImportacao.Automatico, TipoOrigemImportacao.Loja, 999, arquivos, new DateTime(2015, 11, 25)).ToArray();

            Assert.AreEqual(5, result.Length);

            Assert.IsFalse(result[0].IsArquivoValido);
            Assert.IsFalse(result[1].IsArquivoValido);
            Assert.IsFalse(result[2].IsArquivoValido);
            Assert.IsFalse(result[3].IsArquivoValido);
            Assert.IsFalse(result[4].IsArquivoValido);

            Assert.AreEqual(1, log.Criticas);
            Assert.AreEqual(5, log.Invalidos);
        }

        [Test]
        [Category("ImportacaoInventario")]
        public void LerArquivo_RtlSupercenter_RodapeFormatoInvalido()
        {
            LogMock log = new LogMock();
            ParametroMock parametro = new ParametroMock(1, 1);

            string root = Path.GetDirectoryName(typeof(LogMock).Assembly.Location);
            string pathRelative = @"Importing\Inventario\ArquivosTeste\Rtl\Supercenter";
            string[] files = new string[]
            { 
                @"rodape.invalido",
            };

            var arquivos = files.Select(f => Path.Combine(root, pathRelative, f)).Single();

            ILeitorArquivoInventario target = new LeitorArquivoInventario(log, parametro);

            var result = target.LerArquivosRtlSupercenter(TipoProcessoImportacao.Automatico, TipoOrigemImportacao.Loja, 999, arquivos, new DateTime(2015, 11, 25)).ToArray();

            Assert.AreEqual(1, result.Length);

            Assert.IsFalse(result[0].IsArquivoValido);
            Assert.AreEqual(126, result[0].Itens.Count);
            Assert.IsTrue(result[0].LeuCabecalho);

            Assert.AreEqual(0, log.Criticas);
            Assert.AreEqual(1, log.Invalidos);
        }

        [Test]
        [Category("ImportacaoInventario")]
        public void LerArquivo_RtlSupercenter_Cabecalho1DataInvalida()
        {
            LogMock log = new LogMock();
            ParametroMock parametro = new ParametroMock(1, 1);

            string root = Path.GetDirectoryName(typeof(LogMock).Assembly.Location);
            string pathRelative = @"Importing\Inventario\ArquivosTeste\Rtl\Supercenter";
            string[] files = new string[] 
            { 
                @"data.invalida"
            };

            var arquivos = files.Select(f => Path.Combine(root, pathRelative, f)).Single();

            ILeitorArquivoInventario target = new LeitorArquivoInventario(log, parametro);

            var result = target.LerArquivosRtlSupercenter(TipoProcessoImportacao.Automatico, TipoOrigemImportacao.Loja, 999, arquivos, new DateTime(2015, 11, 25)).ToArray();

            Assert.AreEqual(1, result.Length);

            Assert.IsFalse(result[0].IsArquivoValido);

            Assert.AreEqual(0, log.Criticas);
            Assert.AreEqual(1, log.Invalidos);
        }

        [Test]
        [Category("ImportacaoInventario")]
        public void LerArquivo_RtlSupercenter_Cabecalho1LojaInvalida()
        {
            LogMock log = new LogMock();
            ParametroMock parametro = new ParametroMock(1, 1);

            string root = Path.GetDirectoryName(typeof(LogMock).Assembly.Location);
            string pathRelative = @"Importing\Inventario\ArquivosTeste\Rtl\Supercenter";
            string[] files = new string[] 
            { 
                @"loja.invalida"
            };

            var arquivos = files.Select(f => Path.Combine(root, pathRelative, f)).Single();

            ILeitorArquivoInventario target = new LeitorArquivoInventario(log, parametro);

            var result = target.LerArquivosRtlSupercenter(TipoProcessoImportacao.Automatico, TipoOrigemImportacao.Loja, 999, arquivos, new DateTime(2015, 11, 25)).ToArray();

            Assert.AreEqual(1, result.Length);

            Assert.IsFalse(result[0].IsArquivoValido);

            Assert.AreEqual(0, log.Criticas);
            Assert.AreEqual(1, log.Invalidos);
        }

        [Test]
        [Category("ImportacaoInventario")]
        public void LerArquivo_RtlSupercenter_Cabecalho2Multidepartamento()
        {
            LogMock log = new LogMock();
            ParametroMock parametro = new ParametroMock(1, 1);

            string root = Path.GetDirectoryName(typeof(LogMock).Assembly.Location);
            string pathRelative = @"Importing\Inventario\ArquivosTeste\Rtl\Supercenter";
            string[] files = new string[] 
            { 
                @"multidepartamento.invalido"
            };

            var arquivos = files.Select(f => Path.Combine(root, pathRelative, f)).Single();

            ILeitorArquivoInventario target = new LeitorArquivoInventario(log, parametro);

            var result = target.LerArquivosRtlSupercenter(TipoProcessoImportacao.Automatico, TipoOrigemImportacao.Loja, 999, arquivos, new DateTime(2015, 11, 25)).ToArray();

            Assert.AreEqual(1, result.Length);

            Assert.IsFalse(result[0].IsArquivoValido);
            Assert.IsTrue(result[0].IsMultiDepartamento);

            Assert.AreEqual(0, log.Criticas);
            Assert.AreEqual(1, log.Invalidos);
        }

        [Test]
        [Category("ImportacaoInventario")]
        public void LerArquivo_RtlSupercenter_Cabecalho3NaoFinal()
        {
            LogMock log = new LogMock();
            ParametroMock parametro = new ParametroMock(1, 1);

            string root = Path.GetDirectoryName(typeof(LogMock).Assembly.Location);
            string pathRelative = @"Importing\Inventario\ArquivosTeste\Rtl\Supercenter";
            string[] files = new string[] 
            { 
                @"parcial.invalido"
            };

            var arquivos = files.Select(f => Path.Combine(root, pathRelative, f)).Single();

            ILeitorArquivoInventario target = new LeitorArquivoInventario(log, parametro);

            var result = target.LerArquivosRtlSupercenter(TipoProcessoImportacao.Automatico, TipoOrigemImportacao.Loja, 999, arquivos, new DateTime(2015, 11, 25)).ToArray();

            Assert.AreEqual(1, result.Length);

            Assert.IsFalse(result[0].IsArquivoValido);
            Assert.AreEqual(TipoArquivoInventario.Nenhum, result[0].TipoArquivo);

            Assert.AreEqual(0, log.Criticas);
            Assert.AreEqual(1, log.Invalidos);
        }

        [Test]
        [Category("ImportacaoInventario")]
        public void LerArquivo_RtlSupercenter_ErroItem()
        {
            LogMock log = new LogMock();
            ParametroMock parametro = new ParametroMock(1, 1);

            string root = Path.GetDirectoryName(typeof(LogMock).Assembly.Location);
            string pathRelative = @"Importing\Inventario\ArquivosTeste\Rtl\Supercenter";
            string[] files = new string[] 
            { 
                @"erroitem.valido"
            };

            var arquivos = files.Select(f => Path.Combine(root, pathRelative, f)).Single();

            ILeitorArquivoInventario target = new LeitorArquivoInventario(log, parametro);

            var result = target.LerArquivosRtlSupercenter(TipoProcessoImportacao.Automatico, TipoOrigemImportacao.Loja, 999, arquivos, new DateTime(2015, 11, 25)).ToArray();

            Assert.AreEqual(1, result.Length);

            Assert.IsTrue(result[0].IsArquivoValido);
            Assert.AreEqual(TipoArquivoInventario.Final, result[0].TipoArquivo);
            Assert.AreEqual(126, result[0].Itens.Count());
            Assert.IsTrue(result[0].Itens[0].Erro.StartsWith(@"System.FormatException"));

            Assert.AreEqual(0, log.Criticas);
            Assert.AreEqual(1, log.Invalidos);
        }

        #endregion

        #region Rtl - Sams

        [Test]
        [Category("ImportacaoInventario")]
        public void LerArquivo_RtlSams_Ok()
        {
            LogMock log = new LogMock();
            ParametroMock parametro = new ParametroMock(1, 1);

            string root = Path.GetDirectoryName(typeof(LogMock).Assembly.Location);
            string pathRelative = @"Importing\Inventario\ArquivosTeste\Rtl\Sams";
            string[] files = new string[] 
            { 
                @"spi28.0121440",
                @"spi28.3121488",
                @"semvaloresopcionais.valido"
            };

            var arquivos = files.Select(f => Path.Combine(root, pathRelative, f));

            ILeitorArquivoInventario target = new LeitorArquivoInventario(log, parametro);

            var result = target.LerArquivosRtlSams(TipoProcessoImportacao.Automatico, TipoOrigemImportacao.Loja, 999, arquivos, TipoArquivoInventario.Parcial, new DateTime(2016, 03, 28)).ToArray();

            Assert.AreEqual(3, result.Length);

            var result0121440 = result.Single(r => r.NomeArquivo == @"spi28.0121440");
            var result3121488 = result.Single(r => r.NomeArquivo == @"spi28.3121488");
            var resultsemopcionais = result.Single(r => r.NomeArquivo == @"semvaloresopcionais.valido");

            Assert.IsTrue(result0121440.IsArquivoValido);
            Assert.IsTrue(result3121488.IsArquivoValido);
            Assert.IsTrue(resultsemopcionais.IsArquivoValido);

            Assert.AreEqual(TipoArquivoInventario.Parcial, result0121440.TipoArquivo);
            Assert.AreEqual(TipoArquivoInventario.Parcial, result3121488.TipoArquivo);
            Assert.AreEqual(TipoArquivoInventario.Parcial, resultsemopcionais.TipoArquivo);

            Assert.AreEqual(4943, result0121440.CdLoja);
            Assert.AreEqual(new DateTime(2016, 04, 01, 11, 26, 37), result0121440.DataArquivo);
            Assert.AreEqual(new DateTime(2016, 03, 28), result0121440.DataInventario);
            Assert.AreEqual(999, result0121440.IdLojaImportacao);
            Assert.IsTrue(result0121440.IsArquivoValido);
            Assert.IsFalse(result0121440.IsMultiDepartamento);
            Assert.AreEqual(9, result0121440.Itens.Count());
            Assert.IsTrue(result0121440.LeuCabecalho);
            Assert.AreEqual("spi28.0121440", result0121440.NomeArquivo);
            Assert.AreEqual(TipoArquivoInventario.Parcial, result0121440.TipoArquivo);
            Assert.AreEqual(3842.9m, result0121440.TotalEstoqueFinal);
            Assert.AreEqual(9, result0121440.TotalItensContados);
            Assert.AreEqual(56, result0121440.UltimoCdDepartamentoLido);

            var item = result0121440.Itens.First();

            Assert.AreEqual(56, item.CdDepartamento);
            Assert.AreEqual(119740, item.CdItem);
            Assert.IsNull(item.CdUpc);
            Assert.AreEqual("JOSE", item.Completo);
            Assert.AreEqual(5.5m, item.CustoUnitario);
            Assert.AreEqual("TANGERINA/CREMENV IMP.", item.DescricaoItem);
            Assert.AreEqual(string.Empty, item.DescricaoTamanho);
            Assert.IsNull(item.Erro);
            Assert.AreEqual("1005", item.NumeroEstoque);
            Assert.AreEqual(8.28m, item.PrecoUnitarioVarejo);
            Assert.AreEqual(97m, item.QtItem);
            Assert.AreEqual(105.03m, item.QuantidadeDif);
            Assert.AreEqual(-8.03m, item.QuantidadeOnHand);
            Assert.AreEqual("KG/", item.Tamanho);
            Assert.AreEqual(533.5m, item.TotalAumentadoContg);
            Assert.AreEqual(577.67m, item.TotalAumentadoDif);
            Assert.AreEqual(-44.17m, item.TotalAumentadoOnHand);
            Assert.AreEqual(new DateTime(2016, 04, 01, 11, 24, 00), item.UltimaContagem);

            item = result0121440.Itens.Last();

            Assert.AreEqual(56, item.CdDepartamento);
            Assert.AreEqual(797879, item.CdItem);
            Assert.IsNull(item.CdUpc);
            Assert.AreEqual("JOSE", item.Completo);
            Assert.AreEqual(2.2m, item.CustoUnitario);
            Assert.AreEqual("ABOBORA PAULISTA/ABOBORA PAUL", item.DescricaoItem);
            Assert.AreEqual(string.Empty, item.DescricaoTamanho);
            Assert.IsNull(item.Erro);
            Assert.AreEqual("740", item.NumeroEstoque);
            Assert.AreEqual(6.97m, item.PrecoUnitarioVarejo);
            Assert.AreEqual(132m, item.QtItem);
            Assert.AreEqual(135.24m, item.QuantidadeDif);
            Assert.AreEqual(-3.24m, item.QuantidadeOnHand);
            Assert.AreEqual("KG/2 DIAS", item.Tamanho);
            Assert.AreEqual(290.4m, item.TotalAumentadoContg);
            Assert.AreEqual(297.53m, item.TotalAumentadoDif);
            Assert.AreEqual(-7.13m, item.TotalAumentadoOnHand);
            Assert.AreEqual(new DateTime(2016, 04, 01, 11, 26, 00), item.UltimaContagem);

            Assert.AreEqual(0m, resultsemopcionais.Itens[0].QuantidadeOnHand);
            Assert.AreEqual(0m, resultsemopcionais.Itens[0].TotalAumentadoOnHand);
            Assert.AreEqual(0m, resultsemopcionais.Itens[0].QuantidadeDif);
            Assert.AreEqual(0m, resultsemopcionais.Itens[0].TotalAumentadoDif);

            Assert.AreEqual(0, log.Criticas);
            Assert.AreEqual(0, log.Invalidos);
        }

        [Test]
        [Category("ImportacaoInventario")]
        public void LerArquivo_RtlSams_CabecalhoFormatoInvalido()
        {
            LogMock log = new LogMock();
            ParametroMock parametro = new ParametroMock(1, 1);

            string root = Path.GetDirectoryName(typeof(LogMock).Assembly.Location);
            string pathRelative = @"Importing\Inventario\ArquivosTeste\Rtl\Sams";
            string[] files = new string[] 
            { 
                @"cabecalho.invalido",
            };

            var arquivos = files.Select(f => Path.Combine(root, pathRelative, f)).Single();

            ILeitorArquivoInventario target = new LeitorArquivoInventario(log, parametro);

            var result = target.LerArquivosRtlSams(TipoProcessoImportacao.Automatico, TipoOrigemImportacao.Loja, 999, arquivos, TipoArquivoInventario.Parcial, new DateTime(2015, 11, 25)).ToArray();

            Assert.AreEqual(1, result.Length);

            Assert.IsFalse(result[0].IsArquivoValido);

            Assert.AreEqual(1, log.Criticas);
            Assert.AreEqual(1, log.Invalidos);
        }

        [Test]
        [Category("ImportacaoInventario")]
        public void LerArquivo_RtlSams_RodapeFormatoInvalido()
        {
            LogMock log = new LogMock();
            ParametroMock parametro = new ParametroMock(1, 1);

            string root = Path.GetDirectoryName(typeof(LogMock).Assembly.Location);
            string pathRelative = @"Importing\Inventario\ArquivosTeste\Rtl\Sams";
            string[] files = new string[] 
            { 
                @"rodape.invalido",
            };

            var arquivos = files.Select(f => Path.Combine(root, pathRelative, f)).Single();

            ILeitorArquivoInventario target = new LeitorArquivoInventario(log, parametro);

            var result = target.LerArquivosRtlSams(TipoProcessoImportacao.Automatico, TipoOrigemImportacao.Loja, 999, arquivos, TipoArquivoInventario.Parcial, new DateTime(2015, 11, 25)).ToArray();

            Assert.AreEqual(1, result.Length);

            Assert.IsFalse(result[0].IsArquivoValido);
            Assert.AreEqual(9, result[0].Itens.Count);
            Assert.IsTrue(result[0].LeuCabecalho);

            Assert.AreEqual(0, log.Criticas);
            Assert.AreEqual(1, log.Invalidos);
        }

        [Test]
        [Category("ImportacaoInventario")]
        public void LerArquivo_RtlSams_Cabecalho2Multidepartamento()
        {
            LogMock log = new LogMock();
            ParametroMock parametro = new ParametroMock(1, 1);

            string root = Path.GetDirectoryName(typeof(LogMock).Assembly.Location);
            string pathRelative = @"Importing\Inventario\ArquivosTeste\Rtl\Sams";
            string[] files = new string[] 
            { 
                @"multidepartamento.invalido"
            };

            var arquivos = files.Select(f => Path.Combine(root, pathRelative, f)).Single();

            ILeitorArquivoInventario target = new LeitorArquivoInventario(log, parametro);

            var result = target.LerArquivosRtlSams(TipoProcessoImportacao.Automatico, TipoOrigemImportacao.Loja, 999, arquivos, TipoArquivoInventario.Parcial, new DateTime(2015, 11, 25)).ToArray();

            Assert.AreEqual(1, result.Length);

            Assert.IsFalse(result[0].IsArquivoValido);
            Assert.IsTrue(result[0].IsMultiDepartamento);

            Assert.AreEqual(0, log.Criticas);
            Assert.AreEqual(1, log.Invalidos);
        }

        [Test]
        [Category("ImportacaoInventario")]
        public void LerArquivo_RtlSams_FinalOk()
        {
            LogMock log = new LogMock();
            ParametroMock parametro = new ParametroMock(1, 1);

            string root = Path.GetDirectoryName(typeof(LogMock).Assembly.Location);
            string pathRelative = @"Importing\Inventario\ArquivosTeste\Rtl\Sams";
            string[] files = new string[] 
            { 
                @"final.valido"
            };

            var arquivos = files.Select(f => Path.Combine(root, pathRelative, f)).Single();

            ILeitorArquivoInventario target = new LeitorArquivoInventario(log, parametro);

            var result = target.LerArquivosRtlSams(TipoProcessoImportacao.Automatico, TipoOrigemImportacao.Loja, 999, arquivos, TipoArquivoInventario.Final, new DateTime(2015, 11, 25)).ToArray();

            Assert.AreEqual(1, result.Length);

            Assert.IsTrue(result[0].IsArquivoValido);
            Assert.AreEqual(TipoArquivoInventario.Final, result[0].TipoArquivo);

            Assert.AreEqual(0, log.Criticas);
            Assert.AreEqual(0, log.Invalidos);
        }

        [Test]
        [Category("ImportacaoInventario")]
        public void LerArquivo_RtlSams_ErroFinalImportandoParcial()
        {
            LogMock log = new LogMock();
            ParametroMock parametro = new ParametroMock(1, 1);

            string root = Path.GetDirectoryName(typeof(LogMock).Assembly.Location);
            string pathRelative = @"Importing\Inventario\ArquivosTeste\Rtl\Sams";
            string[] files = new string[] 
            { 
                @"final.valido"
            };

            var arquivos = files.Select(f => Path.Combine(root, pathRelative, f)).Single();

            ILeitorArquivoInventario target = new LeitorArquivoInventario(log, parametro);

            var result = target.LerArquivosRtlSams(TipoProcessoImportacao.Automatico, TipoOrigemImportacao.Loja, 999, arquivos, TipoArquivoInventario.Parcial, new DateTime(2015, 11, 25)).ToArray();

            Assert.AreEqual(1, result.Length);

            Assert.IsFalse(result[0].IsArquivoValido);
            Assert.AreEqual(TipoArquivoInventario.Final, result[0].TipoArquivo);

            Assert.AreEqual(0, log.Criticas);
            Assert.AreEqual(1, log.Invalidos);
        }

        [Test]
        [Category("ImportacaoInventario")]
        public void LerArquivo_RtlSams_ErroItem()
        {
            LogMock log = new LogMock();
            ParametroMock parametro = new ParametroMock(1, 1);

            string root = Path.GetDirectoryName(typeof(LogMock).Assembly.Location);
            string pathRelative = @"Importing\Inventario\ArquivosTeste\Rtl\Sams";
            string[] files = new string[]
            { 
                @"erroitem.valido"
            };

            var arquivos = files.Select(f => Path.Combine(root, pathRelative, f)).Single();

            ILeitorArquivoInventario target = new LeitorArquivoInventario(log, parametro);

            var result = target.LerArquivosRtlSams(TipoProcessoImportacao.Automatico, TipoOrigemImportacao.Loja, 999, arquivos, TipoArquivoInventario.Parcial, new DateTime(2015, 11, 25)).ToArray();

            Assert.AreEqual(1, result.Length);

            Assert.IsTrue(result[0].IsArquivoValido);
            Assert.AreEqual(TipoArquivoInventario.Parcial, result[0].TipoArquivo);
            Assert.AreEqual(9, result[0].Itens.Count());
            Assert.AreEqual(@"O arquivo \'erroitem.valido\' é inválido, não foi possivel ler o item : - 119740, - , - TANGERINA/CREMENV IMP., - 1005, - 5.50, - 8.28, - 04/01/2016 11:24:00, - , - 97.00, - , - , - , - , -JOSE - KG//,", result[0].Itens[0].Erro);

            Assert.AreEqual(0, log.Criticas);
            Assert.AreEqual(1, log.Invalidos);
        }

        #endregion

        #region Pipe

        [Test]
        [Category("ImportacaoInventario")]
        public void LerArquivo_Pipe_Ok()
        {
            LogMock log = new LogMock();
            ParametroMock parametro = new ParametroMock(120, 120);

            string root = Path.GetDirectoryName(typeof(LogMock).Assembly.Location);
            string pathRelative = @"Importing\Inventario\ArquivosTeste\Pipe\140";
            string[] files = new string[] 
            { 
                @"CINV0076.CAT.STARTED.04051201",
                @"CINV0080.ITM.FINALIZED.01261130",
                @"DESCOMPACTADOS\CINV0097.ITM.FINALIZED.02241805",
                @"DESCOMPACTADOS\CINV0098.ITM.FINALIZED.02241830",
            };

            var arquivos = files.Select(f => Path.Combine(root, pathRelative, f));

            ILeitorArquivoInventario target = new LeitorArquivoInventario(log, parametro);

            var result = target.LerArquivosPipe(TipoProcessoImportacao.Automatico, TipoOrigemImportacao.Loja, 1, 999, arquivos, new DateTime(2016, 04, 04)).ToArray();

            Assert.AreEqual(4, result.Length);

            Assert.IsTrue(result[0].IsArquivoValido);
            Assert.IsTrue(result[1].IsArquivoValido);
            Assert.IsTrue(result[2].IsArquivoValido);
            Assert.IsTrue(result[3].IsArquivoValido);

            Assert.AreEqual(TipoArquivoInventario.Final, result[0].TipoArquivo);
            Assert.AreEqual(TipoArquivoInventario.Final, result[1].TipoArquivo);
            Assert.AreEqual(TipoArquivoInventario.Final, result[2].TipoArquivo);
            Assert.AreEqual(TipoArquivoInventario.Final, result[3].TipoArquivo);

            var result04051201 = result.Single(a => a.NomeArquivo == @"CINV0076.CAT.STARTED.04051201");

            Assert.AreEqual(140, result04051201.CdLoja);
            Assert.AreEqual(new DateTime(2016, 04, 04), result04051201.DataArquivo);
            Assert.AreEqual(new DateTime(2016, 04, 04), result04051201.DataInventario);
            Assert.AreEqual(999, result04051201.IdLojaImportacao);
            Assert.IsTrue(result04051201.IsArquivoValido);
            Assert.IsNull(result04051201.IsMultiDepartamento);
            Assert.AreEqual(24, result04051201.Itens.Count());
            Assert.IsTrue(result04051201.LeuCabecalho);
            Assert.AreEqual("CINV0076.CAT.STARTED.04051201", result04051201.NomeArquivo);
            Assert.AreEqual(TipoArquivoInventario.Final, result04051201.TipoArquivo);
            Assert.IsNull(result04051201.TotalEstoqueFinal);
            Assert.IsNull(result04051201.TotalItensContados);
            Assert.AreEqual(76, result04051201.UltimoCdDepartamentoLido);

            var item = result04051201.Itens.First();

            Assert.AreEqual(76, item.CdDepartamento);
            Assert.AreEqual(141008, item.CdItem);
            Assert.IsNull(item.CdUpc);
            Assert.IsNull(item.Completo);
            Assert.IsNull(item.CustoUnitario);
            Assert.IsNull(item.DescricaoItem);
            Assert.IsNull(item.DescricaoTamanho);
            Assert.IsNull(item.Erro);
            Assert.IsNull(item.NumeroEstoque);
            Assert.IsNull(item.PrecoUnitarioVarejo);
            Assert.AreEqual(240.886m, item.QtItem);
            Assert.IsNull(item.QuantidadeDif);
            Assert.IsNull(item.QuantidadeOnHand);
            Assert.IsNull(item.Tamanho);
            Assert.IsNull(item.TotalAumentadoContg);
            Assert.IsNull(item.TotalAumentadoDif);
            Assert.IsNull(item.TotalAumentadoOnHand);
            Assert.AreEqual(new DateTime(2016, 04, 04, 17, 52, 21), item.UltimaContagem);

            item = result04051201.Itens.Last();

            Assert.AreEqual(76, item.CdDepartamento);
            Assert.AreEqual(756341, item.CdItem);
            Assert.IsNull(item.CdUpc);
            Assert.IsNull(item.Completo);
            Assert.IsNull(item.CustoUnitario);
            Assert.IsNull(item.DescricaoItem);
            Assert.IsNull(item.DescricaoTamanho);
            Assert.IsNull(item.Erro);
            Assert.IsNull(item.NumeroEstoque);
            Assert.IsNull(item.PrecoUnitarioVarejo);
            Assert.AreEqual(1119.925m, item.QtItem);
            Assert.IsNull(item.QuantidadeDif);
            Assert.IsNull(item.QuantidadeOnHand);
            Assert.IsNull(item.Tamanho);
            Assert.IsNull(item.TotalAumentadoContg);
            Assert.IsNull(item.TotalAumentadoDif);
            Assert.IsNull(item.TotalAumentadoOnHand);
            Assert.AreEqual(new DateTime(2016, 04, 04, 17, 27, 39), item.UltimaContagem);

            Assert.AreEqual(0, log.Criticas);
            Assert.AreEqual(0, log.Invalidos);
        }

        [Test]
        [Category("ImportacaoInventario")]
        public void LerArquivo_Pipe_DataForaIntervaloInvalido()
        {
            LogMock log = new LogMock();
            ParametroMock parametro = new ParametroMock(1, 1);

            string root = Path.GetDirectoryName(typeof(LogMock).Assembly.Location);
            string pathRelative = @"Importing\Inventario\ArquivosTeste\Pipe\140";
            string[] files = new string[] 
            { 
                @"CINV0080.ITM.FINALIZED.01261130",
            };

            var arquivos = files.Select(f => Path.Combine(root, pathRelative, f));

            ILeitorArquivoInventario target = new LeitorArquivoInventario(log, parametro);

            var result = target.LerArquivosPipe(TipoProcessoImportacao.Automatico, TipoOrigemImportacao.Loja, 1, 999, arquivos, new DateTime(2016, 04, 04)).ToArray();

            Assert.AreEqual(1, result.Length);

            Assert.IsFalse(result[0].IsArquivoValido);

            Assert.AreEqual(0, log.Criticas);

            // nao foi emitida nenhuma mensagem de inválido, nem teve exception, apenas ignorou arquivo
            Assert.AreEqual(0, log.Invalidos);
        }

        #endregion
    }
}
