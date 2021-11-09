namespace Syncfusion.Blazor.Calendars
{
    /// <summary>
    /// The DatePicker is a graphical user interface component that allows the user to select or enter a date value.
    /// </summary>
    public partial class SfDatePicker<TValue>
    {
        private const string INVALID = "invalid";
        private const string MODIFIED_INVALID = "modified invalid";
        private const string MODIFIED_VALID = "modified valid";
        private const string ERROR_CLASS = "e-error";
        private const string SUCCESS_CLASS = "e-success";
        private const string RTL = "e-rtl";
        private const string NOEDIT = "e-non-edit";
        private const string POPUP_CONTAINER = "e-popup-wrapper e-popup-container";

        /// <summary>
        /// Specifies the popup class.
        /// </summary>
        /// <exclude/>
        protected const string POPUP = "e-popup";
        private const string DAY_HEADER_WIDE = "e-calendar-day-header-lg";
        private const string CALENDAR_ROOT = "e-calendar e-lib";
        /// <summary>
        /// Specifies the component element class.
        /// </summary>
        /// <exclude/>
        protected virtual string ROOT { get; set; } = "e-control e-datepicker e-lib";
        /// <summary>
        /// Specifies the container class.
        /// </summary>
        /// <exclude/>
        protected virtual string CONTAINER_CLASS { get; set; } = "e-date-wrapper e-date-container";
        /// <summary>
        /// Specifies the component class.
        /// </summary>
        /// <exclude/>
        protected const string DATE_PICKER = "e-datepicker";
        private const string DEPTH = "Depth";
        private const string MIN = "Min";
        private const string MAX = "Max";
        private const string LOCALE = "Locale";
        private const string ARABIC = "ar";
        private const string THAILAND = "th";
        /// <summary>
        /// Specifies the aria expanded.
        /// </summary>
        /// <exclude/>
        protected const string ARIA_EXPANDED = "aria-expanded";
        private const string FALSE = "false";
        /// <summary>
        /// Specifies the true.
        /// </summary>
        /// <exclude/>
        protected const string TRUE = "true";
        private const string ARIA_LIVE = "aria-live";
        private const string ASSERTIVE = "assertive";
        private const string ARIA_AUTOMIC = "aria-atomic";
        private const string ARIA_HAS_POPUP = "aria-haspopup";
        private const string ARIA_ACTIVE_DESCENDANT = "aria-activedescendant";
        private const string NULL_VALUE = "null";
        private const string ARIA_OWN = "aria-owns";
        private const string OPTIONS = "_options";
        private const string ROLE = "role";
        private const string COMBOBOX = "combobox";
        private const string AUTO_CORRECT = "autocorrect";
        private const string OFF = "off";
        private const string SPELL_CHECK = "spellcheck";
        private const string ARIA_INVALID = "aria-invalid";
        /// <summary>
        /// Specifies the active class to the list.
        /// </summary>
        /// <exclude/>
        protected const string ACTIVE = "e-active";
        /// <summary>
        /// Specifies the input readonly.
        /// </summary>
        /// <exclude/>
        protected const string READ_ONLY = "readonly";
        /// <summary>
        /// Specifies the input focus.
        /// </summary>
        /// <exclude/>
        protected const string INPUT_FOCUS = "e-input-focus";
        private const string ALT_UP_ARROW = "altUpArrow";
        private const string ALT_DOWN_ARROW = "altDownArrow";
        private const string ESCAPE = "escape";
        private const string ENTER = "enter";
        private const string TAB = "tab";
        private const string SHIFT_TAB = "shiftTab";
        private const char ARABIC_START_DIGIT = (char)1632;
        private const char ARABIC_END_DIGIT = (char)1641;
        private const char THAILAND_START_DIGIT = (char)3664;
        private const char THAILAND_END_DIGIT = (char)3675;
        private const string VALUE = "Value";
        private const string FORMAT = "Format";
        protected const string DATE_ICON = "e-date-icon e-icons";
        /// <summary>
        /// Specifies the model class.
        /// </summary>
        /// <exclude/>
        protected const string MODEL = "model";
        /// <summary>
        /// Specifies the target.
        /// </summary>
        /// <exclude/>
        protected const string BODY = "body";
        private const string DEVICE = "e-device";
        private const string FORMAT_YEAR = "yyyy";
        private const string FORMAT_MONTH = "MMMM dd";
        private const string FORMAT_DAY = "ddd,";
        /// <summary>
        /// Specifies the model header.
        /// </summary>
        /// <exclude/>
        protected const string MODEL_HEADER = "e-model-header";
        /// <summary>
        /// Specifies the model year.
        /// </summary>
        /// <exclude/>
        protected const string MODEL_YEAR = "e-model-year";
        /// <summary>
        /// Specifies the model day.
        /// </summary>
        /// <exclude/>
        protected const string MODEL_DAY = "e-model-day";
        /// <summary>
        /// Specifies the model month.
        /// </summary>
        /// <exclude/>
        protected const string MODEL_MONTH = "e-model-month";
        /// <summary>
        /// Specifies the popup holder.
        /// </summary>
        /// <exclude/>
        protected const string POPUP_HOLDER = "e-datepicker e-popup-holder";
    }
}
