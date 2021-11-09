using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// A class that holds options to control the accordion click action.
    /// </summary>
    public class AccordionClickArgs
    {
        /// <summary>
        /// Returns the current Accordion item.
        /// </summary>
        public AccordionItemModel Item { get; set; }

        /// <summary>
        /// Returns the name of the event.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Supplies information about a mouse event that is being raised.
        /// </summary>
        public MouseEventArgs OriginalEvent { get; set; }
    }

    /// <summary>
    /// A class that holds options to control the expanding item action.
    /// </summary>
    public class ExpandEventArgs
    {
        /// <summary>
        /// Specifies a value that indicates to prevent the current action.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Returns the Accordion Item Index.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Returns the expand or collapse state.
        /// </summary>
        public bool IsExpanded { get; set; }

        /// <summary>
        /// Returns the current Accordion Item Object.
        /// </summary>
        public AccordionItemModel Item { get; set; }

        /// <summary>
        /// Returns the name of the event.
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// A class that holds options to control the collapsing item action.
    /// </summary>
    public class CollapseEventArgs : ExpandEventArgs
    {
    }

    /// <summary>
    /// A class that holds options to control the expanded item action.
    /// </summary>
    public class ExpandedEventArgs
    {
        /// <summary>
        /// Returns  the Accordion Item Index.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Returns  the expand or collapse state.
        /// </summary>
        public bool IsExpanded { get; set; }

        /// <summary>
        /// Returns the current Accordion Item Object.
        /// </summary>
        public AccordionItemModel Item { get; set; }

        /// <summary>
        /// Returns the name of the event.
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// A class that holds options to control the collapsed item action.
    /// </summary>
    public class CollapsedEventArgs : ExpandedEventArgs
    {
    }

    /// <summary>
    /// A class that holds option for Accordion Items.
    /// </summary>
    public class AccordionItemModel
    {
        /// <summary>
        /// Sets the header to be displayed for the Accordion item.
        /// </summary>
        public RenderFragment HeaderTemplate { get; set; }

        /// <summary>
        /// Sets the content to be displayed for the Accordion item.
        /// </summary>
        public RenderFragment ContentTemplate { get; set; }

        /// <summary>
        /// Sets the text content to be displayed for the Accordion item.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Defines the single/multiple classes (separated by a space) that should be used for Accordion item customization.
        /// </summary>
        public string CssClass { get; set; }

        /// <summary>
        /// Specifies a value that indicates whether the accordion item is disabled or not.
        /// </summary>
        public bool Disabled { get; set; }

        /// <summary>
        /// Specifies a Boolean value that indicates whether the accordion pane is expanded or not.
        /// </summary>
        public bool Expanded { get; set; }

        /// <summary>
        /// Sets the header text to be displayed for the Accordion item.
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// Defines an icon with the given custom CSS class that is to be rendered before the header text.
        /// Add the css classes to the `IconCss` property and write the css styles to the defined class to set the images/icons.
        /// Adding icon is applicable only to the header.
        /// </summary>
        public string IconCss { get; set; }

        /// <summary>
        /// Specifies a Boolean value that indicates whether the accordion item is visible or not.
        /// </summary>
        public bool Visible { get; set; } = true;

        /// <summary>
        /// Sets Id attribute for accordion item.
        /// </summary>
        public string Id { get; set; }
    }
}