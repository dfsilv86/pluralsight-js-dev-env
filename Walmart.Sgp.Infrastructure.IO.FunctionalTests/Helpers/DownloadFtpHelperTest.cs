using System;
using System.IO;
using System.Linq;
using System.Net;
using NUnit.Framework;
using Walmart.Sgp.Infrastructure.IO.Helpers;

namespace Walmart.Sgp.Infrastructure.IO.FunctionalTests
{
    [TestFixture]
    [Category("FTP")]
    public class DownloadFtpHelperTest
    {
        [Test]
        public void DownloadFtpHelper_FindFiles_Ok()
        {
            Uri local = new Uri("ftp://localhost/test/");
            NetworkCredential networkCredential = new NetworkCredential("testuser", "testuser");

            var files = DownloadFtpHelper.EncontrarArquivos(local, networkCredential, null, false);

            Assert.AreEqual(5, files.Count());
            Assert.AreEqual(4, files.Where(f => !f.IsDiretorio).Count());
            Assert.AreEqual(1, files.Where(f => f.IsDiretorio).Count());

            files = DownloadFtpHelper.EncontrarArquivos(local, networkCredential, "arquivo*", false);

            Assert.IsTrue(files.Count() == 2);

            files = DownloadFtpHelper.EncontrarArquivos(local, networkCredential, "teste", false);

            Assert.IsTrue(files.Count() == 2);
        }

        [Test]
        public void DownloadFtpHelper_DownloadFiles_Ok()
        {
            string root = Path.GetDirectoryName(typeof(DownloadFtpHelperTest).Assembly.Location);

            string localPath = Path.Combine(root, @"Temp\DownloadFtpHelper\Test");

            if (Directory.Exists(localPath))
            {
                Directory.Delete(localPath, true);
            }

            Directory.CreateDirectory(localPath);

            Uri local = new Uri("ftp://localhost/test/");
            NetworkCredential networkCredential = new NetworkCredential("testuser", "testuser");

            var files = DownloadFtpHelper.EncontrarArquivos(local, networkCredential, "arquivo*", false);

            Assert.IsTrue(files.Count() == 2);

            var localFiles = DownloadFtpHelper.TransferirArquivos(localPath, networkCredential, files.Select(f => f.Uri).ToArray());

            foreach (var file in localFiles)
            {
                Assert.IsTrue(File.Exists(file));
            }
        }
    }
}
