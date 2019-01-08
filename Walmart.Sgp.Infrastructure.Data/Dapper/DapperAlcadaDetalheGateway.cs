using System;
using System.Collections.Generic;
using System.Linq;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para AlcadaDetalhe utilizando o Dapper.
    /// </summary>
    public class DapperAlcadaDetalheGateway : EntityDapperDataGatewayBase<AlcadaDetalhe>, IAlcadaDetalheGateway
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperAlcadaDetalheGateway" />.
        /// </summary>
        /// <param name="databases">O registro de bases de dados da aplicação.</param>
        public DapperAlcadaDetalheGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "AlcadaDetalhe", "IDAlcadaDetalhe")
        {
        }

        /// <summary>
        /// Obtém o nome das colunas que devem ser consideradas nas operações de SELECT, INSERT e UPDATE.
        /// </summary>
        protected override IEnumerable<string> ColumnsName
        {
            get
            {
                return new string[] { "idAlcada", "idRegiaoAdministrativa", "idBandeira", "idDepartamento", "vlPercentualAlterado" };
            }
        }

        /// <summary>
        /// Obter uma lista de AlcadaDetalhe por IdAlcada.
        /// </summary>
        /// <param name="idAlcada">O id da Alcada.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>A lista de AlcadaDetalhe.</returns>
        public IEnumerable<AlcadaDetalhe> ObterPorIdAlcada(int idAlcada, Paging paging)
        {
            var result = this.Resource.Query<AlcadaDetalhe, RegiaoAdministrativa, Bandeira, Departamento, AlcadaDetalhe>(
                Sql.AlcadaDetalhe.ObterPorIdAlcada,
                new { idAlcada },
                (a, r, b, d) =>
                {
                    a.RegiaoAdministrativa = r;
                    a.RegiaoAdministrativa.IdRegiaoAdministrativa = a.IDRegiaoAdministrativa;

                    a.Bandeira = b;
                    a.Bandeira.IDBandeira = a.IDBandeira;

                    a.Departamento = d;
                    a.Departamento.IDDepartamento = a.IDDepartamento;

                    return a;
                },
                "SplitOn1,SplitOn2,IDDivisao");

            if (paging == null)
            {
                return result;
            }

            return result.AsPaging(paging);
        }
    }
}
