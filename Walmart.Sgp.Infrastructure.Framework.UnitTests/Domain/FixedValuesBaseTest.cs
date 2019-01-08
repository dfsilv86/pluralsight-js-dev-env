using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Movimentacao;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Domain
{
    [TestFixture]
    [Category("Framework")]
    public class FixedValuesBaseTest
    {
        [Test]
        public void Equals_DiffValues_DiffResults()
        {
            Assert.AreEqual(TipoMovimento.AjusteInventario, TipoMovimento.AjusteInventario);
            Assert.AreNotEqual(TipoMovimento.AjusteInventario, TipoMovimento.Entrada);
            Assert.AreNotEqual(TipoMovimento.AjusteInventario, 1);
        }

        [Test]
        public void GetHashCode_DiffValues_DiffResults()
        {
            Assert.AreEqual(TipoMovimento.AjusteInventario.GetHashCode(), TipoMovimento.AjusteInventario.GetHashCode());
            Assert.AreNotEqual(TipoMovimento.AjusteInventario.GetHashCode(), TipoMovimento.Entrada.GetHashCode());            
        }

        [Test]
        public void GetTypeCode_NoArgs_Type()
        {
            Assert.AreEqual(TypeCode.Int32, TipoRelacionamento.Manipulado.GetTypeCode());
            Assert.AreEqual(TypeCode.String, TipoMovimento.AjusteInventario.GetTypeCode());
        }

        [Test]
        public void ToString_FormatProvider_String()
        {
            Assert.AreEqual("N", TipoMovimento.Nenhum.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual("I", TipoMovimento.AjusteInventario.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual("E", TipoMovimento.Entrada.ToString(CultureInfo.InvariantCulture));
        }

        [Test]
        public void ToInt16_FormatProvider_Int16()
        {
            Assert.AreEqual(3, ValorTipoReabastecimento.CrossDocking3.ToInt16(CultureInfo.InvariantCulture));
            Assert.AreEqual(7, ValorTipoReabastecimento.Dsd7.ToInt16(CultureInfo.InvariantCulture));
            Assert.AreEqual(20, ValorTipoReabastecimento.StapleStock20.ToInt16(CultureInfo.InvariantCulture));
        }

        [Test]
        public void ToInt32_FormatProvider_Int32()
        {
            Assert.AreEqual(0, TipoArquivoInventario.Nenhum.ToInt32(CultureInfo.InvariantCulture));
            Assert.AreEqual(1, TipoArquivoInventario.Final.ToInt32(CultureInfo.InvariantCulture));
            Assert.AreEqual(2, TipoArquivoInventario.Parcial.ToInt32(CultureInfo.InvariantCulture));
        }

        [Test]
        public void ToType_FormatProvider_Type()
        {
            Assert.AreEqual("0", TipoArquivoInventario.Nenhum.ToType(typeof(string), CultureInfo.InvariantCulture));
            Assert.AreEqual("1", TipoArquivoInventario.Final.ToType(typeof(string), CultureInfo.InvariantCulture));
            Assert.AreEqual("2", TipoArquivoInventario.Parcial.ToType(typeof(string), CultureInfo.InvariantCulture));
        }

        [Test]
        public void ToType_SameType_NoChange()
        {
            TipoArquivoInventario target = TipoArquivoInventario.Nenhum;
            
            object converted = Convert.ChangeType((object)target, typeof(TipoArquivoInventario));

            Assert.AreEqual((object)target, converted);
        }

        [Test]
        public void NotImplementedMethods_Args_Exception()
        {
            Assert.Throws<NotImplementedException>(() => TipoMovimento.AjusteInventario.ToBoolean(CultureInfo.InvariantCulture));
            Assert.Throws<NotImplementedException>(() => TipoMovimento.AjusteInventario.ToByte(CultureInfo.InvariantCulture));
            Assert.Throws<NotImplementedException>(() => TipoMovimento.AjusteInventario.ToChar(CultureInfo.InvariantCulture));
            Assert.Throws<NotImplementedException>(() => TipoMovimento.AjusteInventario.ToDateTime(CultureInfo.InvariantCulture));
            Assert.Throws<NotImplementedException>(() => TipoMovimento.AjusteInventario.ToDateTime(CultureInfo.InvariantCulture));
            Assert.Throws<NotImplementedException>(() => TipoMovimento.AjusteInventario.ToDecimal(CultureInfo.InvariantCulture));
            Assert.Throws<NotImplementedException>(() => TipoMovimento.AjusteInventario.ToDouble(CultureInfo.InvariantCulture));            
            Assert.Throws<NotImplementedException>(() => TipoMovimento.AjusteInventario.ToInt64(CultureInfo.InvariantCulture));
            Assert.Throws<NotImplementedException>(() => TipoMovimento.AjusteInventario.ToSByte(CultureInfo.InvariantCulture));
            Assert.Throws<NotImplementedException>(() => TipoMovimento.AjusteInventario.ToSingle(CultureInfo.InvariantCulture));            
            Assert.Throws<NotImplementedException>(() => TipoMovimento.AjusteInventario.ToUInt16(CultureInfo.InvariantCulture));
            Assert.Throws<NotImplementedException>(() => TipoMovimento.AjusteInventario.ToUInt32(CultureInfo.InvariantCulture));
            Assert.Throws<NotImplementedException>(() => TipoMovimento.AjusteInventario.ToUInt64(CultureInfo.InvariantCulture));
        }

        [Test]
        public void ValueAsObject_NoArgs_Value()
        {
            TipoArquivoInventario target = TipoArquivoInventario.Final;

            Assert.AreEqual(1, ((IFixedValue) target).ValueAsObject);
        }
    }
}
