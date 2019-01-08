using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Helpers
{
    class TestEntity : EntityBase
    {
        public int Foo { get; set; }

        public int? NullableFoo { get; set; }

        public decimal Bar { get; set; }

        public decimal? NullableBar { get; set; }

        public string FooBar { get; set; }

        public DateTime BarFoo { get; set; }

        public DateTime? NullableBarFoo { get; set; }

        public TestFixedValueInt FixedInt { get; set;}

        public TestFixedValueString FixedString { get; set; }
    }

    class TestFixedValueInt : FixedValuesBase<int>
    {
        private TestFixedValueInt(int value) 
            : base(value)
        {
        }

        public static TestFixedValueInt One = new TestFixedValueInt(1);
    }

    class TestFixedValueString : FixedValuesBase<string>
    {
        private TestFixedValueString(string value) 
            : base(value)
        {
        }

        public static TestFixedValueString First = new TestFixedValueString("First");
    }
}
