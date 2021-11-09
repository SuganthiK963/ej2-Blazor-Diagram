using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Buttons
{
    /// <summary>
    /// Specifies the Partial class SfChip.
    /// </summary>
    public partial class SfChip : SfBaseComponent
    {
        /// <summary>
        /// Specifies the ChildContent.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Specifies the custom classes to be added to the chip element used to customize the chip component.
        /// </summary>
        [Parameter]
        public string CssClass { get; set; } = string.Empty;

        /// <summary>
        /// This chips property helps to render chipitem component.
        /// </summary>
        [Parameter]
        public List<ChipItem> Chips { get; set; }

        /// <summary>
        /// This enableDelete property helps to enable delete functionality.
        /// </summary>
        [Parameter]
        public bool EnableDelete { get; set; }

        /// <summary>
        /// Enable or disable rendering component in right to left direction.
        /// </summary>
        [Parameter]
        public bool EnableRtl { get; set; }

        /// <summary>
        /// This enabled property helps to enable/disable chipitem component.
        /// </summary>
        [Parameter]
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// This selectedChips property helps to select chip items.
        /// </summary>
        [Parameter]
        public string[] SelectedChips { get; set; } = Array.Empty<string>();

        /// <summary>
        /// Specifies the callback to trigger when the selected chips changes.
        /// </summary>
        /// <remarks>
        /// You can get the updated SelectedChips value.
        /// </remarks>
        /// <example>
        /// In the below code example, the SelectedChips value can be obtained from the <c>SelectedChipsChanged</c> event.
        /// <code><![CDATA[
        /// <SfChip Selection="SelectionType.Multiple" SelectedChipsChanged="SelectedChipsChanged">
        ///     <ChipItems>
        ///         <ChipItem Text = "Small"></ChipItem>
        ///         <ChipItem Text = "Medium"></ChipItem>
        ///         <ChipItem Text = "Large"></ChipItem>
        ///         <ChipItem Text = "Extra Large"></ChipItem>
        ///     </ChipItems>
        /// </SfChip>
        /// @code {
        ///     private void SelectedChipsChanged(string[] args) {
        ///         string[] selectedChipItem = args;
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<string[]> SelectedChipsChanged { get; set; }

        /// <summary>
        /// This selection property enables chip selection type.
        /// </summary>
        [Parameter]
        public SelectionType Selection { get; set; } = SelectionType.None;

        /// <summary>
        /// You can add the additional html attributes such as title, native events etc., to the wrapper element.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> HtmlAttributes { get; set; }
    }
}