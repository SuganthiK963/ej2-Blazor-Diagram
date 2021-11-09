using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.SplitButtons
{
    /// <summary>
    /// A ButtonGroup is a container that groups a series of buttons on a single line. It has an optional radio button (single selection) and checkbox (multiple selection) behavior.
    /// It may contain DropDownButton or SplitButton component.
    /// </summary>
    public partial class SfButtonGroup : SfBaseComponent
    {
        internal string inputName = SfBaseUtils.GenerateID("SfButtonGroup");
        internal List<ButtonGroupButton> btnItems = new List<ButtonGroupButton>();

        /// <exclude/>
        /// <summary>
        /// Sets content for buttongroup element including HTML and its customizations.
        /// </summary>
        [Parameter]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Defines class/multiple classes separated by a space in the buttongroup container element. The ButtonGroup types and
        /// size customizations can be achieved by using this property.
        /// </summary>
        [Parameter]
        public string CssClass { get; set; }

        /// <exclude/>
        /// <summary>
        /// You can add the additional html attributes such as id, title etc., to the ButtonGroup container element.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Dictionary<string, object> HtmlAttributes { get; set; }

        /// <summary>
        /// Specifies the selection modes. The possible values are,
        ///  `Default`: Default selection in the ButtonGroup.
        ///  `Single`: Allows you to select a single button in the ButtonGroup.
        ///  `Multiple`: Allows you to select more than one button in the ButtonGroup.
        /// </summary>
        [Parameter]
        public SelectionMode Mode { get; set; }

        /// <summary>
        /// Triggers once the component rendering is completed.
        /// </summary>
        [Parameter]
        public EventCallback<object> Created { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                await SfBaseUtils.InvokeEvent<object>(Created, null);
            }
        }

        internal void UpdateChildProperty(ButtonGroupButton Button)
        {
            btnItems.Add(Button);
        }
    }

    /// <summary>
    /// Defines the selection mode of ButtonGroup.
    /// </summary>
    public enum SelectionMode
    {
        /// <summary>
        /// Default selection in the ButtonGroup.
        /// </summary>
        Default,

        /// <summary>
        /// Allows you to select a single button in the ButtonGroup.
        /// </summary>
        Single,

        /// <summary>
        /// Allows you to select more than one button in the ButtonGroup.
        /// </summary>
        Multiple,
    }
}
