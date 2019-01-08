using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.Framework.Processing
{
    /// <summary>
    /// O contexto de execução de um processo.
    /// </summary>
    public static class ProcessingContext
    {
        /// <summary>
        /// Um contexto que não está ligado a nenhum processamento.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly MemoryProcessingContext Dummy = new MemoryProcessingContext();

        //// Não inicializar diretamente no field os ThreadStatics
        //// https://msdn.microsoft.com/en-us/library/system.threadstaticattribute(v=vs.110).aspx
        [ThreadStatic]
        private static volatile IProcessingContext s_current;

        /// <summary>
        /// Obtém ou define o contexto de execução atual.
        /// </summary>
        public static IProcessingContext Current
        {
            get
            {
                if (null == s_current)
                {
                    s_current = ProcessingContext.Dummy;
                }

                return s_current;
            }

            set
            {
                s_current = value;
            }
        }
    }
}
