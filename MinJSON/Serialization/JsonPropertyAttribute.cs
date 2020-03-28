using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace MinJSON.Serialization
{
    /// <summary>
    /// Add extra information during the serialization on a instance variable.
    /// MinJson will only serialize public instance variable and any instance variable with this attribute
    /// </summary>
    [System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class JsonPropertyAttribute : Attribute
    {
        public JsonPropertyAttribute()
        {

        }

        public string PropertyName { get; set; }

        /// <summary>
        /// Get or set the custom converter used for serialization process.
        /// In case of field it will apply on the field value
        /// In case of array or any type implemented <see cref="IEnumerable"/> interface, will be applied on every individual element
        /// </summary>
        public IJsonValueConverter ValueConverter { get; set; }

        /// <summary>
        /// Get or set the custom converter for non-string key value (For type that implement <see cref="IDictionary{TKey, TValue}"/> interface)
        /// </summary>
        public IJsonKeyConverter KeyConverter { get; set; }
    }
}
