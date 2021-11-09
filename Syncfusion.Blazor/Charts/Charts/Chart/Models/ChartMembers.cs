using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Internal;
using Syncfusion.Blazor.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.Charts.Internal;
using Syncfusion.Blazor.Charts.Chart.Internal;
using System.Linq;
using System.Collections.Specialized;

[assembly: InternalsVisibleTo("Syncfusion.Blazor.StockChart")]

namespace Syncfusion.Blazor.Charts
{
    public partial class SfChart : SfDataBoundComponent
    {
        #region CHART PRIVATE FIELD
        private HighlightMode highlightMode;
        private SelectionMode selectionMode;
        private SelectionPattern selectionPattern;
        private SelectionPattern highlightPattern;
        private bool isMultiSelect;
        private Theme theme = Theme.Material;
        private string background;
        private string title = string.Empty;
        private string height = "100%";
        private string width = "100%";
        private bool isTransposed;
        private bool enableSideBySidePlacement = true;
        private string[] palettes = Array.Empty<string>();
        private bool updateLayout;
        private bool refreshLayout;
        private IEnumerable<object> dataSource;
        private Query query;
        private bool enableRTL;
        #endregion

        internal ZoomContent ZoomingContent { get; set; }

        internal ZoomToolkit ZoomingToolkitContent { get; set; }

        /// <summary>
        /// The height of the chart as a string accepts input both as '100px' or '100%'.
        /// If specified as '100%, chart renders to the full height of its parent element.
        /// </summary>
        [Parameter]
        public string Height
        {
            get
            {
                return height;
            }

            set
            {
                if (height != value)
                {
                    height = value;
                    updateLayout = IsRendered;
                }
            }
        }

        /// <summary>
        /// The width of the chart as a string accepts input as both like '100px' or '100%'.
        /// If specified as '100%, chart renders to the full width of its parent element.
        /// </summary>
        [Parameter]
        public string Width
        {
            get
            {
                return width;
            }

            set
            {
                if (width != value)
                {
                    width = value;
                    updateLayout = IsRendered;
                }
            }
        }

        /// <summary>
        /// Gets and sets the title of the chart component.
        /// </summary>
        [Parameter]
        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                if (title != value)
                {
                    title = value;
                    if (IsRendered)
                    {
                        ChartTitleRenderer.RendererShouldRender = true;
                        ChartTitleRenderer.SetTitleCollection(InitialRect);
                        ChartTitleRenderer.ProcessRenderQueue();
                    }
                }
            }
        }

        /// <summary>
        /// Gets and sets the sub title of the chart component.
        /// </summary>
        [Parameter]
        public string SubTitle { get; set; } = string.Empty;

        /// <summary>
        /// Defines id of the chart component.
        /// </summary>
        [Parameter]
        public string ID { get; set; } = SfBaseUtils.GenerateID("chart");

        /// <summary>
        /// Gets and sets whether the chart should be render in transposed manner or not.
        /// </summary>
        [Parameter]
        public bool IsTransposed
        {
            get
            {
                return isTransposed;
            }

            set
            {
                if (isTransposed != value)
                {
                    isTransposed = value;
                    if (IsRendered)
                    {
                        InitiAxis();
                        refreshLayout = IsRendered;
                    }
                }
            }
        }

        /// <summary>
        /// Gets and sets the theme for the chart.
        /// </summary>
        [Parameter]
        public Theme Theme
        {
            get
            {
                return theme;
            }

            set
            {
                if (theme != value)
                {
                    theme = value;
                    if (IsRendered)
                    {
                        ChartThemeStyle = ChartHelper.GetChartThemeStyle(theme.ToString());
                        OnThemeChanged();
                    }
                }
            }
        }

        /// <summary>
        /// Gets and sets the palette for the chart series.
        /// </summary>
        [Parameter]
        public string[] Palettes
        {
            get
            {
                return palettes;
            }

            set
            {
                if (!palettes.SequenceEqual(value))
                {
                    if (value == null)
                    {
                        return;
                    }

                    palettes = value.Clone() as string[];
                    if (IsRendered)
                    {
                        SeriesContainer.OnThemeChanged();
                    }
                }
            }
        }

        /// <summary>
        /// Specifies the DataSource for the chart. It can be an array of JSON objects or an instance of DataManager.
        /// </summary>
        [Parameter]
        public IEnumerable<object> DataSource
        {
            get
            {
                return dataSource;
            }

            set
            {
                if (dataSource != value)
                {
                    dataSource = value;
                    if (dataSource is INotifyCollectionChanged)
                    {
                        ((INotifyCollectionChanged)dataSource).CollectionChanged += DataCollectionChanged;
                    }
                }
            }
        }

        /// <summary>
        /// Specifies query to select data from DataSource. This property is applicable only when the DataSource is `SfDataManager`.
        /// </summary>
        [Parameter]
        public Query Query
        {
            get
            {
                return query;
            }

            set
            {
                if (query == null || !query.Equals(value))
                {
                    query = value;
                    if (IsRendered)
                    {
                       OnQueryChanged();
                    }
                }
            }
        }

        /// <summary>
        /// Gets and sets the background color of the chart that accepts value in hex and rgba as a valid CSS color string.
        /// </summary>
        [Parameter]
        public string Background
        {
            get
            {
                return background;
            }

            set
            {
                if (background != value)
                {
                    background = value;
                    if (IsRendered)
                    {
                        ChartBorderRenderer.RendererShouldRender = true;
                        ChartBorderRenderer.ProcessRenderQueue();
                    }
                }
            }
        }

        /// <summary>
        /// Enable / Disable the chart element render from right to left.
        /// </summary>
        [Parameter]
        public bool EnableRTL
        {
            get
            {
                return enableRTL;
            }

            set
            {
                if (enableRTL != value)
                {
                    enableRTL = value;
                    if (IsRendered)
                    {
                        OnLayoutChange();
                    }
                }
            }
        }

        /// <summary>
        /// Option for enable the side by side placement.
        /// </summary>
        [Parameter]
        public bool EnableSideBySidePlacement
        {
            get
            {
                return enableSideBySidePlacement;
            }

            set
            {
                if (enableSideBySidePlacement != value)
                {
                    enableSideBySidePlacement = value;
                    updateLayout = true;
                }
            }
        }


        /// <summary>
        /// Gets and sets both axis interval will be calculated automatically with respect to the zoomed range.
        /// </summary>
        [Parameter]
        public bool EnableAutoIntervalOnBothAxis { get; set; }

        /// <summary>
        /// Gets and sets the background image for chart.
        /// </summary>
        [Parameter]
        public string BackgroundImage { get; set; } = string.Empty;

        /// <summary>
        /// Gets and sets the selection mode of the chart component.
        /// </summary>
        [Parameter]
        public SelectionMode SelectionMode
        {
            get
            {
                return selectionMode;
            }

            set
            {
                if (selectionMode != value)
                {
                    selectionMode = value;
                    if (SelectionModule == null && IsScriptLoaded)
                    {
                        SelectionModule = new Selection(this);
                        SelectionModule.InvokeSelection();
                    }

                    SelectionModule?.SelectionModeChanged();
                }
            }
        }

        /// <summary>
        /// Gets and sets the highlight mode of the chart component.
        /// </summary>
        [Parameter]
        public HighlightMode HighlightMode
        {
            get
            {
                return highlightMode;
            }

            set
            {
                if (highlightMode != value)
                {
                    highlightMode = value;
                    if (HighlightModule == null && IsScriptLoaded)
                    {
                        HighlightModule = new Highlight(this);
                        HighlightModule.InvokeHighlight();
                        SelectionModule?.CallSeriesStyles();
                    }

                    HighlightModule?.PropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets and sets the selection pattern of the chart component.
        /// </summary>
        [Parameter]
        public SelectionPattern SelectionPattern
        {
            get
            {
                return selectionPattern;
            }

            set
            {
                if (selectionPattern != value)
                {
                    selectionPattern = value;
                    if (SelectionModule != null)
                    {
                        SelectionModule.CallSeriesStyles();
                    }
                }
            }
        }

        /// <summary>
        /// Gets and sets the highlight pattern of the chart component.
        /// </summary>
        [Parameter]
        public SelectionPattern HighlightPattern
        {
            get
            {
                return highlightPattern;
            }

            set
            {
                if (highlightPattern != value)
                {
                    highlightPattern = value;
                    if (HighlightModule != null)
                    {
                        HighlightModule.CallSeriesStyles(false);
                    }
                }
            }
        }

        /// <summary>
        /// Option for enable the multi select of the chart component.
        /// </summary>
        [Parameter]
        public bool IsMultiSelect
        {
            get
            {
                return isMultiSelect;
            }

            set
            {
                if (isMultiSelect != value)
                {
                    isMultiSelect = value;
                    if (SelectionModule != null)
                    {
                        SelectionModule.ClearDraggedRects();
                        SelectionModule.OnPropertyChanged();
                        ParentRect?.ClearElements();
                    }
                }
            }
        }

        /// <summary>
        /// Option for enable to allow the multi select of the chart component.
        /// </summary>
        [Parameter]
        public bool AllowMultiSelection
        {
            get
            {
                return isMultiSelect;
            }

            set
            {
                if (isMultiSelect != value)
                {
                    isMultiSelect = value;
                    if (SelectionModule != null)
                    {
                        SelectionModule.ClearDraggedRects();
                        SelectionModule.OnPropertyChanged();
                        ParentRect.ClearElements();
                    }
                }
            }
        }

        /// <summary>
        /// Option for enable the group separator for yaxis label.
        /// </summary>
        [Parameter]
        public bool UseGroupingSeparator { get; set; }

        /// <summary>
        /// Gets and sets the access text for chart title.
        /// </summary>
        [Parameter]
        public string Description { get; set; }

        /// <summary>
        /// Gets and sets the tabIndex for chart title.
        /// </summary>
        [Parameter]
        public double TabIndex { get; set; } = 1;

        /// <summary>
        /// Option for enable the animation for chart.
        /// </summary>
        [Parameter]
        public bool EnableAnimation { get; set; } = true;

        /// <summary>
        /// Gets and sets the custom class for the chart.
        /// </summary>
        [Parameter]
        public string CustomClass { get; set; } = string.Empty;

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
        [Parameter]
        public bool EnablePersistence { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
        [Parameter]
        public bool EnableCanvas { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
        [Parameter]
        public bool EnableExport { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
        [Parameter]
        public bool AllowExport { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
        public void SetAnnotationValue(double annotationIndex, string content)
        {
        }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Parameter]
        public bool IsStockChart { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Parameter]
        public Action<RenderTreeBuilder> StockEventsRender { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Parameter]
        public Func<double> GetTooltipTop { get; set; }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (updateLayout)
            {
                updateLayout = false;
                OnDimensionChanged();
            }
        }

        private void OnThemeChanged()
        {
            ChartBorderRenderer?.OnThemeChanged();
            ChartAreaRenderer?.OnThemeChanged();
            ChartTitleRenderer?.OnThemeChanged();
            AxisContainer.OnThemeChanged();
            SeriesContainer.OnThemeChanged();
            LegendRenderer?.OnThemeChanged();
        }

        private void OnDimensionChanged()
        {
            CalculateAvailableSize();
            OnLayoutChange();
        }

        private void DataCollectionChanged(object source, NotifyCollectionChangedEventArgs e)
        {
            if (SeriesContainer != null)
            {
                foreach (ChartSeriesRenderer seriesRenderer in SeriesContainer.Renderers)
                {
                    seriesRenderer.Series.DataCollectionChanged(source, e);
                }
            }
        }

        private async void OnQueryChanged()
        {
            await GetRemoteData();
            foreach (ChartSeriesRenderer seriesRenderer in SeriesContainer.Renderers)
            {
                seriesRenderer.UpdateCurrentViewData(SeriesContainer.Data);
                await seriesRenderer.UpdateSeriesData(true);
            }
        }
    }
}