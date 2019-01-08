using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.UnitTests.Acessos
{
    [TestFixture]
    [Category("Domain")]
    public class AlcadaAuditStrategyTest
    {
        #region Fields

        private static readonly string[] AlcadaAuditProperties = new string[] 
        { 
            "IDAlcada", 
            "IDPerfil",
            "blAlterarSugestao", 
            "blAlterarInformacaoEstoque", 
            "blAlterarPercentual", 
            "vlPercentualAlterado", 
            "blZerarItem"
        };

        private static readonly string[] AlcadaDetalheAuditProperties = new string[] 
        { 
            "vlPercentualAlterado", 
        };

        #endregion

        [Test]
        public void DidInsert_Alcada_LogInsert()
        {
            IAuditService auditService = MockRepository.GenerateMock<IAuditService>();

            auditService.Expect(s => s.LogInsert<Alcada>(null, null)).IgnoreArguments();
            auditService.Stub(s => s.LogInsert<Alcada>(null, null)).IgnoreArguments().Throw(new InvalidOperationException());
            auditService.Stub(s => s.LogUpdate<Alcada>(null, null)).IgnoreArguments().Throw(new InvalidOperationException());
            auditService.Stub(s => s.LogDelete<Alcada>(null, null)).IgnoreArguments().Throw(new InvalidOperationException());
            auditService.Expect(s => s.LogInsert<AlcadaDetalhe>(null, null)).IgnoreArguments();
            auditService.Stub(s => s.LogInsert<AlcadaDetalhe>(null, null)).IgnoreArguments().Throw(new InvalidOperationException());
            auditService.Stub(s => s.LogUpdate<AlcadaDetalhe>(null, null)).IgnoreArguments().Throw(new InvalidOperationException());
            auditService.Stub(s => s.LogDelete<AlcadaDetalhe>(null, null)).IgnoreArguments().Throw(new InvalidOperationException());

            var target = new AlcadaAuditStrategy(null, auditService, AlcadaAuditProperties, AlcadaDetalheAuditProperties);

            target.DidInsert(new Alcada(), new AlcadaDetalhe());

            auditService.VerifyAllExpectations();
        }

        [Test]
        public void DidUpdate_Alcada_LogUpdate()
        {
            Alcada original = new Alcada() { Detalhe = new AlcadaDetalhe[] { new AlcadaDetalhe { Id = 1, vlPercentualAlterado = 2 }, new AlcadaDetalhe { Id = 2, vlPercentualAlterado = 3 }, new AlcadaDetalhe { Id = 3, vlPercentualAlterado = 4 } } };

            IAuditService auditService = MockRepository.GenerateMock<IAuditService>();

            auditService.Stub(s => s.LogInsert<Alcada>(null, null)).IgnoreArguments().Throw(new InvalidOperationException());
            auditService.Expect(s => s.LogUpdate<Alcada>(null, null)).IgnoreArguments();
            auditService.Stub(s => s.LogUpdate<Alcada>(null, null)).IgnoreArguments().Throw(new InvalidOperationException());
            auditService.Stub(s => s.LogDelete<Alcada>(null, null)).IgnoreArguments().Throw(new InvalidOperationException());
            auditService.Stub(s => s.LogInsert<AlcadaDetalhe>(null, null)).IgnoreArguments().Throw(new InvalidOperationException());
            auditService.Expect(s => s.LogUpdate<AlcadaDetalhe>(null, null)).IgnoreArguments();
            auditService.Stub(s => s.LogUpdate<AlcadaDetalhe>(null, null)).IgnoreArguments().Throw(new InvalidOperationException());
            auditService.Stub(s => s.LogDelete<AlcadaDetalhe>(null, null)).IgnoreArguments().Throw(new InvalidOperationException());

            var target = new AlcadaAuditStrategy(original, auditService, AlcadaAuditProperties, AlcadaDetalheAuditProperties);

            target.DidUpdate(new Alcada(), new AlcadaDetalhe() { Id = 1, vlPercentualAlterado = 2 }, new AlcadaDetalhe() { Id = 2, vlPercentualAlterado = 10 });

            auditService.VerifyAllExpectations();
        }

        [Test]
        public void WillDelete_Alcada_LogDelete()
        {
            IAuditService auditService = MockRepository.GenerateMock<IAuditService>();

            auditService.Stub(s => s.LogInsert<Alcada>(null, null)).IgnoreArguments().Throw(new InvalidOperationException());
            auditService.Stub(s => s.LogUpdate<Alcada>(null, null)).IgnoreArguments().Throw(new InvalidOperationException());
            auditService.Expect(s => s.LogDelete<Alcada>(null, null)).IgnoreArguments();
            auditService.Stub(s => s.LogDelete<Alcada>(null, null)).IgnoreArguments().Throw(new InvalidOperationException());
            auditService.Stub(s => s.LogInsert<AlcadaDetalhe>(null, null)).IgnoreArguments().Throw(new InvalidOperationException());
            auditService.Stub(s => s.LogUpdate<AlcadaDetalhe>(null, null)).IgnoreArguments().Throw(new InvalidOperationException());
            auditService.Expect(s => s.LogDelete<AlcadaDetalhe>(null, null)).IgnoreArguments();
            auditService.Stub(s => s.LogDelete<AlcadaDetalhe>(null, null)).IgnoreArguments().Throw(new InvalidOperationException());

            var target = new AlcadaAuditStrategy(null, auditService, AlcadaAuditProperties, AlcadaDetalheAuditProperties);

            target.WillDelete(new AlcadaDetalhe(), new Alcada());

            auditService.VerifyAllExpectations();
        }
    }
}
