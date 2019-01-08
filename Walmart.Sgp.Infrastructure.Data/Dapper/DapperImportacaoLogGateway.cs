using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Infrastructure.Data.Common;
using Walmart.Sgp.Infrastructure.Data.Databases;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Gateway para o log de importação de inventário.
    /// </summary>
    public class DapperImportacaoLogGateway : DapperDataGatewayBase<string>, ILeitorLogger
    {
        #region Constructor
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperImportacaoLogGateway"/>.
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        public DapperImportacaoLogGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp)
        {
        }
        #endregion

        #region ILeitorLogger

        /// <summary>
        /// Insere log informativo de processamento.
        /// </summary>
        /// <param name="nomeAcao">O nome da ação.</param>
        /// <param name="mensagem">A mensagem informativa.</param>
        public void InserirLogProcessamento(string nomeAcao, string mensagem)
        {
            this.StoredProcedure.Execute("PR_InventarioImportacaoAcao_Incluir", new { Nome_Acao = nomeAcao, Desc_Acao = mensagem });
        }

        /// <summary>
        /// Insere log de erro de processamento.
        /// </summary>
        /// <param name="nomeAcao">O nome da ação.</param>
        /// <param name="mensagemErro">A mensagem de erro.</param>
        public void InserirLogErroProcessamento(string nomeAcao, string mensagemErro)
        {
            this.StoredProcedure.Execute("PR_InventarioImportacaoErro_Incluir", new { Nome_Acao = nomeAcao, Desc_Erro = mensagemErro });
        }

        /// <summary>
        /// Insere log de crítica do arquivo.
        /// </summary>
        /// <param name="idLoja">O id de loja.</param>
        /// <param name="mensagemCritica">A descrição da crítica.</param>
        /// <param name="idInventarioCriticaTipo">O id de inventario critica tipo.</param>
        /// <param name="idInventario">O id de inventario.</param>
        /// <param name="idDepartamento">O id de departamento.</param>
        /// <param name="idCategoria">O id de categoria.</param>
        /// <param name="dataInventario">A data de inventário.</param>
        public void InserirInventarioCritica(int idLoja, string mensagemCritica, short idInventarioCriticaTipo, int? idInventario, int? idDepartamento, long? idCategoria, DateTime? dataInventario)
        {
            this.StoredProcedure.Execute("PR_InventarioCritica_Incluir", new { idLoja, dsCritica = mensagemCritica, idInventarioCriticaTipo, idInventario, idDepartamento, idCategoria, dhInventario = dataInventario });
        }

        /// <summary>
        /// Exclui log de críticas anteriores.
        /// </summary>
        /// <param name="idLoja">O id de loja.</param>
        /// <param name="dataInventario">A data de inventario.</param>
        /// <param name="idDepartamento">O id de departamento.</param>
        /// <param name="idCategoria">O id de categoria.</param>
        /// <remarks>Exclusão lógica.</remarks>
        public void ExcluirInventarioCritica(int idLoja, DateTime dataInventario, int? idDepartamento, int? idCategoria)
        {
            // Exclusão lógica
            this.Command.Execute(SqlResourceReader.Read(Sql.InventarioCritica.ExcluirInventarioCritica), new { idLoja, dataInventario, idDepartamento, idCategoria });
        }

        /// <summary>
        /// Critica inventário conforme existência de custos apurados.
        /// </summary>
        /// <param name="idInventario">O id do inventário.</param>
        /// <returns>A quantidade de críticas.</returns>
        public int ApurarCriticaInventarioSemCusto(int idInventario)
        {
            return this.StoredProcedure.ExecuteScalar<int>("PR_IncluirExcluirCriticaItensSemCustoApurado", new { IDInventario = idInventario });
        }

        #endregion
    }
}
