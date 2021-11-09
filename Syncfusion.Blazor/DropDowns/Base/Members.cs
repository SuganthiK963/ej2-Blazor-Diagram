using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Syncfusion.Blazor.Data;

namespace Syncfusion.Blazor.DropDowns
{
    /// <summary>
    /// The DropDownBase can be created data source and fetch the list of data from data bound component.
    /// </summary>
    /// <typeparam name="T">Specifies the type of SfDropDownBase.</typeparam>
    public partial class SfDropDownBase<T> : SfDataBoundComponent
    {
        /// <summary>
        /// <para>The `Fields` property maps the columns of the data table and binds the data to the component.</para>
        /// <list type="bullet">
        /// <item>
        /// <term>Text</term>
        /// <description>Maps the text column from data table for each list item.</description>
        /// </item>
        /// <item>
        /// <term>Value</term>
        /// <description>Maps the value column from data table for each list item.</description>
        /// </item>
        /// <item>
        /// <term>IconCss</term>
        /// <description>Maps the icon class column from data table for each list item.</description>
        /// </item>
        /// <item>
        /// <term>GroupBy</term>
        /// <description>Group the list items with it's related items by mapping groupBy field.</description>
        /// </item>
        /// </list>
        /// </summary>
        internal FieldSettingsModel Fields { get; set; }

        /// <summary>
        /// <para>Enable or disable persisting component's state between page reloads.</para>
        /// <para>If enabled,  the `Value` state will be persisted.</para>
        /// <list type="bullet">
        /// <item>
        /// <term>Value</term>
        /// </item>
        /// </list>
        /// </summary>
        [Parameter]
        public bool EnablePersistence { get; set; }

        /// <summary>
        /// Accepts the template design and assigns it to each list item present in the popup.
        /// </summary>
        [Parameter]
        public RenderFragment<T> ItemTemplate { get; set; }

        /// <summary>
        /// Accepts the template design and assigns it to the group headers present in the popup list.
        /// </summary>
        [Parameter]
        public RenderFragment<ComposedItemModel<T>> GroupTemplate { get; set; }

        /// <summary>
        /// Accepts the template design and assigns it to popup list of component, when no data is available on the component.
        /// </summary>
        [Parameter]
        public RenderFragment NoRecordsTemplate { get; set; }

        /// <summary>
        /// Accepts the template and assigns it to the popup list content of the component, when the data fetch request from the remote server fails.
        /// </summary>
        [Parameter]
        public RenderFragment ActionFailureTemplate { get; set; }

        /// <summary>
        /// <para>Specifies the `SortOrder` to sort the data source.</para>
        /// <para>The available type of sort orders are.</para>
        /// <list type="bullet">
        /// <item>
        /// <term>None</term>
        /// <description>The data source is not sorting.</description>
        /// </item>
        /// <item>
        /// <term>Ascending</term>
        /// <description>The data source is sorting with ascending order.</description>
        /// </item>
        /// <item>
        /// <term>Descending</term>
        /// <description>The data source is sorting with descending order.</description>
        /// </item>
        /// </list>
        /// </summary>
        [Parameter]
        public SortOrder SortOrder { get; set; }

        private SortOrder sortOrder { get; set; }

        /// <summary>
        /// Specifies a boolean value that indicates whether the component allows the user to interact with it.
        /// </summary>
        [Parameter]
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// <para>Accepts the list items either through local or remote service and binds it to the component.</para>
        /// <para>It can be an array of JSON Objects or an instance of `DataManager`.</para>
        /// </summary>
        [Parameter]
        public IEnumerable<T> DataSource { get; set; }

        private IEnumerable<T> dataSource { get; set; }

        /// <summary>
        /// Accepts the external `Query` that execute along with data processing.
        /// </summary>
        [Parameter]
        public Query Query { get; set; }

        private Query query { get; set; }

        /// <summary>
        /// Determines on which filter type, the component needs to be considered on search action.
        /// </summary>
        [Parameter]
        public virtual FilterType FilterType { get; set; }

        /// <summary>
        /// <para>When set to `false`, consider the `case-sensitive` on performing the search to find suggestions.</para>
        /// <para>By default, consider the casing.</para>
        /// </summary>
        [Parameter]
        public bool IgnoreCase { get; set; } = true;

        /// <summary>
        /// ignoreAccent set to true, then ignores the diacritic characters or accents when filtering.
        /// </summary>
        [Parameter]
        public bool IgnoreAccent { get; set; }

        /// <summary>
        /// specifies the z-index value of the component popup element.
        /// </summary>
        [Parameter]
        public double ZIndex { get; set; } = 1000;

        /// <summary>
        /// Specifies the edit context of dropdown base.
        /// </summary>
        [CascadingParameter]
        protected EditContext DropDownsEditContext { get; set; }
    }
}