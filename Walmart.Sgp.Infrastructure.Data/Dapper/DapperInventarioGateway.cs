using System;
using System.Collections.Generic;
using System.Linq;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Infrastructure.Data.Common;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Data.Dtos;
using Walmart.Sgp.Infrastructure.Framework.Commons;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para inventário utilizando o Dapper.
    /// </summary>
    public class DapperInventarioGateway : EntityDapperDataGatewayBase<Inventario>, IInventarioGateway
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperInventarioGateway"/>.
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        public DapperInventarioGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "Inventario", "IDInventario")
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém o nome das colunas que devem ser consideradas nas operações de SELECT, INSERT e UPDATE.
        /// </summary>
        protected override IEnumerable<string> ColumnsName
        {
            get
            {
                return new string[]
                {
                    "IDLoja",
                    "IDDepartamento",
                    "IDCategoria",
                    "IDBandeira",
                    "dhInventario",
                    "tpImportacao",
                    "dhImportacao",
                    "cdUsuarioImportacao",
                    "dhAprovacaoLoj",
                    "cdUsuarioAprovacaoLoj",
                    "dhAprovacaoOpe",
                    "cdUsuarioAprovacaoOpe",
                    "dhCancelamentoLoj",
                    "cdUsuarioCancelamentoLoj",
                    "dhCancelamentoOpe",
                    "cdUsuarioCancelamentoOpe",
                    "dhContabilizacao",
                    "stInventario",
                    "tpInventario",
                    "dhFinalizacaoLoj",
                    "cdUsuarioFinalizacaoLoj",
                    "dhAberturaLoj",
                    "cdUsuarioAberturaLoj",
                    "dhInventarioArquivo",
                };
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Localiza a data do inventário da loja informada.
        /// </summary>
        /// <param name="idLoja">O id da loja.</param>
        /// <returns>A data do inventário, se existir.</returns>
        public DateTime? ObterDataInventarioDaLoja(int idLoja)
        {
            return StoredProcedure.ExecuteScalar<DateTime?>("ObtemDataInventario", new { IdLoja = idLoja });
        }

        /// <summary>
        /// Obtém o número de lojas sem agendamento.
        /// A loja só é considerada como agendada quando todos seus respectivos departamentos por sistema estão agendados.
        /// </summary>
        /// <param name="idUsuario">O id do usuário para verificação do sistema.</param>
        /// <returns>Quantidade de lojas sem agendamento.</returns>
        public int ObterQuantidadeLojasSemAgendamento(int idUsuario)
        {
            return StoredProcedure.ExecuteScalar<int>("PR_LojasNaoAgendadas", new { idUsuario });
        }

        /// <summary>
        /// Obtém a lista de inventários agendados e abertos.
        /// </summary>
        /// <param name="idLoja">O id da loja.</param>
        /// <param name="dataInventario">A data do inventário.</param>
        /// <param name="idDepartamento">O id do departamento.</param>
        /// <param name="idCategoria">O id da categoria.</param>
        /// <returns>Os Inventario que estão abertos.</returns>
        public IEnumerable<Inventario> ObterInventariosAbertosParaImportacao(int idLoja, DateTime? dataInventario, int? idDepartamento, int? idCategoria)
        {
            return StoredProcedure.Query<Inventario>("PR_Get_InventariosAbertosParaImportacao", new { idLoja, dhInventario = dataInventario, idDepartamento, idCategoria });
        }

        /// <summary>
        /// Realiza o cancelamento do inventário informado.
        /// </summary>
        /// <param name="inventario">O inventário.</param>
        public void CancelarInventario(Inventario inventario)
        {
            StoredProcedure.Execute(
                "PR_InventarioCancelar",
                new
                {
                    IdInventario = inventario.IDInventario,
                    IdUsuario = RuntimeContext.Current.User.Id,
                    IdBandeira = inventario.IDBandeira,
                    IdLoja = inventario.IDLoja,
                    DhInventario = inventario.dhInventario
                });
        }

        /// <summary>
        /// Obtém os inventários que correspondem ao filtro especificado.
        /// </summary>
        /// <param name="filtro">O filtro.</param>
        /// <param name="paging">A paginacao</param>
        /// <returns>Os inventários.</returns>
        public IEnumerable<InventarioSumario> ObterSumarizadoPorFiltro(InventarioFiltro filtro, Paging paging)
        {
            return Resource.Query<InventarioSumario, Loja, Departamento, Categoria, InventarioSumario>(
                Sql.Inventario.ObterSumarizadoPorFiltro,
                new
                {
                    filtro.DhInventario,
                    filtro.IdBandeira,
                    filtro.IdCategoria,
                    filtro.IdDepartamento,
                    filtro.IdLoja,
                    stInventario = (int?)filtro.StInventario
                },
                MapearInventarioSumario,
                "SplitOn1,SplitOn2,SplitOn3").AsPaging(paging);
        }

        /// <summary>
        /// Obtém o custo total por filtro.
        /// </summary>
        /// <param name="filtro">O filtro.</param>
        /// <returns>O valor do custo total.</returns>
        public decimal ObterCustoTotalPorFiltro(InventarioFiltro filtro)
        {
            return Resource.ExecuteScalar<decimal>(
                Sql.Inventario.ObterCustoTotalPorFiltro,
                new
                {
                    filtro.DhInventario,
                    filtro.IdBandeira,
                    filtro.IdCategoria,
                    filtro.IdDepartamento,
                    filtro.IdLoja,
                    stInventario = (int?)filtro.StInventario
                });
        }

        /// <summary>
        /// Conta os inventários para a loja, departamento que estão no intervalo de data e que possuem um dos status informados.
        /// </summary>
        /// <param name="idLoja">O id da loja.</param>
        /// <param name="idDepartamento">O id do departamento.</param>
        /// <param name="intervaloDhInventario">O intervalo para a data do inventário.</param>
        /// <param name="stInventarios">Os status do inventario.</param>
        /// <returns>O número de inventários.</returns>
        public int ContarInventarios(int idLoja, int idDepartamento, RangeValue<DateTime> intervaloDhInventario, params InventarioStatus[] stInventarios)
        {
            return Resource.ExecuteScalar<int>(
                Sql.Inventario.ContarInventarios,
                new
                {
                    idLoja,
                    idDepartamento,
                    inicioDhInventario = intervaloDhInventario.StartValue,
                    fimDhInventario = intervaloDhInventario.EndValue,
                    stInventarios = stInventarios.Select(s => s.Value)
                });
        }

        /// <summary>
        /// Obtém o inventário estruturado pelo id especificado.
        /// </summary>
        /// <param name="id">O id do inventário.</param>
        /// <returns>
        /// O inventário estruturado.
        /// </returns>
        public InventarioSumario ObterEstruturadoPorId(int id)
        {
            return
                Resource.Query<InventarioSumario, Loja, Bandeira, Departamento, Categoria, InventarioUsuariosDto, InventarioSumario>(
                    Sql.Inventario.ObterEstruturadoPorId,
                    new { id },
                    MapearInventarioSumario,
                    "SplitOn1,SplitOn2,SplitOn3,SplitOn4,SplitOn5")
                    .FirstOrDefault();
        }

        /// <summary>
        /// Obtém o ultimo inventário da loja.
        /// </summary>
        /// <param name="idLoja">O id da loja.</param>
        /// <param name="idDepartamento">O id do departamento.</param>
        /// <param name="dhUltimoFechamentoFiscalLoja">A data do ultimo fechamento fiscal da loja.</param>
        /// <returns>
        /// O ultimo inventario da loja ou <c>null</c> caso a loja não possua inventários.
        /// </returns>
        public Inventario ObterUltimo(int idLoja, int? idDepartamento, DateTime dhUltimoFechamentoFiscalLoja)
        {
            return Resource.QueryOne<Inventario>(
                Sql.Inventario.ObterUltimoPorLojaDepartamento,
                new { idLoja, idDepartamento, dhUltimoFechamentoFiscalLoja });
        }

        /// <summary>
        /// Retorna um valor indicando se o inventario especificado possui
        /// itens inativos ou deletados.
        /// </summary>
        /// <param name="idInventario">O id de inventario.</param>
        /// <returns>
        /// <c>true</c> se o inventário possui itens inativos ou
        /// deletados; caso contrario <c>false</c>.
        /// </returns>
        public bool PossuiItemInativoDeletado(int idInventario)
        {
            return Resource.ExecuteScalar<bool>(
                Sql.Inventario.PossuiItemInativoDeletado,
                new { id = idInventario });
        }

        /// <summary>
        /// Retorna um valor indicando se o inventario especificado
        /// possui sortimento inválido.
        /// </summary>
        /// <param name="idInventario">O id de inventario.</param>
        /// <returns>
        /// <c>true</c> se o possui sortimento invalido;
        /// caso contrário <c>false</c>.
        /// </returns>
        public bool PossuiSortimentoInvalido(int idInventario)
        {
            return Resource.ExecuteScalar<bool>(
                Sql.Inventario.PossuiSortimentoInvalido,
                new { id = idInventario });
        }

        /// <summary>
        /// Retorna um valor indicando se o inventario especificado
        /// possui itens com custo de cadastro.
        /// </summary>
        /// <param name="idInventario">O id de inventario.</param>
        /// <returns>
        /// <c>true</c> se algum item possui custo de cadastro;
        /// caso contrário <c>false</c>.
        /// </returns>
        public bool PossuiItemComCustoDeCadastro(int idInventario)
        {
            return Resource.ExecuteScalar<bool>(
                Sql.Inventario.PossuiItemComCustoDeCadastro,
                new { id = idInventario });
        }

        /// <summary>
        /// Finaliza o inventário especificado.
        /// </summary>
        /// <param name="inventario">O inventário.</param>
        public void Finalizar(Inventario inventario)
        {
            StoredProcedure.Execute(
                "PR_InventarioFinalizar",
                new
                {
                    inventario.IDInventario,
                    inventario.IDBandeira,
                    inventario.dhInventario,
                    inventario.IDLoja,
                    IDUsuario = RuntimeContext.Current.User.Id
                });
        }

        /// <summary>
        /// Obtém o inventário aprovado e finalizado na data especificada.
        /// </summary>
        /// <param name="request">O filtro.</param>
        /// <returns>O inventário.</returns>
        public Inventario ObterInventarioAprovadoFinalizadoMesmaData(ImportarInventarioAutomaticoRequest request)
        {
            int? idCategoria = null;
            int? idDepartamento = null;

            if (request.CdSistema == 1)
            {
                idDepartamento = this.Command.ExecuteScalar<int>("SELECT D.IDDepartamento FROM Departamento D WITH (NOLOCK) WHERE D.CdSistema=1 AND D.blAtivo=1 AND D.CdDepartamento = @CdDepartamento", new { cdDepartamento = request.CdDepartamento });
            }
            else
            {
                // Na tabela Categoria, quando CdSistema=2, então CdCategoria = CdDepartamento da tabela departamento (mesmo nome)
                // Não existe conceito de departamento para atacado, apenas categoria em diante
                idCategoria = this.Command.ExecuteScalar<int>("SELECT C.IDCategoria FROM Categoria C WITH (NOLOCK) WHERE C.CdSistema=2 AND C.blAtivo=1 AND C.CdCategoria = @CdDepartamento -- isso nao e um typo", new { cdDepartamento = request.CdDepartamento });
            }

            return this.StoredProcedure.QueryOne<Inventario>("PR_Get_InventarioAprovadoFinalizadoMesmaData", new { IdLoja = request.IdLoja, IdDepartamento = idDepartamento, IdCategoria = idCategoria, dhInventario = request.DataInventario });
        }

        /// <summary>
        /// Reverte o inventário para o status informado.
        /// </summary>
        /// <param name="inventario">O inventário.</param>
        /// <param name="novoStatus">O status para o qual o inventário será revertido.</param>
        public void ReverterParaStatus(Inventario inventario, InventarioStatus novoStatus)
        {
            StoredProcedure.Execute(
                "PR_InventarioVoltarStatus",
                new
                {
                    inventario.IDInventario,
                    stInventarioAntigo = inventario.stInventario,
                    stInventarioNovo = novoStatus,
                    inventario.IDLoja,
                    inventario.dhInventario,
                    cdUsuarioVoltaStatus = RuntimeContext.Current.User.Id
                });
        }

        /// <summary>
        /// Retorna um valor indicando se o inventario especificado possui
        /// itens do tipo vinculado de entrada.
        /// </summary>
        /// <param name="idInventario">O id de inventario.</param>
        /// <returns>
        /// <c>true</c> se o inventário possui itens do tipo
        /// vinculado de entrada;caso contrario <c>false</c>.
        /// </returns>
        public bool PossuiItemVinculadoEntrada(int idInventario)
        {
            return Resource.ExecuteScalar<bool>(
                Sql.Inventario.PossuiItemVinculadoEntrada,
                new { id = idInventario });
        }

        /// <summary>
        /// Ajusta estoque inventariado.
        /// </summary>
        /// <param name="idInventario">O id do inventário.</param>
        /// <param name="aprovacao">Indica se é uma operação de aprovação.</param>
        public void AjustarEstoqueInventariado(int idInventario, bool aprovacao)
        {
            StoredProcedure.Execute(
                "PR_InventarioAjustarEstoque",
                new
                {
                    IdInventario = idInventario,
                    IdUsuario = RuntimeContext.Current.User.Id,
                    Operacao = aprovacao ? "A" : null
                });
        }

        private static InventarioSumario MapearInventarioSumario(
            InventarioSumario inventario,
            Loja loja,
            Departamento departamento,
            Categoria categoria)
        {
            inventario.Loja = loja;
            if (loja != null)
            {
                inventario.IDLoja = loja.IDLoja;
            }

            inventario.Departamento = departamento;
            if (departamento != null)
            {
                inventario.IDDepartamento = departamento.IDDepartamento;
            }

            inventario.Categoria = categoria;
            if (categoria != null)
            {
                inventario.IDCategoria = categoria.IDCategoria;
            }

            return inventario;
        }

        private static InventarioSumario MapearInventarioSumario(
           InventarioSumario inventario,
           Loja loja,
           Bandeira bandeira,
           Departamento departamento,
           Categoria categoria,
           InventarioUsuariosDto usuarios)
        {
            MapearInventarioSumario(inventario, loja, departamento, categoria);
            inventario.Bandeira = bandeira;
            if (bandeira != null)
            {
                inventario.IDBandeira = bandeira.IDBandeira;
            }

            MapearUsuariosInventario(inventario, usuarios);

            return inventario;
        }

        private static void MapearUsuariosInventario(Inventario inventario, InventarioUsuariosDto usuarios)
        {
            if (usuarios == null)
            {
                return;
            }

            inventario.UsuarioAberturaLoja = CriarUsuario(usuarios.UsuarioAberturaLojaId, usuarios.UsuarioAberturaLojaUserName);
            inventario.UsuarioImportacao = CriarUsuario(usuarios.UsuarioImportacaoId, usuarios.UsuarioImportacaoUserName);
            inventario.UsuarioFinalizacaoLoja = CriarUsuario(usuarios.UsuarioFinalizacaoLojaId, usuarios.UsuarioFinalizacaoLojaUserName);
            inventario.UsuarioAprovacaoLoja = CriarUsuario(usuarios.UsuarioAprovacaoLojaId, usuarios.UsuarioAprovacaoLojaUserName);
        }

        private static Usuario CriarUsuario(int? id, string username)
        {
            return !id.HasValue
                ? null
                : new Usuario
                {
                    Id = id.Value,
                    UserName = username
                };
        }
        #endregion
    }
}
