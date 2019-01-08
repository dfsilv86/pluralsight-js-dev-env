using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;

namespace Walmart.Sgp.Domain.Processos
{
    /// <summary>
    /// Representa a carga de processos de uma loja.
    /// </summary>
    public class LojaProcessosCarga
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="LojaProcessosCarga"/>
        /// </summary>
        public LojaProcessosCarga(DateTime data, Bandeira bandeira, Loja loja)
        {
            Data = data;
            Bandeira = bandeira;
            Loja = loja;
            Cargas = new List<ProcessoCarga>();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém a bandeira.
        /// </summary>
        public Bandeira Bandeira { get; private set; }

        /// <summary>
        /// Obtém a loja.
        /// </summary>
        public Loja Loja { get; private set; }

        /// <summary>
        /// Obtém a data.
        /// </summary>
        public DateTime Data { get; private set; }

        /// <summary>
        /// Obtém o status geral.
        /// </summary>
        public ProcessoCargaStatus StatusGeral
        {
            get
            {
                if (PossuiErro())
                {
                    return ProcessoCargaStatus.Erro;
                }
                else if (PossuiEmAndamento())
                {
                    return ProcessoCargaStatus.EmAndamento;
                }
                else if (PossuiNaoIniciado())
                {
                    return ProcessoCargaStatus.NaoIniciado;
                }
                else
                {
                    return ProcessoCargaStatus.Concluido;
                }
            }
        }       

        /// <summary>
        /// Obtém as cargas de processos.
        /// </summary>
        public IList<ProcessoCarga> Cargas { get; private set; }
        #endregion

        #region Methods
        private bool PossuiNaoIniciado()
        {
            return Cargas.Any(c => c.Status == ProcessoCargaStatus.NaoIniciado);
        }

        private bool PossuiEmAndamento()
        {
            return Cargas.Any(c => c.Status == ProcessoCargaStatus.EmAndamento);
        }

        private bool PossuiErro()
        {
            return Cargas.Any(c => c.Status == ProcessoCargaStatus.Erro);
        }
        #endregion
    }
}
