namespace Syncfusion.Blazor.DropDowns
{
    /// <summary>
    /// ListBox component used to display a list of items. Users can select one or more items in the list using a checkbox or by keyboard selection.
    /// It supports sorting, grouping, reordering and drag and drop of items.
    /// </summary>
    public partial class SfListBox<TValue, TItem>
    {
        private const string CONTAINER = "e-listbox-container";
        private const string TOOLBAR = " e-listboxtool-container";
        private const string TOOL = "e-listbox-tool";
        private const string RIGHT = " e-right";
        private const string LEFT = " e-left";
        private const string COMPONENTCLASS = "e-control e-listbox e-lib";
        private const string BTNCLASS = "e-lib e-btn e-control e-icon-btn";
        private const string BTNICONS = "e-btn-icon e-icons e-";
        private const string LISTBOX = "listbox";
        private const string SELECTALL = "e-selectall-parent";
        private const string SELECTALLTEXT = "e-all-text";
        private const string BTN = "button";
        private new const string SPACE = " ";
        private const string RTL = " e-rtl";
        private const string FILTERPARENT = "e-filter-parent";
        private const string DISPLAY = "display: none;";
        private const string SELECT = "e-hidden-select";
        private const string DISABLED = "e-disabled";
        private const string ROLE = "role";
        private const string TOOLBARROLE = "toolbar";
        private const string CREATED = "Created";
        private const string FILTERCLASS = "e-input-filter";
        private const string DISABLE = " e-disabled";
        private const string ULCLASS = "e-list-parent e-ul";
        private const string NORECORDCLASS = "e-list-nrt";
        private const string PREVENTDRAG = "e-drag-prevent";
        private const string PRESENTATION = "presentation";
        private const string OPTION = "option";
        private const string ACTIONCOMPLETE = "ActionComplete";
        private const string ACTIONBEGIN = "ActionBegin";
        private const string MINHEIGHT = "min-height: 100px;";
        private const string ULSTYLE = "flex: 1; overflow: auto;";
        private const string STYLE = "style";
        private const string HEIGHT = "height:";
        private const string ONITEMRENDER = "OnItemRender";
        private const string NORECORDKEY = "ListBox_NoRecordsTemplate";
        private const string NORECORDVALUE = "No records found";
        private const string ACTIONFAILUREKEY = "ListBox_ActionFailureTemplate";
        private const string ACTIONFAILUREVALUE = "The action failure";
        private const string SELECTALLKEY = "ListBox_SelectAllText";
        private const string SELECTALLVALUE = "Select All";
        private const string UNSELECTALLKEY = "ListBox_UnSelectAllText";
        private const string UNSELECTALLVALUE = "Unselect All";
        private const string MOVEUPKEY = "ListBox_MoveUp";
        private const string MOVEUPVALUE = "Move Up";
        private const string MOVEDOWNKEY = "ListBox_MoveDown";
        private const string MOVEDOWNVALUE = "Move Down";
        private const string MOVETOKEY = "ListBox_MoveTo";
        private const string MOVETOVALUE = "Move To";
        private const string MOVEFROMKEY = "ListBox_MoveFrom";
        private const string MOVEFROMVALUE = "Move From";
        private const string MOVEALLTOKEY = "ListBox_MoveAllTo";
        private const string MOVEALLTOVALUE = "Move All To";
        private const string MOVEALLFROMKEY = "ListBox_MoveAllFrom";
        private const string MOVEALLFROMVALUE = "Move All From";
        private const string MIXED = "mixed";
        private const string SELECTED = "e-selected";
        private const string FOCUSED = "e-focused";
        private const string VALUECHANGE = "ValueChange";
        private const string ARROWDOWN = "ArrowDown";
        private const string ARROWUP = "ArrowUp";
        private const string KEYA = "KeyA";
        private const string SPACEKEY = "Space";
        private const string MOVEUP = "MoveUp";
        private const string MOVEDOWN = "MoveDown";
        private const string MOVETO = "MoveTo";
        private const string MOVEFROM = "MoveFrom";
        private const string MOVEALLTO = "MoveAllTo";
        private const string MOVEALLFROM = "MoveAllFrom";
        private const string DATAVALUE = "data-value";
        private const string TABINDEX = "tabindex";
        private const string TRUE = "true";
        private const string FALSE = "false";
        private const string ARIASELECTED = "aria-selected";
        private const string INITIALIZE = "sfBlazor.ListBox.initialize";
        private const string DESTROY = "sfBlazor.ListBox.destroy";
        private const string PROPERTYCHANGED = "sfBlazor.ListBox.onPropertyChanged";
        private const string GETSCOPEDLISTBOX = "sfBlazor.ListBox.getScopedListBox";
        private const string TOOLBARCLASS = "e-listbox-wrapper e-wrapper e-lib e-filter-list";
        private const string HOME = "Home";
        private const string END = "End";
    }
}