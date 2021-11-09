using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.SplitButtons
{
    /// <summary>
    /// Used to configure the progress button events.
    /// </summary>
    public partial class ProgressButtonEvents : SfBaseComponent
    {
        [CascadingParameter]
        private SfProgressButton parent { get; set; }

        /// <summary>
        /// Triggers when the progress starts.
        /// </summary>
        [Parameter]
        public EventCallback<ProgressEventArgs> OnBegin { get; set; }

        /// <summary>
        /// Triggers once the component rendering is completed.
        /// </summary>
        [Parameter]
        public EventCallback<object> Created { get; set; }

        /// <summary>
        /// Triggers when the progress is completed.
        /// </summary>
        [Parameter]
        public EventCallback<ProgressEventArgs> OnEnd { get; set; }

        /// <summary>
        /// Triggers when the progress is incomplete.
        /// </summary>
        [Parameter]
        public EventCallback<Exception> OnFailure { get; set; }

        /// <summary>
        /// Triggers in specified intervals.
        /// </summary>
        [Parameter]
        public EventCallback<ProgressEventArgs> Progressing { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            parent.Delegates = this;
        }
    }
}
