using Microsoft.AspNetCore.Components;
using System;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Specifies the configuration of the error bar settings.
    /// </summary>
    public partial class ChartErrorBarSettings : ChartSubComponent, IChartElement
    {
        private bool refreshErrorBar;

        private ChartErrorBarRenderer renderer;

        private Type rendererType;

        #region ERRORBAR PRIVATE FIELDS

        private ErrorBarDirection direction = ErrorBarDirection.Both;
        private string color;
        private ChartErrorBarCapSettings errorBarCap = new ChartErrorBarCapSettings();
        private double horizontalError = 1;
        private double horizontalNegativeError = 1;
        private double horizontalPositiveError = 1;
        private ErrorBarMode mode = ErrorBarMode.Vertical;
        private ErrorBarType type = ErrorBarType.Fixed;
        private double verticalError = 1;
        private double verticalNegativeError = 3;
        private double verticalPositiveError = 3;
        private bool visible;
        private double width = 1;
        #endregion

        string IChartElement.RendererKey { get; set; }

        [CascadingParameter]
        private ChartSeries Series { get; set; }

        internal ChartErrorBarRenderer Renderer
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

        #region ERRORBAR API

        /// <summary>
        /// The color for stroke of the error bar, which accepts value in hex, rgba as a valid CSS color string.
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
                    if (Renderer != null)
                    {
                        Renderer?.UpdateCustomization(nameof(Color));
                    }
                }
            }
        }

        /// <summary>
        /// The direction of the error bar . They are
        /// both - Renders both direction of error bar.
        /// minus - Renders minus direction of error bar.
        /// plus - Renders plus direction error bar.
        /// </summary>
        [Parameter]
        public ErrorBarDirection Direction
        {
            get
            {
                return direction;
            }

            set
            {
                if (direction != value)
                {
                    direction = value;
                    if (Renderer != null)
                    {
                        refreshErrorBar = true;
                    }
                }
            }
        }

        /// <summary>
        /// Options for customizing the cap of the error bar.
        /// </summary>
        [Parameter]
        public ChartErrorBarCapSettings ErrorBarCap
        {
            get
            {
                return errorBarCap;
            }

            set
            {
                if (errorBarCap != value)
                {
                    errorBarCap = value;
                }
            }
        }

        /// <summary>
        /// Denotes the horizontal error of the error bar.
        /// </summary>
        [Parameter]
        public double HorizontalError
        {
            get
            {
                return horizontalError;
            }

            set
            {
                if (horizontalError != value)
                {
                    horizontalError = value;
                    if (Renderer != null)
                    {
                        refreshErrorBar = true;
                    }
                }
            }
        }

        /// <summary>
        /// Denotes the horizontal negative error of the error bar.
        /// </summary>
        [Parameter]
        public double HorizontalNegativeError
        {
            get
            {
                return horizontalNegativeError;
            }

            set
            {
                if (horizontalNegativeError != value)
                {
                    horizontalNegativeError = value;
                    if (Renderer != null)
                    {
                        refreshErrorBar = true;
                    }
                }
            }
        }

        /// <summary>
        /// Denotes the horizontal positive error of the error bar.
        /// </summary>
        [Parameter]
        public double HorizontalPositiveError
        {
            get
            {
                return horizontalPositiveError;
            }

            set
            {
                if (horizontalPositiveError != value)
                {
                    horizontalPositiveError = value;
                    if (Renderer != null)
                    {
                        refreshErrorBar = true;
                    }
                }
            }
        }

        /// <summary>
        /// The mode of the error bar . They are
        /// Vertical - Renders a vertical error bar.
        /// Horizontal - Renders a horizontal error bar.
        /// Both - Renders both side error bar.
        /// </summary>
        [Parameter]
        public ErrorBarMode Mode
        {
            get
            {
                return mode;
            }

            set
            {
                if (mode != value)
                {
                    mode = value;
                    if (Renderer != null)
                    {
                        refreshErrorBar = true;
                    }
                }
            }
        }

        /// <summary>
        /// The type of the error bar . They are
        /// Fixed - Renders a fixed type error bar.
        /// Percentage - Renders a percentage type error bar.
        /// StandardDeviation - Renders a standard deviation type error bar.
        /// StandardError -Renders a standard error type error bar.
        /// Custom -Renders a custom type error bar.
        /// </summary>
        [Parameter]
        public ErrorBarType Type
        {
            get
            {
                return type;
            }

            set
            {
                if (type != value)
                {
                    type = value;
                    if (Renderer != null)
                    {
                        refreshErrorBar = true;
                    }
                }
            }
        }

        /// <summary>
        /// Denotes the vertical error of the error bar.
        /// </summary>
        [Parameter]
        public double VerticalError
        {
            get
            {
                return verticalError;
            }

            set
            {
                if (verticalError != value)
                {
                    verticalError = value;
                    if (Renderer != null)
                    {
                        refreshErrorBar = true;
                    }
                }
            }
        }

        /// <summary>
        /// Denotes the vertical negative error of the error bar.
        /// </summary>
        [Parameter]
        public double VerticalNegativeError
        {
            get
            {
                return verticalNegativeError;
            }

            set
            {
                if (verticalNegativeError != value)
                {
                    verticalNegativeError = value;
                    if (Renderer != null)
                    {
                        refreshErrorBar = true;
                    }
                }
            }
        }

        /// <summary>
        /// Denotes the vertical positive error of the error bar.
        /// </summary>
        [Parameter]
        public double VerticalPositiveError
        {
            get
            {
                return verticalPositiveError;
            }

            set
            {
                if (verticalPositiveError != value)
                {
                    verticalPositiveError = value;
                    if (Renderer != null)
                    {
                        refreshErrorBar = true;
                    }
                }
            }
        }

        /// <summary>
        /// If set true, error bar for data gets rendered.
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
                }
            }
        }

        /// <summary>
        /// The stroke width of the error bar..
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
                    if (Renderer != null)
                    {
                        Renderer?.UpdateCustomization(nameof(Width));
                    }
                }
            }
        }
        #endregion

        internal void UpdateErrorBarProperty(string key, object keyValue)
        {
            if (key == nameof(ErrorBarCap))
            {
                ErrorBarCap = errorBarCap = (ChartErrorBarCapSettings)keyValue;
            }
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            rendererType = typeof(ChartErrorBarRenderer);
            Series = (ChartSeries)Tracker;
            Series.UpdateSeriesProperties("ErrorBar", this);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (refreshErrorBar)
            {
                refreshErrorBar = false;
                Renderer.UpdateErrorBar();
            }

            Series.UpdateSeriesProperties("ErrorBar", this);
        }

        internal override void ComponentDispose()
        {
            Series = null;
            ChildContent = null;
            ErrorBarCap = null;
        }
    }
}