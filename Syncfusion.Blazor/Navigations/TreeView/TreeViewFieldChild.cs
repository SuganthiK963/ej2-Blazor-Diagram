using System;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Navigations.Internal;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    ///  A class used for configuring the TreeView child element fields properties.
    /// </summary>
    /// <typeparam name="TValue">"Specifies the Tvalue paramater".</typeparam>
    public partial class TreeViewFieldChild<TValue> : TreeViewCommonFieldsSettings<TValue>
    {
        [CascadingParameter]
        private TreeViewFieldsSettings<TValue> Parent { get; set; }

         /// <summary>
        /// Specifies the Treeview child content.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>"Task".</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateChildProperties("child", this);
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
        }
    }
}