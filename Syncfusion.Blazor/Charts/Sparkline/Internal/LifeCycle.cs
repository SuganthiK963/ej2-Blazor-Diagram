using Syncfusion.Blazor.Internal;
using Syncfusion.Blazor.Sparkline.Internal;
using System.Threading.Tasks;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;

namespace Syncfusion.Blazor.Charts
{
    public partial class SfSparkline<TValue> : SfDataBoundComponent
    {
        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            ScriptModules = SfScriptModules.SfSparkline;
            ThemeStyle = SparklineHelper.GetStyle(Theme);
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
                elementInfo = await InvokeMethod<ElementInfo>("sfBlazor.getElementBoundsById", false, new object[] { ID });
                if (elementInfo != null)
                {
                    SetFontKeys();
                    await GetCharSizeList();
                    await ComponentRender(true);
                }
            }

            await base.OnAfterRenderAsync(firstRender);
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

            List<Dictionary<string, Location>> charSizeList = JsonSerializer.Deserialize<List<Dictionary<string, Location>>>(result);
            for (int i = 0; i < charSizeList.Count; i++)
            {
                charSizeList[i].ToList().ForEach(keyValue => SizePerCharacter.TryAdd(keyValue.Key + "_" + fontKeys[i], new RectInfo { Width = keyValue.Value.X, Height = keyValue.Value.Y }));
            }
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            type = NotifyPropertyChanges(nameof(Type), Type, type);
            palette = NotifyPropertyChanges(nameof(Palette), Palette, palette);
            height = NotifyPropertyChanges(nameof(Height), Height, height);
            width = NotifyPropertyChanges(nameof(Width), Width, width);
            query = NotifyPropertyChanges(nameof(Query), Query, query);
            dataSource = NotifyPropertyChanges(nameof(DataSource), DataSource, dataSource);
            UpdateObservableEvents(nameof(DataSource), DataSource);
            enableRtl = NotifyPropertyChanges(nameof(EnableRtl), EnableRtl, enableRtl);
            endPointColor = NotifyPropertyChanges(nameof(EndPointColor), EndPointColor, endPointColor);
            fill = NotifyPropertyChanges(nameof(Fill), Fill, fill);
            format = NotifyPropertyChanges(nameof(Format), Format, format);
            highPointColor = NotifyPropertyChanges(nameof(HighPointColor), HighPointColor, highPointColor);
            lineWidth = NotifyPropertyChanges(nameof(LineWidth), LineWidth, lineWidth);
            lowPointColor = NotifyPropertyChanges(nameof(LowPointColor), LowPointColor, lowPointColor);
            negativePointColor = NotifyPropertyChanges(nameof(NegativePointColor), NegativePointColor, negativePointColor);
            opacity = NotifyPropertyChanges(nameof(Opacity), Opacity, opacity);
            rangePadding = NotifyPropertyChanges(nameof(RangePadding), RangePadding, rangePadding);
            startPointColor = NotifyPropertyChanges(nameof(StartPointColor), StartPointColor, startPointColor);
            theme = NotifyPropertyChanges(nameof(Theme), Theme, theme);
            tiePointColor = NotifyPropertyChanges(nameof(TiePointColor), TiePointColor, tiePointColor);
            enableGroupingSeparator = NotifyPropertyChanges(nameof(EnableGroupingSeparator), EnableGroupingSeparator, enableGroupingSeparator);
            valueType = NotifyPropertyChanges(nameof(ValueType), ValueType, valueType);
            xname = NotifyPropertyChanges(nameof(XName), XName, xname);
            yname = NotifyPropertyChanges(nameof(YName), YName, yname);
            if (IsRendered && PropertyChanges.Any())
            {
                await OnPropertyChanged(PropertyChanges, nameof(SfSparkline<TValue>));
                PropertyChanges.Clear();
            }
        }

        private async Task ComponentRender(bool isCDNScript = false)
        {
            await ProcessData();
            CalculateSvgSize(isCDNScript);
        }

        internal override async Task OnAfterScriptRendered()
        {
            ElementInfo elementData = await InvokeMethod<ElementInfo>("sfBlazor.Sparkline.initialize", false, new object[] { element, DotnetObjectReference, Height, Width });
            elementInfo = elementData;
            if (elementData != null && AvailableSize == null)
            {
                await ComponentRender();
            }
        }

        internal override async void ComponentDispose()
        {
            base.ComponentDispose();
            UpdateObservableEvents(nameof(DataSource), DataSource, true);
            ChildContent = null;
            ChartContent = null;
            AvailableSize = null;
            ThemeStyle = null;
            Rendering = null;
            elementInfo = null;
            VisiblePoints = null;
            SparklineData = null;
            NumericPoints = null;
            DataPoints = null;
            DataSource = dataSource = null;
            Palette = palette = null;
            Query = query = null;
            RangeBandSettings = null;
            Trackline = null;
            Tooltip = null;
            MarkerSettings = null;
            AxisSettings = null;
            Border = null;
            ContainerArea = null;
            DataLabelSettings = null;
            Padding = null;
            Events = null;
            TooltipSettings = null;
            processedData = null;
            if (IsRendered && !IsDisposed)
            {
                await InvokeMethod("sfBlazor.Sparkline.destroy", new object[] { element });
            }
        }
    }
}