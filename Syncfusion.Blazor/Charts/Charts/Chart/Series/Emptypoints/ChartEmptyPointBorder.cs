using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    public class ChartEmptyPointBorder : ChartDefaultBorder
    {
        internal bool NeedEmptyPointBorderUpdate;

        #region EMPTYPOINTBORDER PRIVATE FIELDS

        private string color = "transparent";
        private double width;

        #endregion

        [CascadingParameter]
        private ChartEmptyPointSettings DynamicParent { get; set; }

        #region EMPTYPOINTBORDER API

        /// <summary>
        /// Specifies the border color of the empty point.
        /// </summary>
        [Parameter]
        public override string Color
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
                    NeedEmptyPointBorderUpdate = true;
                }
            }
        }

        /// <summary>
        /// Specifies the width of the empty point.
        /// </summary>
        [Parameter]
        public override double Width
        {
            get
            {
                return width;
            }

            set
            {
                if (width != value)
                {
                    width = value;
                    NeedEmptyPointBorderUpdate = true;
                }
            }
        }
        #endregion

        protected override void OnInitialized()
        {
            base.OnInitialized();
            DynamicParent = (ChartEmptyPointSettings)Tracker;
            DynamicParent.UpdateEmptyPointProperties("Border", this);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (NeedEmptyPointBorderUpdate)
            {
                NeedEmptyPointBorderUpdate = false;
                DynamicParent.Series.Marker.Renderer?.UpdateDirection();
            }

            DynamicParent.UpdateEmptyPointProperties("Border", this);
        }

        internal override void ComponentDispose()
        {
            DynamicParent = null;
        }
    }
}
