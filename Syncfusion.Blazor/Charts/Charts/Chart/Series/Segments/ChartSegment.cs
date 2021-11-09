using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Defines the collection of regions that helps to differentiate a line Type series.
    /// </summary>
    public class ChartSegment : ChartSubComponent
    {
        #region CHARTSEGMENT PRIVATE FIELDS

        private object values;
        private string color;
        private string dashArray;
        private bool isPropertyChanged;
        #endregion

        #region EMPTYPOINT API

        [CascadingParameter]
        private ChartSeries Series { get; set; }

        [CascadingParameter]
        private ChartSegments Parent { get; set; }

        /// <summary>
        /// Sets and gets the value of the segment series.
        /// </summary>
        [Parameter]
        public object Value
        {
            get
            {
                return values;
            }

            set
            {
                if (values != value)
                {
                    values = value;
                    isPropertyChanged = Series != null;
                }
            }
        }

        /// <summary>
        /// Sets and gets the color of the segment series.
        /// </summary>
        [Parameter]
        public string Color
        {
            get
            {
                return color;
            }

            set
            {
                if (color != value)
                {
                    color = value;
                    isPropertyChanged = Series != null;
                }
            }
        }

        /// <summary>
        /// Sets and gets the dash array of the segment series.
        /// </summary>
        [Parameter]
        public string DashArray
        {
            get
            {
                return dashArray;
            }

            set
            {
                if (dashArray != value)
                {
                    dashArray = value;
                    isPropertyChanged = Series != null;
                }
            }
        }
        #endregion

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Parent = (ChartSegments)Tracker;
            Parent.Segments.Add(this);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (isPropertyChanged)
            {
                isPropertyChanged = false;
                Series.Renderer.UpdateDirection();
                Series.Renderer.ProcessRenderQueue();
            }
        }

        internal override void ComponentDispose()
        {
            Series = null;
            Parent = null;
        }

        internal void SetSegmentValue(object segmentValue, string color)
        {
            Value = segmentValue;
            Color = color;
        }
    }
}
