using System;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Navigations.Internal;
using System.Collections.Generic;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// Interface for OnOpen/OnClose event.
    /// </summary>
    public class BeforeOpenCloseMenuEventArgs<T>
    {
        /// <summary>
        /// Set true to prevent menu from opening.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Specifies the menu container element.
        /// </summary>
        public ElementReference Element { get; set; }

        /// <summary>
        /// Specifies the current menu items.
        /// </summary>
        public List<T> Items { get; set; }

        /// <summary>
        /// Specifies name of the event.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Specifies the parent item.
        /// </summary>
        public T ParentItem { get; set; }

        /// <summary>
        /// Specifies the menu container height to show the scrollable menu.
        /// It is applicable only when the EnableScrolling property is enabled.
        /// </summary>
        public double ScrollHeight { get; set; }

        /// <summary>
        /// Specifies the clientY position of the menu.
        /// </summary>
        public double? Top { get; set; }

        /// <summary>
        /// Specifies the clientX position of the menu.
        /// </summary>
        public double? Left { get; set; }
    }

    /// <summary>
    /// Interface for OnItemRender/ItemSelected event.
    /// </summary>
    public class MenuEventArgs<T>
    {
        /// <summary>
        /// Specifies the menu container element.
        /// </summary>
        public ElementReference Element { get; set; }

        /// <summary>
        /// Specifies the item select event.
        /// </summary>
        public System.EventArgs Event { get; set; }

        /// <summary>
        /// Specifies the selected item.
        /// </summary>
        public T Item { get; set; }

        /// <summary>
        /// Specifies name of the event.
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// Interface for Opened/Closed event.
    /// </summary>
    public class OpenCloseMenuEventArgs<T>
    {
        /// <summary>
        /// Specifies the menu container element.
        /// </summary>
        public ElementReference Element { get; set; }

        /// <summary>
        /// Specifies the current menu items.
        /// </summary>
        public List<T> Items { get; set; }

        /// <summary>
        /// Specifies name of the event.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Specifies the parent item.
        /// </summary>
        public T ParentItem { get; set; }

        /// <summary>
        /// Specifies the Navigation Index.
        /// <exclude/>
        /// </summary>
        public int NavigationIndex { get; set; }
    }

    /// <summary>
    /// Interface for MenuItem.
    /// </summary>
    public class MenuItemModel : ItemModelBase
    {
        /// <summary>
        /// Specifies the list of menu item model.
        /// </summary>
        public List<MenuItemModel> Items { get; set; }
    }
}

namespace Syncfusion.Blazor.Navigations.Internal
{
    public class ItemModelBase
    {
        /// <summary>
        /// Specifies the class to include icons.
        /// </summary>
        public string IconCss { get; set; }

        /// <summary>
        /// Specifies the menu item id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Separator are either horizontal or vertical lines used to group menu items.
        /// </summary>
        public bool Separator { get; set; }

        /// <summary>
        /// Specifies the menu item disable state.
        /// </summary>
        public bool Disabled { get; set; }

        /// <summary>
        /// Specifies the menu item hidden state.
        /// </summary>
        public bool Hidden { get; set; }

        /// <summary>
        /// Specifies the text of the menu item.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Specifies the URL of the menu item.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Used to add additional attributes to the menu item.
        /// </summary>
        public Dictionary<string, object> HtmlAttributes { get; set; }
    }

    public class ItemModel<T> : ItemModelBase
    {
        public List<T> Items { get; set; }

        public string ParentId { get; set; }
    }

    public class ClassCollection
    {
        public string ItemClass { get; set; }

        public List<ClassCollection> ClassList { get; set; }
    }

    public class MenuOptions
    {
        public ElementReference Element { get; set; }

        public ElementReference Popup { get; set; }

        public int ItemIndex { get; set; }

        public double ScrollHeight { get; set; }

        public bool IsRtl { get; set; }

        public bool IsVertical { get; set; }

        public bool ShowItemOnClick { get; set; }

        public bool EnableScrolling { get; set; }

        public List<int> NavigationIndex { get; set; }

        public Orientation Orientation { get; set; }
    }

    public class CurrentNavProps
    {
        public int ItemIndex { get; set; }

        public List<ClassCollection> ItemClasses { get; set; }

        public int UlIndex { get; set; }
    }
}