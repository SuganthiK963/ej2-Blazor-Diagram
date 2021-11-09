using Syncfusion.Blazor.Charts.BulletChart.Internal;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// The interface specifies the properties of the bullet chart component.
    /// </summary>
    public interface IBulletChart
    {
        /// <summary>
        /// It defines the category for the data source.
        /// </summary>
        public string CategoryField { get; set; }

        /// <summary>
        /// Specifies the EnableGroupSeparator for the Bullet Chart.
        /// </summary>
        public bool EnableGroupSeparator { get; set; }

        /// <summary>
        /// To enable right to left.
        /// </summary>
        public bool EnableRtl { get; set; }

        /// <summary>
        /// Specifies the height for the Bullet Chart.
        /// </summary>
        public string Height { get; set; }

        /// <summary>
        /// Specifies the interval for an scale.
        /// </summary>
        public double Interval { get; set; }

        /// <summary>
        /// Specifies the format of the bullet chart axis labels.
        /// </summary>
        public string LabelFormat { get; set; }

        /// <summary>
        /// Specifies the axis label position of the bullet chart.
        /// </summary>
        public LabelsPlacement LabelPosition { get; set; }

        /// <summary>
        /// Specifies the maximum range of an scale.
        /// </summary>
        public double Maximum { get; set; }

        /// <summary>
        /// Specifies the manimum range of an scale.
        /// </summary>
        public double Minimum { get; set; }

        /// <summary>
        /// Specifies the interval of minor ticks.
        /// </summary>
        public double MinorTicksPerInterval { get; set; }

        /// <summary>
        /// If set to true, the axis will render at the opposite side of its default position.
        /// </summary>
        public bool OpposedPosition { get; set; }

        /// <summary>
        /// Orientation of the scale.
        /// </summary>
        public OrientationType Orientation { get; set; }

        /// <summary>
        /// Specifies the sub title of the bullet chart.
        /// </summary>
        public string Subtitle { get; set; }

        /// <summary>
        /// TabIndex value for the bullet chart.
        /// </summary>
        public int TabIndex { get; set; }

        /// <summary>
        /// Default stroke of comparative measure.
        /// </summary>
        public string TargetColor { get; set; }

        /// <summary>
        /// The DataSource field that contains the target value.
        /// </summary>
        public string TargetField { get; set; }

        /// <summary>
        /// Options for customizing comparative bar color bullet chart.
        /// </summary>
        public double TargetWidth { get; set; }

        /// <summary>
        /// Specifies the theme for the bullet chart.
        /// </summary>
        public Theme Theme { get; set; }

        /// <summary>
        /// Specifies the tick position of the bullet chart.
        /// </summary>
        public TickPosition TickPosition { get; set; }

        /// <summary>
        /// Specifies the title of the bullet chart.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Sets the title position of bullet chart.
        /// </summary>
        public TextPosition TitlePosition { get; set; }

        /// <summary>
        /// Default type on feature measure.
        /// </summary>
        public FeatureType Type { get; set; }

        /// <summary>
        /// The DataSource field that contains the value value.
        /// </summary>
        public string ValueField { get; set; }

        /// <summary>
        /// Default stroke color of feature measure.
        /// </summary>
        public string ValueFill { get; set; }

        /// <summary>
        /// Options for customizing feature bar height of the bullet chart.
        /// </summary>
        public double ValueHeight { get; set; }

        /// <summary>
        /// Specifies the width for the Bullet Chart.
        /// </summary>
        public string Width { get; set; }

        /// <summary>
        /// Sets and gets format for the bullet chart axis and data label format.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Set the id string for the bullet chart component.
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// The DataSource field that contains the target value.
        /// </summary>
        public List<TargetType> TargetTypes { get; set; }

        /// <summary>
        /// Specifies color mapping ranges of the component.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public List<BulletChartRange> Ranges { get; set; }

        /// <summary>
        /// Specifies title style of the component.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public BulletChartTitleStyle TitleStyle { get; set; }

        /// <summary>
        /// Specifies border of the component value bar.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public BulletChartValueBorder ValueBorder { get; set; }

        /// <summary>
        /// Specifies sub-title style of the component.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public BulletChartSubTitleStyle SubtitleStyle { get; set; }

        /// <summary>
        /// Specifies animation of the component value and target bar.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public BulletChartAnimation Animation { get; set; }

        /// <summary>
        /// Specifies border of the component.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public BulletChartBorder Border { get; set; }

        /// <summary>
        /// Specifies category label style of the component.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public BulletChartCategoryLabelStyle CategoryLabelStyle { get; set; }

        /// <summary>
        /// Specifies data label style of the component.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public BulletChartDataLabel DataLabel { get; set; }

        /// <summary>
        /// Specifies label style of the component.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public BulletChartLabelStyle LabelStyle { get; set; }

        /// <summary>
        /// Specifies legend settings of the component.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public BulletChartLegendSettings LegendSettings { get; set; }

        /// <summary>
        /// Specifies major tick lines of the component.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public BulletChartMajorTickLines MajorTickLines { get; set; }

        /// <summary>
        /// Specifies minor tick lines of the component.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public BulletChartMinorTickLines MinorTickLines { get; set; }

        /// <summary>
        /// Specifies margin of the component.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public BulletChartMargin Margin { get; set; }

        /// <summary>
        /// Specifies helper methods of the component rendering.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public BulletChartHelper Helper { get; set; }

        /// <summary>
        /// Specifies rendering of the component.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public BulletChartRender Render { get; set; }

        /// <summary>
        /// Specifies legend rendering of the component.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public BulletChartLegend ChartLegend { get; set; }

        /// <summary>
        /// Specifies size of the component.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public SizeInfo AvailableSize { get; set; }

        /// <summary>
        /// Specifies style of the component.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Style Style { get; set; }

        /// <summary>
        /// Specifies scale rendering of the component.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public BulletChartScaleRender ScaleRender { get; set; }

        /// <summary>
        /// Specifies events of the component.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public BulletChartEvents Events { get; set; }

        /// <summary>
        /// Specifies property binding manipulations.
        /// </summary>
        /// <param name="propertyChanges">Represents the properties.</param>
        /// <param name="parent">Represents the class belongs too.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Task OnPropertyChanged(Dictionary<string, object> propertyChanges, string parent);

        /// <summary>
        /// Specifies the rendering direction of the component.
        /// </summary>
        /// <returns>Returns bool value.</returns>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsRtlEnabled();
    }
}