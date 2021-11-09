using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Internal;
using System;
using System.Text.RegularExpressions;
using Syncfusion.Blazor.Charts.Internal;
using Syncfusion.Blazor.DataVizCommon;
using Syncfusion.Blazor.Charts.RangeNavigator.Internal;
using System.Text.Json;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts.Chart.Internal
{
    internal class RangeTooltip
    {
        private string elementId;
        private SfRangeNavigator control;

        internal RangeTooltip(SfRangeNavigator range)
        {
            control = range;
            elementId = range.Id;
        }

        internal SVGTooltip LeftTooltip { get; set; }

        internal SVGTooltip RightTooltip { get; set; }

        internal ElementReference LeftElement { get; set; }

        internal ElementReference RightElement { get; set; }

        internal async Task RenderThumbTooltip(RangeSlider rangeSlider)
        {
            if (!double.IsNaN(rangeSlider.CurrentStart) && !double.IsNaN(rangeSlider.CurrentEnd))
            {
                string content = GetTooltipContent(rangeSlider.CurrentStart);
                Rect rect = control.IsRtlEnabled() ? rangeSlider.RightRect : rangeSlider.LeftRect;
                if (GetContentSize(content) > rect.Width)
                {
                    rect = rangeSlider.MidRect;
                }
                LeftTooltip = await RenderTooltipAsync(LeftTooltip, rect, rangeSlider.StartX, content);
                content = GetTooltipContent(rangeSlider.CurrentEnd);
                rect = control.IsRtlEnabled() ? rangeSlider.LeftRect : rangeSlider.RightRect;
                if (GetContentSize(content) > rect.Width)
                {
                    rect = rangeSlider.MidRect;
                    rect.X = control.Series.Count > 0 ? 0 : rect.X;
                }
                RightTooltip = await RenderTooltipAsync(RightTooltip, rect, rangeSlider.EndX, content);
                await control.InvokeMethod(RangeConstants.RENDERTOOLTIP, new object[] { JsonSerializer.Serialize(LeftTooltip), JsonSerializer.Serialize(RightTooltip), elementId + "_leftTooltip", elementId + "_rightTooltip", control.Element });
            }
        }

        private string GetTooltipContent(double point)
        {
            RangeNavigatorRangeTooltipSettings tooltip = control.Tooltip;
            ChartAxisRenderer xaxis = control.ChartSeries.XAxisRenderer;
            string formatText, format = tooltip.Format ?? control.LabelFormat;
            bool isCustom = !string.IsNullOrEmpty(Regex.Match(format, "{value}").Value);
            if (xaxis.Axis.ValueType == ValueType.DateTime)
            {
                return Intl.GetDateFormat(new DateTime(1970, 1, 1).AddMilliseconds(point), string.IsNullOrEmpty(format) ? "MM/dd/yyyy" : format);
            }
            else
            {
#pragma warning disable BL0005
                xaxis.Axis.Format = Intl.GetNumericFormat(point, isCustom ? string.Empty : format);
#pragma warning restore BL0005
                format = string.IsNullOrEmpty(format) && control.UseGroupingSeparator == true ? "N" : format;
                formatText = Intl.GetNumericFormat(xaxis.Axis.ValueType == ValueType.Logarithmic ? Math.Round(Math.Pow(xaxis.Axis.LogBase, point), 2) : Math.Round(point, 2), isCustom ? string.Empty : format);
                return !control.UseGroupingSeparator ? formatText.Replace(",", string.Empty, StringComparison.InvariantCulture) : formatText;
            }
        }

        private async Task<SVGTooltip> RenderTooltipAsync(SVGTooltip svgTooltip, Rect bounds, double pointX, string content)
        {
            RangeNavigatorRangeTooltipSettings tooltip = control.Tooltip;
            RangeTooltipRenderEventArgs argsData = new RangeTooltipRenderEventArgs
            {
                EventName = RangeConstants.TOOLTIPRENDER,
                Text = content,
                TextStyle = tooltip.TextStyle
            };
            if (control.RangeNavigatorEvents?.TooltipRender != null)
            {
                await SfBaseUtils.InvokeEvent<RangeTooltipRenderEventArgs>(control.RangeNavigatorEvents?.TooltipRender, argsData);
            }

            double left = 0;
            if (!argsData.Cancel)
            {
                if (svgTooltip == null)
                {
                    svgTooltip = new SVGTooltip()
                    {
                        Location = new ToolLocationModel { X = pointX, Y = control.RangeSliderModule.SliderY },
                        Content = new string[] { argsData.Text },
                        ArrowPadding = 8,
                        Inverted = control.Series.Count > 0,
                        AreaBounds = new AreaBoundsModel { X = bounds.X, Y = bounds.Y, Height = bounds.Height, Width = bounds.Width },
                        Fill = tooltip.Fill,
                        Theme = control.Theme.ToString(),
                        ClipBounds = new ToolLocationModel { X = left },
                        Border = new TooltipBorderModel { Color = tooltip.Border.Color, Width = tooltip.Border.Width },
                        Opacity = tooltip.Opacity,
                        Template = tooltip.TooltipTemplate,
                        Header = string.Empty,
                        Shapes = Array.Empty<TooltipShape>(),
                        TextStyle = new TextStyleModel
                        {
                            Size = argsData.TextStyle.Size,
                            Color = argsData.TextStyle.Color,
                            FontFamily = argsData.TextStyle.FontFamily,
                            FontWeight = argsData.TextStyle.FontWeight,
                            FontStyle = argsData.TextStyle.FontStyle,
                            Opacity = argsData.TextStyle.Opacity
                        },
                        AvailableSize = control.AvailableSize,
                        Data = new TemplateData
                        {
                            X = GetTooltipContent(control.StartValue),
                            Y = GetTooltipContent(control.EndValue),
                            Text = content
                        }
                    };
                }
                else
                {
                    svgTooltip.Location = new ToolLocationModel { X = pointX, Y = control.RangeSliderModule.SliderY };
                    svgTooltip.Content = new string[] { argsData.Text };
                    svgTooltip.AreaBounds = new AreaBoundsModel { X = bounds.X, Y = bounds.Y, Height = bounds.Height, Width = bounds.Width };
                    svgTooltip.Fill = tooltip.Fill;
                    svgTooltip.Theme = control.Theme.ToString();
                    svgTooltip.ClipBounds = new ToolLocationModel { X = left };
                    svgTooltip.Border = new TooltipBorderModel { Color = tooltip.Border.Color, Width = tooltip.Border.Width };
                    svgTooltip.Opacity = tooltip.Opacity;
                    svgTooltip.Template = tooltip.TooltipTemplate;
                    svgTooltip.TextStyle = new TextStyleModel
                    {
                        Size = argsData.TextStyle.Size,
                        Color = argsData.TextStyle.Color,
                        FontFamily = argsData.TextStyle.FontFamily,
                        FontWeight = argsData.TextStyle.FontWeight,
                        FontStyle = argsData.TextStyle.FontStyle,
                        Opacity = argsData.TextStyle.Opacity
                    };
                    svgTooltip.Data = new TemplateData
                    {
                        X = GetTooltipContent(control.StartValue),
                        Y = GetTooltipContent(control.EndValue),
                        Text = content
                    };
                }
            }

            return svgTooltip;
        }

        private double GetContentSize(string content)
        {
            if (control.Tooltip.TooltipTemplate != null)
            {
                return 0;
            }
            else
            {
                return ChartHelper.MeasureText(content, SfRangeNavigator.GetFontOptions(control.Tooltip.TextStyle)).Width + 20;
            }
        }

        internal void Dispose()
        {
            elementId = null;
            control = null;
            LeftTooltip = null;
            RightTooltip = null;
        }
    }
}