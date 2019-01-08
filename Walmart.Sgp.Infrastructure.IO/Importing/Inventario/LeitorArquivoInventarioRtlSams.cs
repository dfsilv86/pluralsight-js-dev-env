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
    /// Leitor de arquivo RTL para sistema Sam's.
    /// </summary>
    public class LeitorArquivoInventarioRtlSams : LeitorArquivoInventarioRtlBase
    {
        #region Constructors

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="LeitorArquivoInventarioRtlSams"/>.
        /// </summary>
        /// <param name="tipoProcesso">O tipo de processo de importação (automático ou manual).</param>
        /// <param name="tipoOrigem">O tipo de origem da importação (loja ou HO).</param>
        /// <param name="idLojaImportacao">O id da loja a qual o arquivo de inventário pertence.</param>
        /// <param name="arquivo">O arquivo.</param>
        /// <param name="tipoArquivo">O tipo de arquivo.</param>
        /// <param name="dataInventario">A data de inventario.</param>
        /// <param name="logger">O logger.</param>
        public LeitorArquivoInventarioRtlSams(TipoProcessoImportacao tipoProcesso, TipoOrigemImportacao tipoOrigem, int idLojaImportacao, string arquivo, TipoArquivoInventario tipoArquivo, DateTime dataInventario, ILeitorLogger logger)
            : this(tipoProcesso, tipoOrigem, idLojaImportacao, new string[] { arquivo }, tipoArquivo, dataInventario, logger)
        {
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="LeitorArquivoInventarioRtlSams"/>.
        /// </summary>
        /// <param name="tipoProcesso">O tipo de processo de importação (automático ou manual).</param>
        /// <param name="tipoOrigem">O tipo de origem da importação (loja ou HO).</param>
        /// <param name="idLojaImportacao">O id da loja a qual o arquivo de inventário pertence.</param>
        /// <param name="arquivos">Os arquivos.</param>
        /// <param name="tipoArquivo">O tipo de arquivo.</param>
        /// <param name="dataInventario">A data de inventario.</param>
        /// <param name="logger">O logger.</param>
        public LeitorArquivoInventarioRtlSams(TipoProcessoImportacao tipoProcesso, TipoOrigemImportacao tipoOrigem, int idLojaImportacao, IEnumerable<string> arquivos, TipoArquivoInventario tipoArquivo, DateTime dataInventario, ILeitorLogger logger)
            : base(tipoProcesso, tipoOrigem, idLojaImportacao, arquivos, dataInventario, null, logger)
        {
            this.TipoArquivo = tipoArquivo;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Obtém o tipo de arquivo.
        /// </summary>
        public TipoArquivoInventario TipoArquivo { get; private set; }

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
            arquivo.TipoArquivo = this.TipoArquivo;

            if (linha1.Length > 156 && new string[] { "RODD", "RODA" }.Contains(linha1.Substring(0, 8).Trim()))
            {
                string dt = linha1.Substring(11, 8);
                string hr = linha1.Substring(23, 8);
                
                // string tmp = dt.Substring(3, 2) + "/" + dt.Substring(0, 2) + "/20" + dt.Substring(6, 2) + " " + hr;
                string tmp = "20{0:N2}-{1:N2}-{2:N2} {3}".With(dt.Substring(6, 2), dt.Substring(0, 2), dt.Substring(3, 2), hr);

                arquivo.DataArquivo = DateTime.Parse(tmp, CultureInfo.InvariantCulture);

                // linha1.Substring(143, 8) == "CLUB NO."                
                // no legado, se não for "CLUB NO.", le a string "CLUB NO." para uma variavel e no final do processo tenta dar int.Parse nela, abortando tudo
                // apos abortar, mais tarde, a comparacao dentre CdLoja == inventario.Loja.CdLoja falha e não faz nada.
                // aqui, tenta dar int.Parse no inicio do processo e retorna arquivo.IsArquivoValido = false se falhar
                arquivo.CdLoja = int.Parse(linha1.Substring(152, 5).Trim(), CultureInfo.InvariantCulture);
            }
            else
            {
                throw new ImportacaoArquivoInventarioException(Texts.ImportLogArquivoInvalido, Texts.InvalidFileContents.With(arquivo.NomeArquivo), Texts.InvalidFileContentsCritic.With(arquivo.NomeArquivo));
            }

            bool isMultiDepartamento = linha2.Contains("MULTI") || linha2.Contains("MÚLTI");
            arquivo.IsMultiDepartamento = isMultiDepartamento;

            if (isMultiDepartamento)
            {
                throw new ImportacaoArquivoInventarioException(Texts.ImportLogArquivoInvalido, Texts.InvalidMultiDepartmentInventoryFile.With(arquivo.NomeArquivo));
            }

            TipoArquivoInventario tipoArquivo = TipoArquivoInventario.Nenhum;

            if (linha3.Substring(94).Contains("RLT EXPERMTL"))
            {
                tipoArquivo = TipoArquivoInventario.Parcial;
            }
            else if (linha3.Substring(94).Contains("COMPLETD RLT EXPERMTAL") || linha3.Substring(94).Contains("RELAT FINAL"))
            {
                tipoArquivo = TipoArquivoInventario.Final;
            }

            if (arquivo.TipoArquivo != tipoArquivo)
            {
                arquivo.TipoArquivo = tipoArquivo;
                throw new ImportacaoArquivoInventarioException(Texts.ImportLogArquivoInvalido, Texts.InvalidInventoryFileKind.With(tipoArquivo, arquivo.TipoArquivo, arquivo.CdLoja));
            }
            else
            {
                this.Logger.InserirLogProcessamento(Texts.ImportLogArquivoValido, Texts.InventoryFileKindNotice.With(arquivo.NomeArquivo, arquivo.TipoArquivo == TipoArquivoInventario.Final ? "COMPLETD RLT EXPERMTAL / RELAT FINAL" : "RLT EXPERMTL"));
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
            int dpto = int.Parse(linha.Substring(12).Trim(), CultureInfo.InvariantCulture);

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
                item.CdUpc = null;

                item.Tamanho = linhaInicio.Substring(14, 14).Trim();
                item.NumeroEstoque = linhaInicio.Substring(29, 21).Trim();
                item.CustoUnitario = decimal.Parse(linhaInicio.Substring(51, 9).Trim().Replace("$", string.Empty), CultureInfo.InvariantCulture);

                // primeira sublinha
                string linhaDescricao = linhasItem.First();

                item.DescricaoItem = linhaDescricao.Substring(4, 40).Trim();

                item.DescricaoTamanho = string.Empty;

                item.PrecoUnitarioVarejo = decimal.Parse(linhaDescricao.Substring(51, 9).Trim().Replace("$", string.Empty), CultureInfo.InvariantCulture);

                // ultima sublinha
                string linhaUsuario = linhasItem.Last();

                item.Completo = linhaUsuario.Substring(186, 7).Trim();

                item.UltimaContagem = DateTime.Parse(linhaUsuario.Substring(80, 16).Trim(), CultureInfo.InvariantCulture);

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
            catch (Exception)
            {
                string msg = Texts.InvalidInventoryFileItemCritic.With(
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

                item.Erro = msg;
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
                arquivo.TotalEstoqueFinal = decimal.Parse(leitor.CurrentLine.Substring(117, 13).Replace("$", string.Empty), CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Verifica se a linha atual é a primeira de um rodapé de item.
        /// </summary>
        /// <param name="leitor">O leitor do arquivo.</param>
        /// <returns>True caso linha inicie o rodapé.</returns>
        protected override bool TestarInicioRodape(StatefulFileReader leitor)
        {
            return (leitor.CurrentLine.Length > 111 && leitor.CurrentLine[111] == '=') || (leitor.CurrentLine.Length > 100 && leitor.CurrentLine.Substring(88, 12).Trim() == "*** FALTARAM");
        }

        /// <summary>
        /// Verifica se a linha atual é a linha com o código de departamento.
        /// </summary>
        /// <param name="leitor">O leitor do arquivo.</param>
        /// <returns>True caso seja a linha com o departamento.</returns>
        protected override bool TestarLinhaDepartamento(StatefulFileReader leitor)
        {
            return leitor.CurrentLine.Substring(0, 9).Trim() == "Categor";
        }

        #endregion
    }
}
