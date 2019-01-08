using System;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Item;

namespace Walmart.Sgp.Domain.Processos
{
    /// <summary>
    /// Representa a execução de um processo.
    /// </summary>
    public class ProcessoExecucao
    {
        #region Properties
        /// <summary>
        /// Obtém ou define a Data.
        /// </summary>
        public DateTime Data { get; set; }

        /// <summary>
        /// Obtém ou define o IDItemDetalhe.
        /// </summary>
        public int IDItemDetalhe { get; set; }

        /// <summary>
        /// Obtém ou define o IDLoja.
        /// </summary>
        public int IDLoja { get; set; }

        /// <summary>
        /// Obtém ou define a Mensagem.
        /// </summary>
        public string Mensagem { get; set; }

        /// <summary>
        /// Obtém ou define o ItemDetalhe.
        /// </summary>
        public ItemDetalhe ItemDetalhe { get; set; }

        /// <summary>
        /// Obtém ou define a Loja.
        /// </summary>
        public Loja Loja { get; set; }

        /// <summary>
        /// Obtém ou define o ProcessoTipoExcecao.
        /// </summary>
        public ProcessoTipoExcecao ProcessoTipoExcecao { get; set; }

        /// <summary>
        /// Obtém ou define o Processo.
        /// </summary>
        public Processo Processo { get; set; }
        #endregion      
    }
}
