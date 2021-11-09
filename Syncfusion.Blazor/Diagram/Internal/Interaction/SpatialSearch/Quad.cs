using System.Collections.ObjectModel;

namespace Syncfusion.Blazor.Diagram.Internal
{
    internal class Quad
    {
        private SpatialSearch spatialSearch;
        internal double Left;
        internal double Top;
        internal ObservableCollection<DiagramContainer> Objects = new ObservableCollection<DiagramContainer>();
        internal Quad Parent;
        internal Quad First;
        internal Quad Second;
        internal Quad Third;
        internal Quad Fourth;
        internal double Width;
        internal double Height;

        internal Quad(double left, double top, double width, double height, SpatialSearch spatialSearch)
        {
            this.Left = left;
            this.Top = top;
            this.Width = width;
            this.Height = height;
            this.spatialSearch = spatialSearch;
        }
        internal Quad AddIntoAQuad(DiagramContainer node)
        {
            QuadAddition quadAddition = new QuadAddition();
            this.spatialSearch.SetCurrentNode(node);
            Quad quad = null;
            while (!quadAddition.IsAdded)
            {
                quadAddition = this.spatialSearch.ParentQuad.Add();
                quad = quadAddition.Quad;
            }
            return quad;
        }
        private QuadAddition Add()
        {
            Quad quad = null;
            QuadAddition quadAddition = new QuadAddition();
            if (this.IsContained())
            {
                quad = this.SelectQuad();
                quadAddition.IsAdded = true;
                quadAddition.Quad = quad;
                return quadAddition;
            }
            else
            {
                Quad newParent;
                bool isEmpty = this.Objects.Count == 0 && this.First == null && this.Second == null && this.Third == null && this.Fourth == null;
                double newWidth = this.Width * 2;
                double newHeight = this.Height * 2;
                if (this.spatialSearch.ChildLeft < this.Left)
                {
                    if (this.spatialSearch.ChildTop < this.Top)
                    {
                        newParent = new Quad(this.Left - this.Width, this.Top - this.Height, newWidth, newHeight, this.spatialSearch);
                        if (!isEmpty)
                        {
                            newParent.Fourth = this;
                        }
                    }
                    else
                    {
                        newParent = new Quad(this.Left - this.Width, this.Top, newWidth, newHeight, this.spatialSearch);
                        if (!isEmpty)
                        {
                            newParent.Second = this;
                        }
                    }
                }
                else if (this.spatialSearch.ChildTop < this.Top)
                {
                    newParent = new Quad(this.Left, this.Top - this.Height, newWidth, newHeight, this.spatialSearch);
                    if (!isEmpty)
                    {
                        newParent.Third = this;
                    }
                }
                else
                {
                    newParent = new Quad(this.Left, this.Top, newWidth, newHeight, this.spatialSearch);
                    if (!isEmpty)
                    {
                        newParent.First = this;
                    }
                }
                this.Parent = newParent;
                this.spatialSearch.ParentQuad = newParent;
                quadAddition.IsAdded = false;
                quadAddition.Quad = quad;
                return quadAddition;
            }
        }
        private Quad SelectQuad()
        {
            Quad target = null;
            Quad current = this;
            while (current != null)
            {
                QuadSet quadSet = current.GetQuad();
                current = quadSet.Source;
                target = quadSet.Target ?? this;
            }
            return target;
        }
        private QuadSet GetQuad()
        {
            QuadSet quadSet = new QuadSet();
            double halfWidth = this.Width / 2;
            double halfHeight = this.Height / 2;
            if (halfWidth >= 1000 && halfHeight >= 1000)
            {
                double xCenter = this.Left + halfWidth;
                double yCenter = this.Top + halfHeight;
                if (this.spatialSearch.ChildRight <= xCenter)
                {
                    if (this.spatialSearch.ChildBottom <= yCenter)
                    {
                        if (this.First == null)
                        {
                            Quad newQuad = new Quad(this.Left, this.Top, halfWidth, halfHeight, this.spatialSearch)
                            {
                                Parent = this
                            };
                            this.First = newQuad;
                        }
                        quadSet.Source = this.First;
                        return quadSet;
                    }
                    if (this.spatialSearch.ChildTop >= yCenter)
                    {
                        if (this.Third == null)
                        {
                            Quad newQuad = new Quad(this.Left, yCenter, halfWidth, halfHeight, this.spatialSearch)
                            {
                                Parent = this
                            };
                            this.Third = newQuad;
                        }
                        quadSet.Source = this.Third;
                        return quadSet;
                    }

                }
                else if (this.spatialSearch.ChildLeft >= xCenter)
                {
                    if (this.spatialSearch.ChildBottom <= yCenter)
                    {
                        if (this.Second == null)
                        {
                            Quad newQuad = new Quad(xCenter, this.Top, halfWidth, halfHeight, this.spatialSearch)
                            {
                                Parent = this
                            };
                            this.Second = newQuad;
                        }
                        quadSet.Source = this.Second;
                        return quadSet;
                    }
                    if (this.spatialSearch.ChildTop >= yCenter)
                    {
                        if (this.Fourth == null)
                        {
                            Quad newQuad = new Quad(xCenter, yCenter, halfWidth, halfHeight, this.spatialSearch)
                            {
                                Parent = this
                            };
                            this.Fourth = newQuad;
                        }
                        quadSet.Source = this.Fourth;
                        return quadSet;

                    }
                }

            }
            this.Objects.Add(this.spatialSearch.ChildNode);
            quadSet.Target = this;
            return quadSet;

        }
        internal bool IsContained()
        {
            if (spatialSearch.ChildLeft >= this.Left && spatialSearch.ChildRight <= this.Left + this.Width &&
            spatialSearch.ChildTop >= this.Top && spatialSearch.ChildBottom <= this.Top + this.Height)
            {
                return true;
            }
            return false;

        }
        internal void FindQuads(DiagramRect currentViewPort, ObservableCollection<Quad> quads)
        {
            if (this.First != null && this.First.IsIntersect(currentViewPort))
            {
                this.First.FindQuads(currentViewPort, quads);
            }
            if (this.Second != null && this.Second.IsIntersect(currentViewPort))
            {
                this.Second.FindQuads(currentViewPort, quads);
            }

            if (this.Third != null && this.Third.IsIntersect(currentViewPort))
            {
                this.Third.FindQuads(currentViewPort, quads);
            }

            if (this.Fourth != null && this.Fourth.IsIntersect(currentViewPort))
            {
                this.Fourth.FindQuads(currentViewPort, quads);
            }
            if (this.Objects.Count > 0)
            {
                quads.Add(this);
            }
        }
        private bool IsIntersect(DiagramRect currentViewPort)
        {
            if (this.Left + this.Width < currentViewPort.Left || this.Top + this.Height < currentViewPort.Top || this.Left > currentViewPort.Right || this.Top > currentViewPort.Bottom)
            {
                return false;
            }
            return true;
        }

        internal void Dispose()
        {
            if (spatialSearch != null)
            {
                spatialSearch = null;
            }
            if (Objects.Count < 0)
            {
                for (int i = 0; i < Objects.Count; i++)
                {
                    Objects[i].Dispose();
                    Objects[i] = null;
                }
                Objects.Clear();
                Objects = null;
            }

            if (Parent != null)
            {
                Parent.Dispose();
                Parent = null;
            }

            if (First != null)
            {
                First.Dispose();
                First = null;
            }
            if (Second != null)
            {
                Second.Dispose();
                Second = null;
            }
            if (Third != null)
            {
                Third.Dispose();
                Third = null;
            }
            if (Fourth != null)
            {
                Fourth.Dispose();
                Fourth = null;
            }
        }
    }

    internal class QuadAddition
    {
        internal Quad Quad;
        internal bool IsAdded;
    }
    internal class QuadSet
    {
        internal Quad Target;
        internal Quad Source;
    }
}
