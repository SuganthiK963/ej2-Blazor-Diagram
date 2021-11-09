using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Internal;
using System.Linq;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization options for stockchart axis stripline.
    /// </summary>
    public class StockChartCommonStripLine : SfBaseComponent
    {
        private string segmentAxisName;
        private object segmentEnd;
        private object segmentStart;
        private double size;
        private SizeType sizeType;
        private object start;
        private bool startFromAxis;
        private string text;
        private Anchor verticalAlignment;
        private bool visible;
        private ZIndex zindex;
        private bool isSegmented;
        private double opacity;
        private object repeatEvery;
        private object repeatUntil;
        private double rotation;
        private string color;
        private string dashArray;
        private double delay;
        private double duration;
        private bool enable;
        private object end;
        private Anchor horizontalAlignment;
        private bool isRepeat;

        [CascadingParameter]
        internal StockChartCommonAxis BaseParent { get; set; }

        internal ChartCommonBorder Border { get; set; } = new ChartCommonBorder();

        internal ChartCommonFont TextStyle { get; set; } = new ChartCommonFont();

        /// <summary>
        /// Color of the strip line.
        /// </summary>
        [Parameter]
        public string Color { get; set; } = "#808080";

        /// <summary>
        /// Dash Array of the strip line.
        /// </summary>
        [Parameter]
        public string DashArray { get; set; }

        /// <summary>
        /// The option to delay animation of the series.
        /// </summary>
        [Parameter]
        public double Delay { get; set; }

        /// <summary>
        /// The duration of animation in milliseconds.
        /// </summary>
        [Parameter]
        public double Duration { get; set; } = 1000;

        /// <summary>
        /// If set to true, series gets animated on initial loading.
        /// </summary>
        [Parameter]
        public bool Enable { get; set; }

        /// <summary>
        /// End value of the strip line.
        /// </summary>
        [Parameter]
        public object End { get; set; }

        /// <summary>
        /// Defines the position of the strip line text horizontally. They are,
        /// Start: Places the strip line text at the start.
        /// Middle: Places the strip line text in the middle.
        /// End: Places the strip line text at the end.
        /// </summary>
        [Parameter]
        public Anchor HorizontalAlignment { get; set; } = Anchor.Middle;

        /// <summary>
        /// isRepeat value of the strip line.
        /// </summary>
        [Parameter]
        public bool IsRepeat { get; set; }

        /// <summary>
        /// isSegmented value of the strip line.
        /// </summary>
        [Parameter]
        public bool IsSegmented { get; set; }

        /// <summary>
        /// Strip line Opacity.
        /// </summary>
        [Parameter]
        public double Opacity { get; set; } = 1;

        /// <summary>
        /// RepeatEvery value of the strip line.
        /// </summary>
        [Parameter]
        public object RepeatEvery { get; set; }

        /// <summary>
        /// RepeatUntil value of the strip line.
        /// </summary>
        [Parameter]
        public object RepeatUntil { get; set; }

        /// <summary>
        /// The angle to which the strip line text gets rotated.
        /// </summary>
        [Parameter]
        public double Rotation { get; set; }

        /// <summary>
        /// SegmentAxisName of the strip line.
        /// </summary>
        [Parameter]
        public string SegmentAxisName { get; set; }

        /// <summary>
        /// SegmentEnd value of the strip line.
        /// </summary>
        [Parameter]
        public object SegmentEnd { get; set; }

        /// <summary>
        /// SegmentStart value of the strip line.
        /// </summary>
        [Parameter]
        public object SegmentStart { get; set; }

        /// <summary>
        /// Size of the strip line, when it starts from the origin.
        /// </summary>
        [Parameter]
        public double Size { get; set; }

        /// <summary>
        /// Size type of the strip line.
        /// </summary>
        [Parameter]
        public SizeType SizeType { get; set; } = SizeType.Auto;

        /// <summary>
        /// Start value of the strip line.
        /// </summary>
        [Parameter]
        public object Start { get; set; }

        /// <summary>
        ///  If set true, strip line get render from axis origin.
        /// </summary>
        [Parameter]
        public bool StartFromAxis { get; set; }

        /// <summary>
        /// Strip line text.
        /// </summary>
        [Parameter]
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Defines the position of the strip line text vertically. They are,
        /// Start: Places the strip line text at the start.
        /// Middle: Places the strip line text in the middle.
        /// End: Places the strip line text at the end.
        /// </summary>
        [Parameter]
        public Anchor VerticalAlignment { get; set; } = Anchor.Middle;

        /// <summary>
        /// If set true, strip line for axis renders.
        /// </summary>
        [Parameter]
        public bool Visible { get; set; } = true;

        /// <summary>
        /// Specifies the order of the strip line. They are,
        /// Behind: Places the strip line behind the series elements.
        /// Over: Places the strip line over the series elements.
        /// </summary>
        [Parameter]
        public ZIndex ZIndex { get; set; } = ZIndex.Behind;

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            color = NotifyPropertyChanges(nameof(Color), Color, color);
            dashArray = NotifyPropertyChanges(nameof(DashArray), DashArray, dashArray);
            delay = NotifyPropertyChanges(nameof(Delay), Delay, delay);
            duration = NotifyPropertyChanges(nameof(Duration), Duration, duration);
            enable = NotifyPropertyChanges(nameof(Enable), Enable, enable);
            end = NotifyPropertyChanges(nameof(End), End, end);
            horizontalAlignment = NotifyPropertyChanges(nameof(HorizontalAlignment), HorizontalAlignment, horizontalAlignment);
            isRepeat = NotifyPropertyChanges(nameof(IsRepeat), IsRepeat, isRepeat);
            isSegmented = NotifyPropertyChanges(nameof(IsSegmented), IsSegmented, isSegmented);
            opacity = NotifyPropertyChanges(nameof(Opacity), Opacity, opacity);
            repeatEvery = NotifyPropertyChanges(nameof(RepeatEvery), RepeatEvery, repeatEvery);
            repeatUntil = NotifyPropertyChanges(nameof(RepeatUntil), RepeatUntil, repeatUntil);
            rotation = NotifyPropertyChanges(nameof(Rotation), Rotation, rotation);
            segmentAxisName = NotifyPropertyChanges(nameof(SegmentAxisName), SegmentAxisName, segmentAxisName);
            segmentEnd = NotifyPropertyChanges(nameof(SegmentEnd), SegmentEnd, segmentEnd);
            segmentStart = NotifyPropertyChanges(nameof(SegmentStart), SegmentStart, segmentStart);
            size = NotifyPropertyChanges(nameof(Size), Size, size);
            sizeType = NotifyPropertyChanges(nameof(SizeType), SizeType, sizeType);
            start = NotifyPropertyChanges(nameof(Start), Start, start);
            startFromAxis = NotifyPropertyChanges(nameof(StartFromAxis), StartFromAxis, startFromAxis);
            text = NotifyPropertyChanges(nameof(Text), Text, text);
            verticalAlignment = NotifyPropertyChanges(nameof(VerticalAlignment), VerticalAlignment, verticalAlignment);
            visible = NotifyPropertyChanges(nameof(Visible), Visible, visible);
            zindex = NotifyPropertyChanges(nameof(ZIndex), ZIndex, zindex);
            if (PropertyChanges.Any() && IsRendered)
            {
                PropertyChanges.Clear();
                BaseParent.StockChartInstance.OnStockChartPropertyChanged();
            }
        }

        internal override void ComponentDispose()
        {
            BaseParent = null;
            Border = null;
            SegmentEnd = segmentEnd = null;
            SegmentStart = segmentStart = null;
            Start = start = null;
            End = end = null;
            RepeatEvery = repeatEvery = null;
            RepeatUntil = repeatUntil = null;
            TextStyle = null;
        }
    }
}