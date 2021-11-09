using System.Collections.Generic;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Lists.Internal
{
    /// <exclude/>
    /// /// <summary>
    /// Component for executing primitive list items common functionalities.
    /// </summary>
    /// <typeparam name="TValue">The generic type parameter.</typeparam>
    public partial class CreateListFromPrimitive<TValue> : ListBaseFoundation<TValue>
    {
        private ListBaseOptionModel<TValue> ListBaseOptionsInternal { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (ListBaseOptionModel != null && ListBaseOptionsInternal != ListBaseOptionModel)
            {
                ListBaseOptionsInternal = ListBaseOptionModel;
            }
        }

        /// <summary>
        /// Method used for get attributes.
        /// </summary>
        /// <returns>return attributes for ul element.</returns>
        internal Dictionary<string, object> GetAttributes()
        {
            Dictionary<string, object> htmlAttributes = new Dictionary<string, object>();
            SfBaseUtils.UpdateDictionary("class", ClassNames.Ul, htmlAttributes);
            SfBaseUtils.UpdateDictionary("role", ListBaseOptionModel.AriaAttributes?.ListRole, htmlAttributes);
            return htmlAttributes;
        }

        // call item creating event
        internal void CallItemCreating(TValue item)
        {
            InvokeItemCreate(item);
        }

        // call Item created event
        internal void CallItemCreated(TValue item)
        {
            InvokeItemCreate(item, null, true);
        }

        internal override void ComponentDispose()
        {
            ListBaseOptionsInternal = null;
            ListBaseOptionModel = null;
            ClassNames = null;
        }
    }
}