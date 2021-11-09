using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.SplitButtons;
using System.Collections.Generic;

namespace Syncfusion.Blazor.Charts.Internal
{
    /// <summary>
    /// Internal Customized dropdown component for stockchart.
    /// </summary>
    public partial class StockChartDropDown : SfBaseComponent
    {
        /// <summary>
        /// Specifies the content.
        /// </summary>
        [Parameter]
        public string Content { get; set; }

        /// <summary>
        /// Specifies dropdowm items.
        /// </summary>
        [Parameter]
        public List<DropDownMenuItem> Items { get; set; }

        /// <summary>
        /// Specifies the item selected event for drop down.
        /// </summary>
        [Parameter]
        public EventCallback<MenuEventArgs> ItemSelected { get; set; }
    }
}