using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Inventarios
{
    /// <summary>
    /// Representa as operações permitidas sobre um inventário.
    /// </summary>
    public class InventarioOperacoesPermitidas
    {
        /// <summary>
        /// Obtém ou define um valor que indica se o usuário tem permissão de voltar o status do inventário.
        /// </summary>        
        public bool VoltarStatus { get; set; }

        /// <summary>
        /// Obtém ou define um valor que indica se o usuário pode aprovar.
        /// </summary>
        public bool Aprovar { get; set; }

        /// <summary>
        /// Obtém ou define um valor que indica se o usuário pode finalizar.
        /// </summary>
        public bool Finalizar { get; set; }

        /// <summary>
        /// Obtém ou define um valor que indica se o usuário pode cancelar.
        /// </summary>
        public bool Cancelar { get; set; }

        /// <summary>
        /// Obtém ou define um valor que indica se o usuário pode exportar comparação de estoque.
        /// </summary>
        public bool ExportarComparacaoEstoque { get; set; }

        /// <summary>
        /// Obtém ou define a mensagem referente à comparação de estoque.
        /// </summary>
        public string MensagemComparacaoEstoque { get; set; }

        /// <summary>
        /// Obtém ou define um valor que indica se o usuário pode excluir itens de inventário.
        /// </summary>        
        public bool ExcluirItem { get; set; }

        /// <summary>
        /// Obtém ou define um valor que indica se o usuário pode salvar itens de inventário.
        /// </summary>        
        public bool SalvarItem { get; set; }

        /// <summary>
        /// Obtém ou define um valor que indica se o usuário pode alterar itens de inventário.
        /// </summary>
        public SpecResult AlterarItem { get; set; }
    }
}
