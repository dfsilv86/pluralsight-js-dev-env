using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Reabastecimento.Specs.CargaMassiva.ItemEntrada
{
    /// <summary>
    /// Especificação que valida se o item de saída pode ser vinculado (deve ser Staple, Prime primario, e possuir itens secundarios na mesma xref).
    /// </summary>
    public class ItemSaidaDeveSerValidoSpec : RelacaoItemLojaCDVinculoSpecBase<object>
    {
        private readonly Func<long, int, int, int, bool?> m_delegate;
        private readonly int m_cdSistema;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ItemSaidaDeveSerValidoSpec"/>.
        /// </summary>
        /// <param name="obterItemSaidaAtendeRequisitos">O delegate para o metodo que verifica se o item atende aos requisitos.</param>
        /// <param name="cdSistema">O código do sistema.</param>
        public ItemSaidaDeveSerValidoSpec(Func<long, int, int, int, bool?> obterItemSaidaAtendeRequisitos, int cdSistema)
        {
            m_delegate = obterItemSaidaAtendeRequisitos;
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
            get { return Texts.ItemOutputMustBeValid; }
        }

        /// <summary>
        /// Valida se um grupo de RelacaoItemLojaCDVinculo é válido.
        /// </summary>
        /// <param name="group">O grupo.</param>
        /// <returns>True caso válido, false caso contrário.</returns>
        protected override bool IsSatisfiedByGroup(IGrouping<object, RelacaoItemLojaCDVinculo> group)
        {
            var cdItem = group.First().CdItemDetalheEntrada;
            var cdLoja = (int)group.First().CdLoja;

            var isValid = this.m_delegate(cdItem, cdLoja, m_cdSistema, (int)group.First().CdCD);

            if (!isValid.HasValue)
            {
                return true;
            }

            return isValid.Value;
        }
    }
}
