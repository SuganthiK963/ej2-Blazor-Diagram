using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Syncfusion.Blazor.Charts
{
    internal static class Extensions
    {
        public static void CreateCascadingValue<TValue>(this ComponentBase componentBase, RenderTreeBuilder builder, int seq, int seq0, TValue arg0, int seq1, RenderFragment arg1)
        {
            builder.OpenComponent<CascadingValue<TValue>>(seq);
            builder.AddAttribute(seq0, "Value", arg0);
            builder.AddAttribute(seq1, "ChildContent", arg1);
            builder.CloseComponent();
        }
    }
}
