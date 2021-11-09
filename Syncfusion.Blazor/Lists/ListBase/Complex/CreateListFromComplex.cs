using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Lists.Internal
{
    /// <exclude/>
    /// <summary>
    /// Component to create list from complex data for executing complex list items common functionalities.
    /// </summary>
    /// <typeparam name="TValue">The generic type parameter.</typeparam>
    public partial class CreateListFromComplex<TValue> : ListBaseFoundation<TValue>
    {
        private ListBaseOptionModel<TValue> ListBaseOptionsInternal { get; set; }

        internal const string CHECKBOXLISTCLASS = "e-listview-checkbox ";
        internal const string CHECKLIST = "e-checklist";
        internal const string LEFT = "left";
        internal const string RIGHT = "right";
        internal const string CHECKBOX = "e-checkbox";
        internal const string DISPLAY = "none";
        internal const string SPACE = " ";
        private string itemId;
        private string lifeCycle = string.Empty;

        private IDictionary<string, object> ListAttributesInternal { get; set; }

        [CascadingParameter(Name = "ListBase")]
        internal CreateListFromComplex<TValue> Parent { get; set; }

        /// <summary>
        /// Defines the content which has to be passed.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Defines the li element position.
        /// </summary>
        [Parameter]
        public string LiElementPosition { get; set; }

        /// <summary>
        /// Defines the id property.
        /// </summary>
        [Parameter]
        public string ListId { get; set; }

        /// <summary>
        /// Gets or sets ListBase component.
        /// </summary>
        [CascadingParameter(Name = "ListBase")]
        protected ListBaseFoundation<TValue> ListBase { get; set; }

        /// <summary>
        /// Gets or sets MappedData.
        /// </summary>
        [Parameter]
        public FieldsValueMapping<List<TValue>> MappedData { get; set; }

        /// <summary>
        /// Gets or sets IsGroupItem field.
        /// </summary>
        [Parameter]
        public bool ListIsGroupItem { get; set; }

        /// <summary>
        /// Gets or sets Data field.
        /// </summary>
        [Parameter]
        public TValue Data { get; set; }

        /// <summary>
        /// Gets or sets GroupItemData field.
        /// </summary>
        [Parameter]
        public ComposedItemModel<TValue> GroupItemData { get; set; }

        /// <summary>
        /// Gets or sets the item template field.
        /// </summary>
        [Parameter]
        public bool ListIsItemTemplate { get; set; }

        /// <summary>
        /// Gets or sets the group item template field.
        /// </summary>
        //[Parameter]
        public bool ListIsGroupTemplate { get; set; }

        /// <summary>
        /// Gets or sets the random id field.
        /// </summary>
        [Parameter]
        public string RandomID { get; set; }

        /// <summary>
        /// Gets or sets the index field.
        /// </summary>
        [Parameter]
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the class names field.
        /// </summary>
        [Parameter]
        public ClassList ListClassNames { get; set; }

        /// <summary>
        /// Gets or sets the ListBaseOptionModel field.
        /// </summary>
        [Parameter]
        public ListBaseOptionModel<TValue> SfListBaseOptionModel { get; set; }


        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            BaseMapping();
            ListIsGroupItem = false;
            ListIsItemTemplate = IsItemTemplate;
            ListClassNames = ClassNames;
            SfListBaseOptionModel = ListBaseOptionModel;
            ListAttributesInternal = GetAttributes();
            lifeCycle = "OnInitialized";
        }

        /// <summary>
        /// Method invoked when any changes in component state occurs.
        /// </summary>
        /// <returns>Task.</returns>
        protected override Task OnParametersSetAsync()
        {
            BaseMapping();
            if (ListBaseOptionsInternal != null && IsRendered ? ListBaseOptionsInternal != ListBaseOptionModel : true)
            {
                MapSettings(ListBaseOptionsInternal);
            }

            return base.OnParametersSetAsync();
        }

        private void BaseMapping()
        {
            if (ListBaseOptionModel != null && ListBaseOptionsInternal != ListBaseOptionModel)
            {
                ListBaseOptionsInternal = ListBaseOptionModel;
            }
        }

        // call item creating event
        internal void CallItemCreating(TValue item, FieldsValueMapping<List<TValue>> mappedData = null)
        {
            InvokeItemCreate(item, mappedData);
        }

        // call item created event
        internal void CallItemCreated(TValue item, FieldsValueMapping<List<TValue>> mappedData = null)
        {
            InvokeItemCreate(item, mappedData, true);
        }

        // return  Ul element attribute details
        internal Dictionary<string, object> GetListAttributes()
        {
            Dictionary<string, object> htmlAttributes = new Dictionary<string, object>();
            SfBaseUtils.UpdateDictionary("class", ClassNames.Ul, htmlAttributes);
            SfBaseUtils.UpdateDictionary("role", "presentation", htmlAttributes);
            return htmlAttributes;
        }

        /// <summary>
        /// Method invoked when any changes in component state occurs.
        /// </summary>
        protected override void OnParametersSet()
        {
            ListAttributesInternal = GetAttributes();
        }

        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender">Set to true for the first time component rendering; otherwise gets false.</param>
        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                lifeCycle = "OnAfterRender";
            }
        }

        // ItemCreated event invoke method
        private void GroupCallItemCreated(bool itemCreated)
        {
            if (ListIsGroupItem)
            {
                if (!itemCreated)
                {
                    CallItemCreating(Data, null);
                }
                else
                {
                    CallItemCreated(Data, null);
                }
            }
            else
            {
                if (!itemCreated)
                {
                    CallItemCreating(Data, GetMappedData(Data));
                }
                else
                {
                    CallItemCreated(Data, GetMappedData(Data));
                }
            }
        }

        // returns the list item classes for the list
        private string GetListItemClass(FieldsValueMapping<List<TValue>> mappedData)
        {
            bool hasChild = mappedData?.Child != null;
            List<string> classNames = new List<string>();
            classNames.Add(ListIsGroupItem ? ListClassNames.Group : ListClassNames.Li);
            classNames.Add($"{ListClassNames.Level}-{SfListBaseOptionModel.AriaAttributes?.Level}");
            if (hasChild)
            {
                classNames.Add(ListClassNames.HasChild);
            }

            if (MappedData?.Enabled != null && MappedData?.Enabled == false)
            {
                classNames.Add(ListClassNames.Disabled);
            }

            if (!string.IsNullOrEmpty(SfListBaseOptionModel.ItemClass))
            {
                classNames.Add(SfListBaseOptionModel.ItemClass);
            }

            if (MappedData != null && MappedData.HtmlAttributes != null)
            {
                if (MappedData.HtmlAttributes.ContainsKey("Class"))
                {
                    classNames.Add($"{MappedData.HtmlAttributes["Class"]}");
                }
            }

            if (SfListBaseOptionModel.ModuleName == "list" && SfListBaseOptionModel.ShowCheckBox)
            {
                classNames.Add(CHECKLIST);
            }

            return string.Join(SPACE, classNames.ToArray());
        }

        // returns the list item attributes for the list
        private IDictionary<string, object> GetAttributes()
        {
            string itemId = GroupItemData != null ? GroupItemData.Id : MappedData?.Id;
            string id = itemId ?? RandomID + "-" + Index;
            string itemRole = ListIsGroupItem ? SfListBaseOptionModel.AriaAttributes?.GroupItemRole : "option";
            Dictionary<string, object> attributes = new Dictionary<string, object>();
            this.itemId = id;
            SfBaseUtils.UpdateDictionary("class", GetListItemClass(MappedData), attributes);
            SfBaseUtils.UpdateDictionary("role", itemRole, attributes);
            SfBaseUtils.UpdateDictionary("data-uid", id, attributes);
            SfBaseUtils.UpdateDictionary("id", ListId + "_" + id, attributes);
            SfBaseUtils.UpdateDictionary("aria-level", SfListBaseOptionModel.AriaAttributes?.Level.ToString(CultureInfo.CurrentCulture), attributes);
            if (!string.IsNullOrEmpty(GetVisibilityStyle()))
            {
                SfBaseUtils.UpdateDictionary("style", GetVisibilityStyle(), attributes);
            }

            if (MappedData != null && MappedData.HtmlAttributes != null)
            {
                for (int i = 0; i < MappedData.HtmlAttributes.Count; i++)
                {
                    if (MappedData.HtmlAttributes.ElementAt(i).Key != "class" && MappedData.HtmlAttributes.ElementAt(i).Key != "style")
                    {
                        SfBaseUtils.UpdateDictionary(MappedData.HtmlAttributes.ElementAt(i).Key, MappedData.HtmlAttributes.ElementAt(i).Value, MappedData.HtmlAttributes);
                    }
                }
            }

            return attributes;
        }

        // returns the classes for the wrapper element of the generating list
        private string GetWrapperClass()
        {
            List<string> wrapperClasses = new List<string>();
            wrapperClasses.Add(ListClassNames.TextContent);
            if (SfListBaseOptionModel.ExpandCollapse && MappedData.Child != null)
            {
                wrapperClasses.Add(ListClassNames.IconWrapper);
            }

            if (SfListBaseOptionModel.ModuleName == "list" && SfListBaseOptionModel.ShowCheckBox)
            {
                var checkboxLiClass = CheckBoxPosition == CheckBoxPosition.Left ? LEFT : RIGHT;
                wrapperClasses.Add(CHECKBOX + " " + CHECKBOX + "-" + checkboxLiClass);
            }

            return string.Join(SPACE, wrapperClasses.ToArray());
        }

        // returns the display attribute for the list item
        private string GetVisibilityStyle()
        {
            List<string> styleClassLists = new List<string>();
            if (!string.IsNullOrEmpty(LiElementPosition))
            {
                styleClassLists.Add("top:" + LiElementPosition);
            }

            if (MappedData != null)
            {
                if (MappedData.IsVisible != null && MappedData.IsVisible == false)
                {
                    styleClassLists.Add("display:" + DISPLAY);
                }

                if (MappedData.HtmlAttributes != null)
                {
                    Dictionary<string, object> additionalAttribute = MappedData.HtmlAttributes;
                    if (additionalAttribute.ContainsKey("style"))
                    {
                        styleClassLists.Add($"{additionalAttribute["style"]}");
                    }
                }
            }

            return string.Join(";", styleClassLists.ToArray());
        }

        private void GroupCallList()
        {
            if (lifeCycle == "OnAfterRender" || lifeCycle == "OnInitialized")
            {
                if (IsRendered)
                {
                    GroupCallItemCreated(true);
                }
                else
                {
                    GroupCallItemCreated(false);
                }
            }
        }

        internal override void ComponentDispose()
        {
            ListBaseOptionModel = null;
            ListBaseOptionsInternal = null;
            ItemCreated = null;
            ItemCreating = null;
            Template = null;
            GroupTemplate = null;
            DataSource = null;
            ListParent = null;
            ClassNames = null;
        }
    }
}