using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para relacionamento transferencia utilizando o Dapper.
    /// </summary>
    public class DapperRelacionamentoTransferenciaGateway : EntityDapperDataGatewayBase<RelacionamentoTransferencia>, IRelacionamentoTransferenciaGateway
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperRelacionamentoTransferenciaGateway"/>.
        /// </summary>
        /// <param name="databases">O registro de bases de dados da aplicação.</param>
        public DapperRelacionamentoTransferenciaGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "RelacionamentoTransferencia", "IDRelacionamentoTransferencia")
        {
        }

        /// <summary>
        /// Obtém o nome das colunas que devem ser consideradas nas operações de SELECT, INSERT e UPDATE.
        /// </summary>
        protected override IEnumerable<string> ColumnsName
        {
            get
            {
                return new string[] { "IDItemDetalheOrigem", "IDItemDetalheDestino", "IDLoja", "dtCriacao", "IDUsuario", "blAtivo", "dtInativo" };
            }
        }

        /// <summary>
        /// Obtém um item pelo seu id e retorna o item com informações das entidades associadas.
        /// </summary>
        /// <param name="id">O id do relacionamento transferencia.</param>
        /// <returns>O RelacionamentoTransferencia com informações de ItemDetalheOrigem, ItemDetalheDestino, Loja e Usuario.</returns>
        public RelacionamentoTransferencia ObterEstruturadoPorId(int id)
        {
            // TODO: QueryOne<> com overloads para mapear multiplas classes
            return this.Resource.Query<RelacionamentoTransferencia, ItemDetalhe, Departamento, ItemDetalhe, Departamento, Loja, Usuario, RelacionamentoTransferencia>(
                Sql.RelacionamentoTransferencia.ObterEstruturadoPorId,
                new { id },
                MapRelacionamentoTransferencia,
                "SplitOn1,SplitOn2,SplitOn3,SplitOn4,SplitOn5,SplitOn6").SingleOrDefault();
        }

        /// <summary>
        /// Pesquisa os itens relacionados de acordo com o id do item destino
        /// </summary>
        /// <param name="idItemDetalheDestino">O id do item destino</param>
        /// <param name="paging">A paginacao</param>
        /// <returns>Os itens com relacionamento</returns>
        public IEnumerable<RelacionamentoTransferencia> PesquisarItensRelacionados(long idItemDetalheDestino, Paging paging)
        {
            var args = new
            {
                idItemDetalheDestino = idItemDetalheDestino
            };

            return this.Resource.Query<RelacionamentoTransferencia, Loja, ItemDetalhe, RelacionamentoTransferencia>(
                Sql.RelacionamentoTransferencia.PesquisarItensRelacionados,
                args,
                MapItensRelacionados,
                "SplitOn1,SplitOn2").AsPaging(paging);
        }

        /// <summary>
        /// Pesquisa detalhe de relacionamentos transferencia pelos filtros informados
        /// </summary>
        /// <param name="filtro">Os filtros</param>
        /// <param name="paging">A paginacao</param>
        /// <returns>Os relacionamentos transferencia</returns>
        public IEnumerable<RelacionamentoTransferenciaConsolidado> PesquisarPorFiltro(RelacionamentoTransferenciaFiltro filtro, Paging paging)
        {
            var args = new
            {
                idBandeira = filtro.IdBandeira,
                idLoja = filtro.IdLoja,
                idDepartamento = filtro.IdDepartamento,
                cdItem = filtro.CdItem,
                dsItem = filtro.DsItem
            };

            return Resource.Query<RelacionamentoTransferenciaConsolidado>(Sql.RelacionamentoTransferencia.PesquisarPorFiltro, args).AsPaging(paging);
        }

        /// <summary>
        /// Obtem a quantidade de registros pelo item destino
        /// </summary>
        /// <param name="idItemDetalheDestino">O id do item destino</param>
        /// <returns>A quantidade de registros</returns>
        public int ObterQuantidadePorItemDestino(long idItemDetalheDestino)
        {
            return this.Resource.ExecuteScalar<int?>(Sql.RelacionamentoTransferencia.ObterQuantidadePorItemDestino, new { idItemDetalheDestino }) ?? 0;
        }

        /// <summary>
        /// Pesquina os relacionamento conforme os filtros
        /// </summary>
        /// <param name="idItemDetalheDestino">O id do item detalhe</param>
        /// <param name="idItemDetalheOrigem">O id do item origem</param>
        /// <param name="idLoja">O id da loja</param>
        /// <param name="blAtivo">O status</param>
        /// <returns>Os relacionamentos</returns>
        public IEnumerable<RelacionamentoTransferencia> PesquisarPorItemDestinoOrigemLojaAtivo(long idItemDetalheDestino, long idItemDetalheOrigem, int idLoja, bool blAtivo = true)
        {
            var args = new
            {
                idItemDetalheDestino = idItemDetalheDestino,
                idItemDetalheOrigem = idItemDetalheOrigem,
                idLoja = idLoja,
                blAtivo = blAtivo ? 1 : 0
            };

            return this.Resource.Query<RelacionamentoTransferencia>(Sql.RelacionamentoTransferencia.PesquisarPorItemDestinoOrigemLojaStatus, args);
        }

        /// <summary>
        /// Pesquisa os itens relacionados de acordo com o id do item destino
        /// </summary>
        /// <param name="cdItemDestino">O codigo do item destino</param>
        /// <param name="paging">A paginacao</param>
        /// <returns>Os itens com relacionamentos</returns>
        public IEnumerable<RelacionamentoTransferencia> PesquisarItensRelacionadosPorCdItemDestino(long cdItemDestino, Paging paging)
        {
            var args = new
            {
                cdItemDestino = cdItemDestino
            };

            return this.Resource.Query<RelacionamentoTransferencia, Loja, ItemDetalhe, RelacionamentoTransferencia>(
                Sql.RelacionamentoTransferencia.PesquisarItensRelacionadosPorCdItemDestino,
                args,
                MapItensRelacionados,
                "SplitOn1,SplitOn2").AsPaging(paging);
        }

        private RelacionamentoTransferencia MapRelacionamentoTransferencia(RelacionamentoTransferencia relacionamentoTransferencia, ItemDetalhe itemDetalheOrigem, Departamento departamentoOrigem, ItemDetalhe itemDetalheDestino, Departamento departamentoDestino, Loja loja, Usuario usuario)
        {
            relacionamentoTransferencia.ItemDetalheOrigem = itemDetalheOrigem;
            relacionamentoTransferencia.ItemDetalheOrigem.Departamento = departamentoOrigem;
            relacionamentoTransferencia.ItemDetalheDestino = itemDetalheDestino;
            relacionamentoTransferencia.ItemDetalheDestino.Departamento = departamentoDestino;
            relacionamentoTransferencia.Loja = loja;
            relacionamentoTransferencia.Usuario = usuario;
            return relacionamentoTransferencia;
        }

        private RelacionamentoTransferencia MapItensRelacionados(RelacionamentoTransferencia relacionamentoTransferencia, Loja loja, ItemDetalhe itemDetalheOrigem)
        {
            relacionamentoTransferencia.Loja = loja;
            relacionamentoTransferencia.ItemDetalheOrigem = itemDetalheOrigem;
            return relacionamentoTransferencia;
        }
    }

}
