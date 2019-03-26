using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloureLib.Core
{
    public class CloureImage
    {
        public string Name { get; set; }
        public string Title { get; set; }
        private byte[] bytes { get; set; }

        public byte[] GetBytes()
        {
            return bytes;
        }

        public string ToJSONString()
        {
            string json = "";
            json += "{";
            json += "\"Name\":\"" + Name + "\",";
            json += "\"Data\":\"" + Convert.ToBase64String(bytes) + "\"";
            json += "}";

            return json;
        }
    }
}
