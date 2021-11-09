using System;
using System.Collections.Generic;
using System.Text;

namespace Syncfusion.Blazor.Diagram
{
    /// <summary> 
    /// Represents the common configuration settings for printing  and exporting the diagram.  
    /// </summary> 
    public interface IDiagramPrintExportSettings
    {
        /// <summary> 
        /// Gets or sets the height of the page to be printed. 
        /// </summary> 
        /// <value>The height of the page to be printed. The default value will be <seealso cref="PageSettings.Height"/>.</value> 
        double PageHeight { get; set; }

        /// <summary> 
        /// Gets or sets the width of the page to be printed. 
        /// </summary> 
        /// <value>The width of the page to be printed. The default value will be <seealso cref="PageSettings.Width"/>.</value> 
        double PageWidth { get; set; }

        /// <summary> 
        /// Gets or sets the margin of the page to be printed/exported. 
        /// </summary> 
        /// <value>Space around the content to be printed/exported.The default value for margin is 25 for all sides.</value> 
        Margin Margin { get; set; }

        /// <summary> 
        /// Gets or sets the orientation of the page to be printed. 
        /// </summary> 
        /// <value>One of the <see cref="PageOrientation"/> enumeration values. The default value is <seealso cref="PageOrientation.Landscape"/>. </value> 
        PageOrientation Orientation { get; set; }

        /// <summary> 
        /// Gets or sets the value to indicate whether to print or export the entire diagram to single page or multiple pages. 
        /// </summary> 
        /// <value> true, if the diagram printed or exported to single page. Otherwise, diagram printed or exported into multiple pages or images. The default value is false. </value> 
        bool FitToPage { get; set; }

        /// <summary> 
        /// Gets or sets the region in the diagram which need to be printed or exported. 
        /// </summary> 
        /// <value>One of the <see cref="DiagramPrintExportRegion"/> enumeration values. The default value is <seealso cref="DiagramPrintExportRegion.PageSettings"/></value>
        /// <remarks> 
        /// * PageSettings - The region within the x,y, width and height values of page settings is printed or exported. 
        /// * Content – Content of the diagram without empty space around the content is printed or exported. 
        /// * ClipBounds - The region specified using <see cref="DiagramPrintSettings.ClipBounds"/> property is exported. This is applicable for exporting only. 
        /// </remarks> 
        DiagramPrintExportRegion Region { get; set; }

        /// <summary> 
        /// Gets or sets the region that to be exported when <see cref="Region"/> is <see cref="DiagramPrintExportRegion.ClipBounds"/>. 
        /// </summary> 
        /// <value>The bounds to be used for clipping area sizing. The default is a <seealso cref="DiagramRect"/>(0,0,0,0)</value> 
        DiagramRect ClipBounds { get; set; }
    }

    /// <summary> 
    /// Represents the configuration settings for printing the diagram. 
    /// </summary> 
    public class DiagramPrintSettings : IDiagramPrintExportSettings
    {
        private double _pageHeight;
        private double _pageWidth;
        private Margin _margin = new Margin() { Left = 25, Top = 25, Right = 25, Bottom = 25 };
        private PageOrientation _pageOrientation = PageOrientation.Landscape;
        private bool _fitToPage;
        private DiagramPrintExportRegion _region = DiagramPrintExportRegion.PageSettings;
        private DiagramRect _clipbounds;
        /// <summary> 
        /// Gets or sets the height of the page to be printed. 
        /// </summary> 
        /// <value>The height of the page to be printed. The default value will be <seealso cref="PageSettings.Height"/>.</value> 
        public double PageHeight
        {
            get
            {
                return _pageHeight;
            }
            set
            {
                _pageHeight = value;
            }
        }
        /// <summary> 
        /// Gets or sets the width of the page to be printed. 
        /// </summary> 
        /// <value>The width of the page to be printed. The default value will be <seealso cref="PageSettings.Width"/>.</value> 
        public double PageWidth
        {
            get
            {
                return _pageWidth;
            }
            set
            {
                _pageWidth = value;
            }
        }
        /// <summary> 
        /// Gets or sets the margin of the page to be printed/exported. 
        /// </summary> 
        /// <value>Space around the content to be printed/exported.The default value for margin is 25 for all sides.</value> 
        public Margin Margin
        {
            get
            {
                return _margin;
            }
            set
            {
                _margin = value;
            }
        }
        /// <summary> 
        /// Gets or sets the orientation of the page to be printed. 
        /// </summary> 
        /// <value>One of the <see cref="PageOrientation"/> enumeration values. The default value is <seealso cref="PageOrientation.Landscape"/>. </value> 
        public PageOrientation Orientation
        {
            get
            {
                return _pageOrientation;
            }
            set
            {
                _pageOrientation = value;
            }
        }
        /// <summary> 
        /// Gets or sets the value to indicate whether to print or export the entire diagram to single page or multiple pages. 
        /// </summary> 
        /// <value> true, if the diagram printed or exported to single page. Otherwise, diagram printed or exported into multiple pages or images. The default value is false. </value> 
        public bool FitToPage
        {
            get
            {
                return _fitToPage;
            }
            set
            {
                _fitToPage = value;
            }
        }

        /// <summary> 
        /// Gets or sets the region in the diagram which need to be printed or exported. 
        ///</summary> 
        /// <value>One of the <see cref="DiagramPrintExportRegion"/> enumeration values. The default value is <seealso cref="DiagramPrintExportRegion.PageSettings"/> </value>
        ///<remarks> 
        /// * PageSettings - The region within the x,y, width and height values of page settings is printed or exported. 
        /// * Content – Content of the diagram without empty space around the content is printed or exported. 
        /// * ClipBounds - The region specified using <see cref="DiagramPrintSettings.ClipBounds"/> property is exported. This is applicable for exporting only. 
        /// </remarks> 
        public DiagramPrintExportRegion Region
        {
            get
            {
                return _region;
            }
            set
            {
                _region = value;
            }
        }
        /// <summary> 
        /// Gets or sets the region that to be exported when <see cref="DiagramPrintSettings.Region"/> is <see cref="DiagramPrintExportRegion.ClipBounds"/>. 
        /// </summary> 
        /// <value>The bounds to be used for clipping area sizing. The default is a <seealso cref="DiagramRect"/>(0,0,0,0)</value> 
        public DiagramRect ClipBounds
        {
            get
            {
                return _clipbounds;
            }
            set
            {
                _clipbounds = value;
            }
        }
    }

    /// <summary> 
    /// Represents the configuration settings for exporting the diagram. 
    /// </summary> 
    public class DiagramExportSettings : IDiagramPrintExportSettings
    {
        private double _pageHeight;
        private double _pageWidth;
        private Margin _margin = new Margin() { Left = 25, Top = 25, Right = 25, Bottom = 25 };
        private PageOrientation _pageOrientation = PageOrientation.Landscape;
        private bool _fitToPage;
        private DiagramPrintExportRegion _region = DiagramPrintExportRegion.PageSettings;
        private DiagramRect _clipbounds;
        /// <summary> 
        /// Gets or sets the height of the page to be printed. 
        /// </summary> 
        /// <value>The height of the page to be printed. The default value will be <seealso cref="PageSettings.Height"/>.</value> 
        public double PageHeight
        {
            get
            {
                return _pageHeight;
            }
            set
            {
                _pageHeight = value;
            }
        }
        /// <summary> 
        /// Gets or sets the width of the page to be printed. 
        /// </summary> 
        /// <value>The width of the page to be printed. The default value will be <seealso cref="PageSettings.Width"/>.</value> 
        public double PageWidth
        {
            get
            {
                return _pageWidth;
            }
            set
            {
                _pageWidth = value;
            }
        }
        /// <summary> 
        /// Gets or sets the margin of the page to be printed/exported. 
        /// </summary> 
        /// <value>Space around the content to be printed/exported.The default value for margin is 25 for all sides.</value> 
        public Margin Margin
        {
            get
            {
                return _margin;
            }
            set
            {
                _margin = value;
            }
        }
        /// <summary> 
        /// Gets or sets the orientation of the page to be printed. 
        /// </summary> 
        /// <value>One of the <see cref="PageOrientation"/> enumeration values. The default value is <seealso cref="PageOrientation.Landscape"/>. </value> 
        public PageOrientation Orientation
        {
            get
            {
                return _pageOrientation;
            }
            set
            {
                _pageOrientation = value;
            }
        }
        /// <summary> 
        /// Gets or sets the value to indicate whether to print or export the entire diagram to single page or multiple pages. 
        /// </summary> 
        /// <value> true, if the diagram printed or exported to single page. Otherwise, diagram printed or exported into multiple pages or images. The default value is false. </value> 
        public bool FitToPage
        {
            get
            {
                return _fitToPage;
            }
            set
            {
                _fitToPage = value;
            }
        }

        /// <summary> 
        /// Gets or sets the region in the diagram which need to be printed or exported. 
        /// </summary> 
        /// <value>One of the <see cref="DiagramPrintExportRegion"/> enumeration values. The default value is <see cref="DiagramPrintExportRegion.PageSettings"/></value>
        /// <remarks> 
        /// * PageSettings - The region within the x,y, width and height values of page settings is printed or exported. 
        /// * Content – Content of the diagram without empty space around the content is printed or exported. 
        /// * ClipBounds - The region specified using <see cref="DiagramExportSettings.ClipBounds"/> property is exported. This is applicable for exporting only. 
        /// </remarks> 
        public DiagramPrintExportRegion Region
        {
            get
            {
                return _region;
            }
            set
            {
                _region = value;
            }
        }
        /// <summary> 
        /// Gets or sets the region that to be exported when <see cref="DiagramExportSettings.Region"/> is <see cref="DiagramPrintExportRegion.ClipBounds"/>. 
        /// </summary> 
        /// <value>The bounds to be used for clipping area sizing. The default is a <seealso cref="DiagramRect"/>(0,0,0,0)</value> 
        public DiagramRect ClipBounds
        {
            get
            {
                return _clipbounds;
            }
            set
            {
                _clipbounds = value;
            }
        }
    }
}
