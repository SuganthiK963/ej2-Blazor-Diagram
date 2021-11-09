using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Diagram.Internal;
using Syncfusion.Blazor.Internal;
using System;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Diagram
{
    /// <summary>
    /// Represents the visual guidance while dragging or arranging the objects.
    /// </summary>
    /// <example>
    /// <code lang="Razor">
    /// <![CDATA[
    /// <SfDiagramComponent Width = "1000px" Height="500px">
    ///     <SnapSettings Constraints="SnapConstraints.ShowLines">
    ///         <HorizontalGridLines LineColor="blue" LineDashArray="2,2" LineIntervals="@LineInterval">
    ///         </HorizontalGridLines>
    ///         <VerticalGridLines LineColor="blue" LineDashArray="2,2" LineIntervals="@LineInterval">
    ///         </VerticalGridLines>
    ///     </SnapSettings>
    /// </SfDiagramComponent>
    /// @code
    /// {
    ///    //Set the line intervals for the gridlines
    ///    public double[] LineInterval { get; set; } = new double[] 
    ///    {
    ///         1.25, 14, 0.25, 15, 0.25, 15, 0.25, 15, 0.25, 15
    ///    };
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public class GridLines : SfBaseComponent
    {
        private const string LIGHTGRAAY = "lightgray";
        private string lineColor = LIGHTGRAAY;
        private string lineDashArray = string.Empty;
        private double[] lineIntervals = { 1.25, 18.75, 0.25, 19.75, 0.25, 19.75, 0.25, 19.75, 0.25, 19.75 };
        private double[] dotIntervals = { 1, 19, 0.5, 19.5, 0.5, 19.5, 0.5, 19.5, 0.5, 19.5 };
        private double[] snapIntervals = { 20 };

        /// <summary>
        /// Specifies a set of intervals to snap the objects
        /// </summary>
        [JsonIgnore]
        internal double[] ScaledInterval { get; set; } = { 20 };

        [JsonIgnore]
        [CascadingParameter]
        internal SnapSettings Parent { get; set; }

        /// <summary>
        /// Defines the color of the horizontal or vertical gridlines.
        /// </summary>
        [Parameter]
        [JsonPropertyName("lineColor")]
        public string LineColor { get; set; } = LIGHTGRAAY;

        /// <summary>
        /// Defines the pattern of dashes and gaps in the horizontal or vertical gridlines.
        /// </summary>
        [Parameter]
        [JsonPropertyName("lineDashArray")]
        public string LineDashArray { get; set; } = string.Empty;

        /// <summary>
        ///  The thickness and the space between horizontal/vertical gridlines can be customized by using line intervals.
        /// </summary>
        [Parameter]
        [JsonPropertyName("lineIntervals")]
        public double[] LineIntervals { get; set; } = { 1.25, 18.75, 0.25, 19.75, 0.25, 19.75, 0.25, 19.75, 0.25, 19.75 };

        /// <summary>
        /// Represents the pattern of gaps defined by a set of dots.
        /// </summary>
        [Parameter]
        [JsonPropertyName("dotIntervals")]
        public double[] DotIntervals { get; set; } = { 1, 19, 0.5, 19.5, 0.5, 19.5, 0.5, 19.5, 0.5, 19.5 };

        /// <summary>
        /// Specifies a set of intervals for snapping the objects. By default, the objects are snapped towards the nearest grid line.
        /// </summary>
        [Parameter]
        [JsonPropertyName("snapIntervals")]
        public double[] SnapIntervals { get; set; } = { 20 };
        
        /// <summary>
        /// Specifies the callback to trigger when the linecolor changes.
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public EventCallback<string> LineColorChanged { get; set; }
        /// <summary>
        /// Specifies the callback to trigger when the linedasharray changes.
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public EventCallback<string> LineDashArrayChanged { get; set; }
        /// <summary>
        /// Specifies the callback to trigger when the lineintervals changes.
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public EventCallback<double[]> LineIntervalsChanged { get; set; }
        /// <summary>
        /// Specifies the callback to trigger when the snapintervals changes.
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public EventCallback<double[]> SnapIntervalsChanged { get; set; }
        /// <summary>
        /// Specifies the callback to trigger when the dotintervals changes.
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public EventCallback<double[]> DotIntervalsChanged { get; set; }

        /// <summary>
        /// Method invoked when any changes in component state occurs.
        /// </summary>
        /// <returns>="Task".</returns>

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            lineColor = BaseUtil.UpdateDictionary(nameof(LineColor), lineColor, LineColor, PropertyChanges);
            lineDashArray = BaseUtil.UpdateDictionary(nameof(LineDashArray), lineDashArray, LineColor, PropertyChanges);
            lineIntervals = BaseUtil.UpdateDictionary(nameof(LineIntervals), lineIntervals, LineIntervals, PropertyChanges);
            snapIntervals = BaseUtil.UpdateDictionary(nameof(SnapIntervals), snapIntervals, SnapIntervals, PropertyChanges);
            dotIntervals = BaseUtil.UpdateDictionary(nameof(DotIntervals), dotIntervals, DotIntervals, PropertyChanges);
        }
        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender">Set to true for the first time component rendering; otherwise gets false.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (PropertyChanges.Any())
            {
                if (!PropertyChanges.ContainsKey("LineColor"))
                {
                    Parent.RefreshSnapSettings(PropertyChanges, true);
                }
                else
                {
                    Parent.Parent.DiagramStateHasChanged();
                }
            }
            await base.OnAfterRenderAsync(firstRender);
        }
        internal async Task<GridLines> PropertyUpdate(GridLines gridLines)
        {
            LineColor = await SfBaseUtils.UpdateProperty(gridLines.LineColor, LineColor, LineColorChanged);
            LineDashArray = await SfBaseUtils.UpdateProperty(gridLines.LineDashArray, LineDashArray, LineDashArrayChanged);
            LineIntervals = await SfBaseUtils.UpdateProperty(gridLines.LineIntervals, LineIntervals, LineIntervalsChanged);
            SnapIntervals = await SfBaseUtils.UpdateProperty(gridLines.SnapIntervals, SnapIntervals, SnapIntervalsChanged);
            DotIntervals = await SfBaseUtils.UpdateProperty(gridLines.DotIntervals, DotIntervals, DotIntervalsChanged);
            return this;
        }
        /// <summary>
        /// This method releasing all unmanaged resources.
        /// </summary>
        public new void Dispose()
        {
            LineColor = lineColor = null;
            LineDashArray = lineDashArray = null;
            if (lineIntervals != null)
            {
                Array.Clear(lineIntervals, 0, lineIntervals.Length);
                lineIntervals = null;
            }
            if (LineIntervals != null)
            {
                Array.Clear(LineIntervals, 0, LineIntervals.Length);
                LineIntervals = null;
            }
            if (DotIntervals != null)
            {
                Array.Clear(DotIntervals, 0, DotIntervals.Length);
                DotIntervals = null;
            }
            if (dotIntervals != null)
            {
                Array.Clear(dotIntervals, 0, dotIntervals.Length);
                dotIntervals = null;
            }
            if (SnapIntervals != null)
            {
                Array.Clear(SnapIntervals, 0, SnapIntervals.Length);
                SnapIntervals = null;
            }
            if (snapIntervals != null)
            {
                Array.Clear(snapIntervals, 0, snapIntervals.Length);
                snapIntervals = null;
            }
            if (ScaledInterval != null)
            {
                Array.Clear(ScaledInterval, 0, ScaledInterval.Length);
                ScaledInterval = null;
            }
        }
    }
}
