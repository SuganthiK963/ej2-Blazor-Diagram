using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Syncfusion.Blazor.Inputs
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DropEffect
    {
        [EnumMember(Value = "Copy")]
        Copy,
        [EnumMember(Value = "Move")]
        Move,
        [EnumMember(Value = "Link")]
        Link,
        [EnumMember(Value = "None")]
        None,
        [EnumMember(Value = "Default")]
        Default,
    }
}