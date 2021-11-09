namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// ContextMenu is a graphical user interface that appears on the user right click/touch hold operation.
    /// </summary>
    public partial class SfContextMenu<TValue>
    {
        private const string CONTAINER = "e-contextmenu-container e-sfcontextmenu";
        private const string CONTEXTMENU = "e-control e-contextmenu e-lib e-menu-parent";
        private const string SUBMENU = "e-menu-parent e-ul";
        private const string HEADER = "e-menu-item e-menu-header";
        private const string PREVIOUS = "e-menu-icon e-icons e-previous";
        private const string SPACE = " ";
        private const string CARET = "e-icons e-caret";
        private const string URL = "e-menu-text e-menu-url";
        private const string SFCONTEXTMENU = "sfcontextmenu";
        private const string ICON = "e-menu-icon";
        private const string INITIALIZE = "sfBlazor.ContextMenu.initialize";
        private const string PROPERTYCHANGED = "sfBlazor.ContextMenu.onPropertyChanged";
        private const string TRANSPARENT = " e-transparent";
        private const string CREATED = "Created";
        private const string OPENED = "Opened";
        private const string ONOPEN = "OnOpen";
        private const string BLOCKSTYLE = " display: block;";
        private const string CMENUPOS = "sfBlazor.ContextMenu.contextMenuPosition";
        private const string SUBMENUPOS = "sfBlazor.ContextMenu.subMenuPosition";
        private const string DESTROY = "sfBlazor.ContextMenu.destroy";
        private const string MENUCONTAINER = " e-menu-container e-menu-popup e-lib e-control";
        private const string MENUSUBMENUPOS = "sfBlazor.Menu.subMenuPosition";
    }
}