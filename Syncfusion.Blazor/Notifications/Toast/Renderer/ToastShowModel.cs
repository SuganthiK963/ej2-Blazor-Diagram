using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Notifications.Internal
{
    /// <summary>
    /// Provides information about a ToastShowModel.
    /// </summary>
    public partial class ToastShowModel : ToastModel
    {
        /// <summary>
        /// Specifies toast is rendered or not.
        /// </summary>
        public bool IsRendered { get; set; }

        /// <summary>
        /// Specifies the toast ID.
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Provides information about a TitleTemplate.
        /// </summary>
        public RenderFragment TitleTemplate { get; set; }

        /// <summary>
        /// Provides information about a Templates.
        /// </summary>
        public RenderFragment Templates { get; set; }

        /// <summary>
        /// Provides information about a ChildContent.
        /// </summary>
        public RenderFragment ChildContent { get; set; }
    }

    /// <summary>
    /// Provides information about a ToastContentModel.
    /// </summary>
    public partial class ToastContentModel
    {
        /// <summary>
        /// Provides information about a Model.
        /// </summary>
        public ToastShowModel Model { get; set; }

        /// <summary>
        /// Specifies the current toast index.
        /// </summary>
        public int Index { get; set; }
    }

    /// <summary>
    /// Provides information about a ToastProgressBar class.
    /// </summary>
    public class ToastProgressBar
    {
        /// <summary>
        /// Specifies the toast maximum hide time.
        /// </summary>
        public double MaxHideTime { get; set; }

        /// <summary>
        /// Specifies the toast estimated time of arrival.
        /// </summary>
        public double HideEstimatedTimeOfArrival { get; set; }
    }
}