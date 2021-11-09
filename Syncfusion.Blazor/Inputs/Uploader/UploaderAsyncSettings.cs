using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using System.Collections.Generic;

namespace Syncfusion.Blazor.Inputs
{
    /// <summary>
    /// Specifies the Uploader Events of the component.
    /// </summary>
    public class UploaderAsyncSettings : SfBaseComponent
    {
        [CascadingParameter]
        private SfUploader Parent { get; set; }

        /// <summary>
        /// Specifies the content that has to be passed.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Specifies the ChunkSize to split the large file into chunks, and upload it to the server in a sequential order.
        /// <para>If the ChunkSize property has value, the Uploader enables the chunk upload by default.
        /// It must be specified in bytes value.</para>
        /// </summary>
        [Parameter]
        public double ChunkSize { get; set; }

        private double _chunkSize { get; set; }

        /// <summary>
        /// Specifies the URL of remove action that receives the file information and handle the remove operation in server.
        /// <para>The remove action type must be POST request and define "RemoveFileNames" attribute to get file information that will be removed.
        /// This property is optional.</para>
        /// </summary>
        [Parameter]
        public string RemoveUrl { get; set; } = string.Empty;

        private string _removeUrl { get; set; }

        /// <summary>
        /// Specifies the delay time in milliseconds that the automatic retry happens after the delay.
        /// </summary>
        [Parameter]
        public double RetryAfterDelay { get; set; } = 500;

        private double _retryAfterDelay { get; set; }

        /// <summary>
        /// Specifies the number of retries that the Uploader can perform on the file failed to upload.
        /// By default, the Uploader set 3 as maximum retries. This property must be specified to prevent infinity looping.
        /// </summary>
        [Parameter]
        public int RetryCount { get; set; } = 3;

        private int _retryCount { get; set; }

        /// <summary>
        /// Specifies the URL of save action that will receive the upload files and save in the server.
        /// <para>The save action type must be POST request and define the argument as same input name used to render the component.
        /// The upload operations could not perform without this property.</para>
        /// </summary>
        [Parameter]
        public string SaveUrl { get; set; } = string.Empty;

        private string _saveUrl { get; set; }

        /// <summary>
        /// Triggers while initial rendering of the component.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent?.UpdateChildProperties("AsyncSettings", this);
            _chunkSize = ChunkSize;
            _removeUrl = RemoveUrl;
            _retryAfterDelay = RetryAfterDelay;
            _retryCount = RetryCount;
            _saveUrl = SaveUrl;
            await Parent?.CallStateHasChangedAsync();
        }

        /// <summary>
        /// Triggers after the component was rendered.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            _chunkSize = NotifyPropertyChanges(nameof(ChunkSize), ChunkSize, _chunkSize);
            _removeUrl = NotifyPropertyChanges(nameof(RemoveUrl), RemoveUrl, _removeUrl);
            _retryAfterDelay = NotifyPropertyChanges(nameof(RetryAfterDelay), RetryAfterDelay, _retryAfterDelay);
            _retryCount = NotifyPropertyChanges(nameof(RetryCount), RetryCount, _retryCount);
            _saveUrl = NotifyPropertyChanges(nameof(SaveUrl), SaveUrl, _saveUrl);
            if (PropertyChanges.Count > 0 && IsRendered && Parent != null)
            {
                Parent.UpdateChildProperties("AsyncSettings", this);
                var options = Parent.GetProperty();
                var asyncProps = new Dictionary<string, object>() { { "AsyncSettings", this } };
                await InvokeMethod("sfBlazor.Uploader.propertyChanges", new object[] { Parent.FileElement, options, asyncProps });
            }
        }

        internal override void ComponentDispose()
        {
            Parent = null;
        }
    }
}