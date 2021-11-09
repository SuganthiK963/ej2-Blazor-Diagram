using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the range of scroll bar range.
    /// </summary>
    public partial class ChartAxisScrollbarSettingsRange : ChartCommonScrollbarSettingsRange
    {
        [CascadingParameter]
        private ChartAxisScrollbarSettings ScrollBar { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            ScrollBar.UpdateRange(this);
        }

        internal override void ComponentDispose()
        {
            base.ComponentDispose();
            ChildContent = null;
        }

        internal void SetMinMax(string min, string max)
        {
            Minimum = min;
            Maximum = max;
        }
    }
}