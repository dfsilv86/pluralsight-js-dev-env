using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Domain.Inventarios
{
    /// <summary>
    /// Define a interface de um logger de leitor de arquivos.
    /// </summary>
    public interface ILeitorLogger
    {
        /// <summary>
        /// Insere log informativo de processamento.
        /// </summary>
        /// <param name="nomeAcao">O nome da ação.</param>
        /// <param name="mensagem">A mensagem informativa.</param>
        void InserirLogProcessamento(string nomeAcao, string mensagem);

        /// <summary>
        /// Insere log de erro de processamento.
        /// </summary>
        /// <param name="nomeAcao">O nome da ação.</param>
        /// <param name="mensagemErro">A mensagem de erro.</param>
        void InserirLogErroProcessamento(string nomeAcao, string mensagemErro);

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
        void InserirInventarioCritica(int idLoja, string mensagemCritica, short idInventarioCriticaTipo, int? idInventario, int? idDepartamento, long? idCategoria, DateTime? dataInventario);

        /// <summary>
        /// Exclui log de críticas anteriores.
        /// </summary>
        /// <param name="idLoja">O id de loja.</param>
        /// <param name="dataInventario">A data de inventario.</param>
        /// <param name="idDepartamento">O id de departamento.</param>
        /// <param name="idCategoria">O id de categoria.</param>
        /// <remarks>Exclusão lógica.</remarks>
        void ExcluirInventarioCritica(int idLoja, DateTime dataInventario, int? idDepartamento, int? idCategoria);

        /// <summary>
        /// Critica inventário conforme existência de custos apurados.
        /// </summary>
        /// <param name="idInventario">O id do inventário.</param>
        /// <returns>A quantidade de críticas.</returns>
        int ApurarCriticaInventarioSemCusto(int idInventario);
    }
}
