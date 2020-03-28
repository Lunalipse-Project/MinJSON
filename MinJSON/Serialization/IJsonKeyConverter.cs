using System;
using System.Collections.Generic;
using System.Text;

namespace MinJSON.Serialization
{
    public interface IJsonKeyConverter
    {
        string ConvertTo(object key);
        object ConvertFrom(string propertyKey);
    }
}
