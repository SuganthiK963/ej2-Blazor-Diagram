using System;
using System.Collections.Generic;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using Syncfusion.Blazor.Inputs.Internal;
using Syncfusion.Blazor.Internal;
using System.ComponentModel;

namespace Syncfusion.Blazor.Inputs
{
    /// <summary>
    /// Specifies the list of files that will be preloaded on rendering of uploader component.
    /// </summary>
    public partial class SfUploader
    {
        /// <summary>
        /// It is used to convert bytes value into kilobytes or megabytes depending on the size based
        /// on [binary prefix](https://en.wikipedia.org/wiki/Binary_prefix).
        /// </summary>
        /// <param name="bytes">Specifies the file size in bytes.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<string> BytesToSize(double bytes)
        {
            return await InvokeMethod<string>("sfBlazor.Uploader.bytesToSize", false, new object[] { FileElement, bytes });
        }
        /// <summary>
        /// It is used to convert bytes value into kilobytes or megabytes depending on the size based
        /// on [binary prefix](https://en.wikipedia.org/wiki/Binary_prefix).
        /// </summary>
        /// <param name="bytes">Specifies the file size in bytes.</param>
        /// <returns>Task.</returns>
        public async Task<string> BytesToSizeAsync(double bytes)
        {
            return await BytesToSize(bytes);
        }
        /// <summary>
        /// Stops the in-progress chunked upload based on the file data.
        /// When the file upload is canceled, the partially uploaded file is removed from server.
        /// </summary>
        /// <param name="fileData">specifies the files data to cancel the progressing file.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Cancel(FileInfo[] fileData = null)
        {
            await InvokeMethod("sfBlazor.Uploader.cancel", new object[] { FileElement, fileData });
        }
        /// <summary>
        /// Stops the in-progress chunked upload based on the file data.
        /// When the file upload is canceled, the partially uploaded file is removed from server.
        /// </summary>
        /// <param name="fileData">specifies the files data to cancel the progressing file.</param>
        /// <returns>Task.</returns>
        public async Task CancelAsync(FileInfo[] fileData = null)
        {
            await Cancel(fileData);
        }
        /// <summary>
        /// Clear all the file entries from list that can be uploaded files or added in upload queue.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task ClearAll()
        {
            await InvokeMethod("sfBlazor.Uploader.clearAll", new object[] { FileElement });
        }
        /// <summary>
        /// Clear all the file entries from list that can be uploaded files or added in upload queue.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task ClearAllAsync()
        {
            await ClearAll();
        }
        /// <summary>
        /// Create the file list for specified files data.
        /// </summary>
        /// <param name="fileData">Specifies the file data.</param>
        /// <param name="isSelectedFile">true if the file is selected.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task CreateFileList(FileInfo[] fileData, bool? isSelectedFile = null)
        {
            await InvokeMethod("sfBlazor.Uploader.createFileList", new object[] { FileElement, fileData, isSelectedFile });
        }
        /// <summary>
        /// Get the data of files which are shown in file list.
        /// </summary>
        /// <param name="index">Specifies the index.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<List<FileInfo>> GetFilesData(double? index = null)
        {
            return await InvokeMethod<List<FileInfo>>("sfBlazor.Uploader.getFilesData", false, new object[] { FileElement, index });
        }
        /// <summary>
        /// Get the data of files which are shown in file list.
        /// </summary>
        /// <param name="index">Specifies the index.</param>
        /// <returns>Task.</returns>
        public async Task<List<FileInfo>> GetFilesDataAsync(Nullable<double> index = null)
        {
            return await GetFilesData(index);
        }
        /// <summary>
        /// Pauses the in-progress chunked upload based on the file data.
        /// </summary>
        /// <param name="fileData">specifies the files data to pause from uploading.</param>
        /// <param name="custom">Set true if used custom UI.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Pause(List<FileInfo> fileData = null, bool? custom = null)
        {
            await InvokeMethod("sfBlazor.Uploader.pause", new object[] { FileElement, fileData, custom });
        }
        /// <summary>
        /// Pauses the in-progress chunked upload based on the file data.
        /// </summary>
        /// <param name="fileData">specifies the files data to pause from uploading.</param>
        /// <param name="custom">Set true if used custom UI.</param>
        /// <returns>Task.</returns>
        public async Task PauseAsync(List<FileInfo> fileData = null, Nullable<bool> custom = null)
        {
            await Pause(fileData, custom);
        }
        /// <summary>
        /// Remove the uploaded file from server manually by calling the remove URL action.
        /// <para>If you pass an empty argument to this method, the complete file list can be cleared,
        /// otherwise remove the specific file based on its argument ("file_data").</para>
        /// </summary>
        /// <param name="fileData">specifies the files data to remove from file list/server.</param>
        /// <param name="customTemplate">Set true if the component rendering with customize template.</param>
        /// <param name="removeDirectly">Set true if files remove without removing event.</param>
        /// <param name="postRawFile">Set false, to post file name only to the remove action.</param>
        /// <param name="args"></param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Remove(FileInfo[] fileData = null, bool? customTemplate = null, bool? removeDirectly = null, bool? postRawFile = null, object args = null)
        {
            await InvokeMethod("sfBlazor.Uploader.remove", new object[] { FileElement, fileData, customTemplate, removeDirectly, postRawFile, args });
        }
        /// <summary>
        /// Remove the uploaded file from server manually by calling the remove URL action.
        /// <para>If you pass an empty argument to this method, the complete file list can be cleared,
        /// otherwise remove the specific file based on its argument ("file_data").</para>
        /// </summary>
        /// <param name="fileData">specifies the files data to remove from file list/server.</param>
        /// <param name="customTemplate">Set true if the component rendering with customize template.</param>
        /// <param name="removeDirectly">Set true if files remove without removing event.</param>
        /// <param name="postRawFile">Set false, to post file name only to the remove action.</param>
        /// <param name="args"></param>
        /// <returns>Task.</returns>
        public async Task RemoveAsync(FileInfo[] fileData = null, Nullable<bool> customTemplate = null, Nullable<bool> removeDirectly = null, Nullable<bool> postRawFile = null, object args = null)
        {
            await Remove(fileData, customTemplate, removeDirectly, postRawFile, args);
        }
        /// <summary>
        /// Resumes the chunked upload that is previously paused based on the file data.
        /// </summary>
        /// <param name="fileData">specifies the files data to resume the paused file.</param>
        /// <param name="custom"></param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Resume(FileInfo[] fileData = null, bool? custom = null)
        {
            await InvokeMethod("sfBlazor.Uploader.resume", new object[] { FileElement, fileData, custom });
        }
        /// <summary>
        /// Resumes the chunked upload that is previously paused based on the file data.
        /// </summary>
        /// <param name="fileData">specifies the files data to resume the paused file.</param>
        /// <param name="custom"></param>
        /// <returns>Task.</returns>
        public async Task ResumeAsync(FileInfo[] fileData = null, Nullable<bool> custom = null)
        {
            await Resume(fileData, custom);
        }
        /// <summary>
        /// Retries the canceled or failed file upload based on the file data.
        /// <param name="fileData">specifies the files data to retry the canceled or failed file</param>
        /// <param name="fromcanceledStage">Set true to retry from canceled stage and set false to retry from initial stage.</param>
        /// <param name="custom"></param>
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Retry(FileInfo[] fileData = null, bool? fromcanceledStage = null, bool? custom = null)
        {
            await InvokeMethod("sfBlazor.Uploader.retry", new object[] { FileElement, fileData, fromcanceledStage, custom });
        }
        /// <summary>
        /// Retries the canceled or failed file upload based on the file data.
        /// <param name="fileData">specifies the files data to retry the canceled or failed file</param>
        /// <param name="fromcanceledStage">Set true to retry from canceled stage and set false to retry from initial stage.</param>
        /// <param name="custom"></param>
        /// </summary>
        /// <returns>Task.</returns>
        public async Task RetryAsync(FileInfo[] fileData = null, Nullable<bool> fromcanceledStage = null, Nullable<bool> custom = null)
        {
            await Retry(fileData, fromcanceledStage, custom);
        }
        /// <summary>
        /// Allows you to sort the file data alphabetically based on its file name clearly.
        /// </summary>
        /// <param name="filesData">specifies the files data for upload.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<List<FileInfo>> SortFileList(FileInfo[] filesData = null)
        {
            return await InvokeMethod<List<FileInfo>>("sortFileList", false, new object[] { FileElement, filesData });
        }
        /// <summary>
        /// Allows you to sort the file data alphabetically based on its file name clearly.
        /// </summary>
        /// <param name="filesData">specifies the files data for upload.</param>
        /// <returns>Task.</returns>
        public async Task<List<FileInfo>> SortFileListAsync(FileInfo[] filesData = null)
        {
            return await SortFileList(filesData);
        }
        /// <summary>
        /// Allows you to call the upload process manually by calling save URL action.
        /// <para>To process the selected files (added in upload queue), pass an empty argument otherwise
        /// upload the specific file based on its argument.</para>
        /// </summary>
        /// <param name="files">specifies the files data for upload.</param>
        /// <param name="custom">specifies the custom files.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Upload(FileInfo[] files = null, bool? custom = null)
        {
            await InvokeMethod("sfBlazor.Uploader.upload", new object[] { FileElement, files, custom });
        }
        /// <summary>
        /// Allows you to call the upload process manually by calling save URL action.
        /// <para>To process the selected files (added in upload queue), pass an empty argument otherwise
        /// upload the specific file based on its argument.</para>
        /// </summary>
        /// <param name="files">specifies the files data for upload.</param>
        /// <param name="custom">specifies the custom files.</param>
        /// <returns>Task.</returns>
        public async Task UploadAsync(FileInfo[] files = null, Nullable<bool> custom = null)
        {
            await Upload(files, custom);
        }
        /// <summary>
        /// Task which retrieves the file data.
        /// </summary>
        /// <param name="file">Specifies the file.</param>
        /// <returns>Task.</returns>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task GetFileDetails(List<FileInfo> file)
        {
            await GetFilesDetails(file);
        }

        /// <summary>
        /// Task which creates the file list.
        /// </summary>
        /// <param name="fileData">Specifies the file data.</param>
        /// <param name="isForm">true if the component inside form.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task CreateFileList(List<UploadFileDetails> fileData, bool isForm)
        {
            await ServerCreateFileList(fileData, isForm);
        }

        /// <summary>
        /// Task which clears the file list.
        /// </summary>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ClearAllFile()
        {
            ClearAllFileList();
        }

        /// <summary>
        /// Method which removes the file data.
        /// </summary>
        /// <param name="index">Specifies the index.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task RemoveFileData(int index)
        {
            await RemoveFileList(index);
        }

        /// <summary>
        /// Method which update the file data  in server.
        /// </summary>
        /// <param name="fileData">Specifies the filedata.</param>
        /// <param name="isForm">true if the component rendered inside the form component.</param>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void UpdateServerFileData(List<UploadFileDetails> fileData, bool isForm)
        {
            ServerFileData(fileData, isForm);
        }

        /// <summary>
        /// Task which specifies the selected event.
        /// </summary>
        /// <param name="args">Selected event arguments.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<SelectedEventArgs> SelectedEvent(SelectedEventArgs args)
        {
            var selectArgs = args;
            if (UploaderEvents != null && UploaderEvents.FileSelected.HasDelegate && string.IsNullOrEmpty(UploadAsyncSettings?.SaveUrl) && AutoUpload)
            {
                IsShowFileList = false;
            }

            await SfBaseUtils.InvokeEvent(UploaderEvents?.FileSelected, selectArgs);
            return selectArgs;
        }

        /// <summary>
        /// Task which specifies the RemovingEvent.
        /// </summary>
        /// <param name="args">Specifies the removing event arguments.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<RemovingEventArgs> RemovingEvent(RemovingEventArgs args)
        {
            var removeArgs = args;
            await SfBaseUtils.InvokeEvent(UploaderEvents?.OnRemove, removeArgs);
            return removeArgs;
        }

        /// <summary>
        /// Task which specifies the action complete event.
        /// </summary>
        /// <param name="args">Specifies the action complete event arguments.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<ActionCompleteEventArgs> ActionCompleteEvent(ActionCompleteEventArgs args)
        {
            var actionArgs = args;
            await SfBaseUtils.InvokeEvent(UploaderEvents?.OnActionComplete, actionArgs);
            return actionArgs;
        }

        /// <summary>
        /// Task which specifies the success event .
        /// </summary>
        /// <param name="args">Specifies the success event arguments.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<SuccessEventArgs> SuccessEvent(SuccessEventArgs args)
        {
            var successArgs = args;
            await SfBaseUtils.InvokeEvent(UploaderEvents?.Success, successArgs);
            return successArgs;
        }

        /// <summary>
        /// Task which specifies the change event.
        /// </summary>
        /// <param name="args">Specifies the upload change event arguments.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task ChangeEvent(UploadChangeEventArgs args)
        {
            if (!string.IsNullOrEmpty(UploadAsyncSettings?.SaveUrl) && args != null)
            {
                args.Files = null;
            }

            await SfBaseUtils.InvokeEvent(UploaderEvents?.ValueChange, args);
        }

        /// <summary>
        /// Task which specifies the failure event.
        /// </summary>
        /// <param name="args">FailureEventArgs arguments.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<FailureEventArgs> FailureEvent(FailureEventArgs args)
        {
            var failArgs = args;
            await SfBaseUtils.InvokeEvent(UploaderEvents?.OnFailure, failArgs);
#pragma warning disable CS0618 // Type or member is obsolete
            await SfBaseUtils.InvokeEvent(UploaderEvents?.OnFailured, failArgs);
#pragma warning restore CS0618 // Type or member is obsolete
            return failArgs;
        }

        /// <summary>
        /// Task which specifies the chunk failure event.
        /// </summary>
        /// <param name="args">FailureEventArgs arguments.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [JSInvokable]
        public async Task<FailureEventArgs> ChunkFailureEvent(FailureEventArgs args)
        {
            var failArgs = args;
            await SfBaseUtils.InvokeEvent(UploaderEvents?.OnChunkFailure, failArgs);
#pragma warning disable CS0618 // Type or member is obsolete
            await SfBaseUtils.InvokeEvent(UploaderEvents?.OnChunkFailured, failArgs);
#pragma warning restore CS0618 // Type or member is obsolete
            return failArgs;
        }

        /// <summary>
        /// Task which specifies the file list rendering event.
        /// </summary>
        /// <param name="args">FileListRenderingEventArgs arguments.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task FileListRenderingEvent(FileListRenderingEventArgs args)
        {
            await SfBaseUtils.InvokeEvent(UploaderEvents?.OnFileListRender, args);
        }

        /// <summary>
        /// Task which specifies the progress event.
        /// </summary>
        /// <param name="args">ProgressEventArgs arguments.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<ProgressEventArgs> ProgressEvent(ProgressEventArgs args)
        {
            await SfBaseUtils.InvokeEvent(UploaderEvents?.Progressing, args);
            return args;
        }

        /// <summary>
        /// Task which specifies the canceling event.
        /// </summary>
        /// <param name="args">CancelEventArgs arguments.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<CancelEventArgs> CancelingEvent(CancelEventArgs args)
        {
            var cancelArgs = args;
            await SfBaseUtils.InvokeEvent(UploaderEvents?.OnCancel, cancelArgs);
            return cancelArgs;
        }

        /// <summary>
        /// Task which specifies the uploading event.
        /// </summary>
        /// <param name="args">UploadingEventArgs arguments.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<UploadingEventArgs> UploadingEvent(UploadingEventArgs args)
        {
            var uploadArgs = args;
            await SfBaseUtils.InvokeEvent(UploaderEvents?.OnUploadStart, uploadArgs);
            return uploadArgs;
        }

        /// <summary>
        /// Task which specifies the chunk uploading event.
        /// </summary>
        /// <param name="args">UploadingEventArgs arguments.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<UploadingEventArgs> ChunkUploadingEvent(UploadingEventArgs args)
        {
            var uploadArgs = args;
            await SfBaseUtils.InvokeEvent(UploaderEvents?.OnChunkUploadStart, uploadArgs);
            return uploadArgs;
        }

        /// <summary>
        /// Task which specifies the chunk success event.
        /// </summary>
        /// <param name="args">SuccessEventArgs arguments.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<SuccessEventArgs> ChunkSuccessEvent(SuccessEventArgs args)
        {
            var successArgs = args;
            await SfBaseUtils.InvokeEvent(UploaderEvents?.OnChunkSuccess, successArgs);
            return successArgs;
        }

        /// <summary>
        /// Task which specifies the pausing event.
        /// </summary>
        /// <param name="args">PauseResumeEventArgs arguments.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<PauseResumeEventArgs> PausingEvent(PauseResumeEventArgs args)
        {
            var pauseArgs = args;
            await SfBaseUtils.InvokeEvent(UploaderEvents?.Paused, pauseArgs);
            return pauseArgs;
        }

        /// <summary>
        /// Task which specifies the resuming event.
        /// </summary>
        /// <param name="args">PauseResumeEventArgs arguments.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<PauseResumeEventArgs> ResumingEvent(PauseResumeEventArgs args)
        {
            var resumeArgs = args;
            await SfBaseUtils.InvokeEvent(UploaderEvents?.OnResume, resumeArgs);
            return resumeArgs;
        }

        /// <summary>
        /// Task which specifies the before upload event.
        /// </summary>
        /// <param name="args">BeforeUploadEventArgs arguments.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<BeforeUploadEventArgs> BeforeUploadEvent(BeforeUploadEventArgs args)
        {
            var beforeUploadArgs = args;
            await SfBaseUtils.InvokeEvent(UploaderEvents?.BeforeUpload, beforeUploadArgs);
            return beforeUploadArgs;
        }

        /// <summary>
        /// Task specifies the before remove event.
        /// </summary>
        /// <param name="args">BeforeRemoveEventArgs arguments.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<BeforeRemoveEventArgs> BeforeRemoveEvent(BeforeRemoveEventArgs args)
        {
            var beforeRemoveArgs = args;
            await SfBaseUtils.InvokeEvent(UploaderEvents?.BeforeRemove, beforeRemoveArgs);
            return beforeRemoveArgs;
        }

        /// <summary>
        /// Task specifies the clearing event.
        /// </summary>
        /// <param name="args">ClearingEventArgs arguments.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<ClearingEventArgs> ClearingEvent(ClearingEventArgs args)
        {
            var clearingArgs = args;
            await SfBaseUtils.InvokeEvent(UploaderEvents?.OnClear, clearingArgs);
            return clearingArgs;
        }
    }
}
