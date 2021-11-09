using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.ComponentModel;

namespace Syncfusion.Blazor.DataVizCommon
{
    public partial class SvgText
    {
        [Parameter]
        public string X { get; set; }
        [Parameter]
        public string Y { get; set; }
        [Parameter]
        public string Id { get; set; }
        [Parameter]
        public string Fill { get; set; }
        [Parameter]
        public string FontSize { get; set; }
        [Parameter]
        public string FontStyle { get; set; }
        [Parameter]
        public string FontFamily { get; set; }
        [Parameter]
        public string FontWeight { get; set; }
        [Parameter]
        public string TextAnchor { get; set; }
        [Parameter]
        public string Text { get; set; }
        [Parameter]
        public RenderFragment ChildContent { get; set; }
        [Parameter]
        public string DominantBaseline { get; set; }
        [Parameter]
        public string Transform { get; set; }
        [Parameter]
        public string AccessibilityText { get; set; } = string.Empty;
        [Parameter]
        public string TabIndex { get; set; } = string.Empty;
        [Parameter]
        public string Style { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
#pragma warning disable CA2227 
        public Dictionary<string, object> HtmlAttributes { get { return htmlAttributes; } set { htmlAttributes = value; } }
#pragma warning restore CA2227 

        private Dictionary<string, object> htmlAttributes { get; set; }

        private double opacity { get; set; } = 1;
        internal void ChangeText(string text, string color = null)
        {
            Text = text;
            if (color != null)
            {
                Fill = color;
            }
            StateHasChanged();
        }
        internal void ChangeOpacity(double opacity)
        {
            this.opacity = opacity;
            InvokeAsync(StateHasChanged);
        }

        internal void Dispose()
        {
            Text = null;
            ChildContent = null;
        }
    }
}