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
    /// Representa uma InventarioAgendamento.
    /// </summary>
    public class InventarioAgendamento : EntityBase
    {
        #region Properties
        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public override int Id
        {
            get
            {
                return IDInventarioAgendamento;
            }
            
            set
            {
                IDInventarioAgendamento = value;
            }
        }

        /// <summary>
        /// Obtém ou define IDInventarioAgendamento.
        /// </summary>
        public int IDInventarioAgendamento { get; set; }

        /// <summary>
        /// Obtém ou define IDInventario.
        /// </summary>
        public int IDInventario { get; set; }

        /// <summary>
        /// Obtém ou define o inventário.
        /// </summary>
        public Inventario Inventario { get; set; }

        /// <summary>
        /// Obtém ou define IDFormato.
        /// </summary>
        public int? IDFormato { get; set; }

        /// <summary>
        /// Obtém ou define dtAgendamento.
        /// </summary>
        public DateTime dtAgendamento { get; set; }

        /// <summary>
        /// Obtém ou define stAgendamento.
        /// </summary>
        public InventarioAgendamentoStatus stAgendamento { get; set; }

        /// <summary>
        /// Obtém ou define dhCriacao.
        /// </summary>
        public DateTime dhCriacao { get; set; }

        /// <summary>
        /// Obtém ou define IDUsuarioCriacao.
        /// </summary>
        public int IDUsuarioCriacao { get; set; }

        /// <summary>
        /// Obtém ou define dhAlteracao.
        /// </summary>
        public DateTime? dhAlteracao { get; set; }

        /// <summary>
        /// Obtém ou define a bandeira.
        /// </summary>
        public Bandeira Bandeira { get; set; }

        /// <summary>
        /// Obtém ou define a loja.
        /// </summary>
        public Loja Loja { get; set; }

        /// <summary>
        /// Obtém ou define o departamento.
        /// </summary>
        public Departamento Departamento { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Cria os agendamentos na data para a combinação de lojas e departamentos informados.
        /// </summary>
        /// <param name="dtAgendamento">A data de agendamento.</param>
        /// <param name="lojas">As lojas que serão utilizadas no agendamento.</param>
        /// <param name="departamentos">Os departamentos que serão utilizados no agendamento.</param>
        /// <returns>Os agendamentods.</returns>
        public static IEnumerable<InventarioAgendamento> Create(DateTime dtAgendamento, IEnumerable<Loja> lojas, IEnumerable<Departamento> departamentos)
        {
            var agendamentos = new List<InventarioAgendamento>();

            foreach (var loja in lojas)
            {
                foreach (var departamento in departamentos)
                {
                    var agendamento = new InventarioAgendamento
                    {
                        Inventario = new Inventario
                        {
                            IDLoja = loja.Id,
                            Loja = loja,
                            IDDepartamento = departamento.Id,
                            Departamento = departamento,
                            dhInventario = dtAgendamento
                        },
                        dtAgendamento = dtAgendamento,
                    };

                    agendamentos.Add(agendamento);
                }
            }            

            return agendamentos;
        }

        /// <summary>
        /// Marca como agendado.
        /// </summary>
        /// <param name="inventario">O inventário.</param>
        /// <param name="idUsuario">O id do usuário.</param>
        /// <param name="data">A data de criação.</param>
        public void MarcarComoAgendado(Inventario inventario, int idUsuario, DateTime data)
        {
            IDInventario = inventario.IDInventario;
            stAgendamento = InventarioAgendamentoStatus.Agendado;
            dhCriacao = data;
            IDUsuarioCriacao = idUsuario;
        }
        #endregion
    }
}
