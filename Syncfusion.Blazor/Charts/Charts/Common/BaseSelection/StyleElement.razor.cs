using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts.Internal
{
    public partial class StyleElement
    {
        private RenderFragment TemplateContent { get; set; }
        private bool allowStyles { get; set; }

#pragma warning disable CA1822 
        private RenderFragment RenderStyles(string styles) => builder =>
#pragma warning restore CA1822
        {
            builder.AddMarkupContent(1, styles);
        };

        internal void AppendStyleElement(string styleContent)
        {
            allowStyles = true;
            TemplateContent = RenderStyles(styleContent);
            InvokeAsync(StateHasChanged);
        }
    }
}