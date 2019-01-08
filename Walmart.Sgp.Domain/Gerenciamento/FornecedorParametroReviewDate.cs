namespace Walmart.Sgp.Domain.Gerenciamento
{
    /// <summary>
    /// Representa as linhas da grid de review date na tela de parametros do fornecedor.
    /// </summary>
    public class FornecedorParametroReviewDate
    {
        private TipoIntervalo m_tpInterval;

        /// <summary>
        /// Obtém ou define o LojaCD.
        /// </summary>
        public int LojaCD { get; set; }

        /// <summary>
        /// Obtém ou define o vlLeadTime.
        /// </summary>
        public short vlLeadTime { get; set; }

        /// <summary>
        /// Obtém ou define o tpWeek.
        /// </summary>
        public TipoSemana tpWeek { get; set; }

        /// <summary>
        /// Obtém ou define o tpInterval.
        /// </summary>
        public TipoIntervalo tpInterval
        {
            get
            { 
                return m_tpInterval; 
            }

            set
            {
                if (value == null || value.Value != TipoIntervalo.Semanal.Value)
                {
                    m_tpInterval = TipoIntervalo.Quinzenal;
                }
                else
                {
                    m_tpInterval = value;
                }
            }
        }

        /// <summary>
        /// Obtém ou define o isSegunda.
        /// </summary>
        public bool isSegunda { get; set; }

        /// <summary>
        /// Obtém ou define o isTerca.
        /// </summary>
        public bool isTerca { get; set; }

        /// <summary>
        /// Obtém ou define o isQuarta.
        /// </summary>
        public bool isQuarta { get; set; }

        /// <summary>
        /// Obtém ou define o isQuinta.
        /// </summary>
        public bool isQuinta { get; set; }

        /// <summary>
        /// Obtém ou define o isSexta.
        /// </summary>
        public bool isSexta { get; set; }

        /// <summary>
        /// Obtém ou define o isSabado.
        /// </summary>
        public bool isSabado { get; set; }

        /// <summary>
        /// Obtém ou define o isDomingo.
        /// </summary>
        public bool isDomingo { get; set; }

        /// <summary>
        /// Obtém ou define o dsWeek.
        /// </summary>
        public string dsWeek { get; set; }

        /// <summary>
        /// Obtém ou define o dsInterval.
        /// </summary>
        public string dsInterval { get; set; }

        /// <summary>
        /// Obtém ou define a quantidade de registros obtidos totais.
        /// </summary>
        public int QtdRows { get; set; }
    }
}
