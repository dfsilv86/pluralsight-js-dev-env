using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.IO;
using System.Collections;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Text
{
    [TestFixture]
    [Category("Framework")]
    public class StringExtensionsTest
    {
        [Test]
        public void With_Args_Formatted()
        {
            var actual = "Test {0} and {1}".With(1, 2);
            Assert.AreEqual("Test 1 and 2", actual);
        }

        [Test]
        public void EscapeForFormat_String_Escaped()
        {
            var theString = "Test {0} and {1}";

            var actual = theString.EscapeForFormat().With(1, 2);
            Assert.AreEqual(theString, actual);
        }

        [Test]
        public void EscapeForFormat_Null_Null()
        {
            string theString = null;

            var actual = theString.EscapeForFormat();

            Assert.IsNull(actual);
        }

        [Test]
        public void IsExecutableExtension_Null_False()
        {
            string actual = null;

            Assert.IsFalse(actual.IsExecutableExtension());
        }

        [Test]
        public void IsExecutableExtension_Extension_True()
        {
            var files = new string[] { "command.com", "xcopy.exe", "autoexec.bat", "console.cmd", "shortcut.pif", "installer.msi" };

            files.ToList().ForEach(theFilename =>
            {
                Assert.AreEqual(true, Path.GetExtension(theFilename).IsExecutableExtension());
            });
        }

        [Test]
        public void IsExecutableExtension_Extension_False()
        {
            var files = new string[] { "image.png", "file.bmp", "document.docx", "report.pdf", "plan.xls" };

            files.ToList().ForEach(theFilename =>
            {
                Assert.AreEqual(false, Path.GetExtension(theFilename).IsExecutableExtension());
            });
        }

        [Test]
        public void JoinWords_Null_StringEmpty()
        {
            Assert.AreEqual(string.Empty, ((IEnumerable<int>)null).JoinWords());
        }

        [Test]
        public void JoinWords_NoItem_NoSeparator()
        {
            Assert.AreEqual("", (new int[0]).JoinWords());
        }

        [Test]
        public void JoinWords_OneItem_NoSeparator()
        {
            Assert.AreEqual("1", (new int[] { 1 }).JoinWords());
        }

        [Test]
        public void JoinWords_TwoItems_AndSeparator()
        {
            Assert.AreEqual("1 e 2", (new int[] { 1, 2 }).JoinWords());
        }

        [Test]
        public void JoinWords_ThreeOrMoreItems_CommasAndSeparator()
        {
            Assert.AreEqual("1, 2 e 3", (new int[] { 1, 2, 3 }).JoinWords());
            Assert.AreEqual("1, 2, 3 e 4", (new int[] { 1, 2, 3, 4 }).JoinWords());
        }

        [Test]
        public void GetWord_IndexOne_SecondWord()
        {
            Assert.AreEqual("world", "hello world".GetWord(1));
        }

        [Test]
        public void GetWord_IndexOutOfBounds_Null()
        {
            Assert.IsNull("hello world".GetWord(2));
        }

        [Test]
        public void GetWord_EmptyString_Null()
        {
            Assert.IsNull("".GetWord(0));
        }

        [Test]
        public void ToStringSN_True_S()
        {
            Assert.AreEqual("S", true.ToStringSN());
        }

        [Test]
        public void ToStringSN_False_N()
        {
            Assert.AreEqual("N", false.ToStringSN());
        }

        [Test]
        public void ToInt32_InputIsNotANumber_Zero()
        {
            Assert.AreEqual(0, "".ToInt32());
            Assert.AreEqual(0, "something".ToInt32());
        }

        [Test]
        public void ToInt32_InputIsNotANumber_InputNumber()
        {
            Assert.AreEqual(123, "123".ToInt32());
        }
    }
}
