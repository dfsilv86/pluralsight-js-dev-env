using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Domain.Item
{
    /// <summary>
    /// Representa o retorno da pesquisa de itens relacionados.
    /// </summary>
    public class ItensRelacionadosResponse
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ItensRelacionadosResponse"/>.
        /// </summary>
        /// <param name="entrada">Os itens de entrada.</param>
        /// <param name="derivado">Os itens derivados.</param>
        /// <param name="insumo">Os insumos.</param>
        /// <param name="saida">Os itens de saida.</param>
        /// <param name="transformado">Os itens transformados.</param>
        public ItensRelacionadosResponse(IEnumerable<dynamic> entrada, IEnumerable<dynamic> derivado, IEnumerable<dynamic> insumo, IEnumerable<dynamic> saida, IEnumerable<dynamic> transformado)
        {
            this.Entrada = entrada;
            this.Derivado = derivado;
            this.Insumo = insumo;
            this.Saida = saida;
            this.Transformado = transformado;
        }

        /// <summary>
        /// Obtém os itens relacionados de entrada.
        /// </summary>
        public IEnumerable<dynamic> Entrada { get; private set; }

        /// <summary>
        /// Obtém os itens relacionados derivados.
        /// </summary>
        public IEnumerable<dynamic> Derivado { get; private set; }

        /// <summary>
        /// Obtém os itens relacionados insumo.
        /// </summary>
        public IEnumerable<dynamic> Insumo { get; private set; }

        /// <summary>
        /// Obtém os itens relacionados de saida.
        /// </summary>
        public IEnumerable<dynamic> Saida { get; private set; }

        /// <summary>
        /// Obtém os itens relacionados transformados.
        /// </summary>
        public IEnumerable<dynamic> Transformado { get; private set; }
    }
}
