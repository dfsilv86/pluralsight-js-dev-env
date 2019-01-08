using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Infrastructure.Framework.FileVault;
using Walmart.Sgp.Infrastructure.Framework.Processing;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Processing
{
    [TestFixture]
    [Category("Framework")]
    [Category("Processing")]
    public class ProcessOrderArgumentExtensionsTest
    {
        private readonly Func<FileVaultTicket, string> mockMoveToInputFile = (fvt) => "Teste\\{0}".With(fvt.FileName);
        private readonly Func<string, FileVaultTicket> mockTicketForInputFile = (filePath) => FileVaultTicket.Create(Path.GetFileName(filePath));

        [Test]
        public void ToArgumentList_SimpleParameters_ArgumentList()
        {
            Dictionary<string, object> target = new Dictionary<string, object>();

            target["int"] = 1;
            target["long"] = (long)2;
            target["string"] = "test";
            target["float"] = 123.4f;
            target["intNullable"] = (int?)3;
            target["stringNull"] = (string)null;
            target["dateTime"] = new DateTime(2016, 02, 03, 04, 05, 06, 777);
            target["timeSpan"] = new TimeSpan(07, 04, 05);

            var result = target.ToArgumentList(new string[] { "float", "timeSpan" }, mockMoveToInputFile);

            Assert.AreEqual(8, result.Count);

            var intArg = result.SingleOrDefault(x => x.Name == "int");
            var longArg = result.SingleOrDefault(x => x.Name == "long");
            var stringArg = result.SingleOrDefault(x => x.Name == "string");
            var floatArg = result.SingleOrDefault(x => x.Name == "float");
            var intNullableArg = result.SingleOrDefault(x => x.Name == "intNullable");
            var stringNullArg = result.SingleOrDefault(x => x.Name == "stringNull");
            var dateTimeArg = result.SingleOrDefault(x => x.Name == "dateTime");
            var timeSpanArg = result.SingleOrDefault(x => x.Name == "timeSpan");

            Assert.IsNotNull(intArg);
            Assert.IsNotNull(longArg);
            Assert.IsNotNull(stringArg);
            Assert.IsNotNull(floatArg);
            Assert.IsNotNull(intNullableArg);
            Assert.IsNotNull(stringNullArg);
            Assert.IsNotNull(dateTimeArg);
            Assert.IsNotNull(timeSpanArg);

            Assert.AreEqual("1", intArg.Value);
            Assert.AreEqual("2", longArg.Value);
            Assert.AreEqual("test", stringArg.Value);
            Assert.AreEqual("123.4", floatArg.Value);
            Assert.AreEqual("3", intNullableArg.Value);
            Assert.IsNull(stringNullArg.Value);
            Assert.AreEqual("2016-02-03 04:05:06.777", dateTimeArg.Value);
            Assert.AreEqual("07:04:05", timeSpanArg.Value);
        }

        [Test]
        public void ToArgumentList_ModelWithArrayAndFileVaultTicket_ArgumentList()
        {
            var ticket1 = FileVaultTicket.Create("Teste1.txt");
            var ticket2 = FileVaultTicket.Create("Teste2.txt");
            var data = DateTime.Now;
            var dataComoString = "{0:yyyy-MM-dd HH:mm:ss.fff}".With(data);

            Dictionary<string, object> target = new Dictionary<string, object>();

            target["request"] = new ImportarInventarioManualRequest()
                {
                    CdSistema = 1,
                    DataInventario = data,
                    IdBandeira = 1,
                    IdLoja = 243,
                    Arquivos = new FileVaultTicket[] { ticket1, ticket2 }
                };

            target["tipoOrigem"] = TipoOrigemImportacao.HO;

            var result = target.ToArgumentList(new string[0], mockMoveToInputFile);

            Assert.AreEqual(7, result.Count);

            var arg1 = result.SingleOrDefault(x => x.Name == "request.CdSistema");
            var arg2 = result.SingleOrDefault(x => x.Name == "request.DataInventario");
            var arg3 = result.SingleOrDefault(x => x.Name == "request.IdBandeira");
            var arg4 = result.SingleOrDefault(x => x.Name == "request.IdLoja");
            var arg5 = result.SingleOrDefault(x => x.Name == "request.Arquivos[0]");
            var arg6 = result.SingleOrDefault(x => x.Name == "request.Arquivos[1]");
            var arg7 = result.SingleOrDefault(x => x.Name == "tipoOrigem");

            Assert.IsNotNull(arg1);
            Assert.IsNotNull(arg2);
            Assert.IsNotNull(arg3);
            Assert.IsNotNull(arg4);
            Assert.IsNotNull(arg5);
            Assert.IsNotNull(arg6);
            Assert.IsNotNull(arg7);

            Assert.AreEqual("1", arg1.Value);
            Assert.AreEqual(dataComoString, arg2.Value);
            Assert.AreEqual("1", arg3.Value);
            Assert.AreEqual("243", arg4.Value);
            Assert.AreEqual("Teste\\{0}".With(ticket1.FileName), arg5.Value);
            Assert.AreEqual("Teste\\{0}".With(ticket2.FileName), arg6.Value);
            Assert.AreEqual("1", arg7.Value);
        }

        [Test]
        public void ToArgumentList_ModelWithDictionary_ArgumentList()
        {
            DateTime date = DateTime.Now;
            string dataComoString = date.ToString("yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);

            Dictionary<string, object> target = new Dictionary<string, object>();
            Dictionary<string, object> dict1 = new Dictionary<string, object>();
            Dictionary<string, object> dict2 = new Dictionary<string, object>();

            dict1["foo"] = "bar";
            dict1["date"] = date;

            dict2["bar"] = "foo";
            dict2["nulled"] = null;
            dict2["array"] = new int[] { 1, 2, 3 };

            target["dict1"] = dict1;
            target["dict2"] = dict2;

            var result = target.ToArgumentList(new string[0], mockMoveToInputFile);

            Assert.AreEqual(7, result.Count);

            var arg1 = result.SingleOrDefault(x => x.Name == "dict1[foo]");
            var arg2 = result.SingleOrDefault(x => x.Name == "dict1[date]");
            var arg3 = result.SingleOrDefault(x => x.Name == "dict2[bar]");
            var arg4 = result.SingleOrDefault(x => x.Name == "dict2[nulled]");
            var arg5 = result.SingleOrDefault(x => x.Name == "dict2[array][0]");
            var arg6 = result.SingleOrDefault(x => x.Name == "dict2[array][1]");
            var arg7 = result.SingleOrDefault(x => x.Name == "dict2[array][2]");

            Assert.IsNotNull(arg1);
            Assert.IsNotNull(arg2);
            Assert.IsNotNull(arg3);
            Assert.IsNotNull(arg4);
            Assert.IsNotNull(arg5);
            Assert.IsNotNull(arg6);
            Assert.IsNotNull(arg7);

            Assert.AreEqual("bar", arg1.Value);
            Assert.AreEqual(dataComoString, arg2.Value);
            Assert.AreEqual("foo", arg3.Value);
            Assert.IsNull(arg4.Value);
            Assert.AreEqual("1", arg5.Value);
            Assert.AreEqual("2", arg6.Value);
            Assert.AreEqual("3", arg7.Value);
        }

        [Test]
        public void ToArgumentList_UnsupportedType_Exception()
        {
            Dictionary<string, object> target = new Dictionary<string, object>();

            target["unsupported"] = new System.FtpStyleUriParser();

            Assert.Throws<NotSupportedException>(() =>
            {
                var result = target.ToArgumentList(new string[0], mockMoveToInputFile);
            });
        }

        [Test]
        public void ToArgumentList_IEnumerable_ArrayLikeList()
        {
            Dictionary<string, object> target = new Dictionary<string, object>();

            target["ienumerable"] = CreateIEnumerable();

            var result = target.ToArgumentList(new string[0], mockMoveToInputFile);

            Assert.AreEqual(10, result.Count);

            var first = result.SingleOrDefault(x => x.Name == "ienumerable[0]");

            Assert.IsNotNull(first);
            Assert.AreEqual("0", first.Value);

            var last = result.SingleOrDefault(x => x.Name == "ienumerable[9]");

            Assert.IsNotNull(last);
            Assert.AreEqual("9", last.Value);
        }

        [Test]
        public void ToServiceParameters_ArgumentList_SimpleParameters()
        {
            Dictionary<string, object> target = new Dictionary<string, object>();

            target["int"] = 1;
            target["long"] = (long)2;
            target["string"] = "test";
            target["float"] = 123.4f;
            target["intNullable"] = (int?)3;
            target["stringNull"] = (string)null;
            target["dateTime"] = new DateTime(2016, 02, 03, 04, 05, 06, 777);
            target["timeSpan"] = new TimeSpan(07, 04, 05);

            var argList = target.ToArgumentList(new string[0], mockMoveToInputFile);

            Dictionary<string, Type> types = new Dictionary<string, Type>();
            types["int"] = typeof(int);
            types["long"] = typeof(long);
            types["string"] = typeof(string);
            types["float"] = typeof(float);
            types["intNullable"] = typeof(int?);
            types["stringNull"] = typeof(string);
            types["dateTime"] = typeof(DateTime);
            types["timeSpan"] = typeof(TimeSpan);

            var result = argList.ToServiceParameters(types, mockTicketForInputFile);

            Assert.AreEqual(8, result.Count);

            Assert.AreEqual((int)1, result["int"]);
            Assert.AreEqual((long)2, result["long"]);
            Assert.AreEqual("test", result["string"]);
            Assert.AreEqual((float)123.4, result["float"]);
            Assert.AreEqual((int?)3, result["intNullable"]);
            Assert.IsNull(result["stringNull"]);
            Assert.AreEqual(new DateTime(2016, 02, 03, 04, 05, 06, 777), result["dateTime"]);
            Assert.AreEqual(new TimeSpan(07, 04, 05), result["timeSpan"]);
        }

        [Test]
        public void ToServiceParameters_ArgumentsForModelWithArrayAndFileVaultTicket_Parameters()
        {
            var ticket1 = FileVaultTicket.Create("Teste1.txt");
            var ticket2 = FileVaultTicket.Create("Teste2.txt");
            var data = DateTime.Now;
            var dataComoString = "{0:yyyy-MM-dd HH:mm:ss.fff}".With(data);

            Dictionary<string, object> source = new Dictionary<string, object>();

            source["request"] = new ImportarInventarioManualRequest()
            {
                CdSistema = 1,
                DataInventario = data,
                IdBandeira = 1,
                IdLoja = 243,
                Arquivos = new FileVaultTicket[] { ticket1, ticket2 }
            };

            source["tipoOrigem"] = TipoOrigemImportacao.HO;

            var target = source.ToArgumentList(new string[0], mockMoveToInputFile);

            Dictionary<string, Type> parameterTypes = new Dictionary<string, Type>();
            parameterTypes["request"] = typeof(ImportarInventarioManualRequest);
            parameterTypes["tipoOrigem"] = typeof(TipoOrigemImportacao);

            var parameters = target.ToServiceParameters(parameterTypes, mockTicketForInputFile);

            Assert.AreEqual(2, parameters.Count());

            var requestParam = parameters["request"];
            var tipoOrigemParam = parameters["tipoOrigem"];

            Assert.IsNotNull(requestParam);
            Assert.IsNotNull(tipoOrigemParam);

            Assert.AreEqual(TipoOrigemImportacao.HO, tipoOrigemParam);

            Assert.IsInstanceOf<ImportarInventarioManualRequest>(requestParam);

            ImportarInventarioManualRequest theRequestParam = (ImportarInventarioManualRequest)requestParam;

            Assert.AreEqual(1, theRequestParam.CdSistema);
            Assert.AreEqual(DateTime.Parse(dataComoString), theRequestParam.DataInventario);
            Assert.AreEqual(1, theRequestParam.IdBandeira);
            Assert.AreEqual(243, theRequestParam.IdLoja);
            Assert.IsNotNull(theRequestParam.Arquivos);

            Assert.AreEqual(2, theRequestParam.Arquivos.Count());

            Assert.IsTrue(theRequestParam.Arquivos.Any(x => x.FileName == ticket1.FileName));
            Assert.IsTrue(theRequestParam.Arquivos.Any(x => x.FileName == ticket2.FileName));
        }

        [Test]
        public void ToServiceParameters_ArgumentsForModelWithDictionary_Parameters()
        {
            DateTime date = DateTime.Now;
            string dataComoString = date.ToString("yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);

            Dictionary<string, object> target = new Dictionary<string, object>();
            Dictionary<string, object> dict1 = new Dictionary<string, object>();
            Dictionary<string, object> dict2 = new Dictionary<string, object>();

            dict1["foo"] = "bar";
            dict1["date"] = date;

            dict2["bar"] = "foo";
            dict2["nulled"] = null;
            dict2["array"] = new int[] { 1, 2, 3 };

            target["dict1"] = dict1;
            target["dict2"] = dict2;

            var arguments = target.ToArgumentList(new string[0], mockMoveToInputFile);

            Dictionary<string, Type> parameterTypes = new Dictionary<string, Type>();
            parameterTypes["dict1"] = parameterTypes["dict2"] = typeof(Dictionary<string, object>);

            var result = arguments.ToServiceParameters(parameterTypes, mockTicketForInputFile);

            Assert.AreEqual(2, result.Count);

            var arg1 = result["dict1"] as Dictionary<string, object>;
            var arg2 = result["dict2"] as Dictionary<string, object>;
            var array = arg2["array"] as Dictionary<string, object>;

            Assert.IsNotNull(arg1);
            Assert.IsNotNull(arg2);
            Assert.IsNotNull(array);

            Assert.AreEqual("bar", arg1["foo"]);
            Assert.AreEqual(dataComoString, arg1["date"]);
            Assert.AreEqual("foo", arg2["bar"]);
            Assert.IsNull(arg2["nulled"]);
            Assert.AreEqual("1", array["0"]);
            Assert.AreEqual("2", array["1"]);
            Assert.AreEqual("3", array["2"]);
        }

        [Test]
        public void ToServiceParameters_ArgumentsForDictionaryOfEnumerablesOfDateTime_Parameters()
        {
            DateTime[] datas = new DateTime[] { DateTime.Now.AddHours(-1), DateTime.Now.AddHours(-2), DateTime.Now.AddHours(-3), DateTime.Now.AddHours(-4) };
            string[] datasComoString = new string[4];
            for (var i = 0; i < datas.Length; i++)
            {
                datasComoString[i] = datas[i].ToString("yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
            }

            Dictionary<string, object> target = new Dictionary<string, object>();
            Dictionary<int, IEnumerable<DateTime>> dict1 = new Dictionary<int, IEnumerable<DateTime>>();
            Dictionary<int, List<DateTime>> dict2 = new Dictionary<int, List<DateTime>>();

            dict1[2] = new DateTime[] { datas[0], datas[1] };
            dict1[5] = new List<DateTime>(new DateTime[] { datas[2], datas[3] });

            dict2[1] = new List<DateTime>(new DateTime[] { datas[2], datas[1] });
            dict2[9] = new List<DateTime>(new DateTime[] { datas[3], datas[0] });
            dict2[36] = new List<DateTime>(new DateTime[] { datas[3], datas[0], datas[3] });

            target["dict1"] = dict1;
            target["dict2"] = dict2;

            var arguments = target.ToArgumentList(new string[0], mockMoveToInputFile);

            Dictionary<string, Type> parameterTypes = new Dictionary<string, Type>();
            parameterTypes["dict1"] = typeof(IDictionary<int, IEnumerable<DateTime>>);
            parameterTypes["dict2"] = typeof(Dictionary<int, List<DateTime>>);

            var result = arguments.ToServiceParameters(parameterTypes, mockTicketForInputFile);

            Assert.AreEqual(2, result.Count);

            var arg1 = result["dict1"] as IDictionary<int, IEnumerable<DateTime>>;
            var arg2 = result["dict2"] as Dictionary<int, List<DateTime>>;

            Assert.IsNotNull(arg1);
            Assert.IsNotNull(arg2);

            Assert.AreEqual(2, arg1.Count);
            Assert.AreEqual(3, arg2.Count);

            Assert.AreEqual(2, arg1[2].Count());
            Assert.AreEqual(2, arg1[5].Count());

            Assert.AreEqual(2, arg2[1].Count());
            Assert.AreEqual(2, arg2[9].Count());
            Assert.AreEqual(3, arg2[36].Count());

            Assert.AreEqual(datasComoString[0], arg1[2].First().ToString("yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture));
            Assert.AreEqual(datasComoString[3], arg1[5].Last().ToString("yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture));

            Assert.AreEqual(datasComoString[2], arg2[1][0].ToString("yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture));
            Assert.AreEqual(datasComoString[1], arg2[1][1].ToString("yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture));
            Assert.AreEqual(datasComoString[3], arg2[36][0].ToString("yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture));
            Assert.AreEqual(datasComoString[0], arg2[36][1].ToString("yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture));
            Assert.AreEqual(datasComoString[3], arg2[36][2].ToString("yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture));
        }

        [Test]
        public void ToServiceParameters_MalformedName_Exception()
        {
            List<ProcessOrderArgument> target = new List<ProcessOrderArgument>();

            // inválido, pois unsupported é um dicionário, não um objeto.
            target.Add(new ProcessOrderArgument() { Name = "unsupported.0", Value = "1" });

            Dictionary<string, Type> types = new Dictionary<string, Type>();
            types["unsupported"] = typeof(Dictionary<int, int>);

            Assert.Throws<InvalidOperationException>(() =>
            {
                var result = target.ToServiceParameters(types, mockTicketForInputFile);
            });
        }

        [Test]
        public void ToServiceParameters_OldDictionary_Parameters()
        {
            List<ProcessOrderArgument> target = new List<ProcessOrderArgument>();

            target.Add(new ProcessOrderArgument() { Name = "unsupported[a]", Value = "1" });

            Dictionary<string, Type> types = new Dictionary<string, Type>();
            types["unsupported"] = typeof(System.Collections.Hashtable);

            var result = target.ToServiceParameters(types, mockTicketForInputFile);

            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.ContainsKey("unsupported"));

            var hash = result["unsupported"] as IDictionary;

            Assert.IsNotNull(hash);

            Assert.AreEqual(1, hash.Count);

            foreach (var key in hash.Keys)
            {
                Assert.AreEqual("a", key);
            }

            foreach (var value in hash.Values)
            {
                Assert.AreEqual("1", value);
            }
        }

        [Test]
        public void GetParameterNames_Null_EmptyArgumentList()
        {
            List<ProcessOrderArgument> target = null;

            var result = target.GetParameterNames();

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public void GetParameterNames_ArgumentList_Names()
        {
            var ticket1 = FileVaultTicket.Create("Teste1.txt");
            var ticket2 = FileVaultTicket.Create("Teste2.txt");
            var data = DateTime.Now;
            var dataComoString = "{0:yyyy-MM-dd HH:mm:ss.fff}".With(data);

            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters["request"] = new ImportarInventarioManualRequest()
            {
                CdSistema = 1,
                DataInventario = data,
                IdBandeira = 1,
                IdLoja = 243,
                Arquivos = new FileVaultTicket[] { ticket1, ticket2 }
            };

            parameters["tipoOrigem"] = TipoOrigemImportacao.HO;

            var argumentList = parameters.ToArgumentList(new string[0], mockMoveToInputFile);

            var result = argumentList.GetParameterNames();

            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Contains("request"));
            Assert.IsTrue(result.Contains("tipoOrigem"));
        }

        [Test]
        public void GetParameterNames_Parameters_Names()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters["request"] = 1;
            parameters["request.CdSistema"] = 1;
            parameters["tipoOrigem"] = 2;

            var result = parameters.GetParameterNames();

            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Contains("request"));
            Assert.IsTrue(result.Contains("tipoOrigem"));
        }

        [Test]
        public void DeserializeValue_SerializedValues_DeserializedValues()
        {
            FileVaultTicket fileVaultTicket = FileVaultTicket.Create("Teste1.xyz");
            string fileVaultTicketAsString = FileVaultTicket.Serialize(fileVaultTicket);

            Assert.IsNull(ProcessOrderArgumentExtensions.DeserializeValue(null, typeof(object), mockTicketForInputFile));
            Assert.IsNull(ProcessOrderArgumentExtensions.DeserializeValue(null, typeof(int?), mockTicketForInputFile));
            Assert.IsNull(ProcessOrderArgumentExtensions.DeserializeValue(null, typeof(TipoProcessoImportacao), mockTicketForInputFile));
            Assert.IsNull(ProcessOrderArgumentExtensions.DeserializeValue(null, typeof(DateTime?), mockTicketForInputFile));
            Assert.IsNull(ProcessOrderArgumentExtensions.DeserializeValue(null, typeof(TimeSpan?), mockTicketForInputFile));
            Assert.IsNull(ProcessOrderArgumentExtensions.DeserializeValue(null, typeof(FileVaultTicket), mockTicketForInputFile));
            Assert.AreEqual("A", ProcessOrderArgumentExtensions.DeserializeValue("A", typeof(string), mockTicketForInputFile));
            Assert.AreEqual(1, ProcessOrderArgumentExtensions.DeserializeValue("1", typeof(int), mockTicketForInputFile));
            Assert.AreEqual(1, ProcessOrderArgumentExtensions.DeserializeValue("1", typeof(int?), mockTicketForInputFile));
            Assert.AreEqual(1L, ProcessOrderArgumentExtensions.DeserializeValue("1", typeof(long), mockTicketForInputFile));
            Assert.AreEqual(1L, ProcessOrderArgumentExtensions.DeserializeValue("1", typeof(long?), mockTicketForInputFile));
            Assert.AreEqual(1.2f, ProcessOrderArgumentExtensions.DeserializeValue("1.2", typeof(float), mockTicketForInputFile));
            Assert.AreEqual(1.2f, ProcessOrderArgumentExtensions.DeserializeValue("1.2", typeof(float?), mockTicketForInputFile));
            Assert.AreEqual(1.2d, ProcessOrderArgumentExtensions.DeserializeValue("1.2", typeof(double), mockTicketForInputFile));
            Assert.AreEqual(1.2d, ProcessOrderArgumentExtensions.DeserializeValue("1.2", typeof(double?), mockTicketForInputFile));
            Assert.AreEqual(1.2m, ProcessOrderArgumentExtensions.DeserializeValue("1.2", typeof(decimal), mockTicketForInputFile));
            Assert.AreEqual(1.2m, ProcessOrderArgumentExtensions.DeserializeValue("1.2", typeof(decimal?), mockTicketForInputFile));
            Assert.AreEqual(new DateTime(2016, 02, 03, 04, 05, 06), ProcessOrderArgumentExtensions.DeserializeValue("2016-02-03 04:05:06", typeof(DateTime), mockTicketForInputFile));
            Assert.AreEqual(new DateTime(2016, 02, 03, 04, 05, 06), ProcessOrderArgumentExtensions.DeserializeValue("2016-02-03 04:05:06", typeof(DateTime?), mockTicketForInputFile));
            Assert.AreEqual(new TimeSpan(04, 05, 06), ProcessOrderArgumentExtensions.DeserializeValue("04:05:06", typeof(TimeSpan), mockTicketForInputFile));
            Assert.AreEqual(new TimeSpan(04, 05, 06), ProcessOrderArgumentExtensions.DeserializeValue("04:05:06", typeof(TimeSpan?), mockTicketForInputFile));
            Assert.AreEqual(fileVaultTicket.FileName, ((FileVaultTicket)ProcessOrderArgumentExtensions.DeserializeValue(mockMoveToInputFile(fileVaultTicket), typeof(FileVaultTicket), mockTicketForInputFile)).FileName);
            Assert.AreEqual(TipoProcessoImportacao.Manual, ProcessOrderArgumentExtensions.DeserializeValue("1", typeof(TipoProcessoImportacao), mockTicketForInputFile));
        }

        private IEnumerable<int> CreateIEnumerable()
        {
            for (var i = 0; i < 10; i++)
            {
                yield return i;
            }
        }
    }
}
