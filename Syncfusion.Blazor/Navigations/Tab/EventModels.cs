using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// A class that holds options to control the adding or added item.
    /// </summary>
    public class AddEventArgs
    {
        /// <summary>
        /// Defines the added Tab item element.
        /// </summary>
        public List<TabItemModel> AddedItems { get; set; }

        /// <summary>
        /// Specifies a value whether to prevent the current action or not.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Specifies the name of the event.
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// A class that holds options to control the removing and removed item.
    /// </summary>
    public class RemoveEventArgs
    {
        /// <summary>
        /// Specifies a value whether to prevent the current action or not.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Specifies the name of the event.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Returns the removed Tab item index.
        /// </summary>
        public int RemovedIndex { get; set; }
    }

    /// <summary>
    /// A class that holds options to control the dragged item action.
    /// </summary>
    public class DragEventArgs
    {
        /// <summary>
        /// Specify a value whether to prevent the drag action or not.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Returns the index of Tab item.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Returns the index of dragged Tab item.
        /// </summary>
        public TabItem DraggedItem { get; set; }

        /// <summary>
        /// Returns the index of dropped Tab item.
        /// </summary>
        public TabItem DroppedItem { get; set; }

        /// <summary>
        /// Return the Client X value of target element.
        /// </summary>
        public double Left { get; set; }

        /// <summary>
        /// Return the Client Y value of target element.
        /// </summary>
        public double Top { get; set; }
    }

    /// <summary>
    /// A class that holds options to control the selected item action.
    /// </summary>
    public class SelectEventArgs
    {
        /// <summary>
        /// Specify a value whether to prevent the current action or not.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Returns a value whether content selection is done through swiping or not.
        /// </summary>
        public bool IsSwiped { get; set; }

        /// <summary>
        /// Specifies the name of the event.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Returns the index of previous Tab item.
        /// </summary>
        public int PreviousIndex { get; set; }

        /// <summary>
        /// Returns the index of selected Tab item.
        /// </summary>
        public int SelectedIndex { get; set; }
    }

    /// <summary>
    /// A class that holds options to control the selecting item action.
    /// </summary>
    public class SelectingEventArgs
    {
        /// <summary>
        /// Specifies a value whether to prevent the current action or not.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Returns a value whether the content selection is done through swiping or not.
        /// </summary>
        public bool IsSwiped { get; set; }

        /// <summary>
        /// Returns the name of the event.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Returns the index of the previous Tab item.
        /// </summary>
        public int PreviousIndex { get; set; }

        /// <summary>
        /// Returns the index of the selected Tab item.
        /// </summary>
        public int SelectedIndex { get; set; }

        /// <summary>
        /// Returns the index of the selecting Tab item.
        /// </summary>
        public int SelectingIndex { get; set; }
    }

    /// <summary>
    /// A class to define the properties of a tab header.
    /// </summary>
    public class HeaderModel
    {
        /// <summary>
        /// Specifies the icon class that is used to render an icon in the Tab header.
        /// </summary>
        public string IconCss { get; set; } = string.Empty;

        /// <summary>
        /// Options for positioning the icon in the Tab item header. This property depends on the `IconCss` property.
        /// The possible values are:
        /// - left: Places the icon to the `left` of the item.
        /// - top: Places the icon on the `top` of the item.
        /// - right: Places the icon to the `right` end of the item.
        /// - bottom: Places the icon at the `bottom` of the item.
        /// </summary>
        public string IconPosition { get; set; } = "left";

        /// <summary>
        /// Specifies the display text of the Tab header.
        /// </summary>
        public string Text { get; set; } = string.Empty;
    }

    /// <summary>
    /// A class to define the properties of a tab Item.
    /// </summary>
    public class TabItemModel
    {
        /// <summary>
        /// Specifies the content of Tab item.
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Sets the CSS classes to the Tab item to customize its styles.
        /// </summary>
        public string CssClass { get; set; } = string.Empty;

        /// <summary>
        /// Specifies a value that indicates whether the control is disabled or not.
        /// </summary>
        public bool Disabled { get; set; }

        /// <summary>
        /// The object used for configuring the Tab item header properties.
        /// </summary>
        public HeaderModel Header { get; set; }

        /// <summary>
        /// Specifies the header content of the Tab item.
        /// </summary>
        public RenderFragment HeaderTemplate { get; set; }

        /// <summary>
        /// Specifies a value that indicates whether the tab is visible or not.
        /// </summary>
        public bool Visible { get; set; } = true;
    }
}