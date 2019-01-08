using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Inventarios;

namespace Walmart.Sgp.Infrastructure.IO.FunctionalTests.Importing.Inventario
{
    public class ParametroMock : IParametroService
    {
        private int? m_qtdDiasVarejo;
        private int? m_qtdDiasAtacado;

        public ParametroMock(int? qtdDiasAtacado, int? qtdDiasVarejo)
        {
            m_qtdDiasAtacado = qtdDiasAtacado;
            m_qtdDiasVarejo = qtdDiasVarejo;
        }

        public Parametro Obter()
        {
            return new Parametro
            {
                qtdDiasArquivoInventarioAtacado = m_qtdDiasAtacado,
                qtdDiasArquivoInventarioVarejo = m_qtdDiasVarejo
            };
        }

        public Parametro ObterEstruturado()
        {
            return new Parametro
            {
                UsuarioAdministrador = new Usuario
                {
                    UserName = "usuario1",
                    FullName = "Usuário 1"
                },
                qtdDiasArquivoInventarioAtacado = m_qtdDiasAtacado,
                qtdDiasArquivoInventarioVarejo = m_qtdDiasVarejo
            };
        }

        public void Salvar(Parametro parametro)
        {
            throw new NotImplementedException();
        }
    }
}
