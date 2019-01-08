using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Walmart.Sgp.Infrastructure.Framework.FileVault;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.FileVault
{
    [TestFixture]
    [Category("Framework")]
    [Category("FileVault")]
    public class FileVaultTicketTest
    {
        [Test]
        public void Create_FileName_Created()
        {
            var actual = FileVaultTicket.Create("arquivo.txt");

            Assert.IsNotNull(actual.Id);
            Assert.IsNotNull(actual.CreatedDate);
            Assert.IsNotNull(actual.PartialPath);
            Assert.IsNotNull(actual.FileName);
        }

        [Test]
        public void Deserialize_Null_Null()
        {
            string original = null;
            var actual = FileVaultTicket.Deserialize(original);

            Assert.IsNull(actual);
        }

        [Test]
        public void Deserialize_IdAndCreatedDate_Deserialized()
        {
            var original = FileVaultTicket.Create("arquivo.txt");
            var actual = FileVaultTicket.Deserialize(original.Id, original.CreatedDate);

            Assert.AreEqual(original.CreatedDate, actual.CreatedDate);
            Assert.AreEqual(original.FileName, actual.FileName);
            Assert.AreEqual(original.Id, actual.Id);
            Assert.AreEqual(original.PartialPath, actual.PartialPath);
        }

        [Test]
        public void Deserialize_String_Deserialized()
        {
            var original = FileVaultTicket.Create("arquivo.txt");
            var actual = FileVaultTicket.Deserialize(FileVaultTicket.Serialize(original));

            Assert.AreEqual(original.CreatedDate, actual.CreatedDate);
            Assert.AreEqual(original.FileName, actual.FileName);
            Assert.AreEqual(original.Id, actual.Id);
            Assert.AreEqual(original.PartialPath, actual.PartialPath);
        }

        [Test]
        public void TryParse_Null_NotParsed()
        {
            string original = null;
            FileVaultTicket actual;

            var result = FileVaultTicket.TryParse(original, out actual);

            Assert.IsFalse(result);
            Assert.IsNull(actual);
        }

        [Test]
        public void TryParse_IdAndCreatedDate_Parsed()
        {
            var original = FileVaultTicket.Create("arquivo.txt");
            var serialized = FileVaultTicket.Serialize(original);
            FileVaultTicket actual;

            var result = FileVaultTicket.TryParse(serialized, out actual);

            Assert.IsTrue(result);
            Assert.AreEqual(original.CreatedDate, actual.CreatedDate);
            Assert.AreEqual(original.FileName, actual.FileName);
            Assert.AreEqual(original.Id, actual.Id);
            Assert.AreEqual(original.PartialPath, actual.PartialPath);
        }

        [Test]
        public void TryParse_StringWithNoSeparator_NotParsed()
        {
            var original = "arquivo.xlsxxaf539cd8-f7da-4f4b-906d-c6dc06872919";
            FileVaultTicket actual;

            var result = FileVaultTicket.TryParse(original, out actual);

            Assert.IsFalse(result);
            Assert.IsNull(actual);
        }

        [Test]
        public void TryParse_StringWithMoreThanTwoSeparators_NotParsed()
        {
            var original = "arquivo.xlsxxaf539cd8-f7da-4f4b-906d-c6dc06872919|asd|asd";
            FileVaultTicket actual;

            var result = FileVaultTicket.TryParse(original, out actual);

            Assert.IsFalse(result);
            Assert.IsNull(actual);
        }

        [Test]
        public void TryParse_StringWithNoId_NotParsed()
        {
            var original = "|2016-01-01 23:59:59.999";
            FileVaultTicket actual;

            var result = FileVaultTicket.TryParse(original, out actual);

            Assert.IsFalse(result);
            Assert.IsNull(actual);
        }

        [Test]
        public void TryParse_InvalidId_NotParsed()
        {
            FileVaultTicket actual;

            var result = FileVaultTicket.TryParse("arquivo.xlsx.af539cd8-f7da-4f4b-906dxc6dc06872919|2016-01-01 23:59:59.000", out actual);

            Assert.IsFalse(result);
            Assert.IsNull(actual);

            result = FileVaultTicket.TryParse("arquivo.xlsx.af539cd8-f7da-4f4bx906d-c6dc06872919|2016-01-01 23:59:59.000", out actual);

            Assert.IsFalse(result);
            Assert.IsNull(actual);

            result = FileVaultTicket.TryParse("arquivo.xlsx.af539cd8-f7dax4f4b-906d-c6dc06872919|2016-01-01 23:59:59.000", out actual);

            Assert.IsFalse(result);
            Assert.IsNull(actual);

            result = FileVaultTicket.TryParse("arquivo.xlsx.af539cd8xf7da-4f4b-906d-c6dc06872919|2016-01-01 23:59:59.000", out actual);

            Assert.IsFalse(result);
            Assert.IsNull(actual);

            result = FileVaultTicket.TryParse("arquivo.xlsxxaf539cd8-f7da-4f4b-906d-c6dc06872919|2016-01-01 23:59:59.000", out actual);

            Assert.IsFalse(result);
            Assert.IsNull(actual);

            result = FileVaultTicket.TryParse("arquivo.xlsx|2016-01-01 23:59:59.000", out actual);

            Assert.IsFalse(result);
            Assert.IsNull(actual);
        }

        [Test]
        public void TryParse_StringWithNoDate_NotParsed()
        {
            var original = "arquivo.xlsx.af539cd8-f7da-4f4b-906d-c6dc06872919|asd";
            FileVaultTicket actual;

            var result = FileVaultTicket.TryParse(original, out actual);

            Assert.IsFalse(result);
            Assert.IsNull(actual);
        }

        [Test]
        public void Serialize_ValidTicket_ValidString()
        {
            var fvt = FileVaultTicket.Create("Teste.txt");

            var createdDate = fvt.CreatedDate;

            var createdDateNoMs = new DateTime(createdDate.Year, createdDate.Month, createdDate.Day, createdDate.Hour, createdDate.Minute, createdDate.Second);

            var serialized = FileVaultTicket.Serialize(fvt);

            var serializedDate = serialized.Split('|')[1];

            Assert.AreEqual("{0:yyyy-MM-dd HH:mm:ss}.000".With(createdDateNoMs), serializedDate);
        }
    }
}
