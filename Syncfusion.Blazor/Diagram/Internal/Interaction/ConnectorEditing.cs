using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Syncfusion.Blazor.Diagram.Internal
{
    internal class ConnectorEditing : InteractionControllerBase
    {
        private readonly Actions endPoint;
        private ConnectorSegment selectedSegment;
        private int segmentIndex;

        internal ConnectorEditing(SfDiagramComponent diagram, Actions endPoint) : base(diagram)
        {
            this.endPoint = endPoint;
        }

        public override void OnMouseDown(DiagramMouseEventArgs args)
        {
            Connector connector = null;
            if (args.Element != null)
            {
                if (args.Element is Connector source)
                    connector = source;
                else if (args.Element is DiagramSelectionSettings selector)
                    connector = selector.Connectors[0];
            }
            this.InAction = true;
            if (args.Element != null) this.UndoElement = (IDiagramObject)(args.Element).Clone() as DiagramSelectionSettings;
            base.OnMouseDown(args);
            if (connector != null)
            {
                // Sets the selected segment         
                for (int i = 0; i < connector.SegmentCollection.Count; i++)
                {
                    ConnectorSegment segment = connector.SegmentCollection[i];
                    if (this.endPoint == Actions.OrthogonalThumb)
                    {
                        for (int j = 0; j < segment.Points.Count - 1; j++)
                        {
                            DiagramPoint segPoint = new DiagramPoint(0, 0)
                            {
                                X = ((segment.Points[j].X + segment.Points[j + 1].X) / 2),
                                Y = ((segment.Points[j].Y + segment.Points[j + 1].Y) / 2)
                            };
                            if (ActionsUtil.Contains(this.CurrentPosition, segPoint, 30))
                            {
                                this.selectedSegment = segment;
                                this.segmentIndex = j;
                            }
                        }
                    }
                    else
                    {
                        if (ActionsUtil.Contains(this.CurrentPosition, ((StraightSegment)segment).Point, 10))
                        {
                            this.selectedSegment = segment;
                            this.segmentIndex = i;
                        }
                    }
                }
            }
        }

        public override bool OnMouseMove(DiagramMouseEventArgs args)
        {
            base.OnMouseMove(args);
            if (this.InAction)
            {
                double diffX = (this.CurrentPosition.X - this.PreviousPosition.X);
                double diffY = (this.CurrentPosition.Y - this.PreviousPosition.Y);

                if (diffX != 0 || diffY != 0)
                {
                    DiagramSelectionSettings helperObject = CommandHandler.RenderHelper();
                    Connector connector = helperObject.Connectors[0];
                    currentPoint = args.Position;
                    // Sets the selected segment         
                    for (int i = 0; i < connector.SegmentCollection.Count; i++)
                    {
                        ConnectorSegment segment = connector.SegmentCollection[i];
                        if (this.endPoint == Actions.OrthogonalThumb)
                        {
                            for (int j = 0; j < segment.Points.Count - 1; j++)
                            {
                                DiagramPoint segPoint = new DiagramPoint(0, 0)
                                {
                                    X = ((segment.Points[j].X + segment.Points[j + 1].X) / 2),
                                    Y = ((segment.Points[j].Y + segment.Points[j + 1].Y) / 2)
                                };
                                if (ActionsUtil.Contains(this.CurrentPosition, segPoint, 10))
                                {
                                    this.selectedSegment = segment;
                                    this.segmentIndex = j;
                                    break;
                                }
                            }
                        }
                    }
                    SfDiagramComponent.IsProtectedOnChange = false;
                    if (endPoint == Actions.OrthogonalThumb)
                    {
                        this.DragOrthogonalSegment(helperObject.Connectors[0], this.selectedSegment as OrthogonalSegment, this.CurrentPosition, this.segmentIndex);
                    }
                    if (endPoint == Actions.SegmentEnd)
                    {
                        CommandHandler.DragControlPoint(helperObject.Connectors[0], diffX, diffY, this.segmentIndex);
                    }
                    List<DiagramPoint> points = helperObject.Connectors[0].GetConnectorPoints();
                    DiagramUtil.UpdateConnector(helperObject.Connectors[0], points.Count > 0 ? points : helperObject.Connectors[0].IntermediatePoints);
                    helperObject.Connectors[0].Wrapper.Measure(new DiagramSize());
                    helperObject.Connectors[0].Wrapper.Arrange(helperObject.Connectors[0].Wrapper.DesiredSize);
                    CommandHandler.RefreshDiagram();
                    SfDiagramComponent.IsProtectedOnChange = true;
                }

            }
            this.PreviousPosition = this.CurrentPosition;
            return !this.blocked;
        }

        public override void OnMouseUp(DiagramMouseEventArgs args)
        {
            if (endPoint == Actions.SegmentEnd)
            {
                if (args.Info?.CtrlKey != null && args.Info.ShiftKey.HasValue && args.Info.CtrlKey.Value && args.Info.ShiftKey.Value && args.Element != null)
                {
                    Connector connector = args.Element is DiagramSelectionSettings selector ? selector.Connectors[0] : args.Element as Connector;
                    AddOrRemoveSegment(connector, this.CurrentPosition);
                    if (connector != null)
                    {
                        List<DiagramPoint> points = connector.GetConnectorPoints();
                        DiagramUtil.UpdateConnector(connector, points.Count > 0 ? points : connector.IntermediatePoints);
                    }

                    if (connector != null)
                    {
                        connector.Wrapper.Measure(new DiagramSize());
                        connector.Wrapper.Arrange(connector.Wrapper.DesiredSize);
                    }

                    CommandHandler.RefreshDiagram();
                }
                else
                {
                    SfDiagramComponent.IsProtectedOnChange = false;
                    Connector connector = (args.Element as DiagramSelectionSettings)?.Connectors[0];
                    double diffX = ((StraightSegment)CommandHandler.HelperObject.Connectors[0].SegmentCollection[this.segmentIndex]).Point.X - (connector.SegmentCollection[this.segmentIndex] as StraightSegment).Point.X;
                    double diffY = ((StraightSegment)CommandHandler.HelperObject.Connectors[0].SegmentCollection[this.segmentIndex]).Point.Y - (connector.SegmentCollection[this.segmentIndex] as StraightSegment).Point.Y;
                    CommandHandler.DragControlPoint(connector, diffX, diffY, this.segmentIndex);
                    List<DiagramPoint> points = connector.GetConnectorPoints();
                    DiagramUtil.UpdateConnector(connector, points.Count > 0 ? points : connector.IntermediatePoints);
                    connector.Wrapper.Measure(new DiagramSize());
                    connector.Wrapper.Arrange(connector.Wrapper.DesiredSize);
                    CommandHandler.HelperObject = null;
                    CommandHandler.RefreshDiagram();
                    SfDiagramComponent.IsProtectedOnChange = true;
                }
            }
            else if (endPoint == Actions.OrthogonalThumb)
            {
                if (this.selectedSegment != null && this.CommandHandler.HelperObject != null)
                {
                    SfDiagramComponent.IsProtectedOnChange = false;
                    Connector connector = this.CommandHandler.HelperObject.Connectors[0];
                    int index = connector.SegmentCollection.IndexOf(this.selectedSegment);
                    if (index != -1)
                    {
                        OrthogonalSegment prev = (connector.SegmentCollection.Count > index - 1) ? connector.SegmentCollection[index - 1] as OrthogonalSegment : null;
                        OrthogonalSegment next = (connector.SegmentCollection.Count > index + 1) ? connector.SegmentCollection[index + 1] as OrthogonalSegment : null;
                        if (index == connector.SegmentCollection.Count - 2
                            && UpdateLastSegment(connector, this.selectedSegment as OrthogonalSegment))
                        {
                            connector.SegmentCollection.RemoveAt(connector.SegmentCollection.Count - 2);
                        }
                        else
                        {
                            if (prev != null && prev.Length.HasValue && Math.Abs(prev.Length.Value) < 5)
                            {
                                if (index != 1)
                                {
                                    this.RemovePrevSegment(connector, index);
                                }
                            }
                            else if (next != null)
                            {
                                double len = DiagramPoint.DistancePoints(next.Points[0], next.Points[1]);
                                double length = next.Length ?? len;
                                if ((Math.Abs(length) <= 5))
                                {
                                    this.RemoveNextSegment(connector, index);
                                }
                            }
                        }
                        SfDiagramComponent.IsProtectedOnChange = true;
                    }
                    this.CommandHandler.UpdateConnectorProperties();
                }
            }
            if (this.UndoElement != null)
            {
                if (args.Element != null)
                {
                    InternalHistoryEntry entry = new InternalHistoryEntry()
                    {
                        Type = HistoryEntryType.SegmentChanged,
                        RedoObject = (args.Element).Clone() as IDiagramObject,
                        UndoObject = this.UndoElement.Clone() as DiagramSelectionSettings,
                        Category = EntryCategory.InternalEntry,
                    };
                    this.CommandHandler.AddHistoryEntry(entry);
                }
            }
            base.OnMouseUp(args);
        }

        private void AddOrRemoveSegment(Connector connector, DiagramPoint point)
        {
            if (connector.Type == ConnectorSegmentType.Straight)
            {
                bool updateSeg = false; int? segmentIndex = null;
                for (int i = 0; i < connector.SegmentCollection.Count; i++)
                {
                    StraightSegment segment = (StraightSegment)(connector.SegmentCollection)[i];
                    if (segment != null && ActionsUtil.Contains(point, segment.Point, connector.HitPadding))
                    {
                        segmentIndex = i;
                        updateSeg = true;
                    }
                }
                if (updateSeg)
                {
                    if (connector.SegmentCollection != null && connector.SegmentCollection[segmentIndex.Value] != null && connector.SegmentCollection[segmentIndex.Value].Type == ConnectorSegmentType.Straight)
                    {
                        if (connector.SegmentCollection[segmentIndex.Value + 1] is StraightSegment previous)
                        {
                            SegmentCollectionChangeEventArgs args = new SegmentCollectionChangeEventArgs()
                            {
                                Cancel = false,
                                Element = connector,
                                RemovedSegments = new DiagramObjectCollection<ConnectorSegment>() { connector.SegmentCollection[segmentIndex.Value] },
                                Type = CollectionChangedAction.Remove
                            };
                            CommandHandler.InvokeDiagramEvents(DiagramEvent.SegmentCollectionChange, args);
                            if (!args.Cancel)
                            {
                                connector.SegmentCollection.RemoveAt(segmentIndex.Value);
                                if (connector.SegmentCollection[segmentIndex.Value] is StraightSegment segment) previous.Points[0] = segment.Points[0];
                            }
                        }
                    }
                }
                else
                {
                    int index = FindIndex(connector, point);
                    if (connector.SegmentCollection != null && connector.SegmentCollection[index] != null && connector.SegmentCollection[index].Type == ConnectorSegmentType.Straight)
                    {
                        StraightSegment newSegment = new StraightSegment() { Type = ConnectorSegmentType.Straight, Point = point, Points = new List<DiagramPoint>() };
                        if (connector.SegmentCollection[index] is StraightSegment segment)
                        {
                            newSegment.Points.Add(segment.Points[0]);
                            newSegment.Points.Add(point);
                            SegmentCollectionChangeEventArgs args = new SegmentCollectionChangeEventArgs()
                            {
                                Element = connector,
                                Cancel = false,
                                AddedSegments = new DiagramObjectCollection<ConnectorSegment>() { newSegment },
                                Type = CollectionChangedAction.Add
                            };
                            CommandHandler.InvokeDiagramEvents(DiagramEvent.SegmentCollectionChange, args);
                            if (!args.Cancel)
                            {
                                segment.Points[0] = point;
                                connector.SegmentCollection.Insert(index, newSegment);
                            }
                        }
                    }
                }
            }
        }

        private static int FindIndex(Connector connector, DiagramPoint point)
        {
            ObservableCollection<StraightSegment> intersectingSegments = new ObservableCollection<StraightSegment>();
            for (int i = 0; i < connector.SegmentCollection.Count; i++)
            {
                if (connector.SegmentCollection[i] is StraightSegment segment)
                {
                    DiagramRect rect = DiagramRect.ToBounds(new List<DiagramPoint> { segment.Points[0], segment.Points[1] });
                    rect.Inflate(connector.HitPadding);

                    if (rect.ContainsPoint(point))
                    {
                        intersectingSegments.Add(segment);
                    }
                }
            }
            if (intersectingSegments.Count == 1)
            {
                return connector.SegmentCollection.IndexOf(intersectingSegments[0]);
            }
            else
            {
                double min = 0; int index = 0;
                for (int i = 0; i < intersectingSegments.Count; i++)
                {
                    StraightSegment seg = intersectingSegments[i];
                    double v = (point.Y - seg.Points[0].Y) / (seg.Points[1].Y - point.Y);
                    double h = (point.X - seg.Points[0].X) / (seg.Points[1].X - point.X);
                    double ratio = Math.Abs(v - h);
                    if (i == 0) { min = ratio; index = 0; }
                    if (ratio < min) { min = ratio; index = i; }
                }
                return connector.SegmentCollection.IndexOf(intersectingSegments[index]);
            }
        }

        private void RemovePrevSegment(Connector connector, int index)
        {
            OrthogonalSegment first = connector.SegmentCollection[index - 2] as OrthogonalSegment;
            OrthogonalSegment next = (OrthogonalSegment)connector.SegmentCollection[index + 1];
            if (next != null)
            {
                double length = next.Length ?? DiagramPoint.DistancePoints(next.Points[0], next.Points[1]);
                if (!(length <= 5))
                {
                    DiagramObjectCollection<ConnectorSegment> removeSegments = new DiagramObjectCollection<ConnectorSegment>()
                    {
                        connector.SegmentCollection[index], connector.SegmentCollection[index-1]
                    };
                    SegmentCollectionChangeEventArgs args = new SegmentCollectionChangeEventArgs()
                    {
                        Element = connector,
                        RemovedSegments = removeSegments,
                        Cancel = false,
                        Type = CollectionChangedAction.Remove
                    };
                    CommandHandler.InvokeDiagramEvents(DiagramEvent.SegmentCollectionChange, args);
                    if (!args.Cancel)
                    {
                        OrthogonalSegment last = connector.SegmentCollection[index + 1] as OrthogonalSegment;
                        //connector.segments.splice(index - 1, 2);
                        connector.SegmentCollection.RemoveAt(index);
                        connector.SegmentCollection.RemoveAt(index - 1);
                        OrthogonalSegment segment = this.selectedSegment as OrthogonalSegment;
                        if (segment != null && (segment.Direction == Direction.Left || segment.Direction == Direction.Right))
                        {
                            if (first != null)
                            {
                                if (last != null)
                                {
                                    first.Points[^1].X = last.Points[0].X;
                                    last.Points[0].Y = first.Points[^1].Y;
                                }
                            }
                        }
                        else
                        {
                            if (first != null)
                            {
                                if (last != null)
                                {
                                    first.Points[^1].Y = last.Points[0].Y;
                                    last.Points[0].X = first.Points[^1].X;
                                }
                            }
                        }
                        if (segment != null && segment.Length.HasValue)
                        {
                            FindSegmentDirection(first);
                        }
                        FindSegmentDirection(last);
                    }
                }
            }
        }

        private static void FindSegmentDirection(OrthogonalSegment segment)
        {
            if (segment.Direction.HasValue && segment.Length.HasValue)
            {
                segment.Length = DiagramPoint.DistancePoints(segment.Points[0], segment.Points[^1]);
                segment.Direction = DiagramPoint.Direction(segment.Points[0], segment.Points[^1]);
            }
        }

        private void RemoveNextSegment(Connector connector, int index)
        {
            OrthogonalSegment segment = this.selectedSegment as OrthogonalSegment;
            OrthogonalSegment first = (connector.SegmentCollection.Count > index - 1) ? connector.SegmentCollection[index - 1] as OrthogonalSegment : null;
            OrthogonalSegment last = (connector.SegmentCollection.Count > index + 2) ? connector.SegmentCollection[index + 2] as OrthogonalSegment : null;
            OrthogonalSegment next = (connector.SegmentCollection.Count > index + 1) ? connector.SegmentCollection[index + 1] as OrthogonalSegment : null;
            DiagramObjectCollection<ConnectorSegment> removeSegments; SegmentCollectionChangeEventArgs args;
            if (next != null && next.Length.HasValue)
            {
                removeSegments = new DiagramObjectCollection<ConnectorSegment>()
                {
                    connector.SegmentCollection[index], connector.SegmentCollection[index+1]
                };
                args = new SegmentCollectionChangeEventArgs()
                {
                    Element = connector,
                    RemovedSegments = removeSegments,
                    Cancel = false,
                    Type = CollectionChangedAction.Remove
                };
                CommandHandler.InvokeDiagramEvents(DiagramEvent.SegmentCollectionChange, args);
                if (!args.Cancel)
                {
                    connector.SegmentCollection.RemoveAt(index);
                    connector.SegmentCollection.RemoveAt(index);
                    if (segment != null && (segment.Direction == Direction.Top || segment.Direction == Direction.Bottom))
                    {
                        if (last != null)
                        {
                            last.Points[0].Y = segment.Points[0].Y;
                            if (first != null) first.Points[^1].X = last.Points[0].X;
                        }
                    }
                    else
                    {
                        if (last != null)
                        {
                            if (segment != null) last.Points[0].X = segment.Points[0].X;
                            if (first != null) first.Points[^1].Y = last.Points[0].Y;
                        }
                    }
                }
            }
            else
            {
                removeSegments = new DiagramObjectCollection<ConnectorSegment>() { connector.SegmentCollection[index + 1] };
                args = new SegmentCollectionChangeEventArgs() { Element = connector, Cancel = false, RemovedSegments = removeSegments, Type = CollectionChangedAction.Remove };
                CommandHandler.InvokeDiagramEvents(DiagramEvent.SegmentCollectionChange, args);
                if (!args.Cancel)
                {
                    //connector.segments.splice(index + 1, 1);
                    connector.SegmentCollection.RemoveAt(index + 1);
                    if (segment != null && (segment.Direction == Direction.Top || segment.Direction == Direction.Bottom))
                    {
                        if (first != null)
                            if (next != null)
                                first.Points[^1].X = next.Points[^1].X;
                    }
                    else
                    {
                        if (first != null)
                            if (next != null)
                                first.Points[^1].Y = next.Points[^1].Y;
                    }
                    FindSegmentDirection(first);
                    if (segment != null)
                    {
                        segment.Length = null;
                        segment.Direction = null;
                    }
                }
            }
            if (first != null && last != null && !args.Cancel)
            {
                first.Length = DiagramPoint.DistancePoints(first.Points[0], last.Points[0]);
                first.Direction = DiagramPoint.Direction(first.Points[0], last.Points[0]);
                if (last.Length.HasValue)
                {
                    last.Length = DiagramPoint.DistancePoints(first.Points[^1], last.Points[^1]);
                    List<DiagramPoint> point1 = first.Points; List<DiagramPoint> point2 = last.Points;
                    last.Direction = DiagramPoint.Direction(point1[^1], point2[^1]);
                }
            }
        }

        private bool DragOrthogonalSegment(Connector obj, OrthogonalSegment segment, DiagramPoint point, int segmentIndex)
        {
            DiagramPoint segmentPoint = new DiagramPoint()
            {
                X = ((segment.Points[segmentIndex].X + segment.Points[segmentIndex + 1].X) / 2),
                Y = ((segment.Points[segmentIndex].Y + segment.Points[segmentIndex + 1].Y) / 2)
            };
            double ty = point.Y - segmentPoint.Y;
            double tx = point.X - segmentPoint.X;
            int index = obj.SegmentCollection.IndexOf(segment); bool update = false;
            if (index != -1)
            {
                if (index == 0 && obj.SegmentCollection.Count == 1 && segment.Points.Count == 2)
                {
                    index = this.AddSegments(obj, segment, tx, ty, index);
                    update = true;
                }
                else if (index == obj.SegmentCollection.Count - 1 && (segment.Direction == null || segment.Length == null))
                {
                    index = this.AddTerminalSegment(obj, segment, segmentIndex);
                    update = true;
                }
                else if (index == 0)
                {
                    index = this.InsertFirstSegment(obj, segment, tx, ty);
                    update = true;
                }
                if (index != 0)
                {
                    if (update)
                    {
                        this.selectedSegment = obj.SegmentCollection[index] as OrthogonalSegment;
                        this.segmentIndex = 0;
                    }
                    this.UpdateAdjacentSegments(obj, index, tx, ty);
                    List<DiagramPoint> points = obj.GetConnectorPoints();
                    DiagramUtil.UpdateConnector(obj, points.Count > 0 ? points : obj.IntermediatePoints);
                    obj.Wrapper.Measure(new DiagramSize());
                    obj.Wrapper.Arrange(obj.Wrapper.DesiredSize);
                    CommandHandler.RefreshDiagram();
                }
            }
            return true;
        }

        private int AddSegments(Connector obj, OrthogonalSegment segment, double tx, double ty, int coll)
        {
            int index;
            DiagramObjectCollection<ConnectorSegment> segments = new DiagramObjectCollection<ConnectorSegment>(); double len;
            double length = DiagramPoint.DistancePoints(segment.Points[0], segment.Points[1]);
            Direction segmentDirection = DiagramPoint.Direction(segment.Points[0], segment.Points[1]);
            segments.Add(new OrthogonalSegment() { Type = ConnectorSegmentType.Orthogonal, Direction = segmentDirection, Length = length / 4 });
            Direction direction = (segment.Points[0].Y.Equals(segment.Points[1].Y)) ? ((ty > 0) ? Direction.Bottom : Direction.Top) : ((tx > 0) ? Direction.Right : Direction.Left);
            len = (segment.Points[0].X.Equals(segment.Points[1].X)) ? ty : tx;
            segments.Add(new OrthogonalSegment() { Type = ConnectorSegmentType.Orthogonal, Direction = direction, Length = len });
            segments.Add(new OrthogonalSegment() { Type = ConnectorSegmentType.Orthogonal, Direction = segmentDirection, Length = length / 2 });
            SegmentCollectionChangeEventArgs args = new SegmentCollectionChangeEventArgs()
            {
                Element = obj,
                Cancel = false,
                AddedSegments = segments,
                Type = CollectionChangedAction.Add
            };
            CommandHandler.InvokeDiagramEvents(DiagramEvent.SegmentCollectionChange, args);
            if (!args.Cancel)
            {
                for (int i = 0; i < obj.SegmentCollection.Count; i++)
                {
                    segments.Add(obj.SegmentCollection[i]);
                }
                obj.SegmentCollection = segments;
                index = coll + 2;
            }
            else
            {
                index = coll;
            }
            return index;
        }

        private int InsertFirstSegment(Connector obj, OrthogonalSegment segment, double tx, double ty)
        {
            Direction? direction = null; double length = 0; DiagramObjectCollection<ConnectorSegment> segments = new DiagramObjectCollection<ConnectorSegment>();
            int index = 0; OrthogonalSegment insertSegment;
            if (!string.IsNullOrEmpty(obj.SourcePortID) && segment.Length.HasValue && (obj.SegmentCollection[0]).Points.Count > 2)
            {
                OrthogonalSegment prev = null;
                for (int i = 0; i < segment.Points.Count - 1; i++)
                {
                    double len = DiagramPoint.DistancePoints(segment.Points[i], segment.Points[i + 1]);
                    Direction dir = DiagramPoint.Direction(segment.Points[i], segment.Points[i + 1]);
                    insertSegment = new OrthogonalSegment() { Type = ConnectorSegmentType.Orthogonal, Direction = dir, Length = len };
                    if (insertSegment.Length.HasValue && insertSegment.Length.Value == 0)
                    {
                        if (prev != null && (prev.Direction == Direction.Top || prev.Direction == Direction.Bottom))
                        {
                            insertSegment.Direction = tx > 0 ? Direction.Right : Direction.Left;
                        }
                        else
                        {
                            insertSegment.Direction = ty > 0 ? Direction.Bottom : Direction.Top;
                        }
                    }
                    prev = insertSegment;
                    segments.Add(insertSegment);
                }
            }
            else
            {
                segments.Add(new OrthogonalSegment() { Type = ConnectorSegmentType.Orthogonal, Direction = segment.Direction, Length = segment.Length / 3 });
                if (segment.Direction == Direction.Bottom || segment.Direction == Direction.Top)
                {
                    length = Math.Abs(tx);
                    direction = tx > 0 ? Direction.Right : Direction.Left;
                }
                else
                {
                    length = Math.Abs(ty);
                    direction = ty > 0 ? Direction.Bottom : Direction.Top;
                }
                insertSegment = new OrthogonalSegment() { Type = ConnectorSegmentType.Orthogonal, Direction = direction, Length = length };
                segments.Add(insertSegment);
            }
            SegmentCollectionChangeEventArgs args = new SegmentCollectionChangeEventArgs()
            {
                Element = obj,
                AddedSegments = segments,
                Cancel = false,
                Type = CollectionChangedAction.Add
            };
            CommandHandler.InvokeDiagramEvents(DiagramEvent.SegmentCollectionChange, args);
            if (!args.Cancel)
            {
                if (!string.IsNullOrEmpty(obj.SourcePortID) && segment.Length.HasValue && (obj.SegmentCollection[0]).Points.Count > 2)
                {
                    obj.SegmentCollection.RemoveAt(0); index = 1;
                }
                else
                {
                    if (obj.SegmentCollection[1] is OrthogonalSegment nextSegment && nextSegment.Length.HasValue)
                    {
                        nextSegment.Length = (direction != nextSegment.Direction) ? nextSegment.Length.Value + length : nextSegment.Length.Value - length;
                    }
                    index = 2; segment.Length = 2 * segment.Length / 3;
                }
                for (int i = 0; i < obj.SegmentCollection.Count; i++)
                {
                    segments.Add(obj.SegmentCollection[i]);
                }
                obj.SegmentCollection = segments;
                List<DiagramPoint> points = obj.GetConnectorPoints();
                DiagramUtil.UpdateConnector(obj, points.Count > 0 ? points : obj.IntermediatePoints);
                obj.Wrapper.Measure(new DiagramSize());
                obj.Wrapper.Arrange(obj.Wrapper.DesiredSize);
                CommandHandler.RefreshDiagram();
            }
            return index;
        }

        private void UpdateAdjacentSegments(Connector obj, int index, double tx, double ty)
        {
            if (obj.SegmentCollection[index] is OrthogonalSegment current)
            {
                DiagramPoint endPt = current.Points[^1];
                DiagramPoint startPoint = current.Points[0];
                bool isNextUpdate = true;
                if (current.Type == ConnectorSegmentType.Orthogonal)
                {
                    current.Points[0] = startPoint; current.Points[^1] = endPt;
                    if (obj.SegmentCollection[index - 1] is OrthogonalSegment prev)
                    {
                        isNextUpdate = this.UpdatePreviousSegment(tx, ty, obj, index);
                    }
                    if (obj.SegmentCollection.Count - 1 > index && isNextUpdate)
                    {
                        OrthogonalSegment nextSegment = obj.SegmentCollection[index + 1] as OrthogonalSegment;
                        UpdateNextSegment(current, nextSegment, tx, ty);
                    }
                }
            }
        }

        private int AddTerminalSegment(Connector connector, OrthogonalSegment segment, int segmentIndex)
        {
            int index = connector.SegmentCollection.IndexOf(segment);
            double len; Direction dir; DiagramObjectCollection<ConnectorSegment> segments = new DiagramObjectCollection<ConnectorSegment>();
            OrthogonalSegment removeSegment = connector.SegmentCollection[^1] as OrthogonalSegment;
            connector.SegmentCollection.RemoveAt(connector.SegmentCollection.Count - 1);
            OrthogonalSegment last = (connector.SegmentCollection.Count > 0) ? connector.SegmentCollection[^1] as OrthogonalSegment : null;
            OrthogonalSegment first = (last != null && last.Type == ConnectorSegmentType.Orthogonal) ? last : null;
            for (int i = 0; i < segment.Points.Count - 2; i++)
            {
                len = DiagramPoint.DistancePoints(segment.Points[i], segment.Points[i + 1]);
                dir = DiagramPoint.Direction(segment.Points[i], segment.Points[i + 1]);
                OrthogonalSegment insertseg = new OrthogonalSegment() { Type = ConnectorSegmentType.Orthogonal, Length = len, Direction = dir };
                segments.Add(insertseg);
                first = insertseg;
            }
            int sec = segmentIndex;
            if (segment.Points.Count == 2 || sec == segment.Points.Count - 2)
            {
                if (first != null)
                {
                    first.Length += 5;
                }
                if (sec != -1)
                {
                    len = 2 * DiagramPoint.DistancePoints(segment.Points[^2], segment.Points[^1]) / 3;
                    dir = DiagramPoint.Direction(segment.Points[^2], segment.Points[^1]);
                    OrthogonalSegment newSegment = new OrthogonalSegment() { Type = ConnectorSegmentType.Orthogonal, Length = len, Direction = dir };
                    segments.Add(newSegment);
                }
            }
            OrthogonalSegment lastSegment = new OrthogonalSegment() { Type = ConnectorSegmentType.Orthogonal };
            segments.Add(lastSegment);
            SegmentCollectionChangeEventArgs args = new SegmentCollectionChangeEventArgs()
            {
                Element = connector,
                Cancel = false,
                AddedSegments = segments,
                Type = CollectionChangedAction.Add
            };
            CommandHandler.InvokeDiagramEvents(DiagramEvent.SegmentCollectionChange, args);
            if (!args.Cancel)
            {
                for (int i = 0; i < segments.Count; i++)
                {
                    connector.SegmentCollection.Add(segments[i]);
                }
                index += segmentIndex;
                List<DiagramPoint> points = connector.GetConnectorPoints();
                DiagramUtil.UpdateConnector(connector, points.Count > 0 ? points : connector.IntermediatePoints);
                connector.Wrapper.Measure(new DiagramSize());
                connector.Wrapper.Arrange(connector.Wrapper.DesiredSize);
                CommandHandler.RefreshDiagram();
            }
            else
            {
                connector.SegmentCollection.Add(removeSegment);
            }
            return index;
        }

        private void UpdatePortSegment(OrthogonalSegment prev, Connector connector, int index)
        {
            if (index == 1 && prev.Points.Count == 2 && prev.Length < 0)
            {
                Corners source = connector.SourceWrapper.Corners;
                OrthogonalSegment current = connector.SegmentCollection[index] as OrthogonalSegment;
                DiagramObjectCollection<ConnectorSegment> segment = new DiagramObjectCollection<ConnectorSegment>();
                OrthogonalSegment newSegment = new OrthogonalSegment() { Type = ConnectorSegmentType.Orthogonal, Length = 13, Direction = prev.Direction };
                segment.Add(newSegment);
                double len;
                if (current != null && current.Direction == Direction.Left)
                {
                    len = (current.Points[0].X - (source.MiddleLeft.X - 20));
                }
                else if (current != null && current.Direction == Direction.Right)
                {
                    len = ((source.MiddleRight.X + 20) - current.Points[0].X);
                }
                else if (current.Direction == Direction.Bottom)
                {
                    len = ((source.BottomCenter.Y + 20) - current.Points[0].Y);
                }
                else
                {
                    len = (current.Points[0].Y - (source.TopCenter.Y - 20));
                }
                newSegment = new OrthogonalSegment() { Type = ConnectorSegmentType.Orthogonal, Length = len, Direction = current.Direction };
                segment.Add(newSegment); current.Length -= len;
                if (connector.SegmentCollection[index + 1] is OrthogonalSegment next)
                {
                    if (next.Length.HasValue && next.Direction.HasValue)
                    {
                        if (next.Direction == prev.Direction)
                        {
                            next.Length -= 13;
                        }
                        else if (prev.Direction != null && next.Direction == ConnectorUtil.GetOppositeDirection(prev.Direction.Value))
                        {
                            next.Length += 13;
                        }
                    }
                }
                connector.SegmentCollection = segment.Concat(connector.SegmentCollection);
                this.selectedSegment = connector.SegmentCollection[3];
            }
        }

        private bool UpdatePreviousSegment(double tx, double ty, Connector connector, int index)
        {
            bool isNextUpdate = true;
            if (connector.SegmentCollection[index] is OrthogonalSegment current)
            {
                if (connector.SegmentCollection[index - 1] is OrthogonalSegment prev)
                {
                    prev.Points[^1] = current.Points[0];
                    bool isSourceNode = string.IsNullOrEmpty(connector.SourceID) ||
                                        !string.IsNullOrEmpty(connector.SourcePortID);
                    if (prev.Type == ConnectorSegmentType.Orthogonal)
                    {
                        if (prev.Direction == Direction.Bottom)
                        {
                            prev.Length += ty;
                        }
                        else if (prev.Direction == Direction.Top)
                        {
                            prev.Length -= ty;
                        }
                        else if (prev.Direction == Direction.Right)
                        {
                            prev.Length += tx;
                        }
                        else
                        {
                            prev.Length -= tx;
                        }

                        if (!string.IsNullOrEmpty(connector.SourcePortID) && prev.Length < 0)
                        {
                            this.UpdatePortSegment(prev, connector, index);
                        }
                        else if (!string.IsNullOrEmpty(connector.SourceID) &&
                                 string.IsNullOrEmpty(connector.SourcePortID) && prev.Length < 0 && index == 1)
                        {
                            isNextUpdate = false;
                            this.UpdateFirstSegment(connector, current);
                        }

                        if (isSourceNode)
                        {
                            ChangeSegmentDirection(prev);
                        }
                    }
                }
            }
            return isNextUpdate;
        }

        private static void ChangeSegmentDirection(OrthogonalSegment segment)
        {
            if (segment.Length < 0)
            {
                if (segment.Direction != null)
                    segment.Direction = ConnectorUtil.GetOppositeDirection(segment.Direction.Value);
                segment.Length *= -1;
            }
        }

        private static void UpdateNextSegment(OrthogonalSegment current, OrthogonalSegment next, double tx, double ty)
        {
            DiagramPoint pt = current.Points[^1]; next.Points[0] = current.Points[^1];
            if (next != null && next.Type == ConnectorSegmentType.Orthogonal)
            {
                if (next.Length.HasValue)
                {
                    if (next.Direction == Direction.Left || next.Direction == Direction.Right)
                    {
                        if (tx != 0)
                        {
                            next.Length = (next.Direction == Direction.Right) ? next.Length - tx : next.Length + tx;
                            if (next.Length.HasValue)
                            {
                                ChangeSegmentDirection(next);
                            }
                        }
                    }
                    else
                    {
                        if (ty != 0)
                        {
                            next.Length = (next.Direction == Direction.Bottom) ? next.Length - ty : next.Length + ty;
                            if (next.Length.HasValue)
                            {
                                ChangeSegmentDirection(next);
                            }
                        }
                    }
                }
            }
        }

        private void UpdateFirstSegment(Connector connector, OrthogonalSegment selectedSegment)
        {
            int index = connector.SegmentCollection.IndexOf(selectedSegment); bool insertFirst = false;
            OrthogonalSegment current = connector.SegmentCollection[index] as OrthogonalSegment;
            OrthogonalSegment prev = connector.SegmentCollection[index - 1] as OrthogonalSegment;
            if (prev.Length < 0 && !string.IsNullOrEmpty(connector.SourceID))
            {
                Corners sourceNode = (connector).SourceWrapper.Corners;
                DiagramObjectCollection<ConnectorSegment> segments = new DiagramObjectCollection<ConnectorSegment>();
                OrthogonalSegment segValues; bool removeCurrentPrev = false;
                ChangeSegmentDirection(current);
                OrthogonalSegment next = (connector.SegmentCollection.Count > index + 1) ? connector.SegmentCollection[index + 1] as OrthogonalSegment : null;
                OrthogonalSegment nextNext = (connector.SegmentCollection.Count > index + 2) ? connector.SegmentCollection[index + 2] as OrthogonalSegment : null;
                if (next != null)
                {
                    ChangeSegmentDirection(next);
                }
                if (nextNext != null)
                {
                    ChangeSegmentDirection(nextNext);
                }

                DiagramPoint sourcePoint;
                switch (prev.Direction)
                {
                    case Direction.Top:
                    case Direction.Bottom:
                        sourcePoint = current != null && (current.Length > 0 && current.Direction == Direction.Left) ? sourceNode.MiddleLeft : sourceNode.MiddleRight;
                        if (current.Length > sourceNode.Width / 2)
                        {
                            if (Math.Abs(prev.Length.Value) < sourceNode.Height / 2)
                            {
                                prev.Length = DiagramPoint.DistancePoints(sourceNode.Center, prev.Points[^1]);
                                current.Points[0].X = sourcePoint.X;
                                current.Length = DiagramPoint.DistancePoints(current.Points[0], current.Points[^1]);
                                current.Length -= 20;
                                insertFirst = true;
                            }
                        }
                        else
                        {
                            if (next != null)
                            {
                                if (next.Direction.HasValue && next.Length.HasValue)
                                {
                                    next.Points[0].Y = sourcePoint.Y;
                                    next.Points[0].X = next.Points[^1].X = (current.Direction == Direction.Right) ?
                                        sourcePoint.X + 20 : sourcePoint.X - 20;
                                }
                            }
                            insertFirst = true;
                            removeCurrentPrev = true;
                        }
                        break;
                    case Direction.Left:
                    case Direction.Right:
                        sourcePoint = current != null && (current.Length > 0 && current.Direction == Direction.Top) ? sourceNode.TopCenter : sourceNode.BottomCenter;
                        if (current.Length > sourceNode.Height / 2)
                        {
                            if (Math.Abs(prev.Length.Value) < sourceNode.Width / 2)
                            {
                                prev.Length = DiagramPoint.DistancePoints(sourceNode.Center, prev.Points[^1]);
                                current.Points[0].Y = sourcePoint.Y;
                                current.Length = DiagramPoint.DistancePoints(current.Points[0], current.Points[^1]);
                                current.Length -= 20;
                                insertFirst = true;
                            }
                        }
                        else
                        {
                            if (next != null)
                            {
                                if (next.Direction.HasValue && next.Length.HasValue)
                                {
                                    next.Points[0].X = sourcePoint.X;
                                    next.Points[0].Y = next.Points[^1].Y = (current.Direction == Direction.Bottom) ?
                                        sourcePoint.Y + 20 : sourcePoint.Y - 20;
                                }
                            }
                            insertFirst = true;
                            removeCurrentPrev = true;
                        }
                        break;
                }
                ChangeSegmentDirection(prev);
                ChangeSegmentDirection(current);
                if (insertFirst)
                {
                    segValues = new OrthogonalSegment() { Type = ConnectorSegmentType.Orthogonal, Direction = current.Direction, Length = 20 };
                    segments.Add(segValues);
                    if (removeCurrentPrev)
                    {
                        if (next != null)
                        {
                            if (next.Direction.HasValue && next.Length.HasValue)
                            {
                                next.Length = DiagramPoint.DistancePoints(next.Points[0], next.Points[^1]);
                            }
                        }
                        if (nextNext != null)
                        {
                            if (nextNext.Direction.HasValue && nextNext.Length.HasValue)
                            {
                                nextNext.Length = DiagramPoint.DistancePoints(
                                    next.Points[^1], nextNext.Points[^1]);
                            }
                        }
                        connector.SegmentCollection.RemoveAt(index - 1);
                        connector.SegmentCollection.RemoveAt(index - 1);
                    }
                    connector.SegmentCollection = segments.Concat(connector.SegmentCollection);
                }
                this.selectedSegment = ((removeCurrentPrev) ? connector.SegmentCollection[index - 1] : connector.SegmentCollection[index + 1]);
                List<DiagramPoint> points = connector.GetConnectorPoints();
                DiagramUtil.UpdateConnector(connector, points.Count > 0 ? points : connector.IntermediatePoints);
                connector.Wrapper.Measure(new DiagramSize());
                connector.Wrapper.Arrange(connector.Wrapper.DesiredSize);
                CommandHandler.RefreshDiagram();
            }
        }

        private static bool UpdateLastSegment(Connector connector, OrthogonalSegment selectedSegment)
        {
            if (!string.IsNullOrEmpty(connector.TargetID) && !string.IsNullOrEmpty(connector.TargetPortID))
            {
                DiagramPoint line1Start; DiagramPoint line1End; DiagramPoint line2Start; DiagramPoint line2End;
                Corners corners = connector.TargetWrapper.Corners;
                DiagramPoint firstSegPoint = selectedSegment.Points[0];
                DiagramPoint lastSegPoint = selectedSegment.Points[^1];

                if (selectedSegment.Direction == Direction.Right || selectedSegment.Direction == Direction.Left)
                {
                    line1Start = new DiagramPoint(firstSegPoint.X, firstSegPoint.Y);
                    line1End = new DiagramPoint()
                    {
                        X = (selectedSegment.Direction == Direction.Left) ? lastSegPoint.X - corners.Width / 2 : lastSegPoint.X + corners.Width / 2,
                        Y = lastSegPoint.Y
                    };
                    line2Start = new DiagramPoint(corners.Center.X, corners.Center.Y - corners.Height);
                    line2End = new DiagramPoint(corners.Center.X, corners.Center.Y + corners.Height);
                }
                else
                {
                    line1Start = new DiagramPoint(firstSegPoint.X, firstSegPoint.Y);
                    line1End = new DiagramPoint(lastSegPoint.X,
                            (selectedSegment.Direction == Direction.Bottom) ? lastSegPoint.Y + corners.Height / 2 : lastSegPoint.Y - corners.Height / 2);
                    line2Start = new DiagramPoint(corners.Center.X - corners.Width, corners.Center.Y);
                    line2End = new DiagramPoint(corners.Center.X + corners.Width, corners.Center.Y);
                }
                Segment line1 = new Segment() { X1 = line1Start.X, Y1 = line1Start.Y, X2 = line1End.X, Y2 = line1End.Y };
                Segment line2 = new Segment() { X1 = line2Start.X, Y1 = line2Start.Y, X2 = line2End.X, Y2 = line2End.Y };
                return (DiagramUtil.Intersect3(line1, line2).Enabled);
            }
            return false;
        }
    }
}