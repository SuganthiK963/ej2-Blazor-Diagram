using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the multi level labels of the axis.
    /// </summary>
    public class ChartMultiLevelLabels : ChartSubComponent
    {
        [CascadingParameter]
        internal ChartAxis Axis { get; set; }

        internal List<ChartMultiLevelLabel> MultiLevelLabels { get; set; } = new List<ChartMultiLevelLabel>();

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Axis = (ChartAxis)Tracker;
            Axis.UpdateAxisProperties("MultiLevelLabels", MultiLevelLabels);
        }

        internal override void ComponentDispose()
        {
            Axis = null;
            ChildContent = null;
            MultiLevelLabels = null;
        }
    }
}