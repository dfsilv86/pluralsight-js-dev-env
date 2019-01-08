using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Infrastructure.Framework.Processing;

namespace Walmart.Sgp.Domain.Inventarios
{    
    /// <summary>
    /// Define a interface de um serviço de importação de inventario.
    /// </summary>
    public interface IInventarioImportacaoService
    {        
        /// <summary>
        /// Realiza importação automática.
        /// </summary>
        /// <param name="request">Os parâmetros da importação.</param>
        /// <param name="tipoOrigemImportacao">O tipo de origem do processo de importação (Loja ou HO).</param>
        /// <returns>
        /// O resultado da operação.
        /// </returns>
        [ServiceProcess("ImportarAutomatico", MaxPerUser = 1)]
        ImportarInventarioResponse ImportarAutomatico(ImportarInventarioAutomaticoRequest request, [ExposedParameter]TipoOrigemImportacao tipoOrigemImportacao);

        /// <summary>
        /// Realiza importação manual.
        /// </summary>
        /// <param name="request">Os parâmetros da importação.</param>
        /// <param name="tipoOrigemImportacao">O tipo de origem do processo de importação (Loja ou HO).</param>
        /// <returns>
        /// O resultado da operação.
        /// </returns>
        [ServiceProcess("ImportarManual", MaxPerUser = 1)]
        ImportarInventarioResponse ImportarManual(ImportarInventarioManualRequest request, [ExposedParameter]TipoOrigemImportacao tipoOrigemImportacao);
 
        /// <summary>
        /// Caso atacado, carrega a categoria correspondente ao departamento selecionado (atacado / cdSistema=2 não possui departamentos, então departamento=categoria)
        /// </summary>
        /// <param name="request">Os parâmetros de processamento.</param>
        /// <remarks>Quando cdSistema=2, a tabela categoria possui registros onde cdCategoria e dsCategoria correspondem aos registros na tabela departamento (cdDepartamento, dsDepartamento).</remarks>
        /// <returns>O id da categoria equivalente do departamento no atacado, quando aplicado.</returns>
        int? CarregarCategoriaAtacado(ImportarInventarioAutomaticoRequest request);

        /// <summary>
        /// Obtém a lista de prefixos de arquivos de importação considerados válidos conforme a parametrização atual do sistema.
        /// </summary>
        /// <returns>Lista de prefixos válidos.</returns>
        IEnumerable<string> ObterPrefixosArquivos();
    }
}
