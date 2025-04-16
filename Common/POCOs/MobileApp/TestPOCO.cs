using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.POCOs.MobileApp
{
    public class TestPOCO : Poco
    {
        public TestPOCO() { }
        public TestPOCO(int testInt)
        {
            TestInt = testInt;
        }
        public int? TestInt { get; set; }
        

    }

}
