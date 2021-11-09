using System;
using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Inputs
{
    /// <summary>
    /// Specifies the list of files that will be preloaded on rendering of uploader component.
    /// </summary>
    public partial class SfUploader : SfBaseComponent
    {
        /// <summary>
        /// Specifies the id of the Uploader component.
        /// </summary>
        [Parameter]
        public string ID { get; set; }

        /// <summary>
        /// Specifies the content that has to be passed.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Specifies the extensions of the file types allowed in the Uploader component and pass the extensions
        /// with comma separators.
        /// <para> For example,if you want to upload specific image files, pass `AllowedExtensions` as ".jpg,.png".</para>
        /// </summary>
        [Parameter]
        public string AllowedExtensions { get; set; } = string.Empty;

        private string allowedExtensions { get; set; }

        /// <summary>
        /// Configures the save and remove URL to perform the upload operations in the server asynchronously.
        /// </summary>
        [Obsolete("The AsyncSettings is deprecated and will no longer be used.")]
        [Parameter]
        public UploaderAsyncSettings AsyncSettings
        {
            get { return UploadAsyncSettings; }
            set { UploadAsyncSettings = value;  }
        }

        private UploaderAsyncSettings asyncSettings { get; set; }

        private UploaderAsyncSettings UploadAsyncSettings { get; set; }

        /// <summary>
        /// By default, the Uploader component initiates automatic upload when the files are added in upload queue.
        /// <para>If you want to manipulate the files before uploading to server, disable the AutoUpload property.</para>
        /// <para>The buttons "upload" and "clear" will be hided from file list when AutoUpload property is true.</para>
        /// </summary>
        [Parameter]
        public bool AutoUpload { get; set; } = true;

        private bool autoUpload { get; set; }

        /// <summary>
        /// You can customize the default text of "browse, clear, and upload" buttons with plain text or HTML elements.
        /// The buttons' text can be customized from localization also.
        /// <para>If you configured both Locale and Buttons property,the Uploader component considers the Buttons property value.</para>
        /// </summary>
        [Obsolete("The Buttons is deprecated and will no longer be used.")]
        [Parameter]
        public UploaderButtons Buttons { get { return UploadButtons; } set { UploadButtons = value; } }

        private UploaderButtons buttons { get; set; }
        private UploaderButtons UploadButtons { get; set; }

        /// <summary>
        /// Specifies the CSS class name that can be appended with root element of the Uploader.
        /// One or more custom CSS classes can be added to a Uploader.
        /// </summary>
        [Parameter]
        public string CssClass { get; set; } = string.Empty;

        private string cssClass { get; set; }

        /// <summary>
        /// Specifies a boolean value that indicates whether the folder of files can be browsed in the Uploader component.
        /// </summary>
        [Parameter]
        public bool DirectoryUpload { get; set; }

        private bool directoryUpload { get; set; }

        /// <summary>
        /// Specifies the drop target to handle the drag-and-drop upload.
        /// By default, the Uploader creates wrapper around file input that will act as drop target.
        /// </summary>
        [Parameter]
        public string DropArea { get; set; }

        private string dropArea { get; set; }

        /// <summary>
        /// Specifies the drag operation effect to the Uploader component.
        /// <para> Possible values are .</para>
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
        [Parameter]
        public DropEffect DropEffect { get; set; } = DropEffect.Default;

        private DropEffect dropEffect { get; set; }

        /// <summary>
        /// Enable or disable persisting Uploader state between page reloads. If enabled, the `Files` state will be persisted.
        /// </summary>
        [Parameter]
        public bool EnablePersistence { get; set; } = false;

        private bool enablePersistence { get; set; }

        /// <summary>
        /// Enable or disable rendering Uploader in right to left direction.
        /// </summary>
        [Parameter]
        public bool EnableRtl { get; set; }

        private bool enableRtl { get; set; }

        /// <summary>
        /// Specifies a boolean value that indicates whether the Uploader allows the user to interact with it.
        /// </summary>
        [Parameter]
        public bool Enabled { get; set; } = true;

        private bool enabled { get; set; }

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
        [Obsolete("The Files is deprecated and will no longer be used.")]
        [Parameter]
        public List<UploaderUploadedFiles> Files 
        { 
            get { return UploadedFiles; } 
            set { UploadedFiles = value; }
        }

        private List<UploaderUploadedFiles> files { get; set; }

        private List<UploaderUploadedFiles> UploadedFiles { get; set; }

        /// <summary>
        /// <para>You can add the additional html attributes such as styles, class, and more to the root element.</para>
        /// <para>If you configured both property and equivalent html attributes, then the component considers the property value.</para>
        /// </summary>
        [Parameter]
        public Dictionary<string, object> HtmlAttributes { get; set; }

        /// <summary>
        /// You can add the additional input attributes such as disabled, value, and more to the root element.
        /// If you configured both property and equivalent input attribute, then the component considers the property value.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> InputAttributes { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Specifies the global culture and localization of the Uploader component.
        /// </summary>
        [Obsolete("The Locale is deprecated and will no longer be used. Hereafter, the Locale works based on the current culture.")]
        [Parameter]
        public string Locale { get; set; } = string.Empty;
        internal string UploaderLocale { get; set; } = string.Empty;

        /// <summary>
        /// Specifies the maximum allowed file size to be uploaded in bytes.
        /// The property used to make sure that you cannot upload too large files.
        /// </summary>
        [Parameter]
        public double MaxFileSize { get; set; } = 30000000;

        private double maxFileSize { get; set; }

        /// <summary>
        /// Specifies the minimum file size to be uploaded in bytes.
        /// The property used to make sure that you cannot upload empty files and small files.
        /// </summary>
        [Parameter]
        public double MinFileSize { get; set; }

        private double minFileSize { get; set; }

        /// <summary>
        /// Specifies a boolean value that indicates whether the multiple files can be browsed or
        /// dropped simultaneously in the Uploader component.
        /// </summary>
        [Obsolete("This Multiple property is deprecated. Use AllowMultiple property to achieve the functionality.")]
        [Parameter]
        public bool Multiple
        {
            get { return UploadMultiple; }
            set { UploadMultiple = value; }
        }
        private bool multiple { get; set; }

        private bool UploadMultiple { get; set; } = true;

        /// <summary>
        /// Specifies a boolean value that indicates whether the multiple files can be browsed or dropped simultaneously in the Uploader component.
        /// </summary>
        [Parameter]
        public bool AllowMultiple { get; set; } = true;

        private bool allowMultiple { get; set; }

        /// <summary>
        /// By default, the file Uploader component is processing the multiple files simultaneously.
        /// <para>If SequentialUpload property is enabled, the file upload component performs the upload one after the other.</para>
        /// </summary>
        [Parameter]
        public bool SequentialUpload { get; set; }

        private bool sequentialUpload { get; set; }

        /// <summary>
        /// Specifies a boolean value that indicates whether the default file list can be rendered.
        /// The property used to prevent default file list and design own template for file list.
        /// </summary>
        [Parameter]
        public bool ShowFileList { get; set; } = true;

        private bool showFileList { get; set; }

        /// <summary>
        /// Specifies the tab order of the component.
        /// </summary>
        [Parameter]
        public int TabIndex { get; set; }

        /// <summary>
        /// Specifies the HTML string that used to customize the content of each file in the list.
        /// </summary>
        [Parameter]
        public RenderFragment<FileInfo> Template { get; set; }

        /// <summary>
        /// Triggers when the content of input has changed and gets focus-out.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Parameter]
        public EventCallback<ChangeEventArgs> OnChange { get; set; }

        /// <summary>
        /// Method provides initial values to the properties.
        /// </summary>
        /// <exclude/>
        protected void PropertyInitialized()
        {
            UpdateChildProperties("Buttons", UploadButtons);
            UpdateChildProperties("Files", UploadedFiles);
            UpdateChildProperties("AsyncSettings", UploadAsyncSettings);
            allowedExtensions = AllowedExtensions;
            autoUpload = AutoUpload;
            cssClass = CssClass;
            directoryUpload = DirectoryUpload;
            dropArea = DropArea;
            dropEffect = DropEffect;
            enablePersistence = EnablePersistence;
            enableRtl = EnableRtl;
            enabled = Enabled;
            files = UploadedFiles;
            maxFileSize = MaxFileSize;
            minFileSize = MinFileSize;
            allowMultiple = AllowMultiple;
            sequentialUpload = SequentialUpload;
            showFileList = ShowFileList;
            if (string.IsNullOrEmpty(ID))
            {
                ID = SfBaseUtils.GenerateID("uploader");
            }
        }

        /// <summary>
        /// Triggers while dynamically updating the component properties.
        /// </summary>
        /// <exclude/>
        protected void PropertyParametersSet()
        {
            allowedExtensions = NotifyPropertyChanges(nameof(AllowedExtensions), AllowedExtensions, allowedExtensions);
            asyncSettings = NotifyPropertyChanges(nameof(UploadAsyncSettings), UploadAsyncSettings, asyncSettings);
            autoUpload = NotifyPropertyChanges(nameof(AutoUpload), AutoUpload, autoUpload);
            buttons = NotifyPropertyChanges(nameof(UploadButtons), UploadButtons, buttons);
            NotifyPropertyChanges(nameof(CssClass), CssClass, cssClass);
            directoryUpload = NotifyPropertyChanges(nameof(DirectoryUpload), DirectoryUpload, directoryUpload);
            dropArea = NotifyPropertyChanges(nameof(DropArea), DropArea, dropArea);
            dropEffect = NotifyPropertyChanges(nameof(DropEffect), DropEffect, dropEffect);
            enablePersistence = NotifyPropertyChanges(nameof(EnablePersistence), EnablePersistence, enablePersistence);
            enableRtl = NotifyPropertyChanges(nameof(EnableRtl), EnableRtl, enableRtl);
            enabled = NotifyPropertyChanges(nameof(Enabled), Enabled, enabled);
            files = NotifyPropertyChanges(nameof(UploadedFiles), UploadedFiles, files);
            maxFileSize = NotifyPropertyChanges(nameof(MaxFileSize), MaxFileSize, maxFileSize);
            minFileSize = NotifyPropertyChanges(nameof(MinFileSize), MinFileSize, minFileSize);
            allowMultiple = NotifyPropertyChanges(nameof(AllowMultiple), AllowMultiple, allowMultiple);
            sequentialUpload = NotifyPropertyChanges(nameof(SequentialUpload), SequentialUpload, sequentialUpload);
            showFileList = NotifyPropertyChanges(nameof(ShowFileList), ShowFileList, showFileList);
        }
    }
}