using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Lists.Internal
{
    /// <summary>
    /// Component to create list from primitive data for executing primitive list items common functionalities.
    /// </summary>
    /// <typeparam name="TValue">The generic type parameter.</typeparam>
    public partial class PrimitiveListItem<TValue> : ListItemBase<TValue>
    {
        [CascadingParameter(Name = "ListBase")]
        internal CreateListFromPrimitive<TValue> Parent { get; set; }

        private Dictionary<string, object> HtmlAttributesInternal { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        protected override void OnInitialized()
        {
            Index = ListBase.DataSource.IndexOf(Data);
            Parent?.CallItemCreating(Data);
            HtmlAttributesInternal = GetAttributes();
        }

        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender">Set to true for the first time component rendering; otherwise gets false.</param>
        /// <returns>Task.</returns>
        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Parent?.CallItemCreated(Data);
            }

            return base.OnAfterRenderAsync(firstRender);
        }

        // returns the attributes for the list items
        private Dictionary<string, object> GetAttributes()
        {
            string itemRole = ListBaseOptionModel.AriaAttributes?.ItemRole;
            string id = RandomID + "-" + Index;
            string classNames = ClassNames.Li;
            if (ListBaseOptionModel.ItemClass != null)
            {
                classNames += " " + ListBaseOptionModel.ItemClass;
            }

            Dictionary<string, object> attributes = new Dictionary<string, object>();
            SfBaseUtils.UpdateDictionary("class", classNames, attributes);
            SfBaseUtils.UpdateDictionary("role", itemRole, attributes);
            SfBaseUtils.UpdateDictionary("id", id, attributes);
            if (ListBaseOptionModel.IsSingleLevel)
            {
                SfBaseUtils.UpdateDictionary("data-value", Data.ToString(), attributes);
            }

            return attributes;
        }

        internal override void ComponentDispose()
        {
            ListBaseOptionModel = null;
            ListBase = null;
            Parent = null;
            ClassNames = null;
            HtmlAttributesInternal = null;
        }
    }
}
