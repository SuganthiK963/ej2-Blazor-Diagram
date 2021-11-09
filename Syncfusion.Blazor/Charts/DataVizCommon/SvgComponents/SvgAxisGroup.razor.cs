using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Timers;

namespace Syncfusion.Blazor.DataVizCommon
{
#pragma warning disable CA1001 // Types that own disposable fields should be disposable
    public partial class SvgAxisGroup
#pragma warning restore CA1001 // Types that own disposable fields should be disposable
    {
        private DateTime start;
        private int duration = 100;
        private Timer timer = new Timer();
        private string horizontalId;
        private string verticalId;
        private string axisTooltipId;

        [Parameter]
        public Dictionary<string, object> Attributes { get; set; }

        [Parameter]
        public List<Dictionary<string, object>> PathAttributes { get; set; } = new List<Dictionary<string, object>>();

        [Parameter]
        public List<Dictionary<string, object>> TextAttributes { get; set; } = new List<Dictionary<string, object>>();

        [Parameter]
        public string ChartId { get; set; }

        [Parameter]
        public double LineWidth { get; set; }

        [Parameter]
        public string LineColor { get; set; }

        [Parameter]
        public string DashArray { get; set; }

        [Parameter]
        public string HorizontalDir { get; set; }

        [Parameter]
        public string VerticalDir { get; set; }

        internal bool IsFirstRender { get; set; }

        protected override void OnInitialized()
        {
            IsFirstRender = true;
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            horizontalId = ChartId + "_HorizontalLine";
            verticalId = ChartId + "_VerticalLine";
            axisTooltipId = ChartId + "_crosshair_axis";
        }

        internal void ChangeCrossHairValues(string horizontalCross, string verticalCross, List<Dictionary<string, object>> path, List<Dictionary<string, object>> text)
        {
            HorizontalDir = horizontalCross;
            VerticalDir = verticalCross;
            PathAttributes = path;
            TextAttributes = text;
            InvokeAsync(StateHasChanged);
        }

        internal void SetOpacity(double opacity)
        {
#pragma warning disable CA1305
            Attributes["opacity"] = opacity.ToString();
#pragma warning restore CA1305
            InvokeAsync(StateHasChanged);
        }

        internal void StartFadeOut(int duration = 100)
        {
            timer.Stop();
            timer = new Timer(duration / 100);
            timer.Elapsed += FadeOut;
            timer.Enabled = true;
            start = DateTime.Now;
            this.duration = duration;
        }

        private void FadeOut(object source, ElapsedEventArgs eventArgs)
        {
            TimeSpan t = eventArgs.SignalTime - start;
            if (t.TotalMilliseconds < duration)
            {
                SetOpacity(1 - (t.TotalMilliseconds / duration));
            }
            else
            {
                SetOpacity(0);
                ((Timer)source).Stop();
                ((Timer)source).Close();
                ((Timer)source).Dispose();
            }
        }

        internal void SetAttributes(Dictionary<string, object> groupAttributes, Dictionary<string, object> pathAttributes)
        {
            Attributes = groupAttributes;
            foreach (KeyValuePair<string, object> keyValue in pathAttributes)
            {
                switch (keyValue.Key)
                {
                    case "chartId":
                        ChartId = keyValue.Value as string;
                        break;
                    case "lineWidth":
                        LineWidth = Convert.ToDouble(keyValue.Value, null);
                        break;
                    case "lineColor":
                        LineColor = keyValue.Value as string;
                        break;
                    case "dashArray":
                        DashArray = keyValue.Value as string;
                        break;
                    case "horizontalDir":
                        HorizontalDir = keyValue.Value as string;
                        break;
                    case "verticalDir":
                        VerticalDir = keyValue.Value as string;
                        break;
                    case "pathAttributes":
                        PathAttributes = (List<Dictionary<string, object>>)keyValue.Value;
                        break;
                    case "textAttributes":
                        TextAttributes = (List<Dictionary<string, object>>)keyValue.Value;
                        break;
                }
            }

            IsFirstRender = false;
            InvokeAsync(StateHasChanged);
        }
    }
}