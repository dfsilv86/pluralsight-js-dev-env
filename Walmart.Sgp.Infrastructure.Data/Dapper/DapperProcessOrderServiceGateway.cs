using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Processing;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para process order service utilizando o Dapper.
    /// </summary>
    public class DapperProcessOrderServiceGateway : EntityDapperDataGatewayBase<ProcessOrderService>, IProcessOrderServiceGateway
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperProcessOrderServiceGateway"/>.
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        public DapperProcessOrderServiceGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "ProcessOrderService", "ProcessOrderServiceId")
        {
        }

        /// <summary>
        /// Obtém o nome das colunas que devem ser consideradas nas operações de SELECT, INSERT e UPDATE.
        /// </summary>
        protected override IEnumerable<string> ColumnsName
        {
            get { return new string[] { "ProcessOrderId", "ServiceTypeName", "ServiceMethodName", "ResultTypeFullName", "ResultFilePath", "MaxGlobal", "MaxPerUser", "RoleId", "StoreId", "BandeiraId" }; }
        }

        /// <summary>
        /// Atualiza uma entidade existente.
        /// </summary>
        /// <param name="entity">A entidade a ser atualizada. Deve possuir a propriedade Id preenchida.</param>
        /// <remarks>
        /// Será atualizada a entidade que possui o Id informado no modelo.
        /// </remarks>
        public override void Update(ProcessOrderService entity)
        {
            base.Update(
                "ResultTypeFullName=@ResultTypeFullName,ResultFilePath=@ResultFilePath",
                "ProcessOrderServiceId=@ProcessOrderServiceId",
                new { entity.ProcessOrderServiceId, entity.ResultFilePath, entity.ResultTypeFullName });
        }
    }
}
