using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor
{
    public partial class LocaleData
    {
        [JsonPropertyName("colorpicker")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ColorPickerLocale ColorPicker { get; set; } = new ColorPickerLocale();
        /// <summary>
        /// Returns the upload locale.
        /// </summary>
        [JsonPropertyName("uploader")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public UploaderLocale Uploader { get; set; } = new UploaderLocale();
        /// <summary>
        /// Returns the numerictextbox locale.
        /// </summary>
        [JsonPropertyName("numerictextbox")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public NumericTextBoxLocale NumericTextBox { get; set; } = new NumericTextBoxLocale();

        [JsonPropertyName("slider")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public SliderLocale Slider { get; set; } = new SliderLocale();
        /// <summary>
        /// Returns the formValidator locale.
        /// </summary>
        [JsonPropertyName("formValidator")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public FormValidatorLocale FormValidator { get; set; } = new FormValidatorLocale();
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public class ColorPickerLocale
    {
        public string Apply { get; set; } = "Apply";

        public string Cancel { get; set; } = "Cancel";

        public string ModeSwitcher { get; set; } = "Switch Mode";
    }
    /// <summary>
    /// Gets or sets the UploaderLocale property.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class UploaderLocale
    {
        /// <summary>
        /// Gets or sets the Browse property.
        /// </summary>
        public string Browse { get; set; } = "Browse...";
        /// <summary>
        /// Gets or sets the Clear property.
        /// </summary>
        public string Clear { get; set; } = "Clear";
        /// <summary>
        /// Gets or sets the Upload property.
        /// </summary>
        public string Upload { get; set; } = "Upload";
        /// <summary>
        /// Gets or sets the DropFilesHint property.
        /// </summary>
        [JsonPropertyName("dropFilesHint")]
        public string DropFilesHint { get; set; } = "Or drop files here";
        /// <summary>
        /// Gets or sets the InvalidMaxFileSize property.
        /// </summary>
        [JsonPropertyName("invalidMaxFileSize")]
        public string InvalidMaxFileSize { get; set; } = "File size is too large";
        /// <summary>
        /// Gets or sets the InvalidMinFileSize property.
        /// </summary>
        [JsonPropertyName("invalidMinFileSize")]
        public string InvalidMinFileSize { get; set; } = "File size is too small";
        /// <summary>
        /// Gets or sets the InvalidFileType property.
        /// </summary>
        [JsonPropertyName("invalidFileType")]
        public string InvalidFileType { get; set; } = "File type is not allowed";
        /// <summary>
        /// Gets or sets the UploadFailedMessage property.
        /// </summary>
        [JsonPropertyName("uploadFailedMessage")]
        public string UploadFailedMessage { get; set; } = "File failed to upload";
        /// <summary>
        /// Gets or sets the UploadSuccessMessage property.
        /// </summary>
        [JsonPropertyName("uploadSuccessMessage")]
        public string UploadSuccessMessage { get; set; } = "File uploaded successfully";
        /// <summary>
        /// Gets or sets the RemovedSuccessMessage property.
        /// </summary>
        [JsonPropertyName("removedSuccessMessage")]
        public string RemovedSuccessMessage { get; set; } = "File removed successfully";
        /// <summary>
        /// Gets or sets the RemovedFailedMessage property.
        /// </summary>
        [JsonPropertyName("removedFailedMessage")]
        public string RemovedFailedMessage { get; set; } = "Unable to remove file";
        /// <summary>
        /// Gets or sets the InProgress property.
        /// </summary>
        [JsonPropertyName("inProgress")]
        public string InProgress { get; set; } = "Uploading";
        /// <summary>
        /// Gets or sets the ReadyToUploadMessage property.
        /// </summary>
        [JsonPropertyName("readyToUploadMessage")]
        public string ReadyToUploadMessage { get; set; } = "Ready to upload";
        /// <summary>
        /// Gets or sets the Abort property.
        /// </summary>
        [JsonPropertyName("abort")]
        public string Abort { get; set; } = "Abort";
        /// <summary>
        /// Gets or sets the Remove property.
        /// </summary>
        [JsonPropertyName("remove")]
        public string Remove { get; set; } = "Remove";
        /// <summary>
        /// Gets or sets the Cancel property.
        /// </summary>
        [JsonPropertyName("cancel")]
        public string Cancel { get; set; } = "Cancel";
        /// <summary>
        /// Gets or sets the Delete property.
        /// </summary>
        [JsonPropertyName("delete")]
        public string Delete { get; set; } = "Delete file";
        /// <summary>
        /// Gets or sets the PauseUpload property.
        /// </summary>
        [JsonPropertyName("pauseUpload")]
        public string PauseUpload { get; set; } = "File upload paused";
        /// <summary>
        /// Gets or sets the Browse property.
        /// </summary>
        [JsonPropertyName("Pause")]
        public string Pause { get; set; } = "Pause";
        /// <summary>
        /// Gets or sets the Resume property.
        /// </summary>
        [JsonPropertyName("resume")]
        public string Resume { get; set; } = "Resume";
        /// <summary>
        /// Gets or sets the Retry property.
        /// </summary>
        [JsonPropertyName("retry")]
        public string Retry { get; set; } = "Retry";
        /// <summary>
        /// Gets or sets the FileUploadCancel property.
        /// </summary>
        [JsonPropertyName("fileUploadCancel")]
        public string FileUploadCancel { get; set; } = "File upload canceled";
    }
    /// <summary>
    /// Gets or sets the NumericTextBoxLocale property.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class NumericTextBoxLocale
    {
        /// <summary>
        /// Gets or sets the IncrementTitle property.
        /// </summary>
        [JsonPropertyName("incrementTitle")]
        public string IncrementTitle { get; set; } = "Increment value";
        /// <summary>
        /// Gets or sets the DecrementTitle property.
        /// </summary>
        [JsonPropertyName("decrementTitle")]
        public string DecrementTitle { get; set; } = "Decrement value";
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public class SliderLocale
    {
        [JsonPropertyName("incrementTitle")]
        public string IncrementTitle { get; set; } = "Increase";

        [JsonPropertyName("decrementTitle")]
        public string DecrementTitle { get; set; } = "Decrease";
    }
    /// <summary>
    /// Gets or sets the FormValidatorLocale property.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class FormValidatorLocale
    {
        /// <summary>
        /// Gets or sets the Required property.
        /// </summary>
        [JsonPropertyName("required")]
        public string Required { get; set; } = "This field is required.";
        /// <summary>
        /// Gets or sets the Email property.
        /// </summary>
        [JsonPropertyName("email")]
        public string Email { get; set; } = "Please enter a valid email address.";
        /// <summary>
        /// Gets or sets the Url property.
        /// </summary>
        [JsonPropertyName("url")]
        public string Url { get; set; } = "Please enter a valid URL.";
        /// <summary>
        /// Gets or sets the Date property.
        /// </summary>
        [JsonPropertyName("date")]
        public string Date { get; set; } = "Please enter a valid date.";
        /// <summary>
        /// Gets or sets the DateIso property.
        /// </summary>
        [JsonPropertyName("dateIso")]
        public string DateIso { get; set; } = "Please enter a valid date ( ISO ).";
        /// <summary>
        /// Gets or sets the Creditcard property.
        /// </summary>
        [JsonPropertyName("creditcard")]
        public string Creditcard { get; set; } = "Please enter valid card number";
        /// <summary>
        /// Gets or sets the Number property.
        /// </summary>
        [JsonPropertyName("number")]
        public string Number { get; set; } = "Please enter a valid number.";
        /// <summary>
        /// Gets or sets the Digits property.
        /// </summary>
        [JsonPropertyName("digits")]
        public string Digits { get; set; } = "Please enter only digits.";
        /// <summary>
        /// Gets or sets the MaxLength property.
        /// </summary>
        [JsonPropertyName("maxLength")]
        public string MaxLength { get; set; } = "Please enter no more than {0} characters.";
        /// <summary>
        /// Gets or sets the MinLength property.
        /// </summary>
        [JsonPropertyName("minLength")]
        public string MinLength { get; set; } = "Please enter at least {0} characters.";
        /// <summary>
        /// Gets or sets the RangeLength property.
        /// </summary>
        [JsonPropertyName("rangeLength")]
        public string RangeLength { get; set; } = "Please enter a value between {0} and {1} characters long.";
        /// <summary>
        /// Gets or sets the Range property.
        /// </summary>
        [JsonPropertyName("range")]
        public string Range { get; set; } = "Please enter a value between {0} and {1}.";
        /// <summary>
        /// Gets or sets the Max property.
        /// </summary>
        [JsonPropertyName("max")]
        public string Max { get; set; } = "Please enter a value less than or equal to {0}.";
        /// <summary>
        /// Gets or sets the Min property.
        /// </summary>
        [JsonPropertyName("min")]
        public string Min { get; set; } = "Please enter a value greater than or equal to {0}.";
        /// <summary>
        /// Gets or sets the Regex property.
        /// </summary>
        [JsonPropertyName("regex")]
        public string Regex { get; set; } = "Please enter a correct value.";
        /// <summary>
        /// Gets or sets the Tel property.
        /// </summary>
        [JsonPropertyName("tel")]
        public string Tel { get; set; } = "Please enter a valid phone number.";
        /// <summary>
        /// Gets or sets the Pattern property.
        /// </summary>
        [JsonPropertyName("pattern")]
        public string Pattern { get; set; } = "Please enter a correct pattern value.";
        /// <summary>
        /// Gets or sets the EqualTo property.
        /// </summary>
        [JsonPropertyName("equalTo")]
        public string EqualTo { get; set; } = "Please enter the valid match text";
    }
}