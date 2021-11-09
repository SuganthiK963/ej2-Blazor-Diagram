using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    public class ChartLegendSettings : ChartSubComponent, ILegendBase
    {
        private ChartLegendRenderer renderer;

        #region Backing private fields
        private bool visible = true;
        private string width;
        private string height;
        private ChartLocation location = new ChartLocation();
        private LegendPosition position = LegendPosition.Auto;
        private double padding = 8;
        private Alignment alignment = Alignment.Center;
        private ChartLegendTextStyle textStyle = new ChartLegendTextStyle();
        private double shapeWidth = 10;
        private double shapeHeight = 10;
        private ChartLegendBorder border = new ChartLegendBorder();
        private ChartLegendMargin margin = new ChartLegendMargin();
        private double shapePadding = 5;
        private string background = Constants.TRANSPARENT;
        private double opacity = 1;
        private bool toggleVisibility = true;
        private string description;
        private double tabIndex = 3;
        #endregion

        internal ChartLegendRenderer Renderer
        {
            get
            {
                return renderer;
            }

            set
            {
                if (renderer != value)
                {
                    renderer = value;
                    renderer?.OnParentParameterSet();
                }
            }
        }

        [CascadingParameter]
        internal SfChart Owner { get; set; }

        #region API

        /// <summary>
        /// Specifies the visibility of legend.
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
                    if (renderer != null)
                    {
                        renderer.RendererShouldRender = true;
                        Owner.OnLayoutChange();
                    }
                }
            }
        }

        /// <summary>
        /// Specifies the width for legend.
        /// </summary>
        [Parameter]
        public string Width
        {
            get
            {
                return width;
            }

            set
            {
                if (width != value)
                {
                    width = value;
                }
            }
        }

        /// <summary>
        /// Specifies the height for legend.
        /// </summary>
        [Parameter]
        public string Height
        {
            get
            {
                return height;
            }

            set
            {
                if (height != value)
                {
                    height = value;
                }
            }
        }

        /// <summary>
        /// Specifies the location of the legend, relative to the chart.
        /// If x is 20, legend moves by 20 pixels to the right of the chart. It requires the `position` to be `Custom`.
        /// </summary>
        [Parameter]
        public ChartLocation Location
        {
            get
            {
                return location;
            }

            set
            {
                if (location != value)
                {
                    location = value;
                }
            }
        }

        /// <summary>
        /// Position of the legend in the chart are,
        ///  Auto: Places the legend based on area type.
        ///  Top: Displays the legend at the top of the chart.
        ///  Left: Displays the legend at the left of the chart.
        ///  Bottom: Displays the legend at the bottom of the chart.
        ///  Right: Displays the legend at the right of the chart.
        ///  Custom: Displays the legend  based on the given x and y values.
        /// </summary>
        [Parameter]
        public LegendPosition Position
        {
            get
            {
                return position;
            }

            set
            {
                if (position != value)
                {
                    position = value;
                    if (renderer != null)
                    {
                        renderer.RendererShouldRender = true;
                        Owner.OnLayoutChange();
                    }
                }
            }
        }

        /// <summary>
        /// Option to customize the padding between legend items.
        /// </summary>
        [Parameter]
        public double Padding
        {
            get
            {
                return padding;
            }

            set
            {
                if (padding != value)
                {
                    padding = value;
                }
            }
        }

        /// <summary>
        /// Legend in chart can be aligned as follows:
        ///  Near: Aligns the legend to the left of the chart.
        ///  Center: Aligns the legend to the center of the chart.
        ///  Far: Aligns the legend to the right of the chart.
        /// </summary>
        [Parameter]
        public Alignment Alignment
        {
            get
            {
                return alignment;
            }

            set
            {
                if (alignment != value)
                {
                    alignment = value;
                }
            }
        }

        /// <summary>
        /// Options to customize the legend text.
        /// </summary>
        [Parameter]
        public ChartLegendTextStyle TextStyle
        {
            get
            {
                return textStyle;
            }

            set
            {
                if (textStyle != value)
                {
                    textStyle = value;
                }
            }
        }

        /// <summary>
        /// Shape width of the legend shape.
        /// </summary>
        [Parameter]
        public double ShapeWidth
        {
            get
            {
                return shapeWidth;
            }

            set
            {
                if (shapeWidth != value)
                {
                    shapeWidth = value;
                }
            }
        }

        /// <summary>
        /// Shape height of the legend shape.
        /// </summary>
        [Parameter]
        public double ShapeHeight
        {
            get
            {
                return shapeHeight;
            }

            set
            {
                if (shapeHeight != value)
                {
                    shapeHeight = value;
                }
            }
        }

        /// <summary>
        /// Options to customize the border of the legend.
        /// </summary>
        [Parameter]
        public ChartLegendBorder Border
        {
            get
            {
                return border;
            }

            set
            {
                if (border != value)
                {
                    border = value;
                }
            }
        }

        /// <summary>
        /// Options to customize left, right, top and bottom margins of the legend.
        /// </summary>
        [Parameter]
        public ChartLegendMargin Margin
        {
            get
            {
                return margin;
            }

            set
            {
                if (margin != value)
                {
                    margin = value;
                }
            }
        }

        /// <summary>
        /// Padding between the legend shape and text.
        /// </summary>
        [Parameter]
        public double ShapePadding
        {
            get
            {
                return shapePadding;
            }

            set
            {
                if (shapePadding != value)
                {
                    shapePadding = value;
                }
            }
        }

        /// <summary>
        /// The background of the chart legend area.
        /// </summary>
        [Parameter]
        public string Background
        {
            get
            {
                return background;
            }

            set
            {
                if (background != value)
                {
                    background = value;
                }
            }
        }

        /// <summary>
        ///  Options to customize left, right, top and bottom margins of the chart.
        /// </summary>
        [Parameter]
        public double Opacity
        {
            get
            {
                return opacity;
            }

            set
            {
                if (opacity != value)
                {
                    opacity = value;
                }
            }
        }

        /// <summary>
        /// If set to true, series' visibility collapses based on the legend visibility.
        /// </summary>
        [Parameter]
        public bool ToggleVisibility
        {
            get
            {
                return toggleVisibility;
            }

            set
            {
                if (toggleVisibility != value)
                {
                    toggleVisibility = value;
                }
            }
        }

        /// <summary>
        /// Description for legends.
        /// </summary>
        [Parameter]
        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                if (description != value)
                {
                    description = value;
                }
            }
        }

        /// <summary>
        /// TabIndex value for the legend.
        /// </summary>
        [Parameter]
        public double TabIndex
        {
            get
            {
                return tabIndex;
            }

            set
            {
                if (tabIndex != value)
                {
                    tabIndex = value;
                }
            }
        }

        /// <summary>
        /// Enables / Disables the inverse rendering of the legend symbol and text.
        /// </summary>
        [Parameter]
        public bool IsInversed { get; set; }
        #endregion

        internal void UpdateLegendProperties(string key, object keyValue)
        {
            switch (key)
            {
                case nameof(Location):
                    Location = (ChartLocation)keyValue;
                    break;
                case nameof(Border):
                    Border = (ChartLegendBorder)keyValue;
                    break;
                case nameof(TextStyle):
                    TextStyle = (ChartLegendTextStyle)keyValue;
                    break;
                case nameof(Margin):
                    Margin = (ChartLegendMargin)keyValue;
                    break;
            }
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (!Visible)
            {
                Owner.LegendRenderer.RemoveFromRenderQueue(Owner.LegendRenderer);
                Owner.LegendRenderer = null;
                return;
            }

            Owner.LegendRenderer.LegendSettings = this;
            Renderer = Owner.LegendRenderer;
            Owner.LegendRenderer.Legend = (ILegendBase)this;
        }

        internal override void ComponentDispose()
        {
            base.ComponentDispose();
            location?.ComponentDispose();
            margin?.ComponentDispose();
            textStyle?.ComponentDispose();
            border?.ComponentDispose();
            Owner = null;
            ChildContent = null;
        }
    }
}
