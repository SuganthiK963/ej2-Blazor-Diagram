using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Spinner
{
    /// <summary>
    /// Configure event handlers for the Spinner component.
    /// </summary>
    public class SpinnerEvents : SfBaseComponent
    {
        [CascadingParameter]
        private SfSpinner BaseParent { get; set; }

        /// <summary>
        /// Event triggers after the Spinner is created.
        /// </summary>
        [Parameter]
        public EventCallback<object> Created { get; set; }

        /// <summary>
        /// Event triggers before the Spinner is opened.
        /// </summary>
        [Parameter]
        public EventCallback<SpinnerEventArgs> OnBeforeOpen { get; set; }

        /// <summary>
        /// Event triggers before the Spinner is closed.
        /// </summary>
        [Parameter]
        public EventCallback<SpinnerEventArgs> OnBeforeClose { get; set; }

        /// <summary>
        /// Event triggers after the Spinner is destroyed.
        /// </summary>
        [Parameter]
        public EventCallback<object> Destroyed { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            BaseParent.Delegates = this;
        }

        internal override void ComponentDispose()
        {
            BaseParent = null;
        }
    }
}