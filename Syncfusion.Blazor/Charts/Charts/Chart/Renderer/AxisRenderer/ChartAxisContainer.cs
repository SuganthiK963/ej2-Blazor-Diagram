using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.DataVizCommon;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts.Internal
{

    public interface IRequireSeries
    {
        ChartSeriesRenderer Series { get; set; }

        void OnSeriesChanged();
    }

    public class ChartAxisRendererContainer : ChartRendererContainer
    {
        private int axisIndex;

        private Rect availableRect;

        internal List<IRequireSeries> ElementsRequiredSeries = new List<IRequireSeries>();

        private AxisLayout axisLayout;

        private bool needsLayout = true;

        internal Dictionary<string, ChartAxis> Axes { get; set; } = new Dictionary<string, ChartAxis>();

        internal Dictionary<string, ChartAxis> HorizontalAxes = new Dictionary<string, ChartAxis>();

        internal Dictionary<string, ChartAxis> VerticalAxes = new Dictionary<string, ChartAxis>();

        internal bool IsScrolling { get; set; }

        internal bool IsScrollExist { get; set; }

        internal List<ChartAxis> ScrollbarAxes { get; set; } = new List<ChartAxis>();

        internal Scrollbar ScrollbarModule { get; set; }

        internal bool isScrollSettingEnabled { get; set; }

        internal AxisLayout AxisLayout
        {
            get
            {
                return axisLayout;
            }

            set
            {
                if (axisLayout != value)
                {
                    needsLayout = true;
                    if (axisLayout != null)
                    {
                        axisLayout.ClearAxes();
                    }

                    axisLayout = value;

                    if (axisLayout != null)
                    {
                        foreach (ChartAxis axis in Axes.Values)
                        {
                            axisLayout.AddAxis(axis);
                        }
                    }

                    axisLayout.Chart = Owner;
                }
            }
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            AddToRenderQueue(this);
            Owner.AxisContainer = this;
            SvgRenderer = Owner.SvgRenderer;
        }

        protected override bool ShouldRender()
        {
            return RendererShouldRender || ContainerUpdate;
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
        }

        protected override void OnElementAdded(IChartElement element)
        {
            if (element != null)
            {
                ChartAxis axis = element as ChartAxis;
                if (!Axes.Values.Contains(axis))
                {
                    Axes[axis.GetName()] = axis;
                    AxisLayout?.AddAxis(axis);
                }

                if (Owner.InitialRect != null)
                {
                    StateHasChanged();
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "<Pending>")]
        public override void AddRenderer(IChartElementRenderer renderer)
        {
            if (renderer != null && !Renderers.Contains(renderer))
            {
                ContainerPrerender = false;
                RendererShouldRender = true;
                Renderers.Add(renderer);
                int index = Renderers.IndexOf(renderer);
                int defaultRendererCount = Renderers.FindAll(renderer => (renderer as ChartAxisRenderer).IsDefaultRenderer()).Count;
                if (renderer.GetType().Equals(typeof(ParetoAxisRenderer)))
                {
                    ChartAxis axis = InitAxis();
                    OnElementAdded(axis);
                    OnRendererAdded(renderer, axis);
                }
                else if (renderer.GetType().Equals(typeof(PrimaryXAxisRenderer)))
                {
                    ChartAxis axis = new ChartPrimaryXAxis();
                    OnElementAdded(axis);
                    OnRendererAdded(renderer, axis);
                }
                else if (renderer.GetType().Equals(typeof(PrimaryYAxisRenderer)))
                {
                    ChartAxis axis = new ChartPrimaryYAxis();
                    OnElementAdded(axis);
                    OnRendererAdded(renderer, axis);
                }
                else
                {
                    OnRendererAdded(renderer, Elements[index - defaultRendererCount]);
                }
            }
        }

        protected override void OnRendererAdded(IChartElementRenderer renderer, IChartElement element)
        {
            if (renderer is IRequireSeries)
            {
                ElementsRequiredSeries.Add(renderer as IRequireSeries);
            }

            ChartAxisRenderer axisRenderer = renderer as ChartAxisRenderer;
            if (axisRenderer != null)
            {
                axisRenderer.Index = axisIndex++;
                axisRenderer.Axis = element as ChartAxis;
                axisRenderer.Axis.Renderer = axisRenderer;
                if (axisRenderer.Axis.Container == null)
                {
                    axisRenderer.Axis.Container = Owner;
                }

                if (Owner.InitialRect != null)
                {
                    needsLayout = true;
                    Owner.ProcessOnLayoutChange();
                }
            }
        }

        protected override void OnRendererRemoved(IChartElementRenderer renderer)
        {
            if (!IsDisposed)
            {
                Owner.ProcessOnLayoutChange();
            }
        }

        private static ChartAxis InitAxis()
        {
            ChartAxis newAxis = new ChartAxis
            {
#pragma warning disable BL0005
                Name = "SecondaryAxis",
                MajorGridLines = new ChartAxisMajorGridLines { Width = 0 },
                MajorTickLines = new ChartAxisMajorTickLines { Width = 0 },
                LineStyle = new ChartAxisLineStyle { Width = 0 },
                Minimum = 0,
                Maximum = 100,
                RowIndex = 0,
                OpposedPosition = true,
                RendererType = typeof(NumericAxisRenderer),
                LabelFormat = "{value}%"
#pragma warning restore BL0005
            };

            return newAxis;
        }

        protected override void OnElementRemoved(IChartElement element)
        {
            if (element != null)
            {
                ChartAxis axis = element as ChartAxis;
                RemoveRenderer(axis.Renderer);
                Owner.AxisOutSideContainer.RemoveRenderer(axis.Renderer?.OutSideRenderer);
                if (Axes.Values.Contains(axis))
                {
                    Axes.Remove(axis.GetName());
                }

                if (!Owner.ChartDisposed())
                {
                    StateHasChanged();
                }
            }
        }

        internal void RemoveParetoAxis(ChartAxis axis)
        {
            RemoveRenderer(axis.Renderer);
            Owner.AxisOutSideContainer.RemoveRenderer(axis.Renderer.OutSideRenderer);
            Axes.Remove(axis.GetName());
        }

        public override void HandleChartSizeChange(Rect rect)
        {
            if (availableRect != rect || needsLayout)
            {
                CalculateScrollbar();
                ComputePlotAreaBounds(rect);
                availableRect = rect;
                /* availableSize = newSize;
                 ComputePlotAreaBounds(newSize);
                 if (plotAreaSize != AxisLayout.PlotAreaSize || offset != AxisLayout.Offset)
                 {
                     plotAreaSize = AxisLayout.PlotAreaSize;
                     offset = AxisLayout.Offset;
                 }
                 needsLayout = false;
                 availableSize = newSize;*/
                RendererShouldRender = true;
            }
        }

        private void CalculateScrollbar()
        {
            ScrollbarAxes.Clear();
            foreach (ChartAxisRenderer axisRender in Renderers)
            {
                if (ScrollbarModule == null && (axisRender.Axis.ScrollbarSettings.Enable || axisRender.Axis.EnableScrollbarOnZooming))
                {
                    isScrollSettingEnabled = true;
                    ScrollbarModule = new Scrollbar(Owner);
                    Owner.ScrollElement = Owner.ScrollElement == null ? CreateScrollbarDiv() : Owner.ScrollElement;
                    break;
                }
            }

            if (isScrollSettingEnabled && ScrollbarModule != null)
            {
                ScrollbarModule.Axes = Axes;
            }

            foreach (ChartAxisRenderer axisRender in Renderers)
            {
                axisRender.Series = new List<ChartSeries>();

                if (ScrollbarModule != null && axisRender.ZoomingScrollBar == null)
                {
                    ScrollbarModule.InjectTo(axisRender, Owner);
                }

                if (ScrollbarModule != null && ((Owner.ZoomSettings.EnableScrollbar && axisRender.Axis.EnableScrollbarOnZooming) || axisRender.Axis.ScrollbarSettings.Enable))
                {
                    ScrollbarAxes.Add(axisRender.Axis);
                }
            }
        }

        internal RenderFragment CreateScrollbarDiv() => builder =>
        {
            builder.OpenElement(SvgRendering.Seq++, "div");
            builder.AddAttribute(SvgRendering.Seq++, "id", Owner.ID + "_scrollElement");
            builder.CloseElement();
        };

        private void ComputePlotAreaBounds(Rect newRect)
        {
            CheckAreaType();
            AxisLayout.ComputePlotAreaBounds(newRect);
            foreach (ChartAxisRenderer renderer in Renderers)
            {
                renderer.ClearAxisInfo();
                if (renderer.Orientation != Orientation.Null)
                {
                    AxisLayout.AxisRenderingCalculation(renderer);
                }
            }
        }

        internal void UpdateAxisRendering()
        {
            foreach (ChartAxisRenderer renderer in Renderers)
            {
                renderer.ClearAxisInfo();
                if (renderer.Orientation != Orientation.Null)
                {
                    renderer.RendererShouldRender = true;
                    AxisLayout.AxisRenderingCalculation(renderer);
                    renderer.ProcessRenderQueue();
                }
            }
        }

        public override void ProcessRenderQueue()
        {
            foreach (ChartAxisRenderer renderer in Renderers)
            {
                renderer.HandleChartSizeChange(availableRect);
                renderer.ProcessRenderQueue();
            }
        }

        protected override void BuildRenderers(RenderTreeBuilder builder)
        {
            if (builder == null)
            {
                return;
            }

            int seq = 0;
            if (ContainerUpdate)
            {
                AddDefaultRenderer(builder, seq);
                foreach (IChartElement element in Elements)
                {
                    if (element.RendererType == null)
                    {
                        continue;
                    }

                    builder.OpenComponent(seq++, element.RendererType);
                    builder.CloseComponent();
                }

                RendererShouldRender = false;
            }
        }

        private void AddDefaultRenderer(RenderTreeBuilder builder, int seq)
        {
            if (Elements.Count == 0)
            {
                builder.OpenComponent(seq++, typeof(PrimaryXAxisRenderer));
                builder.CloseComponent();
                builder.OpenComponent(seq++, typeof(PrimaryYAxisRenderer));
                builder.CloseComponent();
            }
            else
            {
                if (!Elements.Any(element => (element as ChartAxis).GetName() == "PrimaryXAxis"))
                {
                    builder.OpenComponent(seq++, typeof(PrimaryXAxisRenderer));
                    builder.CloseComponent();
                }

                if (!Elements.Any(element => (element as ChartAxis).GetName() == "PrimaryYAxis"))
                {
                    builder.OpenComponent(seq++, typeof(PrimaryYAxisRenderer));
                    builder.CloseComponent();
                }
            }
        }

        private void CheckAreaType()
        {
            // var areaType = Owner.GetAreaType();
            var areaType = Owner.GetAreaType();

            if (areaType == ChartAreaType.CartesianAxes)
            {
                AxisLayout = AxisLayout is CartesianAxisLayout ? AxisLayout : new CartesianAxisLayout();
            }
            else
            {
                AxisLayout = new PolarRadarAxisLayout();
            }
        }

        // TODO: Have to call series.OnAxisChanged if its axis changes.
        public void AssignAxisToSeries(IEnumerable<IRequireAxis> seriesList)
        {
            if (seriesList == null)
            {
                return;
            }

            foreach (KeyValuePair<string, ChartAxis> axis in Axes)
            {
                axis.Value.Renderer.SeriesRenderer.Clear();
                axis.Value.Renderer.Orientation = Orientation.Null;
                axis.Value.Renderer.IsStack100 = false;
            }

            foreach (IRequireAxis series in seriesList)
            {
                string axisName = series.XAxisName;
                series.XAxisRenderer = Axes[axisName].Renderer;
                Axes[axisName].Renderer.Orientation = (!Owner.RequireInvertedAxis) ? Orientation.Horizontal : Orientation.Vertical;
                Axes[axisName].Renderer.SeriesRenderer.Add(series as ChartSeriesRenderer);

                axisName = series.YAxisName;
                series.YAxisRenderer = Axes[axisName].Renderer;
                Axes[axisName].Renderer.Orientation = (!Owner.RequireInvertedAxis) ? Orientation.Vertical : Orientation.Horizontal;
                Axes[axisName].Renderer.SeriesRenderer.Add(series as ChartSeriesRenderer);
            }
        }

        internal void OnThemeChanged()
        {
            foreach (ChartAxisRenderer renderer in Renderers)
            {
                renderer.OnThemeChange();
                renderer.ProcessRenderQueue();
            }
        }
    }
}
