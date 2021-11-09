using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.DataVizCommon;
using System.Collections.Generic;

namespace Syncfusion.Blazor.Charts
{
    public partial class SelectionStyleComponent
    {
        [Parameter]
        public string ComponentId { get; set; }

        [Parameter]
#pragma warning disable CA2227 
        public List<PatternOptions> GivenPattern { get; set; } = new List<PatternOptions>();
#pragma warning restore CA2227 
        internal void DrawPattern(List<PatternOptions> data)
        {
            if (data != null)
            {
                GivenPattern = data;
                InvokeAsync(StateHasChanged);
            }
        }
    }
}