using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratorTests
{
    public class Class1
    {
        public Class1(string _nothing)
        {
            Nothing = _nothing;
        }
        public string Nothing { get; set; }

        public void Method1()
        {
            string temp = "asdasdasd";
            temp = temp + "qwerty";
            Nothing = Nothing + temp;
        }
    }

    public class Class2
    {
        public Class2(string _nothing)
        {
            Nothing = _nothing;
        }
        public string Nothing { get; set; }

        public string Method2()
        {
            string temp = "asdasdasd";
            Nothing = temp;
            return Nothing;
        }
    }
}
