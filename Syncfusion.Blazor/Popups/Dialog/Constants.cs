namespace Syncfusion.Blazor.Popups
{
    public partial class SfDialog : SfBaseComponent
    {
        #region Common string constants
        private const string X = "X";
        private const string Y = "Y";
        private const string TYPE = "type";
        private const string ARIA_LABEL = "aria-label";
        private const string DRAG = "Drag";
        private const string FADE = "Fade";
        private const string RTL = "e-rtl";
        private const string BODY = "body";
        private const string NONE = "none";
        private const string BTN = "button";
        private const string DELAY = "delay";
        private const string CLASS = "class";
        private const string STYLE = "style";
        private const string TITLE = "title";
        private const string MODAL = "modal";
        private const string CLOSE = "Close";
        private const string WIDTH = "Width";
        private const string CENTER = "center";
        private const string DIALOG = "dialog";
        private const string HEIGHT = "Height";
        private const string ZINDEX = "ZIndex";
        private const string TARGET = "Target";
        private const string ESCAPE = "Escape";
        private const string Z_INDEX = "z-index";
        private const string ISMODAL = "IsModal";
        private const string ELEMENT = "element";
        private const string IS_MODAL = "IsModal";
        private const string DURATION = "duration";
        private const string CSSCLASS = "CssClass";
        private const string POSITION = "position";
        private const string DRAG_STOP = "DragStop";
        private const string MIN_HEIGHT = "MinHeight";
        private const string DRAG_START = "DragStart";
        private const string ENABLE_RTL = "EnableRtl";
        private const string ANIMATE_EFFECT = "effect";
        private const string SOUTH_WEST = "south-west";
        private const string SOUTH_EAST = "south-east";
        private const string NORTH_WEST = "north-west";
        private const string NORTH_EAST = "north-east";
        private const string ALL_DIRECTIONS = "south north east west north-east north-west south-east south-west";
        private const string SPACE = " ";
        private const string OVERLAY = "e-dlg-overlay";
        private const string USER_ACTION = "User Action";
        private const string DIALOG_MODAL = "e-dlg-modal";
        private const string POPUP_CLOSE = "e-popup-close";
        private const string RESIZABLE = "e-dlg-resizable";
        private const string CONTAINER = "e-dlg-container";
        private const string DIALOG_CLOSE = "Dialog_Close";
        private const string ENABLE_RESIZE = "EnableResize";
        private const string ALLOW_DRAGGING = "AllowDragging";
        private const string CLOSE_ON_ESCAPE = "CloseOnEscape";
        private const string ALLOWMAXHEIGHT = "allowMaxHeight";
        private const string DESTROY_DRAGGABLE = "destroyDraggable";
        private const string PREVENT_VISIBILITY = "preventVisibility";
        private const string ANIMATION_SETTINGS = "animationSettings";
        private const string RESIZE_ICON_DIRECTION = "resizeIconDirection";
        private const string CREATED_ENABLED = "createdEnabled";
        private const string DIALOG_ON_LOAD_CLASS = "e-dlg-load-on-demand";
        #endregion

        #region Dictionary string constants
        private const string DICTIONARY_TARGET = "target";
        private const string DICTIONARY_WIDTH = "width";
        private const string DICTIONARY_HEIGHT = "height";
        private const string DICTIONARY_ZINDEX = "zIndex";
        private const string DICTIONARY_IS_MODAL = "isModal";
        private const string DICTIONARY_VISIBLE = "visible";
        private const string DICTIONARY_CSSCLASS = "cssClass";
        private const string DICTIONARY_ENABLE_RTL = "enableRtl";
        private const string DICTIONARY_MIN_HEIGHT = "minHeight";
        private const string DICTIONARY_ENABLE_RESIZE = "enableResize";
        private const string DICTIONARY_ALLOW_DRAGGING = "allowDragging";
        private const string DICTIONARY_CLOSE_ON_ESCAPE = "closeOnEscape";
        private const string DICTIONARY_LOAD_ON_DEMAND = "isLoadOnDemand";
        #endregion

        #region JS invoke method string constants
        private const string SF_BLAZOR = "sfBlazor.";
        private const string SF_BLAZOR_DIALOG = SF_BLAZOR + "Dialog.";
        private const string JS_SHOW = SF_BLAZOR_DIALOG + "show";
        private const string JS_HIDE = SF_BLAZOR_DIALOG + "hide";
        private const string JS_DESTROY = SF_BLAZOR_DIALOG + "destroy";
        private const string JS_INITIALIZE = SF_BLAZOR_DIALOG + "initialize";
        private const string JS_GET_MAX_HEIGHT = SF_BLAZOR_DIALOG + "getMaxHeight";
        private const string JS_FOCUS_CONTENT = SF_BLAZOR_DIALOG + "focusContent";
        private const string JS_GET_CLASS_LIST = SF_BLAZOR_DIALOG + "getClassList";
        private const string JS_POPUP_CLOSE = SF_BLAZOR_DIALOG + "popupCloseHandler";
        private const string JS_PROPERTY_CHANGED = SF_BLAZOR_DIALOG + "propertyChanged";
        private const string JS_REFRESH_POSITION = SF_BLAZOR_DIALOG + "refreshPosition";
        #endregion

        #region Event name string constants
        private const string OPENED = "Opened";
        private const string CLOSED = "Closed";
        private const string CREATED = "Created";
        private const string DESTROYED = "Destroyed";
        #endregion
    }
}