using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Data.Memory;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.UnitTests.Acessos
{
    [TestFixture]
    [Category("Domain")]
    public class AlcadaServiceTest
    {
        [Test]
        public void ValidarDuplicidadeDetalhe_DetalheDuplicado_Satisfied()
        {
            var gateway = MockRepository.GenerateMock<IAlcadaGateway>();
            var logSv = MockRepository.GenerateMock<IAuditService>();
            var target = new AlcadaService(gateway, logSv);

            var entidade = new Alcada()
            {
                IDAlcada = 1,
                Detalhe = new List<AlcadaDetalhe>()
                {
                    new AlcadaDetalhe()
                    {
                        IDAlcada = 1,
                        IDAlcadaDetalhe = 2,
                        Bandeira = new Domain.EstruturaMercadologica.Bandeira()
                        {
                            IDBandeira = 1
                        },
                        Departamento = new Domain.EstruturaMercadologica.Departamento()
                        {
                            IDDepartamento = 2
                        },
                        RegiaoAdministrativa = new Domain.EstruturaMercadologica.RegiaoAdministrativa()
                        {
                            IdRegiaoAdministrativa = 1
                        }
                    }
                }
            };

            gateway.Expect(g => g.ObterEstruturado(1, null)).Return(new Alcada()
            {
                Detalhe = new List<AlcadaDetalhe>()
                {
                    new AlcadaDetalhe()
                    {
                        IDAlcada = 1,
                        IDAlcadaDetalhe = 2,
                        Bandeira = new Domain.EstruturaMercadologica.Bandeira()
                        {
                            IDBandeira = 2
                        },
                        Departamento = new Domain.EstruturaMercadologica.Departamento()
                        {
                            IDDepartamento = 2
                        },
                        RegiaoAdministrativa = new Domain.EstruturaMercadologica.RegiaoAdministrativa()
                        {
                            IdRegiaoAdministrativa = 2
                        }
                    }
                }
            });

            var result = target.ValidarDuplicidadeDetalhe(entidade);

            Assert.IsTrue(result.Satisfied);
        }

        [Test]
        public void ValidarDuplicidadeDetalhe_DetalheDuplicado_NotSatisfied()
        {
            var gateway = MockRepository.GenerateMock<IAlcadaGateway>();
            var logSv = MockRepository.GenerateMock<IAuditService>();
            var target = new AlcadaService(gateway, logSv);

            var entidade = new Alcada()
            {
                IDAlcada = 1,
                Detalhe = new List<AlcadaDetalhe>()
                {
                    new AlcadaDetalhe()
                    {
                        IDAlcada = 1,
                        IDAlcadaDetalhe = 1,
                        Bandeira = new Domain.EstruturaMercadologica.Bandeira()
                        {
                            IDBandeira = 1
                        },
                        Departamento = new Domain.EstruturaMercadologica.Departamento()
                        {
                            IDDepartamento = 1
                        },
                        RegiaoAdministrativa = new Domain.EstruturaMercadologica.RegiaoAdministrativa()
                        {
                            IdRegiaoAdministrativa = 1
                        }
                    },
                    new AlcadaDetalhe()
                    {
                        IDAlcada = 1,
                        IDAlcadaDetalhe = 2,
                        Bandeira = new Domain.EstruturaMercadologica.Bandeira()
                        {
                            IDBandeira = 1
                        },
                        Departamento = new Domain.EstruturaMercadologica.Departamento()
                        {
                            IDDepartamento = 1
                        },
                        RegiaoAdministrativa = new Domain.EstruturaMercadologica.RegiaoAdministrativa()
                        {
                            IdRegiaoAdministrativa = 1
                        }
                    }
                }
            };

            gateway.Expect(g => g.ObterEstruturado(1, null)).Return(entidade);

            var result = target.ValidarDuplicidadeDetalhe(entidade);

            Assert.IsFalse(result.Satisfied);
        }

        [Test]
        public void ObterPorPerfil_ValidPerfil_Alcada()
        {
            var gateway = new MemoryAlcadaGateway();
            var logSv = MockRepository.GenerateMock<IAuditService>();
            var target = new AlcadaService(gateway, logSv);

            var result = target.ObterPorPerfil(2);

            Assert.AreEqual(0, result.IDAlcada);
            Assert.AreEqual(2, result.IDPerfil);
        }

        [Test]
        public void ObterPorPerfil_InvalidPerfil_Exception()
        {
            var gateway = new MemoryAlcadaGateway();
            var logSv = MockRepository.GenerateMock<IAuditService>();
            var target = new AlcadaService(gateway, logSv);

            Assert.Throws<NotSatisfiedSpecException>(() => { target.ObterPorPerfil(0); });
        }

        [Test]
        public void ObterEstruturadoPorPerfil_AlcadaExistente_Alcada()
        {
            var gateway = MockRepository.GenerateMock<IAlcadaGateway>();
            var logSv = MockRepository.GenerateMock<IAuditService>();

            gateway.Expect(g => g.ObterEstruturadoPorPerfil(8)).Return(new
            Alcada
            {
                IDAlcada = 3,
                IDPerfil = 8,
                Papel = new Papel
                {
                    Id = 8,
                    Name = "Papel 8"
                }
            });

            var target = new AlcadaService(gateway, logSv);

            var result = target.ObterEstruturadoPorPerfil(8);

            Assert.AreEqual(3, result.IDAlcada);
            Assert.IsNotNull(result.Papel);
            Assert.AreEqual(8, result.Papel.Id);
        }

        [Test]
        public void ObterEstruturadoPorPerfil_AlcadaInexistente_NovaAlcada()
        {
            var gateway = MockRepository.GenerateMock<IAlcadaGateway>();
            var logSv = MockRepository.GenerateMock<IAuditService>();

            gateway.Expect(g => g.ObterEstruturadoPorPerfil(10)).Return(new
            Alcada
            {
                IDAlcada = 0,
                IDPerfil = 10,
                Papel = new Papel
                {
                    Id = 10,
                    Name = "Papel 10"
                }
            });

            var target = new AlcadaService(gateway, logSv);

            var result = target.ObterEstruturadoPorPerfil(10);

            Assert.IsTrue(result.IsNew);
            Assert.IsNotNull(result.Papel);
            Assert.AreEqual(10, result.Papel.Id);
        }

        [Test]
        public void Salvar_AlcadaInvalida_Exception()
        {
            var alcada = new Alcada
            {
                vlPercentualAlterado = -1,
                blAlterarPercentual = true
            };

            var target = new AlcadaService(null, null);
            Assert.Throws<NotSatisfiedSpecException>(() =>
            {
                target.Salvar(alcada);
            });
        }

        [Test]
        public void Salvar_NovaAlcada_Insere()
        {
            var gateway = MockRepository.GenerateMock<IAlcadaGateway>();

            gateway.Expect(g => g.Insert(null, null)).IgnoreArguments();
            gateway.Stub(g => g.Insert(null, null)).IgnoreArguments().Throw(new InvalidOperationException());
            gateway.Stub(g => g.Insert((Alcada)null)).IgnoreArguments().Throw(new InvalidOperationException());
            gateway.Stub(g => g.Update(null)).IgnoreArguments().Throw(new InvalidOperationException());
            gateway.Stub(g => g.Update(null, null)).IgnoreArguments().Throw(new InvalidOperationException());

            var logSv = MockRepository.GenerateMock<IAuditService>();

            gateway.Stub(g => g.ObterEstruturadoPorPerfil(10)).Return(new
            Alcada
            {
                IDAlcada = 0,
                IDPerfil = 10,
                Papel = new Papel
                {
                    Id = 10,
                    Name = "Papel 10"
                }
            });

            var target = new AlcadaService(gateway, logSv);

            var alcada = target.ObterEstruturadoPorPerfil(10);
            alcada.blAlterarPercentual = true;
            alcada.blAlterarSugestao = true;
            alcada.vlPercentualAlterado = 20;

            target.Salvar(alcada);

            gateway.VerifyAllExpectations();
        }

        [Test]
        public void Salvar_AlcadaExistente_Atualiza()
        {
            var gateway = MockRepository.GenerateMock<IAlcadaGateway>();

            gateway.Expect(g => g.Update(null, null)).IgnoreArguments();
            gateway.Stub(g => g.Update(null, null)).IgnoreArguments().Throw(new InvalidOperationException());
            gateway.Stub(g => g.Update(null)).IgnoreArguments().Throw(new InvalidOperationException());
            gateway.Stub(g => g.Insert((Alcada)null)).IgnoreArguments().Throw(new InvalidOperationException());
            gateway.Stub(g => g.Insert(null, null)).IgnoreArguments().Throw(new InvalidOperationException());

            var logSv = MockRepository.GenerateMock<IAuditService>();

            gateway.Stub(g => g.ObterEstruturadoPorPerfil(10)).Return(new
            Alcada
            {
                IDAlcada = 1,
                IDPerfil = 10,
                Papel = new Papel
                {
                    Id = 10,
                    Name = "Papel 10"
                }
            });

            var target = new AlcadaService(gateway, logSv);

            var alcada = target.ObterEstruturadoPorPerfil(10);
            alcada.blAlterarPercentual = true;
            alcada.blAlterarSugestao = true;
            alcada.vlPercentualAlterado = 20;

            target.Salvar(alcada);

            gateway.VerifyAllExpectations();
        }

        [Test]
        public void Remover_Alcada_Removido()
        {
            var alcadaDetalhe = new AlcadaDetalhe
            {
                Id = 1,
                vlPercentualAlterado = 1
            };

            var original = new Alcada
            {
                Detalhe = new AlcadaDetalhe[] { alcadaDetalhe },
                IDAlcada = 1,
                IDPerfil = 10,
                Papel = new Papel
                {
                    Id = 10,
                    Name = "Papel 10"
                }
            };

            var gateway = MockRepository.GenerateMock<IAlcadaGateway>();
            var logSv = MockRepository.GenerateMock<IAuditService>();

            gateway.Expect(g => g.ObterEstruturado(10, null)).Return(original);

            gateway.Expect(g => g.Delete(10, null)).IgnoreArguments();
            gateway.Stub(g => g.Delete(10)).IgnoreArguments().Throw(new InvalidOperationException());
            gateway.Stub(g => g.Delete(10, null)).IgnoreArguments().Throw(new InvalidOperationException());

            var target = new AlcadaService(gateway, logSv);

            target.Remover(10);

            gateway.VerifyAllExpectations();
            logSv.VerifyAllExpectations();
        }
    }
}
