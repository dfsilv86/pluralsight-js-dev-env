using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Infrastructure.IO.Importing.Inventario
{
    /// <summary>
    /// Leitor de arquivo RTL para sistema Supercenter.
    /// </summary>
    public class LeitorArquivoInventarioRtlSupercenter : LeitorArquivoInventarioRtlBase
    {
        #region Constructors

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="LeitorArquivoInventarioRtlSupercenter"/>.
        /// </summary>
        /// <param name="tipoProcesso">O tipo de processo de importação (automático ou manual).</param>
        /// <param name="tipoOrigem">O tipo de origem da importação (loja ou HO).</param>
        /// <param name="idLojaImportacao">O id da loja a qual o arquivo de inventário pertence.</param>
        /// <param name="arquivo">O arquivo.</param>
        /// <param name="dataInventario">A data de inventario.</param>
        /// <param name="qtdDiasInventario">A quantidade de dias permitida.</param>
        /// <param name="logger">O logger.</param>
        public LeitorArquivoInventarioRtlSupercenter(TipoProcessoImportacao tipoProcesso, TipoOrigemImportacao tipoOrigem, int idLojaImportacao, string arquivo, DateTime dataInventario, int qtdDiasInventario, ILeitorLogger logger)
            : this(tipoProcesso, tipoOrigem, idLojaImportacao, new string[] { arquivo }, dataInventario, qtdDiasInventario, logger)
        {
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="LeitorArquivoInventarioRtlSupercenter"/>.
        /// </summary>
        /// <param name="tipoProcesso">O tipo de processo de importação (automático ou manual).</param>
        /// <param name="tipoOrigem">O tipo de origem da importação (loja ou HO).</param>
        /// <param name="idLojaImportacao">O id da loja a qual o arquivo de inventário pertence.</param>
        /// <param name="arquivos">Os arquivos.</param>
        /// <param name="dataInventario">A data de inventario.</param>
        /// <param name="qtdDiasInventario">A quantidade de dias permitida.</param>
        /// <param name="logger">O logger.</param>
        public LeitorArquivoInventarioRtlSupercenter(TipoProcessoImportacao tipoProcesso, TipoOrigemImportacao tipoOrigem, int idLojaImportacao, IEnumerable<string> arquivos, DateTime dataInventario, int qtdDiasInventario, ILeitorLogger logger)
            : base(tipoProcesso, tipoOrigem, idLojaImportacao, arquivos, dataInventario, qtdDiasInventario, logger)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Interpreta as linhas do cabeçalho do arquivo.
        /// </summary>
        /// <param name="linha1">A primeira linha.</param>
        /// <param name="linha2">A segunda linha.</param>
        /// <param name="linha3">A terceira linha.</param>
        /// <param name="arquivo">Os dados do arquivo sendo lido.</param>
        /// <returns>True caso arquivo válido.</returns>
        protected override bool LerLinhasCabecalho(string linha1, string linha2, string linha3, ArquivoInventario arquivo)
        {
            if (linha1.Length > 156 && new string[] { "RODD", "RODA" }.Contains(linha1.Substring(0, 8).Trim()))
            {
                string dt = linha1.Substring(11, 8);
                string hr = linha1.Substring(23, 8);
                
                // string tmp = dt.Substring(3, 2) + "/" + dt.Substring(0, 2) + "/20" + dt.Substring(6, 2) + " " + hr;
                string tmp = "20{0:N2}-{1:N2}-{2:N2} {3}".With(dt.Substring(6, 2), dt.Substring(0, 2), dt.Substring(3, 2), hr);

                arquivo.DataArquivo = DateTime.Parse(tmp, CultureInfo.InvariantCulture);

                // linha1.Substring(143, 8) == "LOJA NO."                
                // no legado, se não for "LOJA NO.", le a string "LOJA NO." para uma variavel e no final do processo tenta dar int.Parse nela, abortando tudo
                // apos abortar, mais tarde, a comparacao dentre CdLoja == inventario.Loja.CdLoja falha e não faz nada.
                // aqui, tenta dar int.Parse no inicio do processo e retorna arquivo.IsArquivoValido = false se falhar
                arquivo.CdLoja = int.Parse(linha1.Substring(152, 5).Trim(), CultureInfo.InvariantCulture);

                if (!this.IsDataArquivoDentroDoIntervalo(arquivo))
                {
                    return false;
                }
            }
            else
            {
                throw new ImportacaoArquivoInventarioException(Texts.ImportLogArquivoInvalido, Texts.InvalidFileContents.With(arquivo.NomeArquivo), Texts.InvalidFileContentsCritic.With(arquivo.NomeArquivo));
            }

            bool isMultiDepartamento = linha2.Contains("MULTI");
            arquivo.IsMultiDepartamento = isMultiDepartamento;

            if (isMultiDepartamento)
            {
                throw new ImportacaoArquivoInventarioException(Texts.ImportLogArquivoInvalido, Texts.InvalidMultiDepartmentInventoryFile.With(arquivo.NomeArquivo));
            }

            bool isArquivoFinal = linha3.Length > 105 && linha3.Substring(94).Contains("RELAT FINAL");

            if (!isArquivoFinal)
            {
                throw new ImportacaoArquivoInventarioException(Texts.ImportLogArquivoInvalido, Texts.FileKindIsNotFinal.With(arquivo.NomeArquivo));
            }
            else
            {
                this.Logger.InserirLogProcessamento(Texts.ImportLogArquivoValido, Texts.InventoryFileKindNotice.With(arquivo.NomeArquivo, "FINAL"));

                arquivo.TipoArquivo = TipoArquivoInventario.Final;
            }

            return true;
        }

        /// <summary>
        /// Interpreta a linha com o código de departamento.
        /// </summary>
        /// <param name="linha">A linha.</param>
        /// <param name="arquivo">Os dados do arquivo sendo lido.</param>
        protected override void LerLinhaDepartamento(string linha, ArquivoInventario arquivo)
        {
            int dpto = int.Parse(linha.Substring(6).Trim(), CultureInfo.InvariantCulture);

            arquivo.UltimoCdDepartamentoLido = dpto;
        }

        /// <summary>
        /// Interpreta as linha do item no arquivo.
        /// </summary>
        /// <param name="linhaInicio">A primeira linha do item, contendo o seu código.</param>
        /// <param name="linhasItem">As linhas seguintes do item, contendo a quantidade.</param>
        /// <param name="linhaFim">A linha final do item, contendo os totais.</param>
        /// <param name="arquivo">Os dados do arquivo sendo lido.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected override void LerLinhasItem(string linhaInicio, string[] linhasItem, string linhaFim, ArquivoInventario arquivo)
        {
            ArquivoInventarioItem item = new ArquivoInventarioItem();

            item.CdDepartamento = arquivo.UltimoCdDepartamentoLido;

            try
            {
                // primeira linha
                item.CdItem = long.Parse(linhaInicio.Substring(4, 10).Trim(), CultureInfo.InvariantCulture);

                string tmpUpc = linhaInicio.Substring(16, 2).Trim();
                long notUsed;
                if (long.TryParse(tmpUpc, out notUsed))
                {
                    item.CdUpc = long.Parse(linhaInicio.Substring(15, 15).Trim(), CultureInfo.InvariantCulture);
                }

                item.Tamanho = linhaInicio.Substring(32, 16).Trim();
                item.NumeroEstoque = linhaInicio.Substring(48, 21).Trim();
                item.CustoUnitario = decimal.Parse(linhaInicio.Substring(70, 9).Trim().Replace("$", string.Empty), CultureInfo.InvariantCulture);

                // primeira sublinha
                string linhaDescricao = linhasItem.First();

                item.DescricaoItem = linhaDescricao.Substring(4, 26).Trim();

                item.DescricaoTamanho = linhaDescricao.Substring(31, 16).Trim();

                item.PrecoUnitarioVarejo = decimal.Parse(linhaDescricao.Substring(70, 9).Trim().Replace("$", string.Empty), CultureInfo.InvariantCulture);

                // ultima sublinha
                string linhaUsuario = linhasItem.Last();

                item.Completo = linhaUsuario.Substring(187, 7).Trim();

                item.UltimaContagem = DateTime.Parse(linhaUsuario.Substring(83, 16).Trim(), CultureInfo.InvariantCulture);

                // ultima linha
                item.QtItem = decimal.Parse(linhaFim.Substring(105, 12).Trim(), CultureInfo.InvariantCulture);
                item.TotalAumentadoContg = decimal.Parse(linhaFim.Substring(119, 13).Trim().Replace("$", string.Empty), CultureInfo.InvariantCulture);

                if (linhaFim.Substring(134, 2).ToString().Trim() == "**")
                {
                    item.QuantidadeOnHand = 0;
                }
                else
                {
                    item.QuantidadeOnHand = decimal.Parse(linhaFim.Substring(134, 10).Trim(), CultureInfo.InvariantCulture);
                }

                if (linhaFim.Substring(146, 2).ToString().Trim() == "**")
                {
                    item.TotalAumentadoOnHand = 0m;
                }
                else
                {
                    item.TotalAumentadoOnHand = decimal.Parse(linhaFim.Substring(146, 13).Trim().Replace("$", string.Empty), CultureInfo.InvariantCulture);
                }

                if (linhaFim.Substring(161, 2).ToString().Trim() == "**")
                {
                    item.QuantidadeDif = 0;
                }
                else
                {
                    item.QuantidadeDif = decimal.Parse(linhaFim.Substring(161, 10).Trim(), CultureInfo.InvariantCulture);
                }

                if (linhaFim.Substring(173, 2).ToString().Trim() == "**")
                {
                    item.TotalAumentadoDif = 0m;
                }
                else
                {
                    item.TotalAumentadoDif = decimal.Parse(linhaFim.Substring(linhaFim.Length - 13, 13).Trim().Replace("$", string.Empty), CultureInfo.InvariantCulture);
                }
            }
            catch (Exception ex)
            {
                string msg = @"O arquivo '{0}' é inválido, não foi possivel ler o item : - {1}, - {2}, - {3}, - {4}, - {5}, - {6}, - {7}, - {8}, - {9}, - {10}, - {11}, - {12}, - {13}, -{14} - {15},".With(
                    arquivo.NomeArquivo,
                    item.CdItem,
                    item.CdUpc,
                    item.DescricaoItem,
                    item.NumeroEstoque,
                    item.CustoUnitario,
                    item.PrecoUnitarioVarejo,
                    item.UltimaContagem,
                    item.TotalAumentadoContg,
                    item.QtItem,
                    item.TotalAumentadoOnHand,
                    item.QuantidadeOnHand,
                    item.QuantidadeDif,
                    item.TotalAumentadoDif,
                    item.Completo,
                    item.Tamanho + "/" + item.DescricaoTamanho);

                this.Logger.InserirLogProcessamento(Texts.ImportLogArquivoInvalido, msg);

                item.Erro = ex.ToString();
            }

            arquivo.Itens.Add(item);
        }

        /// <summary>
        /// Interpreta a linha com o contador no rodapé do arquivo.
        /// </summary>
        /// <param name="leitor">O leitor do arquivo.</param>
        /// <param name="arquivo">Os dados do arquivo sendo lido.</param>
        /// <returns>True caso seja a última linha a ser lida do arquivo.</returns>
        protected override bool LerLinhaContador(StatefulFileReader leitor, ArquivoInventario arquivo)
        {
            if (leitor.CurrentLine.Length > 33 && leitor.CurrentLine.Substring(20, 15) == "tens Contads = ")
            {
                arquivo.TotalItensContados = int.Parse(leitor.CurrentLine.Substring(34).Trim(), CultureInfo.InvariantCulture);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Interpreta a linha com os totais gerais do arquivo.
        /// </summary>
        /// <param name="leitor">O leitor do arquivo.</param>
        /// <param name="arquivo">Os dados do arquivo sendo lido.</param>
        protected override void LerLinhaTotais(StatefulFileReader leitor, ArquivoInventario arquivo)
        {
            if (leitor.CurrentLine.Length > 100 && leitor.CurrentLine.Substring(89, 4) == "Cust")
            {
                arquivo.TotalEstoqueFinal = decimal.Parse(leitor.CurrentLine.Substring(118, 13).Replace("$", string.Empty), CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Verifica se a linha atual é a primeira de um rodapé de item.
        /// </summary>
        /// <param name="leitor">O leitor do arquivo.</param>
        /// <returns>True caso linha inicie o rodapé.</returns>
        protected override bool TestarInicioRodape(StatefulFileReader leitor)
        {
            return leitor.CurrentLine.Length > 111 && leitor.CurrentLine[111] == '=';
        }

        /// <summary>
        /// Verifica se a linha atual é a linha com o código de departamento.
        /// </summary>
        /// <param name="leitor">O leitor do arquivo.</param>
        /// <returns>True caso seja a linha com o departamento.</returns>
        protected override bool TestarLinhaDepartamento(StatefulFileReader leitor)
        {
            return new string[] { "Depto:", "Dept:" }.Contains(leitor.CurrentLine.Substring(0, 6).Trim());
        }

        #endregion
    }
}
