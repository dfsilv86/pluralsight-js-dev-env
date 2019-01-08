using System.Collections.Generic;
using Walmart.Sgp.Domain.Inventarios;

namespace Walmart.Sgp.Domain.MultisourcingVendor
{
    /// <summary>
    /// Define a interface para serviço de cadastro de Multisourcing.
    /// </summary>
    public interface IMultisourcingService
    {
        /// <summary>
        /// Obtém um Multisourcing pelo seu id.
        /// </summary>
        /// <param name="id">O id.</param>
        /// <returns>O Multisourcing.</returns>
        Multisourcing Obter(long id);
        
        /////// <summary>
        /////// Realiza a importação massiva de multisourcing através de um arquivo excel.
        /////// </summary>
        /////// <param name="filepath">Caminho do arquivo.</param>
        /////// <param name="cdUsuario">O código do usuário que está realizando a importação.</param>
        /////// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /////// <returns>Multisourcing bool.</returns>
        ////bool ImportarMultisourcing(string filepath, int cdUsuario, int cdSistema);
        
        /// <summary>
        /// Salva os dados de multisourcing.
        /// </summary>
        /// <param name="multisourcings">Os multisourcings.</param>
        /// <param name="cdUsuario">O código de usuario.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        void SalvarMultisourcings(IEnumerable<Multisourcing> multisourcings, int cdUsuario, int cdSistema);

        /// <summary>
        /// Insere Ou Atualiza um Multisourcing com base nos itens selecionados.
        /// </summary>
        /// <param name="itens">Os itens de entrada.</param>
        /// <param name="cdUsuario">O ID do usuário logado.</param>
        /// <param name="cdSistema">O código do sistema.</param>
        void SalvarMultisourcing(IEnumerable<ItemDetalheCD> itens, int cdUsuario, int cdSistema);

        /// <summary>
        /// Excluir um cadastro de multisourcing.
        /// </summary>
        /// <param name="itens"> Lista de itens que compoem o cad.</param>
        void Excluir(IEnumerable<ItemDetalheCD> itens);

        /// <summary>
        /// Excluir um cadastro de multisourcing.
        /// </summary>
        /// <param name="cdItemSaida">O código do item de saída.</param>
        /// <param name="cdCD">O código do CD.</param>
        void Excluir(long cdItemSaida, long cdCD);
    }
}
