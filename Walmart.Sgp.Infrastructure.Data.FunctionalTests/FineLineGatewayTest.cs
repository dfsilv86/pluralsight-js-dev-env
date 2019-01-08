using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Data.Dapper;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.FunctionalTests.Dapper
{
    [TestFixture]
    [Category("Ado")]
    [Category("Dapper")]
    public class FineLineGatewayTest : DataGatewayTestBase<IFineLineGateway>
    {
        [Test]
        public void PesquisarPorFineLineSubcategoriaCategoriaDepartamentoESistema_Args_SubcategoriaCategoriaDepartamentoDivisaoCarregados()
        {
            this.RunForEachGateway((appDbs, target) =>
            {
                var actual = target.PesquisarPorFineLineSubcategoriaCategoriaDepartamentoESistema(null, null, null, null, 1, 1, new Paging()).ToList();
                Assert.AreNotEqual(0, actual.Count);

                foreach(var c in actual)
                {
                    Assert.IsNotNull(c.Subcategoria);
                    Assert.AreNotEqual(0, c.Subcategoria.Id);

                    Assert.IsNotNull(c.Subcategoria.Categoria);
                    Assert.AreNotEqual(0, c.Subcategoria.Categoria.Id);

                    Assert.IsNotNull(c.Subcategoria.Categoria.Departamento);
                    Assert.AreNotEqual(0, c.Subcategoria.Categoria.Departamento.Id);

                    Assert.IsNotNull(c.Subcategoria.Categoria.Departamento.Divisao);
                    Assert.AreNotEqual(0, c.Subcategoria.Categoria.Departamento.Divisao.Id);
                }
            });
        }

        [Test]
        public void PesquisarPorFineLineSubcategoriaCategoriaDepartamentoESistema_Paging_PagingResults()
        {
            this.RunForEachGateway((appDbs, target) =>
            {
                var actual = target.PesquisarPorFineLineSubcategoriaCategoriaDepartamentoESistema(null, null, null, null, 1, 1, new Paging(0, 100, "IDFineLine")).ToList();
                Assert.AreNotEqual(0, actual.Count);

                foreach (var c in actual)
                {
                    Assert.IsNotNull(c.Subcategoria);
                    Assert.AreNotEqual(0, c.Subcategoria.Id);

                    Assert.IsNotNull(c.Subcategoria.Categoria);
                    Assert.AreNotEqual(0, c.Subcategoria.Categoria.Id);

                    Assert.IsNotNull(c.Subcategoria.Categoria.Departamento);
                    Assert.AreNotEqual(0, c.Subcategoria.Categoria.Departamento.Id);

                    Assert.IsNotNull(c.Subcategoria.Categoria.Departamento.Divisao);
                    Assert.AreNotEqual(0, c.Subcategoria.Categoria.Departamento.Divisao.Id);
                }
            });
        }
    }
}
