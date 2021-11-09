using Syncfusion.Blazor.Charts.AccumulationChart.Internal;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Syncfusion.Blazor.Charts.Chart.Internal;
using System.Collections.Generic;
using System;
using Syncfusion.Blazor.DataVizCommon;
using System.Collections.Specialized;

namespace Syncfusion.Blazor.Charts
{
    public partial class SfAccumulationChart : SfDataBoundComponent, IAccumulationChart
    {
        private bool isObservableWired { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task OnAccumulationChartParametersSet()
        {
            if (!object.ReferenceEquals(DataSource, dataSource))
            {
                UpdateObservableEventsForObject(nameof(DataSource), dataSource, true);
                UpdateObservableEventsForObject(nameof(DataSource), DataSource);
            }

            title = Title = NotifyPropertyChanges(nameof(Title), Title, title);
            subTitle = NotifyPropertyChanges(nameof(SubTitle), SubTitle, subTitle);
            background = NotifyPropertyChanges(nameof(Background), Background, background);
            backgroundImage = NotifyPropertyChanges(nameof(BackgroundImage), BackgroundImage, backgroundImage);
            enableAnimation = NotifyPropertyChanges(nameof(EnableAnimation), EnableAnimation, enableAnimation);
            enableBorderOnMouseMove = NotifyPropertyChanges(nameof(EnableBorderOnMouseMove), EnableBorderOnMouseMove, EnableBorderOnMouseMove);
            enableSmartLabels = NotifyPropertyChanges(nameof(EnableSmartLabels), EnableSmartLabels, enableSmartLabels);
            height = NotifyPropertyChanges(nameof(Height), Height, height);
            width = NotifyPropertyChanges(nameof(Width), Width, width);
            highlightMode = NotifyPropertyChanges(nameof(HighlightMode), HighlightMode, highlightMode);
            highlightPattern = NotifyPropertyChanges(nameof(HighlightPattern), HighlightPattern, highlightPattern);
            isMultiSelect = NotifyPropertyChanges(nameof(IsMultiSelect), IsMultiSelect, isMultiSelect);
            selectionMode = NotifyPropertyChanges(nameof(SelectionMode), SelectionMode, selectionMode);
            selectionPattern = NotifyPropertyChanges(nameof(SelectionPattern), SelectionPattern, selectionPattern);
            theme = NotifyPropertyChanges(nameof(Theme), Theme, theme);
            query = NotifyPropertyChanges(nameof(Query), Query, query);
            dataSource = DataSource = NotifyPropertyChanges(nameof(DataSource), DataSource, dataSource);
            isObservableWired = DataVizCommonHelper.BindObservable(this, nameof(DataSource), DataSource, isObservableWired);
            if (IsRendered && PropertyChanges.Any())
            {
                PropertyUpdate update = new PropertyUpdate();
                foreach (string property in PropertyChanges?.Keys)
                {
                    animateSeries = false;
                    LegendClickRedraw = false;
                    switch (property)
                    {
                        case nameof(Theme):
                            animateSeries = true;
                            AccChartThemeStyle = ChartHelper.GetChartThemeStyle(Theme.ToString());
                            ProcessData();
                            return;
                        case "Title":
                        case "SubTitle":
                        case "Height":
                        case "Width":
                        case "Margin":
                        case "TitleStyle":
                        case "SubTitleStyle":
                            update.RefreshBounds = true;
                            break;
                        case "LegendSettings":
                        case nameof(Center):
                            update.RefreshBounds = true;
                            update.RefreshElements = true;
                            break;
                        case "DataSource":
                            if (VisibleSeries != null && VisibleSeries.First().DataSource == null)
                            {
                                SetDataManager<object>(DataSource);
                                VisibleSeries.First().DataModule = (IEnumerable<object>)await DataManager.ExecuteQuery<object>(new Data.Query());
                            }

                            ProcessData();
                            update.RefreshBounds = true;
                            break;
                        case "Series":
                            ProcessData();
                            return;

                        case "Background":
                        case "Border":
                        case "Annotations":
                        case "enableSmartLabels":
                            update.RefreshElements = true;
                            break;
                        case "IsMultiSelect":
                        case "SelectedDataIndexes":
                        case "SelectionMode":
                            if (AccumulationSelectionModule != null)
                            {
                                if (AccumulationSelectionModule.SelectedDataIndexes.Count != 0)
                                {
                                    AccumulationSelectionModule.InvokeSelection();
                                }
                                else
                                {
                                    AccumulationSelectionModule.RedrawSelection();
                                }
                            }

                            break;
                        case nameof(SelectionPattern):
                        case nameof(HighlightPattern):
                            {
                                Rendering?.PathElementList?.Clear();
                                CreateChart();
                                return;
                            }
                    }
                }

                if (!update.RefreshBounds && update.RefreshElements)
                {
                    ChartContent = null;
                    shouldRender = true;
#pragma warning disable CA2007
                    await GetContainerSize();
                    SetContainerSize();
#pragma warning restore CA2007
                }
                else if (update.RefreshBounds)
                {
                    ChartContent = null;
                    shouldRender = true;
                    VisibleSeries[0].RefreshPoints(VisibleSeries[0].Points);
                    CalculateBounds();
                    InstanceInitialization();
                    CreateChart();
                }

                PropertyChanges.Clear();
            }
        }

        protected override void OnObservableChange(string propertyName, object sender, bool isCollectionChanged = false, NotifyCollectionChangedEventArgs e = null)
        {
            if (PropertyChanges.ContainsKey("DataSource") && !IsDisposed)
            {
                try
                {
                    foreach (AccumulationChartSeries series in VisibleSeries)
                    {
                        series.DataModule = (IEnumerable<object>)DataSource;
                    }
                    Refresh(false);
                }
                catch
                {
                    if (!IsDisposed)
                    {
                        throw;
                    }
                }

                PropertyChanges.Remove("DataSource");
            }
        }
    }
}