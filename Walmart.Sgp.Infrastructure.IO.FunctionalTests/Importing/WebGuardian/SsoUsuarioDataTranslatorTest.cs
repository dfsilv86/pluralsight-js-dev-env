﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Walmart.Sgp.Infrastructure.IO.Importing;
using Walmart.Sgp.Infrastructure.IO.Importing.WebGuardian;

namespace Walmart.Sgp.Infrastructure.IO.FunctionalTests.Importing.WebGuardian
{
    [TestFixture]
    [Category("WebGuardian")]
    public class SsoUsuarioDataTranslatorTest
    {
        [Test]
        public void Translate_ValidUserCredentials_UsuarioInfo()
        {
            var options = new SsoOptions
            {
                ApplicationCode = 47,
                EmailDomain = "cwi.com.br",
                ProfileCode = 178,
                UserName = "dsilv19",
                UserPassword = "Walmart29"
            };

            var target = new SsoUsuarioDataTranslator(new WebGuardianSsoService(options.EmailDomain), options);
            var actual = target.Translate();
            Assert.IsNotNull(actual);
            Assert.AreEqual("dsilv19", actual.Usuario.UserName);

            Assert.IsNotNull(actual.Papel);
            Assert.AreEqual("SGP-Administrador", actual.Papel.Name);        
        }
    }
}