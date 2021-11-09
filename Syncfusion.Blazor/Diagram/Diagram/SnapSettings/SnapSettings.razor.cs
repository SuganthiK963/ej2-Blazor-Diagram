using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Diagram.Internal;
using Syncfusion.Blazor.Internal;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Diagram
{
    /// <summary>
    /// Customizes and controls the gridlines and the snap behavior of the diagram.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <SfDiagramComponent Height="500px">
    ///     // Shows the horizontal gridlines
    ///     <SnapSettings Constraints ="SnapConstraints.ShowLines" SnapAngle="10">
    ///         <HorizontalGridLines LineColor = "blue" LineDashArray="2,2">
    ///         </HorizontalGridLines>
    ///         <VerticalGridLines LineColor = "blue" LineDashArray="2,2">
    ///         </VerticalGridLines>
    ///     </SnapSettings>
    /// </SfDiagramComponent>
    /// ]]>
    /// </code>
    /// </example>
    public partial class SnapSettings : SfBaseComponent
    {
        private SnapConstraints constraints;
        private double snapObjectDistance;
        private double snapAngle;
        private GridType gridType;

        internal List<string> PathData = new List<string>();
        internal List<DiagramPoint> DotsData = new List<DiagramPoint>();
        internal bool IsUpdateGridLine;

        [CascadingParameter]
        [JsonIgnore]
        internal SfDiagramComponent Parent { get; set; }

        /// <summary>
        /// Gets or sets the child content of the SnapSettings.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public RenderFragment ChildContent { get; set; }
        /// <summary>
        /// Defines the pattern of vertical gridlines.
        /// </summary>
        [Parameter]
        [JsonPropertyName("verticalGridLines")]
        public VerticalGridLines VerticalGridLines { get; set; }

        /// <summary>
        /// Defines the pattern of horizontal gridlines
        /// </summary>
        [Parameter]
        [JsonPropertyName("horizontalGridLines")]
        public HorizontalGridLines HorizontalGridLines { get; set; }

        /// <summary>
        /// Enables or disables the features of gridlines and SnapSettings.
        /// </summary>
        [Parameter]
        [JsonPropertyName("constraints")]
        public SnapConstraints Constraints { get; set; } = SnapConstraints.All;
        /// <summary>
        /// Specifies the callback to trigger when the constraints changes.
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public EventCallback<SnapConstraints> ConstraintsChanged { get; set; }
        /// <summary>
        /// Defines the minimum distance between the selected object and the nearest object. By default, it is 5.
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <SfDiagramComponent>
        ///     <SnapSettings SnapDistance="10">
        ///     </SnapSettings>
        /// </SfDiagramComponent>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        [JsonPropertyName("snapDistance")]
        public double SnapDistance { get; set; } = 5;
        /// <summary>
        /// Specifies the callback to trigger when the SnapObjectDistance changes.
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public EventCallback<double> SnapDistanceChanged { get; set; }
        /// <summary>
        /// SnapAngle defines the angle by which the object needs to be rotated. By default, 5.
        /// </summary>
        [Parameter]
        [JsonPropertyName("snapAngle")]
        public double SnapAngle { get; set; } = 5;
        /// <summary>
        /// Specifies the callback to trigger when the SnaAngle changes.
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public EventCallback<double> SnapAngleChanged { get; set; }
        /// <summary>
        /// Defines the diagram Grid pattern.
        /// </summary>
        /// <remarks>
        /// The GridType can be set to lines or dots. By default, the GridType is set to lines.
        /// </remarks>
        [Parameter]
        [JsonPropertyName("gridType")]
        public GridType GridType { get; set; }
        /// <summary>
        /// Specifies the callback to trigger when the GridType changes.
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public EventCallback<GridType> GridTypeChanged { get; set; }

        internal static List<double> GetLineIntervals(SnapSettings snapSetings, bool isHorizontal)
        {
            List<double> intervals = new List<double>();
            double[] lineIntervals;
            if (snapSetings.GridType == GridType.Lines)
            {
                lineIntervals = isHorizontal ? snapSetings.HorizontalGridLines.LineIntervals : snapSetings.VerticalGridLines.LineIntervals;
            }
            else
            {
                lineIntervals = isHorizontal ? snapSetings.HorizontalGridLines.DotIntervals : snapSetings.VerticalGridLines.DotIntervals;
            }
            int i;
            for (i = 0; i < lineIntervals.Length; i++)
            {
                intervals.Add(lineIntervals[i]);
            }

            if (snapSetings.GridType == GridType.Dots)
            {
                intervals.Add(lineIntervals[intervals.Count - 2]);
                intervals.Add(lineIntervals[intervals.Count - 2]);
            }
            return intervals;
        }

        internal static double ScaleSnapInterval(SnapSettings snapSettings, double scale)
        {
            if (scale >= 2)
            {
                while (scale >= 2)
                {
                    scale /= 2;
                }
            }
            else if (scale <= 0.5)
            {
                while (scale <= 0.5)
                {
                    scale *= 2;
                }
            }
            int i;
            (snapSettings.HorizontalGridLines).ScaledInterval = snapSettings.HorizontalGridLines.SnapIntervals;
            (snapSettings.VerticalGridLines).ScaledInterval = snapSettings.VerticalGridLines.SnapIntervals;
            if (scale != 1)
            {
                GridLines gridlines = snapSettings.HorizontalGridLines;
                gridlines.ScaledInterval = new double[gridlines.SnapIntervals.Length];
                for (i = 0; i < gridlines.SnapIntervals.Length; i++)
                {
                    gridlines.ScaledInterval[i] = gridlines.SnapIntervals[i] * scale;
                }
                gridlines = snapSettings.VerticalGridLines;
                gridlines.ScaledInterval = new double[gridlines.SnapIntervals.Length];
                for (i = 0; i < gridlines.SnapIntervals.Length; i++)
                {
                    gridlines.ScaledInterval[i] = gridlines.SnapIntervals[i] * scale;
                }
            }
            return scale;
        }

        internal static ObservableCollection<BaseAttributes> GetAttributes(bool horizontalLine, SnapSettings snapSettings, double scale)
        {
            ObservableCollection<BaseAttributes> horizontalLineAttributes;
            if (snapSettings.GridType == GridType.Lines)
            {
                List<double> intervals = GetLineIntervals(snapSettings, horizontalLine);
                double spaceX = 0;
                horizontalLineAttributes = GetHorizontalAttributes(intervals, spaceX, snapSettings.HorizontalGridLines, horizontalLine, snapSettings.VerticalGridLines, GetPatternDimension(snapSettings, snapSettings.GridType == GridType.Lines), scale);
            }
            else
            {
                List<double> horizontalIntervals = GetLineIntervals(snapSettings, horizontalLine);
                List<double> verticalIntervals = GetLineIntervals(snapSettings, !horizontalLine);
                horizontalLineAttributes = GetGridDotAttributes(horizontalLine, snapSettings.HorizontalGridLines, horizontalIntervals, snapSettings.VerticalGridLines, verticalIntervals, scale);

            }
            return horizontalLineAttributes;
        }

        internal static ObservableCollection<BaseAttributes> GetHorizontalAttributes(List<double> intervals, double spaceX, HorizontalGridLines horizontalGridLines, bool isHorizontal, VerticalGridLines verticalGridLines, double snapSettingsWidth, double scale)
        {
            int i;
            ObservableCollection<BaseAttributes> options = new ObservableCollection<BaseAttributes>();
            for (i = 0; i < intervals.Count; i += 2)
            {
                PathAttributes option = new PathAttributes { Opacity = 1, Visible = true };
                string d = ((spaceX + intervals[i] / 2) * scale).ToString(CultureInfo.InvariantCulture);
                option.StrokeWidth = intervals[i];

                if (isHorizontal)
                {
                    option.Data = "M0," + d + " L" + snapSettingsWidth * scale + "," + d + " Z";
                }
                else
                {
                    option.Data = "M" + d + ",0 L" + d + "," + snapSettingsWidth * scale + " Z";
                }

                option.Stroke = isHorizontal ? horizontalGridLines.LineColor : verticalGridLines.LineColor;
                option.Opacity = 1;
                option.ClassValues = intervals[i] == 1.25 ? "e-diagram-thick-grid" : "e-diagram-thin-grid";
                options.Add(option);
                spaceX += intervals[i + 1] + intervals[i];
            }
            return options;
        }

        private static ObservableCollection<BaseAttributes> RenderDotGrid(double d, double spaceY,
            ObservableCollection<BaseAttributes> options, bool horizontalGridLine, string lineColor, List<double> interval, double scale)
        {
            int j;
            for (j = 1; j < interval.Count; j += 2)
            {
                CircleAttributes option = new CircleAttributes { Opacity = 1, Visible = true };
                double radius = (j == interval.Count - 1) ? interval[0] : interval[j - 1];
                double dy = spaceY * scale;
                option.Radius = radius;
                option.CenterX = horizontalGridLine ? dy : d;
                option.CenterY = horizontalGridLine ? d : dy;
                option.Fill = lineColor;
                options.Add(option);
                spaceY += interval[j] + interval[j - 1];
            }
            return options;
        }
        private static double GetSpaceValue(double space, List<double> intervals, int i)
        {
            space = (i - 1 > 0) ? intervals[i - 1] + space : 0;
            return space;
        }
        internal static ObservableCollection<BaseAttributes> GetGridDotAttributes(bool horizontalGridLine, HorizontalGridLines horizontalGridLines, List<double> Intervals, VerticalGridLines verticalGridLines, List<double> interval, double scale)
        {
            ObservableCollection<BaseAttributes> options = new ObservableCollection<BaseAttributes>();
            string dotColor = horizontalGridLine ? horizontalGridLines.LineColor : verticalGridLines.LineColor;
            double space = 0;
            double spaceY = 0;
            int i;
            for (i = 0; i < Intervals.Count; i += 2)
            {
                space = GetSpaceValue(space, Intervals, i);
                double d = space * scale;
                options = RenderDotGrid(d, spaceY, options, horizontalGridLine, dotColor, interval, scale);
                space += Intervals[i];
            }
            return options;
        }



        internal static double GetPatternDimension(SnapSettings snapSettings, bool isHeight)
        {
            double height = 0;
            double width = 0;
            int i;
            double[] horizontalLineIntervals = snapSettings.GridType == GridType.Lines ? snapSettings.HorizontalGridLines.LineIntervals : snapSettings.HorizontalGridLines.DotIntervals;
            for (i = 0; i < horizontalLineIntervals.Length; i++)
            {
                height += horizontalLineIntervals[i];
            };
            double[] verticalLineIntervals = snapSettings.GridType == GridType.Lines ? snapSettings.VerticalGridLines.LineIntervals : snapSettings.VerticalGridLines.DotIntervals;
            for (i = 0; i < verticalLineIntervals.Length; i++)
            {
                width += verticalLineIntervals[i];
            };
            var dimension = isHeight ? height : width;
            return dimension;
        }

        internal static SnapSettings Initialize()
        {
            SnapSettings snapSettings = new SnapSettings();
            snapSettings.HorizontalGridLines ??= new HorizontalGridLines();
            snapSettings.VerticalGridLines ??= new VerticalGridLines();
            return snapSettings;
        }
        internal void UpdateHorizontalGridValues(HorizontalGridLines value)
        {
            HorizontalGridLines = value;
        }
        internal void UpdateVerticalGridValues(VerticalGridLines value)
        {
            VerticalGridLines = value;
        }

        internal void UpdateLineSettings(SnapSettings value)
        {
            HorizontalGridLines = value.HorizontalGridLines ?? new HorizontalGridLines();
            VerticalGridLines = value.VerticalGridLines ?? new VerticalGridLines();
            Parent.UpdateSnapSettings(this);
        }
        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(true);
            UpdateLineSettings(this);
            gridType = GridType;
            constraints = Constraints;
            snapAngle = SnapAngle;
            snapObjectDistance = SnapDistance;
            Parent.UpdateSnapSettings(this);
        }
        /// <summary>
        /// Method invoked when any changes in component state occur.
        /// </summary>
        /// <returns>="Task".</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            gridType = BaseUtil.UpdateDictionary(nameof(GridType), gridType, GridType, PropertyChanges);
            constraints = BaseUtil.UpdateDictionary(nameof(Constraints), constraints, Constraints, PropertyChanges);
            snapObjectDistance = BaseUtil.UpdateDictionary(nameof(SnapDistance), snapObjectDistance, SnapDistance, PropertyChanges);
            snapAngle = BaseUtil.UpdateDictionary(nameof(SnapAngle), snapAngle, SnapAngle, PropertyChanges);
            if (PropertyChanges.Any())
                Parent.UpdateSnapSettings(this);
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
                this.RefreshSnapSettings(PropertyChanges);
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        internal void RefreshSnapSettings(Dictionary<string, object> PropertyChanges, bool? isRefreshSnapsettings = false)
        {
            if (PropertyChanges.Keys.Contains("GridType") || (isRefreshSnapsettings.HasValue && isRefreshSnapsettings.Value))
            {
                this.PathData = new List<string>();
                this.DotsData = new List<DiagramPoint>();
                this.IsUpdateGridLine = true;
            }
            Parent.DiagramStateHasChanged();
        }

        internal async Task PropertyUpdate(SnapSettings snapSettings)
        {
            GridType = await SfBaseUtils.UpdateProperty(snapSettings.GridType, GridType, GridTypeChanged, null, null);
            Constraints = await SfBaseUtils.UpdateProperty(snapSettings.Constraints, Constraints, ConstraintsChanged, null, null);
            SnapDistance = await SfBaseUtils.UpdateProperty(snapSettings.SnapDistance, SnapDistance, SnapDistanceChanged, null, null);
            SnapAngle = await SfBaseUtils.UpdateProperty(snapSettings.SnapAngle, SnapAngle, SnapAngleChanged, null, null);
            if (snapSettings.HorizontalGridLines != null)
                HorizontalGridLines = await HorizontalGridLines.PropertyUpdate(snapSettings.HorizontalGridLines) as HorizontalGridLines;
            else HorizontalGridLines = null;
            if (snapSettings.VerticalGridLines != null)
                VerticalGridLines = await VerticalGridLines.PropertyUpdate(snapSettings.VerticalGridLines) as VerticalGridLines;
            else VerticalGridLines = null;
            gridType = GridType;
            constraints = Constraints;
            snapAngle = SnapAngle;
            snapObjectDistance = SnapDistance;
            if (Parent != null)
            {
                UpdateLineSettings(this);
            }
        }

        internal void GetPathData(ObservableCollection<BaseAttributes> attributes)
        {
            int i;
            for (i = 0; i < attributes.Count; i++)
            {
                BaseAttributes attribute = attributes[i];
                if (attribute is PathAttributes pathAttributes)
                {
                    this.PathData.Add(pathAttributes.Data);
                    pathAttributes.Data = "";
                }
                else if(attribute is CircleAttributes circleAttributes)
                {
                    this.DotsData.Add(new DiagramPoint(circleAttributes.CenterX, circleAttributes.CenterY));
                    circleAttributes.CenterX = 0;
                    circleAttributes.CenterY = 0;
                }
            }
        }

        /// <summary>
        /// This method releasing all unmanaged resources.
        /// </summary>
        public new void Dispose()
        {
            if (Parent != null)
            {
                Parent = null;
            }
            if (ChildContent != null)
            {
                ChildContent = null;
            }
            if (VerticalGridLines != null)
            {
                VerticalGridLines.Dispose();
                VerticalGridLines = null;
            }
            if (HorizontalGridLines != null)
            {
                HorizontalGridLines.Dispose();
                HorizontalGridLines = null;
            }
        }
    }
}