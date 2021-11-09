using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Internal;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Globalization;
using System;

namespace Syncfusion.Blazor.Charts.BulletChart.Internal
{
    /// <summary>
    /// Specifies tooltip rendering of the bullet chart.
    /// </summary>
    /// <typeparam name="TValue">Represents the generic data type of the bullet chart tooltip.</typeparam>
    public partial class BulletChartTooltipRender<TValue>
    {
        private string style = string.Empty;
        private List<string> tooltipText = new List<string>();
        private int dataId;
        private CultureInfo culture = CultureInfo.InvariantCulture;

        [CascadingParameter]
        internal SfBulletChart<TValue> BulletChart { get; set; }

        internal bool IsTooltipRender { get; set; }

        internal TextStyle FontInfo { get; set; } = new TextStyle();

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            if (BulletChart.Tooltip != null)
            {
                BulletChart.ChartToolTip = this;
                SetTooltipTextStyle();
            }
        }

        internal RenderFragment RenderTooltipTemplate()
        {
            RenderFragment fragment = builder =>
            {
                int seq = 0;
                builder.OpenElement(seq++, "div");
                builder.AddAttribute(seq++, "id", BulletChart.ID + "_parent_template");
                builder.AddContent(seq++, BulletChart.Tooltip.Template(BulletChart.DataSource.ToList()[dataId]));
                builder.CloseElement();
            };
            return fragment;
        }

        internal TextStyle SetTooltipTextStyle()
        {
            BulletChartTooltipTextStyle style = BulletChart.Tooltip.TextStyle;
            if (style != null)
            {
                FontInfo.Color = style.Color;
                FontInfo.FontFamily = style.FontFamily;
                FontInfo.FontStyle = style.FontStyle;
                FontInfo.FontWeight = style.FontWeight;
                FontInfo.Opacity = style.Opacity;
                FontInfo.Size = style.Size;
            }

            return FontInfo;
        }

        internal async Task DisplayTooltip(string elementId, double mouseX, double mouseY)
        {
            BulletChartTooltipEventArgs args = new BulletChartTooltipEventArgs();
            TextStyle textStyle = SetTooltipTextStyle();
            int id = int.Parse(elementId.Split("_").Last(), null);
            IDictionary<string, object> data = BulletChart.Render.ProcessedData[id];
            data.TryGetValue(BulletChart.ValueField ?? string.Empty, out object valueField);
            data.TryGetValue(BulletChart.TargetField ?? string.Empty, out object targetField);
            dataId = id;
            args.Value = (valueField ?? string.Empty).ToString();
            args.Target = new List<string>();
            if (targetField is IEnumerable)
            {
                IEnumerator iterator = (targetField as IEnumerable).GetEnumerator();
                while (iterator.MoveNext())
                {
                    args.Target.Add(iterator.Current.ToString());
                }
            }
            else
            {
                double target = double.Parse((targetField ?? string.Empty).ToString(), null);
                args.Target.Add(string.Format(null, "{0}", target));
            }

            string argsText = "Value : " + BulletChart.Helper.GetText(args.Value, BulletChart.LabelFormat, BulletChart.Format, BulletChart.EnableGroupSeparator);
            tooltipText = new List<string>();
            tooltipText.Add(argsText);
            for (int i = 0; i < args.Target.Count; i++)
            {
                string content = "Target" + (i == 0 ? string.Empty : "_" + i) + " : " + BulletChart.Helper.GetText(args.Target[i], BulletChart.LabelFormat, BulletChart.Format, BulletChart.EnableGroupSeparator);
                tooltipText.Add(content);
                argsText += "<br/> " + content;
            }

            args.Text = argsText;
            await SfBaseUtils.InvokeEvent<BulletChartTooltipEventArgs>(BulletChart.Events?.TooltipRender, args);
            if (!args.Cancel)
            {
                style = "position: absolute; z-index: 13000; display: block;";
                string fill = !string.IsNullOrEmpty(BulletChart.Tooltip.Fill) ? BulletChart.Tooltip.Fill : BulletChart.Style.TooltipFill,
                borderColor = BulletChart.Tooltip.Border != null ? BulletChart.Tooltip.Border.Color : "Black";
                double borderWidth = BulletChart.Tooltip.Border != null ? BulletChart.Tooltip.Border.Width : 1,
                xpos = mouseX, ypos = mouseY;
                if (BulletChart.Tooltip.Template != null)
                {
                    style = "position: absolute;left:" + (xpos + 20).ToString(culture) + "px;top:" + (ypos + 20).ToString(culture) + "px;";
                }
                else
                {
                    style += "left:" + (xpos + 20).ToString(culture) + "px;" + "top:" + (ypos + 20).ToString(culture) + "px;" + "-webkit-border-radius: 5px 5px 5px 5px; -moz-border-radius: 5px 5px 5px 5px;-o-border-radius: 5px 5px 5px 5px;" +
                            "border-radius: 5px 5px 5px 5px;" + "background-color:" + fill + ";" + "color:" + (string.IsNullOrEmpty(textStyle.Color) ? BulletChart.Style.TooltipFontColor : textStyle.Color) + "; border:"
                            + borderWidth.ToString(culture) + "px Solid " + borderColor + ";" + "padding-bottom: 7px;" + "font-style:" + textStyle.FontStyle + "; padding-left: 10px; font-family:" + textStyle.FontFamily + ";font-size:" + textStyle.Size + ";padding-right: 10px; padding-top: 7px";
                }

                StateHasChanged();
            }
            else
            {
                style = string.Empty;
            }
        }

        internal void RemoveTooltip()
        {
            IsTooltipRender = false;
            StateHasChanged();
        }

        internal override void ComponentDispose()
        {
            BulletChart = null;
            tooltipText = null;
        }
    }
}