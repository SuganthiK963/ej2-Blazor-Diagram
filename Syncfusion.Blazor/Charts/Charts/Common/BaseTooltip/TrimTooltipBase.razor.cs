using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.Charts.Chart.Models;
using System.Globalization;
using System.Timers;

namespace Syncfusion.Blazor.Charts.Internal
{
    public partial class TrimTooltipBase
    {
        [Parameter]
        public string Id { get; set; }

        [Parameter]
        public string Style { get; set; }

        [Parameter]
        public string Content { get; set; }

        private Timer timer { get; set; } = new Timer();

        private CultureInfo culture { get; set; } = CultureInfo.InvariantCulture;

        internal void ChangeContent(string id = "", bool remove = false)
        {
            if (remove)
            {
                Id = Content = string.Empty;
                Style = null;
                InvokeAsync(StateHasChanged);
            }
            else if (id == Id)
            {
                Style = null;
                Content = string.Empty;
                InvokeAsync(StateHasChanged);
            }
        }

        internal void ShowTooltip(string text, double x, double y, double areaWidth, double areaHeight, string id)
        {
#pragma warning disable CA2000
#pragma warning disable BL0005
            Chart.Internal.Size textSize = ChartHelper.MeasureText(text, new ChartFontOptions() { FontFamily = "Segoe UI", Size = "12px", FontStyle = "Normal", FontWeight = "Regular" });
            double width = textSize.Width + 5;
#pragma warning restore BL0005
#pragma warning restore CA2000
            x = (x + width > areaWidth) ? x - (width + 15) : x + 15;
            y = (y + textSize.Height > areaHeight) ? y - (textSize.Height / 2) : y;
            Id = id;
            Content = text;
            x = x < 0 ? 0 : x;
            y = y < 0 ? 0 : y;
            Style = "top:" + (y - 10).ToString(culture) + "px;left:" + x.ToString(culture) + "px;background-color: rgb(255, 255, 255) !important; color:black !important; " +
                    "position:absolute;border:1px solid rgb(112, 112, 112); padding-left : 3px; padding-right : 2px;" + "padding-bottom : 2px; padding-top : 2px; font-size:12px; font-family: 'Segoe UI';pointer-events: none;";
            InvokeAsync(StateHasChanged);
        }

        internal void FadeOutTooltip(double fadeOutDuration)
        {
            timer.Stop();
            timer.Interval = fadeOutDuration;
            timer.AutoReset = false;
            if (Content != null)
            {
                timer.Elapsed += OnTimeOut;
                timer.Start();
            }

            StateHasChanged();
        }

        private void OnTimeOut(object source, ElapsedEventArgs e)
        {
            Content = null;
        }
    }
}