using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Serviço de domínio relacionado a departamento.
    /// </summary>
    public class DepartamentoService : EntityDomainServiceBase<Departamento, IDepartamentoGateway>, IDepartamentoService
    {
        #region Constructor
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DepartamentoService" />.
        /// </summary>
        /// <param name="mainGateway">O table data gateway para departamento.</param>
        public DepartamentoService(IDepartamentoGateway mainGateway)
            : base(mainGateway)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Obtém um departamento pelo seu código de departamento e estrutura mercadológica.
        /// </summary>
        /// <param name="cdDepartamento">O código de departamento.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <param name="modoPereciveis">Informa o modo pereciveis.</param>
        /// <returns>O departamento.</returns>
        /// <remarks>Retorna apenas se blPerecivel='S' (comportamento padrão das lookups de departamento)</remarks>
        public Departamento ObterPorDepartamentoESistema(int cdDepartamento, byte cdSistema, string modoPereciveis)
        {
            Assert(new { MarketingStructure = cdSistema }, new AllMustBeInformedSpec());

            return this.MainGateway.ObterPorDepartamentoESistema(cdDepartamento, cdSistema, modoPereciveis);
        }

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
        public IEnumerable<Departamento> PesquisarPorDivisaoESistema(int? cdDepartamento, string dsDepartamento, bool? blPerecivel, int? cdDivisao, byte cdSistema, Paging paging)
        {
            Assert(new { MarketingStructure = cdSistema }, new AllMustBeInformedSpec());

            return this.MainGateway.PesquisarPorDivisaoESistema(cdDepartamento, dsDepartamento, blPerecivel, cdDivisao, cdSistema, paging);
        }

        /// <summary>
        /// Obtém os departamentos pelo código de sistema informado e se são perecíveis.
        /// </summary>
        /// <param name="cdSistema">O código do sistema.</param>
        /// <param name="blPerecivel">Se são perecíveis.</param>
        /// <returns>Os departamentos.</returns>
        public IEnumerable<Departamento> ObterPorSistema(int cdSistema, bool blPerecivel)
        {            
            return MainGateway.Find("cdSistema = @cdSistema AND blPerecivel = @blPerecivel", new { cdSistema, blPerecivel = blPerecivel ? "S" : "N" });
        }

        /// <summary>
        /// Obtém o departamento pelo código.
        /// </summary>
        /// <param name="cdSistema">O código do sistema.</param>
        /// <param name="cdDepartamento">O código do departamento.</param>
        /// <returns>O departamento.</returns>
        public Departamento ObterPorCdDepartamento(int cdSistema, int cdDepartamento)
        {
            return MainGateway.Find("cdSistema = @cdSistema AND cdDepartamento = @cdDepartamento", new { cdSistema, cdDepartamento }).FirstOrDefault();
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
            return MainGateway.ObterEstruturadoPorId(idDepartamento);
        }

        /// <summary>
        /// Atualizars the perecivel.
        /// </summary>
        /// <param name="idDepartamento">O id de departamento.</param>
        /// <param name="perecivel">if set to <c>true</c> [perecivel].</param>
        public void AtualizarPerecivel(int idDepartamento, bool perecivel)
        {
            var departamento = new Departamento { Id = idDepartamento };
            departamento.blPerecivel = perecivel.ToStringSN();
            departamento.Stamp();
            MainGateway.Update(
                "blPerecivel = @blPerecivel, DhAtualizacao = @DhAtualizacao, CdUsuarioAtualizacao = @CdUsuarioAtualizacao",
                departamento);
        }

        #endregion                  
    }
}
