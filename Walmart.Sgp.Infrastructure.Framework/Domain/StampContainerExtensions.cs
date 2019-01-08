using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Infrastructure.Framework.Domain
{
    /// <summary>
    /// Extension methods para IStampContainer.
    /// </summary>
    public static class StampContainerExtensions
    {
        /// <summary>
        /// Carimba a entidade que possui as propriedades de carimbo de inclusão e alteração.
        /// </summary>
        /// <param name="container">O container.</param>
        public static void Stamp(this IStampContainer container)
        {
            if (container.IsNew)
            {
                container.StampInsert();
            }
            else
            {
                container.StampUpdate();
            }
        }

        /// <summary>
        /// Carimba as propriedades de inclusão.
        /// </summary>
        /// <param name="container">O container.</param>
        public static void StampInsert(this IStampContainer container)
        {
            var user = RuntimeContext.Current.User;

            container.CdUsuarioCriacao = user.Id;
            container.DhCriacao = DateTime.Now;
        }

        /// <summary>
        /// Carimba as propriedades de atualização.
        /// </summary>
        /// <param name="container">O container.</param>
        public static void StampUpdate(this IStampContainer container)
        {
            var user = RuntimeContext.Current.User;

            container.CdUsuarioAtualizacao = user.Id;
            container.DhAtualizacao = DateTime.Now;
        }
    }
}
