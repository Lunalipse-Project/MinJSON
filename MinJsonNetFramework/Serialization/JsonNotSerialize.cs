using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinJSON.Serialization
{
    [System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class JsonNotSerialize : Attribute
    {
        public JsonNotSerialize()
        {

        }
    }
}
