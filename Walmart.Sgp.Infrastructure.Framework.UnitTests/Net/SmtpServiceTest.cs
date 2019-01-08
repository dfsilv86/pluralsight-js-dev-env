using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Rhino.Mocks;
using Walmart.Sgp.Infrastructure.Framework.Net;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Net
{
    [TestFixture]
    [Category("Framework")]
    public class SmtpServiceTest
    {
        [Test]
        public void Send_EmailSemRemetente_EmailEnviado()
        {
            var cfg = new SmtpConfiguration();
            cfg.Domain = "testdomain.com";
            cfg.FromAddress = "testfrom@testdomain.com";
            cfg.FromDisplayName = "Test From Display Name";
            cfg.Password = "testpassword";
            cfg.Port = 2016;
            cfg.Server = "testserver.com";
            cfg.User = "testuser";

            var smtp = MockRepository.GenerateMock<IMailClient>();

            var svc = new SmtpService(cfg, smtp);

            var destinatarios = new List<MailAddress>()
            {
                new MailAddress("totest@testdomain.com")
            };

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write("Test attach content");
            writer.Flush();
            stream.Position = 0;

            var att = new List<Attachment>()
            {
                new Attachment(stream,"testfile","text/plain")
            };

            svc.Send(destinatarios, "Test Subject", "Test Body", att);

            smtp.AssertWasCalled(s => s.Send(Arg<MailMessage>.Is.Anything));
        }

        [Test]
        public void Send_Email_EmailEnviado()
        {
            var cfg = new SmtpConfiguration();
            cfg.Domain = "testdomain.com";
            cfg.FromAddress = "testfrom@testdomain.com";
            cfg.FromDisplayName = "Test From Display Name";
            cfg.Password = "testpassword";
            cfg.Port = 2016;
            cfg.Server = "testserver.com";
            cfg.User = "testuser";

            var smtp = MockRepository.GenerateMock<IMailClient>();

            var svc = new SmtpService(cfg, smtp);

            var from = new MailAddress("fromtest@testdomain.com");

            var destinatarios = new List<MailAddress>()
            {
                new MailAddress("totest@testdomain.com")
            };

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write("Test attach content");
            writer.Flush();
            stream.Position = 0;

            var att = new List<Attachment>()
            {
                new Attachment(stream,"testfile","text/plain")
            };

            svc.Send(from, destinatarios, "Test Subject", "Test Body", att);

            smtp.AssertWasCalled(s => s.Send(Arg<MailMessage>.Is.Anything));
        }
    }
}
