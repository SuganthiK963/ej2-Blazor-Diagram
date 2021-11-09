using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Strip lines are used to shade the different ranges in plot area in different colors to improve the readability of the chart.
    /// You can annotate it with text to indicate what that particular region indicates.
    /// </summary>
    public class ChartStriplines : ChartSubComponent, ISubcomponentTracker
    {
        private int pendingParametersSetCount;

        [CascadingParameter]
        internal ChartAxis Axis { get; set; }

        internal List<ChartStripline> StripLines { get; set; } = new List<ChartStripline>();

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Axis = (ChartAxis)Tracker;
        }

        void ISubcomponentTracker.PushSubcomponent()
        {
            pendingParametersSetCount++;
        }

        void ISubcomponentTracker.PopSubcomponent()
        {
            pendingParametersSetCount--;
            if (pendingParametersSetCount == 0)
            {
                Axis.Container.StriplineBehindContainer.Prerender();
                Axis.Container.StriplineOverContainer.Prerender();
            }
        }
    }
}