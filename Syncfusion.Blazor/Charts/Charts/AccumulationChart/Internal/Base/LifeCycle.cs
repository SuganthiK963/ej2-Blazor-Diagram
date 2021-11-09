using Syncfusion.Blazor.Charts.AccumulationChart.Internal;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.Internal;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using Syncfusion.Blazor.Charts.Chart.Models;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    public partial class SfAccumulationChart : SfDataBoundComponent, IAccumulationChart
    {
        private bool isScriptLoaded;

        protected override async Task OnInitializedAsync()
        {
#pragma warning disable CA2007
            await base.OnInitializedAsync();
            if (string.IsNullOrEmpty(ID))
            {
                ID = SfBaseUtils.GenerateID("SfCharts");
            }

            ScriptModules = SfScriptModules.SfAccumulationChart;
            DependentScripts = new List<ScriptModules>() { Blazor.Internal.ScriptModules.SvgBase, Blazor.Internal.ScriptModules.SfSvgExport };
            AccChartThemeStyle = ChartHelper.GetChartThemeStyle(Theme.ToString());
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            await OnAccumulationChartParametersSet();
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
                await InvokeMethod<DomRect>(AccumulationChartConstants.INITIALIZE, false, new object[] { Element, DotnetObjectReference });
                DomRect elementOffset = await InvokeMethod<DomRect>(AccumulationChartConstants.GETPARENTELEMENTBOUNDSBYID, false, new object[] { ID });
                isScriptLoaded = true;
                if (elementOffset != ElementOffset && elementOffset != null)
                {
                    ElementOffset = elementOffset;
                    if (!(Height.Contains("px", StringComparison.InvariantCulture) && Width.Contains("px", StringComparison.InvariantCulture)))
                    {
                        Size previousSize = new Size(AvailableSize.Width, AvailableSize.Height);
                        SetContainerSize();
                        if (previousSize.Width != AvailableSize.Width || previousSize.Height != AvailableSize.Height)
                        {
                            Refresh();
                        }
                    }
                }

                await CalculateSecondaryElementPosition();
            }
            catch
            {
                if (!IsDisposed)
                {
                    throw;
                }
            }
        }

        private async Task GetContainerSize(string getElementBounds = AccumulationChartConstants.GETPARENTELEMENTBOUNDSBYID)
        {
            DomRect elementOffset = await InvokeMethod<DomRect>(getElementBounds, false, new object[] { ID });
            if (elementOffset != ElementOffset && elementOffset != null)
            {
                ElementOffset = elementOffset;
            }
        }

        private async Task GetOtherLanguageCharSize()
        {
            List<string> distinctKeys = new List<string>();
            foreach (AccumulationChartSeries series in Series)
            {
                series.Points?.ForEach(label => GetDistinctCharacter(label.Text, series.DataLabel.Font, distinctKeys));
                GetDistinctCharacter(series.Name, LegendSettings.TextStyle, distinctKeys);
            }
            await LoadCharacterDictionary(distinctKeys);
        }

        private static void GetDistinctCharacter(string text, ChartCommonFont font, List<string> distinctKeys)
        {
            if (!string.IsNullOrEmpty(text))
            {
                foreach (char character in text)
                {
                    string key = character + Constants.UNDERSCORE + font.FontWeight + Constants.UNDERSCORE + font.FontStyle + Constants.UNDERSCORE + font.FontFamily;
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
                return;
            string result = await InvokeMethod<string>(AccumulationChartConstants.GETCHARSIZEBYFONTKEYS, false, new object[] { distinctKeys });
            if (result == null)
                return;
            Dictionary<string, SymbolLocation> charSizeList = JsonSerializer.Deserialize<Dictionary<string, SymbolLocation>>(result);
            foreach (KeyValuePair<string, SymbolLocation> charSize in charSizeList)
            {
                ChartHelper.sizePerCharacter.TryAdd(charSize.Key, new Size { Width = charSize.Value.X, Height = charSize.Value.Y });
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await ProcessChart();
            }

            await base.OnAfterRenderAsync(firstRender);

            // After chart's element appended to DOM, explode and selections are processed, so this method included in this lifecycle.
            if (isScriptLoaded && ChartContent != null && Series != null)
            {
                DoAfterRender();
            }
        }

        private async Task ProcessChart()
        {
            /*
             * In order to prevent background calculation processing even though component has been disposed of, the exceptions are treated by Try catch block for component disposal.
             * Ex: Quickly navigation between pages exception will throw, this has handled here.
             * This solution has suggested by MS blazor docs (https://docs.microsoft.com/en-us/aspnet/core/blazor/fundamentals/handle-errors?view=aspnetcore-3.1#component-disposal)
             */
            try
            {
                await SetCharSize();
                if (!(Height.Contains("px", StringComparison.InvariantCulture) && Width.Contains("px", StringComparison.InvariantCulture)))
                {
                    string methodName = (!SyncfusionService.options.IgnoreScriptIsolation) ? AccumulationChartConstants.INITAILELEMENTBOUNDSBYID : AccumulationChartConstants.GETPARENTELEMENTBOUNDSBYID;
                    await GetContainerSize(methodName);
                }
                SetContainerSize();
                await RenderAccumulationChart();
            }
            catch
            {
                if (!IsDisposed)
                {
                    throw;
                }
            }
        }

        private async Task SetCharSize()
        {
            SetAccFontKeys();
            await GetCharSizeList(fontKeys);
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

            string methodName = (!SyncfusionService.options.IgnoreScriptIsolation) ? AccumulationChartConstants.GETCHARCOLLECTIONSIZE : AccumulationChartConstants.CHARTINTROP_GETCHARCOLLECTIONSIZE;
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

        private async Task RenderAccumulationChart()
        {
            NumberFormatter.NumberDecimalSeparator = ", ";
            NumberFormatter.NumberGroupSeparator = ".";
            await GetQueryValue();
            InitPrivateVariable();
            CalculateVisibleSeries();
            SetContainerSize();
            ProcessData();
        }

        private void InitPrivateVariable()
        {
            SvgId = ID + AccumulationChartConstants.SVG;
            SecondaryElementOffset = new DomRect() { Left = 0, Top = 0 };
        }

        private async Task GetQueryValue()
        {
            if (Query != null && series != null)
            {
                object data = await GenerateAndExecuteQuery();
                foreach (AccumulationChartSeries item in series)
                {
                    if (item.DataModule == null || item.DataModule.ToList().Count == 0)
                    {
                        item.DataModule = (IEnumerable<object>)data;
                    }
                }
            }
        }

        private async Task<object> GenerateAndExecuteQuery()
        {
            object data = null;
            if (DataManager != null)
            {
                data = await DataManager.ExecuteQuery<object>(Query);
            }

            return (object)data;
        }

        private async void UnWireEvents()
        {
            if (IsRendered && !IsDisposed)
            {
                await InvokeMethod(AccumulationChartConstants.DESTROY, new object[] { Element });
            }
#pragma warning restore CA2007
        }

        internal override void ComponentDispose()
        {
            // RenderFragments
            ChildContent = null;
            AnnotationContent = null;
            ChartContent = null;
            TooltipContent = null;
            TooltipBase = null;
            DatalabelTemplate = null;
            StyleFragment = null;
            TrimTooltip = null;
            Series.Clear();
            Annotations = null;
            LegendSettings = null;
            SelectedDataIndexes = null;
            Border = null;
            TitleStyle = null;
            SubTitleStyle = null;
            Margin = null;
            Center = null;
            Tooltip = null;

            // Dispose handling for internal objects
            Rendering?.Dispose();
            Rendering = null;
            AccBaseModule?.Dispose();
            AccBaseModule = null;
            PieSeriesModule?.Dispose();
            PieSeriesModule = null;
            FunnelSeriesModule?.Dispose();
            FunnelSeriesModule = null;
            PyramidSeriesModule?.Dispose();
            PyramidSeriesModule = null;
            DataLabelModule?.Dispose();
            DataLabelModule = null;
            AccumulationLegendModule?.Dispose();
            AccumulationLegendModule = null;
            AccumulationSelectionModule?.Dispose();
            AccumulationSelectionModule = null;
            AccumulationTooltipModule?.Dispose();
            AccumulationTooltipModule = null;
            UnWireEvents();
        }
    }
}