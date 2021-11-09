using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Internal;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;

namespace Syncfusion.Blazor.Charts
{
    public class ChartTooltipInfo : ChartDataPointInfo
    {
        public ChartTooltipInfo()
        {
        }
    }

    public class ChartDataPointInfo : PointInfo
    {
        public ChartDataPointInfo()
        {
        }

        public object Close { get; set; }

        public object High { get; set; }

        public object Low { get; set; }

        public object Open { get; set; }

        public string Text { get; set; }

        public object Volume { get; set; }

        public object X { get; set; }

        public object Y { get; set; }
    }

    /// <summary>
    /// TODO: Use this class from accumulation chart.
    /// </summary>
    /// <exclude />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class BaseEventArgs
    {
        /// <summary>
        /// Defines the event cancel status.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Defines the name of the event.
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// Specifies the event arguments available on chart point render.
    /// </summary>
    public class PointRenderEventArgs : BaseEventArgs
    {
        internal PointRenderEventArgs(string name, bool cancel, Point point, ChartSeries series, string fill, ChartEventBorder border, double width = 0, double height = 0, ChartShape shape = ChartShape.Circle)
        {
            Name = name;
            Cancel = cancel;
            Point = point;
            Series = series;
            Shape = shape;
            Fill = fill;
            Border = border;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Defines the current point border.
        /// </summary>
        public ChartEventBorder Border { get; set; }

        /// <summary>
        /// Defines the current point fill color.
        /// </summary>
        public string Fill { get; set; }

        /// <summary>
        /// Defines the current point height.
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Defines the current point.
        /// </summary>
        public Point Point { get; private set; }

        /// <summary>
        /// Defines the current series of the point.
        /// </summary>
        public ChartSeries Series { get; private set; }

        /// <summary>
        /// Defines the current point marker shape.
        /// </summary>
        public ChartShape Shape { get; set; }

        /// <summary>
        /// Defines the current point width.
        /// </summary>
        public double Width { get; set; }
    }

    /// <summary>
    /// Specifies the event arguments available on chart series render.
    /// </summary>
    public class SeriesRenderEventArgs : BaseEventArgs
    {
        internal SeriesRenderEventArgs(string name, bool cancel, string fill, IEnumerable<object> data, ChartSeries chartSeries)
        {
            Name = name;
            Cancel = cancel;
            Fill = fill;
            Data = data;
            Series = chartSeries;
        }

        /// <summary>
        /// Defines the current series fill color.
        /// </summary>
        public string Fill { get; set; }

        /// <summary>
        /// Defines the current series data.
        /// </summary>
        public IEnumerable<object> Data { get; set; }

        /// <summary>
        /// Defines the current series.
        /// </summary>
        public ChartSeries Series { get; private set; }
    }

    /// <summary>
    /// Specifies the event arguments available for resize event of the chart component.
    /// </summary>
    public class ResizeEventArgs : BaseEventArgs
    {
        internal ResizeEventArgs(string name, bool cancel, SfChart chart, Size current, Size previous)
        {
            Name = name;
            Cancel = cancel;
            Chart = chart;
            CurrentSize = current;
            PreviousSize = previous;
        }

        /// <summary>
        /// Defines the chart.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
        public SfChart Chart { get; private set; }

        /// <summary>
        /// Defines the current size of the  chart.
        /// </summary>
        public Size CurrentSize { get; private set; }

        /// <summary>
        /// Defines the previous size of the  chart.
        /// </summary>
        public Size PreviousSize { get; private set; }
    }

    /// <summary>
    /// Specifies the event arguments available for export event of the chart component.
    /// </summary>
    public class ExportEventArgs : BaseEventArgs
    {
        internal ExportEventArgs(string name, string dataUrl)
        {
            Name = name;
            DataUrl = dataUrl;
        }

        /// <summary>
        /// Defines the DataUrl for export file.
        /// </summary>
        public string DataUrl { get; private set; }
    }

    /// <summary>
    /// Specifies the event arguments available to click on point.
    /// </summary>
    public class PointEventArgs : BaseEventArgs
    {
        internal PointEventArgs(string name, bool cancel, SfChart chart, double pageX, double pageY, Point point, double pointIndex, ChartSeries series, double seriesIndex, double x, double y, bool isRightClick)
        {
            Name = name;
            Cancel = cancel;
            Chart = chart;
            PageX = pageX;
            PageY = pageY;
            Point = point;
            PointIndex = pointIndex;
            Series = series;
            SeriesIndex = seriesIndex;
            X = x;
            Y = y;
            IsRightClick = isRightClick;
        }

        //// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
        public SfChart Chart { get; private set; }

        /// <summary>
        /// Defines current window page x location.
        /// </summary>
        public double PageX { get; private set; }

        /// <summary>
        /// Defines current window page y location.
        /// </summary>
        public double PageY { get; private set; }

        /// <summary>
        /// Defines the current point.
        /// </summary>
        public Point Point { get; private set; }

        /// <summary>
        /// Defines the point index.
        /// </summary>
        public double PointIndex { get; private set; }

        /// <summary>
        /// Defines the current series.
        /// </summary>
        public ChartSeries Series { get; private set; }

        /// <summary>
        /// Defines the series index.
        /// </summary>
        public double SeriesIndex { get; private set; }

        /// <summary>
        /// Defines current mouse x location.
        /// </summary>
        public double X { get; private set; }

        /// <summary>
        /// Defines current mouse y location.
        /// </summary>
        public double Y { get; private set; }

        /// <summary>
        /// Defines the points is clicked by right click or not.
        /// </summary>
        public bool IsRightClick { get; private set; }
    }

    /// <summary>
    /// Defines the information of the chart point.
    /// </summary>
    public class PointInfo
    {
        /// <summary>
        /// Defines the point index.
        /// </summary>
        public double PointIndex { get; set; }

        /// <summary>
        /// Defines the point text.
        /// </summary>
        public string PointText { get; set; }

        /// <summary>
        /// Defines the x value of the point.
        /// </summary>
        public object PointX { get; set; }

        /// <summary>
        /// Defines the y value of the point.
        /// </summary>
        public object PointY { get; set; }

        /// <summary>
        /// Defines the chart series index.
        /// </summary>
        public double SeriesIndex { get; set; }

        /// <summary>
        /// Defines the chart series name.
        /// </summary>
        public string SeriesName { get; set; }
    }

    /// <summary>
    /// Specifies the event arguments available for the tooltip render events in the chart component.
    /// </summary>
    public class TooltipRenderEventArgs : BaseEventArgs
    {
        internal TooltipRenderEventArgs(string name, bool cancel, PointInfo data, string headerText, object point, object series, string template, string text, ChartDefaultFont textStyle)
        {
            Name = name;
            Cancel = cancel;
            Data = data;
            HeaderText = headerText;
            Point = point;
            Series = series;
            Template = template;
            Text = text;
            TextStyle = textStyle;
        }

        /// <summary>
        ///  Defines the point informations.
        /// </summary>
        public PointInfo Data { get; private set; }

        /// <summary>
        /// Defines the header text for the tooltip.
        /// </summary>
        public string HeaderText { get; set; }

        /// <summary>
        /// Defines current tooltip point.
        /// </summary>
        public object Point { get; private set; }

        /// <summary>
        /// Defines current tooltip series.
        /// </summary>
        public object Series { get; private set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
        public string Template { get; private set; }

        /// <summary>
        /// Defines tooltip text collections.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Defines tooltip text style.
        /// </summary>
        public ChartDefaultFont TextStyle { get; set; }
    }

    /// <summary>
    /// Specifies the event arguments available for the legend item click events in the chart component.
    /// </summary>
    public class LegendClickEventArgs : BaseEventArgs
    {
        internal LegendClickEventArgs(string name, bool cancel, SfChart chart, LegendShape legendShape, ChartSeries series, string legendText)
        {
            Name = name;
            Cancel = cancel;
            Chart = chart;
            LegendShape = legendShape;
            Series = series;
            LegendText = legendText;
        }

        /// <summary>
        /// Defines the instance of the chart.
        /// </summary>
        public SfChart Chart { get; private set; }

        /// <summary>
        /// Defines the Legend shape.
        /// </summary>
        public LegendShape LegendShape { get; set; }

        /// <summary>
        /// Defines the current series.
        /// </summary>
        public ChartSeries Series { get; private set; }

        /// <summary>
        /// Defines the current legend text.
        /// </summary>
        public string LegendText { get; private set; }
    }

    /// <summary>
    /// Specifies the event arguments available for the legend render events in the chart component.
    /// </summary>
    public class LegendRenderEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Defines the legend text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Defines the legend color.
        /// </summary>
        public string Fill { get; set; }

        /// <summary>
        /// Defines the legend shape.
        /// </summary>
        public LegendShape Shape { get; set; }

        /// <summary>
        /// Defines the marker text.
        /// </summary>
        public ChartShape MarkerShape { get; set; }
    }

    /// <summary>
    /// Specifies the event arguments available for on axis label render events in the chart component.
    /// </summary>
    public class AxisLabelRenderEventArgs : BaseEventArgs
    {
        internal AxisLabelRenderEventArgs(string name, bool cancel, ChartAxis axis, string text, double value, ChartDefaultFont labelStyle)
        {
            Name = name;
            Cancel = cancel;
            Axis = axis;
            Text = text;
            Value = value;
            LabelStyle = labelStyle;
        }

        /// <summary>
        /// Defines the current axis.
        /// </summary>
        public ChartAxis Axis { get; private set; }

        /// <summary>
        /// Defines axis current label text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Defines axis current label value.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Defines axis current label font style.
        /// </summary>
        public ChartDefaultFont LabelStyle { get; set; }
    }

    /// <summary>
    /// Specifies the event arguments available for on range calculated events in the chart component.
    /// </summary>
    public class AxisRangeCalculatedEventArgs : BaseEventArgs
    {
        internal AxisRangeCalculatedEventArgs(string name, bool cancel, double minimum, double maximum, double interval, Rect bounds, string axisName)
        {
            Name = name;
            Cancel = cancel;
            Minimum = minimum;
            Maximum = maximum;
            Interval = interval;
            Bounds = bounds;
            AxisName = axisName;
        }

        /// <summary>
        /// Defines axis current min range.
        /// </summary>
        public double Minimum { get; set; }

        /// <summary>
        /// Defines axis current max range.
        /// </summary>
        public double Maximum { get; set; }

        /// <summary>
        /// Defines axis current interval.
        /// </summary>
        public double Interval { get; set; }

        /// <summary>
        /// Define current axis bounds.
        /// </summary>
        public Rect Bounds { get; private set; }

        /// <summary>
        /// Defines current axis name.
        /// </summary>
        public string AxisName { get; private set; }


    }

    /// <summary>
    /// Specifies the event arguments available for the text render events in the chart component.
    /// </summary>
    public class TextRenderEventArgs : BaseEventArgs
    {
        internal TextRenderEventArgs(string name, bool cancel, ChartSeries series, Point point, LabelLocation location, string text, string color, BorderModel border, RenderFragment<ChartDataPointInfo> template, ChartDefaultFont font, int seriesIndex)
        {
            Name = name;
            Cancel = cancel;
            Series = series;
            Point = point;
            Location = location;
            Text = text;
            Color = color;
            Border = border;
            Template = template;
            Font = font;
            SeriesIndex = seriesIndex;
        }

        /// <summary>
        /// Defines the current series.
        /// </summary>
        public ChartSeries Series { get; private set; }

        /// <summary>
        /// Defines the text point.
        /// </summary>
        public Point Point { get; private set; }

        /// <summary>
        /// Defines the text location.
        /// </summary>
        public LabelLocation Location { get; private set; }

        /// <summary>
        /// Defines the text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Defines the text color.
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Defines the information of the border.
        /// </summary>
        public BorderModel Border { get; set; }

        /// <summary>
        /// Defines the template.
        /// </summary>
        public RenderFragment<ChartDataPointInfo> Template { get; set; }

        /// <summary>
        /// Defines the information of the font.
        /// </summary>
        public ChartDefaultFont Font { get; set; }

        /// <summary>
        /// Defines the current series index.
        /// </summary>
        public int SeriesIndex { get; private set; }
    }

    /// <summary>
    /// Specifies the event arguments available for the multi level label render events in the chart component.
    /// </summary>
    public class AxisMultiLabelRenderEventArgs : BaseEventArgs
    {
        internal AxisMultiLabelRenderEventArgs(string name, bool cancel, ChartAxis axis, object customAttributes, string text, ChartDefaultFont textStyle, Alignment alignment)
        {
            Name = name;
            Cancel = cancel;
            Axis = axis;
            CustomAttributes = customAttributes;
            Text = text;
            TextStyle = textStyle;
            Alignment = alignment;
        }

        /// <summary>
        /// Defines text alignment for multi labels.
        /// </summary>
        public Alignment Alignment { get; set; }

        /// <summary>
        /// Defines the current axis.
        /// </summary>
        public ChartAxis Axis { get; private set; }

        /// <summary>
        /// Defines custom objects for multi labels.
        /// </summary>
        public object CustomAttributes { get; private set; }

        /// <summary>
        /// Defines axis current label text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Defines font style for multi labels.
        /// </summary>
        public ChartDefaultFont TextStyle { get; set; }
    }

    /// <summary>
    /// Specifies the event arguments available for on scroll changed events in the chart component.
    /// </summary>
    public class ScrollEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Defines the current scroll axis.
        /// </summary>
        public ChartAxis Axis { get; set; }

        /// <summary>
        /// Defines axis current range.
        /// </summary>
        public ChartAxisScrollbarSettingsRange CurrentRange { get; set; }

        /// <summary>
        /// Defines axis previous range.
        /// </summary>
        public ChartAxisScrollbarSettingsRange PreviousAxisRange { get; set; }

        /// <summary>
        /// Defines the previous range.
        /// </summary>
        public VisibleRangeModel PreviousRange { get; set; }

        /// <summary>
        /// Defines the previous Zoom Factor.
        /// </summary>
        public double PreviousZoomFactor { get; set; }

        /// <summary>
        /// Defines the previous Zoom Position.
        /// </summary>
        public double PreviousZoomPosition { get; set; }

        /// <summary>
        /// Defines the current range.
        /// </summary>
        public VisibleRangeModel Range { get; set; }

        /// <summary>
        /// Defines the current Zoom Factor.
        /// </summary>
        public double ZoomFactor { get; set; }

        /// <summary>
        /// Defines the current Zoom Position.
        /// </summary>
        public double ZoomPosition { get; set; }
    }

    /// <summary>
    /// Specifies the event arguments available for the zooming events in the chart component.
    /// </summary>
    public class ZoomingEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Defines the collection of axis.
        /// </summary>
#pragma warning disable CA2227
        public List<AxisData> AxisCollection { get; set; }
    }

    /// <summary>
    /// Specifies the axis information in the chart component.
    /// </summary>
    public class AxisData
    {
        /// <summary>
        /// Defines axis name.
        /// </summary>
        public string AxisName { get; set; }

        /// <summary>
        /// Defines axis range.
        /// </summary>
        public VisibleRangeModel AxisRange { get; set; }

        /// <summary>
        /// Defines the value of the zoom factor.
        /// </summary>
        public double ZoomFactor { get; set; }

        /// <summary>
        /// Defines the value of the zoom position.
        /// </summary>
        public double ZoomPosition { get; set; }
    }

    /// <summary>
    /// Specifies the event arguments available for the shared tooltip render events in the chart component.
    /// </summary>
    public class SharedTooltipRenderEventArgs : BaseEventArgs
    {
        internal SharedTooltipRenderEventArgs(string name, bool cancel, List<string> text, ChartDefaultFont textStyle, string headerText, List<PointInfo> data)
        {
            Name = name;
            Cancel = cancel;
            Text = text;
            TextStyle = textStyle;
            HeaderText = headerText;
            Data = data;
        }

        /// <summary>
        ///  Defines the text.
        /// </summary>
        public List<string> Text { get; set; }

#pragma warning restore CA2227
        /// <summary>
        ///  Defines the text style.
        /// </summary>
        public ChartDefaultFont TextStyle { get; private set; }

        /// <summary>
        ///  Defines the text of the header.
        /// </summary>
        public string HeaderText { get; set; }

        /// <summary>
        ///  Defines the information of the point.
        /// </summary>
        public List<PointInfo> Data { get; private set; }
    }

    /// <summary>
    /// Specifies the event arguments available for OnCrosshairMove event in the chart component.
    /// </summary>
    public class CrosshairMoveEventArgs
    {
        /// <summary>
        /// Defines axis information on crosshair move.
        /// </summary>
        public List<CrosshairAxisInfo> AxisInfo { get; set; } = new List<CrosshairAxisInfo>();
    }

    /// <summary>
    /// Specifies the event arguments available for on selction changed events in the chart component.
    /// </summary>
    public class SelectionCompleteEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Defines current selected Data X, Y values.
        /// </summary>
        public List<PointXY> SelectedDataValues { get; set; }
    }

    /// <summary>
    /// Specifies the event arguments available for the multi level label render events in the chart component.
    /// </summary>
    public class MultiLevelLabelClickEventArgs : BaseEventArgs
    {
        internal MultiLevelLabelClickEventArgs(string name, bool cancel, string text, ChartAxis axis, object customAttributes, object end, double level, object start)
        {
            Name = name;
            Cancel = cancel;
            Text = text;
            Axis = axis;
            CustomAttributes = customAttributes;
            End = end;
            Level = level;
            Start = start;
        }

        /// <summary>
        /// Defines the current axis.
        /// </summary>
        public ChartAxis Axis { get; private set; }

        /// <summary>
        /// Defines custom objects for multi labels.
        /// </summary>
        public object CustomAttributes { get; private set; }

        /// <summary>
        /// Defines end value of the multi level labels.
        /// </summary>
        public object End { get; private set; }

        /// <summary>
        /// Defines current level.
        /// </summary>
        public double Level { get; private set; }

        /// <summary>
        /// Defines start value of the multi level labels.
        /// </summary>
        public object Start { get; private set; }

        /// <summary>
        /// Defines current label text.
        /// </summary>
        public string Text { get; private set; }
    }

    /// <summary>
    /// Specifies the event arguments available for the editing events in the chart component.
    /// </summary>
    public class DataEditingEventArgs : BaseEventArgs
    {
        internal DataEditingEventArgs(string name, double newValue, double oldValue, Point point, double pointIndex, ChartSeries series, double seriesIndex)
        {
            Name = name;
            NewValue = newValue;
            OldValue = oldValue;
            Point = point;
            PointIndex = pointIndex;
            Series = series;
            SeriesIndex = seriesIndex;
        }

        /// <summary>
        /// Defines the current point new value.
        /// </summary>
        public double NewValue { get; private set; }

        /// <summary>
        /// Defines the current point old value.
        /// </summary>
        public double OldValue { get; private set; }

        /// <summary>
        /// Defines the current point.
        /// </summary>
        public Point Point { get; private set; }

        /// <summary>
        /// Defines the current point index.
        /// </summary>
        public double PointIndex { get; private set; }

        /// <summary>
        /// Defines the current chart series.
        /// </summary>
        public ChartSeries Series { get; private set; }

        /// <summary>
        /// Defines the current series index.
        /// </summary>
        public double SeriesIndex { get; private set; }
    }

    /// <summary>
    /// Specifies the event arguments available for on axis label click events in the chart component.
    /// </summary>
    public class AxisLabelClickEventArgs : BaseEventArgs
    {
        internal AxisLabelClickEventArgs(string name, SfChart chart, ChartAxis axis, string text, string labelID, int index, ChartInternalLocation location, double value)
        {
            Name = name;
            Chart = chart;
            Axis = axis;
            Text = text;
            LabelID = labelID;
            Index = index;
            Location = location;
            Value = value;
        }

        /// <summary>
        /// Defines the chart instance when labelClick.
        /// </summary>
        public SfChart Chart { get; private set; }

        /// <summary>
        /// Defines the current axis.
        /// </summary>
        public ChartAxis Axis { get; private set; }

        /// <summary>
        /// Defines axis current label text.
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Defines axis current label element id.
        /// </summary>
        public string LabelID { get; private set; }

        /// <summary>
        /// Defines axis current label index.
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// Defines the current annotation location.
        /// </summary>
        public ChartInternalLocation Location { get; private set; }

        /// <summary>
        /// Defines axis current label value.
        /// </summary>
        public double Value { get; private set; }
    }

    /// <summary>
    /// Specifies the event arguments available for on loaded events in the chart component.
    /// </summary>
    public class LoadedEventArgs : BaseEventArgs
    {
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
        public SfChart Chart { get; set; }
    }

    /// <exclude />
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This class is deprecated and will no longer be used.")]
    public class IAfterExportEventArgs { }

    /// <exclude />
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This class is deprecated and will no longer be used.")]
    public class IAnimationCompleteEventArgs { }

    /// <exclude />
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This class is deprecated and will no longer be used.")]
    public class IExportEventArgs { }

    /// <exclude />
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This class is deprecated and will no longer be used.")]
    public class IDataEditingEventArgs { }

    /// <exclude />
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This class is deprecated and will no longer be used.")]
    public class IDragCompleteEventArgs { }

    /// <exclude />
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This class is deprecated and will no longer be used.")]
    public class ILegendClickEventArgs { }

    /// <exclude />
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This class is deprecated and will no longer be used.")]
    public class ILoadedEventArgs { }

    /// <exclude />
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This class is deprecated and will no longer be used.")]
    public class IMultiLevelLabelClickEventArgs { }

    /// <exclude />
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This class is deprecated and will no longer be used.")]
    public class IResizeEventArgs { }

    /// <exclude />
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This class is deprecated and will no longer be used.")]
    public class IScrollEventArgs { }

    /// <exclude />
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This class is deprecated and will no longer be used.")]
    public class ISelectionCompleteEventArgs { }

}