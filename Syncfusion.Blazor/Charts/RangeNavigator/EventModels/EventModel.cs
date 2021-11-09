using System.Collections.Generic;
using System.ComponentModel;
using Syncfusion.Blazor.Charts.Chart.Models;
using Syncfusion.Blazor.Charts.RangeNavigator.Internal;
using Syncfusion.Blazor.DataVizCommon;
using Syncfusion.Blazor.Navigations;
using System;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Specifies the event arguments which has common for range navigator component.
    /// </summary>
    /// <exclude />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class RangeNavigatorEventArgs
    {
        /// <summary>
        /// Defines the event cancel status.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Defines the name of the event.
        /// </summary>
        [Obsolete("This property is deprecated and will no longer be used.")]
        public string Name
        {
            get
            {
                return EventName;
            }
        }

        internal string EventName { get; set; }
    }

    /// <summary>
    /// Specifies the event arguments available for on loaded events in the range navigator component.
    /// </summary>
    public class RangeLoadedEventArgs : RangeNavigatorEventArgs
    {
        /// <summary>
        /// Defines the theme of the range navigator.
        /// </summary>
        public Theme Theme { get; set; }

        /// <summary>
        /// Defines the range navigator chart instance.
        /// </summary>
        [Obsolete("This property is deprecated and will no longer be used.")]
        public SfRangeNavigator RangeNavigator
        {
            get
            {
                return Navigator;
            }

            set
            {
                Navigator = value;
            }
        }

        internal SfRangeNavigator Navigator { get; set; }
    }

    /// <summary>
    /// Specifies the event arguments available for resize event of the range navigator component.
    /// </summary>
    public class RangeResizeEventArgs : RangeNavigatorEventArgs
    {
        /// <summary>
        /// Defines the range navigator chart instance.
        /// </summary>
        [Obsolete("This property is deprecated and will no longer be used.")]
        public SfRangeNavigator RangeNavigator { get; set; }

        /// <summary>
        /// Defines the current size of the range navigator.
        /// </summary>
        public Size CurrentSize { get; set; }

        /// <summary>
        /// Defines the previous size of the range navigator.
        /// </summary>
        public Size PreviousSize { get; set; }
    }

    /// <summary>
    /// Specifies the event arguments available for selector event of the range navigator component.
    /// </summary>
    public class RangeSelectorRenderEventArgs : RangeNavigatorEventArgs
    {
        /// <summary>
        /// Defines selector collections.
        /// </summary>
        public List<ToolbarItem> Selector { get; set; }

        /// <summary>
        /// enable custom format for calendar.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This argument is deprecated and will no longer be used.")]
        public bool EnableCustomFormat { get; set; }

        /// <summary>
        /// content fro calendar format.
        /// </summary>
        public string Content { get; set; }
    }

    /// <summary>
    /// Specifies the event arguments available for the changed events in the range navigator component.
    /// </summary>
    public class ChangedEventArgs : RangeNavigatorEventArgs
    {
        /// <summary>
        /// Defines the start value.
        /// </summary>
        public object Start { get; set; }

        /// <summary>
        /// Defines the end value.
        /// </summary>
        public object End { get; set; }

        /// <summary>
        /// Defines the selected data.
        /// </summary>
        public List<DataPoint> SelectedData { get; set; }

        /// <summary>
        /// Defined the zoomPosition of the range navigator.
        /// </summary>
        public double ZoomPosition { get; set; }

        /// <summary>
        /// Defined the zoomFactor of the range navigator.
        /// </summary>
        public double ZoomFactor { get; set; }
    }

    /// <summary>
    /// Specifies the event arguments available for the tooltip render events in the range navigator component.
    /// </summary>
    public class RangeTooltipRenderEventArgs : RangeNavigatorEventArgs
    {
        /// <summary>
        /// Defines tooltip text collections.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Defines tooltip text style.
        /// </summary>
        public ChartCommonFont TextStyle { get; set; }
    }

    /// <summary>
    /// Specifies the event arguments available for the label render events in the range navigator component.
    /// </summary>
    public class RangeLabelRenderEventArgs : RangeNavigatorEventArgs
    {
        /// <summary>
        /// Defines label text collections.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Defines label style for labels.
        /// </summary>
        public ChartCommonFont LabelStyle { get; set; }

        /// <summary>
        /// Defines region for labels.
        /// </summary>
        public Rect Region { get; set; }

        /// <summary>
        /// Defines the value for label.
        /// </summary>
        public double Value { get; set; }
    }
}