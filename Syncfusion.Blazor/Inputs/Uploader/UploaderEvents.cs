using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Inputs
{
    /// <summary>
    /// Specifies the Uploader Events of the component.
    /// </summary>
    public class UploaderEvents : SfBaseComponent
    {
        [CascadingParameter]
        private SfUploader BaseParent { get; set; }

        /// <summary>
        /// Triggers after all the selected files has processed to upload successfully or failed to server.
        /// </summary>
        [Parameter]
        public EventCallback<ActionCompleteEventArgs> OnActionComplete { get; set; }

        /// <summary>
        /// Triggers on remove the uploaded file. The event used to get confirm before remove the file from server.
        /// </summary>
        [Parameter]
        public EventCallback<BeforeRemoveEventArgs> BeforeRemove { get; set; }

        /// <summary>
        /// Triggers when the upload process before. This event is used to add additional parameter with upload request.
        /// </summary>
        [Parameter]
        public EventCallback<BeforeUploadEventArgs> BeforeUpload { get; set; }

        /// <summary>
        /// Fires if cancel the chunk file uploading.
        /// </summary>
        [Parameter]
        public EventCallback<CancelEventArgs> OnCancel { get; set; }

        /// <summary>
        /// Triggers when changes occur in uploaded file list by selecting or dropping files.
        /// </summary>
        [Parameter]
        public EventCallback<UploadChangeEventArgs> ValueChange { get; set; }

        /// <summary>
        /// Fires if the chunk file failed to upload.
        /// </summary>
        [Obsolete("This OnChunkFailured event is deprecated. Use OnChunkFailure event to achieve the functionality.")]
        [Parameter]
        public EventCallback<FailureEventArgs> OnChunkFailured { get; set; }

        /// <summary>
        /// Fires if the chunk file failed to upload.
        /// </summary>
        [Parameter]
        public EventCallback<FailureEventArgs> OnChunkFailure { get; set; }

        /// <summary>
        /// Fires when the chunk file uploaded successfully.
        /// </summary>
        [Parameter]
        public EventCallback<SuccessEventArgs> OnChunkSuccess { get; set; }

        /// <summary>
        /// Fires when every chunk upload process gets started. This event is used to add additional parameter with upload request.
        /// </summary>
        [Parameter]
        public EventCallback<UploadingEventArgs> OnChunkUploadStart { get; set; }

        /// <summary>
        /// Triggers before clearing the items in file list when clicking "clear".
        /// </summary>
        [Parameter]
        public EventCallback<ClearingEventArgs> OnClear { get; set; }

        /// <summary>
        /// Triggers when the component is created.
        /// </summary>
        [Parameter]
        public EventCallback<object> Created { get; set; }

        /// <summary>
        /// Triggers when the AJAX request fails on uploading or removing files.
        /// </summary>
        [Obsolete("This OnFailured event is deprecated. Use OnFailured event to achieve the functionality.")]
        [Parameter]
        public EventCallback<FailureEventArgs> OnFailured { get; set; }

        /// <summary>
        /// Triggers when the AJAX request fails on uploading or removing files.
        /// </summary>
        [Parameter]
        public EventCallback<FailureEventArgs> OnFailure { get; set; }

        /// <summary>
        /// Triggers before rendering each file item from the file list in a page.
        /// It helps to customize specific file item structure.
        /// </summary>
        [Parameter]
        public EventCallback<FileListRenderingEventArgs> OnFileListRender { get; set; }

        /// <summary>
        /// Fires if pause the chunk file uploading.
        /// </summary>
        [Parameter]
        public EventCallback<PauseResumeEventArgs> Paused { get; set; }

        /// <summary>
        /// Triggers when uploading a file to the server using the AJAX request.
        /// </summary>
        [Parameter]
        public EventCallback<ProgressEventArgs> Progressing { get; set; }

        /// <summary>
        /// Triggers on removing the uploaded file. The event used to get confirm before removing the file from server.
        /// </summary>
        [Parameter]
        public EventCallback<RemovingEventArgs> OnRemove { get; set; }

        /// <summary>
        /// DEPRECATED-Triggers before rendering each file item from the file list in a page.
        /// It helps to customize specific file item structure.
        /// </summary>
        [Parameter]
        public EventCallback<RenderingEventArgs> Rendering { get; set; }

        /// <summary>
        /// Fires if resume the paused chunk file upload.
        /// </summary>
        [Parameter]
        public EventCallback<PauseResumeEventArgs> OnResume { get; set; }

        /// <summary>
        /// Triggers after selecting or dropping the files by adding the files in upload queue.
        /// </summary>
        [Parameter]
        public EventCallback<SelectedEventArgs> FileSelected { get; set; }

        /// <summary>
        /// Triggers when the AJAX request gets success on uploading files or removing files.
        /// </summary>
        [Parameter]
        public EventCallback<SuccessEventArgs> Success { get; set; }

        /// <summary>
        /// Triggers when the upload process gets started. This event is used to add additional parameter with upload request.
        /// </summary>
        [Parameter]
        public EventCallback<UploadingEventArgs> OnUploadStart { get; set; }

        /// <summary>
        /// Triggers while initial rendering of the component.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            BaseParent.UploaderEvents = this;
        }

        internal override void ComponentDispose()
        {
            BaseParent = null;
        }
    }
}