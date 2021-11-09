using System;
using System.Collections.Generic;

namespace Syncfusion.Blazor.Diagram.Internal
{
    internal class ILayout
    {
        internal double AnchorX;
        internal double AnchorY;
        internal int MaxLevel;
        internal Dictionary<string, IDiagramObject> NameTable { get; set; } = new Dictionary<string, IDiagramObject>();
        internal LayoutType Type;
        internal Node RootNode;
        internal SfDiagramComponent Parent { get; set; }
        internal DiagramObjectCollection<Node> FirstLevelNodes { get; set; } = new DiagramObjectCollection<Node>();
        internal LayoutOrientation Orientation;
        internal Dictionary<string, LayoutInfo> GraphNodes { get; set; } = new Dictionary<string, LayoutInfo>();
        internal List<LevelBounds> Levels { get; set; } = new List<LevelBounds>();
        internal int VerticalSpacing;
        internal int HorizontalSpacing;
        internal HorizontalAlignment HorizontalAlignment;
        internal VerticalAlignment VerticalAlignment;
        internal TreeInfo LayoutInfo;
        internal int Level;
        internal List<INode> Objects { get; set; } = new List<INode>();
        internal LayoutMargin LayoutMargin { get; set; } = new LayoutMargin();
        internal Func<IDiagramObject, TreeInfo, TreeInfo> GetLayoutInfo { get; set; }
        internal Func<IDiagramObject, BranchType> GetBranch { get; set; }
        internal DiagramRect Bounds;
        internal string FixedNode;
        internal string Root;
    }
    internal class Bounds
    {
        internal double X;
        internal double Y;
        internal double Right;
        internal double Bottom;
        internal int CanMoveBy;
    }

    internal class LevelBounds
    {
        internal Bounds RBounds;
    }
    internal class INode : Node
    {
        internal int DifferenceX;
        internal int DifferenceY;
    }
    /// <summary>
    /// Represents the necessary information about a node's children and the way to arrange them.
    /// </summary>
    internal class LayoutInfo
    {
        /// <summary>
        /// Specifies the collection of subtree alignments in a layout. 
        /// </summary>
        public SubTreeAlignmentType Type { get; set; }
        /// <summary>
        /// Arranges the child nodes with the parent based on its type.
        /// </summary>
        public Orientation Orientation { get; set; }
        /// <summary>
        /// Specifies the position of the node to be arranged. 
        /// </summary>
        public double Offset { get; set; }
        /// <summary>
        /// Specifies whether the layout contains a subtree or not.
        /// </summary>
        public bool HasSubTree { get; set; }
        /// <summary>
        /// Represents the value to be moved in the layout.
        /// </summary>
        public int SubTreeTranslation { get; set; }
        /// <summary>
        /// Represents the arrangement of the nodes in the layout.
        /// </summary>
        public TreeInfo Tree { get; set; } = new TreeInfo();
        /// <summary>
        /// Represents the breadth space for assistants.
        /// </summary>
        public int? CanMoveBy { get; set; }
        /// <summary>
        /// Represents the same positions to be arranged for the leaf in the tree. 
        /// </summary>
        public bool Translate { get; set; }
        /// <summary>
        /// Represents the value of the X coordinate in a layout .
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// Represents the value of the Y coordinate in a layout. 
        /// </summary>
        public double Y { get; set; }
        /// <summary>
        /// Represents the collection of values where the subtree has been positioned.
        /// </summary>
        public List<int> Intersect { get; set; }
        /// <summary>
        /// Represents the mid value of the layout in the diagram.
        /// </summary>
        public double? Mid { get; set; }
        /// <summary>
        /// Represents the highest number of levels in the layout. 
        /// </summary>
        public int MaxLevel { get; set; }
        /// <summary>
        /// Represents whether the translate has been completed or not.
        /// </summary>
        public bool Translated { get; set; }
        /// <summary>
        /// Represents the space to be left from the previous sub tree. 
        /// </summary>
        public double? Diff { get; set; }
        internal FirstChildInfo FirstChild { get; set; } = new FirstChildInfo();
        internal double TreeWidth { get; set; }
        internal Bounds TreeBounds { get; set; }
        internal List<Bounds> LevelBounds { get; set; }
        internal Bounds PrevBounds { get; set; }
        /// <summary>
        /// Represents the exact level of the tree in the layout. 
        /// </summary>
        public int? ActualLevel { get; set; }
        internal Bounds ChildBounds { get; set; }
    }
    internal class FirstChildInfo
    {
        internal double X;
        internal string Child;
        internal int? CanMoveBy;
        internal string ID;
    }
    internal class TranslateInfo
    {
        internal ILayout Layout;
        internal Node Shape;
        internal Bounds ShapeBounds;
        internal Bounds TreeBounds;
        internal DiagramRect Dim;
        internal int Level;
    }
    /// <summary>
    /// Defines the properties of the connector
    /// </summary>
    internal class IConnector : Connector
    {
        internal string id;
        internal string sourceID;
        internal string targetID;
        internal bool visible;
        internal List<DiagramPoint> points;
        internal ConnectorSegmentType type;
    }
}