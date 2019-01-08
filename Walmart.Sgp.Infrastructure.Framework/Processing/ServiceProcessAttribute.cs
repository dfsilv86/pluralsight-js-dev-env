using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.Framework.Processing
{
    /// <summary>
    /// Atributo que define configurações de um processo enfileirável.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ServiceProcessAttribute : Attribute
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ServiceProcessAttribute"/>.
        /// </summary>
        /// <param name="processName">Nome do processo.</param>
        public ServiceProcessAttribute(string processName)
        {
            this.ProcessName = processName;
            this.MaxPerUser = 1;
            this.EnableQueueing = true;
        }

        /// <summary>
        /// Obtém o nome do processo. Por padrão, a chave de tradução do nome deste processo será Process[ProcessName], e o nome do state de retorno cadastrado como rota no angular será [ProcessName]Result.
        /// </summary>
        public string ProcessName { get; private set; }

        /// <summary>
        /// Obtém ou define o número máximo de execuções em um determinado momento para todos os usuários.
        /// </summary>
        /// <remarks>0 significa sem limite global. Default 0.</remarks>
        public int MaxGlobal { get; set; }

        /// <summary>
        /// Obtém ou define o número máximo de execuções em um determinado momento para um usuário específico.
        /// </summary>
        /// <remarks>0 significa sem limite por criador. Default 1.</remarks>
        public int MaxPerUser { get; set; }

        /// <summary>
        /// Obtém ou define um valor que indica se este processo pode ser adicionado à fila de execução e executado em outro momento.
        /// </summary>
        /// <remarks>Default true.</remarks>
        public bool EnableQueueing { get; set; }
    }
}
