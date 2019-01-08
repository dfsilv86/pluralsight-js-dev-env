using System.Collections.Generic;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.Inventarios.Specs
{
    /// <summary>
    /// Especificação referente a se usuario pode exportar comparacao estoque.
    /// </summary>
    public class UsuarioPodeExportarComparacaoEstoqueSpec : UsuarioPermissaoInventarioSpecBase
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="UsuarioPodeExportarComparacaoEstoqueSpec"/>
        /// </summary>
        /// <param name="inventario">O inventario.</param>
        public UsuarioPodeExportarComparacaoEstoqueSpec(Inventario inventario)
            : base(inventario)
        {
        }

        /// <summary>
        /// Obtém o texto que indica o que não pode ser feito.
        /// </summary>
        protected override string NotSatisfiedReason
        {
            get { return Texts.OnlyFinishedOrApprovedInventoriesCanBeReported; }
        }

        /// <summary>
        /// Obtém o id da permissão.
        /// </summary>
        protected override string IdPermissao
        {
            get { return InventarioPermissoes.ExportarComparacaoEstoque; }
        }

        /// <summary>
        /// Obtém os status validos do inventário.
        /// </summary>
        protected override IEnumerable<InventarioStatus> StatusValidos
        {
            get { return new[] { InventarioStatus.Aprovado, InventarioStatus.Finalizado }; }
        }
    }
}