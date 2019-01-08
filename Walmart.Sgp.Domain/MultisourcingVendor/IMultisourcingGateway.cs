using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.MultisourcingVendor
{
    /// <summary>
    /// Define a interface de um table data gateway para Multisourcing.
    /// </summary>
    public interface IMultisourcingGateway : IDataGateway<Multisourcing>
    {
        /// <summary>
        /// Obtém um Multisourcing pelo seu id.
        /// </summary>
        /// <param name="id">O id.</param>
        /// <returns>O Multisourcing.</returns>
        Multisourcing Obter(long id);

        /// <summary>
        /// Obtém o multisourcing pelo idRelacionamentoItemSecundario e idCD.
        /// </summary>
        /// <param name="idRelacionamentoItemSecundario">O id do item de relacionamento secundário (entrada).</param>
        /// <param name="idCD">O id do CD.</param>
        /// <returns>O multisourcing.</returns>
        Multisourcing ObterPorRelacionamentoItemSecundarioECD(long idRelacionamentoItemSecundario, int idCD);
                        
        /// <summary>
        /// Obtem um Multisourcing pelo Id com seus ItemDetalheEntrada, ItemDetalheSaida e FornecedorParametro
        /// </summary>
        /// <param name="id">O id do Multisourcing.</param>
        /// <returns>O Multisourcing.</returns>
        Multisourcing ObterComItemDetalhesEFp(long id);

        /// <summary>
        /// Obtem uma lista de Multisourcing pelo item de saida e CD.
        /// </summary>
        /// <param name="cdItemSaida">O código do item de saída.</param>
        /// <param name="cdCD">O código do CD.</param>
        /// <returns>Retorna uma lista de multisourcing.</returns>
        IEnumerable<Multisourcing> ObterPorCdItemSaidaEcdCD(long cdItemSaida, long cdCD);

        /// <summary>
        /// Verifica se o relacionamento possui um cadastro de multisorucing.
        /// </summary>
        /// <param name="rip">A entidade do relacionamento.</param>
        /// <returns>Se o relacionamento possui cadastro.</returns>
        bool VerificaRelacionamentoExistente(RelacionamentoItemPrincipal rip);

        /// <summary>
        /// Obtém os registros de item detalhe entrada e saída, juntamente com o multisourcing caso esteja cadastrado, utilizando como base os filtros informados.
        /// </summary>
        /// <param name="idItemDetalhe">O id do item detalhe de saída.</param>
        /// <param name="idDepartamento">O id do departamento.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <param name="idCD">O id do CD.</param>
        /// <param name="filtroMS">Se deve incluir itens possível multisourcing (1 - Sim, 0 - Não, 2 - Todos).</param>
        /// <param name="filtroCadastro">Incluir itens que possuem cadastro (1 - Sim, 0 - Não, 2 - Todos).</param>
        /// <returns>Retorna os registros de item detalhe entrada e saída, juntamente com o multisourcing caso esteja cadastrado, utilizando como base os filtros informados.</returns>
        IEnumerable<Multisourcing> ObtemItemDetalheSaidaEntradaMultisourcing(long? idItemDetalhe, int idDepartamento, int cdSistema, int? idCD, int filtroMS, int filtroCadastro);
    }
}
