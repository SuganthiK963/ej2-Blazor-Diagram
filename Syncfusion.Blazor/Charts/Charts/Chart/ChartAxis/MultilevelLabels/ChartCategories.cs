using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the categories for the chart labels.
    /// </summary>
    public class ChartCategories : ChartSubComponent
    {
        [CascadingParameter]
        internal ChartMultiLevelLabel MultilevelLabel { get; set; }

        internal SfChart Chart { get; set; }

        internal ChartAxis Axis { get; set; }

        internal List<ChartCategory> Categories { get; set; } = new List<ChartCategory>();

        protected override void OnInitialized()
        {
            base.OnInitialized();
            MultilevelLabel = (ChartMultiLevelLabel)Tracker;
            Axis = MultilevelLabel.MultilevelLabelCollection.Axis;
            Chart = Axis.Container;
            MultilevelLabel.UpdateMultiLevelLabelProperties(nameof(Categories), Categories);
        }

        internal override void ComponentDispose()
        {
            MultilevelLabel = null;
            ChildContent = null;
            Categories = null;
        }
    }
}