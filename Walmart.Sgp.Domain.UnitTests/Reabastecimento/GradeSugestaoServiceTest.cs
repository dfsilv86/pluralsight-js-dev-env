using NUnit.Framework;
using Rhino.Mocks;
using System.Collections;
using System.Collections.Generic;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Infrastructure.Data.Memory;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;
using System.Linq;

namespace Walmart.Sgp.Domain.UnitTests.Reabastecimento
{
    [TestFixture]
    [Category("Domain")]
    public class GradeSugestaoServiceTest
    {
        [Test]
        public void Atualizar_Existente_Atualizado()
        {
            var gateway = new MemoryGradeSugestaoGateway();
            
            var inserted = new GradeSugestao
            {
                cdSistema = 1,
                IDBandeira = 2,
                IDDepartamento = 3,
                IDLoja = 4,
                vlHoraInicial = 1130,
                vlHoraFinal = 1600
            };

            gateway.Insert(inserted);

            var target = new GradeSugestaoService(gateway);

            target.Atualizar(new GradeSugestao
            {
                IDGradeSugestao = inserted.IDGradeSugestao,
                cdSistema = 1,
                IDBandeira = 2,
                IDDepartamento = 3,
                IDLoja = 4,
                vlHoraInicial = 1100,
                vlHoraFinal = 1700
            });

            Assert.AreEqual(1, gateway.Entities.Count);
            var entity = gateway.Entities[0];
            Assert.AreEqual(1100, entity.vlHoraInicial);
            Assert.AreEqual(1700, entity.vlHoraFinal);
            Assert.IsTrue(entity.DhAtualizacao.HasValue);
            Assert.IsTrue(entity.CdUsuarioAtualizacao.HasValue);
        }

        [Test]
        public void Atualizar_SemCamposObrigatorios_Exception()
        {
            var gateway = new MemoryGradeSugestaoGateway();

            var inserted = new GradeSugestao
            {
                cdSistema = 1,
                IDBandeira = 2,
                IDDepartamento = 3,
                IDLoja = 4,
                vlHoraInicial = 1130,
                vlHoraFinal = 1600
            };

            gateway.Insert(inserted);

            var target = new GradeSugestaoService(gateway);

            Assert.Throws<NotSatisfiedSpecException>(() =>
            {
                target.Atualizar(new GradeSugestao
                {
                    IDGradeSugestao = inserted.IDGradeSugestao,
                    cdSistema = 1,
                    IDBandeira = 0,
                    IDDepartamento = 3,
                    IDLoja = 4,
                    vlHoraInicial = 0,
                    vlHoraFinal = 0
                });
            });            
        }

        [Test]
        public void Atualizar_HoraInicialInvalida_Exception()
        {
            var gateway = new MemoryGradeSugestaoGateway();

            var inserted = new GradeSugestao
            {
                cdSistema = 1,
                IDBandeira = 2,
                IDDepartamento = 3,
                IDLoja = 4,
                vlHoraInicial = 1130,
                vlHoraFinal = 1600
            };

            gateway.Insert(inserted);

            var target = new GradeSugestaoService(gateway);

            Assert.Throws<NotSatisfiedSpecException>(() =>
            {
                target.Atualizar(new GradeSugestao
                {
                    IDGradeSugestao = inserted.IDGradeSugestao,
                    cdSistema = 1,
                    IDBandeira = 2,
                    IDDepartamento = 3,
                    IDLoja = 4,
                    vlHoraInicial = 1700,
                    vlHoraFinal = 1045
                });
            });     
        }

        [Test]
        public void Atualizar_RegistroDuplicado_Exception()
        {
            var gateway = new MemoryGradeSugestaoGateway();
            gateway.Insert(new GradeSugestao
            {
                cdSistema = 1,
                IDBandeira = 2,
                IDDepartamento = 3,
                IDLoja = 4,
                vlHoraInicial = 1130,
                vlHoraFinal = 1600
            });

            var idPrimeiraEntidade = gateway.Entities[0].Id;

            gateway.Insert(new GradeSugestao
            {
                cdSistema = 1,
                IDBandeira = 3,
                IDDepartamento = 3,
                IDLoja = 4,
                vlHoraInicial = 1130,
                vlHoraFinal = 1600
            });

            var target = new GradeSugestaoService(gateway);

            target.Atualizar(new GradeSugestao
            {
                IDGradeSugestao = idPrimeiraEntidade,
                cdSistema = 1,
                IDBandeira = 4,
                IDDepartamento = 3,
                IDLoja = 4,
                vlHoraInicial = 800,
                vlHoraFinal = 1800
            });

            var actual = gateway.FindById(idPrimeiraEntidade);
            Assert.AreEqual(2, actual.IDBandeira);
            Assert.AreEqual(800, actual.vlHoraInicial);
            Assert.AreEqual(1800, actual.vlHoraFinal);

            Assert.Throws<NotSatisfiedSpecException>(() =>
            {
                target.Atualizar(new GradeSugestao
                {
                    IDGradeSugestao = idPrimeiraEntidade,
                    cdSistema = 1,
                    IDBandeira = 3,
                    IDDepartamento = 3,
                    IDLoja = 4,
                    vlHoraInicial = 800,
                    vlHoraFinal = 1800
                });
            }, Texts.SuggestionGridAlreadyExists); 
        }

        [Test]
        public void SalvarNovas_SugestoesUnicas_Salvo()
        {
            var gateway = new MemoryGradeSugestaoGateway();
            var target = new GradeSugestaoService(gateway);

            Assert.AreEqual(0, gateway.Entities.Count);

            var sugestoes = new[] 
            {
                new GradeSugestao 
                {
                    cdSistema = 2,
                    IDBandeira = 1,
                    IDDepartamento = 2,
                    IDLoja = 3,
                    vlHoraInicial = 800,
                    vlHoraFinal = 900
                },
                new GradeSugestao 
                {
                    cdSistema = 2,
                    IDBandeira = 1,
                    IDDepartamento = 2,
                    IDLoja = 4,
                    vlHoraInicial = 800,
                    vlHoraFinal = 900
                },
                new GradeSugestao 
                {
                    cdSistema = 2,
                    IDBandeira = 1,
                    IDDepartamento = 2,
                    IDLoja = 5,
                    vlHoraInicial = 800,
                    vlHoraFinal = 900
                }
            };

            target.SalvarNovas(sugestoes);

            Assert.AreEqual(3, gateway.Entities.Count);
            Assert.AreEqual(3, gateway.Entities.Count(e => e.CdUsuarioCriacao.HasValue && e.DhCriacao.HasValue));
        }

        [Test]
        public void SalvarNovas_SugestoesDuplicadasEntreSi_Exception()
        {
            var gateway = new MemoryGradeSugestaoGateway();
            var target = new GradeSugestaoService(gateway);

            Assert.AreEqual(0, gateway.Entities.Count);

            var sugestoes = new[] 
            {
                new GradeSugestao 
                {
                    cdSistema = 2,
                    IDBandeira = 1,
                    IDDepartamento = 2,
                    IDLoja = 3,
                    vlHoraInicial = 800,
                    vlHoraFinal = 900
                },
                new GradeSugestao
                {
                    cdSistema = 2,
                    IDBandeira = 1,
                    IDDepartamento = 2,
                    IDLoja = 4,
                    vlHoraInicial = 800,
                    vlHoraFinal = 1200
                },
                new GradeSugestao // Duplicada
                {
                    cdSistema = 2,
                    IDBandeira = 1,
                    IDDepartamento = 2,
                    IDLoja = 4,
                    vlHoraInicial = 800,
                    vlHoraFinal = 900
                }
            };

            Assert.Throws<NotSatisfiedSpecException>(() =>
            {
                target.SalvarNovas(sugestoes);
            });

            Assert.AreEqual(0, gateway.Entities.Count);
        }

        [Test]
        public void SalvarNovas_SugestoesDuplicadasNoGateway_Exception()
        {
            var gateway = new MemoryGradeSugestaoGateway();
            var target = new GradeSugestaoService(gateway);

            gateway.Insert(new GradeSugestao
            {
                cdSistema = 2,
                IDBandeira = 1,
                IDDepartamento = 2,
                IDLoja = 4,
                vlHoraInicial = 800,
                vlHoraFinal = 900
            });

            var sugestoes = new[] 
            {
                new GradeSugestao 
                {
                    cdSistema = 2,
                    IDBandeira = 1,
                    IDDepartamento = 2,
                    IDLoja = 3,
                    vlHoraInicial = 800,
                    vlHoraFinal = 900
                },
                new GradeSugestao // Ja existe no gateway
                {
                    cdSistema = 2,
                    IDBandeira = 1,
                    IDDepartamento = 2,
                    IDLoja = 4,
                    vlHoraInicial = 800,
                    vlHoraFinal = 1200
                },
                new GradeSugestao 
                {
                    cdSistema = 2,
                    IDBandeira = 1,
                    IDDepartamento = 2,
                    IDLoja = 5,
                    vlHoraInicial = 800,
                    vlHoraFinal = 900
                }
            };

            Assert.Throws<NotSatisfiedSpecException>(() =>
            {
                target.SalvarNovas(sugestoes);
            });

            Assert.AreEqual(1, gateway.Entities.Count);
        }

        [Test]
        public void Remover_IdentificadorValido_Removido()
        {
            var gateway = new MemoryGradeSugestaoGateway();

            var inserted = new GradeSugestao
            {
                cdSistema = 1,
                IDBandeira = 2,
                IDDepartamento = 3,
                IDLoja = 4,
                vlHoraInicial = 1130,
                vlHoraFinal = 1600
            };

            gateway.Insert(inserted);
            Assert.AreEqual(1, gateway.Entities.Count);

            var target = new GradeSugestaoService(gateway);
            target.Remover(inserted.Id);

            Assert.AreEqual(0, gateway.Entities.Count);
        }

        [Test]
        public void ContarExistentes_Configuracao_Contagem()
        {
            var gateway = new MemoryGradeSugestaoGateway();
            var target = new GradeSugestaoService(gateway);

            gateway.Insert(new GradeSugestao
            {
                cdSistema = 2,
                IDBandeira = 1,
                IDDepartamento = 2,
                IDLoja = 4,
                vlHoraInicial = 800,
                vlHoraFinal = 900
            });

            gateway.Insert(new GradeSugestao
            {
                cdSistema = 2,
                IDBandeira = 1,
                IDDepartamento = 2,
                IDLoja = 5,
                vlHoraInicial = 800,
                vlHoraFinal = 900
            });

            var actual = target.ContarExistentes(2, 1, 4, 2);
            Assert.AreEqual(1, actual);
        }
    }
}