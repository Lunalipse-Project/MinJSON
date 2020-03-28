using MinJSON.JSON;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinJSON.Serialization
{
    public interface IJsonValueConverter
    {
        JsonValue ConvertTo(object obj);
        object ConvertFromJson(JsonValue jsonValue);
    }
}
