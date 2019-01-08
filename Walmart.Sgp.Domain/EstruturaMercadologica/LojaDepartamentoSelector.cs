using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Selector para lojas X departamentos.
    /// </summary>
    /// <remarks>
    /// Permite selecionar 4 situações de combinação entre lojas e departamentos.
    /// * 1 departamento de 1 loja.
    /// * 1 departamento de todas as lojas.
    /// * Todos os departamentos de 1 loja.
    /// * Todos os departamentos de todas as lojas.
    /// </remarks>
    public sealed class LojaDepartamentoSelector
    {
        #region Fields
        private readonly ILojaService m_lojaService;
        private readonly IDepartamentoService m_departamentoService;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="LojaDepartamentoSelector" />.
        /// </summary>
        /// <param name="lojaService">O serviço de loja.</param>
        /// <param name="departamentoService">O serviço de departamento.</param>
        public LojaDepartamentoSelector(ILojaService lojaService, IDepartamentoService departamentoService)
        {
            m_lojaService = lojaService;
            m_departamentoService = departamentoService;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém as lojas selecionadas.
        /// </summary>
        public IEnumerable<Loja> Lojas { get; private set; }

        /// <summary>
        /// Obtém os departamentos selecionados.
        /// </summary>
        public IEnumerable<Departamento> Departamentos { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Seleciona 1 departamento de 1 loja.
        /// </summary>
        /// <param name="cdSistema">O código do sistema.</param>
        /// <param name="cdLoja">O id da loja.</param>
        /// <param name="cdDepartamento">O id do departamento.</param>
        public void SelecionarUmDepartamentoDeUmaLoja(int cdSistema, int cdLoja, int cdDepartamento)
        {
            SpecService.Assert(new { cdLoja, cdDepartamento }, new AllMustBeInformedSpec());

            Lojas = new Loja[] { m_lojaService.ObterPorCdLoja(cdSistema, cdLoja) };
            Departamentos = new Departamento[] { m_departamentoService.ObterPorCdDepartamento(cdSistema, cdDepartamento) };
        }

        /// <summary>
        /// Seleciona 1 departamento de todas as lojas.
        /// </summary>
        /// <param name="cdSistema">O código do sistema.</param>
        /// <param name="idBandeira">O id da bandeira.</param>
        /// <param name="cdDepartamento">O id do departamento.</param>
        public void SelecionarUmDepartamentoDeTodasAsLojas(int cdSistema, int idBandeira, int cdDepartamento)
        {
            SpecService.Assert(new { idBandeira, cdDepartamento }, new AllMustBeInformedSpec());

            Lojas = m_lojaService.ObterLojasPorBandeira(idBandeira);
            Departamentos = new Departamento[] { m_departamentoService.ObterPorCdDepartamento(cdSistema, cdDepartamento) };
        }

        /// <summary>
        /// Seleciona todos os departamentos de 1 loja.
        /// </summary>
        /// <param name="cdSistema">O código do sistema.</param>
        /// <param name="cdLoja">O código da loja.</param>
        public void SelecionarTodosOsDepartamentosDeUmaLoja(int cdSistema, int cdLoja)
        {
            SpecService.Assert(new { cdSistema, cdLoja }, new AllMustBeInformedSpec());

            Lojas = new Loja[] { m_lojaService.ObterPorCdLoja(cdSistema, cdLoja) };
            Departamentos = m_departamentoService.ObterPorSistema(cdSistema, true);
        }

        /// <summary>
        /// Seleciona todos os departamentos de todas as lojas.
        /// </summary>
        /// <param name="cdSistema">O código do sistema.</param>
        /// <param name="idBandeira">O id da bandeira.</param>
        public void SelecionarTodosOsDepartamentosDeTodasAsLojas(int cdSistema, int idBandeira)
        {
            SpecService.Assert(new { cdSistema, idBandeira }, new AllMustBeInformedSpec());

            Lojas = m_lojaService.ObterLojasPorBandeira(idBandeira);
            Departamentos = m_departamentoService.ObterPorSistema(cdSistema, true);
        }

        /// <summary>
        /// Descobre através dos parâmetros qual das 4 seleções disponíveis deve ser realizada:
        /// * Se cdLoja e cdDepatamento forem diferentes de nulo: 1 departamento de 1 loja.
        /// * Se cdDepartamento for diferente de nulo e loja nula: 1 departamento de todas as lojas.
        /// * Se cdDepartamento for nulo e loja não for nula? todos os departamentos de 1 loja.
        /// * Se cdLoja e cdDepatamento forem nulos: todos os departamentos de todas as lojas.
        /// </summary>
        /// <param name="cdSistema">O código do sistema.</param>
        /// <param name="idBandeira">O id da bandeira.</param>
        /// <param name="cdLoja">O código da loja.</param>
        /// <param name="cdDepartamento">O id do departamento.</param>
        public void Selecionar(int cdSistema, int idBandeira, int? cdLoja, int? cdDepartamento)
        {
            if (cdLoja.HasValue)
            {
                if (cdDepartamento.HasValue)
                {
                    SelecionarUmDepartamentoDeUmaLoja(cdSistema, cdLoja.Value, cdDepartamento.Value);
                }
                else
                {
                    SelecionarTodosOsDepartamentosDeUmaLoja(cdSistema, cdLoja.Value);
                }
            }
            else
            {
                if (cdDepartamento.HasValue)
                {
                    SelecionarUmDepartamentoDeTodasAsLojas(cdSistema, idBandeira, cdDepartamento.Value);
                }
                else
                {
                    SelecionarTodosOsDepartamentosDeTodasAsLojas(cdSistema, idBandeira);
                }
            }
        }
        #endregion
    }
}
