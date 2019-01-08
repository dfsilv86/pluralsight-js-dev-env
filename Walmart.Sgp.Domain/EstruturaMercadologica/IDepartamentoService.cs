using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Define a interface de um serviço de departamento.
    /// </summary>
    public interface IDepartamentoService : IDomainService<Departamento>
    {
        /// <summary>
        /// Obtém um departamento pelo seu código de departamento e estrutura mercadológica.
        /// </summary>
        /// <param name="cdDepartamento">O código de departamento.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <param name="modoPereciveis">Indica como o serviço deve tratar retorno de departamentos em relação a estes serem perecíveis ou outro.</param>
        /// <returns>O departamento.</returns>
        /// <remarks>Conforme modoPereciveis, retorna todos departamentos ou apenas se blPerecivel='S' (comportamento padrão das lookups de departamento)</remarks>
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
        /// <returns>A lista de departamentos.</returns>
        /// <remarks>Traz apenas a descrição da divisão.</remarks>
        IEnumerable<Departamento> PesquisarPorDivisaoESistema(int? cdDepartamento, string dsDepartamento, bool? blPerecivel, int? cdDivisao, byte cdSistema, Paging paging);

        /// <summary>
        /// Obtém os departamentos pelo código de sistema informado e se são perecíveis.
        /// </summary>
        /// <param name="cdSistema">O código do sistema.</param>
        /// <param name="blPerecivel">Se são perecíveis.</param>
        /// <returns>Os departamentos.</returns>
        IEnumerable<Departamento> ObterPorSistema(int cdSistema, bool blPerecivel);

        /// <summary>
        /// Obtém o departamento pelo código.
        /// </summary>
        /// <param name="cdSistema">O código do sistema.</param>
        /// <param name="cdDepartamento">O código do departamento.</param>
        /// <returns>O departamento.</returns>
        Departamento ObterPorCdDepartamento(int cdSistema, int cdDepartamento);

        /// <summary>
        /// Obtém o departamento junto com a divisão por id.
        /// </summary>
        /// <param name="idDepartamento">O id de departamento.</param>
        /// <returns>O departamento estruturado.</returns>
        Departamento ObterEstruturadoPorId(int idDepartamento);

        /// <summary>
        /// Atualiza o indicador de perecivel do departamento.
        /// </summary>
        /// <param name="idDepartamento">O id de departamento.</param>
        /// <param name="perecivel">Indica se o departamento é perecivel.</param>
        void AtualizarPerecivel(int idDepartamento, bool perecivel);
    }
}
