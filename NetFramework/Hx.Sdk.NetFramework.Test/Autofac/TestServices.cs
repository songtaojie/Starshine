using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Sdk.NetFramework.Test.Autofac
{
    class TestServices : ITestService
    {
        public void TestAutofac()
        {
            Console.WriteLine("Autofac成功");
            Console.ReadLine();
        }
    }
}
