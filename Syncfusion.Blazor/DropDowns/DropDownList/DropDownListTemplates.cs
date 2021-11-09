using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.DropDowns.Internal;

namespace Syncfusion.Blazor.DropDowns
{
    /// <summary>
    /// The DropDownList has been provided with several options to customize each list item, group title, selected value, header, and footer elements.
    /// </summary>
    /// <typeparam name="TItem">Specifies the type of DropDownListTemplates.</typeparam>
    public partial class DropDownListTemplates<TItem> : DropDownsTemplates<TItem>
    {
        /// <summary>
        /// Accepts the template design and assigns it to the selected list item in the input element of the component.
        /// </summary>
        [Parameter]
        public RenderFragment<TItem> ValueTemplate { get; set; }

        /// <summary>
        /// Triggers while initial rendering of the component.
        /// </summary>
        /// <exclude/>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (ValueTemplate != null)
            {
                Parent.UpdateDropDownTemplate(nameof(ValueTemplate), null, ValueTemplate);
            }
        }
    }
}
