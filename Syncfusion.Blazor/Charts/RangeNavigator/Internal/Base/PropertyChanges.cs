using System.Linq;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    public partial class SfRangeNavigator : SfDataBoundComponent
    {
        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            allowIntervalData = NotifyPropertyChanges(nameof(AllowIntervalData), AllowIntervalData, allowIntervalData);
            height = Height = NotifyPropertyChanges(nameof(Height), Height, height);
            enableGrouping = NotifyPropertyChanges(nameof(EnableGrouping), EnableGrouping, enableGrouping);
            width = Width = NotifyPropertyChanges(nameof(Width), Width, width);
            enableRtl = NotifyPropertyChanges(nameof(EnableRtl), EnableRtl, enableRtl);
            groupBy = NotifyPropertyChanges(nameof(GroupBy), GroupBy, groupBy);
            interval = NotifyPropertyChanges(nameof(Interval), Interval, interval);
            intervalType = NotifyPropertyChanges(nameof(IntervalType), IntervalType, intervalType);
            labelFormat = NotifyPropertyChanges(nameof(LabelFormat), LabelFormat, labelFormat);
            labelIntersectAction = NotifyPropertyChanges(nameof(LabelIntersectAction), LabelIntersectAction, labelIntersectAction);
            labelPosition = NotifyPropertyChanges(nameof(LabelPosition), LabelPosition, labelPosition);
            query = NotifyPropertyChanges(nameof(Query), Query, query);
            theme = NotifyPropertyChanges(nameof(Theme), Theme, theme);
            useGroupingSeparator = NotifyPropertyChanges(nameof(UseGroupingSeparator), UseGroupingSeparator, useGroupingSeparator);
            valueType = NotifyPropertyChanges(nameof(ValueType), ValueType, valueType);
            xname = NotifyPropertyChanges(nameof(XName), XName, xname);
            yname = NotifyPropertyChanges(nameof(YName), YName, yname);
            animationDuration = NotifyPropertyChanges(nameof(AnimationDuration), AnimationDuration, animationDuration);
            tickPosition = NotifyPropertyChanges(nameof(TickPosition), TickPosition, tickPosition);
            secondaryLabelAlignment = NotifyPropertyChanges(nameof(SecondaryLabelAlignment), SecondaryLabelAlignment, secondaryLabelAlignment);
            logBase = NotifyPropertyChanges(nameof(LogBase), LogBase, logBase);
            minimum = NotifyPropertyChanges(nameof(Minimum), Minimum, minimum);
            maximum = NotifyPropertyChanges(nameof(Maximum), Maximum, maximum);
            dataSource = NotifyPropertyChanges(nameof(DataSource), DataSource, dataSource);
            disableRangeSelector = NotifyPropertyChanges(nameof(DisableRangeSelector), DisableRangeSelector, disableRangeSelector);
            value = NotifyPropertyChanges(nameof(Value), Value, value);
            allowSnapping = NotifyPropertyChanges(nameof(AllowSnapping), AllowSnapping, allowSnapping);
            await OnRangeNaivgatorParametersSet();
        }

        internal async Task OnRangeNaivgatorParametersSet()
        {
            bool renderer = false, refreshBounds = false, refreshRange = false;
            if (IsRendered && PropertyChanges.Any() && ChartContent != null)
            {
                if (IsValueUpdated)
                {
                    PropertyChanges.Remove(nameof(Value));
                }

                AnimateSeries = false;
                foreach (string property in PropertyChanges?.Keys)
                {
                    AnimateSeries = false;
                    switch (property)
                    {
                        case "Width":
                        case "Height":
                            await SetContainerSize();
                            refreshBounds = true;
                            break;
                        case "NavigatorBorder":
                        case "EnableGrouping":
                        case "LabelPosition":
                        case "TickPosition":
                        case "LabelStyle":
                            CalculateAvailableSize();
                            refreshBounds = true;
                            break;
                        case "EnableRtl":
                        case "XName":
                        case "YName":
                        case "Query":
                        case "Minimum":
                        case "Maximum":
                        case "Interval":
                        case "IntervalType":
                        case "LogBase":
                        case "ValueType":
                        case "MajorGridLines":
                        case "MinorGridLines":
                        case "Border":
                        case "Thumb":
                        case "NavigatorStyleSettings":
                        case "LabelFormat":
                        case "Skeleton":
                        case "SkeletonType":
                        case "SecondaryLabelAlignment":
                        case "MajorTickLines":
                            renderer = true;
                            break;
                        case "DataSource":
                        case "Series":
                        case "Margin":
                            renderer = true;
                            refreshBounds = true;
                            break;
                        case "Theme":
                            AnimateSeries = true;
                            break;
                        case "Value":
                            StartValue = double.NaN;
                            EndValue = double.NaN;
                            refreshRange = true;
                            break;
                    }
                }

                if (!refreshBounds && renderer)
                {
                    ChartSeries.XMin = double.PositiveInfinity;
                    ChartSeries.XMax = double.NegativeInfinity;
                    ChartSeries.RenderChart();
                    CreateChart();
                }

                // Issue fix for Range Navigator size gets reduced when the data source is refreshed
                if (refreshBounds && renderer)
                {
                    ChartSeries.XMin = ChartSeries.YMin = double.PositiveInfinity;
                    ChartSeries.XMax = ChartSeries.YMax = double.NegativeInfinity;
                    CalculateVisibleSeries();
                    CalculateBounds();
                    ChartSeries.RenderChart();
                    CreateChart();
                }

                if (refreshBounds && !renderer)
                {
                    CalculateBounds();
                    InstanceInitialization();
                    ChartSeries.RenderChart();
                    CreateChart();
                }

                if (!refreshBounds && !renderer && refreshRange)
                {
                    SetSliderValue();
                    RangeSliderModule.SetSlider(StartValue, EndValue, true, Tooltip.Enable && Tooltip.DisplayMode == TooltipDisplayMode.Always);
                }

                PropertyChanges.Clear();
            }
        }
    }
}