namespace Walmart.Sgp.Domain.Gerenciamento
{
    /// <summary>
    /// Define a interface de um serviço de parâmetro.
    /// </summary>
    public interface IParametroService
    {
        /// <summary>
        /// Obtém o parãmetro.
        /// </summary>
        /// <returns>O parâmetro.</returns>
        Parametro Obter();

        /// <summary>
        /// Salva o parãmetro
        /// </summary>
        /// <param name="parametro">O parâmetro.</param>
        void Salvar(Parametro parametro);

        /// <summary>
        /// Obtém o parâmetro com seus relacionamentos.
        /// </summary>
        /// <returns>
        /// O parâmetro.
        /// </returns>
        Parametro ObterEstruturado();
    }
}