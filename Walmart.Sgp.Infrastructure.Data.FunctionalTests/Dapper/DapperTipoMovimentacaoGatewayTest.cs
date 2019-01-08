using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Walmart.Sgp.Domain.Movimentacao;
using Walmart.Sgp.Infrastructure.Data.Dapper;
using System.Data;

namespace Walmart.Sgp.Infrastructure.Data.FunctionalTests.Dapper
{
    [TestFixture]
    [Category("Dapper")]
    public class DapperTipoMovimentacaoGatewayTest
    {
        [Test]
        [Ignore("Erro ao inserir porque PK é tinyint. Falta definir com o Diego o que será feito.")]
        public void Insert_EnumWithChar_StoragedAsChar()
        {
            this.RunTransaction((appDbs) =>
            {
                new DapperProxy(appDbs.Wlmslp, CommandType.Text).Execute("DBCC CHECKIDENT ('TipoMovimentacao', RESEED, 103)", null);

                var target = new DapperTipoMovimentacaoGateway(appDbs);                                    
                target.Delete("dsTipoMovimentacao = @dsTipoMovimentacao", new { dsTipoMovimentacao = "descrição" });

                var original = new TipoMovimentacao
                {
                    bitApurarCustoMedio = true,
                    dsTipoMovimentacao = "descrição",
                    Ordem = 1,
                    TipoMovimento = TipoMovimento.AjusteInventario
                };
                target.Insert(original);

                var actual = target.FindById(original.Id);
                Assert.AreEqual(TipoMovimento.AjusteInventario, actual.TipoMovimento);

                original.TipoMovimento = TipoMovimento.Entrada;
                target.Update(original);
                actual = target.FindById(original.Id);
                Assert.AreEqual(TipoMovimento.Entrada, actual.TipoMovimento);                

                original.TipoMovimento = TipoMovimento.Saida;
                target.Update(original);
                actual = target.FindById(original.Id);
                Assert.AreEqual(TipoMovimento.Saida, actual.TipoMovimento);                
            });
        }
    }
}
