using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Syncfusion.Blazor.Diagram.Internal
{
    internal class SpatialSearch
    {
        private double pageLeft;
        private double pageRight;
        private double pageTop;
        private double pageBottom;
        private readonly int quadSize = 500;
        private Dictionary<string, Quad> quadTable = new Dictionary<string, Quad>();
        internal Dictionary<string, IDiagramObject> ObjectTable = new Dictionary<string, IDiagramObject>();
        internal double ChildLeft;
        internal double ChildRight;
        internal double ChildTop;
        internal double ChildBottom;
        internal DiagramContainer ChildNode;
        internal DiagramContainer LeftElement;
        internal DiagramContainer RightElement;
        internal DiagramContainer TopElement;
        internal DiagramContainer BottomElement;
        internal Quad ParentQuad;

        internal SpatialSearch()
        {
            this.pageTop = int.MaxValue;
            this.pageBottom = -int.MaxValue;
            this.pageLeft = int.MaxValue;
            this.pageRight = -int.MaxValue;
            this.ParentQuad = new Quad(0, 0, this.quadSize * 2, this.quadSize * 2, this);
        }
        internal ObservableCollection<Quad> FindQuads(DiagramRect region)
        {
            ObservableCollection<Quad> quads = new ObservableCollection<Quad>();
            this.ParentQuad.FindQuads(region, quads);
            return quads;
        }
        internal ObservableCollection<IDiagramObject> FindObjects(DiagramRect region)
        {
            ObservableCollection<Quad> quads = this.FindQuads(region);
            ObservableCollection<IDiagramObject> objects = new ObservableCollection<IDiagramObject>();
            int i;
            int a;
            for (i = 0; i < quads.Count; i++)
            {
                Quad quad = quads[i];
                for (a = 0; a < quad.Objects.Count; a++)
                {
                    DiagramContainer obj = quad.Objects[a];
                    if (obj.OuterBounds.Intersects(region))
                    {
                        IDiagramObject tempObject = this.ObjectTable[obj.ID];
                        objects.Add(tempObject);
                    }
                }
            }
            return objects;
        }
        internal bool UpdateBounds(DiagramContainer node)
        {
            bool modified = false;
            if (node == this.TopElement)
            {
                this.pageTop = int.MaxValue;
                this.TopElement = null;
                this.FindTop(this.ParentQuad);
                modified = true;
            }
            if (node == this.LeftElement)
            {
                this.pageLeft = int.MaxValue;
                this.LeftElement = null;
                this.FindLeft(this.ParentQuad);
                modified = true;
            }
            if (node == this.RightElement)
            {
                pageRight = -int.MaxValue;
                this.RightElement = null;
                this.FindRight(this.ParentQuad);
                modified = true;
            }
            if (node == this.BottomElement)
            {
                this.pageBottom = -int.MaxValue;
                this.BottomElement = null;
                this.FindBottom(this.ParentQuad);
                modified = true;
            }
            return modified;

        }

        internal void RemoveFromAQuad(DiagramContainer node)
        {
            if (this.quadTable[node.ID] != null)
            {
                Quad quad = this.quadTable[node.ID];
                int index = ObjectIndex(quad.Objects, node);
                if (index != -1)
                {
                    quad.Objects.Remove(quad.Objects[index]);
                    this.Update(quad);
                    this.quadTable.Remove(node.ID);
                }
            }

        }
        internal void Update(Quad quad)
        {
            if (quad.Parent != null && quad.First != null && quad.Second != null && quad.Third != null && quad.Fourth != null && quad.Objects.Count == 0)
            {
                Quad parent = quad.Parent;
                if (parent.First == quad)
                {
                    parent.First = null;
                }
                else if (parent.Second == quad)
                {
                    parent.Second = null;
                }
                else if (parent.Third == quad)
                {
                    parent.Third = null;
                }
                else if (parent.Fourth == quad)
                {
                    parent.Fourth = null;
                }
                this.Update(quad.Parent);
            }
            else
            {
                if (quad == this.ParentQuad && quad.First != null && quad.Second != null && quad.Third != null && quad.Fourth != null)
                {
                    quad.Left = 0;
                    quad.Width = 1000;
                    quad.Top = 0;
                    quad.Height = 1000;
                }
                return;
            }
        }
        private static int ObjectIndex(ObservableCollection<DiagramContainer> objects, DiagramContainer node)
        {
            int i;
            for (i = 0; i < objects.Count; i++)
            {
                if (objects[i].ID == node.ID)
                {
                    return i;
                }
            }
            return -1;
        }

        private void AddIntoAQuad(DiagramContainer node)
        {
            Quad quad = this.ParentQuad.AddIntoAQuad(node);
            this.quadTable.Add(node.ID, quad);
        }
        internal bool UpdateQuad(DiagramContainer node)
        {
            this.SetCurrentNode(node);
            DiagramRect nodeBounds = node.OuterBounds;
            if (!(!double.IsNaN(nodeBounds.X) && !double.IsNaN(nodeBounds.Y) &&
           !double.IsNaN(nodeBounds.Width) && !double.IsNaN(nodeBounds.Height)))
            {
                return false;
            }
            if (this.quadTable.Count > 0 && this.quadTable.ContainsKey(node.ID))
            {
                Quad quad = this.quadTable[node.ID];
                if (!quad.IsContained())
                {
                    this.RemoveFromAQuad(node);
                    this.AddIntoAQuad(node);
                }
            }
            else
            {
                this.AddIntoAQuad(node);
            }
            if (this.IsWithinPageBounds(nodeBounds) &&
        this.LeftElement != node &&
        this.TopElement != node &&
        this.RightElement != node &&
        this.BottomElement != node)
            {

            }
            else
            {
                bool modified = false;
                if (!this.pageLeft.Equals(this.ChildLeft) || node != this.LeftElement)
                {
                    if (this.pageLeft >= this.ChildLeft)
                    {
                        this.pageLeft = this.ChildLeft;
                        this.LeftElement = node;
                        modified = true;
                    }
                    else if (node == this.LeftElement)
                    {
                        this.pageLeft = int.MaxValue;
                        this.FindLeft(this.ParentQuad);
                        modified = true;
                    }
                }
                if (!this.pageTop.Equals(this.ChildTop) || node != this.TopElement)
                {
                    if (this.pageTop >= this.ChildTop)
                    {
                        this.pageTop = this.ChildTop;
                        this.TopElement = node;
                        modified = true;
                    }
                    else if (node == this.TopElement)
                    {
                        this.pageTop = int.MaxValue;
                        this.FindTop(this.ParentQuad);
                        modified = true;
                    }
                }

                if (!this.pageBottom.Equals(this.ChildBottom) || node != this.BottomElement)
                {
                    if (this.pageBottom <= this.ChildBottom)
                    {
                        modified = true;
                        this.pageBottom = this.ChildBottom;
                        this.BottomElement = node;
                    }
                    else if (node == this.BottomElement)
                    {
                        this.pageBottom = -int.MaxValue;
                        this.FindBottom(this.ParentQuad);
                        modified = true;
                    }
                }

                if (!this.pageRight.Equals(this.ChildRight) || node != this.RightElement)
                {
                    if (this.pageRight <= this.ChildRight)
                    {
                        this.pageRight = this.ChildRight;
                        this.RightElement = node;
                        modified = true;
                    }
                    else if (node == this.RightElement)
                    {
                        this.pageRight = -int.MaxValue;
                        this.FindRight(this.ParentQuad);
                        modified = true;
                    }
                }
                return modified;

            }
            this.SetCurrentNode(null);
            return false;

        }
        private void FindRight(Quad quad)
        {
            if (quad.Second != null || quad.Fourth != null)
            {
                if (quad.Second != null)
                {
                    this.FindRight(quad.Second);

                }
                if (quad.Fourth != null)
                {
                    this.FindRight(quad.Fourth);
                }
            }
            else
            {
                if (quad.First != null)
                {
                    this.FindRight(quad.First);

                }
                if (quad.Third != null)
                {
                    this.FindRight(quad.Third);
                }
            }

            int i;
            for (i = 0; i < quad.Objects.Count; i++)
            {
                DiagramContainer node = quad.Objects[i];
                if (node != null && node.OuterBounds != null && this.pageRight <= node.OuterBounds.Right)
                {
                    this.pageRight = node.OuterBounds.Right;
                    this.RightElement = node;
                }
            }
        }
        private void FindBottom(Quad quad)
        {
            if (quad.Third != null || quad.Fourth != null)
            {
                if (quad.Third != null)
                {
                    this.FindBottom(quad.Third);
                }
                if (quad.Fourth != null)
                {
                    this.FindBottom(quad.Fourth);
                }
            }
            else
            {
                if (quad.Second != null)
                {
                    this.FindBottom(quad.Second);

                }
                if (quad.First != null)
                {
                    this.FindBottom(quad.First);
                }
            }
            int i;
            for (i = 0; i < quad.Objects.Count; i++)
            {
                DiagramContainer node = quad.Objects[i];
                if (node != null && node.OuterBounds != null && this.pageBottom <= node.OuterBounds.Bottom)
                {
                    this.pageBottom = node.OuterBounds.Bottom;
                    this.BottomElement = node;
                }
            }
        }
        private void FindTop(Quad quad)
        {
            if (quad.First != null || quad.Second != null)
            {
                if (quad.First != null)
                {
                    this.FindTop(quad.First);
                }
                if (quad.Second != null)
                {
                    this.FindTop(quad.Second);
                }
            }
            else
            {
                if (quad.Third != null)
                {
                    this.FindTop(quad.Third);
                }
                if (quad.Fourth != null)
                {
                    this.FindTop(quad.Fourth);
                }
            }
            int i;
            for (i = 0; i < quad.Objects.Count; i++)
            {
                DiagramContainer node = quad.Objects[i];
                if (node != null && node.OuterBounds != null && this.pageTop >= node.OuterBounds.Top)
                {
                    this.pageTop = node.OuterBounds.Top;
                    this.TopElement = node;
                }
            }
        }
        private void FindLeft(Quad quad)
        {
            if (quad.First != null || quad.Third != null)
            {
                if (quad.First != null)
                {
                    this.FindLeft(quad.First);
                }
                if (quad.Third != null)
                {
                    this.FindLeft(quad.Third);
                }
            }
            else
            {
                if (quad.Second != null)
                {
                    this.FindLeft(quad.Second);
                }
                if (quad.Fourth != null)
                {
                    this.FindLeft(quad.Fourth);
                }
            }
            int i;
            for (i = 0; i < quad.Objects.Count; i++)
            {
                DiagramContainer node = quad.Objects[i];
                if (node != null && node.OuterBounds != null && this.pageLeft >= node.OuterBounds.Left)
                {
                    this.pageLeft = node.OuterBounds.Left;
                    this.LeftElement = node;
                }
            }
        }
        private bool IsWithinPageBounds(DiagramRect node)
        {
            if (node.Left >= this.pageLeft && node.Right <= this.pageRight && node.Top >= this.pageTop
            && node.Bottom <= this.pageBottom)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal void SetCurrentNode(DiagramContainer node)
        {
            this.ChildNode = node;
            if (node != null)
            {
                DiagramRect outerBounds = node.OuterBounds;
                this.ChildLeft = outerBounds.Left;
                this.ChildRight = outerBounds.Right;
                this.ChildTop = outerBounds.Top;
                this.ChildBottom = outerBounds.Bottom;
            }
            else
            {
                this.ChildLeft = int.MaxValue;
                this.ChildRight = int.MaxValue;
                this.ChildTop = int.MaxValue;
                this.ChildBottom = int.MaxValue;
            }
        }

        internal DiagramRect GetPageBounds(double? originX = null, double? originY = null)
        {
            if (this.pageLeft.Equals(int.MaxValue))
            {
                return new DiagramRect(0, 0, 0, 0);
            }
            double left = originX.HasValue ? Math.Min(pageLeft, 0) : pageLeft;
            double top = originY.HasValue ? Math.Min(pageTop, 0) : pageTop;
            return new DiagramRect(
                Math.Round(left), Math.Round(top), Math.Round(pageRight - left), Math.Round(pageBottom - top));
        }

        internal void Dispose()
        {
            if (ObjectTable != null)
            {
                ObjectTable.Clear();
                ObjectTable = null;
            }

            if (quadTable != null)
            {
                quadTable.Clear();
                quadTable = null;
            }

            if (ChildNode != null)
            {
                ChildNode.Dispose();
                ChildNode = null;
            }

            if (LeftElement != null)
            {
                LeftElement.Dispose();
                LeftElement = null;
            }

            if (RightElement != null)
            {
                RightElement.Dispose();
                RightElement = null;
            }

            if (TopElement != null)
            {
                TopElement.Dispose();
                TopElement = null;
            }

            if (BottomElement != null)
            {
                BottomElement.Dispose();
                BottomElement = null;
            }

            if (ParentQuad != null)
            {
                ParentQuad = null;
            }

        }
    }
}
