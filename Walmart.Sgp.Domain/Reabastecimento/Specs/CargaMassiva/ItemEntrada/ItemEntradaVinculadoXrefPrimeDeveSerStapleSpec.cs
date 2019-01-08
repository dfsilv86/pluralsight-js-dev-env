using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Reabastecimento.Specs.CargaMassiva.ItemEntrada
{
    /// <summary>
    /// Especificação que valida se o item se encaixa no cenário (é vinculado a uma xref, é de entrada, é prime na xref) e se é Staple.
    /// </summary>
    public class ItemEntradaVinculadoXrefPrimeDeveSerStapleSpec : RelacaoItemLojaCDVinculoSpecBase<object>
    {
        private readonly Func<long, int, int, int, ValorTipoReabastecimento> m_delegate;
        private readonly int m_cdSistema;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ItemEntradaVinculadoXrefPrimeDeveSerStapleSpec"/>.
        /// </summary>
        /// <param name="obterTipoReabastecimentoItemVinculadoXrefPrime">O delegate para o metodo que verifica o tipo de reabastecimento.</param>
        /// <param name="cdSistema">O código do sistema.</param>
        public ItemEntradaVinculadoXrefPrimeDeveSerStapleSpec(Func<long, int, int, int, ValorTipoReabastecimento> obterTipoReabastecimentoItemVinculadoXrefPrime, int cdSistema)
        {
            m_delegate = obterTipoReabastecimentoItemVinculadoXrefPrime;
            m_cdSistema = cdSistema;
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
            get { return Texts.ItemXrefPrimeNotStaple; }
        }

        /// <summary>
        /// Valida se um grupo de RelacaoItemLojaCDVinculo é válido.
        /// </summary>
        /// <param name="group">O grupo.</param>
        /// <returns>True caso válido, false caso contrário.</returns>
        protected override bool IsSatisfiedByGroup(IGrouping<object, RelacaoItemLojaCDVinculo> group)
        {
            var cdItem = group.First().CdItemDetalheEntrada;
            var cdCD = (int)group.First().CdCD;
            var cdLoja = (int)group.First().CdLoja;

            var vlTipoReabastecimento = this.m_delegate(cdItem, cdLoja, m_cdSistema, cdCD);

            if (null == vlTipoReabastecimento)
            {
                return true;
            }

            return vlTipoReabastecimento == ValorTipoReabastecimento.StapleStock20 ||
                vlTipoReabastecimento == ValorTipoReabastecimento.StapleStock22 ||
                vlTipoReabastecimento == ValorTipoReabastecimento.StapleStock40 ||
                vlTipoReabastecimento == ValorTipoReabastecimento.StapleStock42 ||
                vlTipoReabastecimento == ValorTipoReabastecimento.StapleStock43 ||
                vlTipoReabastecimento == ValorTipoReabastecimento.StapleStock81;
        }
    }
}
