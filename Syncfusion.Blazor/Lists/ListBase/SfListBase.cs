using System;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Lists.Internal
{
    /// <exclude/>
    /// <summary>
    /// A list base component for all the Syncfusion Blazor List dependant components to implement the common functionalities.
    /// </summary>
    /// /// <typeparam name="TValue">The generic type parameter.</typeparam>
    public partial class SfListBase<TValue> : ListCommonBase<TValue>
    {
        /// <summary>
        /// ItemCreating event of the listbase which triggers before creating every item of the list.
        /// </summary>
        private Action<ItemCreatedArgs<TValue>> itemCreating = (e) => { };

        /// <summary>
        /// ItemCreating event of the listbase which triggers after every item of the list created.
        /// </summary>
        private Action<ItemCreatedArgs<TValue>> itemCreated = (e) => { };

        /// <exclude/>
        /// <summary>
        /// Update child content from parent component.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        private bool IsPrimitiveTypeInternal { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        protected override void OnInitialized()
        {
            Type type = typeof(TValue);

            // isValueType return true while using decimal value and isPrimitive return true while using int or float type value
            if (type.IsValueType || type.IsPrimitive || type.Name == "String")
            {
                IsPrimitiveTypeInternal = true;
            }
        }

        internal override void ComponentDispose()
        {
            ListBaseOptionModel = null;
            ChildContent = null;
            itemCreating = null;
            ItemCreated = null;
        }
    }
}
