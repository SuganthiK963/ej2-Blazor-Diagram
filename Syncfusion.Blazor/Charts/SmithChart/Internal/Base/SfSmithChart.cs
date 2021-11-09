using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using Microsoft.JSInterop;
using System;
using Syncfusion.Blazor.Charts.SmithChart.Internal;
using Syncfusion.Blazor.Internal;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// The Blazor Smith Chart is a component for showing the parameters of transmission lines in high-frequency circuit applications.
    /// </summary>
    public partial class SfSmithChart : SfDataBoundComponent
    {
        private static List<string> chartFontKeys = new List<string>();
        private string background;
        private List<string> fontKeys = new List<string>();
        private double elementSpacing;
        private string height;
        private double radius;
        private RenderType renderType;
        private Theme theme;
        private string width;

        internal event EventHandler<SmithChartInternalMouseEventArgs> MouseMove;

        internal RenderFragment DatalabelTemplate { get; set; }

        internal RenderFragment TooltipContent { get; set; }

        internal RenderFragment ChartContent { get; set; }

        internal SmithChartEvents SmithChartEvents { get; set; }

        internal bool AnimateSeries { get; set; } = true;

        internal SmithChartBorder Border { get; set; } = new SmithChartBorder();

        internal SmithChartFont Font { get; set; } = new SmithChartFont();

        internal SmithChartHorizontalAxis HorizontalAxis { get; set; } = new SmithChartHorizontalAxis();

        internal SmithChartLegendSettings LegendSettings { get; set; } = new SmithChartLegendSettings();

        internal double MouseX { get; set; }

        internal double MouseY { get; set; }

        /// <summary>
        /// Gets and sets the ID for smith chart.
        /// </summary>
        [Parameter]
        public string ID { get; set; } = SfBaseUtils.GenerateID("SfSmithChart");

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Gets and sets the model type.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
        [Parameter]
        public Type ModelType { get; set; }

        /// <summary>
        /// Gets and sets the background color of the smith chart.
        /// </summary>
        [Parameter]
        public string Background { get; set; }

        /// <summary>
        /// Use to set space between elements.
        /// </summary>
        [Parameter]
        public double ElementSpacing { get; set; } = 10;

        /// <summary>
        /// Gets and sets to enable persistence of the smith chart.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
        [Parameter]
        public bool EnablePersistence { get; set; }

        /// <summary>
        /// Gets and sets right-to-left rendering of the smith chart.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
        [Parameter]
        public bool EnableRtl { get; set; }

        /// <summary>
        /// Gets and sets the height of the smith chart.
        /// </summary>
        [Parameter]
        public string Height { get; set; } = "100%";

        /// <summary>
        /// Gets and sets locale of the smith chart.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
        [Parameter]
        public string Locale { get; set; } = string.Empty;

        internal SmithChartMargin Margin { get; set; } = new SmithChartMargin();

        internal SmithChartRadialAxis RadialAxis { get; set; } = new SmithChartRadialAxis();

        internal List<SmithChartSeries> Series { get; set; } = new List<SmithChartSeries>();

        internal SmithChartTitle Title { get; set; } = new SmithChartTitle();

        /// <summary>
        /// Gets and sets the radius of the smith chart.
        /// </summary>
        [Parameter]
        public double Radius { get; set; } = 1;

        /// <summary>
        /// Gets and sets the render type of the smith chart.
        /// </summary>
        [Parameter]
        public RenderType RenderType { get; set; } = RenderType.Impedance;

        /// <summary>
        /// Gets and sets the theme for the smith chart.
        /// </summary>
        [Parameter]
        public Theme Theme { get; set; } = Theme.Bootstrap4;

        /// <summary>
        /// Gets and sets the width of the smith chart.
        /// </summary>
        [Parameter]
        public string Width { get; set; } = "100%";

        internal static void InvokeEvent<T>(object eventFn, T eventArgs)
        {
            if (eventFn != null)
            {
                var eventHandler = (Action<T>)eventFn;
                eventHandler.Invoke(eventArgs);
            }
        }

        internal void SetFontKeys()
        {
            List<string> keys = new List<string>();
            keys.Add(Title.TextStyle.GetFontKey());
            keys.Add(Title.Subtitle.TextStyle.GetFontKey());
            if (Series.Count > 0)
            {
                keys.Add(Series.First()?.Marker.DataLabel.TextStyle.GetFontKey());
            }

            keys.Add(LegendSettings.TextStyle.GetFontKey());
            fontKeys = keys.Distinct().ToList();
        }

        /// <summary>
        /// JS interopt while mouse move over the smith chart.
        /// </summary>
        /// <param name="args">Represents the mouse event arguments.</param>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void OnSmithChartMouseMove(SmithChartInternalMouseEventArgs args)
        {
            if (args != null)
            {
                MouseX = args.MouseX;
                MouseY = args.MouseY;
            }

            MouseMove?.Invoke(this, args);
            if (TooltipContent != null && IsTooltipEnabled)
            {
                TooltipModule?.MouseMoveHandler(args?.Target);
            }
        }

        /// <summary>
        /// JS interopt while mouse click on the smith chart.
        /// </summary>
        /// <param name="args">Represents the mouse event arguments.</param>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void OnSmithChartMouseClick(SmithChartInternalMouseEventArgs args)
        {
            SmithChartLegendModule?.Click(args ?? new SmithChartInternalMouseEventArgs() { });
        }

        /// <summary>
        /// JS interopt while mouse leave on the smith chart.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void OnSmithChartMouseLeave()
        {
            TooltipModule?.MouseLeaveHandler();
        }

        /// <summary>
        /// JS interopt while window resized.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void OnSmithChartResize()
        {
            ResizeSmithChart();
        }

        /// <summary>
        /// The method is used to render the smith chart again.
        /// </summary>
        /// <param name="isUpdateData">Specifies to update the smith chart data.</param>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async void Refresh(bool isUpdateData = true)
        {
            await RefreshAsync(isUpdateData);
        }

        /// <summary>
        /// The method is used to render the smith chart again.
        /// </summary>
        /// <param name="isUpdateData">Specifies to update the smith chart data.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task RefreshAsync(bool isUpdateData = true)
        {
            ChartContent = null;
            await InvokeAsync(StateHasChanged);
            if (isUpdateData)
            {
                UpdateData();
            }

            isResize = true;
            await SetContainerSize();
            ProcessData();
        }

        private async void UpdateData()
        {
            foreach (SmithChartSeries chartSeries in Series?.ToList())
            {
                chartSeries.CurrentViewData = await chartSeries.UpdatedSeriesData();
            }
        }

        private async void ResizeSmithChart()
        {
            AnimateSeries = false;
            Size previousSize = new Size(AvailableSize.Width, AvailableSize.Height);
            isResize = true;
            await SetContainerSize();
            SmithChartResizeEventArgs argsData = new SmithChartResizeEventArgs(SmithChartConstants.RESIZED, false, AvailableSize, previousSize);
            if (SmithChartEvents?.SizeChanged != null)
            {
                SmithChartEvents.SizeChanged.Invoke(argsData);
            }

            AvailableSize = argsData.Cancel ? argsData.PreviousSize : argsData.CurrentSize;
            ProcessData();
        }

        private void TriggerLoadedEvent()
        {
            InvokeEvent<SmithChartLoadedEventArgs>(SmithChartEvents?.Loaded, new SmithChartLoadedEventArgs { EventName = SmithChartConstants.LOADED });
        }
    }
}