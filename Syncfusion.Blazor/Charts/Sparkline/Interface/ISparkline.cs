using Syncfusion.Blazor.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// The interface specifies the properties of the sparkline component.
    /// </summary>
    public interface ISparkline
    {
        /// <summary>
        /// Set the id string for the bullet chart component.
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Enable right to left rendering of the sparkline.
        /// </summary>
        public bool EnableRtl { get; set; }

        /// <summary>
        /// End point color customizations for the sparkline.
        /// </summary>
        public string EndPointColor { get; set; }

        /// <summary>
        /// Fill color customization for the sparkline.
        /// </summary>
        public string Fill { get; set; }

        /// <summary>
        /// Label format for the sparkline.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Height customization for the sparkline.
        /// </summary>
        public string Height { get; set; }

        /// <summary>
        /// HighPointColor customizations for the sparkline.
        /// </summary>
        public string HighPointColor { get; set; }

        /// <summary>
        /// LineWidth customizations for the sparkline.
        /// </summary>
        public double LineWidth { get; set; }

        /// <summary>
        /// LowPointColor customizations for the sparkline.
        /// </summary>
        public string LowPointColor { get; set; }

        /// <summary>
        /// Negative point color customizations for the sparkline.
        /// </summary>
        public string NegativePointColor { get; set; }

        /// <summary>
        /// Opacity for the series.
        /// </summary>
        public double Opacity { get; set; }

        /// <summary>
        /// Palete for the sparkline.
        /// </summary>
        public string[] Palette { get; set; }

        /// <summary>
        /// Query to be performed in the dataSource of the sparkline.
        /// </summary>
        public Query Query { get; set; }

        /// <summary>
        /// Range padding for the sparkline y axis.
        /// </summary>
        public SparklineRangePadding RangePadding { get; set; }

        /// <summary>
        /// Customization of startPoint color for the sparkline chart.
        /// </summary>
        public string StartPointColor { get; set; }

        /// <summary>
        /// Theme customization for the sparkline .
        /// </summary>
        public Theme Theme { get; set; }

        /// <summary>
        /// Customization of TiePointColor .
        /// </summary>
        public string TiePointColor { get; set; }

        /// <summary>
        /// Series type for the sparkline.
        /// </summary>
        public SparklineType Type { get; set; }

        /// <summary>
        /// Grouping seperator for the sparkline.
        /// </summary>
        public bool EnableGroupingSeparator { get; set; }

        /// <summary>
        /// valueType customization for the sparkline.
        /// </summary>
        public SparklineValueType ValueType { get; set; }

        /// <summary>
        /// Customization of width for the sparkline.
        /// </summary>
        public string Width { get; set; }

        /// <summary>
        /// XName field mapping for the sparkline datasource.
        /// </summary>
        public string XName { get; set; }

        /// <summary>
        /// YName field mapping for the sparkline datasource.
        /// </summary>
        public string YName { get; set; }

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
        public SparklineAxisSettings AxisSettings { get; set; }

        /// <summary>
        /// Specifies the border settings of sparkline.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public SparklineBorder Border { get; set; }

        /// <summary>
        /// Specifies the container settings of sparkline.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public SparklineContainerArea ContainerArea { get; set; }

        /// <summary>
        /// Specifies the data label settings of sparkline.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public SparklineDataLabelSettings DataLabelSettings { get; set; }

        /// <summary>
        /// Specifies the range band settings of sparkline.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public List<SparklineRangeBand> RangeBandSettings { get; set; }

        /// <summary>
        /// Specifies the padding settings of sparkline.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public SparklinePadding Padding { get; set; }

        /// <summary>
        /// Specifies the events of sparkline.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public SparklineEvents Events { get; set; }

        /// <summary>
        /// Specifies to render the component, based on property changes.
        /// </summary>
        /// <param name="propertyChanges">List changed properties.</param>
        /// <param name="parent">The class belongs too.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Task OnPropertyChanged(Dictionary<string, object> propertyChanges, string parent);
    }
}