using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Lists
{
    /// <summary>
    /// The class that holds click event arguments.
    /// </summary>
    /// <typeparam name="T">The generic type parameter.</typeparam>
    public class ClickEventArgs<T>
    {
        /// <summary>
        /// It denotes the clicked item dataSource JSON object.
        /// </summary>
        [JsonPropertyName("itemData")]
        public T ItemData { get; set; }

        /// <summary>
        /// It denotes the event name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// It is used to check whether the element is checked or not.
        /// </summary>
        [JsonPropertyName("isChecked")]
        public bool IsChecked { get; set; }

        /// <summary>
        /// It denotes the index position of cliked element.
        /// </summary>
        [JsonPropertyName("index")]
        public int Index { get; set; }

        /// <summary>
        /// Specifies that event has triggered by user interaction.
        /// </summary>
        [JsonPropertyName("isInteracted")]
        public bool IsInteracted { get; set; }

        /// <summary>
        /// It denotes the selected item text.
        /// </summary>
        [JsonPropertyName("text")]
        public string Text { get; set; }

        /// <summary>
        /// It denotes the cancel argument.
        /// </summary>
        [JsonPropertyName("cancel")]
        public bool Cancel { get; set; }

        /// <summary>
        /// It denotes the level of the nested list items.
        /// </summary>
        public int Level { get; set; }
    }

    /// <summary>
    /// The class that holds back event arguments.
    /// </summary>
    /// <typeparam name="T">The generic type parameter.</typeparam>
    public class BackEventArgs<T>
    {
        /// <summary>
        /// Specifies that event has triggered by user interaction.
        /// </summary>
        [JsonPropertyName("isInteracted")]
        public bool IsInteracted { get; set; }

        /// <summary>
        /// It denotes the level of the nested list items.
        /// </summary>
        public int Level { get; set; }
    }

    /// <summary>
    /// The class that holds action event arguments.
    /// </summary>
    public class ActionEventsArgs
    {
        /// <summary>
        /// Return the total number of records.
        /// </summary>
        public double Count { get; set; }

        /// <summary>
        /// Specify the Event Name.
        /// </summary>

        public string Name { get; set; }
    }

    /// <summary>
    /// The class that holds action failure event arguments.
    /// </summary>
    public class ActionFailureEventsArgs : ActionEventsArgs
    {
        /// <summary>
        /// It denotes exception error.
        /// </summary>
        public Exception Error { get; set; }
    }

    /// <summary>
    /// An interface that holds animation settings.
    /// </summary>
    public class AnimationSettings
    {
        /// <summary>
        /// It is used to specify the time duration of transform object.
        /// </summary>
        public double Duration { get; set; }

        /// <summary>
        /// It is used to specify the easing effect applied while transform.
        /// </summary>
        public string Easing { get; set; }

        /// <summary>
        /// It is used to specify the effect which is shown in sub list transform.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ListViewEffect Effect { get; set; }
    }

    /// <summary>
    /// An interface that holds list selected item.
    /// </summary>
    /// <typeparam name="T">The generic type parameter.</typeparam>
    public class SelectedItems<T>
    {
        /// <summary>
        /// Specifies the selected item dataSource collection.
        /// </summary>
        public List<T> Data { get; set; }

        /// <summary>
        /// Specifies index of the selected element.
        /// Available only in virtualization.
        /// </summary>
        public List<int> Index { get; set; }

        /// <summary>
        /// Specifies the hierarchical parent id collection of the current view.
        /// Available only in nested list with checkbox enabled.
        /// </summary>
        public List<string> ParentId { get; set; }

        /// <summary>
        /// Specifies the selected item text collection.
        /// </summary>
        public List<string> Text { get; set; }
    }

    /// <summary>
    /// An class that holds item aria attributes mapping.
    /// </summary>
    public class AriaAttributesMapping
    {
        /// <summary>
        /// Specifies the item aria attributes mapping for GroupItemRole.
        /// </summary>
        public string GroupItemRole { get; set; }

        /// <summary>
        /// Specifies the item aria attributes mapping for ItemRole.
        /// </summary>
        public string ItemRole { get; set; }

        /// <summary>
        /// Specifies the item aria attributes mapping for ItemText.
        /// </summary>
        public string ItemText { get; set; }

        /// <summary>
        /// Specifies the item aria attributes mapping for Level.
        /// </summary>
        public double Level { get; set; } = 1;

        /// <summary>
        /// Specifies the item aria attributes mapping for ListRole.
        /// </summary>
        public string ListRole { get; set; }

        /// <summary>
        /// Specifies the item aria attributes mapping for WrapperRole.
        /// </summary>
        public string WrapperRole { get; set; }
    }

    /// <summary>
    /// common class model for grouped list.
    /// </summary>
    /// <exclude/>
    /// <typeparam name="T">The generic type parameter.</typeparam>
    public class ComposedItemModel<T>
    {
        /// <summary>
        /// It used to check the list item is grouped item or not.
        /// </summary>
        public bool IsGroupItem { get; set; }

        /// <summary>
        /// It used to specify the data.
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// It used to specify the key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// It used to check the group item is header.
        /// </summary>
        public bool IsHeader => IsGroupItem;

        /// <summary>
        /// It used to specify the id.
        /// </summary>
        public string Id => $"group-list-item-{Key}";

        /// <summary>
        /// It used to specify the text.
        /// </summary>
        public string Text => Key;

        /// <summary>
        /// It used to specify the items.
        /// </summary>
        public List<T> Items { get; set; }
    }
}
