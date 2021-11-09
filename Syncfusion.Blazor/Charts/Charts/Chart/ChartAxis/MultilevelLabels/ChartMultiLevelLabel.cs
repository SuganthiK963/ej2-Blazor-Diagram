using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the multilevel label of the axis.
    /// </summary>
    public class ChartMultiLevelLabel : ChartSubComponent
    {
        [CascadingParameter]
        internal ChartMultiLevelLabels MultilevelLabelCollection { get; set; }

        /// <summary>
        /// Defines the position of the multi level labels. They are,
        /// Near: Places the multi level labels at Near.
        /// Center: Places the multi level labels at Center.
        /// Far: Places the multi level labels at Far.
        /// </summary>
        [Parameter]
        public Alignment Alignment { get; set; } = Alignment.Center;

        /// <summary>
        /// Border of the multi level labels.
        /// </summary>
        [Parameter]
        public ChartAxisMultiLevelLabelBorder Border { get; set; } = new ChartAxisMultiLevelLabelBorder();

        /// <summary>
        /// Multi level categories for multi level labels.
        /// </summary>
        [Parameter]
        public List<ChartCategory> Categories { get; set; } = new List<ChartCategory>();

        /// <summary>
        /// Defines the textOverFlow for multi level labels. They are,
        /// Trim: Trim textOverflow for multi level labels.
        /// Wrap: Wrap textOverflow for multi level labels.
        /// None: None textOverflow for multi level labels.
        /// </summary>
        [Parameter]
        public TextOverflow Overflow { get; set; } = TextOverflow.Wrap;

        /// <summary>
        /// Options to customize the multi level labels.
        /// </summary>
        [Parameter]
        public ChartAxisMultiLevelLabelTextStyle TextStyle { get; set; } = new ChartAxisMultiLevelLabelTextStyle();

        protected override void OnInitialized()
        {
            base.OnInitialized();
            MultilevelLabelCollection = (ChartMultiLevelLabels)Tracker;
            MultilevelLabelCollection.MultiLevelLabels.Add(this);
        }

        internal void UpdateMultiLevelLabelProperties(string key, object keyValue)
        {
            if (key == nameof(Border))
            {
                Border = (ChartAxisMultiLevelLabelBorder)keyValue;
            }
            else if (key == nameof(TextStyle))
            {
                TextStyle = (ChartAxisMultiLevelLabelTextStyle)keyValue;
            }
            else if (key == nameof(Categories))
            {
                Categories = (List<ChartCategory>)keyValue;
            }
        }

        internal override void ComponentDispose()
        {
            MultilevelLabelCollection.MultiLevelLabels?.Remove(this);
            MultilevelLabelCollection = null;
            ChildContent = null;
            Categories = null;
            Border = null;
            TextStyle = null;
        }
    }
}