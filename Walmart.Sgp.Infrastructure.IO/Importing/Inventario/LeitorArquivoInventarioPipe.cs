using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Infrastructure.IO.Importing.Inventario
{
    /// <summary>
    /// Leitor de arquivo de inventário no formato pipe.
    /// </summary>
    /// <remarks>
    /// Utilizado por ambos sistemas.
    /// </remarks>
    public class LeitorArquivoInventarioPipe : LeitorArquivoInventarioBase
    {
        #region Constructor

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="LeitorArquivoInventarioPipe"/>.
        /// </summary>
        /// <param name="tipoProcesso">O tipo de processo de importação (automático ou manual).</param>
        /// <param name="tipoOrigem">O tipo de origem da importação (loja ou HO).</param>
        /// <param name="codigoSistema">O código de sistema (Supercenter ou Sams).</param>
        /// <param name="idLojaImportacao">O id da loja a qual o arquivo de inventário pertence.</param>
        /// <param name="arquivos">Os arquivos.</param>
        /// <param name="dataInventario">A data de inventario.</param>
        /// <param name="qtdDiasInventario">A quantidade de dias permitida.</param>
        /// <param name="logger">O logger.</param>
        public LeitorArquivoInventarioPipe(TipoProcessoImportacao tipoProcesso, TipoOrigemImportacao tipoOrigem, int codigoSistema, int idLojaImportacao, IEnumerable<string> arquivos, DateTime dataInventario, int? qtdDiasInventario, ILeitorLogger logger)
            : base(tipoProcesso, tipoOrigem, idLojaImportacao, arquivos, dataInventario, qtdDiasInventario, logger)
        {
            this.CdSistema = codigoSistema;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Obtém o código do sistema.
        /// </summary>
        public int CdSistema { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Lê o arquivo especificado.
        /// </summary>
        /// <param name="caminhoArquivo">O caminho para o arquivo.</param>
        /// <returns>
        /// As informações do arquivo.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected override ArquivoInventario LerArquivo(string caminhoArquivo)
        {
            FileInfo arquivoInfo = new FileInfo(caminhoArquivo);
            string nomeArquivo = arquivoInfo.Name.ToString();

            int codigoLoja;
            if (arquivoInfo.Directory.Name.ToUpper(CultureInfo.InvariantCulture) == "DESCOMPACTADOS")
            {
                codigoLoja = int.Parse(arquivoInfo.Directory.Parent.Name, CultureInfo.InvariantCulture);
            }
            else
            {
                codigoLoja = int.Parse(arquivoInfo.Directory.Name, CultureInfo.InvariantCulture);
            }

            ArquivoInventario result = new ArquivoInventario(this.IdLojaImportacao, nomeArquivo, this.DataInventario);
            result.CdLoja = codigoLoja;
            result.TipoArquivo = TipoArquivoInventario.Final;

            this.Logger.InserirLogProcessamento(Texts.ImportLogInicioLeituraArquivo, Texts.ImportLogInicioLeituraArquivoDetalhe.With(result.NomeArquivo, DateTime.Now));

            using (StatefulFileReader reader = new StatefulFileReader(caminhoArquivo))
            {
                try
                {
                    bool dataValida = LerPrimeiraLinha(reader, result);

                    result.LeuCabecalho = true;

                    if (dataValida)
                    {
                        LerLinhasItem(reader, result);

                        result.IsArquivoValido = true;
                    }
                }
                catch (ImportacaoArquivoInventarioException iaie)
                {
                    this.Logger.InserirLogProcessamento(iaie.Action, iaie.Message);

                    if (!string.IsNullOrWhiteSpace(iaie.Message2))
                    {
                        this.Logger.InserirInventarioCritica(result.IdLojaImportacao, iaie.Message2, 5, null, null, null, result.DataInventario);
                    }

                    result.IsArquivoValido = false;
                }
                catch (Exception)
                {
                    this.Logger.InserirLogProcessamento(Texts.ImportLogArquivoInvalido, "O arquivo '{0}' é inválido.".With(result.NomeArquivo));
                    result.IsArquivoValido = false;
                }
            }

            this.Logger.InserirLogProcessamento(Texts.ImportLogFinalLeituraArquivo, Texts.ImportLogFinalLeituraArquivoDetalhe.With(result.NomeArquivo, DateTime.Now));

            return result;
        }

        private bool LerPrimeiraLinha(StatefulFileReader reader, ArquivoInventario result)
        {
            result.DataArquivo = DateTime.Parse(reader.CurrentLine.Split('|')[7], CultureInfo.InvariantCulture).Date;

            reader.NextLine();

            if (!this.IsDataArquivoDentroDoIntervalo(result))
            {
                return false;
            }

            return true;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void LerLinhasItem(StatefulFileReader reader, ArquivoInventario result)
        {
            while (reader.CurrentLine != null)
            {
                string[] linha = reader.CurrentLine.Split('|');

                int? codigoItem = null;
                int? codigoDepartamento = null;
                DateTime? dataContagem = null;
                decimal? qtdItem = null;
                ArquivoInventarioItem item = null;

                try
                {
                    codigoItem = int.Parse(linha[0], CultureInfo.InvariantCulture);
                    item = result.Itens.SingleOrDefault(i => i.CdItem == codigoItem);

                    if (null == item)
                    {
                        item = new ArquivoInventarioItem() { CdItem = codigoItem.Value, QtItem = 0 };
                        result.Itens.Add(item);
                    }

                    codigoDepartamento = int.Parse(linha[1], CultureInfo.InvariantCulture);
                    dataContagem = DateTime.Parse(linha[7], CultureInfo.InvariantCulture);
                    qtdItem = decimal.Parse(linha[6], CultureInfo.InvariantCulture) / 1000;

                    item.QtItem += qtdItem.Value;
                    item.UltimaContagem = dataContagem.Value;
                    item.CdDepartamento = codigoDepartamento;

                    result.UltimoCdDepartamentoLido = codigoDepartamento;
                }
                catch (Exception ex)
                {
                    this.Logger.InserirLogProcessamento(Texts.ImportLogArquivoInvalido, @"O arquivo '{0}' é inválido, não foi possivel ler o item : - {1}, - {2}, - {3}, - {4},".With(result.NomeArquivo, codigoItem, dataContagem, qtdItem));

                    if (null != item)
                    {
                        item.Erro = ex.ToString();
                    }
                }

                reader.NextLine();
            }
        }

        #endregion
    }
}
