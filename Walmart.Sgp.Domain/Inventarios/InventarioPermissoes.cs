namespace Walmart.Sgp.Domain.Inventarios
{
    /// <summary>
    /// Contém os nomes das permissões relacionadas ao inventário.
    /// </summary>
    //// TODO:    Criar uma classe para todo o domínio que retorna a permissão de um certa ação, pois o padrão ControllerName.ActionName.
    ////          Definir uma interface no domínio e na web api implementar a classe que lê essa informação das actions.
    public static class InventarioPermissoes
    {   
        /// <summary>
        /// Permissão de adicionar item.
        /// </summary>
        public const string AdicionarItem = "Inventario.ItemAdicionar";

        /// <summary>
        /// Permissão de aprovar.
        /// </summary>
        public const string Aprovar = "Inventario.Aprovar";

        /// <summary>
        /// Permissão de finalizar.
        /// </summary>
        public const string Finalizar = "Inventario.Finalizar";

        /// <summary>
        /// Permissão de exportação de comparação de estoque.
        /// </summary>
        public const string ExportarComparacaoEstoque = "Inventario.ExportarRelatorioComparacaoEstoque";
    }
}