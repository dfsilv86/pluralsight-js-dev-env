using System;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Inventarios.Specs
{
    /// <summary>
    /// Especificação referente a se usuario pode voltar status inventario.
    /// </summary>
    public class UsuarioPodeVoltarStatusInventarioSpec : SpecBase<IRuntimeUser>
    {
        private readonly Inventario m_inventario;
        private readonly IInventarioGateway m_inventarioGateway;
        private readonly IFechamentoFiscalGateway m_fechamentoFiscalGateway;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="UsuarioPodeVoltarStatusInventarioSpec"/>.
        /// </summary>
        /// <param name="inventario">O inventario.</param>
        /// <param name="inventarioGateway">O table data gateway para inventario.</param>
        /// <param name="mFechamentoFiscalGateway">O table data gateway para fechamento fiscal.</param>
        public UsuarioPodeVoltarStatusInventarioSpec(
            Inventario inventario,
            IInventarioGateway inventarioGateway,
            IFechamentoFiscalGateway mFechamentoFiscalGateway)
        {
            m_inventario = inventario;
            m_inventarioGateway = inventarioGateway;
            m_fechamentoFiscalGateway = mFechamentoFiscalGateway;
        }

        /// <summary>
        /// Verifica se o alvo informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O alvo.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo alvo.
        /// </returns>
        public override SpecResult IsSatisfiedBy(IRuntimeUser target)
        {
            if (UsuarioPossuiPermissao(target) && InventarioNoStatusCorreto())
            {
                var mesAberto = ObterMesAberto();
                if (mesAberto.HasValue && mesAberto.Value < m_inventario.dhInventario)
                {
                    var ultimoInventarioLoja = m_inventarioGateway.ObterUltimo(
                        m_inventario.IDLoja,
                        m_inventario.IDDepartamento,
                        mesAberto.Value);

                    if (ultimoInventarioLoja == m_inventario)
                    {
                        return Satisfied();
                    }
                }                                    
            }

            return NotSatisfied(Texts.CannotRevertInventaryStatus);
        }

        private static bool UsuarioPossuiPermissao(IRuntimeUser target)
        {
            return target.IsAdministrator;
        }

        private DateTime? ObterMesAberto()
        {
            var ultimoFechamento = m_fechamentoFiscalGateway.ObterUltimo(m_inventario.IDLoja);
            return ultimoFechamento == null ? null : ultimoFechamento.ObterMesAberto();
        }

        private bool InventarioNoStatusCorreto()
        {
            return m_inventario.stInventario == InventarioStatus.Aprovado ||
                   m_inventario.stInventario == InventarioStatus.Finalizado;
        }
    }
}
