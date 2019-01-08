using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.MultisourcingVendor;
using Walmart.Sgp.Infrastructure.Data.Databases;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para Multisourcing utilizando o Dapper.
    /// </summary>
    public class DapperMultisourcingGateway : EntityDapperDataGatewayBase<Multisourcing>, IMultisourcingGateway
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperMultisourcingGateway"/>.
        /// </summary>
        /// <param name="databases">O registro de bases de dados da aplicação.</param>
        public DapperMultisourcingGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "Multisourcing", "IDMultisourcing")
        {
        }

        /// <summary>
        /// Obtém o nome das colunas que devem ser consideradas nas operações de SELECT, INSERT e UPDATE.
        /// </summary>
        protected override IEnumerable<string> ColumnsName
        {
            get
            {
                return new string[] { "IDCD", "IDRelacionamentoItemSecundario", "vlPercentual", "cdUsuarioInclusao", "dtInclusao", "cdUsuarioAlteracao", "dtAlteracao" };
            }
        }

        /// <summary>
        /// Obtém um Multisourcing pelo seu id.
        /// </summary>
        /// <param name="id">O id.</param>
        /// <returns>O Multisourcing.</returns>
        public Multisourcing Obter(long id)
        {
            return this.Find("IDMultisourcing=@IDMultisourcing", new { IDMultisourcing = id }).SingleOrDefault();
        }

        /// <summary>
        /// Obtém o multisourcing pelo idRelacionamentoItemSecundario e idCD.
        /// </summary>
        /// <param name="idRelacionamentoItemSecundario">O id do item de relacionamento secundário (entrada).</param>
        /// <param name="idCD">O id do CD.</param>
        /// <returns>O multisourcing.</returns>
        public Multisourcing ObterPorRelacionamentoItemSecundarioECD(long idRelacionamentoItemSecundario, int idCD)
        {
            return this.Find(
                "IDRelacionamentoItemSecundario=@IDRelacionamentoItemSecundario AND IDCD=@idCD",
                new { IDRelacionamentoItemSecundario = idRelacionamentoItemSecundario, IDCD = idCD }).SingleOrDefault();
        }

        /// <summary>
        /// Obtem uma lista de Multisourcing pelo item de saida e CD.
        /// </summary>
        /// <param name="cdItemSaida">O código do item de saída.</param>
        /// <param name="cdCD">O código do CD.</param>
        /// <returns>Lista de multisourcing.</returns>
        public IEnumerable<Multisourcing> ObterPorCdItemSaidaEcdCD(long cdItemSaida, long cdCD)
        {
            var args = new { cdItemSaida = cdItemSaida, cdCD = cdCD };

            return this.Resource.Query<Multisourcing>(Sql.Multisourcing.ObterPorCdItemSaidaEcdCD, args);
        }

        /// <summary>
        /// Verifica se o relacionamento possui um cadastro de multisorucing.
        /// </summary>
        /// <param name="rip">A entidade do relacionamento.</param>
        /// <returns>Se o relacionamento possui cadastro.</returns>
        public bool VerificaRelacionamentoExistente(RelacionamentoItemPrincipal rip)
        {
            var args = new { idRip = rip.IDRelacionamentoItemPrincipal };

            var result = this.Resource.QueryOne<int>(Sql.Multisourcing.VerificarRipVinculado, args);

            return result > 0;
        }

        /// <summary>
        /// Obtem um Multisourcing pelo Id com seus ItemDetalheEntrada, ItemDetalheSaida e FornecedorParametro
        /// </summary>
        /// <param name="id">O id do Multisourcing.</param>
        /// <returns>O Multisourcing.</returns>
        public Multisourcing ObterComItemDetalhesEFp(long id)
        {
            var args = new { idMs = id };

            return this.Resource.Query<Multisourcing, FornecedorParametro, ItemDetalhe, ItemDetalhe, Multisourcing>(
                Sql.Multisourcing.ObterComDetalhes,
                args,
                (multisourcing, fornecedorParametro, ide, ids) =>
                {
                    multisourcing.Fornecedor = new Fornecedor();
                    multisourcing.Fornecedor.Parametros.Add(fornecedorParametro);

                    multisourcing.ItemDetalheEntrada = ide;
                    multisourcing.ItemDetalheSaida = ids;

                    return multisourcing;
                },
                "SplitOn1,SplitOn2,SplitOn3").SingleOrDefault();
        }

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
        public IEnumerable<Multisourcing> ObtemItemDetalheSaidaEntradaMultisourcing(long? idItemDetalhe, int idDepartamento, int cdSistema, int? idCD, int filtroMS, int filtroCadastro)
        {
            return this.Resource.Query<ItemDetalhe, dynamic, FornecedorParametro, Fornecedor, CD, Multisourcing, Multisourcing>(
                Sql.Multisourcing.ObtemItemDetalheSaidaEntradaMultisourcing,
                new { idItemDetalheSaida = idItemDetalhe, idDepartamento, cdSistema, idCD, filtroMS, filtroCadastro },
                MapItemDetalheSaidaEntradaMultisourcing,
                "idItemDetalheEntrada,SplitOn2,SplitOn3,SplitOn4,SplitOn5").ToList();
        }

        private Multisourcing MapItemDetalheSaidaEntradaMultisourcing(ItemDetalhe itemDetalheSaida, dynamic itemDetalheEntrada, FornecedorParametro fornecedorParametro, Fornecedor fornecedor, CD cd, Multisourcing multisourcing)
        {
            multisourcing.ItemDetalheSaida = new ItemDetalhe
            {
                IDItemDetalhe = itemDetalheSaida.IDItemDetalhe,
                CdItem = itemDetalheSaida.CdItem,
                DsItem = itemDetalheSaida.DsItem
            };

            multisourcing.ItemDetalheEntrada = new ItemDetalhe
            {
                IDItemDetalhe = (int)itemDetalheEntrada.idItemDetalheEntrada,
                CdItem = itemDetalheEntrada.cdItemDetalheEntrada,
                DsItem = itemDetalheEntrada.dsItemDetalheEntrada,
                QtVendorPackage = itemDetalheEntrada.qtVendorPackage,
                VlPesoLiquido = itemDetalheEntrada.vlPesoLiquido
            };

            fornecedor.Parametros.Add(fornecedorParametro);
            multisourcing.Fornecedor = fornecedor;
            multisourcing.CD = cd.cdCD;

            return multisourcing;
        }
    }
}