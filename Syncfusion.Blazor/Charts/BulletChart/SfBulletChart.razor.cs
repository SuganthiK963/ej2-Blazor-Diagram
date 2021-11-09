using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.BulletChart.Internal;
using Syncfusion.Blazor.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Bullet Chart control is used to display the one or more measures and compares them to a target value with built-in features like orientation, qualitative ranges, multiple measures, labels, tooltip and animation.
    /// </summary>
    /// <typeparam name="TValue">Represents the generic data type of the bullet chart control.</typeparam>
    public partial class SfBulletChart<TValue>
    {
        private bool enableGroupSeparator;
        private bool enableRtl;
        private string height;
        private double interval;
        private string labelFormat;
        private LabelsPlacement labelPosition;
        private TextPosition titlePosition;
        private FeatureType type;
        private string valueFill;
        private double valueHeight;
        private string width;
        private TickPosition tickPosition;
        private double maximum;
        private double minimum;
        private double minorTicksPerInterval;
        private bool opposedPosition;
        private OrientationType orientation;
        private string targetColor;
        private double targetWidth;
        private Theme theme;
        private IEnumerable<TValue> dataSource;
        private string categoryField;

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
        /// Specifies events of the component.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public BulletChartEvents Events { get; set; }

        /// <summary>
        /// Specifies legend rendering of the component.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public BulletChartLegend ChartLegend { get; set; }

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
        /// Specifies margin of the component.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public BulletChartMargin Margin { get; set; }

        /// <summary>
        /// Specifies minor tick lines of the component.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public BulletChartMinorTickLines MinorTickLines { get; set; }

        /// <summary>
        /// Specifies sub-title style of the component.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public BulletChartSubTitleStyle SubtitleStyle { get; set; }

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
        /// Specifies color mapping ranges of the component.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public List<BulletChartRange> Ranges { get; set; }

        /// <summary>
        /// Specifies the rendering information of tooltip.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public BulletChartTooltipRender<TValue> ChartToolTip { get; set; }

        /// <summary>
        /// Specifies the settings of tooltip.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public BulletChartTooltip<TValue> Tooltip { get; set; }

        /// <summary>
        /// The DataSource field that contains the target value.
        /// </summary>
        [Parameter]
        public List<TargetType> TargetTypes { get; set; } = new List<TargetType>() { TargetType.Rect, TargetType.Cross, TargetType.Circle };

        /// <summary>
        /// Sets and gets format for the bullet chart axis and data label.
        /// </summary>
        [Parameter]
        public string Format { get; set; } = string.Empty;

        /// <summary>
        /// Set the id string for the bullet chart component.
        /// </summary>
        [Parameter]
        public string ID { get; set; } = SfBaseUtils.GenerateID("bulletChart");

        /// <summary>
        /// Sets and gets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// It defines the category for the data source.
        /// </summary>
        [Parameter]
        public string CategoryField { get; set; }

        /// <summary>
        /// default value of multiple data bullet chart.
        /// </summary>
        [Parameter]
        public IEnumerable<TValue> DataSource { get; set; }

        /// <summary>
        /// default value for enableGroupSeparator.
        /// </summary>
        [Parameter]
        public bool EnableGroupSeparator { get; set; }

        /// <summary>
        /// Enable or disable persistence of the component.
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
        /// The height of the bullet chart as a string accepts input both as '100px' or '100%'.
        /// If specified as '100%, bullet chart renders to the full height of its parent element.
        /// </summary>
        [Parameter]
        public string Height { get; set; }

        /// <summary>
        /// Specifies the interval for an scale.
        /// </summary>
        [Parameter]
        public double Interval { get; set; } = -1;

        /// <summary>
        /// Specifies the format of the bullet chart axis labels.
        /// </summary>
        [Parameter]
        public string LabelFormat { get; set; } = string.Empty;

        /// <summary>
        /// specifies the axis label position of the bullet chart.
        /// </summary>
        [Parameter]
        public LabelsPlacement LabelPosition { get; set; } = LabelsPlacement.Outside;

        /// <summary>
        /// specifies the locale of the bullet chart.
        /// </summary>
        /// <exclude />
        [Parameter]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
        public string Locale { get; set; }

        /// <summary>
        /// Specifies the maximum range of an scale.
        /// </summary>
        [Parameter]
        public double Maximum { get; set; } = -1;

        /// <summary>
        /// Specifies the minimum range of an scale.
        /// </summary>
        [Parameter]
        public double Minimum { get; set; } = -1;

        /// <summary>
        /// specifies the interval of minor ticks.
        /// </summary>
        [Parameter]
        public double MinorTicksPerInterval { get; set; } = 4;

        /// <summary>
        /// If set to true, the axis will render at the opposite side of its default position.
        /// </summary>
        [Parameter]
        public bool OpposedPosition { get; set; }

        /// <summary>
        /// Orientation of the scale.
        /// </summary>
        [Parameter]
        public OrientationType Orientation { get; set; } = OrientationType.Horizontal;

        /// <summary>
        /// Specifies the sub title of the bullet chart.
        /// </summary>
        [Parameter]
        public string Subtitle { get; set; } = string.Empty;

        /// <summary>
        /// TabIndex value for the bullet chart.
        /// </summary>
        [Parameter]
        public int TabIndex { get; set; } = 1;

        /// <summary>
        /// Default stroke of comparative measure.
        /// </summary>
        [Parameter]
        public string TargetColor { get; set; } = "#191919";

        /// <summary>
        /// The DataSource field that contains the target value.
        /// </summary>
        [Parameter]
        public string TargetField { get; set; } = string.Empty;

        /// <summary>
        /// Options for customizing comparative bar color bullet chart.
        /// </summary>
        [Parameter]
        public double TargetWidth { get; set; } = 5;

        /// <summary>
        /// Specifies the theme for the bullet chart.
        /// </summary>
        [Parameter]
        public Theme Theme { get; set; } = Theme.Bootstrap4;

        /// <summary>
        /// specifies the tick position of the bullet chart.
        /// </summary>
        [Parameter]
        public TickPosition TickPosition { get; set; } = TickPosition.Outside;

        /// <summary>
        /// Specifies the title of the bullet chart.
        /// </summary>
        [Parameter]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Sets the title position of bullet chart.
        /// </summary>
        [Parameter]
        public TextPosition TitlePosition { get; set; } = TextPosition.Top;

        /// <summary>
        /// Default type on feature measure.
        /// </summary>
        [Parameter]
        public FeatureType Type { get; set; } = FeatureType.Rect;

        /// <summary>
        /// The DataSource field that contains the value value.
        /// </summary>
        [Parameter]
        public string ValueField { get; set; } = string.Empty;

        /// <summary>
        /// Default stroke color of feature measure.
        /// </summary>
        [Parameter]
        public string ValueFill { get; set; }

        /// <summary>
        /// Options for customizing feature bar height of the bullet chart.
        /// </summary>
        [Parameter]
        public double ValueHeight { get; set; } = 6;

        /// <summary>
        /// The width of the bullet chart as a string accepts input as both like '100px' or '100%'.
        /// If specified as '100%, bullet chart renders to the full width of its parent element.
        /// </summary>
        [Parameter]
        public string Width { get; set; }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            DependentScripts.Add(Syncfusion.Blazor.Internal.ScriptModules.SfSvgExport);
            UpdateObservableEvents(nameof(DataSource), DataSource);
            interval = NotifyPropertyChanges(nameof(Interval), Interval, interval);
            labelPosition = NotifyPropertyChanges(nameof(LabelPosition), LabelPosition, labelPosition);
            minimum = NotifyPropertyChanges(nameof(Minimum), Minimum, minimum);
            maximum = NotifyPropertyChanges(nameof(Maximum), Maximum, maximum);
            opposedPosition = NotifyPropertyChanges(nameof(OpposedPosition), OpposedPosition, opposedPosition);
            tickPosition = NotifyPropertyChanges(nameof(TickPosition), TickPosition, tickPosition);
            minorTicksPerInterval = NotifyPropertyChanges(nameof(MinorTicksPerInterval), MinorTicksPerInterval, minorTicksPerInterval);
            titlePosition = NotifyPropertyChanges(nameof(TitlePosition), TitlePosition, titlePosition);
            dataSource = NotifyPropertyChanges(nameof(DataSource), DataSource, dataSource);
            height = NotifyPropertyChanges(nameof(Height), Height, height);
            width = NotifyPropertyChanges(nameof(Width), Width, width);
            enableRtl = NotifyPropertyChanges(nameof(EnableRtl), EnableRtl, enableRtl);
            orientation = NotifyPropertyChanges(nameof(Orientation), Orientation, orientation);
            theme = NotifyPropertyChanges(nameof(Theme), Theme, theme);
            categoryField = NotifyPropertyChanges(nameof(CategoryField), CategoryField, categoryField);
            enableGroupSeparator = NotifyPropertyChanges(nameof(EnableGroupSeparator), EnableGroupSeparator, enableGroupSeparator);
            labelFormat = NotifyPropertyChanges(nameof(LabelFormat), LabelFormat, labelFormat);
            type = NotifyPropertyChanges(nameof(Type), Type, type);
            targetColor = NotifyPropertyChanges(nameof(TargetColor), TargetColor, targetColor);
            valueFill = NotifyPropertyChanges(nameof(ValueFill), ValueFill, valueFill);
            targetWidth = NotifyPropertyChanges(nameof(TargetWidth), TargetWidth, targetWidth);
            valueHeight = NotifyPropertyChanges(nameof(ValueHeight), ValueHeight, valueHeight);
            if (PropertyChanges.Count > 0 && IsRendered)
            {
                await OnPropertyChanged(PropertyChanges, nameof(IBulletChart));
                PropertyChanges.Clear();
            }
        }
    }
}