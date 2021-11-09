using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Syncfusion.Blazor.Inputs.Internal;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Components;
using System.Globalization;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Inputs
{
    /// <summary>
    /// The uploader component is useful to upload images, documents, and other files to server.
    /// The component is the extended version of HTML5 that is uploaded with multiple file selection, auto upload, drag and drop, progress bar, preload files, and validation.
    /// </summary>
    public partial class SfUploader : SfBaseComponent
    {
        private List<string> containerAttributes = new List<string>() { "title", "style", "class" };

        private Dictionary<string, object> containerAttr = new Dictionary<string, object>();

        private Dictionary<string, object> browseBtnAttr = new Dictionary<string, object>();

        private Dictionary<string, object> actionBtnAttr = new Dictionary<string, object>();

        private Dictionary<string, object> inputAttr = new Dictionary<string, object>();

        private UploadFileList UploadFileList => new UploadFileList(FileElement, new UploaderStreamReader(this));

        internal UploaderEvents UploaderEvents { get; set; }

        internal async Task CallStateHasChangedAsync() => await InvokeAsync(StateHasChanged);

        private string ContainerClass { get; set; }

        private string RemoveIconDisable { get; set; } = string.Empty;

        private bool IsClearAll { get; set; }

        private bool IsShowRemoveIcon { get; set; }

        private string BrowseBtnContent { get; set; }

        private string InputContainer { get; set; }

        private string DropAreaContainer { get; set; }

        private string FileDropAreaContent { get; set; }

        private bool IsShowFileList { get; set; }

        private bool IsShowProgresBar { get; set; }

        [Inject]
        private ISyncfusionStringLocalizer Localizer { get; set; }

        private string BtnTabIndex { get; set; } = "0";

        private List<UploadFileDetails> FileData { get; set; }

        private string FileListClass { get; set; }

        private string FileListStatus { get; set; }

        private string FileListStatusName { get; set; }

        private string UploadStatus { get; set; }

        private ElementReference ActionButtonRef { get; set; }

        private ElementReference UlElementRef { get; set; }

        internal ElementReference FileElement { get; set; }

        internal int BufferSize { get; set; } = 20480;

        private int FileIndex { get; set; }

        private long ChunkIndex { get; set; }

        private long TotalChunk { get; set; }

        private FileInfo FileInfo { get; set; }

        private long ProgressValue { get; set; }

        internal List<MemoryStream> StreamReader { get; set; } = new List<MemoryStream>() { };

        internal MemoryStream LocalStream { get; set; }

        private bool IsForm { get; set; }

        private bool IsDevice { get; set; }

        /// <summary>
        /// Triggers while initial rendering of the component.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            PropertyInitialized();
            if (!UploadMultiple)
            {
                AllowMultiple = UploadMultiple;
            }

            DependencyScripts();
            PreRender();
            Render();
        }

        /// <summary>
        /// Triggers after the component was rendered.
        /// </summary>
        /// <param name="firstRender">true if the component rendered for the firts time.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                await SfBaseUtils.InvokeEvent<object>(UploaderEvents?.Created, null);
            }
        }

        internal override async Task OnAfterScriptRendered()
        {
            var options = GetProperty();
            await InvokeMethod("sfBlazor.Uploader.initialize", new object[] { FileElement, DotnetObjectReference, options });
            IsDevice = SyncfusionService.IsDeviceMode;
            await RenderPreloadFiles();
        }

        private void DependencyScripts()
        {
            ScriptModules = SfScriptModules.SfUploader;
            DependentScripts.Add(Syncfusion.Blazor.Internal.ScriptModules.Spinner);
        }

        private void PreRender()
        {
            inputAttr = SfBaseUtils.UpdateDictionary("class", "e-control" + SPACE + ROOT + SPACE + "e-lib", inputAttr);
            inputAttr = SfBaseUtils.UpdateDictionary("name", ID.ToString(), inputAttr);
            ContainerClass = CONTROL_CONTAINER + SPACE + "e-lib e-keyboard";
            SetEnabled();
            SetCssClass();
            UpdateHTMLAttributes();
            UpdateInputAttributes();
            UpdateDirectoryAttr();
        }

        private void UpdateInputAttributes()
        {
            if (InputAttributes != null && InputAttributes.Count > 0)
            {
                foreach (var attr in InputAttributes)
                {
                    SfBaseUtils.UpdateDictionary(attr.Key, attr.Value, inputAttr);
                }
            }
        }

        internal Dictionary<string, object> GetProperty()
        {
            var template = (Template != null) ? "Blazor Template" : null;
            return new Dictionary<string, object>
            {
                { ALLOW_EXTENSIONS, AllowedExtensions },
                { ASYNC_SETTING, UploadAsyncSettings },
                { AUTO_UPLOAD, AutoUpload },
                { BUTTON, UploadButtons },
                { CSSCLASS, CssClass },
                { DROPAREA, DropArea },
                { DIRECTORY_UPLOAD, DirectoryUpload },
                { DROP_EFFECT, DropEffect.ToString() },
                { ENABLED, Enabled },
                { PERSISTENCE, EnablePersistence },
                { ENABLE_RTL, EnableRtl || SyncfusionService.options.EnableRtl },
                { UPLOAD_FILES, UploadedFiles },
                { nameof(UploaderLocale), CultureInfo.CurrentCulture.Name },
                { MAX_FILE_SIZE, MaxFileSize },
                { MIN_FILE_SIZE, MinFileSize },
                { SHOW_FILE_LIST, ShowFileList },
                { UPLOAD_MULTIPLE, AllowMultiple },
                { SEQUENTIAL_UPLOAD, SequentialUpload },
                { UPLOAD_TEMPLATE,  template },
                { ACTION_COMPLETE_ENABLED, UploaderEvents?.OnActionComplete != null && UploaderEvents.OnActionComplete.HasDelegate },
                { BEFORE_REMOVE_ENABLED,  UploaderEvents?.BeforeRemove != null && UploaderEvents.BeforeRemove.HasDelegate },
                { BEFORE_UPLOAD_ENABLED,  UploaderEvents?.BeforeUpload != null && UploaderEvents.BeforeUpload.HasDelegate },
                { CANCEL_ENABLED,  UploaderEvents?.OnCancel != null && UploaderEvents.OnCancel.HasDelegate },
                { CHANGE_ENABLED,  UploaderEvents?.ValueChange != null && UploaderEvents.ValueChange.HasDelegate },
                { CHUNK_FAILURE_ENABLED,  UploaderEvents?.OnChunkFailure != null && UploaderEvents.OnChunkFailure.HasDelegate },
                { CHUNK_UPLOADING_ENABLED,  UploaderEvents?.OnChunkUploadStart != null && UploaderEvents.OnChunkUploadStart.HasDelegate },
                { UPLOADING_ENABLED,  UploaderEvents?.OnUploadStart != null && UploaderEvents.OnUploadStart.HasDelegate },
                { CLEAR_ENABLED,  UploaderEvents?.OnClear != null && UploaderEvents.OnClear.HasDelegate },
                { FAILURE_ENABLED,  UploaderEvents?.OnFailure != null && UploaderEvents.OnFailure.HasDelegate },
                { FILE_RENDER_ENABLED,  UploaderEvents?.OnFileListRender != null && UploaderEvents.OnFileListRender.HasDelegate },
                { PAUSED_ENABLED,  UploaderEvents?.Paused != null && UploaderEvents.Paused.HasDelegate },
                { PROGRESSING_ENABLED,  UploaderEvents?.Progressing != null && UploaderEvents.Progressing.HasDelegate },
                { REMOVING_ENABLED,  UploaderEvents?.OnRemove != null && UploaderEvents.OnRemove.HasDelegate },
                { RESUME_ENABLED,  UploaderEvents?.OnResume != null && UploaderEvents.OnResume.HasDelegate },
                { SELECTED_ENABLED,  UploaderEvents?.FileSelected != null && UploaderEvents.FileSelected.HasDelegate },
                { SUCCESS_ENABLED,  UploaderEvents?.Success != null && UploaderEvents.Success.HasDelegate },
                { CHUNK_SUCCESS_ENABLED,  UploaderEvents?.OnChunkSuccess != null && UploaderEvents.OnChunkSuccess.HasDelegate },
                { LOCALE_TEXT,  GetLocaleText() }
            };
        }

        private Dictionary<string, object> GetLocaleText()
        {
            var localeText = new Dictionary<string, object>
            {
                { "browse", Localizer.GetText(BROWSE_KEY) ?? "Browse..." },
                { "abort", Localizer.GetText(ABORT_KEY) ?? "Abort" },
                { "cancel", Localizer.GetText(CANCEL_KEY) ?? "Cancel" },
                { "clear", Localizer.GetText(CLEAR_KEY) ?? "Clear" },
                { "delete", Localizer.GetText(DELETE_KEY) ?? "Delete file" },
                { "dropFilesHint", Localizer.GetText(DROP_FILE_KEY) ?? "Or drop files here" },
                { "fileUploadCancel", Localizer.GetText(FILE_UPLOAD_CANCEL) ?? "File upload canceled" },
                { "inProgress", Localizer.GetText(INPROGRESS_KEY) ?? "Uploading" },
                { "invalidFileType", Localizer.GetText(INVALID_FILE_KEY) ?? "File type is not allowed" },
                { "invalidMaxFileSize", Localizer.GetText(INVALID_MAX_FILE_KEY) ?? "File size is too large" },
                { "invalidMinFileSize", Localizer.GetText(INVALID_MIN_FILE_KEY) ?? "File size is too small" },
                { "pause", Localizer.GetText(PAUSE_KEY) ?? "Pause" },
                { "pauseUpload", Localizer.GetText(PAUSE_UPLOAD_KEY) ?? "File upload paused" },
                { "readyToUploadMessage", Localizer.GetText(READY_UPLOAD_KEY) ?? "Ready to upload" },
                { "remove", Localizer.GetText(REMOVE_KEY) ?? "Remove" },
                { "removedFailedMessage", Localizer.GetText(REMOVED_FAILED_KEY) ?? "Unable to remove file" },
                { "removedSuccessMessage", Localizer.GetText(REMOVED_SUCCESS_KEY) ?? "File removed successfully" },
                { "resume", Localizer.GetText(RESUME_KEY) ?? "Resume" },
                { "retry", Localizer.GetText(RETRY_KEY) ?? "Retry" },
                { "upload", Localizer.GetText(UPLOAD_KEY) ?? "Upload" },
                { "uploadFailedMessage", Localizer.GetText(UPLOAD_FAILED_KEY) ?? "File failed to upload" },
                { "uploadSuccessMessage", Localizer.GetText(UPLOAD_SUCCESS_KEY) ?? "File uploaded successfully" }
            };
            return localeText;
        }

        private void SetCssClass()
        {
            if (!string.IsNullOrEmpty(CssClass))
            {
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, CssClass);
            }
        }

        private void SetEnabled()
        {
            if (!Enabled)
            {
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, DISABLED);
                inputAttr = SfBaseUtils.UpdateDictionary("disabled", "disabled", inputAttr);
                inputAttr = SfBaseUtils.UpdateDictionary("aria-disabled", TRUE, inputAttr);
            }
            else
            {
                ContainerClass = SfBaseUtils.RemoveClass(ContainerClass, DISABLED);
                inputAttr.Remove("disabled");
                inputAttr.Remove("aria-disabled");
            }
        }

        private void UpdateDirectoryAttr()
        {
            if (DirectoryUpload)
            {
                inputAttr = SfBaseUtils.UpdateDictionary("directory", TRUE, inputAttr);
                inputAttr = SfBaseUtils.UpdateDictionary("webkitdirectory", TRUE, inputAttr);
            }
            else
            {
                inputAttr.Remove("directory");
                inputAttr.Remove("webkitdirectory");
            }
        }

        private void Render()
        {
            UpdateBrowsBtn();
            InitializeUpload();
            SetMultipleSelection();
            SetExtensions();
            SetRTL();
        }

        private void UpdateBrowsBtn()
        {
            browseBtnAttr = SfBaseUtils.UpdateDictionary("tabindex", TabIndex, browseBtnAttr);
            var browseLocaleVal = Localizer.GetText(BROWSE_KEY) ?? "Browse...";
            BrowseBtnContent = (UploadButtons.Browse == "Browse...") ? browseLocaleVal : UploadButtons.Browse;
            browseBtnAttr = SfBaseUtils.UpdateDictionary("title", BrowseBtnContent, browseBtnAttr);
            browseBtnAttr = SfBaseUtils.UpdateDictionary("aria-label", "Uploader", browseBtnAttr);
        }

        private void InitializeUpload()
        {
            inputAttr = SfBaseUtils.UpdateDictionary("tabindex", "-1", inputAttr);
            InputContainer = INPUT_CONTAINER;
            DropAreaContainer = DROP_CONTAINER;
            FileDropAreaContent = Localizer.GetText(DROP_FILE_KEY) ?? "Or drop files here";
        }

        private void SetMultipleSelection()
        {
            if (AllowMultiple)
            {
                inputAttr = SfBaseUtils.UpdateDictionary("multiple", "multiple", inputAttr);
            }
            else
            {
                inputAttr.Remove("multiple");
            }
        }

        private void SetExtensions()
        {
            if (!string.IsNullOrEmpty(AllowedExtensions))
            {
                inputAttr = SfBaseUtils.UpdateDictionary("accept", AllowedExtensions, inputAttr);
            }
            else
            {
                inputAttr.Remove("accept");
            }
        }

        private void SetRTL()
        {
            ContainerClass = (EnableRtl || SyncfusionService.options.EnableRtl) ? SfBaseUtils.AddClass(ContainerClass, RTL) : SfBaseUtils.RemoveClass(ContainerClass, RTL);
        }

        private void UpdateHTMLAttributes()
        {
            if (HtmlAttributes != null)
            {
                foreach (var item in (Dictionary<string, object>)HtmlAttributes)
                {
                    if (containerAttributes.IndexOf(item.Key) < 0)
                    {
                        inputAttr = SfBaseUtils.UpdateDictionary(item.Key, item.Value, inputAttr);
                    }
                    else
                    {
                        if (item.Key == "class")
                        {
                            ContainerClass = SfBaseUtils.AddClass(ContainerClass, item.Value?.ToString());
                        }
                        else
                        {
                            containerAttr = SfBaseUtils.UpdateDictionary(item.Key, item.Value, containerAttr);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Triggers while dynamically updating the component properties.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            PropertyParametersSet();
            UpdateBrowsBtn();
            if (PropertyChanges.Count > 0 && IsRendered)
            {
                UpdateDirectoryAttr();
                SetRTL();
                SetExtensions();
                if (PropertyChanges.Count > 0 && PropertyChanges.ContainsKey(nameof(CssClass)))
                {
                    ContainerClass = string.IsNullOrEmpty(ContainerClass) ? ContainerClass : SfBaseUtils.RemoveClass(ContainerClass, cssClass);
                    cssClass = CssClass;
                    SetCssClass();
                }
                if (PropertyChanges.ContainsKey(nameof(UploadedFiles)))
                {
                    await RenderPreloadFiles();
                }

                SetEnabled();
                var options = GetProperty();
                await InvokeMethod("sfBlazor.Uploader.propertyChanges", new object[] { FileElement, options, PropertyChanges });
            }

            UpdateHTMLAttributes();
            UpdateInputAttributes();
        }

        internal async Task GetFilesDetails(List<FileInfo> files)
        {
            try
            {
                int filesPosition = 0;
                FileIndex = 0;
                ProgressValue = 0;
                var getFiles = files;
                IsShowRemoveIcon = false;
                StreamReader = new List<MemoryStream>() { };
                UploadChangeEventArgs eventArgs = new UploadChangeEventArgs();
                eventArgs.Files = new List<UploadFiles>();
                var allFiles = await UploadFileList.FileListData(FileElement);
                foreach (var file in allFiles)
                {
                    try
                    {
                        FileIndex = (file as UploadReadFile).Index + 1;
                        filesPosition++;
                        FileInfo = await UploadFileList.UploaderFileInterop.GetFileInfo(FileElement, FileIndex - 1);
                        StreamReader.Add(new MemoryStream());
                        eventArgs.Files.Add(new UploadFiles() { FileInfo = FileInfo, Stream = StreamReader[FileIndex - 1] });
                        var readyMsgLocaleVal = Localizer.GetText(READY_UPLOAD_KEY) ?? "Ready to upload";
                        var validFiles = FileInfo?.Status == readyMsgLocaleVal && string.IsNullOrEmpty(FileInfo?.ValidationMessages.MaxSize) && string.IsNullOrEmpty(FileInfo?.ValidationMessages.MinSize);
                        if (validFiles)
                        {
                            IsShowProgresBar = true;
                            ProgressValue = 0;
                            LocalStream = null;
                            using (var fileStream = await file.GetFileReader())
                            {
                                var bufferSize = new byte[BufferSize];
                                int totalLength;
#pragma warning disable CA1835 // Prefer the 'Memory'-based overloads for 'ReadAsync' and 'WriteAsync'
                                while ((totalLength = await fileStream.ReadAsync(bufferSize, 0, bufferSize.Length)) != 0)
#pragma warning restore CA1835 // Prefer the 'Memory'-based overloads for 'ReadAsync' and 'WriteAsync'
                                {
                                    RemoveIconDisable = DISABLED;
                                    ChunkIndex = fileStream.Position / Convert.ToInt64(BufferSize);
                                    TotalChunk = fileStream.Length / Convert.ToInt64(BufferSize);
                                    FileData[FileIndex - 1].chunksize = ChunkIndex;
                                    FileData[FileIndex - 1].totalChunksize = TotalChunk;
                                    FileData[FileIndex - 1].Status = "Uploading";
                                    UploadStatus = UPLOAD_INPROGRESS;
                                    ProgressValue = (long)((decimal)fileStream.Position * 100) / fileStream.Length;
                                    var progressEventArgs = new ProgressEventArgs() { Total = (decimal)fileStream.Length, Loaded = (decimal)fileStream.Position, LengthComputable = true, File = FileData[FileIndex - 1], Stream = LocalStream, Operation = "Progressing" };
                                    await SfBaseUtils.InvokeEvent<ProgressEventArgs>(UploaderEvents?.Progressing, progressEventArgs);
                                    LocalStream.CopyTo(StreamReader[FileIndex - 1]);
                                    if (ProgressValue != 100)
                                    {
                                        StateHasChanged();
                                    }
                                }
                            }

                            if (ProgressValue == 100)
                            {
                                IsShowProgresBar = false;
                                UploadStatus = UPLOAD_SUCCESS;
                                RemoveIconDisable = string.Empty;
                                FileInfo.Status = Localizer.GetText(UPLOAD_SUCCESS_KEY) ?? "File uploaded successfully";
                                FileData[FileIndex - 1].Status = FileInfo.Status;
                                FileListStatusName = FileInfo.Status;
                                IsShowRemoveIcon = true;
                                StateHasChanged();
                                await InvokeMethod("sfBlazor.Uploader.raiseSuccessEvent", new object[] { FileElement, FileInfo });
                                if (Template == null)
                                {
                                    await InvokeMethod("sfBlazor.Uploader.serverRemoveIconBindEvent", new object[] { FileElement });
                                }
                            }
                        }
                        else
                        {
                            RemoveIconDisable = string.Empty;

                            // fileIndex--;
                            if (Template == null)
                            {
                                await InvokeMethod("sfBlazor.Uploader.serverRemoveIconBindEvent", new object[] { FileElement });
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        FileData[FileIndex - 1].Status = Localizer.GetText(UPLOAD_FAILED_KEY) ?? "File failed to upload";
                        FileListStatusName = FileData[FileIndex - 1].Status;
                        RemoveIconDisable = string.Empty;
                        StateHasChanged();
                        FailureEventArgs failureEventArgs = new FailureEventArgs()
                        {
                            ChunkIndex = (double)ChunkIndex,
                            ChunkSize = BufferSize,
                            Event = e,
                            File = FileInfo,
                            StatusText = Localizer.GetText(UPLOAD_FAILED_KEY) ?? "File failed to upload",
                            E = e,
                            TotalChunk = (double)TotalChunk
                        };
                        await SfBaseUtils.InvokeEvent<FailureEventArgs>(UploaderEvents?.OnFailure, failureEventArgs);
                    }
                }

                RemoveIconDisable = string.Empty;
                await SfBaseUtils.InvokeEvent<UploadChangeEventArgs>(UploaderEvents?.ValueChange, eventArgs);
            }
            catch (Exception)
            {
            }
        }

        private string GetFileName(string fileName)
        {
            string type = GetFileType(fileName);
            string[] names = fileName.Split(new string[] { "." + type }, StringSplitOptions.None);
            return names[0];
        }

#pragma warning disable CA1822 // Mark members as static
        private string GetFileType(string name)
#pragma warning restore CA1822 // Mark members as static
        {
            string extension = string.Empty;
            int index = name.LastIndexOf(".", StringComparison.Ordinal);
            if (index > 0)
            {
                extension = name.Substring(index + 1);
            }

            return (!string.IsNullOrEmpty(extension)) ? extension : string.Empty;
        }

#pragma warning disable CA1822 // Mark members as static
        private string GetFileSize(double fileSize)
#pragma warning restore CA1822 // Mark members as static
        {
            int i = -1;
            if (fileSize == 0)
            {
                return "0.0 KB";
            }

            do
            {
                fileSize = fileSize / 1024;
                i++;
            }
            while (fileSize > 99);
            if (i >= 2)
            {
                fileSize = fileSize * 1024;
                i = 1;
            }

            string[] sizeType = new string[] { "KB", "MB" };
            return Convert.ToString(Math.Round(fileSize, 1), CultureInfo.InvariantCulture) + SPACE + sizeType[i];
        }

        private async Task RenderPreloadFiles()
        {
            if (Template != null && UploadedFiles != null && UploadedFiles.Any())
            {
                for (int i = 0; i < UploadedFiles.Count; i++)
                {
                    var data = UploadedFiles[i];
                    UploadFileDetails preloadFileData = new UploadFileDetails
                    {
                        Name = data.Name + '.' + data.Type.Split('.')[data.Type.Split('.').Length - 1],
                        Size = data.Size,
                        Status = Localizer.GetText("uploadSuccessMessage"),
                        Type = data.Type,
                        StatusCode = "2",
                    };
                    if (FileData == null)
                    {
                        FileData = new List<UploadFileDetails> { preloadFileData };
                    }
                    else
                    {
                        FileData.Add(preloadFileData);
                    }
                }

                await ServerCreateFileList(FileData, IsForm);
            }
        }

        internal void ServerFileData(List<UploadFileDetails> fileData, bool isFormRender)
        {
            IsForm = isFormRender;
            FileData = fileData;
        }

        internal async Task ServerCreateFileList(List<UploadFileDetails> fileData, bool isFormRender)
        {
            IsForm = isFormRender;
            FileData = fileData;
            FileListClass = File_LIST_CLASS;
            FileListStatus = STATUS;
            IsClearAll = false;
            IsShowRemoveIcon = false;
            IsShowFileList = true;
            IsShowProgresBar = false;
            if (Template != null && UploadedFiles != null && UploadedFiles.Count != 0)
            {
                var uploadedFiles = fileData?.Where(file => file.StatusCode == "2").ToList();
                if (fileData != null && uploadedFiles?.Count == fileData.Count)
                {
                    actionBtnAttr = SfBaseUtils.UpdateDictionary("disabled", "disabled", actionBtnAttr);
                }
                else
                {
                    actionBtnAttr.Remove("disabled");
                }
            }

            StateHasChanged();
            await InvokeMethod("sfBlazor.Uploader.serverFileListElement", new object[] { FileElement, UlElementRef, ActionButtonRef, AutoUpload });
        }

        private void UpdateMinMaxValid(string statusText)
        {
            if (FileData.Count == 1 && !AutoUpload)
            {
                actionBtnAttr = SfBaseUtils.UpdateDictionary("disabled", "disabled", actionBtnAttr);
            }

            UploadStatus = VALIDATION_FAIL;
            FileListStatusName = statusText;
        }

        internal void ValidatedFileSize(UploadFileDetails file)
        {
            if (file.Size < MinFileSize)
            {
                UpdateMinMaxValid(Localizer.GetText(INVALID_MIN_FILE_KEY) ?? "File size is too small");
                file.ValidationMessages.MinSize = FileListStatusName;
            }
            else if (file.Size > MaxFileSize)
            {
                UpdateMinMaxValid(Localizer.GetText(INVALID_MAX_FILE_KEY) ?? "File size is too large");
                file.ValidationMessages.MaxSize = FileListStatusName;
            }
            else if (file.Status == (Localizer.GetText(READY_UPLOAD_KEY) ?? "Ready to upload"))
            {
                if (actionBtnAttr.ContainsKey(DISABLED) && !AutoUpload)
                {
                    actionBtnAttr.Remove("disabled");
                }

                UploadStatus = string.Empty;
                FileListStatusName = Localizer.GetText(READY_UPLOAD_KEY) ?? "Ready to upload";
                file.Status = FileListStatusName;
            }
            else
            {
                UpdateMinMaxValid(Localizer.GetText(INVALID_FILE_KEY) ?? "File type is not allowed");
                file.Status = FileListStatusName;
            }
        }

        internal void ClearAllFileList()
        {
            IsClearAll = true;
            FileData = new List<UploadFileDetails>();
            StateHasChanged();
        }

        internal async Task RemoveFileList(int index)
        {
            if (FileData != null)
            {
                FileData.RemoveRange(index, 1);
                if (FileData.Count == 0)
                {
                    IsClearAll = true;
                }

                StateHasChanged();
                if (Template == null)
                {
                    await InvokeMethod("sfBlazor.Uploader.serverRemoveIconBindEvent", new object[] { FileElement });
                }
            }
        }

        internal void UpdateTemplate(RenderFragment<FileInfo> template)
        {
            Template = template;
            StateHasChanged();
        }

        /// <summary>
        /// Update the dropdownlist fileds.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal void UpdateChildProperties(string key, object fieldValue)
        {
            switch (key)
            {
                case "Files":
                    UploadedFiles = files = (List<UploaderUploadedFiles>)fieldValue;
                    break;
                case "AsyncSettings":
                    var asyncSetting = fieldValue == null ? new UploaderAsyncSettings() : (UploaderAsyncSettings)fieldValue;
                    UploadAsyncSettings = asyncSettings = asyncSetting;
                    break;
                case "Buttons":
                    var button = fieldValue == null ? new UploaderButtons() : (UploaderButtons)fieldValue;
                    UploadButtons = buttons = button;
                    UpdateBrowsBtn();
                    break;
            }
        }
    }
}