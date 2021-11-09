using Microsoft.AspNetCore.Components;
using System;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the datalabel for the series.
    /// </summary>
    public class ChartDataLabel : ChartSubComponent, IChartElement, ISubcomponentTracker
    {
        private int pendingParametersSetCount;
        private ChartMarker marker;
        private ChartDataLabelRenderer renderer;
        private Type rendererType;

        #region Backing Fields of Datalabel
        private bool visible;
        private string name;
        private string fill = Constants.TRANSPARENT;
        private double opacity = 1;
        private double angle;
        private bool enableRotation;
        private LabelPosition position = LabelPosition.Auto;
        private double rx = 5;
        private double ry = 5;
        private Alignment alignment = Alignment.Center;
        private ChartDataLabelBorder border = new ChartDataLabelBorder();
        private ChartDataLabelMargin margin = new ChartDataLabelMargin();
        private ChartDataLabelFont font = new ChartDataLabelFont();
        private string labelIntersectAction = "Hide";
        #endregion

        string IChartElement.RendererKey { get; set; }

        internal ChartDataLabelRenderer Renderer
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

        #region Datalabel API

        /// <summary>
        /// Enables the visiblity of datalabel.
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
        /// Specifies the datalabel mapping name for the series.
        /// </summary>
        [Parameter]
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                if (name != value)
                {
                    name = value;
                }
            }
        }

        /// <summary>
        /// Specifies the fill color of the datalabel.
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
                    Renderer?.DatalabelValueChanged();
                }
            }
        }

        /// <summary>
        /// Defines the opacity of the datalabel.
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
                    Renderer?.DatalabelValueChanged();
                }
            }
        }

        /// <summary>
        /// Specifies the angle at which label rotation takes place.
        /// </summary>
        [Parameter]
        public double Angle
        {
            get
            {
                return angle;
            }

            set
            {
                if (angle != value)
                {
                    angle = value;
                    Renderer?.DatalabelValueChanged();
                }
            }
        }

        /// <summary>
        /// Enable the rotation of datalabel.
        /// </summary>
        [Parameter]
        public bool EnableRotation
        {
            get
            {
                return enableRotation;
            }

            set
            {
                if (enableRotation != value)
                {
                    enableRotation = value;
                    Renderer?.DatalabelValueChanged();
                }
            }
        }

        /// <summary>
        /// Specifies the label position of the datalabel.
        /// </summary>
        [Parameter]
        public LabelPosition Position
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
                    Renderer?.DatalabelValueChanged();
                }
            }
        }

        /// <summary>
        /// Width of rounded corner in horizontal orientation.
        /// </summary>
        [Parameter]
        public double Rx
        {
            get
            {
                return rx;
            }

            set
            {
                if (rx != value)
                {
                    rx = value;
                    Renderer?.DatalabelValueChanged();
                }
            }
        }

        /// <summary>
        ///  Width of rounded corner in vertical orientation.
        /// </summary>
        [Parameter]
        public double Ry
        {
            get
            {
                return ry;
            }

            set
            {
                if (ry != value)
                {
                    ry = value;
                    Renderer?.DatalabelValueChanged();
                }
            }
        }

        /// <summary>
        /// Specifies the alignment for the datalabels.
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
                    Renderer?.DatalabelValueChanged();
                }
            }
        }

        /// <summary>
        /// Options to customize the border of the datalabel.
        /// </summary>
        [Parameter]
        public ChartDataLabelBorder Border
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
                    if (border != null && border.IsPropertyChanged)
                    {
                        border.IsPropertyChanged = false;
                        Renderer?.DatalabelValueChanged();
                    }
                }
            }
        }

        /// <summary>
        /// Options to customize the margin for the datalabel.
        /// </summary>
        [Parameter]
        public ChartDataLabelMargin Margin
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
                    if (margin != null && margin.IsPropertyChanged)
                    {
                        margin.IsPropertyChanged = false;
                        Renderer?.DatalabelValueChanged();
                    }
                }
            }
        }

        /// <summary>
        /// Options to customize the text style of the datalabel.
        /// </summary>
        [Parameter]
        public ChartDataLabelFont Font
        {
            get
            {
                return font;
            }

            set
            {
                if (font != value)
                {
                    font = value;
                    if (font != null && font.IsPropertyChanged)
                    {
                        font.IsPropertyChanged = false;
                        Renderer?.DatalabelValueChanged();
                    }
                }
            }
        }

        /// <summary>
        /// Specifies the custom HTML element in place of datalabel.
        /// </summary>
        [Parameter]
        public RenderFragment<ChartDataPointInfo> Template { get; set; }

        /// <summary>
        /// Specifies the label intersect action for the datalabel.
        /// </summary>
        [Parameter]
        public string LabelIntersectAction
        {
            get
            {
                return labelIntersectAction;
            }

            set
            {
                if (labelIntersectAction != value)
                {
                    labelIntersectAction = value;
                    Renderer?.DatalabelValueChanged();
                }
            }
        }
        #endregion

        protected override void OnInitialized()
        {
            base.OnInitialized();
            marker = (ChartMarker)Tracker;
            RendererType = typeof(ChartDataLabelRenderer);
            marker.UpdateMarkerProperties("DataLabel", this);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            marker.UpdateMarkerProperties("DataLabel", this);
        }

        internal void UpdateDatalabelProperties(string key, object keyValue)
        {
            switch (key)
            {
                case nameof(Margin):
                    Margin = (ChartDataLabelMargin)keyValue;
                    break;
                case nameof(Border):
                    Border = (ChartDataLabelBorder)keyValue;
                    break;
                case nameof(Font):
                    Font = (ChartDataLabelFont)keyValue;
                    break;
            }
        }

        void ISubcomponentTracker.PushSubcomponent()
        {
            pendingParametersSetCount++;
        }

        void ISubcomponentTracker.PopSubcomponent()
        {
            pendingParametersSetCount--;
            if (pendingParametersSetCount == 0)
            {
                marker.Series.Container.SeriesContainer.Prerender();
            }
        }

        internal void SetDataLableValues(ChartDataLabel dataLabel)
        {
            Visible = dataLabel.Visible;
            Name = dataLabel.Name;
            Fill = dataLabel.Fill;
            Angle = dataLabel.Angle;
            EnableRotation = dataLabel.EnableRotation;
            Position = dataLabel.Position;
            Rx = dataLabel.Rx;
            Ry = dataLabel.Ry;
            Alignment = dataLabel.Alignment;
            Border = dataLabel.Border;
            Margin = dataLabel.Margin;
            Font = dataLabel.Font;
            Template = dataLabel.Template;
            LabelIntersectAction = dataLabel.LabelIntersectAction;
        }

        internal override void ComponentDispose()
        {
            marker = null;
            ChildContent = null;
            Margin = null;
            Border = null;
            Font = null;
        }
    }
}