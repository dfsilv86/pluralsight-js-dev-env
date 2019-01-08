using System;
using System.Collections.Generic;
using System.IO;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Infrastructure.IO.Importing.Inventario
{
    /// <summary>
    /// Classe base para um leitor de arquivo no formato Rtl.
    /// </summary>
    public abstract class LeitorArquivoInventarioRtlBase : LeitorArquivoInventarioBase
    {
        #region Constructor

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="LeitorArquivoInventarioRtlBase"/> class.
        /// </summary>
        /// <param name="tipoProcesso">O tipo de processo de importação (automático ou manual).</param>
        /// <param name="tipoOrigem">O tipo de origem da importação (loja ou HO).</param>
        /// <param name="idLojaImportacao">O id da loja a qual o arquivo de inventário pertence.</param>
        /// <param name="arquivos">Os arquivos.</param>
        /// <param name="dataInventario">A data de inventario.</param>
        /// <param name="qtdDiasInventario">A quantidade de dias permitida.</param>
        /// <param name="logger">O logger.</param>
        protected LeitorArquivoInventarioRtlBase(TipoProcessoImportacao tipoProcesso, TipoOrigemImportacao tipoOrigem, int idLojaImportacao, IEnumerable<string> arquivos, DateTime dataInventario, int? qtdDiasInventario, ILeitorLogger logger)
            : base(tipoProcesso, tipoOrigem, idLojaImportacao, arquivos, dataInventario, qtdDiasInventario, logger)
        {
        }

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
            // Formato:
            // 1 ou mais páginas onde página é:
            //   3 linhas em branco
            //   Linha cabeçalho com data e cod loja
            //   Linha cabeçalho com departamento (ou multidepartamento)
            //   Linha cabeçalho com informação sobre ser parcial ou nao
            //   Linha em branco
            //   Linha cabeçalho da grid
            //   Segunda linha cabeçalho da grid
            //   Linha em branco
            //   1 ou mais itens onde item é:
            //     Linha cabeçalho do item
            //     1 ou mais linhas de item
            //     Linha separador dos subtotais (com '-')
            //     Linha de subtotais do item
            //   Quebra de página (fim de página; volta pro inicio) OU separador dos totais (com '='), que inicia o rodapé do arquivo, composto por:
            //     n linhas sobre totalizadores OU quebra de página
            //     1 linha com totais (identificada pela substring 'Cust')
            //     n linhas sobre totalizadores OU quebra de página
            //     1 linha com número de itens lidos (identificada pela substring 'tens contados =')
            FileInfo arquivoInfo = new FileInfo(caminhoArquivo);
            string nomeArquivo = arquivoInfo.Name.ToString();

            ArquivoInventario result = new ArquivoInventario(this.IdLojaImportacao, nomeArquivo, this.DataInventario);

            this.Logger.InserirLogProcessamento(Texts.ImportLogInicioLeituraArquivo, Texts.ImportLogInicioLeituraArquivoDetalhe.With(result.NomeArquivo, DateTime.Now));

            using (StatefulFileReader reader = new StatefulFileReader(caminhoArquivo))
            {
                try
                {
                    while (true)
                    {
                        if (!LerCabecalhoPagina(reader, result))
                        {
                            break;
                        }

                        LerCabecalhoGrid(reader, result);
                        LerItens(reader, result);

                        if (LerRodapeArquivo(reader, result))
                        {
                            break;
                        }
                    }

                    result.IsArquivoValido = true;
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
                    this.Logger.InserirLogProcessamento(Texts.ImportLogArquivoInvalido, Texts.InvalidFileContents.With(result.NomeArquivo));
                    result.IsArquivoValido = false;
                }
            }

            this.Logger.InserirLogProcessamento(Texts.ImportLogFinalLeituraArquivo, Texts.ImportLogFinalLeituraArquivoDetalhe.With(result.NomeArquivo, DateTime.Now));

            return result;
        }

        /// <summary>
        /// Interpreta as linhas do cabeçalho do arquivo.
        /// </summary>
        /// <param name="linha1">A primeira linha.</param>
        /// <param name="linha2">A segunda linha.</param>
        /// <param name="linha3">A terceira linha.</param>
        /// <param name="arquivo">Os dados do arquivo sendo lido.</param>
        /// <returns>True caso arquivo válido.</returns>
        protected abstract bool LerLinhasCabecalho(string linha1, string linha2, string linha3, ArquivoInventario arquivo);

        /// <summary>
        /// Interpreta a linha com o código de departamento.
        /// </summary>
        /// <param name="linha">A linha.</param>
        /// <param name="arquivo">Os dados do arquivo sendo lido.</param>
        protected abstract void LerLinhaDepartamento(string linha, ArquivoInventario arquivo);

        /// <summary>
        /// Interpreta as linha do item no arquivo.
        /// </summary>
        /// <param name="linhaInicio">A primeira linha do item, contendo o seu código.</param>
        /// <param name="linhasItem">As linhas seguintes do item, contendo a quantidade.</param>
        /// <param name="linhaFim">A linha final do item, contendo os totais.</param>
        /// <param name="arquivo">Os dados do arquivo sendo lido.</param>
        protected abstract void LerLinhasItem(string linhaInicio, string[] linhasItem, string linhaFim, ArquivoInventario arquivo);

        /// <summary>
        /// Interpreta a linha com o contador no rodapé do arquivo.
        /// </summary>
        /// <param name="leitor">O leitor do arquivo.</param>
        /// <param name="arquivo">Os dados do arquivo sendo lido.</param>
        /// <returns>True caso seja a última linha a ser lida do arquivo.</returns>
        protected abstract bool LerLinhaContador(StatefulFileReader leitor, ArquivoInventario arquivo);

        /// <summary>
        /// Interpreta a linha com os totais gerais do arquivo.
        /// </summary>
        /// <param name="leitor">O leitor do arquivo.</param>
        /// <param name="arquivo">Os dados do arquivo sendo lido.</param>
        protected abstract void LerLinhaTotais(StatefulFileReader leitor, ArquivoInventario arquivo);

        /// <summary>
        /// Verifica se a linha atual é a primeira de um rodapé de item.
        /// </summary>
        /// <param name="leitor">O leitor do arquivo.</param>
        /// <returns>True caso linha inicie o rodapé.</returns>
        protected abstract bool TestarInicioRodape(StatefulFileReader leitor);

        /// <summary>
        /// Verifica se a linha atual é a linha com o código de departamento.
        /// </summary>
        /// <param name="leitor">O leitor do arquivo.</param>
        /// <returns>True caso seja a linha com o departamento.</returns>
        protected abstract bool TestarLinhaDepartamento(StatefulFileReader leitor);

        private static void LerLinhaEmBranco(StatefulFileReader leitor, ArquivoInventario arquivo)
        {
            if (!string.IsNullOrWhiteSpace(leitor.CurrentLine))
            {
                throw new ImportacaoArquivoInventarioException(Texts.ImportLogArquivoInvalido, Texts.InvalidFileContents.With(arquivo.NomeArquivo));
            }

            leitor.NextLine();
        }

        private static void LerCabecalhoGrid(StatefulFileReader leitor, ArquivoInventario arquivo)
        {
            LerLinhaEmBranco(leitor, arquivo);

            string cabecalhoGrid1 = LerLinhaPreenchida(leitor, arquivo);

            bool isArquivoValido = cabecalhoGrid1.Contains("Cust");

            if (!isArquivoValido)
            {
                throw new ImportacaoArquivoInventarioException(Texts.ImportLogArquivoInvalido, Texts.InvalidFileContents.With(arquivo.NomeArquivo));
            }

            LerLinhaPreenchida(leitor, arquivo);

            LerLinhaEmBranco(leitor, arquivo);
        }

        private static string LerLinhaPreenchida(StatefulFileReader leitor, ArquivoInventario arquivo)
        {
            string result = leitor.CurrentLine;

            if (string.IsNullOrEmpty(result))
            {
                throw new ImportacaoArquivoInventarioException(Texts.ImportLogArquivoInvalido, Texts.InvalidFileContents.With(arquivo.NomeArquivo));
            }

            leitor.NextLine();

            return result;
        }

        private bool LerCabecalhoPagina(StatefulFileReader leitor, ArquivoInventario arquivo)
        {
            LerLinhaEmBranco(leitor, arquivo);
            LerLinhaEmBranco(leitor, arquivo);
            LerLinhaEmBranco(leitor, arquivo);

            string cabecalhoArquivo1 = LerLinhaPreenchida(leitor, arquivo);

            string cabecalhoArquivo2 = LerLinhaPreenchida(leitor, arquivo);

            string cabecalhoArquivo3 = LerLinhaPreenchida(leitor, arquivo);

            if (!arquivo.LeuCabecalho)
            {
                bool dataValida = this.LerLinhasCabecalho(cabecalhoArquivo1, cabecalhoArquivo2, cabecalhoArquivo3, arquivo);
                arquivo.LeuCabecalho = true;

                return dataValida;
            }

            return true;
        }

        private void LerItens(StatefulFileReader leitor, ArquivoInventario arquivo)
        {
            while (true)
            {
                if (leitor.CurrentLine.Length < 6)
                {
                    break;
                }

                if (this.TestarLinhaDepartamento(leitor))
                {
                    string deptoLine = LerLinhaPreenchida(leitor, arquivo);

                    this.LerLinhaDepartamento(deptoLine, arquivo);

                    LerLinhaEmBranco(leitor, arquivo);
                }

                if (this.TestarInicioRodape(leitor))
                {
                    break;
                }

                string linhaInicioItem = LerLinhaPreenchida(leitor, arquivo);

                List<string> linhas = new List<string>();

                while (leitor.CurrentLine.Length > 2)
                {
                    // Cuidado, exatamente nestas posicoes pode ter um valor negativo com -
                    if (leitor.CurrentLine.Length > 108 && leitor.CurrentLine[107] == '-' && leitor.CurrentLine[108] == '-')
                    {
                        break;
                    }

                    string linhaItem = leitor.CurrentLine;

                    linhas.Add(linhaItem);

                    leitor.NextLine();
                }

                leitor.NextLine();

                string linhaTotais = LerLinhaPreenchida(leitor, arquivo);

                this.LerLinhasItem(linhaInicioItem, linhas.ToArray(), linhaTotais, arquivo);
            }
        }

        private bool LerRodapeArquivo(StatefulFileReader leitor, ArquivoInventario arquivo)
        {
            if (!this.TestarInicioRodape(leitor))
            {
                return false;
            }

            while (leitor.NextLine())
            {
                this.LerLinhaTotais(leitor, arquivo);

                if (this.LerLinhaContador(leitor, arquivo))
                {
                    return true;
                }
            }

            throw new ImportacaoArquivoInventarioException(Texts.ImportLogArquivoInvalido, Texts.InvalidFileContents.With(arquivo.NomeArquivo));
        }

        #endregion
    }
}
