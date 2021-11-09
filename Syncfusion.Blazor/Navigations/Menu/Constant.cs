namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// Menu is a graphical user interface that serve as navigation headers for your application.
    /// </summary>
    public partial class SfMenu<TValue>
    {
        private const string CONTAINER = "e-menu-container";
        private const string SFMENU = "sf-menu";
        private const string MENUCLASS = "e-lib e-menu e-control e-menu-parent";
        private const string HEADERTITLE = "Menu";
        private const string CREATED = "Created";
        private const string ZERO = "0";
        private const string HAMBURGER = " e-hamburger";
        private const string VERTICAL = " e-vertical";
        private const string ONOPEN = "OnOpen";
        private const string ONCLOSE = "OnClose";
        private const string FOCUSED = " e-focused";
        private const string SELECTED = " e-selected";
        private const string HEADER = "e-menu-header";
        private const string TITLE = "e-menu-title";
        private const string ICON = "e-icons e-menu-icon";
        private const string INITIALIZE = "sfBlazor.Menu.initialize";
        private const string CALCULATEPOS = "sfBlazor.Menu.calculatePosition";
        private const string FOCUSMENU = "sfBlazor.Menu.focusMenu";
        private const string UPDATESCROLL = "sfBlazor.Menu.updateScroll";
        private const string DESTROY = "sfBlazor.Menu.destroy";
    }
}

namespace Syncfusion.Blazor.Navigations.Internal
{
    public partial class CreateMenuItems<TValue, TItem>
    {
        private const string MENUROLE = "menubar";
        private const string ZERO = "0";
        private const string POPUP = "e-menu-popup";
    }

    public partial class CreateMenuItem<TValue, TItem>
    {
        private const string ITEMROLE = "menuitem";
        private const string MINUSONE = "-1";
        private const string CARET = "e-icons e-caret";
        private const string ICON = "e-menu-icon";
        private const string URL = "e-menu-text e-menu-url";
        private const string SPACE = " ";
        private const string FOCUSED = "e-focused";
        private const string SELECTED = "e-selected";
        private const string SELECT = "ItemSelected";
        private const string SUBMENU = "e-menu-parent e-ul";
        private const string SUBMENUWRAPPER = "e-menu-container e-menu-popup e-lib e-popup e-control";
        private const string TEXTINDENT = "text-indent: ";
        private const string PIXEL = "px;";
        private const string ARROWRIGHT = "ArrowRight";
        private const string ARROWLEFT = "ArrowLeft";
        private const string ARROWDOWN = "ArrowDown";
        private const string ARROWUP = "ArrowUp";
        private const string ENTER = "Enter";
        private const string HOME = "Home";
        private const string END = "End";
        private const string UPDATESCROLL = "sfBlazor.Menu.updateScroll";
    }
}