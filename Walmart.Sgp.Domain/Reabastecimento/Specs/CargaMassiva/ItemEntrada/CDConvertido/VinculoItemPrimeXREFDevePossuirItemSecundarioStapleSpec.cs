using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Reabastecimento.Specs.CargaMassiva
{
    /// <summary>
    /// Especificação que valida se um item é Staple, CD convertido, item prime e faz parte de uma XRef.
    /// </summary>
    public class VinculoItemPrimeXREFDevePossuirItemSecundarioStapleSpec : RelacaoItemLojaCDVinculoSpecBase<object>
    {
        private readonly Func<long, long, long, long, bool> m_verificaItemStaple;

        private readonly int m_cdSistema;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="VinculoItemPrimeXREFDevePossuirItemSecundarioStapleSpec"/>.
        /// </summary>
        /// <param name="verificaItemStaple">O delegate que verifica se um item possui itens secundarios Xref.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        public VinculoItemPrimeXREFDevePossuirItemSecundarioStapleSpec(Func<long, long, long, long, bool> verificaItemStaple, int cdSistema)
        {
            this.m_verificaItemStaple = verificaItemStaple;
            this.m_cdSistema = cdSistema;
        }

        /// <summary>
        /// Obtém a chave para agrupamento.
        /// </summary>
        protected override Func<RelacaoItemLojaCDVinculo, object> Key
        {
            get
            {
                return m => new { m.CdItemDetalheEntrada, m.CdCD, m.CdLoja };
            }
        }

        /// <summary>
        /// A mensagem usada caso o grupo não seja válido.
        /// </summary>
        protected override string NotSatisfiedReason
        {
            get { return Texts.XrefDoesntHaveSecondaryItens; }
        }

        /// <summary>
        /// Valida se um grupo de RelacaoItemLojaCDVinculo é válido.
        /// </summary>
        /// <param name="group">O grupo.</param>
        /// <returns>True caso válido, false caso contrário.</returns>
        protected override bool IsSatisfiedByGroup(IGrouping<object, RelacaoItemLojaCDVinculo> group)
        {
            var cdItem = group.First().CdItemDetalheEntrada;
            var cdCD = group.First().CdCD;
            var cdLoja = group.First().CdLoja;

            // m_verificaItemStaple retorna true se possuir secundarios staple na mesma xref e etc. 
            // (vide metodo ItemPossuiItensXrefSecundarios, consulta RelacaoItemLojaCD.VerificarItemXrefPossuiSecundarios.sql)
            if (!m_verificaItemStaple(cdItem, cdCD, cdLoja, m_cdSistema))
            {
                return false;
            } 

            return true;
        }
    }
}
