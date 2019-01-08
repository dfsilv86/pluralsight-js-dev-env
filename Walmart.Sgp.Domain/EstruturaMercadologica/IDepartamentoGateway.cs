using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Define a interface de um table data gateway para departamento.
    /// </summary>
    public interface IDepartamentoGateway : IDataGateway<Departamento>
    {
        /// <summary>
        /// Obtém um departamento pelo seu código de departamento e estrutura mercadológica.
        /// </summary>
        /// <param name="cdDepartamento">O código de departamento.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <param name="modoPereciveis">Informa o modo pereciveis.</param>
        /// <returns>O departamento.</returns>
        /// <remarks>Retorna apenas se blPerecivel='S' (comportamento padrão das lookups de departamento)</remarks>
        Departamento ObterPorDepartamentoESistema(int cdDepartamento, byte cdSistema, string modoPereciveis);

        /// <summary>
        /// Pesquisa departamentos filtrando pelo código de departamento, descrição do departamento, flag que indica se é de perecíveis, e/ou código da estrutura mercadológica.
        /// </summary>
        /// <param name="cdDepartamento">O código de departamento.</param>
        /// <param name="dsDepartamento">A descrição do departamento.</param>
        /// <param name="blPerecivel">A flag de perecível.</param>
        /// <param name="cdDivisao">O código da divisão.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>O departamento.</returns>
        /// <remarks>Traz apenas a descrição da divisão.</remarks>
        IEnumerable<Departamento> PesquisarPorDivisaoESistema(int? cdDepartamento, string dsDepartamento, bool? blPerecivel, int? cdDivisao, byte cdSistema, Paging paging);

        /// <summary>
        /// Obtém o departamento junto com a divisão por id.
        /// </summary>
        /// <param name="idDepartamento">O id de departamento.</param>
        /// <returns>O departamento estruturado.</returns>
        Departamento ObterEstruturadoPorId(int idDepartamento);
    }
}
