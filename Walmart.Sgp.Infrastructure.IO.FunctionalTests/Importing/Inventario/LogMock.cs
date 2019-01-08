using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Inventarios;

namespace Walmart.Sgp.Infrastructure.IO.FunctionalTests.Importing.Inventario
{
    public class LogMock : ILeitorLogger
    {
        public int Logs { get; set; }

        public int Criticas { get; set; }

        public int Invalidos { get; set; }

        public void InserirLogProcessamento(string nomeAcao, string mensagem)
        {
            Logs++;

            if (nomeAcao.Contains("Inv"))
            {
                Invalidos++;
            }
        }

        public void InserirInventarioCritica(int idLoja, string mensagemCritica, short idInventarioCriticaTipo, int? idInventario, int? idDepartamento, long? idCategoria, DateTime? dataInventario)
        {
            Criticas++;
        }

        public void InserirLogErroProcessamento(string nomeAcao, string mensagemErro)
        {
            throw new NotImplementedException();
        }

        public void ExcluirInventarioCritica(int idLoja, DateTime dataInventario, int? idDepartamento, int? idCategoria)
        {
            throw new NotImplementedException();
        }

        public int ApurarCriticaInventarioSemCusto(int IDInventario)
        {
            throw new NotImplementedException();
        }
    }
}
