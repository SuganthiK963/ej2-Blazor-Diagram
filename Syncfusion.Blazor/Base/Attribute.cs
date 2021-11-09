using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace Syncfusion.Blazor.Internal
{
    /// <summary>
    /// Custom JSON serialization type converter for the Template DataHashTable properties.
    /// </summary>
    internal class BlazorIdJsonConverter : JsonConverter
    {
        private readonly Type types;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlazorIdJsonConverter"/> class.
        /// </summary>
        /// <param name="values">Dictionary values for json convert.</param>
        public BlazorIdJsonConverter(Dictionary<string, object> values)
        {
            if (values.Count != 0)
            {
                types = values.FirstOrDefault().Value.GetType();
                HashData = values;
            }
        }

        /// <summary>
        /// Gets or sets hash data.
        /// </summary>
        public Dictionary<string, object> HashData { get; set; }

        /// <summary>
        /// Gets a value indicating whether this Newtonsoft.Json.JsonConverter can read JSON.
        /// </summary>
        public override bool CanRead
        {
            get { return false; }
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>true if this instance can convert the specified object type; otherwise, false.</returns>
        public override bool CanConvert(Type objectType)
        {
            return types == objectType;
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The Newtonsoft.Json.JsonWriter to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JToken t = JToken.FromObject(value);
            if (t.GetType().Name == "JObject")
            {
                JObject o = (JObject)t;
                var key = HashData.FirstOrDefault(x => x.Value == value).Key;
                if (!o.ContainsKey("BlazId"))
                {
                    o.AddFirst(new JProperty("BlazId", key));
                }

                o.WriteTo(writer);
            }
            else
            {
                t.WriteTo(writer);
            }
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The Newtonsoft.Json.JsonReader to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Used to convert the enum integer values into a string
    /// Also, ignores the string conversion of number enum.
    /// </summary>
    internal class NonFlagStringEnumConverter : StringEnumConverter
    {
        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>true if this instance can convert the specified object type; otherwise, false.</returns>
        public override bool CanConvert(Type objectType)
        {
            if (!base.CanConvert(objectType))
            {
                return false;
            }

            Type underlyingType = Nullable.GetUnderlyingType(objectType) ?? objectType;
            var attributes = underlyingType.GetCustomAttributes(typeof(FlagsAttribute), false);
            return attributes.Length == 0;
        }
    }

    /// <summary>
    /// Used to get the package name for specific component script loading.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    internal sealed class PackageNameAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PackageNameAttribute"/> class.
        /// </summary>
        /// <param name="packageName">package name.</param>
        public PackageNameAttribute(string packageName)
        {
            PackageName = packageName;
        }

        /// <summary>
        /// Gets the package name.
        /// </summary>
        public string PackageName { get; }
    }
}