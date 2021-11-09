using System.Collections.Generic;
using System.Text.Json;
namespace Syncfusion.Blazor.Diagram
{
    internal class MindMap : SfBaseComponent
    {
        internal static void UpdateLayout(DiagramObjectCollection<Node> nodes, Dictionary<string, IDiagramObject> nameTable, Layout layoutProp, DiagramPoint viewport, string uniqueId = null, string root = null)
        {
            string isRoot = CheckRoot(nodes, layoutProp, uniqueId, root);
            if (!string.IsNullOrEmpty(isRoot))
            {
                layoutProp.UpdateFixedNode(isRoot);
            }
            else
            {
                foreach (Node node in nodes)
                {
                    if (node.canShapeLayout)
                    {
                        if (node.InEdges.Count == 0)
                        {
                            layoutProp.UpdateFixedNode(node.ID);
                            break;
                        }
                    }
                }
            }
            Node rootNode = nameTable[layoutProp.FixedNode] as Node;
            DiagramObjectCollection<Node> firstLevelNodes = FindFirstLevelNodes(rootNode, nameTable);
            DiagramObjectCollection<Node> leftNodes = new DiagramObjectCollection<Node>();
            DiagramObjectCollection<Node> rightNodes = new DiagramObjectCollection<Node>();
            foreach (Node node in firstLevelNodes)
            {
                BranchType? align = layoutProp.GetBranch?.Invoke(node) ?? GetBranch(node, firstLevelNodes);
                if (align == BranchType.Left)
                {
                    leftNodes.Add(node);
                }
                else rightNodes.Add(node);
            }
            DiagramRect viewPortBounds = new DiagramRect(0, 0, viewport.X, viewport.Y);
            ((Node)nameTable[layoutProp.FixedNode]).OffsetX = viewPortBounds.X + viewPortBounds.Width / 2;
            ((Node)nameTable[layoutProp.FixedNode]).OffsetY = viewPortBounds.Y + viewPortBounds.Height / 2;
            if (leftNodes.Count > 0)
            {
                UpdateMindMapBranch(nodes, rightNodes, nameTable, layoutProp, viewport, uniqueId, BranchType.Left);
            }
            if (rightNodes.Count > 0)
            {
                UpdateMindMapBranch(nodes, leftNodes, nameTable, layoutProp, viewport, uniqueId, BranchType.Right);
            }
        }
        internal static MindMap Initialize()
        {
            MindMap hierarchicalTree = new MindMap();
            return hierarchicalTree;
        }
        private static void UpdateMindMapBranch(DiagramObjectCollection<Node> nodes, DiagramObjectCollection<Node> excludeNodes, Dictionary<string, IDiagramObject> nameTable, Layout layoutProp, DiagramPoint viewPort, string uniqueId, BranchType side)
        {
            Layout layout = Layout.GetDefaultMindMapLayout(layoutProp);
            layout.UpdateOrientation(side == BranchType.Left ? LayoutOrientation.LeftToRight : LayoutOrientation.RightToLeft);
            ExcludeFromLayout(excludeNodes, nameTable, false);

            using (HierarchicalTree mapLayout = new HierarchicalTree())
            {
                mapLayout.UpdateLayout(nodes, nameTable, layout, viewPort, uniqueId);
            }
            ExcludeFromLayout(excludeNodes, nameTable, true);
        }
        private static void ExcludeFromLayout(DiagramObjectCollection<Node> newCollection, Dictionary<string, IDiagramObject> nameTable, bool exclude)
        {
            foreach (Node newNode in newCollection)
            {
                if (nameTable[newNode.ID] is Node node) node.canShapeLayout = exclude;
            }
        }
        private static string CheckRoot(DiagramObjectCollection<Node> nodes, Layout layoutProp, string uniqueId, string root)
        {
            foreach (Node node in nodes)
            {
                if (node.canShapeLayout)
                {
                    if (uniqueId != null && node.Data != null && node.Data.GetType().Name != "JsonElement" && (node.Data.GetType().GetProperty(uniqueId)?.GetValue(node.Data).ToString() == root || node.Data.GetType().GetProperty(uniqueId)?.GetValue(node.Data).ToString() == layoutProp.Root))
                    {
                        return node.ID;
                    }
                    else if (node.Data != null && uniqueId != null && node.Data.GetType().Name == "JsonElement")
                    {
                        Dictionary<string, object> data = JsonSerializer.Deserialize<Dictionary<string, object>>(node.Data.ToString());
                        if ((string)data[uniqueId] == root || (string)data[uniqueId] == layoutProp.Root)
                        {
                            return node.ID;
                        }
                    }

                }
            }
            return string.Empty;
        }
        private static BranchType GetBranch(Node obj, DiagramObjectCollection<Node> firstLevelNodes)
        {
            int i = firstLevelNodes.IndexOf(obj);
            BranchType side = i % 2 == 0 ? BranchType.Left : BranchType.Right;
            return side;
        }
        private static DiagramObjectCollection<Node> FindFirstLevelNodes(Node node, Dictionary<string, IDiagramObject> nameTable)
        {
            DiagramObjectCollection<Node> fistLevelNodes = new DiagramObjectCollection<Node>();
            if (node != null && node.OutEdges.Count > 0)
            {
                foreach (string outEdge in node.OutEdges)
                {
                    string targetId = (nameTable[outEdge] as Connector)?.TargetID;
                    if (targetId != null)
                        fistLevelNodes.Add(nameTable[targetId] as Node);
                }
            }
            return fistLevelNodes;
        }
    }
}