using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.DropDowns
{
    /// <summary>
    /// ListBox component used to display a list of items. Users can select one or more items in the list using a checkbox or by keyboard selection.
    /// It supports sorting, grouping, reordering and drag and drop of items.
    /// </summary>
    public partial class SfListBox<TValue, TItem> : SfDropDownBase<TItem>
    {
        /// <summary>
        /// Specifies the content that has to be passed.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Specifies the expression for defining the value of the bound.
        /// </summary>
        [Parameter]
        public Expression<Func<TValue>> ValueExpression { get; set; }

        /// <summary>
        /// If 'allowDragAndDrop' is set to true, then you can perform drag and drop of the list item.
        /// ListBox contains same 'scope' property enables drag and drop between multiple ListBox.
        /// </summary>
        [Parameter]
        public bool AllowDragAndDrop { get; set; }

        /// <summary>
        /// To enable the filtering option in this component.
        /// Filter action performs when type in search box and collect the matched item through `Filtering` event.
        /// If searching character does not match, `noRecordsTemplate` property value will be shown.
        /// </summary>
        [Parameter]
        public bool AllowFiltering { get; set; }

        /// <summary>
        /// Sets the CSS classes to root element of this component, which helps to customize the
        /// complete styles.
        /// </summary>
        [Parameter]
        public string CssClass { get; set; } = string.Empty;

        /// <summary>
        /// Enable or disable rendering component in right to left direction.
        /// </summary>
        [Parameter]
        public bool EnableRtl { get; set; }

        /// <summary>
        /// Accepts the value to be displayed as a watermark text on the filter bar.
        /// </summary>
        [Parameter]
        public string FilterBarPlaceholder { get; set; }

        /// <summary>
        /// Sets the height of the ListBox component.
        /// </summary>
        [Parameter]
        public string Height { get; set; } = string.Empty;

        /// <summary>
        /// Sets limitation to the value selection.
        /// based on the limitation, list selection will be prevented.
        /// </summary>
        [Parameter]
        public double MaximumSelectionLength { get; set; } = 500;

        /// <summary>
        /// Defines the connected ListBox reference to group sets of draggable and droppable ListBox.
        /// A draggable ListBox with the scope reference will be accepted by the droppable.
        /// </summary>
        [Parameter]
        public string Scope { get; set; }

        /// <summary>
        /// Sets the specified item to the selected state or gets the selected item in the ListBox.
        /// </summary>
        [Parameter]
        public TValue Value { get; set; }

        /// <summary>
        /// Specifies the callback to trigger when the value changes.
        /// </summary>
        [Parameter]
        public EventCallback<TValue> ValueChanged { get; set; }

        /// <summary>
        /// You can add the additional HTML attributes such as id, title etc., to the listbox element.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Dictionary<string, object> HtmlAttributes
        {
            get { return htmlAttributes; }
            set { htmlAttributes = value; }
        }
    }
}