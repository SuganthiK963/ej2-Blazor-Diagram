using System;
using System.ComponentModel;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Syncfusion.Blazor.Data
{
    /// <summary>
    /// Handles the datamanager converter.
    /// </summary>
    /// <exclude/>
    public class DataManagerTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            return false;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string strValue)
            {
                if (strValue[0] == '(')
                {
                    strValue = strValue.Substring(1, strValue.Length - 2);
                }

                return JsonConvert.DeserializeObject<DataManagerRequest>(strValue, new JsonSerializerSettings
                {
                    ContractResolver = new CustomContractResolver(),
                });
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is DataManagerRequest model)
            {
                return JsonConvert.SerializeObject(model, new JsonSerializerSettings
                {
                    ContractResolver = new CustomContractResolver(),
                });
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    internal class CustomContractResolver : DefaultContractResolver
    {
        protected override JsonContract CreateContract(Type objectType)
        {
            if (typeof(DataManagerRequest).IsAssignableFrom(objectType))
            {
                return CreateObjectContract(objectType);
            }

            return base.CreateContract(objectType);
        }
    }
}
