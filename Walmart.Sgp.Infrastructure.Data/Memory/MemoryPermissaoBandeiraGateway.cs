using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.Memory
{
    /// <summary>
    /// Implementação de um table data gateway para permissao de bandeira em memória.
    /// </summary>
    /// <remarks>
    /// Essa implementação deve ser utilizada apenas para fins de testes unitários.
    /// </remarks>
    public class MemoryPermissaoBandeiraGateway : MemoryDataGateway<PermissaoBandeira>, IPermissaoBandeiraGateway
    {
        /// <summary>
        /// Verifica se usuário possui permissâo à bandeira.
        /// </summary>
        /// <param name="idUsuario">O id do usuário.</param>
        /// <param name="idBandeira">O id da bandeira.</param>
        /// <returns>Retorna true caso possua permissão, false do contrário.</returns>
        public bool UsuarioPossuiPermissaoBandeira(int idUsuario, int idBandeira)
        {
            return true;
        }
    }
}