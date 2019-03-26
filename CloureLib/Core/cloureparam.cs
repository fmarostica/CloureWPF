using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloureLib.Core
{
    public class CloureParam
    {
        public string name { get; private set; }
        public object value { get; private set; }

        public CloureParam(string name, object value)
        {
            this.name = name;
            this.value = value;
        }
    }
}
