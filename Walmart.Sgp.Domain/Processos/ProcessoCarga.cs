using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Commons;

namespace Walmart.Sgp.Domain.Processos
{
    /// <summary>
    /// Representa a carga de um processo.
    /// </summary>
    [DebuggerDisplay("{Nome}: {Status}")]
    public class ProcessoCarga
    {
        #region Properties
        /// <summary>
        /// Obtém ou define o nome.
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Obtém ou define o prazõ de execução estimado em dias.
        /// </summary>
        public int PrazoExecucaoEstimadoEmDias { get; set; }

        /// <summary>
        /// Obtém o status.
        /// </summary>
        public ProcessoCargaStatus Status 
        {
            get 
            {
                if (Erro == null)
                {
                    if (!DataInicioExecucao.HasValue)
                    {
                        return ProcessoCargaStatus.NaoIniciado;
                    }
                    else if (!DataFimExecucao.HasValue)
                    {
                        return ProcessoCargaStatus.EmAndamento;
                    }
                    else
                    {
                        return ProcessoCargaStatus.Concluido;
                    }
                }
                else
                {
                    return ProcessoCargaStatus.Erro;
                }
            }
        }

        /// <summary>
        /// Obtém ou define a data de início da execução.
        /// </summary>
        public DateTime? DataInicioExecucao { get; set; }

        /// <summary>
        /// Obtém ou define a data de fim da execução.
        /// </summary>
        public DateTime? DataFimExecucao { get; set; }

        /// <summary>
        /// Obtém ou define a mensagem de observacao.
        /// </summary>
        public string Observacao { get; set; }

        /// <summary>
        /// Obtém ou define o erro na carga do processo, se existir.
        /// </summary>
        public ProcessoCargaErro Erro { get; set; }
        #endregion      
    }
}
