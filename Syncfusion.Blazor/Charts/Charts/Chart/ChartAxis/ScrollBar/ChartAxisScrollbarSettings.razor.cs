using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the Scrollbar settings of the axis.
    /// </summary>
    public partial class ChartAxisScrollbarSettings : ChartCommonScrollbarSettings
    {
        [CascadingParameter]
        private ChartAxis chartAxis { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            chartAxis = (ChartAxis)Tracker;
            chartAxis.UpdateAxisProperties("ScrollbarSettings", this);
        }

        internal void UpdateRange(object range)
        {
            Range = (ChartAxisScrollbarSettingsRange)range;
        }

        internal override void ComponentDispose()
        {
            base.ComponentDispose();
            chartAxis = null;
            ChildContent = null;
        }
    }
}