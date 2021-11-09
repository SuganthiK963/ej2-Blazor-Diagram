﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
﻿using Microsoft.JSInterop;
﻿using Syncfusion.Blazor.Charts.Chart.Internal;
﻿using Syncfusion.Blazor.Charts.Internal;
using Syncfusion.Blazor.DataVizCommon;
﻿using Syncfusion.Blazor.Internal;
﻿using System;
﻿using System.Collections.Generic;
﻿using System.Globalization;
﻿using System.Linq;
﻿using System.Text.Json;
﻿using System.Text.Json.Serialization;
﻿using System.Threading.Tasks;
﻿using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Syncfusion.Blazor.PivotView")]
namespace Syncfusion.Blazor.Charts
{
    internal interface ISubcomponentTracker
    {
        void PushSubcomponent();

        void PopSubcomponent();
    }

    public partial class SfChart : SfDataBoundComponent, ISubcomponentTracker
    {
        private int pendingParametersSetCount;
        private DateTime previousRequestTime = DateTime.MinValue;
        private bool isSizeSet;
        private Size availableSize = new Size(600, 450);
        private Rect initialRect;
        private string svgCursor = "auto";
        private Crosshair crosshairModule;
        private SvgPrintExport printExport;
        private List<ChartSelectedDataIndex> selectedDataIndexes;

        internal event EventHandler<ChartMouseWheelArgs> WheelEvent;

        internal event EventHandler<ChartInternalMouseEventArgs> MouseClick;

        internal event EventHandler<ChartInternalMouseEventArgs> MouseMove;

        internal event EventHandler<ChartInternalMouseEventArgs> MouseDown;

        internal event EventHandler<ChartInternalMouseEventArgs> MouseUp;

        internal event EventHandler<ChartInternalMouseEventArgs> MouseCancel;

        internal bool IsScriptLoaded { get; set; }

        internal bool Redraw { get; set; }

        internal bool IsChartFirstRender { get; set; }

        private LocaleProvider localeProvider { get; set; }

        [Inject]
        private ISyncfusionStringLocalizer localizer { get; set; }

        internal RenderFragment ScrollElement { get; set; }

        [Parameter]
        [JsonIgnore]
        public RenderFragment ChildContent { get; set; }

        internal ChartLegendRenderer LegendRenderer { get; set; }

        internal CustomLegendRenderer CustomLegendRenderer { get; set; }

        internal ChartSeriesRendererContainer SeriesContainer { get; set; }

        internal ChartAxisRendererContainer AxisContainer { get; set; }

        internal ChartAxisOutsideContainer AxisOutSideContainer { get; set; }

        internal ChartColumnRendererContainer ColumnContainer { get; set; }

        internal ChartRowRendererContainer RowContainer { get; set; }

        internal ChartBorderRenderer ChartBorderRenderer { get; set; }

        internal ChartAreaRenderer ChartAreaRenderer { get; set; }

        internal ChartTitleRenderer ChartTitleRenderer { get; set; }

        internal ChartAnnotationRendererContainer AnnotationContainer { get; set; }

        internal ChartStriplineBehindContainer StriplineBehindContainer { get; set; }

        internal ChartStriplineOverContainer StriplineOverContainer { get; set; }

        internal ChartIndicatorContainer IndicatorContainer { get; set; }

        internal ChartTrendlineContainer TrendlineContainer { get; set; }

        internal bool RequireInvertedAxis { get; set; }

        internal SvgRendering SvgRenderer { get; set; }

        internal ChartRenderer ChartRender { get; set; }

        internal List<ChartRenderer> Renderers { get; private set; } = new List<ChartRenderer>();

        // internal Queue<ChartRenderer> Renderer = new Queue<ChartRenderer>();
        internal List<DynamicPathAnimationOptions> PathAnimationElements { get; set; } = new List<DynamicPathAnimationOptions>();

        internal List<DynamicTextAnimationOptions> TextAnimationElements { get; set; } = new List<DynamicTextAnimationOptions>();

        internal List<DynamicRectAnimationOptions> RectAnimationElements { get; set; } = new List<DynamicRectAnimationOptions>();

        internal ChartAreaType ChartAreaType { get; set; }

        // This is the only field used to main the animation state for chart, Don't add any other bool. (AnimatedSeries && AnimateSeries)
        internal bool ShouldAnimateSeries { get; set; } = true;

        internal DataLabelTemplateContainer DatalabelTemplateContainer { get; set; }

        internal ChartThemeStyle ChartThemeStyle { get; set; }

        internal ChartMargin Margin { get; set; } = new ChartMargin();

        internal ChartZoomSettings ZoomSettings { get; set; } = new ChartZoomSettings();

        internal List<ChartSelectedDataIndex> SelectedDataIndexes { get; set; } = new List<ChartSelectedDataIndex>();

        internal DomRect SecondaryElementOffset { get; set; } = new DomRect();

        internal ChartEvents ChartEvents { get; set; }

        public ScrollbarContent ChartScrollBarContent { get; set; }

        internal ElementReference Element { get; set; }

        internal Zoom ZoomingModule { get; set; }

        internal DataEditing DataEditingModule { get; set; }

        internal Selection SelectionModule { get; set; }

        internal Highlight HighlightModule { get; set; }

        internal Browser Browser { get; set; }

        internal bool IsDoubleTap { get; set; }

        internal bool IsTouch { get; set; }

        internal bool DelayRedraw { get; set; }

        internal bool StartMove { get; set; }

        internal bool DisableTrackTooltip { get; set; }

        internal DomRect ElementOffset { get; set; } = new DomRect();

        internal StyleElement SelectionStyleInstance { get; set; }

        internal StyleElement HighlightStyleInstance { get; set; }

        internal double MouseDownX { get; set; }

        internal double MouseDownY { get; set; }

        internal double PreviousMouseMoveX { get; set; }

        internal double PreviousMouseMoveY { get; set; }

        internal double MouseX { get; set; }

        internal double MouseY { get; set; }

        internal bool IsChartDrag { get; set; }

        internal bool Render { get; set; } = true;

        internal Size AvailableSize
        {
            get
            {
                return availableSize;
            }

            set
            {
                if (availableSize != value)
                {
                    availableSize = value;
                    isSizeSet = true;
                }
            }
        }

        internal Rect InitialRect
        {
            get
            {
                return initialRect;
            }

            set
            {
                if (initialRect != value)
                {
                    initialRect = value;
                    isSizeSet = true;
                }
            }
        }

        internal ChartTooltipSettings Tooltip { get; set; } = new ChartTooltipSettings();

        internal ChartCrosshairSettings Crosshair { get; set; } = new ChartCrosshairSettings();

        internal ChartTooltip TooltipModule { get; set; }

        internal MarkerExplode MarkerExplode { get; set; }

        internal ChartTooltipComponent TemplateTooltip { get; set; }

        internal SvgAxisGroup CrossGroup { get; set; }

        internal TrimTooltipBase TrimTooltip { get; set; }

        internal List<IChartElement> StockChartAnnotations { get; set; } = new List<IChartElement>();

        internal List<ChartSeriesRenderer> VisibleSeriesRenderers { get; set; } = new List<ChartSeriesRenderer>();

        internal ElementReference SvgElement { get; set; }

        internal SvgSelectionRectCollection ParentRect { get; set; }

        internal DotNetObjectReference<object> ChartDotNetReference { get; set; }

        private async Task GetBrowserDeviceInfo()
        {
            Browser = await JSRuntimeExtensions.InvokeAsync<Browser>(JSRuntime, Constants.GETBROWSERDEVICEINFO, Array.Empty<object>());
            if (ZoomingModule != null)
            {
                ZoomingModule.Browser = Browser;
            }
        }

        internal string GetLocalizedLabel(string text)
        {
            return localeProvider.GetText(text);
        }

        internal async Task PerformLayout()
        {
            InitiAxis();
            if (isSizeSet && pendingParametersSetCount == 0)
            {
                ProcessData();

                await GetOtherLanguageCharSize();

                Rect initialClipRect = InitialRect;

                foreach (ChartRenderer renderer in Renderers)
                {
                    renderer.HandleChartSizeChange(initialClipRect);
                }

                IsChartFirstRender = true;
                ProcessRenderQueue();
            }
        }

        private void InitiAxis()
        {
            IsInvertedAxis();
            AxisContainer.AssignAxisToSeries(SeriesContainer.ElementsRequiredAxis);
            IndicatorContainer.AssignAxisToIndicator();
            TrendlineContainer.AssignAxisToTrendline();
            RowContainer.AssignAxisToRow();
            ColumnContainer.AssignAxisToColumn();
        }

        internal void IsInvertedAxis()
        {
            ChartSeries series = SeriesContainer.Elements.Count > 0 ? SeriesContainer.Elements[0] as ChartSeries : null;
            if (series != null)
            {
                bool isPloarAxis = (series.Type == ChartSeriesType.Polar) || (series.Type == ChartSeriesType.Radar);
                bool isBarTypeSeries = series.Type.ToString().Contains("Bar", StringComparison.InvariantCulture);
                RequireInvertedAxis = (isBarTypeSeries && !IsTransposed) || (!isBarTypeSeries && IsTransposed && !isPloarAxis);
            }
        }

        protected override void OnInitialized()
        {
            ScriptModules = SfScriptModules.SfChart;
            DependentScripts = new List<ScriptModules>() { Blazor.Internal.ScriptModules.SvgBase, Blazor.Internal.ScriptModules.SfSvgExport };
            SvgRenderer = new SvgRendering();
            localeProvider = new LocaleProvider(localizer, LocaleStrings.Chart);
            ChartThemeStyle = ChartHelper.GetChartThemeStyle(Theme.ToString());
            base.OnInitialized();
        }

        internal override async Task OnAfterScriptRendered()
        {
            /*
             * In order to prevent background calculation processing even though component has been disposed of, the exceptions are treated by Try catch block for component disposal.
             * Ex: Quickly navigation between pages exception will throw, this has handled here.
             * This solution has suggested by MS blazor docs (https://docs.microsoft.com/en-us/aspnet/core/blazor/fundamentals/handle-errors?view=aspnetcore-3.1#component-disposal)
             */
            try
            {
                IsScriptLoaded = true;
                if (!IsDimensionContainsPixel())
                {
                    DomRect elementOffset = await InvokeMethod<DomRect>(Constants.GETPARENTELEMENTBOUNDSBYID, false, new object[] { ID });
                    if (elementOffset != null)
                    {
                        ElementOffset = elementOffset;
                        Size previousSize = new Size(AvailableSize.Width, AvailableSize.Height);
                        CalculateAvailableSize();
                        SetSvgDimension(Constants.SETSVGDIMENSION);
                        if (previousSize.Width != AvailableSize.Width || previousSize.Height != AvailableSize.Height)
                        {
                            OnLayoutChange();
                        }
                    }
                }
                else
                {
                    SetSvgDimension(Constants.SETSVGDIMENSION);
                }

                CalculateSecondaryElementPosition();
                ChartDotNetReference = DotNetObjectReference.Create<object>(this);
                await InvokeMethod<bool>(Constants.INITIALIZE, false, new object[] { Element, ChartDotNetReference, ZoomSettings.EnableMouseWheelZooming, AxisContainer.isScrollSettingEnabled || ZoomSettings.EnableScrollbar });
                await GetBrowserDeviceInfo();
                UpdateDatalabelTemplate();
                await PerformAnimation();
                PerformSelection();
            }
            catch
            {
                if (!IsDisposed)
                {
                    throw;
                }
            }
        }

        private void InitModules()
        {
            if (ChartAreaType != ChartAreaType.PolarAxes && !AxisContainer.isScrollSettingEnabled && (ZoomSettings.EnableSelectionZooming || ZoomSettings.EnableMouseWheelZooming || ZoomSettings.EnablePinchZooming || ZoomSettings.EnablePan || ZoomSettings.EnableScrollbar))
            {
                ZoomingModule = new Zoom(this);
                if (ZoomSettings.EnableScrollbar)
                {
                    AxisContainer.ScrollbarModule = new Scrollbar(this);
                }
            }

            if (SelectionModule == null && SelectionMode != SelectionMode.None)
            {
                SelectionModule = new Selection(this);
            }

            if (HighlightModule == null && HighlightMode != HighlightMode.None)
            {
                HighlightModule = new Highlight(this);
            }

            if (DataEditingModule == null && ChartAreaType != ChartAreaType.PolarAxes)
            {
                DataEditingModule = new DataEditing(this);
            }

        }

        private string SetChartContainerStyle()
        {
            string chartStyle = string.Empty;
            bool disableScroll = ZoomSettings.EnableSelectionZooming || ZoomSettings.EnablePinchZooming ||
            SelectionMode != SelectionMode.None || Crosshair.Enable || HighlightMode != HighlightMode.None;
            chartStyle += "touch-action: " + (disableScroll ? "none;" : "element;");
            chartStyle += "-ms-touch-action: " + (disableScroll ? "none;" : "element;");
            chartStyle += " -ms-content-zooming: none; -ms-user-select: none; user-select: none; webkit-user-select: none; position: relative; display: block; overflow: hidden;";
            return chartStyle;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            /*
             * In order to prevent background calculation processing even though component has been disposed of, the exceptions are treated by Try catch block for component disposal.
             * Ex: Quickly navigation between pages exception will throw, this has handled here.
             * This solution has suggested by MS blazor docs (https://docs.microsoft.com/en-us/aspnet/core/blazor/fundamentals/handle-errors?view=aspnetcore-3.1#component-disposal)
             */
            try
            {
                if (!isSizeSet)
                {
                    await SetCharSize();
                    string methodName = (!SyncfusionService.options.IgnoreScriptIsolation) ? Constants.INITAILELEMENTBOUNDSBYID : Constants.GETPARENTELEMENTBOUNDSBYID;
                    await GetElementOffset(methodName);
                    SetContainerSize();
                    string svgMethodName = (!SyncfusionService.options.IgnoreScriptIsolation) ? Constants.INITIALSETSVGDIMENSION : Constants.SETSVGDIMENSION;
                    SetSvgDimension(svgMethodName);
                    await GetRemoteData();
                    InitPrivateVariable();
                    InitModules();
                    if (AxisContainer.Renderers.Count == 0 || SeriesContainer.Renderers.Count == 0 ||
                        ColumnContainer.Renderers.Count == 0 || RowContainer.Renderers.Count == 0)
                    {
                        RenderFrame();
                    }

                    await PerformLayout();
                    InitPrivateModules();
                    ApplyZoomkit();
                    TriggerLoadedEvent();
                }

                await base.OnAfterRenderAsync(firstRender);
                ChartScrollBarContent?.CallStateHasChanged();
            }
            catch
            {
                if (!IsDisposed)
                {
                    throw;
                }
            }
        }

        private void SetContainerSize()
        {
            CalculateAvailableSize();
            SetInitialRect();
        }

        private void SetInitialRect()
        {
            double borderWidth = ChartBorderRenderer.ChartBorder.Width;
            double width = AvailableSize.Width - (Margin.Left + Margin.Right + borderWidth);
            double height = AvailableSize.Height - (Margin.Top + Margin.Bottom + borderWidth + 0.25);
            InitialRect = new Rect() { X = Margin.Left, Y = Margin.Top, Width = width, Height = height };
        }

        internal static void InvokeEvent<T>(object eventFn, T eventArgs)
        {
            if (eventFn != null)
            {
                var eventHandler = (Action<T>)eventFn;
                eventHandler.Invoke(eventArgs);
            }
        }

        private void CalculateAvailableSize()
        {
            double height = Height != "null" ? ChartHelper.StringToNumber(Height, ElementOffset.Height) : 450;
            double width = Width != "null" ? ChartHelper.StringToNumber(Width, ElementOffset.Width) : 600;

            AvailableSize = new Size(width > 0 ? width : AvailableSize.Width, height > 0 ? height : AvailableSize.Height);
        }

        private async Task PerformAnimation()
        {
            List<InitialAnimationInfo> animationInfo = new List<InitialAnimationInfo>();
            SeriesContainer.PerformAnimation(animationInfo);
            IndicatorContainer.PerformAnimation(animationInfo);
            TrendlineContainer.PerformAnimation(animationInfo);
            await InvokeMethod(Constants.DOANIMATION, animationInfo);
            ShouldAnimateSeries = false;
        }

        internal async Task PerformRedrawAnimation()
        {
            const int UPDATETHRESHOLD = 100;
            if (Redraw && !IsDisposed && (previousRequestTime == DateTime.MinValue || (DateTime.Now - previousRequestTime).TotalMilliseconds > UPDATETHRESHOLD))
            {
                previousRequestTime = DateTime.Now;
                await Task.Delay(UPDATETHRESHOLD);
                Redraw = false;
                await InvokeMethod(Constants.DOREDRAWANIMATION, new object[] { PathAnimationElements, RectAnimationElements, TextAnimationElements });
            }
        }

        internal void RefreshRedrawElements()
        {
            if (!Redraw)
            {
                PathAnimationElements.Clear();
                TextAnimationElements.Clear();
                RectAnimationElements.Clear();
            }
        }

        private async Task PerformDelayAnimation()
        {
            const int UPDATETHRESHOLD = 100;
            if (ShouldAnimateSeries && !IsDisposed && (previousRequestTime == DateTime.MinValue || (DateTime.Now - previousRequestTime).TotalMilliseconds > UPDATETHRESHOLD))
            {
                previousRequestTime = DateTime.Now;
                await Task.Delay(UPDATETHRESHOLD);
                List<InitialAnimationInfo> animationInfo = new List<InitialAnimationInfo>();
                SeriesContainer.PerformAnimation(animationInfo);
                IndicatorContainer.PerformAnimation(animationInfo);
                TrendlineContainer.PerformAnimation(animationInfo);
                await InvokeMethod(Constants.DOANIMATION, animationInfo);
                ShouldAnimateSeries = false;
            }
        }

        private void PerformSelection()
        {
            if (SelectionModule != null)
            {
                selectedDataIndexes = new List<ChartSelectedDataIndex>();
                selectedDataIndexes = SelectionModule.SelectedDataIndexes.GetRange(0, SelectionModule.SelectedDataIndexes.Count);

                SelectionModule.InvokeSelection();
                BaseSelection.ApppendSelectionPattern();
            }

            if (HighlightModule != null)
            {
                HighlightModule.InvokeHighlight();
                BaseSelection.ApppendSelectionPattern();
            }

            if (selectedDataIndexes?.Count > 0)
            {
                SelectionModule.SelectedDataIndexes = selectedDataIndexes.GetRange(0, selectedDataIndexes.Count);
                SelectionModule.RedrawSelection(SelectionMode);
            }
        }

        private async Task SetCharSize()
        {
            await GetCharSizeList(SetFontKeys());
        }

        private List<string> SetFontKeys()
        {
            List<string> keys = new List<string>();
            keys.Add(ChartTitleRenderer.GetTitleFontKey());
            keys.Add(ChartTitleRenderer.GetSubTitleFontKey());
            foreach (KeyValuePair<string, ChartAxis> keyValue in AxisContainer.Axes)
            {
                ChartAxis axis = keyValue.Value;
                keys.Add(axis.TitleStyle.GetFontKey());
                keys.Add(axis.CrosshairTooltip.TextStyle.GetFontKey());
                axis.StripLines.ForEach(x => keys.Add(x.TextStyle.GetFontKey()));
            }

            if (LegendRenderer != null)
            {
                keys.Add(LegendRenderer.GetFontKey());
            }

            return keys.Distinct().ToList();
        }

        internal async Task GetCharSizeList(List<string> fontKeys)
        {
            List<string> uniqueKeys = new List<string>();
            foreach (string fontKey in fontKeys)
            {
                if (!ChartHelper.ChartFontKeys.Contains(fontKey))
                {
                    uniqueKeys.Add(fontKey);
                    ChartHelper.ChartFontKeys.Add(fontKey);
                }
            }

            if (uniqueKeys.Count == 0)
            {
                return;
            }

            string methodName = (!SyncfusionService.options.IgnoreScriptIsolation) ? Constants.GETCHARCOLLECTIONSIZE : Constants.CHARTINTROP_GETCHARCOLLECTIONSIZE;
            string result = await InvokeMethod<string>(methodName, false, new object[] { uniqueKeys });
            if (result == null)
            {
                return;
            }

            List<Dictionary<string, SymbolLocation>> charSizePairsList = JsonSerializer.Deserialize<List<Dictionary<string, SymbolLocation>>>(result);
            int i = 0;
            foreach (Dictionary<string, SymbolLocation> charSizePairs in charSizePairsList)
            {
                charSizePairs.ToList().ForEach(keyValue => ChartHelper.sizePerCharacter.TryAdd(keyValue.Key + Constants.UNDERSCORE + uniqueKeys[i], new Size { Width = keyValue.Value.X, Height = keyValue.Value.Y }));
                i++;
            }
        }

        private async Task GetElementOffset(string getElementBounds)
        {
            if (!IsDimensionContainsPixel())
            {
                DomRect elementRect = await InvokeMethod<DomRect>(getElementBounds, false, new object[] { ID });

                // To resolve stockchart size issue.
                if (elementRect != null)
                {
                    ElementOffset = elementRect;
                }
            }
        }

        private bool IsDimensionContainsPixel()
        {
            return !string.IsNullOrEmpty(Height) && Height.Contains("px", StringComparison.InvariantCulture) && !string.IsNullOrEmpty(Width) && Width.Contains("px", StringComparison.InvariantCulture);
        }

        private void RenderFrame()
        {
            Prerender();
        }

        private void Prerender()
        {
            SeriesContainer.Prerender();
            AxisContainer.Prerender();
            AxisOutSideContainer.Prerender();
            ColumnContainer.Prerender();
            RowContainer.Prerender();

            // TODO: Need to check and fix for template related features in stockchart.
            if (!IsStockChart)
            {
                NonDefaultContainer(AnnotationContainer);
            }

            NonDefaultContainer(IndicatorContainer);
        }

        private static void NonDefaultContainer(ChartRendererContainer container)
        {
            if (container.Elements.Count != 0)
            {
                container.Prerender();
            }
        }

        private void ProcessData()
        {
            SeriesContainer.ProcessData();
            TrendlineContainer.ProcessData();
            IndicatorContainer.ProcessData();
        }

        private void OnAxisLayoutChange()
        {
            SeriesContainer?.HandleLayoutChange();
        }

        private void ProcessRenderQueue()
        {
            foreach (ChartRenderer renderer in Renderers)
            {
                renderer.ProcessRenderQueue();
            }
        }

        internal void AddAxis(ChartAxis axis)
        {
            AxisContainer.AddElement(axis);
        }

        internal void AddColumn(ChartColumn column)
        {
            ColumnContainer.AddElement(column);
        }

        internal void RemoveColumn(ChartColumn column)
        {
            ColumnContainer.RemoveElement(column);
        }

        internal void AddRow(ChartRow row)
        {
            RowContainer.AddElement(row);
        }

        internal void RemoveRow(ChartRow row)
        {
            RowContainer.RemoveElement(row);
        }

        internal void RemoveAxis(ChartAxis axis)
        {
            AxisContainer.RemoveElement(axis);
        }

        internal void AddSeries(ChartSeries series)
        {
            ChartSeriesRenderer defaultRenderer = SeriesContainer.Renderers.Find(renderer => renderer.GetType().Equals(typeof(DefaultSeriesRenderer))) as ChartSeriesRenderer;
            if (defaultRenderer != null)
            {
                SeriesContainer.RemoveRenderer(defaultRenderer);
            }

            if (SeriesContainer.Elements.Count > 0 && !SeriesContainer.Elements.Contains(series))
            {
                bool isBar = (SeriesContainer.Elements[0] as ChartSeries).Type.ToString().Contains("Bar", StringComparison.InvariantCulture);
                bool isPolar = (SeriesContainer.Elements[0] as ChartSeries).Type == ChartSeriesType.Polar || (SeriesContainer.Elements[0] as ChartSeries).Type == ChartSeriesType.Radar;
                switch (series.Type)
                {
                    case ChartSeriesType.Bar:
                    case ChartSeriesType.StackingBar:
                    case ChartSeriesType.StackingBar100:
                        if (isBar)
                        {
                            SeriesContainer.AddElement(series);
                        }

                        break;
                    case ChartSeriesType.Polar:
                    case ChartSeriesType.Radar:
                        if (isPolar)
                        {
                            SeriesContainer.AddElement(series);
                        }

                        break;
                    default:
                        if (!(isPolar || isBar))
                        {
                            SeriesContainer.AddElement(series);
                        }

                        break;
                }
            }
            else if (!SeriesContainer.Elements.Contains(series))
            {
                SeriesContainer.AddElement(series);
            }
        }

        internal void RemoveSeries(ChartSeries series)
        {
            SeriesContainer.RemoveElement(series);
        }

        internal void InvalidateRender()
        {
            StateHasChanged();
        }

        internal ChartAreaType GetAreaType()
        {
            ChartAreaType = SeriesContainer.GetAreaType();
            return ChartAreaType;
        }

        internal void AddAnnotation(ChartAnnotation annotation)
        {
            if (AnnotationContainer != null)
            {
                AnnotationContainer.AddElement(annotation);
            }
            else
            {
                StockChartAnnotations.Add(annotation);
            }
        }

        internal void RemoveAnnotation(ChartAnnotation annotation)
        {
            if (AnnotationContainer != null)
            {
                AnnotationContainer.RemoveRenderer(annotation.Renderer);
                AnnotationContainer.RemoveElement(annotation);
                AnnotationContainer.InvalidateRenderer();
            }
            else
            {
                StockChartAnnotations.Remove(annotation);
            }
        }

        internal void AddStripline(ChartStripline stripline, ZIndex position)
        {
            if (position == ZIndex.Behind)
            {
                StriplineBehindContainer.AddElement(stripline);
            }
            else
            {
                StriplineOverContainer.AddElement(stripline);
            }
            if (IsScriptLoaded)
            {
                ProcessOnLayoutChange();
            }
        }

        internal void RemoveStripline(ChartStripline stripline, ZIndex position)
        {
            if (position == ZIndex.Behind)
            {
                StriplineBehindContainer.RemoveRenderer(stripline.Renderer);
                StriplineBehindContainer.RemoveElement(stripline);
            }
            else
            {
                StriplineOverContainer.RemoveRenderer(stripline.Renderer);
                StriplineOverContainer.RemoveElement(stripline);
            }
            DelayLayoutChange();
        }

        internal void AddIndicator(ChartIndicator indicator)
        {
            IndicatorContainer.AddElement(indicator);
        }

        internal void RemoveIndicator(ChartIndicator indicator)
        {
            IndicatorContainer.RemoveElement(indicator);
        }

        internal void AddTrendline(ChartTrendline trendline)
        {
            TrendlineContainer.AddElement(trendline);
        }

        internal async void DelayLayoutChange()
        {
            const int UPDATETHRESHOLD = 100;
            isLayoutChange = true;
            if (!IsDisposed && (previousRequestTime == DateTime.MinValue || (DateTime.Now - previousRequestTime).TotalMilliseconds > UPDATETHRESHOLD))
            {
                previousRequestTime = DateTime.Now;
                await Task.Delay(UPDATETHRESHOLD);
                OnLayoutChange();
                isLayoutChange = false;
            }
        }

        internal void OnLayoutChange()
        {
            if (isSizeSet && IsChartFirstRender)
            {
                UpdateRenderers();
                ApplyZoomkit();
                ChartScrollBarContent?.CallStateHasChanged();
            }
        }

        internal void UpdateRenderers()
        {
            SvgRenderer.RefreshElementList();
            RefreshRedrawElements();
            SetInitialRect();
            foreach (ChartRenderer renderer in Renderers)
            {
                renderer.HandleChartSizeChange(InitialRect);
            }

            foreach (ChartRenderer renderer in Renderers)
            {
                renderer.ProcessRenderQueue();
            }
        }

        internal string GetSvgId()
        {
            return ID + Constants.SVG;
        }

        private string GetSvgHeight()
        {
            return AvailableSize != null ? Convert.ToString(AvailableSize.Height, CultureInfo.InvariantCulture) + "px" : "0px";
        }

        /// <summary>
        /// The method is used to get the container div direction.
        /// </summary>
        private string GetDirection()
        {
            return EnableRTL ? Constants.RTL : string.Empty;
        }

        private async void UpdateDatalabelTemplate()
        {
            List<string> templateIdCollection = new List<string>();
            GetSetDataLabelTemplateInfo(templateIdCollection, null);
            string result = await InvokeMethod<string>(Constants.GETDATALABELTEMPLATEBOUNDSBYID, false, new object[] { templateIdCollection });
            List<SymbolLocation> templateSizeList = JsonSerializer.Deserialize<List<SymbolLocation>>(result);
            GetSetDataLabelTemplateInfo(null, templateSizeList);
            RenderDatalabelTemplate();
        }

        private void RenderDatalabelTemplate()
        {
            foreach (ChartSeries series in SeriesContainer?.Elements)
            {
                series.Marker.DataLabel.Renderer?.UpdateDatalabelTemplatePosition();
            }
        }

        /// <summary>
        /// The method is used to get the collection of datalabel template id and set the corresponding template size
        /// It differs by passing parameters.
        /// </summary>
        private void GetSetDataLabelTemplateInfo(List<string> templateIdCollection, List<SymbolLocation> templateSizeList)
        {
            int j = 0;
            foreach (ChartSeriesRenderer series in SeriesContainer.Renderers)
            {
                if (series.Series.Visible)
                {
                    foreach (Point point in series.Points)
                    {
                        for (int i = 0; i < point.TemplateID.Count; i++)
                        {
                            if (templateIdCollection != null)
                            {
                                templateIdCollection.Add(point.TemplateID[i]);
                            }
                            else
                            {
                                point.TemplateSize.Add(new Size(templateSizeList[j].X, templateSizeList[j].Y));
                                j++;
                            }
                        }
                    }
                }
            }
        }

        private async void CalculateSecondaryElementPosition()
        {
            DomRect svgOffset = await InvokeMethod<DomRect>(Constants.GETELEMENTBOUNDSBYID, false, new object[] { SvgId(), false });
            DomRect elementOffset = await InvokeMethod<DomRect>(Constants.GETELEMENTBOUNDSBYID, false, new object[] { ID });
            if (svgOffset == null || elementOffset == null)
            {
                return;
            }

            SecondaryElementOffset.Left = Math.Max(svgOffset.Left - elementOffset.Left, 0);
            SecondaryElementOffset.Top = Math.Max(svgOffset.Top - elementOffset.Top, 0);
        }

        private string GetSvgWidth()
        {
            return AvailableSize != null ? Convert.ToString(AvailableSize.Width, CultureInfo.InvariantCulture) + "px" : "0px";
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
                RenderFrame();
            }
        }

        private async Task GetRemoteData()
        {
            if (DataManager == null)
            {
                return;
            }

            object data = await GenerateAndExecuteQuery((Query != null) ? Query : new Data.Query());
            SeriesContainer.Data = data != null ? (IEnumerable<object>)data : SeriesContainer.Data;
        }

        private async Task<object> GenerateAndExecuteQuery(Data.Query query)
        {
            object data = null;
            data = await DataManager.ExecuteQuery<object>(query);
            return (object)data;
        }

        internal void SetSvgCursor(string cursor)
        {
            svgCursor = cursor;
            SetAttribute(SvgId(), "cursor", cursor);
        }

        internal string SvgId()
        {
            return IsStockChart ? Constants.STOCKCHART_ID : ID + Constants.SVG;
        }

        internal async void SetAttribute(string id, string key, string data)
        {
            await InvokeMethod(Constants.SETATTRIBUTE, new object[] { id, key, data });
        }

        private void ApplyZoomkit()
        {
            if (ChartAreaType != ChartAreaType.PolarAxes && !Redraw && ZoomingModule != null && (!ZoomSettings.EnablePan || ZoomingModule.PerformedUI))
            {
                ZoomingModule.ApplyZoomToolkit(this, AxisContainer.Renderers);
            }
        }

        private void InitPrivateVariable()
        {
            printExport = new SvgPrintExport(JSRuntime);
        }

        private void InitPrivateModules()
        {
            MarkerExplode = new MarkerExplode(this);
            if (Tooltip.Enable)
            {
                TooltipModule = new ChartTooltip(this);
            }

            InitCrossHair();
        }

        private void InitCrossHair()
        {
            if (ChartAreaType != ChartAreaType.PolarAxes && Crosshair.Enable)
            {
                crosshairModule = new Crosshair(this);
            }
            else if (Crosshair.Enable && crosshairModule != null)
            {
                crosshairModule = null;
            }
        }

        private void TriggerLoadedEvent()
        {
            InvokeEvent<LoadedEventArgs>(ChartEvents?.Loaded, new LoadedEventArgs { Chart = this, Name = Constants.LOADED });
        }

        internal bool ChartDisposed()
        {
            return IsDisposed || IsRendererDisposed();
        }

        private bool IsRendererDisposed()
        {
            const string RENDERHANDLE = "_renderHandle";
            const string RENDERER = "_renderer";
            const string DISPOSED = "_disposed";

            // TODO: This solution has been provided for <BLAZ-9697> and <Incident 310842>. Since reflection is used, in future we may need to check the performance and clean this via alternate solution.
            FieldInfo field = GetType().BaseType.BaseType.BaseType.BaseType.GetField(RENDERHANDLE, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            RenderHandle renderHandler = (RenderHandle)field.GetValue(this);
            FieldInfo rendererInfo = renderHandler.GetType().GetField(RENDERER, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            object renderer = rendererInfo.GetValue(renderHandler);
            FieldInfo diposedInfo = renderer.GetType().BaseType.GetField(DISPOSED, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            return diposedInfo != null && (bool)diposedInfo.GetValue(renderer);
        }

        internal async void ProcessOnLayoutChange()
        {
            const int UPDATETHRESHOLD = 100;
            isLayoutChange = true;
            if (!IsDisposed && (previousRequestTime == DateTime.MinValue || (DateTime.Now - previousRequestTime).TotalMilliseconds > UPDATETHRESHOLD))
            {
                previousRequestTime = DateTime.Now;
                await Task.Delay(UPDATETHRESHOLD);
                Prerender();
                RefreshChart();
                ApplyZoomkit();
                ChartScrollBarContent?.CallStateHasChanged();
                isLayoutChange = false;
            }
        }

        private void RefreshChart()
        {
            InitiAxis();
            SeriesContainer.InitSeriesRendererFields();
            ProcessData();
            UpdateRenderers();
        }

        private async void UpdateData()
        {
            foreach (ChartSeriesRenderer seriesRenderer in SeriesContainer.Renderers)
            {
                await seriesRenderer.Series.UpdateSeriesData();
            }
        }

        private async Task GetOtherLanguageCharSize()
        {
            List<string> distinctKeys = new List<string>();
            GetDistinctCharacter(Title, ChartTitleRenderer.TitleStyle, distinctKeys);
            GetDistinctCharacter(SubTitle, ChartTitleRenderer.SubTitleStyle, distinctKeys);
            foreach (KeyValuePair<string, ChartAxis> keyValue in AxisContainer.Axes)
            {
                ChartAxis axis = keyValue.Value;
                GetDistinctCharacter(axis.Title, axis.TitleStyle, distinctKeys);
                axis.StripLines.ForEach(x => GetDistinctCharacter(x.Text, x.TextStyle, distinctKeys));
            }

            foreach (ChartSeriesRenderer seriesRenderer in SeriesContainer.Renderers)
            {
                if (seriesRenderer.XAxisRenderer.Axis.ValueType == ValueType.Category)
                {
                   seriesRenderer.XAxisRenderer.Labels.ForEach(label => GetDistinctCharacter(label, seriesRenderer.XAxisRenderer.Axis.LabelStyle, distinctKeys));
                }

                if (LegendRenderer != null)
                {
                    GetDistinctCharacter(seriesRenderer.Series.Name, LegendRenderer.LegendSettings.TextStyle, distinctKeys);
                }
            }

            await LoadCharacterDictionary(distinctKeys);
        }

        private async Task GetSeriesDistinctCharacter()
        {
            List<string> distinctKeys = new List<string>();
            foreach (ChartSeriesRenderer seriesRenderer in SeriesContainer.Renderers)
            {
                if (seriesRenderer.XAxisRenderer.Axis.ValueType == ValueType.Category)
                {
                    seriesRenderer.XAxisRenderer.Labels.ForEach(label => GetDistinctCharacter(label, seriesRenderer.XAxisRenderer.Axis.LabelStyle, distinctKeys));
                }

                GetDistinctCharacter(seriesRenderer.Series.Name, LegendRenderer.LegendSettings.TextStyle, distinctKeys);
            }

            await LoadCharacterDictionary(distinctKeys);
        }

        private static void GetDistinctCharacter(string text, ChartDefaultFont font, List<string> distinctKeys)
        {
            string key;
            if (ChartHelper.IsRTLText(text))
            {
                key = text + Constants.UNDERSCORE + font.FontWeight + Constants.UNDERSCORE + font.FontStyle + Constants.UNDERSCORE + font.FontFamily;
                if (!ChartHelper.sizePerCharacter.ContainsKey(key) && !ChartHelper.ChartFontKeys.Contains(key))
                {
                    distinctKeys.Add(key);
                    ChartHelper.ChartFontKeys.Add(key);
                }
            }
            else
            {
                foreach (char character in text)
                {
                    key = character + Constants.UNDERSCORE + font.FontWeight + Constants.UNDERSCORE + font.FontStyle + Constants.UNDERSCORE + font.FontFamily;
                    if (!ChartHelper.sizePerCharacter.ContainsKey(key) && !ChartHelper.ChartFontKeys.Contains(key) && !ChartHelper.Font.Chars.ContainsKey(character))
                    {
                        distinctKeys.Add(key);
                        ChartHelper.ChartFontKeys.Add(key);
                    }
                }
            }
        }

        private async Task LoadCharacterDictionary(List<string> distinctKeys)
        {
            if (distinctKeys.Count == 0)
            {
                return;
            }

            string methodName = (!SyncfusionService.options.IgnoreScriptIsolation) ? Constants.GETCHARSIZEBYFONTKEYS : Constants.CHARTINTROP_GETCHARSIZEBYFONTKEYS;
            string result = await InvokeMethod<string>(methodName, false, new object[] { distinctKeys });
            if (result == null)
            {
                return;
            }

            Dictionary<string, SymbolLocation> charSizeList = JsonSerializer.Deserialize<Dictionary<string, SymbolLocation>>(result);
            foreach (KeyValuePair<string, SymbolLocation> charSize in charSizeList)
            {
                ChartHelper.sizePerCharacter.TryAdd(charSize.Key, new Size { Width = charSize.Value.X, Height = charSize.Value.Y });
            }
        }

        private async void SetSvgDimension(string methodName)
        {
            await InvokeMethod(methodName, new object[] { SvgElement, AvailableSize.Width.ToString(CultureInfo.InvariantCulture) + "px", AvailableSize.Height.ToString(CultureInfo.InvariantCulture) + "px" });
        }

        internal override void ComponentDispose()
        {
            UnWireEvents();
        }

        private async void UnWireEvents()
        {
            if (!IsDisposed)
            {
                await InvokeMethod(Constants.DESTROY, new object[] { Element });
            }
        }
    }

    public class ChartDataBoundComponent : SfDataBoundComponent, ISubcomponentTracker
    {
        private bool isPush = true;
        private bool isPop = true;

        [CascadingParameter]
        internal ISubcomponentTracker Tracker { get; set; }

        [Parameter]
        [JsonIgnore]
        public RenderFragment ChildContent { get; set; }

        public void PopSubcomponent()
        {
            if (isPop)
            {
                isPop = false;
                Tracker?.PopSubcomponent();
            }
        }

        public void PushSubcomponent()
        {
            if (isPush)
            {
                isPush = false;
                Tracker?.PushSubcomponent();
            }
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            PushSubcomponent();
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (builder == null)
            {
                return;
            }

            this.CreateCascadingValue(
                builder,
                0,
                1,
                this,
                2,
                (builder2) =>
                {
                    if (ChildContent != null)
                    {
                        ChildContent(builder2);
                    }
                });

            this.CreateCascadingValue(
                builder,
                3,
                4,
                this,
                5,
                (builder2) =>
                {
                    PopSubcomponent();
                });
        }
    }

    public abstract class ChartSubComponent : OwningComponentBase, ISubcomponentTracker
    {
        private bool isPush = true;
        private bool isPop = true;

        internal bool IsPropertyChanged;

        [CascadingParameter]
        internal ISubcomponentTracker Tracker { get; set; }

        [Parameter]
        [JsonIgnore]
        public RenderFragment ChildContent { get; set; }

        public void PopSubcomponent()
        {
            if (isPop)
            {
                isPop = false;
                Tracker?.PopSubcomponent();
            }
        }

        public void PushSubcomponent()
        {
            if (isPush)
            {
                isPush = false;
                Tracker?.PushSubcomponent();
            }
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            PushSubcomponent();
        }

        internal virtual void ComponentDispose()
        {
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(true);
            if (disposing)
            {
                ComponentDispose();
            }
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (builder == null)
            {
                return;
            }

            this.CreateCascadingValue(
                builder,
                0,
                1,
                this,
                2,
                (builder2) =>
                {
                    if (ChildContent != null)
                    {
                        ChildContent(builder2);
                    }
                });

            this.CreateCascadingValue(
                builder,
                3,
                4,
                this,
                5,
                (builder2) =>
                {
                    PopSubcomponent();
                });
        }
    }
}
