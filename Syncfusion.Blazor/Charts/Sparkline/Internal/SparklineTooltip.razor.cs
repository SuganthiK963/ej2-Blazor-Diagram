using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Syncfusion.Blazor.Sparkline.Internal;
using System.Linq;
using System;
using Syncfusion.Blazor.Internal;
using System.Globalization;
using Syncfusion.Blazor.Popups;

namespace Syncfusion.Blazor.Charts.Sparkline.Internal
{
    /// <summary>
    /// Specifies the rendering of tooltip in sparkline component.
    /// </summary>
    /// <typeparam name="TValue">Represents the generic data type of tooltip in sparkline component.</typeparam>
    public partial class SparklineTooltip<TValue>
    {
        private SfTooltip tooltipReference;
        private bool tooltipCreated;
        private double x, y;
        private int pointIndex = -1;
        private string text = string.Empty;
        private bool isTooltipRender;
        private int prevPoint = -1;
        private bool isIE;
        private CultureInfo culture = CultureInfo.InvariantCulture;
        private string fill;

        [CascadingParameter]
        internal SfSparkline<TValue> Parent { get; set; }

        internal TextStyle FontInfo { get; set; } = new TextStyle();

        private void TooltipCreated()
        {
            if (isTooltipRender)
            {
                tooltipCreated = true;
            }
        }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            if (Parent.TooltipSettings != null)
            {
                Parent.Tooltip = this;
                SetTextStyle();
            }
        }

        internal RenderFragment RenderTooltipTemplate()
        {
            RenderFragment fragment = builder =>
            {
                int seq = 0;
                builder.OpenElement(seq++, "div");
                builder.AddAttribute(seq++, "id", Parent.ID + "_parent_template");
                builder.AddContent(seq++, Parent.TooltipSettings.Template(Parent.DataSource.ToList()[pointIndex]));
                builder.CloseElement();
            };
            return fragment;
        }

        private void SetTextStyle()
        {
            if (Parent.TooltipSettings.TextStyle != null)
            {
                SparklineTooltipTextStyle style = Parent.TooltipSettings.TextStyle;
                FontInfo.Color = style.Color;
                FontInfo.Opacity = style.Opacity;
                FontInfo.Size = style.Size;
                FontInfo.FontFamily = style.FontFamily;
                FontInfo.FontStyle = style.FontStyle;
                FontInfo.FontWeight = style.FontWeight;
            }
            else
            {
                FontInfo.Color = Parent.ThemeStyle.TooltipFont;
                FontInfo.Opacity = Parent.ThemeStyle.TooltipTextOpacity;
                FontInfo.FontFamily = Parent.ThemeStyle.FontFamily;
            }
        }

        internal void Rendertooltip(double x, double y, string id, bool isIE)
        {
            SetTextStyle();
            fill = string.IsNullOrEmpty(Parent.TooltipSettings.Fill) ? Parent.ThemeStyle.TooltipFill : Parent.TooltipSettings.Fill;
            this.isIE = isIE;
            if (Parent.Type != SparklineType.Pie)
            {
                double[] trackerPositions = Parent.VisiblePoints?.Select(pt => pt.Location.X).ToArray();
                double pointDiff = double.PositiveInfinity, mousePosition, diff;
                for (int i = 0, len = trackerPositions.Length; i < len; i++)
                {
                    diff = Math.Abs(x - trackerPositions[i]);
                    if (pointDiff > diff)
                    {
                        pointDiff = diff;
                        mousePosition = trackerPositions[i];
                        pointIndex = i;
                    }
                }
            }
            else if (id.Contains("_Sparkline_Pie_", StringComparison.InvariantCulture))
            {
                pointIndex = int.Parse(id.Split("_Sparkline_Pie_")[1], null);
            }

            if (prevPoint != pointIndex)
            {
                if (isTooltipRender && (pointIndex == -1 || !SparklineHelper.WithInBounds(x, y, new RectInfo(Parent.AvailableSize.Height, Parent.AvailableSize.Width, 0, 0))))
                {
                    RemoveTooltip();
                    return;
                }

                isTooltipRender = true;
                prevPoint = pointIndex;
                if (Parent.TooltipSettings.TrackLineSettings != null && Parent.TooltipSettings.TrackLineSettings.Visible)
                {
                    Parent.Trackline.RenderTrackline(Parent.VisiblePoints[pointIndex], Parent.AvailableSize, Parent.Padding);
                }

                SetTooltip(Parent.VisiblePoints[pointIndex]);
            }
        }

        internal void RemoveTooltip()
        {
            if (isTooltipRender)
            {
                prevPoint = -1;
                isTooltipRender = false;
                Parent.Trackline?.RemoveTrackline();
                StateHasChanged();
            }
        }

        private void SetTooltip(SparklineValues sparkValues)
        {
            if (Parent.TooltipSettings.Visible)
            {
                text = GetText(sparkValues.XVal, sparkValues.YVal.ToString(culture), Parent.TooltipSettings.Format, Parent.Format, Parent.EnableGroupingSeparator, sparkValues.XName.ToString());
                if (Parent.Type == SparklineType.Column)
                {
                    x = sparkValues.X + (sparkValues.Width / 2);
                    y = sparkValues.Y;
                }
                else if (Parent.Type == SparklineType.Pie || Parent.Type == SparklineType.WinLoss)
                {
                    x = sparkValues.Location.X;
                    y = sparkValues.Location.Y;

                }
                else
                {
                    y = sparkValues.Y;
                    x = sparkValues.X;
                }
                StateHasChanged();
            }
        }

        internal string GetText(double xdata, string ydata, string labelFormat, string format, bool enableGroupSeparator, string xname)
        {
            string formatText = TextFormatter(labelFormat, xdata, Intl.GetNumericFormat(double.Parse(ydata, null), format), xname);
            return !enableGroupSeparator ? formatText.Replace(",", string.Empty, StringComparison.InvariantCulture) : formatText;
        }

        internal string TextFormatter(string format, double x, string ydata, string xname)
        {
            if (!string.IsNullOrEmpty(format))
            {
                string xdata = x.ToString(culture);
                if (Parent.ValueType == SparklineValueType.Category)
                {
                    xdata = xname;
                }
                else if (Parent.ValueType == SparklineValueType.DateTime)
                {
                    xdata = (new DateTime(1970, 1, 1) + TimeSpan.FromMilliseconds(x)).ToLongDateString();
                }

                return SparklineHelper.FormatText(xdata, ydata, Parent.XName, Parent.YName, format);
            }

            return ydata;
        }

        internal override void ComponentDispose()
        {
            base.ComponentDispose();
            Parent = null;
            FontInfo = null;
        }

        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender">Specifies the first render of the component.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (!firstRender && tooltipCreated)
            {
                await tooltipReference.OpenAsync();
            }
        }
    }
}