using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// A class used for configuring the Accordion animation properties.
    /// </summary>
    public partial class AccordionAnimationSettings : SfBaseComponent
    {
        [CascadingParameter]
        private SfAccordion Parent { get; set; }

        /// <summary>
        /// Child Content for the Accordion Animation Settings.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Specifies the animation to appear while collapsing the Accordion item.
        /// </summary>
        public AccordionAnimationCollapse Collapse { get; set; }

        /// <summary>
        /// Specifies the animation to appear while expanding the Accordion item.
        /// </summary>
        public AccordionAnimationExpand Expand { get; set; }

        internal void UpdateExpandProperties(AccordionAnimationExpand animation)
        {
            Expand = animation ?? new AccordionAnimationExpand();
        }

        internal void UpdateCollapseProperties(AccordionAnimationCollapse animation)
        {
            Collapse = animation ?? new AccordionAnimationCollapse();
        }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateAnimationProperties(this);
            UpdateExpandProperties(Expand);
            UpdateCollapseProperties(Collapse);
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
        }
    }
}