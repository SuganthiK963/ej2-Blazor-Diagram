using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;
using System;
using System.ComponentModel;
using Syncfusion.Blazor.Charts.Sparkline.Internal;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Charts
{
    public partial class SfSparkline<TValue> : SfDataBoundComponent
    {
        private IEnumerable<TValue> dataSource;
        private bool enableRtl;
        private string endPointColor;
        private string fill;
        private string format;
        private string height;
        private string highPointColor;
        private double lineWidth;
        private Query query;
        private SparklineRangePadding rangePadding;
        private string startPointColor;
        private Theme theme;
        private string tiePointColor;
        private SparklineType type;
        private bool enableGroupingSeparator;
        private SparklineValueType valueType;
        private string width;
        private string xname;
        private string yname;
        private string lowPointColor;
        private string negativePointColor;
        private double opacity;
        private string[] palette;

        /// <summary>
        /// Set the id string for the sparkline component.
        /// </summary>
        [Parameter]
        public string ID { get; set; } = SfBaseUtils.GenerateID("sparkline");

        /// <summary>
        /// To configure sparkline data source.
        /// </summary>
        [Parameter]
        public IEnumerable<TValue> DataSource { get; set; }

        /// <summary>
        /// Sets or gets to enable the persistence of sparkline.
        /// </summary>
        /// <exclude />
        [Parameter]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
        public bool EnablePersistence { get; set; }

        /// <summary>
        /// Enable or disable rendering component in right to left direction.
        /// </summary>
        [Parameter]
        public bool EnableRtl { get; set; }

        /// <summary>
        /// To configure sparkline series last x value point color.
        /// </summary>
        [Parameter]
        public string EndPointColor { get; set; } = string.Empty;

        /// <summary>
        /// To configure sparkline series fill.
        /// </summary>
        [Parameter]
        public string Fill { get; set; } = "#00bdae";

        /// <summary>
        /// To apply internationalization for sparkline.
        /// </summary>
        [Parameter]
        public string Format { get; set; } = string.Empty;

        /// <summary>
        /// To configure Sparkline height.
        /// </summary>
        [Parameter]
        public string Height { get; set; } = "100%";

        /// <summary>
        /// To configure sparkline series highest y value point color.
        /// </summary>
        [Parameter]
        public string HighPointColor { get; set; } = string.Empty;

        /// <summary>
        /// To configure sparkline line series width.
        /// </summary>
        [Parameter]
        public double LineWidth { get; set; } = 1;

        /// <summary>
        /// Sets or gets to locale of sparkline.
        /// </summary>
        /// <exclude />
        [Parameter]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
        public string Locale { get; set; } = string.Empty;

        /// <summary>
        /// To configure sparkline series lowest y value point color.
        /// </summary>
        [Parameter]
        public string LowPointColor { get; set; } = string.Empty;

        /// <summary>
        /// To configure sparkline series negative y value point color.
        /// </summary>
        [Parameter]
        public string NegativePointColor { get; set; } = string.Empty;

        /// <summary>
        /// To configure sparkline line series opacity.
        /// </summary>
        [Parameter]
        public double Opacity { get; set; } = 1;

        /// <summary>
        /// To configure sparkline series color palette. It applicable to column and pie type series.
        /// </summary>
        [Parameter]
        public string[] Palette { get; set; }

        /// <summary>
        /// Specifies the query for filter the data.
        /// </summary>
        [Parameter]
        public Query Query { get; set; } = new Query();

        /// <summary>
        /// To configure Sparkline series type.
        /// </summary>
        [Parameter]
        public SparklineRangePadding RangePadding { get; set; } = SparklineRangePadding.None;

        /// <summary>
        /// To configure sparkline series first x value point color.
        /// </summary>
        [Parameter]
        public string StartPointColor { get; set; } = string.Empty;

        /// <summary>
        /// To configure sparkline theme.
        /// </summary>
        [Parameter]
        public Theme Theme { get; set; } = Theme.Bootstrap4;

        /// <summary>
        /// To configure sparkline winloss series tie y value point color.
        /// </summary>
        [Parameter]
        public string TiePointColor { get; set; } = string.Empty;

        /// <summary>
        /// To configure Sparkline series type.
        /// </summary>
        [Parameter]
        public SparklineType Type { get; set; } = SparklineType.Line;

        /// <summary>
        /// To enable the separator.
        /// </summary>
        [Parameter]
        public bool EnableGroupingSeparator { get; set; }

        /// <summary>
        /// To configure sparkline series value type.
        /// </summary>
        [Parameter]
        public SparklineValueType ValueType { get; set; } = SparklineValueType.Numeric;

        /// <summary>
        /// To configure Sparkline width.
        /// </summary>
        [Parameter]
        public string Width { get; set; } = "100%";

        /// <summary>
        /// To configure sparkline series xName.
        /// </summary>
        [Parameter]
        public string XName { get; set; } = string.Empty;

        /// <summary>
        /// To configure sparkline series yName.
        /// </summary>
        [Parameter]
        public string YName { get; set; } = string.Empty;

        /// <summary>
        /// Sets and gets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        internal SparklineTrackline<TValue> Trackline { get; set; }

        internal SparklineTooltip<TValue> Tooltip { get; set; }

        internal SparklineTooltipSettings<TValue> TooltipSettings { get; set; }

        /// <summary>
        /// Specifies the range band settings of sparkline.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public List<SparklineRangeBand> RangeBandSettings { get; set; } = new List<SparklineRangeBand>();

        /// <summary>
        /// Specifies the marker settings of sparkline.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public SparklineMarkerSettings MarkerSettings { get; set; }

        /// <summary>
        /// Specifies the axis settings of sparkline.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public SparklineAxisSettings AxisSettings { get; set; } = new SparklineAxisSettings();

        /// <summary>
        /// Specifies the border settings of sparkline.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public SparklineBorder Border { get; set; } = new SparklineBorder();

        /// <summary>
        /// Specifies the container settings of sparkline.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public SparklineContainerArea ContainerArea { get; set; } = new SparklineContainerArea();

        /// <summary>
        /// Specifies the data label settings of sparkline.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public SparklineDataLabelSettings DataLabelSettings { get; set; } = new SparklineDataLabelSettings();

        /// <summary>
        /// Specifies the padding settings of sparkline.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public SparklinePadding Padding { get; set; } = new SparklinePadding();

        /// <summary>
        /// Specifies the events of sparkline.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public SparklineEvents Events { get; set; }
    }
}