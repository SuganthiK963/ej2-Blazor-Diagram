using Microsoft.AspNetCore.Components;
using System;
using System.ComponentModel;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Annotation is a user defined HTML element that can be placed on chart
    /// We can use annotations to pile up the visual elegance of the chart.
    /// Specifies the customization of annotation.
    /// </summary>
    public class ChartAnnotation : ChartSubComponent, IChartElement
    {
        #region ANNOTATION COMPONENT BACKING FIELDS
        private Units coordinateUnits;
        private Regions region;
        private object x = "0";
        private string horizontalAxisName;
        private string y = "0";
        private string verticalAxisName;
        private bool isPropertyChanged;
        #endregion

        [CascadingParameter]
        private ChartAnnotations Parent { get; set; }

        /// <summary>
        /// Content Template for the annotation.
        /// </summary>
        [Parameter]
        public RenderFragment ContentTemplate { get; set; }

        /// <summary>
        /// Content of the annotation, which accepts the custom element as a string.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated. Use ContentTemplate to achieve this.")]
        [Parameter]
        public string Content { get; set; }

        /// <summary>
        /// Specifies the coordinate units of the annotation. They are
        ///  Pixel - Annotation renders based on x and y pixel value.
        ///  Point - Annotation renders based on x and y axis value.
        /// </summary>
        [Parameter]
        public Units CoordinateUnits
        {
            get
            {
                return coordinateUnits;
            }

            set
            {
                if (coordinateUnits != value)
                {
                    coordinateUnits = value;
                    isPropertyChanged = Parent != null;
                }
            }
        }

        /// <summary>
        /// Information about annotation for assistive technology.
        /// </summary>
        [Parameter]
        public string Description { get; set; }

        /// <summary>
        /// Specifies the regions of the annotation. They are
        ///  Chart - Annotation renders based on chart coordinates.
        ///  Series - Annotation renders based on series coordinates.
        /// </summary>
        [Parameter]
        public Regions Region
        {
            get
            {
                return region;
            }

            set
            {
                if (region != value)
                {
                    region = value;
                    isPropertyChanged = Parent != null;
                }
            }
        }

        /// <summary>
        /// if set coordinateUnit as `Pixel` X specifies the axis value
        /// else is specifies pixel or percentage of coordinate.
        /// </summary>
        [Parameter]
        public object X
        {
            get
            {
                return x;
            }

            set
            {
                if (x == null || !x.Equals(value))
                {
                    x = value;
                    isPropertyChanged = Parent != null;
                }
            }
        }

        /// <summary>
        /// The name of horizontal axis associated with the annotation.
        /// It requires `Axes` of chart.
        /// </summary>
        [Parameter]
        public string XAxisName
        {
            get
            {
                return horizontalAxisName;
            }

            set
            {
                if (horizontalAxisName != value)
                {
                    horizontalAxisName = value;
                    isPropertyChanged = Parent != null;
                }
            }
        }

        /// <summary>
        /// if set coordinateUnit as `Pixel` Y specifies the axis value
        /// else is specifies pixel or percentage of coordinate.
        /// </summary>
        [Parameter]
        public string Y
        {
            get
            {
                return y;
            }

            set
            {
                if (y != value)
                {
                    y = value;
                    isPropertyChanged = Parent != null;
                }
            }
        }

        /// <summary>
        /// The name of vertical axis associated with the annotation.
        /// It requires `Axes` of chart.
        /// </summary>
        [Parameter]
        public string YAxisName
        {
            get
            {
                return verticalAxisName;
            }

            set
            {
                if (verticalAxisName != value)
                {
                    verticalAxisName = value;
                    isPropertyChanged = Parent != null;
                }
            }
        }

        public string RendererKey { get; set; }

        public Type RendererType { get; set; }

        internal ChartAnnotationRenderer Renderer { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Parent = (ChartAnnotations)Tracker;
            Parent.Chart.AddAnnotation(this);
            RendererType = typeof(ChartAnnotationRenderer);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (isPropertyChanged)
            {
                isPropertyChanged = false;
                Renderer.CalculateRenderingOption();
                Renderer.ProcessRenderQueue();
            }
        }
        internal override void ComponentDispose()
        {
            base.ComponentDispose();
            Parent.Chart.RemoveAnnotation(this);
        }
    }
}