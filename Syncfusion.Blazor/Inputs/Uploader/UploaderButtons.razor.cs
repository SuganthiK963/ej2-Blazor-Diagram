using System;
using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Syncfusion.Blazor.Inputs
{
    /// <summary>
    /// Customize the default text of browse, clear, and upload buttons with plain text.
    /// </summary>
    public partial class UploaderButtons : SfBaseComponent
    {
        [CascadingParameter]
        private SfUploader parent { get; set; }

        /// <summary>
        /// Specifies the content that has to be passed.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Specifies the text or html content to browse button.
        /// </summary>
        [Parameter]
        public string Browse { get; set; } = "Browse...";

        private string _browse { get; set; }

        /// <summary>
        /// Specifies the text or html content to clear button.
        /// </summary>
        [Parameter]
        public string Clear { get; set; } = "Clear";

        private string _clear { get; set; }

        /// <summary>
        /// Specifies the text or html content to upload button.
        /// </summary>
        [Parameter]
        public string Upload { get; set; } = "Upload";

        private string _upload { get; set; }

        /// <summary>
        /// Triggers while initial rendering of the component.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            parent?.UpdateChildProperties("Buttons", this);
            _browse = Browse;
            _clear = Clear;
            _upload = Upload;
            await parent?.CallStateHasChangedAsync();
        }

        /// <summary>
        /// Triggers while dynamically updating the component properties.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            _browse = NotifyPropertyChanges(nameof(Browse), Browse, _browse);
            _clear = NotifyPropertyChanges(nameof(Clear), Clear, _clear);
            _upload = NotifyPropertyChanges(nameof(Upload), Upload, _upload);
            if (PropertyChanges.Count > 0 && IsRendered && parent != null)
            {
                parent.UpdateChildProperties("Buttons", this);
                var options = parent.GetProperty();
                var buttonProps = new Dictionary<string, object>() { { "Buttons", this } };
                await InvokeMethod("sfBlazor.Uploader.propertyChanges", new object[] { parent.FileElement, options, buttonProps });
            }
        }

        internal override void ComponentDispose()
        {
            parent = null;
        }
    }
}