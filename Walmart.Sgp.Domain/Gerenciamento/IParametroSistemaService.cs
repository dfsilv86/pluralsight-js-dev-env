using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Gerenciamento
{
    /// <summary>
    /// Define a interface de um serviço de parâmetro sistema.
    /// </summary>
    public interface IParametroSistemaService : IDomainService<ParametroSistema>
    {
        /// <summary>
        /// Obtém o parâmetro sistema por nome
        /// </summary>
        /// <param name="nome">O nome.</param>
        /// <returns>O parâmetro.</returns>
        ParametroSistema ObterPorNome(string nome);
    }
}