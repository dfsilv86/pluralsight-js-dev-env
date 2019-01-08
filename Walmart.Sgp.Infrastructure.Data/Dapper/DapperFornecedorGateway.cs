using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Infrastructure.Data.Common;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para Fornecedor utilizando o Dapper.
    /// </summary>
    public class DapperFornecedorGateway : EntityDapperDataGatewayBase<Fornecedor>, IFornecedorGateway
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperFornecedorGateway" />.
        /// </summary>
        /// <param name="databases">O registro de bases de dados da aplicação.</param>
        public DapperFornecedorGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "Fornecedor", "IDFornecedor")
        {
        }

        /// <summary>
        /// Obtém o nome das colunas que devem ser consideradas nas operações de SELECT, INSERT e UPDATE.
        /// </summary>
        protected override IEnumerable<string> ColumnsName
        {
            get
            {
                return new string[] { "cdFornecedor", "nmFornecedor", "blAtivo", "dhCriacao", "dhAtualizacao", "cdUsuarioCriacao", "cdUsuarioAtualizacao", "stFornecedor", "cdSistema" };
            }
        }

        /// <summary>
        /// Obtém um Fornecedor por cdSistema e cdFornecedor.
        /// </summary>
        /// <param name="cdSistema">O sistema.</param>
        /// <param name="cdFornecedor">O codigo do fornecedor.</param>
        /// <returns>A entidade Fornecedor.</returns>
        public Fornecedor ObterPorSistemaCodigo(short cdSistema, int cdFornecedor)
        {
            string nmFornecedor = null;

            return this.Resource.QueryOne<Fornecedor>(Sql.Fornecedor.ObterPorSistemaCodigoNome, new { cdSistema, cdFornecedor, nmFornecedor });
        }

        /// <summary>
        /// Obtém uma lista de Fornecedor por cdSistema e cdFornecedor.
        /// </summary>
        /// <param name="cdSistema">O sistema.</param>
        /// <param name="cdFornecedor">O codigo do fornecedor.</param>
        /// <param name="nmFornecedor">O nome do fornecedor.</param>
        /// <param name="paging">Paginação do resultado.</param>
        /// <returns>A lista de entidade Fornecedor.</returns>
        public IEnumerable<Fornecedor> ObterListaPorSistemaCodigoNome(short cdSistema, int? cdFornecedor, string nmFornecedor, Paging paging)
        {
            return this.Resource.Query<Fornecedor>(Sql.Fornecedor.ObterPorSistemaCodigoNome, new { cdSistema, cdFornecedor, nmFornecedor }).AsPaging(paging);
        }

        /// <summary>
        /// Obtém o fornecedor ativo com base no código e estrutura mercadológica.
        /// </summary>
        /// <param name="cdV9D">O código do fornecedor.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>Retorna o fornecedor juntamente com os parâmetros ativos.</returns>
        public Fornecedor ObterAtivoPorCodigoESistemaComProjecao(long cdV9D, int cdSistema)
        {
            Dictionary<long, Fornecedor> result = new Dictionary<long, Fornecedor>();

            this.Resource.Query<Fornecedor, FornecedorParametro, Fornecedor>(
                Sql.Fornecedor.ObterAtivoPorCodigoESistemaComProjecao,
                new { cdV9D, cdSistema },
                (fornecedor, fornecedorParametro) =>
                {
                    if (!result.ContainsKey(fornecedor.IDFornecedor))
                    {
                        result[fornecedor.IDFornecedor] = fornecedor;
                    }

                    Fornecedor item = result[fornecedor.IDFornecedor];

                    if (!item.Parametros.Any(i => i.IDFornecedorParametro == fornecedorParametro.IDFornecedorParametro))
                    {
                        item.Parametros.Add(fornecedorParametro);
                    }

                    return item;
                },
                "SplitOn1").ToList();

            return result.Values.SingleOrDefault();
        }

        /// <summary>
        /// Obtém um fornecedor pelo seu id.
        /// </summary>
        /// <param name="id">O id.</param>
        /// <returns>O fornecedor.</returns>
        public Fornecedor Obter(long id)
        {
            return this.Find("IDFornecedor=@IDFornecedor", new { IDFornecedor = id }).SingleOrDefault();
        }

        /// <summary>
        /// Retorna true se o cdFornecedor informado é do tipo Walmart.
        /// </summary>
        /// <param name="cdFornecedor">O codigo do fornecedor.</param>
        /// <returns>Se o fornecedor é Walmart ou não.</returns>
        public bool VerificaVendorWalmart(long cdFornecedor)
        {
            var result = Resource.ExecuteScalar<int>(Sql.Fornecedor.VerificaVendorWalmart, new { CdFornecedor = cdFornecedor });

            return result > 0;
        }
    }
}
