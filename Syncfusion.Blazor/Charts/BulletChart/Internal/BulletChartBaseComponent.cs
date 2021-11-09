using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Charts.BulletChart.Internal;
using Syncfusion.Blazor.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    public partial class SfBulletChart<TValue>
    {
        private static List<string> chartFontKeys = new List<string>();
        private ElementReference element;
        private SizeInfo elementInfo;
        private string secondaryElementStyle = string.Empty;
        private StringComparison comparison = StringComparison.InvariantCulture;
        private CultureInfo culture = CultureInfo.InvariantCulture;
        private string svgHeight;
        private string svgWidth;
        private bool isRender;
        private string prevId = string.Empty;
        private bool resize;
        private List<string> fontKeys = new List<string>();
        private bool animationRender;

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
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            ScriptModules = SfScriptModules.SfBulletChart;
            await base.OnInitializedAsync();
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
                InstanceInitilize();
                elementInfo = await InvokeMethod<SizeInfo>("sfBlazor.getElementBoundsById", false, new object[] { ID });
                if (elementInfo != null)
                {
                    SetFontKeys();
                    await GetCharSizeList();
                    await ComponentRender(true);
                }
            }

            await base.OnAfterRenderAsync(firstRender);
            if(!firstRender && animationRender)
            {
                animationRender = false;
                await DoAnimationprocess();
            }

            if (isRender)
            {
                isRender = false;
                await ComponentRender();
            }
        }

        private static string GetFontKey(TextStyle style)
        {
            return style.FontWeight + '_' + style.FontStyle + '_' + style.FontFamily;
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

            List<Dictionary<string, Rect>> charSizeList = JsonSerializer.Deserialize<List<Dictionary<string, Rect>>>(result);
            for (int i = 0; i < charSizeList?.Count; i++)
            {
                charSizeList[i].ToList().ForEach(keyValue => BulletChartRender.SizePerCharacter.TryAdd(keyValue.Key + "_" + fontKeys[i], new SizeInfo { Width = keyValue.Value.X, Height = keyValue.Value.Y }));
            }
        }

        private void SetFontKeys()
        {
            List<string> keys = new List<string>();
            keys.Add(GetFontKey(Render.LabelStyle));
            keys.Add(GetFontKey(Render.TitleStyle));
            keys.Add(GetFontKey(Render.SubTitleStyle));
            keys.Add(GetFontKey(Render.CategoryStyle));
            keys.Add(GetFontKey(Render.DataLabelStyle));
            if (Tooltip != null && ChartToolTip != null)
            {
                keys.Add(GetFontKey(ChartToolTip.FontInfo));
            }

            fontKeys = keys.Distinct().ToList();
        }

        private void InstanceInitilize()
        {
            Helper = new BulletChartHelper();
            Style = BulletChartHelper.GetThemeStyle(Theme);
            Render = new BulletChartRender(this, Helper);
            ScaleRender = new BulletChartScaleRender(this, Render);
        }

        private async Task ComponentRender(bool isCDNScript = false)
        {
            if (elementInfo != null)
            {
                if (resize)
                {
                    await GetElementSize();
                    resize = false;
                }

                GetContainerSize(elementInfo.Height, elementInfo.Width, isCDNScript);
                Render?.DataProcessing(DataSource);
                Render?.RenderBulletChartBackground();
                await RenderChartElements();
                await SfBaseUtils.InvokeEvent<System.EventArgs>(Events?.Loaded, new System.EventArgs() { });
            }
        }

        internal async Task RenderChartElements()
        {
            Render?.FindRange();
            if (LegendSettings != null && LegendSettings.Visible && ChartLegend != null)
            {
                Render?.GetLegendOptions();
            }

            await Render?.CalculatePosition();
            Render?.RenderBulletElements();
            if (Tooltip != null)
            {
                await SetSecondaryElementPosition();
            }

            StateHasChanged();
        }

        private async Task SetSecondaryElementPosition()
        {
            SizeInfo secondaryInfo = await InvokeMethod<SizeInfo>("sfBlazor.BulletChart.getSecondaryElementPosition", false, new object[] { element });
            if (secondaryInfo != null)
            {
                secondaryElementStyle = "left:" + secondaryInfo.Width.ToString(culture) + "px;top:" + secondaryInfo.Height.ToString(culture) + "px;";
            }
        }

        internal override async Task OnAfterScriptRendered()
        {
            SizeInfo elementData = await InvokeMethod<SizeInfo>("sfBlazor.BulletChart.initialize", false, new object[] { element, Height, Width, DotnetObjectReference });
            elementInfo = elementData;
            if (elementData != null && AvailableSize == null)
            {
                await ComponentRender();
            }

            if (!SyncfusionService.options.IgnoreScriptIsolation)
            {
                await DoAnimationprocess();
            }
            else
            {
                animationRender = true;
            }
        }

        private async Task DoAnimationprocess()
        {
            if (Animation != null)
            {
                await DoValueBarAnimation();
                await DoTargetBarAnimation();
            }

            if (Tooltip != null)
            {
                await SetSecondaryElementPosition();
            }
        }

        private void GetContainerSize(double elementHeight, double elementWidth, bool isCDNScript = false)
        {
            double width = elementWidth > 0 ? elementWidth : 200,
            height = Orientation == OrientationType.Vertical ? 450 : TitlePosition == TextPosition.Left || TitlePosition == TextPosition.Right ? 83 : 126,
            availableHeight = Helper.StringToNumber(Height, elementHeight > 0 ? elementHeight : height, !isCDNScript),
            availableWidth = Helper.StringToNumber(Width, elementWidth > 0 ? elementWidth : width, !isCDNScript);
            AvailableSize = new SizeInfo() { Height = availableHeight.Equals(double.NaN) ? (!string.IsNullOrEmpty(Height) && elementHeight > 0) ? elementHeight : height : availableHeight, Width = availableWidth.Equals(double.NaN) ? elementWidth > 0 ? elementWidth : width : availableWidth };
            GetSvgStyle();
        }

        private void GetSvgStyle()
        {
            if (AvailableSize != null)
            {
                if (!string.IsNullOrEmpty(Height) && !string.IsNullOrEmpty(Width) && Width.Contains("px", comparison) && Height.Contains("px", comparison))
                {
                    svgHeight = AvailableSize.Height.ToString(culture) + "px";
                    svgWidth = AvailableSize.Width.ToString(culture) + "px";
                }
                else
                {
                    svgHeight = AvailableSize.Height.ToString(culture);
                    svgWidth = AvailableSize.Width.ToString(culture);
                }
            }
        }

        /// <summary>
        /// JS interopt while click the component.
        /// </summary>
        /// <param name="elementId">Represents the identification of the element.</param>
        /// <exclude />
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void TriggerClick(string elementId)
        {
            if (!string.IsNullOrEmpty(elementId) && elementId.Equals(ID + "_chart_legend_pageup", comparison))
            {
                ChartLegend.ChangePage(true);
            }
            else if (!string.IsNullOrEmpty(elementId) && elementId.Equals(ID + "_chart_legend_pagedown", comparison))
            {
                ChartLegend.ChangePage(false);
            }
        }

        /// <summary>
        /// JS interopt while mouse move.
        /// </summary>
        /// <param name="elementId">Represents the identification of the element.</param>
        /// <param name="mouseX">Represents the X axis of mouse.</param>
        /// <param name="mouseY">Represents the Y axis of mouse.</param>
        /// <exclude />
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task TriggerMouseMove(string elementId, double mouseX, double mouseY)
        {
            if (Tooltip != null && ChartToolTip != null && Tooltip.Enable && !string.IsNullOrEmpty(elementId) && (elementId.Contains(ID + "_FeatureMeasure_", comparison) || elementId.Contains(ID + "_ComparativeMeasure_", comparison)))
            {
                ChartToolTip.IsTooltipRender = true;
                await ChartToolTip.DisplayTooltip(elementId, mouseX, mouseY);
                await InvokeMethod("sfBlazor.BulletChart.updateElementOpacity", new object[] { elementId, false, !string.IsNullOrEmpty(prevId) && !prevId.Equals(elementId, comparison) ? prevId : string.Empty });
                prevId = elementId;
            }
            else if (ChartToolTip != null && ChartToolTip.IsTooltipRender && !string.IsNullOrEmpty(prevId))
            {
                ChartToolTip.RemoveTooltip();
                await InvokeMethod("sfBlazor.BulletChart.updateElementOpacity", new object[] { prevId, true });
                prevId = string.Empty;
            }
        }

        /// <summary>
        /// JS interopt while resize the window.
        /// </summary>
        /// <exclude />
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void TriggerResize()
        {
            resize = true;
            isRender = true;
            InvokeAsync(() => StateHasChanged());
        }

        internal async Task DoValueBarAnimation()
        {
            List<string> id = new List<string>();
            List<double> valueX = new List<double>();
            Dictionary<string, object> data = new Dictionary<string, object>() { { "duration", Animation.Duration }, { "delay", Animation.Delay } };
            for (int i = 0; i < ScaleRender?.FeatureBars?.Count; i++)
            {
                BarInfo featureBarInfo = ScaleRender.FeatureBars?[i];
                id.Add(featureBarInfo.ID);
                valueX.Add(featureBarInfo.X + (IsRtlEnabled() ? featureBarInfo.Width : 0));
            }

            data.Add("valueY", ScaleRender?.FeatureBars?[0].Height);
            await InvokeMethod("sfBlazor.BulletChart.doValueBarAnimation", new object[] { element, data, id, valueX });
        }

        private async Task DoTargetBarAnimation()
        {
            List<string> id = new List<string>();
            List<double> valueX = new List<double>();
            List<double> valueY = new List<double>();
            Dictionary<string, object> data = new Dictionary<string, object>() { { "duration", Animation.Duration }, { "delay", Animation.Delay } };
            for (int i = 0; i < ScaleRender?.RectMeasureBars?.Count; i++)
            {
                BarInfo measureBarInfo = ScaleRender?.RectMeasureBars?[i];
                id.Add(measureBarInfo.ID);
                valueX.Add(measureBarInfo.X1);
                valueY.Add((measureBarInfo.Y1 + measureBarInfo.Y2) / 2);
            }

            for (int i = 0; i < ScaleRender?.CircleMeasureBars?.Count; i++)
            {
                BarInfo measureBarInfo = ScaleRender?.CircleMeasureBars?[i];
                id.Add(measureBarInfo.ID);
                valueX.Add(measureBarInfo.X1);
                valueY.Add(measureBarInfo.Y1);
            }

            for (int i = 0; i < ScaleRender?.CrossMeasureBars?.Count; i++)
            {
                BarInfo measureBarInfo = ScaleRender?.CrossMeasureBars?[i];
                id.Add(measureBarInfo.ID);
                valueX.Add(measureBarInfo.X1);
                valueY.Add(measureBarInfo.Y1);
            }

            await InvokeMethod("sfBlazor.BulletChart.doTargetBarAnimation", new object[] { element, data, id, valueX, valueY });
        }

        private async Task GetElementSize()
        {
            elementInfo = await InvokeMethod<SizeInfo>("sfBlazor.BulletChart.getElementSize", false, new object[] { element, Height, width });
        }

        /// <summary>
        /// Specifies the rendering direction of the component.
        /// </summary>
        /// <returns>Returns bool value.</returns>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsRtlEnabled()
        {
            return EnableRtl || SyncfusionService.options.EnableRtl;
        }

        /// <summary>
        /// Specifies property binding manipulations.
        /// </summary>
        /// <param name="propertyChanges">Represents the properties.</param>
        /// <param name="parent">Represents the class belongs too.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task OnPropertyChanged(Dictionary<string, object> propertyChanges, string parent)
        {
            if (Render != null && AvailableSize != null && propertyChanges != null && !string.IsNullOrEmpty(parent) && IsRendered)
            {
                bool isRefresh = false;
                if (propertyChanges.ContainsKey("Interval") || propertyChanges.ContainsKey("LabelPosition") || propertyChanges.ContainsKey("Minimum") || propertyChanges.ContainsKey("Maximum") || propertyChanges.ContainsKey("MinorTicksPerInterval")
                    || propertyChanges.ContainsKey("OpposedPosition") || propertyChanges.ContainsKey("TickPosition") || propertyChanges.ContainsKey("TitlePosition") || parent.Equals(nameof(BulletChartMinorTickLines), comparison)
                    || parent.Equals(nameof(BulletChartMajorTickLines), comparison) || parent.Equals(nameof(BulletChartTitleStyle), comparison) || parent.Equals(nameof(BulletChartSubTitleStyle), comparison) || parent.Equals(nameof(BulletChartLabelStyle), comparison))
                {
                    if (parent.Equals(nameof(BulletChartTitleStyle), comparison))
                    {
                        Render.SetTitleStyle();
                    }

                    if (parent.Equals(nameof(BulletChartSubTitleStyle), comparison))
                    {
                        Render.SetSubTitleStyle();
                    }

                    if (parent.Equals(nameof(BulletChartLabelStyle), comparison))
                    {
                        Render.SetLabelStyle();
                    }

                    isRefresh = true;
                    await RenderChartElements();
                }

                if (parent.Equals(nameof(IBulletChart), comparison) && propertyChanges.ContainsKey("Theme"))
                {
                    Style = BulletChartHelper.GetThemeStyle(Theme);
                }

                if (parent.Equals(nameof(IBulletChart), comparison) && (propertyChanges.ContainsKey("Height") || propertyChanges.ContainsKey("Width")))
                {
                    await GetElementSize();
                    if (elementInfo != null)
                    {
                        GetContainerSize(elementInfo.Height, elementInfo.Width);
                    }
                }

                if (parent.Equals(nameof(BulletChartRange), comparison))
                {
                    if (propertyChanges.ContainsKey("Shape") && ChartLegend != null)
                    {
                        await ChartLegend.OnPropertyChanged(propertyChanges, nameof(BulletChartRange));
                        isRefresh = true;
                    }
                    else
                    {
                        ScaleRender.DrawScaleGroup();
                    }
                }

                if (parent.Equals(nameof(BulletChartDataLabelStyle), comparison))
                {
                    Render?.RenderDataLabel();
                }

                if (parent.Equals(nameof(BulletChartValueBorder), comparison))
                {
                    ScaleRender?.SetValueBorder();
                }

                if (parent.Equals(nameof(BulletChartCategoryLabelStyle), comparison))
                {
                    Render?.SetCategoryStyle();
                }

                if (parent.Equals(nameof(BulletChartBorder), comparison))
                {
                    Render.RenderBulletChartBackground();
                }

                if (parent.Equals(nameof(BulletChartLegendSettings), comparison))
                {
                    if (propertyChanges.ContainsKey("Background") || propertyChanges.ContainsKey("Opcacity"))
                    {
                        await ChartLegend?.OnPropertyChanged(propertyChanges, nameof(BulletChartLegendSettings));
                        isRefresh = true;
                    }
                }

                if (!isRefresh)
                {
                    isRender = true;
                    StateHasChanged();
                }
            }
        }

        internal override async void ComponentDispose()
        {
            UpdateObservableEvents(nameof(DataSource), DataSource, true);
            Render?.Dispose();
            ScaleRender?.Dispose();
            Animation = null;
            Border = null;
            Events = null;
            ChartLegend = null;
            ChartToolTip = null;
            CategoryLabelStyle = null;
            DataLabel = null;
            LabelStyle = null;
            LegendSettings = null;
            MajorTickLines = null;
            Margin = null;
            MinorTickLines = null;
            SubtitleStyle = null;
            TitleStyle = null;
            Tooltip = null;
            ValueBorder = null;
            Ranges = null;
            TargetTypes = null;
            ChildContent = null;
            DataSource = null;
            Helper = null;
            Render = null;
            AvailableSize = null;
            Style = null;
            ScaleRender = null;
            elementInfo = null;
            if (IsRendered && !IsDisposed)
            {
                await InvokeMethod("sfBlazor.BulletChart.destroy", new object[] { element });
            }
        }
    }
}