using System;
using System.Collections.Generic;
using System.Linq;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Infrastructure.IO.Importing.Inventario
{
    /// <summary>
    /// Classe base para um leitor de arquivo de inventário.
    /// </summary>
    public abstract class LeitorArquivoInventarioBase
    {
        #region Constructor

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="LeitorArquivoInventarioBase"/>.
        /// </summary>
        /// <param name="tipoProcesso">O tipo de processo de importação (automático ou manual).</param>
        /// <param name="tipoOrigem">O tipo de origem da importação (loja ou HO).</param>
        /// <param name="idLojaImportacao">O id da loja a qual o arquivo de inventário pertence.</param>
        /// <param name="arquivos">Os arquivos.</param>
        /// <param name="dataInventario">A data de inventario.</param>
        /// <param name="qtdDiasInventario">A quantidade de dias permitida.</param>
        /// <param name="logger">O logger.</param>
        protected LeitorArquivoInventarioBase(TipoProcessoImportacao tipoProcesso, TipoOrigemImportacao tipoOrigem, int idLojaImportacao, IEnumerable<string> arquivos, DateTime dataInventario, int? qtdDiasInventario, ILeitorLogger logger)
        {
            this.IdLojaImportacao = idLojaImportacao;
            this.Arquivos = arquivos;
            this.DataInventario = dataInventario;
            this.QtdDiasInventario = qtdDiasInventario;
            this.Logger = logger;
            this.TipoProcessoImportacao = tipoProcesso;
            this.TipoOrigemImportacao = tipoOrigem;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Obtém o id da loja a qual o arquivo de inventário pertence.
        /// </summary>
        public int IdLojaImportacao { get; private set; }

        /// <summary>
        /// Obtém a data do inventário.
        /// </summary>
        public DateTime DataInventario { get; private set; }

        /// <summary>
        /// Obtém quantidade de dias permitida.
        /// </summary>
        public int? QtdDiasInventario { get; private set; }

        /// <summary>
        /// Obtém os arquivos.
        /// </summary>
        public IEnumerable<string> Arquivos { get; private set; }

        /// <summary>
        /// Obtém o tipo de origem do processo de importação.
        /// </summary>
        public TipoOrigemImportacao TipoOrigemImportacao { get; private set; }

        /// <summary>
        /// Obtém o tipo de processo de importação.
        /// </summary>
        public TipoProcessoImportacao TipoProcessoImportacao { get; private set; }

        /// <summary>
        /// Obtém o logger.
        /// </summary>
        protected ILeitorLogger Logger { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Lê os arquivos.
        /// </summary>
        /// <returns>
        /// Lista contendo informações sobre os arquivos lidos e seus itens.
        /// </returns>
        public IEnumerable<ArquivoInventario> LerArquivos()
        {
            this.Logger.InserirLogProcessamento(Texts.ImportLogProcessamentoLeituraArquivo, Texts.ImportLogInicioProcessamentoLeituraArquivo.With(DateTime.Now));

            var result = this.Arquivos.Select(nomeArquivo => this.LerArquivo(nomeArquivo)).ToArray();

            this.Logger.InserirLogProcessamento(Texts.ImportLogProcessamentoLeituraArquivo, Texts.ImportLogFinalProcessamentoLeituraArquivo.With(DateTime.Now));

            return result;
        }

        /// <summary>
        /// Lê o arquivo especificado.
        /// </summary>
        /// <param name="caminhoArquivo">O caminho para o arquivo.</param>
        /// <returns>As informações do arquivo.</returns>
        protected abstract ArquivoInventario LerArquivo(string caminhoArquivo);

        /// <summary>
        /// Determina se a data lida do arquivo está dentro do intervalo de data ao redor do agendamento de inventário, caso especificado.
        /// </summary>
        /// <param name="result">O arquivo sendo lido.</param>
        /// <returns>True caso a data seja válida.</returns>
        protected virtual bool IsDataArquivoDentroDoIntervalo(ArquivoInventario result)
        {
            if (this.QtdDiasInventario.HasValue && 
                (result.DataArquivo.Value.AddDays(this.QtdDiasInventario.Value) < this.DataInventario || 
                 result.DataArquivo.Value.AddDays(-this.QtdDiasInventario.Value) > this.DataInventario))
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
