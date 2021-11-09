using Microsoft.AspNetCore.Components;
using System;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Specifies the marker configuration of the chart.
    /// </summary>
    public class ChartCommonMarker : ChartSubComponent, IChartElement
    {
        #region MARKER PRIVATE FIELDS
        private bool visible;
        private ChartShape shape = ChartShape.Circle;
        private string imageUrl = string.Empty;
        private double height = 5;
        private double width = 5;
        private ChartMarkerBorder border = new ChartMarkerBorder();
        private ChartMarkerOffset offset = new ChartMarkerOffset();
        private string fill = string.Empty;
        private double opacity = 1;
        private ChartDataLabel dataLabel = new ChartDataLabel();
        #endregion

        private ChartMarkerRenderer renderer;
        private Type rendererType;

        string IChartElement.RendererKey { get; set; }

        internal ChartMarkerRenderer Renderer
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

        public Type RendererType
        {
            get
            {
                return rendererType;
            }

            set
            {
                rendererType = value;
            }
        }

        #region MARKER API

        /// <summary>
        /// Enables the visibility of marker for the series.
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
                    renderer?.ToggleVisibility();
                }
            }
        }

        /// <summary>
        /// Defines the shape of the marker.
        /// </summary>
        [Parameter]
        public ChartShape Shape
        {
            get
            {
                return shape;
            }

            set
            {
                if (shape != value)
                {
                    shape = value;
                    Renderer?.UpdateDirection();
                }
            }
        }

        /// <summary>
        /// Specifies the url path for the image.
        /// </summary>
        [Parameter]
        public string ImageUrl
        {
            get
            {
                return imageUrl;
            }

            set
            {
                if (imageUrl != value)
                {
                    imageUrl = value;
                    Renderer?.UpdateDirection();
                }
            }
        }

        /// <summary>
        /// Specifies the height of the marker.
        /// </summary>
        [Parameter]
        public double Height
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
                    Renderer?.UpdateDirection();
                }
            }
        }

        /// <summary>
        /// Specifies the width of the marker.
        /// </summary>
        [Parameter]
        public double Width
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
                    Renderer?.UpdateDirection();
                }
            }
        }

        /// <summary>
        /// Oprions to customize the border of the marker.
        /// </summary>
        [Parameter]
        public ChartMarkerBorder Border
        {
            get
            {
                return border;
            }

            set
            {
                border = value;
                if (border != null && border.IsPropertyChanged)
                {
                    renderer?.UpdateMarkerBorderWidth();
                    Renderer?.UpdateCustomization("Color");
                    border.IsPropertyChanged = false;
                }
            }
        }

        /// <summary>
        /// Options to customize the offset of the marker.
        /// </summary>
        [Parameter]
        public ChartMarkerOffset Offset
        {
            get
            {
                return offset;
            }

            set
            {
                offset = value;
                if (offset != null && offset.IsPropertyChanged)
                {
                    renderer?.UpdateDirection();
                    offset.IsPropertyChanged = false;
                }
            }
        }

        /// <summary>
        /// Specifies the fill color of the marker.
        /// </summary>
        [Parameter]
        public string Fill
        {
            get
            {
                return fill;
            }

            set
            {
                if (fill != value)
                {
                    fill = value;
                    Renderer?.UpdateCustomization("Fill");
                }
            }
        }

        /// <summary>
        /// Specifies the opacity of the marker shape.
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
                    Renderer?.UpdateCustomization("Opacity");
                }
            }
        }

        /// <summary>
        /// Options to customize the datalabel for the series.
        /// </summary>
        [Parameter]
        public ChartDataLabel DataLabel
        {
            get
            {
                return dataLabel;
            }

            set
            {
                if (dataLabel != value)
                {
                    dataLabel = value;
                }
            }
        }

#endregion
        internal void UpdateMarkerProperties(string key, object keyValue)
        {
            switch (key)
            {
                case nameof(Border):
                    Border = (ChartMarkerBorder)keyValue;
                    break;
                case nameof(Offset):
                    Offset = (ChartMarkerOffset)keyValue;
                    break;
                case nameof(DataLabel):
                    DataLabel = (ChartDataLabel)keyValue;
                    break;
            }
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            rendererType = typeof(ChartMarkerRenderer);
        }

        internal override void ComponentDispose()
        {
            DataLabel = null;
            Border = null;
            Offset = null;
        }

        internal void SetMarkerValues(ChartCommonMarker marker)
        {
            Visible = marker.Visible;
            Shape = marker.Shape;
            ImageUrl = marker.ImageUrl;
            Height = marker.Height;
            Width = marker.Width;
            Border = new ChartMarkerBorder();
            Border.SetBorderValues(marker.Border.Color, marker.Border.Width);
            Offset = new ChartMarkerOffset();
            Offset.SetOffsetValues(marker.Offset.X, marker.Offset.Y);
            Fill = fill;
            Opacity = opacity;
        }
    }
}