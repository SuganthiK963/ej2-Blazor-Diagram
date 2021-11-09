namespace Syncfusion.Blazor.Inputs
{
    /// <summary>
    /// The uploader component is useful to upload images, documents, and other files to server.
    /// The component is the extended version of HTML5 that is uploaded with multiple file selection, auto upload, drag and drop, progress bar, preload files, and validation.
    /// </summary>
    public partial class SfUploader : SfBaseComponent
    {
        private const string ROOT = "e-uploader";
        private const string CONTROL_CONTAINER = "e-upload e-control-wrapper e-control-container";
        private const string INPUT_CONTAINER = "e-file-select";
        private const string DROP_AREA = "e-file-drop";
        private const string DROP_CONTAINER = "e-file-select-wrap";
        private const string LIST_PARENT = "e-upload-files";
        private const string File_LIST_CLASS = "e-upload-file-list";
        private const string STATUS = "e-file-status";
        private const string ACTION_BUTTONS = "e-upload-actions";
        private const string UPLOAD_BUTTONS = "e-file-upload-btn e-css e-btn e-flat e-primary";
        private const string CLEAR_BUTTONS = "e-file-clear-btn e-css e-btn e-flat";
        private const string UPLOAD_INPROGRESS = "e-upload-progress";
        private const string UPLOAD_SUCCESS = "e-upload-success";
        private const string VALIDATION_FAIL = "e-validation-fails";
        private const string RTL = "e-rtl";
        private const string DISABLED = "e-disabled";
        private const string RTL_CONTAINER = "e-rtl-container";
        private const string SPACE = " ";
        private const string TRUE = "true";
        private const string ALLOW_EXTENSIONS = "allowedExtensions";
        private const string ASYNC_SETTING = "asyncSettings";
        private const string AUTO_UPLOAD = "autoUpload";
        private const string BUTTON = "buttons";
        private const string CSSCLASS = "cssClass";
        private const string DROPAREA = "dropArea";
        private const string DIRECTORY_UPLOAD = "directoryUpload";
        private const string DROP_EFFECT = "dropEffect";
        private const string ENABLED = "enabled";
        private const string ENABLE_RTL = "enableRtl";
        private const string UPLOAD_FILES = "files";
        private const string MAX_FILE_SIZE = "maxFileSize";
        private const string MIN_FILE_SIZE = "minFileSize";
        private const string SHOW_FILE_LIST = "showFileList";
        private const string UPLOAD_MULTIPLE = "multiple";
        private const string SEQUENTIAL_UPLOAD = "sequentialUpload";
        private const string UPLOAD_TEMPLATE = "template";
        private const string PERSISTENCE = "enablePersistence";
        private const string ACTION_COMPLETE_ENABLED = "actionCompleteEnabled";
        private const string BEFORE_REMOVE_ENABLED = "beforeRemoveEnabled";
        private const string BEFORE_UPLOAD_ENABLED = "beforeUploadEnabled";
        private const string CANCEL_ENABLED = "cancelEnabled";
        private const string CHANGE_ENABLED = "changeEnabled";
        private const string CHUNK_FAILURE_ENABLED = "chunkFailuredEnabled";
        private const string CHUNK_UPLOADING_ENABLED = "chunkUploadingEnabled";
        private const string UPLOADING_ENABLED = "uploadingEnabled";
        private const string CLEAR_ENABLED = "clearEnabled";
        private const string FAILURE_ENABLED = "failuredEnabled";
        private const string FILE_RENDER_ENABLED = "fileListRenderEnabled";
        private const string PAUSED_ENABLED = "pausedEnabled";
        private const string PROGRESSING_ENABLED = "progressingEnabled";
        private const string REMOVING_ENABLED = "removingEnabled";
        private const string RESUME_ENABLED = "resumeEnabled";
        private const string SELECTED_ENABLED = "selectedEnabled";
        private const string SUCCESS_ENABLED = "successEnabled";
        private const string CHUNK_SUCCESS_ENABLED = "chunkSuccessEnabled";
        private const string LOCALE_TEXT = "localeText";
        private const string BROWSE_KEY = "Uploader_Browse";
        private const string ABORT_KEY = "Uploader_Abort";
        private const string CANCEL_KEY = "Uploader_Cancel";
        private const string CLEAR_KEY = "Uploader_Clear";
        private const string DELETE_KEY = "Uploader_Delete";
        private const string DROP_FILE_KEY = "Uploader_DropFilesHint";
        private const string FILE_UPLOAD_CANCEL = "Uploader_FileUploadCancel";
        private const string INPROGRESS_KEY = "Uploader_InProgress";
        private const string INVALID_FILE_KEY = "Uploader_InvalidFileType";
        private const string INVALID_MAX_FILE_KEY = "Uploader_InvalidMaxFileSize";
        private const string INVALID_MIN_FILE_KEY = "Uploader_InvalidMinFileSize";
        private const string PAUSE_KEY = "Uploader_Pause";
        private const string PAUSE_UPLOAD_KEY = "Uploader_PauseUpload";
        private const string READY_UPLOAD_KEY = "Uploader_ReadyToUploadMessage";
        private const string REMOVE_KEY = "Uploader_Remove";
        private const string REMOVED_FAILED_KEY = "Uploader_RemovedFailedMessage";
        private const string REMOVED_SUCCESS_KEY = "Uploader_RemovedSuccessMessage";
        private const string RESUME_KEY = "Uploader_Resume";
        private const string RETRY_KEY = "Uploader_Retry";
        private const string UPLOAD_KEY = "Uploader_Upload";
        private const string UPLOAD_FAILED_KEY = "Uploader_UploadFailedMessage";
        private const string UPLOAD_SUCCESS_KEY = "Uploader_UploadSuccessMessage";
    }
}