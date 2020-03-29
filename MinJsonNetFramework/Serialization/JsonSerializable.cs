using System;
using System.Collections.Generic;
using System.Text;

namespace MinJSON.Serialization
{
    [System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class JsonSerializable : Attribute
    {
        public JsonSerializable(params object[] ConsturctorParams)
        {
            this.ConsturctorParams = ConsturctorParams;
            
        }

        public object[] ConsturctorParams
        {
            get;
        }
    }
}
