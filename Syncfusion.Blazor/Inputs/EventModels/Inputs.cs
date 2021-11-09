using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Inputs
{
    /// <summary>
    /// SPecifies the form event arguments.
    /// </summary>
    public class FormEventArgs
    {
        /// <summary>
        /// Returns the input element.
        /// </summary>
        [JsonProperty("element")]
        public DOM Element { get; set; }

        /// <summary>
        /// Returns the error element for corresponding input.
        /// </summary>
        [JsonProperty("errorElement")]
        public DOM ErrorElement { get; set; }

        /// <summary>
        /// Returns the name of the input element.
        /// </summary>
        [JsonProperty("inputName")]
        public string InputName { get; set; }

        /// <summary>
        /// Returns the error message.
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        /// Returns the status input element.
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }
    }

    /// <summary>
    /// Defines the argument for the blur event.
    /// </summary>
    public class BlurEventArgs
    {
        /// <summary>
        /// returns the model class.
        /// </summary>
        [JsonProperty("model")]
        public object Model { get; set; }
    }

    /// <summary>
    /// Defines the argument for the focus event.
    /// </summary>
    public class FocusEventArgs
    {
        /// <summary>
        /// returns the model class.
        /// </summary>
        [JsonProperty("model")]
        public object Model { get; set; }
    }

    /// <summary>
    /// Default required properties for input components.
    /// </summary>
    public class IInput
    {
        /// <summary>
        ///  Sets the change event mapping function to input.
        /// </summary>
        [JsonProperty("change")]
        public object Change { get; set; }

        /// <summary>
        ///  Sets the css class value to input.
        /// </summary>
        [JsonProperty("cssClass")]
        public string CssClass { get; set; }

        /// <summary>
        ///  Sets the enable rtl value to input.
        /// </summary>
        [JsonProperty("enableRtl")]
        public bool EnableRtl { get; set; }

        /// <summary>
        ///  Sets the enabled value to input.
        /// </summary>
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        /// <summary>
        /// Specifies how the floating label works.
        /// Possible values are:
        ///  Never - Never float the label in the input when the placeholder is available.
        ///  Always - The floating label will always float above the input.
        ///  Auto - The floating label will float above the input after focusing or entering a value in the input.
        /// </summary>
        [JsonProperty("floatLabelType")]
        public object FloatLabelType { get; set; }

        /// <summary>
        ///  Sets the placeholder value to input.
        /// </summary>
        [JsonProperty("placeholder")]
        public string Placeholder { get; set; }

        /// <summary>
        ///  Sets the readonly value to input.
        /// </summary>
        [JsonProperty("readonly")]
        public bool Readonly { get; set; }

        /// <summary>
        ///  Specifies whether to display the Clear button in the input.
        /// </summary>
        [JsonProperty("showClearButton")]
        public bool ShowClearButton { get; set; }
    }
    /// <summary>
    /// Defines the argument for the ActionComplete event.
    /// </summary>
    public class ActionCompleteEventArgs
    {
        /// <summary>
        /// Return the selected file details.
        /// </summary>
        [JsonProperty("fileData")]
        public List<FileInfo> FileData { get; set; }
    }
    /// <summary>
    /// Defines the argument for the BeforeRemove event.
    /// </summary>
    public class BeforeRemoveEventArgs
    {
        /// <summary>
        /// Defines whether the current action can be prevented.
        /// </summary>
        [JsonProperty("cancel")]
        public bool Cancel { get; set; }

        /// <summary>
        /// Returns the XMLHttpRequest instance that is associated with remove action.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("currentRequest")]
        public object CurrentRequest { get; set; } = null;

        /// <summary>
        /// Returns the list of files details that will be removed.
        /// </summary>
        [JsonProperty("filesData")]
        public List<FileInfo> FilesData { get; set; }

        /// <summary>
        /// Defines the additional data with key and value pair format that will be submitted to the remove action.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("customFormData")]
        public object CustomFormData { get; set; } = null;
        /// <summary>
        /// Defines whether the selected raw file send to server remove action.
        /// Set true to send raw file.
        /// Set false to send file name only.
        /// </summary>
        [JsonProperty("postRawFile")]
        public bool PostRawFile { get; set; } = true;
    }
    /// <summary>
    /// Defines the argument for the BeforeUpload event.
    /// </summary>
    public class BeforeUploadEventArgs
    {
        /// <summary>
        /// Defines whether the current action can be prevented.
        /// </summary>
        [JsonProperty("cancel")]
        public bool Cancel { get; set; }

        /// <summary>
        /// Returns the XMLHttpRequest instance that is associated with upload action.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("currentRequest")]
        public object CurrentRequest { get; set; } = null;

        /// <summary>
        /// Returns the list of uploading files.
        /// </summary>
        [JsonProperty("filesData")]
        public List<FileInfo> FilesData { get; set; }

        /// <summary>
        /// Defines the additional data in key and value pair format that will be submitted to the upload action.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("customFormData")]
        public object CustomFormData { get; set; } = null;
    }
    /// <summary>
    /// Defines the argument for the Cancel event.
    /// </summary>
    public class CancelEventArgs
    {
        /// <summary>
        /// Defines whether the current action can be prevented.
        /// </summary>
        [JsonProperty("cancel")]
        public bool Cancel { get; set; }

        /// <summary>
        /// Returns the original event arguments.
        /// </summary>
        [JsonProperty("event")]
        public object Event { get; set; }

        /// <summary>
        /// Returns the file details that will be canceled.
        /// </summary>
        [JsonProperty("fileData")]
        public FileInfo FileData { get; set; }
    }
    /// <summary>
    /// Defines the argument for the Clearing event.
    /// </summary>
    public class ClearingEventArgs
    {
        /// <summary>
        /// Defines whether the current action can be prevented.
        /// </summary>
        [JsonProperty("cancel")]
        public bool Cancel { get; set; }

        /// <summary>
        /// Returns the list of files that will be cleared from the FileList.
        /// </summary>
        [JsonProperty("filesData")]
        public List<FileInfo> FilesData { get; set; }
    }
    /// <summary>
    /// Defines the argument for the Failure event.
    /// </summary>
    public class FailureEventArgs
    {
        /// <summary>
        /// Returns the upload chunk index.
        /// </summary>
        [JsonProperty("chunkIndex")]
        public double ChunkIndex { get; set; }

        /// <summary>
        /// Returns the upload chunk size.
        /// </summary>
        [JsonProperty("chunkSize")]
        public double ChunkSize { get; set; }

        /// <summary>
        /// Returns the original event arguments.
        /// </summary>
        [JsonProperty("e")]
        public object E { get; set; }

        /// <summary>
        /// Returns the original event arguments.
        /// </summary>
        [JsonProperty("event")]
        public object Event { get; set; }

        /// <summary>
        /// Returns the details about upload file.
        /// </summary>
        [JsonProperty("file")]
        public FileInfo File { get; set; }

        /// <summary>
        ///  Defines the files for retry the upload files.
        /// </summary>
        [JsonProperty("retryFiles")]
        public FileInfo[] RetryFiles { get; set; } = null;

        /// <summary>
        /// Returns the upload event operation.
        /// </summary>
        [JsonProperty("operation")]
        public string Operation { get; set; }

        /// <summary>
        /// Returns the upload event operation.
        /// </summary>
        [JsonProperty("response")]
        public ResponseEventArgs Response { get; set; }

        /// <summary>
        /// Returns the upload status.
        /// </summary>
        [JsonProperty("statusText")]
        public string StatusText { get; set; }

        /// <summary>
        /// Returns the total chunk size.
        /// </summary>
        [JsonProperty("totalChunk")]
        public double TotalChunk { get; set; }
    }
    /// <summary>
    /// Defines the argument for the FileInfo.
    /// </summary>
    public class FileInfo
    {
        /// <summary>
        /// Returns where the file selected from, to upload.
        /// </summary>
        [JsonProperty("fileSource")]
        public string FileSource { get; set; }

        /// <summary>
        /// Returns the unique upload file name ID.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Returns the input element mapped with file list item.
        /// </summary>
        [JsonProperty("input")]
        public DOM Input { get; set; }

        /// <summary>
        /// Returns the respective file list item.
        /// </summary>
        [JsonProperty("list")]
        public DOM List { get; set; }

        /// <summary>
        /// Returns the upload file name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Returns the details about upload file.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("rawFile")]
        public object RawFile { get; set; } = null;

        /// <summary>
        /// Returns the size of file in bytes.
        /// </summary>
        [JsonProperty("size")]
        public double Size { get; set; }

        /// <summary>
        /// Returns the status of the file.
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        /// Returns the current state of the file such as Failed, Canceled, Selected, Uploaded, or Uploading.
        /// </summary>
        [JsonProperty("statusCode")]
        public string StatusCode { get; set; }

        /// <summary>
        /// Returns the MIME type of file as a string. Returns empty string if the file’s type is not determined.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
        
        /// <summary>
        /// Returns the mime content type of file as a string.
        /// </summary>
        [JsonProperty("mimeContentType")]
        public string MimeContentType { get; set; }

        /// <summary>
        /// Returns the last modified date of the uploading file.
        /// </summary>
        [JsonProperty("lastModifiedDate")]
        public DateTime LastModifiedDate { get; set; }

        /// <summary>
        /// Returns the list of validation errors (if any).
        /// </summary>
        [JsonProperty("validationMessages")]
        public ValidationMessages ValidationMessages { get; set; }
    }
    /// <summary>
    /// Defines the argument for the FileListRender event.
    /// </summary>
    public class FileListRenderingEventArgs
    {
        /// <summary>
        /// Return the current file item element.
        /// </summary>
        [JsonProperty("element")]
        public DOM Element { get; set; }

        /// <summary>
        /// Return the current rendering file item data as File object.
        /// </summary>
        [JsonProperty("fileInfo")]
        public FileInfo FileInfo { get; set; }

        /// <summary>
        /// Return the index of the file item in the file list.
        /// </summary>
        [JsonProperty("index")]
        public double Index { get; set; }

        /// <summary>
        /// Return whether the file is preloaded.
        /// </summary>
        [JsonProperty("isPreload")]
        public bool IsPreload { get; set; }
    }
    /// <summary>
    /// Defines the argument for the PauseResume event.
    /// </summary>
    public class PauseResumeEventArgs
    {
        /// <summary>
        /// Returns the total number of chunks.
        /// </summary>
        [JsonProperty("chunkCount")]
        public double ChunkCount { get; set; }

        /// <summary>
        /// Returns the index of chunk that is Paused or Resumed.
        /// </summary>
        [JsonProperty("chunkIndex")]
        public double ChunkIndex { get; set; }

        /// <summary>
        /// Returns the chunk size value in bytes.
        /// </summary>
        [JsonProperty("chunkSize")]
        public double ChunkSize { get; set; }

        /// <summary>
        /// Returns the original event arguments.
        /// </summary>
        [JsonProperty("event")]
        public EventArgs Event { get; set; }

        /// <summary>
        /// Returns the file data that is Paused or Resumed.
        /// </summary>
        [JsonProperty("file")]
        public FileInfo File { get; set; }
    }
    /// <summary>
    /// Defines the argument for the Progress event.
    /// </summary>
    public class ProgressEventArgs
    {
        /// <summary>
        /// Returns the original event arguments.
        /// </summary>
        [JsonProperty("e")]
        public object E { get; set; }

        /// <summary>
        /// Returns the file progress is computable or not.
        /// </summary>
        [JsonProperty("lengthComputable")]
        public bool LengthComputable { get; set; }

        /// <summary>
        /// Returns the progressed the uploading file size.
        /// </summary>
        [JsonProperty("loaded")]
        public decimal Loaded { get; set; }

        /// <summary>
        /// Returns the total size of the uploading file.
        /// </summary>
        [JsonProperty("total")]
        public decimal Total { get; set; }

        /// <summary>
        /// Returns the details about upload file.
        /// </summary>
        [JsonProperty("file")]
        public FileInfo File { get; set; }

        /// <summary>
        /// Return the file stream of loaded file content.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("stream")]
        public MemoryStream Stream { get; set; }

        /// <summary>
        /// Returns the upload event operation.
        /// </summary>
        [JsonProperty("operation")]
        public string Operation { get; set; }
    }
    /// <summary>
    /// Defines the argument for the Removing event.
    /// </summary>
    public class RemovingEventArgs
    {
        /// <summary>
        /// Defines whether the current action can be prevented.
        /// </summary>
        [JsonProperty("cancel")]
        public bool Cancel { get; set; }

        /// <summary>
        /// Returns the XMLHttpRequest instance that is associated with remove action.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("currentRequest")]
        public object CurrentRequest { get; set; } = null;

        /// <summary>
        /// Defines the additional data with key and value pair format that will be submitted to the remove action.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("customFormData")]
        public object CustomFormData { get; set; } = null;

        /// <summary>
        /// Returns the original event arguments.
        /// </summary>
        [JsonProperty("event")]
        public object Event { get; set; }

        /// <summary>
        /// Returns the list of files’ details that will be removed.
        /// </summary>
        [JsonProperty("filesData")]
        public List<FileInfo> FilesData { get; set; }

        /// <summary>
        /// Defines whether the selected raw file send to server remove action.
        /// Set true to send raw file.
        /// Set false to send file name only.
        /// </summary>
        [JsonProperty("postRawFile")]
        public bool PostRawFile { get; set; }
    }
    /// <summary>
    /// Defines the argument for the Rendering event.
    /// </summary>
    public class RenderingEventArgs
    {
        /// <summary>
        /// Return the current file item element.
        /// </summary>
        [JsonProperty("element")]
        public DOM Element { get; set; }

        /// <summary>
        /// Return the current rendering file item data as File object.
        /// </summary>
        [JsonProperty("fileInfo")]
        public FileInfo FileInfo { get; set; }

        /// <summary>
        /// Return the index of the file item in the file list.
        /// </summary>
        [JsonProperty("index")]
        public double Index { get; set; }

        /// <summary>
        /// Return whether the file is preloaded.
        /// </summary>
        [JsonProperty("isPreload")]
        public bool IsPreload { get; set; }
    }
    /// <summary>
    /// Defines the argument for the Response event.
    /// </summary>
    public class ResponseEventArgs
    {
        /// <summary>
        /// Returns the current response header.
        /// </summary>
        [JsonProperty("headers")]
        public string Headers { get; set; }
        /// <summary>
        /// Returns the current response readyState.
        /// </summary>
        [JsonProperty("readyState")]
        public object ReadyState { get; set; }
        /// <summary>
        /// Returns the current response statusCode.
        /// </summary>
        [JsonProperty("statusCode")]
        public object StatusCode { get; set; }
        /// <summary>
        /// Returns the current response statusText.
        /// </summary>
        [JsonProperty("statusText")]
        public string StatusText { get; set; }
        /// <summary>
        /// Returns the current response withCredentials.
        /// </summary>
        [JsonProperty("withCredentials")]
        public bool WithCredentials { get; set; }
    }
    /// <summary>
    /// Defines the argument for the Selected event.
    /// </summary>
    public class SelectedEventArgs
    {
        /// <summary>
        /// Defines whether the current action can be prevented.
        /// </summary>
        [JsonProperty("cancel")]
        public bool Cancel { get; set; }

        /// <summary>
        /// Set the current request header to the XMLHttpRequest instance.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("currentRequest")]
        public object CurrentRequest { get; set; } = null;

        /// <summary>
        /// Defines the additional data in key and value pair format that will be submitted to the upload action.
        /// </summary>
        [JsonProperty("customFormData")]
        public object CustomFormData { get; set; }

        /// <summary>
        /// Returns the original event arguments.
        /// </summary>
        [JsonProperty("event")]
        public object Event { get; set; }

        /// <summary>
        /// Returns the list of selected files.
        /// </summary>
        [JsonProperty("filesData")]
        public List<FileInfo> FilesData { get; set; }

        /// <summary>
        /// Specifies whether the file selection has been canceled.
        /// </summary>
        [JsonProperty("isCanceled")]
        public bool IsCanceled { get; set; }

        /// <summary>
        /// Determines whether the file list generates based on the modified data.
        /// </summary>
        [JsonProperty("isModified")]
        public bool IsModified { get; set; }

        /// <summary>
        /// Specifies the modified files data to generate the file items. The argument depends on `isModified` argument.
        /// </summary>
        [JsonProperty("modifiedFilesData")]
        public List<FileInfo> ModifiedFilesData { get; set; }

        /// <summary>
        /// Specifies the step value to the progress bar.
        /// </summary>
        [JsonProperty("progressInterval")]
        public string ProgressInterval { get; set; }

        /// <summary>
        /// Returns the original event argument type.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
    }
    /// <summary>
    /// Defines the argument for the Success event.
    /// </summary>
    public class SuccessEventArgs
    {
        /// <summary>
        /// Returns the upload chunk index.
        /// </summary>
        [JsonProperty("chunkIndex")]
        public double ChunkIndex { get; set; }

        /// <summary>
        /// Returns the upload chunk size.
        /// </summary>
        [JsonProperty("chunkSize")]
        public double ChunkSize { get; set; }

        /// <summary>
        /// Returns the original event arguments.
        /// </summary>
        [JsonProperty("e")]
        public object E { get; set; }

        /// <summary>
        /// Returns the original event arguments.
        /// </summary>
        [JsonProperty("event")]
        public object Event { get; set; }

        /// <summary>
        /// Returns the details about upload file.
        /// </summary>
        [JsonProperty("file")]
        public FileInfo File { get; set; }

        /// <summary>
        /// Returns the upload event operation.
        /// </summary>
        [JsonProperty("operation")]
        public string Operation { get; set; }

        /// <summary>
        /// Returns the upload event operation.
        /// </summary>
        [JsonProperty("response")]
        public ResponseEventArgs Response { get; set; }

        /// <summary>
        /// Returns the upload status.
        /// </summary>
        [JsonProperty("statusText")]
        public string StatusText { get; set; }

        /// <summary>
        /// Returns the total chunk size.
        /// </summary>
        [JsonProperty("totalChunk")]
        public double TotalChunk { get; set; }
    }
    /// <summary>
    /// Defines the argument for the UploadFiles.
    /// </summary>
    public class UploadFiles
    {
        /// <summary>
        /// Return the selected file stream.
        /// </summary>
        public MemoryStream Stream { get; set; }

        /// <summary>
        /// Return the selected file details.
        /// </summary>
        public FileInfo FileInfo { get; set; }
    }
    /// <summary>
    /// Defines the argument for the UploadChangeEventArgs event.
    /// </summary>
    public class UploadChangeEventArgs
    {
        /// <summary>
        /// Returns the list of files that will be cleared from the FileList.
        /// </summary>
        [JsonProperty("files")]
        public List<UploadFiles> Files { get; set; } = null;
    }
    /// <summary>
    /// Defines the argument for the UploadingEventArgs event.
    /// </summary>
    public class UploadingEventArgs
    {
        /// <summary>
        /// Defines whether the current action can be prevented.
        /// </summary>
        [JsonProperty("cancel")]
        public bool Cancel { get; set; }

        /// <summary>
        /// Returns the chunk size in bytes if the chunk upload is enabled.
        /// </summary>
        [JsonProperty("chunkSize")]
        public double ChunkSize { get; set; }

        /// <summary>
        /// Returns the index of current chunk if the chunk upload is enabled.
        /// </summary>
        [JsonProperty("currentChunkIndex")]
        public double CurrentChunkIndex { get; set; }

        /// <summary>
        /// Returns the list of files that will be uploaded.
        /// </summary>
        [JsonProperty("fileData")]
        public FileInfo FileData { get; set; }
    }
    /// <summary>
    /// Defines the argument for the ValidationMessages.
    /// </summary>
    public class ValidationMessages
    {
        /// <summary>
        /// Returns the maximum file size validation message, if selected file size is less than specified maxFileSize property.
        /// </summary>
        [JsonProperty("maxSize")]
        public string MaxSize { get; set; }

        /// <summary>
        /// Returns the minimum file size validation message, if selected file size is less than specified minFileSize property.
        /// </summary>
        [JsonProperty("minSize")]
        public string MinSize { get; set; }
    }

    /// <summary>
    /// Interface for a class AsyncSettings.
    /// </summary>
    public class AsyncSettingsModel
    {
        /// <summary>
        /// Specifies the chunk size to split the large file into chunks, and upload it to the server in a sequential order.
        /// <para>If the ChunkSize property has value, the Uploader enables the chunk upload by default.</para>
        /// <para>It must be specified in bytes value.</para>
        /// </summary>
        [DefaultValue(0)]
        [JsonProperty("chunkSize")]
        public double ChunkSize { get; set; }

        /// <summary>
        /// Specifies the URL of remove action that receives the file information and handle the remove operation in server.
        /// <para>The remove action type must be POST request and define "RemoveFileNames" attribute to get file information that will be removed.</para>
        /// <para>This property is optional.</para>
        /// </summary>
        [DefaultValue("")]
        [JsonProperty("removeUrl")]
        public string RemoveUrl { get; set; } = string.Empty;

        /// <summary>
        /// Specifies the delay time in milliseconds that the automatic retry happens after the delay.
        /// </summary>
        [DefaultValue(500)]
        [JsonProperty("retryAfterDelay")]
        public double RetryAfterDelay { get; set; } = 500;

        /// <summary>
        /// Specifies the number of retries that the Uploader can perform on the file failed to upload.
        /// By default, the Uploader set 3 as maximum retries. This property must be specified to prevent infinity looping.
        /// </summary>
        [DefaultValue(3)]
        [JsonProperty("retryCount")]
        public double RetryCount { get; set; } = 3;

        /// <summary>
        /// Specifies the URL of save action that will receive the upload files and save in the server.
        /// <para>The save action type must be POST request and define the argument as same input name used to render the component.</para>
        /// <para>The upload operations could not perform without this property.</para>
        /// </summary>
        [DefaultValue("")]
        [JsonProperty("saveUrl")]
        public string SaveUrl { get; set; } = string.Empty;
    }

    /// <summary>
    /// Interface for a class ButtonsProps.
    /// </summary>
    public class ButtonsPropsModel
    {
        /// <summary>
        /// Specifies the text or html content to browse button.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("browse")]
        public object Browse { get; set; } = null;

        /// <summary>
        /// Specifies the text or html content to clear button.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("clear")]
        public object Clear { get; set; } = null;

        /// <summary>
        /// Specifies the text or html content to upload button.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("upload")]
        public object Upload { get; set; } = null;
    }

    /// <summary>
    /// Interface for a class FilesProp.
    /// </summary>
    public class FilesPropModel
    {
        /// <summary>
        /// Specifies the name of the file.
        /// </summary>
        [DefaultValue("")]
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Specifies the size of the file.
        /// </summary>
        [DefaultValue(default(double))]
        [JsonProperty("size")]
        public double Size { get; set; } = default;

        /// <summary>
        /// Specifies the type of the file.
        /// </summary>
        [DefaultValue("")]
        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;
    }

    /// <summary>
    /// Interface for a class Uploader.
    /// </summary>
    public class UploaderModel
    {
        /// <summary>
        /// Triggers after all the selected files has processed to upload successfully or failed to server.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("actionComplete")]
        public EventCallback<object> ActionComplete { get; set; }

        /// <summary>
        /// Triggers on remove the uploaded file. The event used to get confirm before remove the file from server.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("beforeRemove")]
        public EventCallback<object> BeforeRemove { get; set; }

        /// <summary>
        /// Triggers when the upload process before. This event is used to add additional parameter with upload request.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("beforeUpload")]
        public EventCallback<object> BeforeUpload { get; set; }

        /// <summary>
        /// Fires if cancel the chunk file uploading.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("canceling")]
        public EventCallback<object> Canceling { get; set; }

        /// <summary>
        /// Triggers when changes occur in uploaded file list by selecting or dropping files.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("change")]
        public UploadChangeEventArgs Change { get; set; } = null;

        /// <summary>
        /// Fires if the chunk file failed to upload.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("chunkFailure")]
        public FailureEventArgs ChunkFailure { get; set; } = null;

        /// <summary>
        /// Fires when the chunk file uploaded successfully.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("chunkSuccess")]
        public SuccessEventArgs ChunkSuccess { get; set; } = null;

        /// <summary>
        /// Fires when every chunk upload process gets started. This event is used to add additional parameter with upload request.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("chunkUploading")]
        public EventCallback<object> ChunkUploading { get; set; }

        /// <summary>
        /// Triggers before clearing the items in file list when clicking "clear".
        /// </summary>
        [JsonIgnore]
        [JsonProperty("clearing")]
        public EventCallback<object> Clearing { get; set; }

        /// <summary>
        /// Triggers when the Uploader is created.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("created")]
        public EventCallback<object> Created { get; set; }

        /// <summary>
        /// Triggers when the AJAX request fails on uploading or removing files.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("failure")]
        public FailureEventArgs Failure { get; set; } = null;

        /// <summary>
        /// Triggers before rendering each file item from the file list in a page.
        /// It helps to customize specific file item structure.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("fileListRendering")]
        public EventCallback<object> FileListRendering { get; set; }

        /// <summary>
        /// Fires if pause the chunk file uploading.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("pausing")]
        public EventCallback<object> Pausing { get; set; }

        /// <summary>
        /// Triggers when uploading a file to the server using the AJAX request.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("progress")]
        public ProgressEventArgs Progress { get; set; } = null;

        /// <summary>
        /// Triggers on removing the uploaded file. The event used to get confirm before removing the file from server.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("removing")]
        public EventCallback<object> Removing { get; set; }

        /// <summary>
        /// DEPRECATED-Triggers before rendering each file item from the file list in a page.
        /// It helps to customize specific file item structure.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("rendering")]
        public EventCallback<object> Rendering { get; set; }

        /// <summary>
        /// Fires if resume the paused chunk file upload.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("resuming")]
        public EventCallback<object> Resuming { get; set; }

        /// <summary>
        /// Triggers after selecting or dropping the files by adding the files in upload queue.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("selected")]
        public EventCallback<object> Selected { get; set; }

        /// <summary>
        /// Triggers when the AJAX request gets success on uploading files or removing files.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("success")]
        public SuccessEventArgs Success { get; set; } = null;

        /// <summary>
        /// Triggers when the upload process gets started. This event is used to add additional parameter with upload request.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("uploading")]
        public EventCallback<object> Uploading { get; set; }

        /// <summary>
        /// Specifies the extensions of the file types allowed in the Uploader component and pass the extensions
        /// with comma separators.
        /// <para> For example,if you want to upload specific image files, pass `AllowedExtensions` as ".jpg,.png".</para>
        /// </summary>
        [DefaultValue("")]
        [JsonProperty("allowedExtensions")]
        public string AllowedExtensions { get; set; } = string.Empty;

        /// <summary>
        /// Configures the save and remove URL to perform the upload operations in the server asynchronously.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("asyncSettings")]
        public UploaderAsyncSettings AsyncSettings { get; set; } = null;

        /// <summary>
        /// By default, the Uploader component initiates automatic upload when the files are added in upload queue.
        /// <para>If you want to manipulate the files before uploading to server, disable the AutoUpload property.</para>
        /// <para>The buttons "upload" and "clear" will be hided from file list when AutoUpload property is true.</para>
        /// </summary>
        [DefaultValue(true)]
        [JsonProperty("autoUpload")]
        public bool AutoUpload { get; set; } = true;

        /// <summary>
        /// You can customize the default text of "browse, clear, and upload" buttons with plain text or HTML elements.
        /// The buttons' text can be customized from localization also.
        /// <para>If you configured both `Locale` and `Buttons` property,the Uploader component considers the `Buttons` property value.</para>
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("buttons")]
        public UploaderButtons Buttons { get; set; } = null;

        /// <summary>
        /// Specifies the CSS class name that can be appended with root element of the Uploader.
        /// One or more custom CSS classes can be added to a Uploader.
        /// </summary>
        [DefaultValue("")]
        [JsonProperty("cssClass")]
        public string CssClass { get; set; } = string.Empty;

        /// <summary>
        /// Specifies a boolean value that indicates whether the folder of files can be browsed in the Uploader component.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("directoryUpload")]
        public bool DirectoryUpload { get; set; } = false;

        /// <summary>
        /// Specifies the drop target to handle the drag-and-drop upload.
        /// By default, the Uploader creates wrapper around file input that will act as drop target.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("dropArea")]
        public object DropArea { get; set; } = null;

        /// <summary>
        /// Specifies the drag operation effect to the Uploader component.
        /// <para> Possible values are.</para>
        /// <list type="bullet">
        /// <item>
        /// <term>Copy</term>
        /// </item>
        /// <item>
        /// <term>Move</term>
        /// </item>
        /// <item>
        /// <term>Link</term>
        /// </item>
        /// <item>
        /// <term>None</term>
        /// </item>
        /// </list>
        /// </summary>
        [DefaultValue(DropEffect.Default)]
        [JsonProperty("dropEffect")]
        [JsonConverter(typeof(StringEnumConverter))]
        public DropEffect DropEffect { get; set; } = DropEffect.Default;

        /// <summary>
        /// Enable or disable persisting Uploader state between page reloads. If enabled, the `Files` state will be persisted.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("enablePersistence")]
        public bool EnablePersistence { get; set; } = false;

        /// <summary>
        /// Enable or disable rendering Uploader in right to left direction.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("enableRtl")]
        public bool EnableRtl { get; set; } = false;

        /// <summary>
        /// Specifies a boolean value that indicates whether the Uploader allows the user to interact with it.
        /// </summary>
        [DefaultValue(true)]
        [JsonProperty("enabled")]
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Specifies the list of files that will be preloaded on rendering of Uploader component.
        /// The property used to view and remove the uploaded files from server.
        /// <para> By default, the files are configured with uploaded successfully state. The following properties are mandatory to configure the preload files:</para>
        /// <list type="bullet">
        /// <item>
        /// <term>Name</term>
        /// </item>
        /// <item>
        /// <term>Size</term>
        /// </item>
        /// <item>
        /// <term>Type</term>
        /// </item>
        /// </list>
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("files")]
        public List<UploaderUploadedFiles> Files { get; set; } = null;

        /// <summary>
        /// <para>You can add the additional html attributes such as styles, class, and more to the root element.</para>
        /// <para>If you configured both property and equivalent html attributes, then the Uploader considers the property value.</para>
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("htmlAttributes")]
        public object HtmlAttributes { get; set; } = null;

        /// <summary>
        /// Specifies the global culture and localization of the Uploader component.
        /// </summary>
        [DefaultValue("")]
        [JsonProperty("locale")]
        public string Locale { get; set; } = string.Empty;

        /// <summary>
        /// Specifies the maximum allowed file size to be uploaded in bytes.
        /// The property used to make sure that you cannot upload too large files.
        /// </summary>
        [DefaultValue(30000000)]
        [JsonProperty("maxFileSize")]
        public double MaxFileSize { get; set; } = 30000000;

        /// <summary>
        /// Specifies the minimum file size to be uploaded in bytes.
        /// The property used to make sure that you cannot upload empty files and small files.
        /// </summary>
        [DefaultValue(0)]
        [JsonProperty("minFileSize")]
        public double MinFileSize { get; set; }

        /// <summary>
        /// Specifies a boolean value that indicates whether the multiple files can be browsed or
        /// dropped simultaneously in the Uploader component.
        /// </summary>
        [DefaultValue(true)]
        [JsonProperty("multiple")]
        public bool Multiple { get; set; } = true;

        /// <summary>
        /// By default, the file Uploader component is processing the multiple files simultaneously.
        /// <para>If SequentialUpload property is enabled, the file upload component performs the upload one after the other.</para>
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("sequentialUpload")]
        public bool SequentialUpload { get; set; } = false;

        /// <summary>
        /// Specifies a boolean value that indicates whether the default file list can be rendered.
        /// The property used to prevent default file list and design own template for file list.
        /// </summary>
        [DefaultValue(true)]
        [JsonProperty("showFileList")]
        public bool ShowFileList { get; set; } = true;

        /// <summary>
        /// Specifies the HTML string that used to customize the content of each file in the list.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("template")]
        public string Template { get; set; } = null;
    }
}