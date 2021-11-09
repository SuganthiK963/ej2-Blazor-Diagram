using System;
using System.Collections.Generic;

namespace Syncfusion.Blazor.Diagram.Internal
{
    internal class Scroller
    {
        private SfDiagramComponent diagram;

        internal TransformFactor Transform = new TransformFactor { TX = 0, TY = 0, Scale = 1 };
        private double currentZoomFactor;
        private double vPortWidth;
        private double vPortHeight;
        private double hOffset;
        private double vOffset;
        private double diagramWidth;
        private double diagramHeight;
        internal double scrollerWidth;
        private double hScrollSize;
        private double vScrollSize;

        internal double CurrentZoom
        {
            get => currentZoomFactor;
            set
            {
                bool isUpdateScrollActions = diagram.ScrollActions.HasFlag(ScrollActions.Interaction);
                diagram.ScrollActions |= ScrollActions.Interaction;
                diagram.ScrollSettings.CZoom = value;
                diagram.ScrollSettings.UpdateCurrentZoomValue();
                currentZoomFactor = value;
                // Consider min/max zoom in initial rendering,
                currentZoomFactor = Math.Min(diagram.ScrollSettings.MaxZoom, currentZoomFactor);
                currentZoomFactor = Math.Max(diagram.ScrollSettings.MinZoom, currentZoomFactor);
                if (!isUpdateScrollActions)
                    diagram.ScrollActions &= ~ScrollActions.Interaction;
            }
        }

        internal double ViewPortHeight
        {
            get => vPortHeight;
            set => vPortHeight = value;
        }

        internal double ViewPortWidth
        {
            get => vPortWidth;
            set => vPortWidth = value;
        }

        internal double HorizontalOffset
        {
            get => hOffset;
            set
            {
                diagram.ScrollActions |= ScrollActions.Interaction;
                diagram.ScrollSettings.HOffset = value;
                diagram.ScrollSettings.UpdateHorizontalValue();
                hOffset = value;
                diagram.ScrollActions &= ~ScrollActions.Interaction;
            }
        }

        internal double VerticalOffset
        {
            get => vOffset;
            set
            {
                diagram.ScrollActions |= ScrollActions.Interaction;
                diagram.ScrollSettings.VOffset = value;
                diagram.ScrollSettings.UpdateVerticalValue();
                vOffset = value;
                diagram.ScrollActions &= ~ScrollActions.Interaction;
            }
        }

        internal Scroller(SfDiagramComponent sfDiagram)
        {
            diagram = sfDiagram;
            Transform = diagram.Scroller != null ? diagram.Scroller.Transform : new TransformFactor { TX = 0, TY = 0, Scale = 1 };
            CurrentZoom = diagram.ScrollSettings.CurrentZoom;
            hOffset = diagram.ScrollSettings.HorizontalOffset;
            vOffset = diagram.ScrollSettings.VerticalOffset;
        }

        internal void UpdateScrollOffsets(double? hOffset = null, double? vOffset = null, bool? isInitialLoading = false)
        {
            DiagramRect pageBounds = GetPageBounds(null, null);
            pageBounds.X *= CurrentZoom;
            pageBounds.Y *= CurrentZoom;
            pageBounds.Width *= CurrentZoom;
            pageBounds.Height *= CurrentZoom;

            if (hOffset != null && vOffset != null)
            {
                double offsetX = Math.Max(0, hOffset.Value - pageBounds.Left);
                double offsetY = Math.Max(0, vOffset.Value - pageBounds.Top);
                HorizontalOffset = offsetX;
                VerticalOffset = offsetY;
                diagram.DiagramContent.SetOffset(offsetX, offsetY);
            }
            else
            {
                if (isInitialLoading.HasValue && isInitialLoading.Value)
                {
                    Transform = new TransformFactor
                    {
                        TX = Math.Max(HorizontalOffset, -pageBounds.Left) / CurrentZoom,
                        TY = Math.Max(VerticalOffset, -pageBounds.Top) / CurrentZoom,
                        Scale = CurrentZoom
                    };
                }
                diagram.DiagramContent.SetOffset(-HorizontalOffset - pageBounds.X, -VerticalOffset - pageBounds.Y);
            }
            Transform = new TransformFactor
            {
                TX = Math.Max(HorizontalOffset, -pageBounds.Left) / CurrentZoom,
                TY = Math.Max(VerticalOffset, -pageBounds.Top) / CurrentZoom,
                Scale = CurrentZoom
            };
        }

        internal void SetScrollOffset(double hOffset, double vOffset)
        {
            DiagramRect pageBounds = GetPageBounds(null, null);
            pageBounds.X *= CurrentZoom;
            pageBounds.Y *= CurrentZoom;
            pageBounds.Width *= CurrentZoom;
            pageBounds.Height *= CurrentZoom;

            double x = -pageBounds.Left;
            double y = -pageBounds.Top;
            bool set = false;
            double viewWidth = ViewPortWidth * CurrentZoom;
            double viewHeight = ViewPortHeight * CurrentZoom;
            double newX = x - hOffset;
            if (!newX.Equals(HorizontalOffset))
            {
                if (x < HorizontalOffset)
                {
                    if (HorizontalOffset > newX)
                    {
                        HorizontalOffset -= hOffset;
                    }
                    else
                    {
                        HorizontalOffset = newX;
                    }
                    set = true;
                }
                double right = Math.Max(pageBounds.Right + vScrollSize, viewWidth);
                if (!set && right < -newX + ViewPortWidth)
                {
                    double actualRight = -newX + viewWidth - vScrollSize;
                    double currentRight = -HorizontalOffset + viewWidth - vScrollSize;
                    if (actualRight < currentRight)
                    {
                        HorizontalOffset = newX;
                    }
                    else
                    {
                        if (actualRight - pageBounds.Right > actualRight - currentRight)
                        {
                            HorizontalOffset = newX;
                        }
                        else
                        {
                            HorizontalOffset = -(pageBounds.Right + vScrollSize - viewWidth);
                        }
                    }
                    set = true;
                }
                if (!set)
                {
                    HorizontalOffset = x - hOffset;
                }
            }
            set = false;

            double newY = y - vOffset;
            if (!newY.Equals(VerticalOffset))
            {
                if (y < VerticalOffset)
                {
                    if (VerticalOffset > newY)
                    {
                        VerticalOffset -= vOffset;
                    }
                    else
                    {
                        VerticalOffset = newY;
                    }
                    set = true;
                }
                double bottom = Math.Max(pageBounds.Bottom + hScrollSize, viewHeight);
                if (!set && bottom < -newY + viewHeight)
                {
                    double actualBottom = -newY + viewHeight - hScrollSize;
                    double currentBottom = -VerticalOffset + viewHeight - hScrollSize;
                    if (actualBottom < currentBottom)
                    {
                        VerticalOffset = newY;
                    }
                    else
                    {
                        if (actualBottom - pageBounds.Bottom > actualBottom - currentBottom)
                        {
                            VerticalOffset = newY;
                        }
                        else
                        {
                            VerticalOffset = -(pageBounds.Bottom + hScrollSize - viewHeight);
                        }
                    }
                    set = true;
                }
                if (!set)
                {
                    VerticalOffset = y - vOffset;
                }
            }

            Transform = new TransformFactor
            {
                TX = Math.Max(HorizontalOffset, -pageBounds.Left) / CurrentZoom,
                TY = Math.Max(VerticalOffset, -pageBounds.Top) / CurrentZoom,
                Scale = CurrentZoom
            };
            SetSize(null, true);
            ScrollChangedEventArgs args = new ScrollChangedEventArgs()
            {
                ScrollX = hOffset,
                ScrollY = vOffset,
                ZoomFactor = currentZoomFactor
            };
            diagram.CommandHandler.InvokeDiagramEvents(DiagramEvent.ScrollChanged, args);
        }

        internal void SetSize(DiagramPoint newOffset = null, bool? isUpdateLayerSize = false)
        {
            DiagramRect pageBounds = GetPageBounds(null, null);
            pageBounds.X *= CurrentZoom;
            pageBounds.Y *= CurrentZoom;
            pageBounds.Width *= CurrentZoom;
            pageBounds.Height *= CurrentZoom;

            double x = Math.Min(pageBounds.X, -HorizontalOffset);
            double y = Math.Min(pageBounds.Y, -VerticalOffset);

            double horizontalScrollSize = scrollerWidth;
            double verticalScrollSize = scrollerWidth;
            if (-VerticalOffset <= pageBounds.Y && -VerticalOffset + ViewPortHeight >= pageBounds.Bottom)
            {
                verticalScrollSize = 0;
            }
            if (-HorizontalOffset <= pageBounds.X && -HorizontalOffset + ViewPortWidth >= pageBounds.Right)
            {
                horizontalScrollSize = 0;
            }
            this.hScrollSize = horizontalScrollSize;
            this.vScrollSize = verticalScrollSize;
            double oldWidth = diagramWidth;
            double oldHeight = diagramHeight;
            diagramWidth = Math.Max(pageBounds.Right, -HorizontalOffset + ViewPortWidth - verticalScrollSize) - x;
            diagramHeight = Math.Max(pageBounds.Bottom, -VerticalOffset + ViewPortHeight - horizontalScrollSize) - y;
            if ((!oldWidth.Equals(diagramWidth) || !oldHeight.Equals(diagramHeight)) && diagram.ScrollSettings.ScrollLimit != ScrollLimitMode.Diagram)
            {
                diagram.DiagramContent.SetSize(diagramWidth, diagramHeight, isUpdateLayerSize != null && isUpdateLayerSize.Value);
            }
            if (diagram.ScrollSettings.ScrollLimit == ScrollLimitMode.Diagram)
            {
                if ((!oldWidth.Equals(diagramWidth) || !oldHeight.Equals(diagramHeight) || CurrentZoom != 1)
                    && ((newOffset == null) || ((VerticalOffset != 0 || VerticalOffset.Equals(newOffset.Y)) &&
                            (HorizontalOffset != 0 || HorizontalOffset.Equals(newOffset.X)))))
                {
                    if (diagram.ScrollActions.HasFlag(ScrollActions.Interaction) && newOffset != null)
                    {
                        Transform = new TransformFactor
                        {
                            TX = Math.Max(newOffset.X, -(pageBounds.Left / CurrentZoom)) / CurrentZoom,
                            TY = Math.Max(newOffset.Y, -(pageBounds.Top / CurrentZoom)) / CurrentZoom,
                            Scale = CurrentZoom
                        };
                        HorizontalOffset = newOffset.X;
                        VerticalOffset = newOffset.Y;
                    }
                    diagram.DiagramContent.SetSize(diagramWidth, diagramHeight, isUpdateLayerSize != null && isUpdateLayerSize.Value);
                    if ((!diagram.ScrollActions.HasFlag(ScrollActions.PropertyChange)) && newOffset != null)
                    {
                        HorizontalOffset = newOffset.X;
                        VerticalOffset = newOffset.Y;
                        Transform = new TransformFactor
                        {
                            TX = Math.Max(newOffset.X, -pageBounds.Left) / CurrentZoom,
                            TY = Math.Max(newOffset.Y, -pageBounds.Top) / CurrentZoom,
                            Scale = CurrentZoom
                        };
                    }
                }
                else if (newOffset != null && oldWidth.Equals(diagramWidth) && oldHeight.Equals(diagramHeight) &&
                  ((diagram.DiagramCanvasScrollBounds.Height > ViewPortHeight &&
                      newOffset.Y < 0 && HorizontalOffset.Equals(newOffset.X) && VerticalOffset == 0) ||
                      (diagram.DiagramCanvasScrollBounds.Width > ViewPortWidth &&
                      newOffset.X < 0 && VerticalOffset.Equals(newOffset.Y) && HorizontalOffset == 0)))
                {
                    VerticalOffset = newOffset.Y;
                    HorizontalOffset = newOffset.X;
                    Transform = new TransformFactor
                    {
                        TX = Math.Max(newOffset.X, -pageBounds.Left) / CurrentZoom,
                        TY = Math.Max(newOffset.Y, -pageBounds.Top) / CurrentZoom,
                        Scale = CurrentZoom
                    };
                }
            }
        }

        internal void SetViewPortSize(double width, double height)
        {
            ViewPortWidth = width;
            ViewPortHeight = height;
        }

        internal DiagramRect GetPageBounds(bool? boundingRect = null, DiagramPrintExportRegion? region = null)
        {
            DiagramRect pageBounds;
            PageSettings pageSettings = diagram.PageSettings;
            if ((region == null || region != DiagramPrintExportRegion.Content) && pageSettings.Width.HasValue && pageSettings.Height.HasValue)
            {
                double width = BaseUtil.GetDoubleValue(pageSettings.Width);
                double height = BaseUtil.GetDoubleValue(pageSettings.Height);
                double negativeWidth = 0;
                double negativeHeight = 0;
                if (pageSettings.MultiplePage)
                {
                    DiagramRect rect = diagram.SpatialSearch.GetPageBounds(0, 0);
                    if (rect.Right > width)
                    {
                        double x = Math.Ceiling(rect.Right / width);
                        width *= x;
                    }
                    if (rect.Bottom > height)
                    {
                        double x = Math.Ceiling(rect.Bottom / height);
                        height *= x;
                    }
                    if (rect.Left < 0 && Math.Abs(rect.Left) > negativeWidth)
                    {
                        double pageWidth = BaseUtil.GetDoubleValue(pageSettings.Width);
                        double x = Math.Ceiling(Math.Abs(rect.Left) / pageWidth);
                        negativeWidth = pageWidth * x;
                    }
                    if (rect.Top < 0 && Math.Abs(rect.Top) > negativeHeight)
                    {
                        double pageHeight = BaseUtil.GetDoubleValue(pageSettings.Height);
                        double x = Math.Ceiling(Math.Abs(rect.Top) / pageHeight);
                        negativeHeight = pageHeight * x;
                    }
                }
                pageBounds = new DiagramRect(-negativeWidth, -negativeHeight, width + negativeWidth, height + negativeHeight);
            }
            else
            {
                double? origin;
                if (boundingRect.HasValue && boundingRect.Value)
                {
                    origin = null;
                }
                else
                {
                    origin = 0;
                }
                pageBounds = diagram.SpatialSearch.GetPageBounds(origin, origin);
            }
            return pageBounds;
        }

        internal List<Segment> GetPageBreak(DiagramRect pageBounds)
        {
            double i = 0; double j = 0;
            List<Segment> collection = new List<Segment>();
            PageMargin margin = diagram.PageSettings.Margin;
            double left = margin.Left; double right = margin.Right;
            double top = margin.Top; double bottom = margin.Bottom;
            double widthCount = 1; double heightCount = 1;
            Segment segment;
            while (pageBounds.Width > i)
            {
                i += diagram.PageSettings.Width ?? pageBounds.Width;
                if (Equals(i, diagram.PageSettings.Width))
                {
                    segment = new Segment
                    {
                        X1 = pageBounds.Left + left,
                        Y1 = pageBounds.Top + top,
                        X2 = pageBounds.Left + left,
                        Y2 = pageBounds.Bottom - bottom
                    };
                    collection.Add(segment);
                }
                if (i < pageBounds.Width)
                {
                    segment = new Segment
                    {
                        X1 = pageBounds.TopLeft.X + diagram.PageSettings.Width.Value * widthCount,
                        Y1 = pageBounds.TopLeft.Y + top,
                        X2 = pageBounds.BottomLeft.X + diagram.PageSettings.Width.Value * widthCount,
                        Y2 = pageBounds.BottomLeft.Y - bottom
                    };
                    collection.Add(segment);
                    widthCount++;
                }
                if (pageBounds.Width.Equals(i))
                {
                    segment = new Segment
                    {
                        X1 = pageBounds.Right - right,
                        Y1 = pageBounds.Top + top,
                        X2 = pageBounds.Right - right,
                        Y2 = pageBounds.Bottom - bottom
                    };
                    collection.Add(segment);
                }
            }
            while (pageBounds.Height > j)
            {
                j += diagram.PageSettings.Height ?? pageBounds.Height;
                if (Equals(j, diagram.PageSettings.Height))
                {
                    segment = new Segment
                    {
                        X1 = pageBounds.Left + left,
                        Y1 = pageBounds.Top + top,
                        X2 = pageBounds.Right - right,
                        Y2 = pageBounds.Top + top
                    };
                    collection.Add(segment);
                }
                if (j < pageBounds.Height)
                {
                    segment = new Segment
                    {
                        X1 = pageBounds.TopLeft.X + left,
                        Y1 = pageBounds.TopLeft.Y + diagram.PageSettings.Height.Value * heightCount,
                        X2 = pageBounds.TopRight.X - right,
                        Y2 = pageBounds.TopRight.Y + diagram.PageSettings.Height.Value * heightCount
                    };
                    collection.Add(segment);
                    heightCount++;
                }
                if (pageBounds.Height.Equals(j))
                {
                    segment = new Segment
                    {
                        X1 = pageBounds.Left + left,
                        Y1 = pageBounds.Bottom - bottom,
                        X2 = pageBounds.Right - right,
                        Y2 = pageBounds.Bottom - bottom
                    };
                    collection.Add(segment);
                }
            }
            return collection;
        }

        internal void Zoom(double factor, double? deltaX = null, double? deltaY = null, DiagramPoint focusPoint = null)
        {
            if (ConstraintsUtil.CanZoom(this.diagram) && factor != 1 || (ConstraintsUtil.CanPan(this.diagram) && factor == 1))
            {
                Matrix matrix = Matrix.IdentityMatrix();
                Matrix.ScaleMatrix(matrix, CurrentZoom, CurrentZoom);
                Matrix.TranslateMatrix(matrix, HorizontalOffset, VerticalOffset);
                focusPoint ??= new DiagramPoint
                {
                    X = (ViewPortWidth / 2 - HorizontalOffset) / CurrentZoom,
                    Y = (ViewPortHeight / 2 - VerticalOffset) / CurrentZoom
                };
                focusPoint = Matrix.TransformPointByMatrix(matrix, focusPoint);
                if ((CurrentZoom * factor) >= diagram.ScrollSettings.MinZoom &&
                    (CurrentZoom * factor) <= diagram.ScrollSettings.MaxZoom)
                {
                    CurrentZoom *= factor;
                    DiagramRect pageBounds = GetPageBounds(null, null);
                    pageBounds.X *= CurrentZoom;
                    pageBounds.Y *= CurrentZoom;

                    //target Matrix
                    Matrix targetMatrix = Matrix.IdentityMatrix();
                    Matrix.ScaleMatrix(targetMatrix, factor, factor, focusPoint.X, focusPoint.Y);
                    Matrix.TranslateMatrix(targetMatrix, deltaX ?? 0, deltaY ?? 0);
                    Matrix.MultiplyMatrix(matrix, targetMatrix);

                    DiagramPoint newOffset = Matrix.TransformPointByMatrix(matrix, new DiagramPoint { X = 0, Y = 0 });
                    if (factor == 1)
                    {
                        newOffset = ApplyScrollLimit(newOffset.X, newOffset.Y);
                    }
                    if (diagram.ScrollActions.HasFlag(ScrollActions.PropertyChange) ||
                        !diagram.ScrollActions.HasFlag(ScrollActions.Interaction) ||
                        diagram.ScrollSettings.ScrollLimit != ScrollLimitMode.Diagram)
                    {
                        Transform = new TransformFactor
                        {
                            TX = Math.Max(newOffset.X, -pageBounds.Left) / this.CurrentZoom,
                            TY = Math.Max(newOffset.Y, -pageBounds.Top) / this.CurrentZoom,
                            Scale = CurrentZoom
                        };
                        HorizontalOffset = newOffset.X;
                        VerticalOffset = newOffset.Y;
                    }
                    SetSize(newOffset);
                    diagram.DiagramContent.SetOffset(-this.HorizontalOffset - pageBounds.X, -this.VerticalOffset - pageBounds.Y);
                    ScrollChangedEventArgs args = new ScrollChangedEventArgs()
                    {
                        ScrollX = hOffset,
                        ScrollY = vOffset,
                        ZoomFactor = currentZoomFactor
                    };
                    diagram.CommandHandler.InvokeDiagramEvents(DiagramEvent.ScrollChanged, args);
                }
            }
        }
        private DiagramPoint ApplyScrollLimit(double hOffset, double vOffset)
        {
            if (diagram.ScrollSettings.ScrollLimit != ScrollLimitMode.Infinity)
            {
                //if (diagram.ScrollSettings.ScrollLimit == ScrollLimit.Limited)
                //{
                //    ScrollableArea scrollableBounds = this.diagram.ScrollSettings.ScrollableArea;
                //    bounds = new Rect(scrollableBounds.X, scrollableBounds.Y, scrollableBounds.Width, scrollableBounds.Height);
                //}
                DiagramRect bounds = GetPageBounds();
                bounds.X *= CurrentZoom;
                bounds.Y *= CurrentZoom;
                bounds.Width *= CurrentZoom;
                bounds.Height *= CurrentZoom;
                hOffset *= -1;
                vOffset *= -1;
                double allowedRight = Math.Max(bounds.Right, ViewPortWidth);
                if (!(hOffset <= bounds.X && (hOffset + ViewPortWidth >= bounds.Right ||
                    hOffset >= bounds.Right - ViewPortWidth)
                    || hOffset >= bounds.X && (hOffset + ViewPortWidth <= allowedRight)))
                {
                    //not allowed case
                    if (hOffset >= bounds.X)
                    {
                        hOffset = Math.Max(
                            bounds.X,
                            Math.Min(hOffset, hOffset - (hOffset + ViewPortWidth - vScrollSize - allowedRight)));
                    }
                    else
                    {
                        double allowed = bounds.Right - ViewPortWidth;
                        hOffset = Math.Min(allowed, bounds.X);
                    }
                }
                double allowedBottom = Math.Max(bounds.Bottom, this.ViewPortHeight);
                if (!(vOffset <= bounds.Y && vOffset + ViewPortHeight >= bounds.Bottom
                    || vOffset >= bounds.Y && vOffset + ViewPortHeight <= allowedBottom))
                {
                    //not allowed case
                    if (vOffset >= bounds.Y)
                    {
                        vOffset = Math.Max(
                            bounds.Y,
                            Math.Min(vOffset, vOffset - (vOffset + ViewPortHeight - hScrollSize - allowedBottom)));
                    }
                    else
                    {
                        double allowed = bounds.Bottom - ViewPortHeight;
                        vOffset = Math.Min(bounds.Y, allowed);
                    }
                }
                hOffset *= -1;
                vOffset *= -1;
            }
            return new DiagramPoint { X = hOffset, Y = vOffset };
        }

        internal void Dispose()
        {
            if (diagram != null)
            {
                diagram = null;
            }
            if (Transform != null)
            {
                Transform.Dispose();
                Transform = null;
            }
        }
    }
    /// <summary>
    /// Represents the transformation factor of the text.
    /// </summary>
    internal class TransformFactor
    {
        /// <summary>
        /// Returns the distance between X old coordinates has to be moved.
        /// </summary>
        public double TX { get; set; }
        /// <summary>
        /// Returns the distance between Y old coordinates has to be moved.
        /// </summary>
        public double TY { get; set; }
        /// <summary>
        /// Returns how the text element fits inside the text element. 
        /// </summary>
        public double Scale { get; set; }

        internal void Dispose()
        {
            TX = 0;
            TY = 0;
            Scale = 0;
        }
    }
}
