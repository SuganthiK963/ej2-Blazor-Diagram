using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace Syncfusion.Blazor.Internal
{
    /// <summary>
    /// Custom JSON serialization type converter for the DataSource properties.
    /// </summary>
#pragma warning disable CA1812
    internal class DataSourceTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value.GetType() == typeof(string))
            {
                writer.WriteRawValue((string)value);
            }
            else if (value.GetType() == typeof(DataManager))
            {
                var result = value as DataManager;
                var jsonResult = JsonConvert.SerializeObject(value, Formatting.Indented);

                writer.WriteRawValue("new sf.data.DataManager(" + jsonResult + ")");
            }
            else
            {
                writer.WriteRawValue(JsonConvert.SerializeObject(value, Formatting.Indented));
            }
        }
    }

    internal class AsyncHandler
    {
    }
}