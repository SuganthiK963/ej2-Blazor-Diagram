using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.CircularGauge.Internal;
using System;
using System.Globalization;

namespace Syncfusion.Blazor.CircularGauge
{
    /// <summary>
    /// The circular gauge component is used to visualize the numeric values on the circular scale.
    /// The circular gauge contains labels, ticks, and an axis line to customize its appearance.
    /// </summary>
    public partial class SfCircularGauge : SfBaseComponent
    {
        private string background;
        private string centerX;
        private string centerY;
        private bool enablePointerDrag;
        private bool enableRangeDrag;
        private string height;
        private bool allowMargin;
        private bool moveToCenter;
        private Theme theme;
        private string title;
        private string width;
        private int toggledIndex = -1;
        private List<LegendIndex> toggledIndexes = new List<LegendIndex>();
        private double svgHeight;
        private double svgWidth;
        private CultureInfo culture = CultureInfo.InvariantCulture;
        private bool isInitialRender;
        private bool isPropertyChanged;
        private bool enableGroupingSeparator;

        /// <summary>
        /// Gets or sets the id string for the circular gauge component.
        /// </summary>
        [Parameter]
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to enable the export to image functionality in circular gauge.
        /// </summary>
        [Parameter]
        public bool AllowImageExport { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to enable the export to pdf functionality in circular gauge.
        /// </summary>
        [Parameter]
        public bool AllowPdfExport { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to enable the print functionality in circular gauge.
        /// </summary>
        [Parameter]
        public bool AllowPrint { get; set; }

        /// <summary>
        /// Gets or sets the options for customizing the axes of circular gauge.
        /// </summary>
        [Parameter]
        public List<CircularGaugeAxis> Axes { get; set; }

        /// <summary>
        /// Gets or sets the background color of the gauge. This property accepts value in hex code, rgba string as a valid CSS color string.
        /// </summary>
        [Parameter]
        public string Background { get; set; }

        /// <summary>
        /// Gets or sets the X coordinate of the circular gauge.
        /// </summary>
        [Parameter]
        public string CenterX { get; set; }

        /// <summary>
        /// Gets or sets the Y coordinate of the circular gauge.
        /// </summary>
        [Parameter]
        public string CenterY { get; set; }

        /// <summary>
        /// Gets or sets the information about gauge for assistive technology.
        /// </summary>
        [Parameter]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to enable the drag movement of the pointer in the circular gauge.
        /// </summary>
        [Parameter]
        public bool EnablePointerDrag { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to enable the drag movement of the ranges in the circular gauge.
        /// </summary>
        [Parameter]
        public bool EnableRangeDrag { get; set; }

        /// <summary>
        /// Gets or sets the height of the circular gauge as a string in order to provide input as both like '100px' or '100%'.
        /// If specified as '100%, gauge will render to the full height of its parent element.
        /// </summary>
        [Parameter]
        public string Height { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to enable the margin padding in circular gauge.
        /// </summary>
        [Parameter]
        public bool AllowMargin { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether or not to place the half or quarter circle in center position, if values not specified for centerX and centerY.
        /// </summary>
        [Parameter]
        public bool MoveToCenter { get; set; }

        /// <summary>
        /// Gets or sets the tab index value for the circular gauge.
        /// </summary>
        [Parameter]
        public int TabIndex { get; set; } = 1;

        /// <summary>
        /// Gets or sets the themes supported for circular gauge.
        /// </summary>
        [Parameter]
        public Theme Theme { get; set; } = Theme.Material;

        /// <summary>
        /// Gets or sets the title for circular gauge.
        /// </summary>
        [Parameter]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether or not to enable the grouping separator for a number.
        /// </summary>
        [Parameter]
        public bool EnableGroupingSeparator { get; set; } = true;

        /// <summary>
        /// Gets or sets the width of the circular gauge as a string in order to provide input as both like '100px' or '100%'.
        /// If specified as '100%, gauge will render to the full width of its parent element.
        /// </summary>
        [Parameter]
        public string Width { get; set; }

        /// <summary>
        /// Gets or sets the border of the circular gauge.
        /// </summary>
        internal CircularGaugeBorder Border { get; set; }

        /// <summary>
        /// Gets or sets the legend properties in circular gauge.
        /// </summary>
        internal CircularGaugeLegendSettings LegendSettings { get; set; }

        /// <summary>
        /// Gets or sets the margin value of the circular gauge.
        /// </summary>
        internal CircularGaugeMargin Margin { get; set; }

        /// <summary>
        /// Gets or sets the styles of the title in circular gauge.
        /// </summary>
        internal CircularGaugeTitleStyle TitleStyle { get; set; }

        /// <summary>
        /// Gets or sets the properties of the tooltip.
        /// </summary>
        internal CircularGaugeTooltipSettings Tooltip { get; set; }

        /// <summary>
        /// Gets or sets the instance of the Circular Gauge element.
        /// </summary>
        internal ElementReference Element { get; set; }

        /// <summary>
        /// Gets or sets the size of the circular gauge.
        /// </summary>
        internal SizeD AvailableSize { get; set; } = new SizeD();

        /// <summary>
        /// Gets or sets the properties to render the axis.
        /// </summary>
        internal AxisRenderer AxisRenderer { get; set; } = new AxisRenderer();

        /// <summary>
        /// Gets or sets a value indicating whether or not to render the Circular Gauge component.
        /// </summary>
        internal bool AllowRefresh { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to show the tooltip.
        /// </summary>
        internal bool IsTooltip { get; set; }

        /// <summary>
        /// Gets or sets the properties of the themes.
        /// </summary>
        internal ThemeStyle ThemeStyles { get; set; }

        /// <summary>
        /// Gets or sets the properties of the Circular Gauge events.
        /// </summary>
        internal CircularGaugeEvents CircularGaugeEvents { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the allow animation.
        /// </summary>
        internal bool AllowAnimation { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether or not, the component is rendered in the Internet Explorer..
        /// </summary>
        internal bool IsIE { get; set; }

        private bool AnimationStarted { get; set; }

        /// <summary>
        /// UpdateChildProperties is used to update the child properties.
        /// </summary>
        /// <param name="key">Specifies the child property.</param>
        /// <param name="keyValue">Specifies the child property value.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void UpdateChildProperties(string key, object keyValue)
        {
            switch (key)
            {
                case "Axes":
                    Axes = (List<CircularGaugeAxis>)keyValue;
                    break;
                case "Margin":
                    Margin = (CircularGaugeMargin)keyValue;
                    break;
                case "Tooltip":
                    Tooltip = (CircularGaugeTooltipSettings)keyValue;
                    break;
                case "Border":
                    Border = (CircularGaugeBorder)keyValue;
                    break;
                case "TitleStyle":
                    TitleStyle = (CircularGaugeTitleStyle)keyValue;
                    break;
                case "LegendSettings":
                    LegendSettings = (CircularGaugeLegendSettings)keyValue;
                    break;
            }
        }

        private async Task OnGaugeParametersSet()
        {
            background = NotifyPropertyChanges(nameof(Background), Background, background);
            allowMargin = NotifyPropertyChanges(nameof(AllowMargin), AllowMargin, allowMargin);
            centerX = NotifyPropertyChanges(nameof(CenterX), CenterX, centerX);
            centerY = NotifyPropertyChanges(nameof(CenterY), CenterY, centerY);
            enableGroupingSeparator = NotifyPropertyChanges(nameof(EnableGroupingSeparator), EnableGroupingSeparator, enableGroupingSeparator);
            enablePointerDrag = NotifyPropertyChanges(nameof(EnablePointerDrag), EnablePointerDrag, enablePointerDrag);
            enableRangeDrag = NotifyPropertyChanges(nameof(EnableRangeDrag), EnableRangeDrag, enableRangeDrag);
            height = NotifyPropertyChanges(nameof(Height), Height, height);
            moveToCenter = NotifyPropertyChanges(nameof(MoveToCenter), MoveToCenter, moveToCenter);
            title = NotifyPropertyChanges(nameof(Title), Title, title);
            width = NotifyPropertyChanges(nameof(Width), Width, width);
            theme = NotifyPropertyChanges(nameof(Theme), Theme, theme);
            if (PropertyChanges.Count > 0 && AxisRenderer != null && AxisRenderer.AxisCollection.Count > 0)
            {
                if (PropertyChanges.ContainsKey("EnablePointerDrag"))
                {
                    await InvokeMethod("sfBlazor.CircularGauge.setPointerDragStatus", new object[] { Element, EnablePointerDrag });
                }

                if (PropertyChanges.ContainsKey("EnableRangeDrag"))
                {
                    await InvokeMethod("sfBlazor.CircularGauge.setRangeDragStatus", new object[] { Element, EnableRangeDrag });
                }

                if (PropertyChanges.ContainsKey("Title"))
                {
                    AxisRenderer.RenderTitle(AxisRenderer.AxisCollection[0]);
                    StateHasChanged();
                }
                else
                {
                    if (PropertyChanges.ContainsKey("Width") || PropertyChanges.ContainsKey("Height"))
                    {
                        await InvokeMethod("sfBlazor.CircularGauge.getContainerSize", new object[] { ID, DotnetObjectReference });
                    }

                    PropertyChangeHandler();
                }

                PropertyChanges.Clear();
            }
        }
    }
}