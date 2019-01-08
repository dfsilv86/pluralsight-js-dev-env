using System.Collections.Generic;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.Memory
{
    /// <summary>
    /// Implementação de um table data gateway para departamento em memória.
    /// </summary>
    /// <remarks>
    /// Essa implementação deve ser utilizada apenas para fins de testes unitários.
    /// </remarks>
    public class MemoryDepartamentoGateway : MemoryDataGateway<Departamento>, IDepartamentoGateway
    {
        /// <summary>
        /// Obtém um departamento pelo seu código de departamento e estrutura mercadológica.
        /// </summary>
        /// <param name="cdDepartamento">O código de departamento.</param>
        /// <param name="cdSistema">O código da estrutura mercadológica.</param>
        /// <param name="modoPereciveis">Informa o modo pereciveis.</param>
        /// <returns>O departamento.</returns>
        /// <remarks>Retorna apenas se blPerecivel='S' (comportamento padrão das lookups de departamento)</remarks>
        public Departamento ObterPorDepartamentoESistema(int cdDepartamento, byte cdSistema, string modoPereciveis)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Pesquisa departamentos filtrando pelo código de departamento, descrição do departamento, flag que indica se é de perecíveis, e/ou código da estrutura mercadológica.
        /// </summary>
        /// <param name="cdDepartamento">O código de departamento.</param>
        /// <param name="dsDepartamento">A descrição do departamento.</param>
        /// <param name="blPerecivel">A flag de perecível.</param>
        /// <param name="cdDivisao">O código da divisão.</param>
        /// <param name="cdSistema">O código da estrutura mercadológica.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>O departamento.</returns>
        /// <remarks>Traz apenas a descrição da divisão.</remarks>
        public IEnumerable<Departamento> PesquisarPorDivisaoESistema(int? cdDepartamento, string dsDepartamento, bool? blPerecivel, int? cdDivisao, byte cdSistema, Paging paging)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Obtém o departamento junto com a divisão por id.
        /// </summary>
        /// <param name="idDepartamento">O id de departamento.</param>
        /// <returns>
        /// O departamento estruturado.
        /// </returns>
        public Departamento ObterEstruturadoPorId(int idDepartamento)
        {
            throw new System.NotImplementedException();
        }
    }
}