using Microsoft.AspNetCore.Components;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Diagram
{
    internal class CrudAction
    {
        [Parameter]
        [JsonPropertyName("read")]
        public string Read { get; set; }
        [Parameter]
        [JsonPropertyName("create")]
        public string Create { get; set; }
        [Parameter]
        [JsonPropertyName("update")]
        public string Update { get; set; }
        [Parameter]
        [JsonPropertyName("destroy")]
        public string Destroy { get; set; }
        [Parameter]
        [JsonPropertyName("customFields")]
        public object[] CustomFields { get; set; }
    }
}
