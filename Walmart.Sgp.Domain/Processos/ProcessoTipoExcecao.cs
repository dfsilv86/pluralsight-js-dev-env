namespace Walmart.Sgp.Domain.Processos
{
    /// <summary>
    /// Representa a tipo de execução de um processo.
    /// </summary>
    public class ProcessoTipoExcecao
    {
        #region Properties
        /// <summary>
        /// Obtém ou define o IdProcessoTipoExcecao.
        /// </summary>
        public int IdProcessoTipoExcecao { get; set; }

        /// <summary>
        /// Obtém ou define a Descricao.
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Obtém ou define a blBLoqueiaEntradaNF.
        /// </summary>
        public bool blBLoqueiaEntradaNF { get; set; }
        #endregion      
    }
}
