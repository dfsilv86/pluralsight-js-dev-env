using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Inventarios
{
    /// <summary>
    /// O conteúdo de um arquivo de inventário importado.
    /// </summary>
    public class ArquivoInventario : IDetalhesArquivo
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ArquivoInventario"/>.
        /// </summary>
        /// <param name="idLojaImportacao">O id de loja a qual o arquivo pertence.</param>
        /// <param name="nomeArquivo">O nome do arquivo.</param>
        /// <param name="dataInventario">A data de inventario.</param>
        public ArquivoInventario(int idLojaImportacao, string nomeArquivo, DateTime dataInventario)
        {
            this.TipoArquivo = TipoArquivoInventario.Nenhum;
            this.IdLojaImportacao = idLojaImportacao;
            this.NomeArquivo = nomeArquivo;
            this.DataInventario = dataInventario;
            this.Itens = new List<ArquivoInventarioItem>();
            this.IsArquivoValido = false;
        }

        /// <summary>
        /// Obtém o id da loja a qual o arquivo pertence.
        /// </summary>
        public int IdLojaImportacao { get; private set; }

        /// <summary>
        /// Obtém o nome do arquivo.
        /// </summary>
        public string NomeArquivo { get; private set; }

        /// <summary>
        /// Obtém a data do inventário.
        /// </summary>
        public DateTime DataInventario { get; private set; }

        /// <summary>
        /// Obtém ou define a data do inventário informada dentro do arquivo.
        /// </summary>
        public DateTime? DataArquivo { get; set; }

        /// <summary>
        /// Obtém ou define o código da loja.
        /// </summary>
        public int? CdLoja { get; set; }

        /// <summary>
        /// Obtém ou define os itens lidos do arquivo.
        /// </summary>
        public IList<ArquivoInventarioItem> Itens { get; set; }

        /// <summary>
        /// Obtém ou define o último código de departamento encontrado no arquivo, se algum.
        /// </summary>
        public int? UltimoCdDepartamentoLido { get; set; }

        /// <summary>
        /// Obtém ou define um valor que indica se o arquivo é multi-departamento.
        /// </summary>
        public bool? IsMultiDepartamento { get; set; }

        /// <summary>
        /// Obtém ou define o número de itens contados, informado no arquivo.
        /// </summary>
        public int? TotalItensContados { get; set; }

        /// <summary>
        /// Obtém ou define o total de estoque informado no arquivo.
        /// </summary>
        public decimal? TotalEstoqueFinal { get; set; }

        /// <summary>
        /// Obtém ou define o tipo do arquivo.
        /// </summary>
        public TipoArquivoInventario TipoArquivo { get; set; }

        /// <summary>
        /// Obtém ou define um valor que indica se o arquivo é valido e foi lido com sucesso.
        /// </summary>
        public bool IsArquivoValido { get; set; }

        /// <summary>
        /// Obtém ou define um valor que indica se o cabeçalho do arquivo foi lido com sucesso.
        /// </summary>
        public bool LeuCabecalho { get; set; }
    }
}
