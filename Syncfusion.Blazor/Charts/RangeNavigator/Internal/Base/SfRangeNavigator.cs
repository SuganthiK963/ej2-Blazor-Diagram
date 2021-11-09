using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Internal;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Charts.Chart.Internal;
using System.Text.Json;
using Syncfusion.Blazor.DataVizCommon;
using System.Linq;
using Syncfusion.Blazor.Charts.RangeNavigator.Internal;
using System;
using Syncfusion.Blazor.Navigations;
using System.Runtime.CompilerServices;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Charts.Internal;

[assembly: InternalsVisibleTo("Syncfusion.Blazor.StockChart")]

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// The range navigator component is quickly visualize high-level data and select a time period with a modern interface to filter data for in-depth analysis.
    /// The range navigator contains labels, ticks, and an axis line to customize its appearance.
    /// </summary>
    public partial class SfRangeNavigator : SfDataBoundComponent
    {
        private static List<string> chartFontKeys = new List<string>();
        private bool enableGrouping;
        private bool enableRtl;
        private RangeIntervalType groupBy;
        private string height;
        private double interval;
        private RangeIntervalType intervalType;
        private string labelFormat;
        private RangeLabelIntersectAction labelIntersectAction;
        private AxisPosition labelPosition;
        private double logBase;
        private string maximum;
        private string minimum;
        private Query query;
        private LabelAlignment secondaryLabelAlignment;
        private Theme theme;
        private AxisPosition tickPosition;
        private bool useGroupingSeparator;
        private object value;
        private RangeValueType valueType;
        private string width;
        private string xname;
        private string yname;
        private bool allowIntervalData;
        private bool allowSnapping;
        private double animationDuration;
        private object dataSource;
        private bool disableRangeSelector;
        private List<string> fontKeys = new List<string>();

        internal event EventHandler<ChartInternalMouseEventArgs> MouseClick;

        internal RangeThemeStyle ThemeStyle { get; set; }

        internal IEnumerable<object> FinalData { get; set; }

        internal bool IsTouch { get; set; }

        internal RangeAxis RangeAxisModule { get; set; }

        internal RangeSlider RangeSliderModule { get; set; }

        internal RangeTooltip TooltipModule { get; set; }

        internal PeriodSelector PeriodSelectorModule { get; set; }

        internal double StartValue { get; set; } = double.NaN;

        internal double EndValue { get; set; } = double.NaN;

        internal double MouseX { get; set; }

        internal double MouseDownX { get; set; }

        internal RangeNavigatorEvents RangeNavigatorEvents { get; set; }

        internal SfRangeNavigator Chart { get; set; }

        internal ChartAxisRenderer XAxisRenderer { get; set; }

        internal ChartAxisRenderer YAxisRenderer { get; set; }

        internal bool AnimateSeries { get; set; }

        internal RenderFragment ChartContent { get; set; }

        internal RenderFragment PeriodSelectorContent { get; set; }

        internal RenderFragment TooltipContent { get; set; }

        internal SvgRendering SvgRenderer { get; set; } = new SvgRendering();

        internal RangeNavigatorHelper RangeHelper { get; set; } = new RangeNavigatorHelper();

        internal RangeNavigatorSeries ChartSeries { get; set; } = new RangeNavigatorSeries();

        internal List<RangeLabelRenderEventArgs> Labels { get; set; } = new List<RangeLabelRenderEventArgs>();

        internal RangeNavigatorMajorGridLines MajorGridLines { get; set; } = new RangeNavigatorMajorGridLines();

        internal RangeNavigatorMajorTickLines MajorTickLines { get; set; } = new RangeNavigatorMajorTickLines();

        internal bool IsValueUpdated { get; set; }

#pragma warning disable BL0005
        internal RangeNavigatorMargin Margin { get; set; } = new RangeNavigatorMargin { Left = 5, Top = 5, Bottom = 5, Right = 5 };

        internal ChartCommonFont LabelStyle { get; set; } = new ChartCommonFont { Size = "12px" };
#pragma warning restore BL0005

        internal RangeNavigatorBorder NavigatorBorder { get; set; } = new RangeNavigatorBorder();

        internal RangeNavigatorStyleSettings NavigatorStyleSettings { get; set; } = new RangeNavigatorStyleSettings();

        internal RangeNavigatorPeriodSelectorSettings PeriodSelectorSettings { get; set; } = new RangeNavigatorPeriodSelectorSettings();

        internal RangeNavigatorRangeTooltipSettings Tooltip { get; set; } = new RangeNavigatorRangeTooltipSettings();

        internal List<RangeNavigatorSeries> Series { get; set; } = new List<RangeNavigatorSeries>();

        /// <summary>
        /// Set and gets the id for the chart component.
        /// </summary>
        [Parameter]
        public string Id { get; set; }

        /// <summary>
        /// Sets and gets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Allow the data to be selected for that particular interval while clicking the particular label.
        /// </summary>
        [Parameter]
        public bool AllowIntervalData { get; set; }

        /// <summary>
        /// Enable snapping for range navigator sliders.
        /// </summary>
        [Parameter]
        public bool AllowSnapping { get; set; }

        /// <summary>
        /// It defines the duration for an animation.
        /// </summary>
        [Parameter]
        public double AnimationDuration { get; set; } = 500;

        /// <summary>
        /// It defines the data source for a range navigator.
        /// </summary>
        [Parameter]
        public object DataSource { get; set; }

        /// <summary>
        /// To render the period selector with out range navigator.
        /// </summary>
        [Parameter]
        public bool DisableRangeSelector { get; set; }

        /// <summary>
        /// To enable deferred update on the range navigator.
        /// </summary>
        /// <exclude />
        [Parameter]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
        public bool EnableDeferredUpdate { get; set; }

        /// <summary>
        /// Specifies the stock chart instance.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Parameter]
        public bool IsStockChart { get; set; }

        /// <summary>
        /// Specifies the chart height.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Parameter]
        public double ChartHeight { get; set; }

        /// <summary>
        /// Specifies the data to be update in range navigator.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Parameter]
        public Action<double, double, RangeNavigatorPeriod> UpdateChartData { get; set; }

        /// <summary>
        /// Specifies the rendering in range navigator.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Parameter]
        public Func<string, bool> ShouldSelectorRender { get; set; }

        /// <summary>
        /// Specifies the event to update the period.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Parameter]
        public Action UpdatePeriodEvent { get; set; }

        /// <summary>
        /// Specifies to update the element in range navigator.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Parameter]
        public Func<List<ToolbarItem>> UpdateCustomElement { get; set; }

        /// <summary>
        /// Specifies to update the dropdown element in range navigator.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Parameter]
        public Action<List<ToolbarItem>> UpdateDropdownElement { get; set; }

        /// <summary>
        /// Specifies to get the range navigator instance..
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Parameter]
        public Func<SfRangeNavigator> GetRangeNavigator { get; set; }

        /// <summary>
        /// Enable grouping for the labels.
        /// </summary>
        [Parameter]
        public bool EnableGrouping { get; set; }

        /// <summary>
        /// To enable persistence state of the range navigator.
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
        /// GroupBy property for the axis.
        /// </summary>
        [Parameter]
        public RangeIntervalType GroupBy { get; set; }

        /// <summary>
        /// The height of the chart as a string accepts input both as '100px' or '100%'.
        /// If specified as '100%, range navigator renders to the full height of its parent element.
        /// </summary>
        [Parameter]
        public string Height { get; set; } = "100%";

        /// <summary>
        /// interval value for the axis.
        /// </summary>
        [Parameter]
        public double Interval { get; set; } = double.NaN;

        /// <summary>
        /// IntervalType for the dateTime axis.
        /// </summary>
        [Parameter]
        public RangeIntervalType IntervalType { get; set; } = RangeIntervalType.Auto;

        /// <summary>
        /// Used to format the axis label that accepts any global string format like 'C', 'n1', 'P' etc.
        /// It also accepts placeholder like '{value}°C' in which value represent the axis label, e.g, 20°C.
        /// </summary>
        [Parameter]
        public string LabelFormat { get; set; } = string.Empty;

        /// <summary>
        /// Specifies, when the axis labels intersect with each other.They are,
        /// None: Shows all the labels.
        /// Hide: Hides the label when it intersects.
        /// </summary>
        [Parameter]
        public RangeLabelIntersectAction LabelIntersectAction { get; set; } = RangeLabelIntersectAction.Hide;

        /// <summary>
        /// Label positions for the axis.
        /// </summary>
        [Parameter]
        public AxisPosition LabelPosition { get; set; } = AxisPosition.Outside;

        /// <summary>
        /// Specifies the locale of the range navigator.
        /// </summary>
        /// <exclude />
        [Parameter]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
        public string Locale { get; set; } = "en-US";

        /// <summary>
        /// Get and sets the base value for log axis.
        /// </summary>
        [Parameter]
        public double LogBase { get; set; } = 10;

        /// <summary>
        /// Get and sets maximum value for the axis.
        /// </summary>
        [Parameter]
        public string Maximum { get; set; }

        /// <summary>
        /// Get and sets minimum value for the axis.
        /// </summary>
        [Parameter]
        public string Minimum { get; set; }

        /// <summary>
        /// Get and sets the query for the data source.
        /// </summary>
        [Parameter]
        public Query Query { get; set; }

        /// <summary>
        /// It specifies the label alignment for secondary axis labels.
        /// </summary>
        [Parameter]
        public LabelAlignment SecondaryLabelAlignment { get; set; } = LabelAlignment.Middle;

        /// <summary>
        /// Specifies the skeleton of the range navigator.
        /// </summary>
        /// <exclude />
        [Parameter]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
        public string Skeleton
        {
            get
            {
                return RangeNavigatorSkeleton;
            }

            set
            {
                RangeNavigatorSkeleton = value;
            }
        }

        internal string RangeNavigatorSkeleton { get; set; } = string.Empty;

        /// <summary>
        /// Specifies the skeleton type of the range navigator.
        /// </summary>
        /// <exclude />
        [Parameter]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
        public SkeletonType SkeletonType { get; set; } = SkeletonType.DateTime;

        /// <summary>
        /// Specifies the theme for the range navigator.
        /// </summary>
        [Parameter]
        public Theme Theme { get; set; } = Theme.Bootstrap4;

        /// <summary>
        /// Specifies the tick Position for axis.
        /// </summary>
        [Parameter]
        public AxisPosition TickPosition { get; set; } = AxisPosition.Outside;

        /// <summary>
        /// Specifies whether a grouping separator should be used for a number.
        /// </summary>
        [Parameter]
        public bool UseGroupingSeparator { get; set; }

        /// <summary>
        /// Selected range for range navigator.
        /// </summary>
        [Parameter]
        public object Value { get; set; }

        /// <summary>
        /// Specifies the callback to trigger when the value changes.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Parameter]
        public EventCallback<object> ValueChanged { get; set; }

        /// <summary>
        /// Get and sets the valueType for the axis.
        /// </summary>
        [Parameter]
        public RangeValueType ValueType { get; set; } = RangeValueType.Double;

        /// <summary>
        /// The width of the range navigator as a string accepts input as both like '100px' or '100%'.
        /// If specified as '100%, range navigator renders to the full width of its parent element.
        /// </summary>
        [Parameter]
        public string Width { get; set; } = "100%";

        /// <summary>
        /// It defines the xName for the range navigator.
        /// </summary>
        [Parameter]
        public string XName { get; set; }

        /// <summary>
        /// It defines the yName for the range navigator.
        /// </summary>
        [Parameter]
        public string YName { get; set; }

        internal static void InvokeEvent<T>(object eventFn, T eventArgs)
        {
            if (eventFn != null)
            {
                var eventHandler = (Action<T>)eventFn;
                eventHandler.Invoke(eventArgs);
            }
        }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Chart = this;
            if (string.IsNullOrEmpty(Id))
            {
                Id = SfBaseUtils.GenerateID("SfRangeNavigator");
            }

            ScriptModules = SfScriptModules.SfRangeNavigator;
            DependentScripts = new List<ScriptModules>()
            {
                Blazor.Internal.ScriptModules.SvgBase,
                Blazor.Internal.ScriptModules.SfSvgExport
            };
        }

        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender">Specifies the first render of the component.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                SetAccFontKeys();
                ElementOffset = await InvokeMethod<DomRect>(RangeConstants.GETELEMENTBOUNDSBYID, false, new object[] { Id });
                if (ElementOffset != null)
                {
                    ElementOffset.Width = ChartHelper.StringToNumber(Width, ElementOffset.Width);
                    ElementOffset.Height = ChartHelper.StringToNumber(Height, ElementOffset.Height);
                    await GetCharSizeList();
                    await ComponentRender(true);
                }
            }

            await base.OnAfterRenderAsync(firstRender);
#if !NETSTANDARD
            await RenderTooltip();
#endif
        }

        private async Task GetCharSizeList()
        {
            List<string> uniqueKeys = new List<string>();
            foreach (string fontKey in fontKeys)
            {
                if (!chartFontKeys.Contains(fontKey))
                {
                    uniqueKeys.Add(fontKey);
                    chartFontKeys.Add(fontKey);
                }
            }

            if (uniqueKeys.Count == 0)
            {
                return;
            }

            string result = await InvokeMethod<string>("sfBlazor.getCharCollectionSize", false, new object[] { uniqueKeys });
            if (string.IsNullOrEmpty(result))
            {
                return;
            }

            List<Dictionary<string, SymbolLocation>> charSizeList = JsonSerializer.Deserialize<List<Dictionary<string, SymbolLocation>>>(result);
            for (int i = 0; i < charSizeList.Count; i++)
            {
                charSizeList[i].ToList().ForEach(keyValue => ChartHelper.sizePerCharacter.TryAdd(keyValue.Key + "_" + fontKeys[i], new Size { Width = keyValue.Value.X, Height = keyValue.Value.Y }));
            }
        }

        internal override async Task OnAfterScriptRendered()
        {
            if (!(Height.Contains("px", StringComparison.InvariantCulture) && Width.Contains("px", StringComparison.InvariantCulture)))
            {
                DomRect elementData = await InvokeMethod<DomRect>(RangeConstants.GETPARENTELEMENTBOUNDSBYID, false, new object[] { Id, DotnetObjectReference });
                if (elementData != null && AvailableSize == null)
                {
                    ElementOffset = elementData;
                    await ComponentRender();
                }
            }

#if !NET5_0
            await RenderTooltip();
#endif
        }

        private async Task RenderTooltip()
        {
            if (TooltipModule != null && Tooltip != null && Tooltip.Enable && Tooltip.DisplayMode == TooltipDisplayMode.Always)
            {
                await TooltipModule.RenderThumbTooltip(RangeSliderModule);
            }
        }

        private async Task ComponentRender(bool isCDNScript = false)
        {
            /*
             * In order to prevent background calculation processing even though component has been disposed of, the exceptions are treated by Try catch block for component disposal.
             * Ex: Quickly navigation between pages exception will throw, this has handled here.
             * This solution has suggested by MS blazor docs (https://docs.microsoft.com/en-us/aspnet/core/blazor/fundamentals/handle-errors?view=aspnetcore-3.1#component-disposal)
             */
            try
            {
                if (!isCDNScript)
                {
                    await GetCharSizeCollection();
                }

                CalculateAvailableSize();
                CalculateVisibleSeries();
                ThemeStyle = RangeNavigatorHelper.GetChartThemeStyle(Theme, this);
                await ProcessData();
            }
            catch
            {
                if (!IsDisposed)
                {
                    throw;
                }
            }
        }

        private async Task GetCharSizeCollection()
        {
            string result = await InvokeMethod<string>(RangeConstants.GETCHARSIZEBYFONT, false, new object[] { fontKeys });
            ChartHelper.sizePerCharacter = JsonSerializer.Deserialize<Dictionary<string, Size>>(result);
        }

        internal override void ComponentDispose()
        {
            base.ComponentDispose();
            TooltipModule?.Dispose();
            RangeAxisModule?.Dispose();
            DotnetObjectReference?.Dispose();
            PeriodSelectorModule?.Dispose();
            ChartHelper = null;
            InitialClipRect = null;
            ElementOffset = null;
            AvailableSize = null;
            VisibleSeries = null;
            Rendering = null;
            PrintExport = null;
            ThemeStyle = null;
            FinalData = null;
            RangeAxisModule = null;
            RangeSliderModule = null;
            TooltipModule = null;
            PeriodSelectorModule = null;
            RangeNavigatorEvents = null;
            Chart = null;
            XAxisRenderer = null;
            YAxisRenderer = null;
            ChartContent = null;
            PeriodSelectorContent = null;
            TooltipContent = null;
            SvgRenderer = null;
            RangeHelper = null;
            ChartSeries = null;
            Labels = null;
            MajorGridLines = null;
            MajorTickLines = null;
            Margin = null;
            LabelStyle = null;
            NavigatorBorder = null;
            NavigatorStyleSettings = null;
            PeriodSelectorSettings = null;
            Tooltip = null;
            Series = null;
            fontKeys = null;
            ChartContent = null;
            PeriodSelectorContent = null;
            TooltipContent = null;
            UnWireEvents();
        }

        internal bool IsRtlEnabled()
        {
            return EnableRtl || SyncfusionService.options.EnableRtl;
        }

        private void SetAccFontKeys()
        {
            List<string> keys = new List<string>();
            keys.Add(LabelStyle.GetFontKey());
            fontKeys = keys.Distinct().ToList();
        }

        private async void UnWireEvents()
        {
            if (IsRendered && !IsDisposed)
            {
                await InvokeMethod(RangeConstants.DESTROY, new object[] { Element });
            }
        }

        internal void UpdateChildProperties(string key, object keyValue)
        {
            switch (key)
            {
                case nameof(Series):
                    Series = (List<RangeNavigatorSeries>)keyValue;
                    break;
                case nameof(Tooltip):
                    Tooltip = (RangeNavigatorRangeTooltipSettings)keyValue;
                    break;
                case nameof(MajorGridLines):
                    MajorGridLines = (RangeNavigatorMajorGridLines)keyValue;
                    break;
                case nameof(MajorTickLines):
                    MajorTickLines = (RangeNavigatorMajorTickLines)keyValue;
                    break;
                case nameof(Margin):
                    Margin = (RangeNavigatorMargin)keyValue;
                    break;
                case nameof(LabelStyle):
                    LabelStyle = (RangeNavigatorLabelStyle)keyValue;
                    break;
                case nameof(NavigatorBorder):
                    NavigatorBorder = (RangeNavigatorBorder)keyValue;
                    break;
                case nameof(NavigatorStyleSettings):
                    NavigatorStyleSettings = (RangeNavigatorStyleSettings)keyValue;
                    break;
                case nameof(PeriodSelectorSettings):
                    PeriodSelectorSettings = (RangeNavigatorPeriodSelectorSettings)keyValue;
                    break;
            }
        }

        /// <summary>
        /// JS interopt while mouse down.
        /// </summary>
        /// <param name="args">Represents the mouse events.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task OnRangeMouseDown(ChartInternalMouseEventArgs args)
        {
            MouseDownX = args != null ? args.MouseX : 0;
            await RangeSliderModule?.MouseDownHandler(args?.Target);
        }

        /// <summary>
        /// JS interopt to set slider range.
        /// </summary>
        /// <param name="start">Represents the mouse start range.</param>
        /// <param name="end">Represents the mouse end range.</param>
        /// <param name="trigger">Represents the event state.</param>
        /// <param name="showTooltip">Represents the tooltip visibility state.</param>
        /// <exclude />
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task SetStartEndValue(double start, double end, bool trigger, bool showTooltip)
        {
            StartValue = start;
            EndValue = end;
            await UpdateValue(start, end);
            RangeSliderModule?.SetSlider(StartValue, EndValue, trigger, showTooltip);
        }

        internal async Task UpdateValue(double start, double end)
        {
            object newValue = null;
            IsValueUpdated = true;
            if (ValueType == RangeValueType.DateTime)
            {
                newValue = new DateTime[] { new DateTime(1970, 1, 1).AddMilliseconds(start), new DateTime(1970, 1, 1).AddMilliseconds(end) };
            }
            else
            {
                newValue = new double[] { start, end };
            }

            Value = await SfBaseUtils.UpdateProperty(newValue, value, ValueChanged);
            value = ValueChanged.HasDelegate ? Value : value;
            IsValueUpdated = false;
        }

        /// <summary>
        /// JS interopt while mouse move.
        /// </summary>
        /// <param name="args">Represents the mouse events.</param>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void OnRangeMouseMove(ChartInternalMouseEventArgs args)
        {
            MouseX = args != null ? args.MouseX : 0;
            RangeSliderModule?.MouseMoveHandler();
        }

        /// <summary>
        /// JS interopt while mouse end.
        /// </summary>
        /// <param name="args">Represents the mouse events.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task OnRangeMouseEnd(ChartInternalMouseEventArgs args)
        {
            MouseX = args != null ? args.MouseX : 0;
            await RangeSliderModule?.MouseUpHandler();
        }

        /// <summary>
        /// JS interopt while mouse click.
        /// </summary>
        /// <param name="args">Represents the mouse events.</param>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void OnRangeMouseClick(ChartInternalMouseEventArgs args)
        {
            CheckTouch(args ?? new ChartInternalMouseEventArgs());
            MouseClick?.Invoke(this, args);
        }

        /// <summary>
        /// JS interopt while mouse leave the component.
        /// </summary>
        /// <param name="args">Represents the mouse events.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task OnRangeMouseLeave(ChartInternalMouseEventArgs args)
        {
            if (RangeSliderModule != null && RangeSliderModule.IsDrag)
            {
                RangeSliderModule.TriggerEvent(ChartSeries.XAxisRenderer.ActualRange);
            }

            MouseX = args != null ? args.MouseX : 0;
            await RangeSliderModule?.MouseCancelHandler();
        }

        /// <summary>
        /// JS interopt while resize the window.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void OnRangeResize()
        {
            if (!IsStockChart)
            {
                ResizeChart();
            }
        }

        private void CheckTouch(ChartInternalMouseEventArgs args)
        {
            IsTouch = args.Type.Contains("touch", StringComparison.InvariantCulture) ? true : args.PointerType == "touch" || args.PointerType == "2";
        }

        protected override bool ShouldRender()
        {
            return IsStockChart ? ShouldSelectorRender.Invoke(Id) : true;
        }

        internal void CallStateHasChanged()
        {
            StateHasChanged();
        }
    }
}