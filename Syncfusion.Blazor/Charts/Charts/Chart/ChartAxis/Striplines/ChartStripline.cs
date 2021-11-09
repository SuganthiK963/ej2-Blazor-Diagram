using Microsoft.AspNetCore.Components;
using System;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the striplines of the axis.
    /// </summary>
    public class ChartStripline : ChartSubComponent, IChartElement
    {
        #region STRIPLINE COMPONENT BACKING FIELDS
        private bool isPropertyChanged;
        private bool isUpdateDirection;
        private string color = "#808080";
        private object start;
        private object end;
        private string dashArray;
        private double size;
        private string text = string.Empty;
        private bool visible = true;
        #endregion

        [CascadingParameter]
        internal ChartStriplines Parent { get; set; }

        /// <summary>
        /// Border of the strip line.
        /// </summary>
        [Parameter]
        public ChartStriplineBorder Border { get; set; } = new ChartStriplineBorder();

        /// <summary>
        /// Color of the strip line.
        /// </summary>
        [Parameter]
        public string Color
        {
            get
            {
                return color;
            }

            set
            {
                if (color != value)
                {
                    color = value;
                    Renderer?.UpdateCustomization(nameof(Color));
                    isPropertyChanged = Parent != null;
                }
            }
        }

        /// <summary>
        /// Dash Array of the strip line.
        /// </summary>
        [Parameter]
        public string DashArray
        {
            get
            {
                return dashArray;
            }

            set
            {
                if (dashArray != value)
                {
                    dashArray = value;
                    Renderer?.UpdateCustomization(nameof(DashArray));
                    isPropertyChanged = Parent != null;
                }
            }
        }

        /// <summary>
        /// End value of the strip line.
        /// </summary>
        [Parameter]
        public object End
        {
            get
            {
                return end;
            }

            set
            {
                if (end == null || !end.Equals(value))
                {
                    end = value;
                    isUpdateDirection = isPropertyChanged = Parent != null;
                }
            }
        }

        /// <summary>
        /// Defines the position of the strip line text horizontally. They are,
        /// Start: Places the strip line text at the start.
        /// Middle: Places the strip line text in the middle.
        /// End: Places the strip line text at the end.
        /// </summary>
        [Parameter]
        public Anchor HorizontalAlignment { get; set; } = Anchor.Middle;

        /// <summary>
        /// IsRepeat value of the strip line.
        /// </summary>
        [Parameter]
        public bool IsRepeat { get; set; }

        /// <summary>
        /// IsSegmented value of the strip line.
        /// </summary>
        [Parameter]
        public bool IsSegmented { get; set; }

        /// <summary>
        /// Strip line Opacity.
        /// </summary>
        [Parameter]
        public double Opacity { get; set; } = 1;

        /// <summary>
        /// repeatEvery value of the strip line.
        /// </summary>
        [Parameter]
        public object RepeatEvery { get; set; }

        /// <summary>
        /// repeatUntil value of the strip line.
        /// </summary>
        [Parameter]
        public object RepeatUntil { get; set; }

        /// <summary>
        /// The angle to which the strip line text gets rotated.
        /// </summary>
        [Parameter]
        public double Rotation { get; set; } = double.NaN;

        /// <summary>
        /// segmentAxisName of the strip line.
        /// </summary>
        [Parameter]
        public string SegmentAxisName { get; set; }

        /// <summary>
        /// segmentEnd value of the strip line.
        /// </summary>
        [Parameter]
        public object SegmentEnd { get; set; }

        /// <summary>
        /// segmentStart value of the strip line.
        /// </summary>
        [Parameter]
        public object SegmentStart { get; set; }

        /// <summary>
        /// Size of the strip line, when it starts from the origin.
        /// </summary>
        [Parameter]
        public double Size
        {
            get
            {
                return size;
            }

            set
            {
                if (size != value)
                {
                    size = value;
                    isUpdateDirection = isPropertyChanged = Parent != null;
                }
            }
        }

        /// <summary>
        /// Size type of the strip line.
        /// </summary>
        [Parameter]
        public SizeType SizeType { get; set; }

        /// <summary>
        /// Start value of the strip line.
        /// </summary>
        [Parameter]
        public object Start
        {
            get
            {
                return start;
            }

            set
            {
                if (start == null || !start.Equals(value))
                {
                    start = value;
                    isUpdateDirection = isPropertyChanged = Parent != null;
                }
            }
        }

        /// <summary>
        ///  If set true, strip line get render from axis origin.
        ///  @default false.
        /// </summary>
        [Parameter]
        public bool StartFromAxis { get; set; }

        /// <summary>
        /// Strip line text.
        /// </summary>
        [Parameter]
        public string Text
        {
            get
            {
                return text;
            }

            set
            {
                if (text != value)
                {
                    text = value;
                    Renderer?.UpdateCustomization(nameof(Text));
                    isPropertyChanged = Parent != null;
                }
            }
        }

        /// <summary>
        /// Options to customize the strip line text.
        /// </summary>
        [Parameter]
        public ChartStriplineTextStyle TextStyle { get; set; } = new ChartStriplineTextStyle();

        /// <summary>
        /// Defines the position of the strip line text vertically. They are,
        ///  Start: Places the strip line text at the start.
        ///  Middle: Places the strip line text in the middle.
        ///  End: Places the strip line text at the end.
        /// </summary>
        [Parameter]
        public Anchor VerticalAlignment { get; set; } = Anchor.Middle;

        /// <summary>
        /// If set true, strip line for axis renders.
        /// </summary>
        [Parameter]
        public bool Visible
        {
            get
            {
                return visible;
            }

            set
            {
                if (visible != value)
                {
                    visible = value;
                    if(Renderer != null)
                    {
                        if(ZIndex == ZIndex.Behind)
                        {
                           Parent.Axis.Container.StriplineBehindContainer.UpdateStriplineCollection();
                        }
                        else
                        {
                           Parent.Axis.Container.StriplineOverContainer.UpdateStriplineCollection();
                        }
                        isPropertyChanged = Parent != null;
                    }
                }
            }
        }

        /// <summary>
        /// Specifies the order of the strip line. They are,
        /// Behind: Places the strip line behind the series elements.
        /// Over: Places the strip line over the series elements.
        /// </summary>
        [Parameter]
        public ZIndex ZIndex { get; set; } = ZIndex.Behind;

        public string RendererKey { get; set; }

        public Type RendererType { get; set; }

        internal ChartStriplineRenderer Renderer { get; set; }

        private Type GetRendererType()
        {
            switch (ZIndex)
            {
                case ZIndex.Behind:
                    return typeof(ChartStriplineBehindRenderer);
                case ZIndex.Over:
                    return typeof(ChartStriplineOverRenderer);
            }

            return null;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Parent = (ChartStriplines)Tracker;
            Parent.Axis.Container.AddStripline(this, ZIndex);
            RendererType = GetRendererType();
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (isPropertyChanged)
            {
                isPropertyChanged = false;
                if (isUpdateDirection)
                {
                    isUpdateDirection = false;
                    Renderer.UpdateDirection();
                }

                Renderer.ProcessRenderQueue();
            }
        }

        internal void SetBorderValues(ChartStriplineBorder border)
        {
            Border = border;
        }

        internal void SetTextStyleValue(ChartStriplineTextStyle textStyle)
        {
            TextStyle = textStyle;
        }

        internal override void ComponentDispose()
        {
            base.ComponentDispose();
            Parent.Axis.Container.RemoveStripline(this, ZIndex);
        }
    }
}