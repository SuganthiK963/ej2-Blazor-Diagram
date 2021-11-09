using Syncfusion.Blazor.Diagram.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace Syncfusion.Blazor.Diagram
{

    /// <summary>
    /// Represents the modeling language for creating process workflows or business process flowcharts in the diagram. 
    /// </summary>
    /// <remarks>
    ///  BPMN diagram helps people to communicate process workflow design ideas effectively.
    /// </remarks>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// Node node = new Node()
    /// {
    ///     ID = "node1",          
    ///     Width = 70,
    ///     Height = 70, 
    ///     OffsetX = 100,
    ///     OffsetY = 300,
    ///     Shape = new BpmnShape() 
    ///     { 
    ///         Type = Shapes.Bpmn, 
    ///         Shape = BpmnShapes.DataObject, 
    ///         DataObject = new BpmnDataObject() 
    ///         { 
    ///             Collection = true, 
    ///             Type = BpmnDataObjects.Output 
    ///         } 
    ///     }
    /// };
    /// ]]>
    /// </code>
    /// </example>
    internal class BpmnDiagrams : NodeBase
    {
        const string LOOP = "Loop";
        const string BOUNDARY = "Boundary";
        const string TYPE = "Type";
        internal DiagramObjectCollection<Node> TextAnnotationNodes = new DiagramObjectCollection<Node>();
        private Dictionary<string, Dictionary<string, Dictionary<string, NodeBase>>> annotationObjects = new Dictionary<string, Dictionary<string, Dictionary<string, NodeBase>>>();
        /// <summary>
        /// Creates a new instance of the BpmnDiagrams from the given BpmnDiagrams.
        /// </summary>
        /// <param name="src">BpmnDiagrams</param>
        public BpmnDiagrams(BpmnDiagrams src) : base(src)
        {
            if (src != null)
            {
                TextAnnotationNodes = new DiagramObjectCollection<Node>();
                if (src.TextAnnotationNodes.Count > 0)
                {
                    foreach (Node node in src.TextAnnotationNodes)
                    {
                        Node node1 = node.Clone() as Node;
                        TextAnnotationNodes.Add(node1);
                    }
                }
                TextAnnotationNodes.Parent = this;
            }
        }
        /// <summary>
        /// Initializes a new instance of the BpmnDiagrams.
        /// </summary>
        public BpmnDiagrams() : base()
        {
        }
        internal DiagramElement InitBpmnContent(DiagramElement content, Node node)
        {
            BpmnShape shape = node.Shape as BpmnShape;
            if (shape != null)
            {
                BpmnShapes bpmnShape = shape.Shape;
                if (bpmnShape == BpmnShapes.Event)
                {
                    content = GetBpmnEventShape(node, shape.Events);
                }
                if (bpmnShape == BpmnShapes.Gateway)
                {
                    content = GetBpmnGatewayShape(node);
                }
                if (bpmnShape == BpmnShapes.Activity)
                {
                    content = GetBpmnActivityShape(node);
                }
                if (bpmnShape == BpmnShapes.DataObject)
                {
                    content = GetBpmnDataObjectShape(node);
                }
                if (bpmnShape == BpmnShapes.Message || bpmnShape == BpmnShapes.DataSource)
                {
                    content = GetBpmnShapes(node);
                }
                if (bpmnShape == BpmnShapes.TextAnnotation)
                {
                    content = RenderBpmnTextAnnotation(node);
                }
            }

            if (shape != null && shape.Annotations.Count > 0 && content is Canvas canvas)
            {
                for (int i = 0; i < shape.Annotations.Count; i++)
                {
                    canvas.Children.Add(this.GetBpmnTextAnnotation(
                        node, shape.Annotations[i], canvas));
                }
                canvas.Style.StrokeDashArray = "2 2 6 2";
            }
            return content;
        }

        internal Canvas GetBpmnTextAnnotation(Node node, BpmnAnnotation annotation, DiagramElement content)
        {
            annotation.ID = string.IsNullOrEmpty(annotation.ID) ? BaseUtil.RandomId() : annotation.ID;
            Canvas annotationsContainer = new Canvas();
            PathElement annotationPath = new PathElement();
            TextElement textElement = new TextElement();
            int margin = 10;
            annotationPath.ID = '_' + annotation.ID + "_path";
            annotationPath.Width = annotation.Width;
            annotationPath.Height = annotation.Height;
            annotationPath.RelativeMode = RelativeMode.Object;
            textElement.ID = node.ID + '_' + annotation.ID + "_text";
            textElement.Content = annotation.Text;
            if (node.Style is TextStyle textStyle)
            {
                textElement.Style = new TextStyle()
                {
                    FontSize = textStyle.FontSize,
                    Italic = textStyle.Italic,
                    Bold = textStyle.Bold,
                    TextWrapping = textStyle.TextWrapping,
                    Color = textStyle.Color,
                    FontFamily = textStyle.FontFamily,
                    WhiteSpace = textStyle.WhiteSpace,
                    TextOverflow = TextOverflow.Wrap,
                    TextAlign = textStyle.TextAlign,
                    TextDecoration = textStyle.TextDecoration,
                    Gradient = null,
                    Opacity = textStyle.Opacity,
                    Fill = "white",
                    StrokeColor = "none",
                    StrokeWidth = 0,
                    StrokeDashArray = textStyle.StrokeDashArray
                };
            }
            textElement.HorizontalAlignment = HorizontalAlignment.Left;
            textElement.VerticalAlignment = VerticalAlignment.Center;
            textElement.RelativeMode = RelativeMode.Object;
            textElement.Margin = new Margin() { Left = 5, Right = 5, Top = 5, Bottom = 5 };
            annotationsContainer.OffsetX = node.OffsetX + annotation.Length *
             Math.Cos(annotation.Angle * (Math.PI / 180));
            annotationsContainer.OffsetY = node.OffsetY + annotation.Length *
                Math.Sin(annotation.Angle * (Math.PI / 180));
            annotationsContainer.Float = true;
            annotationsContainer.Transform = Transform.Self;
            annotationsContainer.ID = node.ID + "_textannotation_" + annotation.ID;
            annotationsContainer.Style.StrokeColor = "transparent";
            annotationsContainer.Margin = new Margin() { Left = margin, Right = margin, Top = margin, Bottom = margin };
            annotationsContainer.RelativeMode = RelativeMode.Object;
            annotationsContainer.RotationAngle = 0;
            annotationsContainer.Children = new ObservableCollection<ICommonElement>() { annotationPath, textElement };
            Node annotationObject = new Node()
            {
                ID = annotationsContainer.ID,
                Shape = new BpmnShape() { Type = Shapes.Bpmn, Shape = BpmnShapes.TextAnnotation }
            };
            DiagramObjectCollection<PointPort> port = new DiagramObjectCollection<PointPort>();
            PointPort port1 = new PointPort()
            {
                ID = annotationPath.ID + "_port",
                Shape = PortShapes.Square,
                Offset = new DiagramPoint() { X = 0, Y = 0.5 }
            };
            port.Add(port1);
            annotationObject.Ports = port;
            annotationObject.OffsetX = annotationsContainer.OffsetX;
            annotationObject.OffsetY = annotationsContainer.OffsetY;
            Shape shape = ((Node)annotationObject).Shape;
            if (shape != null && shape is BpmnShape bpmnShape && bpmnShape.Annotation != null)
            {
                bpmnShape.Annotation.Text = annotation.Text;
                bpmnShape.Annotation.Angle = annotation.Angle;
                bpmnShape.Annotation.Length = annotation.Length;
            }
            annotationObject.Width = annotation.Width;
            annotationObject.Height = annotation.Height;
            annotationObject.Wrapper = annotationsContainer;
            if (annotationObject.Ports.Count > 0)
                annotationsContainer.Children.Add(annotationObject.InitPortWrapper(annotationObject.Ports[0] as Port));
            DiagramRect bounds = new DiagramRect(0, 0, 0, 0);
            double width = BaseUtil.GetDoubleValue(node.Width);
            double height = BaseUtil.GetDoubleValue(node.Height);
            if (width != 0 && height != 0)
            {
                bounds = new DiagramRect(node.OffsetX - width / 2, node.OffsetY - height / 2, width, height);
            }
            SetAnnotationPath(
             bounds, annotationsContainer, new DiagramPoint() { X = annotationObject.OffsetX, Y = annotationObject.OffsetY }, annotationObject,
                ((BpmnShape)node.Shape).Annotation.Length, node.RotationAngle);
            Connector annotationConnector = new Connector
            {
                ID = node.ID + '_' + annotation.ID + "_connector",
                Constraints = ConnectorConstraints.Default & ~(ConnectorConstraints.DragTargetEnd | ConnectorConstraints.DragSourceEnd | ConnectorConstraints.Drag),
                SourceID = node.ID,
                TargetID = annotationsContainer.ID,
                Parent = node.Shape,
                TargetDecorator = new DecoratorSettings { Shape = DecoratorShape.None },
                TargetPortID = annotationObject.Ports[0].ID
            };
            annotationConnector.Init();
            annotationConnector.Wrapper.Float = false;
            annotationConnector.Wrapper.Transform = Transform.Self;
            (content as Canvas)?.Children.Add(annotationConnector.Wrapper);
            ((SfDiagramComponent)node.Parent).DiagramContent.TextAnnotationConnectors.Add(annotationConnector);
            this.TextAnnotationNodes.Add(annotationObject);
            annotationConnector.ZIndex = 10000;
            ((SfDiagramComponent)node.Parent).NameTable.Add(annotationObject.ID, annotationObject);
            annotationObject.ZIndex = 10000;
            string nodeKey = "node";
            string connKey = "connector";

            if (!annotationObjects.ContainsKey(node.ID))
            {
                Dictionary<string, Dictionary<string, NodeBase>> dicEntry = new Dictionary<string, Dictionary<string, NodeBase>>
                {{annotation.ID, new Dictionary<string, NodeBase>()
                {
                    { nodeKey, annotationObject},
                    { connKey, annotationConnector}
                }}};
                annotationObjects.Add(node.ID, dicEntry);
            }
            else
            {
                annotationObjects[node.ID].Add(annotation.ID, new Dictionary<string, NodeBase>()
                {
                    { nodeKey, annotationObject},
                    { connKey, annotationConnector}
                });
            }
            return annotationsContainer;
        }
        internal bool UpdateAnnotationDrag(Node node, SfDiagramComponent diagram, double tx, double ty)
        {
            if (!string.IsNullOrEmpty((node as Node).ProcessId))
            {
                this.Drag(node as Node, tx, ty, diagram);
                return true;
            }
            return false;
        }
        internal void Drag(Node obj, double tx, double ty, SfDiagramComponent diagram)
        {
            Node node = diagram.NameTable[(obj).ProcessId] as Node;
            if (obj.Margin.Top + ty >= 0)
            {
                diagram.IsBeginUpdate = false;
                diagram.OnPropertyChanged("Top", obj.Margin.Top + ty, obj.Margin.Top, obj.Margin);
                diagram.IsBeginUpdate = true;
            }
            if (obj.Margin.Left + tx >= 0)
            {
                diagram.IsBeginUpdate = false;
                diagram.OnPropertyChanged("Left", obj.Margin.Left + tx, obj.Margin.Left, obj.Margin);
                diagram.IsBeginUpdate = true;
            }
            _ = GetChildrenBound(node, obj.ID, diagram);
            UpdateSubProcesses(obj, diagram);
            node.Wrapper.Measure(new DiagramSize());
            node.Wrapper.Arrange(node.Wrapper.DesiredSize);
            this.UpdateDocks(obj as Node, diagram);
        }
        internal void UpdateDocks(Node obj, SfDiagramComponent diagram)
        {
            BpmnSubProcess subProcess = obj.Shape.Type == Shapes.Bpmn ? ((BpmnShape)obj.Shape).Activity.SubProcess : null;
            if (subProcess != null && obj.Shape.Type == Shapes.Bpmn && subProcess.Processes != null && !subProcess.Collapsed)
            {
                DiagramObjectCollection<string> processTable = subProcess.Processes;
                foreach (string i in processTable)
                {
                    Node actualObject = diagram.NameTable.ContainsKey(i) ? diagram.NameTable[i] as Node : null;
                    if (actualObject != null)
                    {
                        diagram.DiagramContent.UpdateConnectorEdges(actualObject);
                        actualObject.Wrapper.Measure(new DiagramSize() { Height = actualObject.Wrapper.Height, Width = actualObject.Wrapper.Width });
                        actualObject.Wrapper.Arrange(actualObject.Wrapper.DesiredSize);
                        if ((actualObject.Shape as BpmnShape)?.Activity.SubProcess.Processes != null
                            && ((BpmnShape)actualObject.Shape).Activity.SubProcess.Processes.Count > 0)
                        {
                            this.UpdateDocks(actualObject, diagram);
                        }
                    }
                }
            }
        }
        internal void AddBpmnProcesses(Node process, string parentId, SfDiagramComponent diagram)
        {
            process.ID = string.IsNullOrEmpty(process.ID) ? BaseUtil.RandomId() : process.ID;
            string id = process.ID;
            Node node = diagram.NameTable[id] as Node;
            if (node == null)
            {
                diagram.Nodes.Add(process);
            }
            (process as Node).ProcessId = parentId;
            Node parentNode = diagram.NameTable[parentId] as Node;
            BpmnSubProcess subProcess = (parentNode?.Shape as BpmnShape)?.Activity.SubProcess;
            if (node != null && parentNode != null && parentNode.Shape.Type == Shapes.Bpmn && node.Shape.Type == Shapes.Bpmn &&
                subProcess?.Processes != null)
            {
                node.ProcessId = parentId;
                DiagramObjectCollection<string> processes = subProcess.Processes;
                if (processes.IndexOf(id) < 0)
                {
                    processes.Add(id);
                }
                parentNode.Wrapper.Children.Add(node.Wrapper);
                parentNode.Wrapper.Measure(new DiagramSize());
                parentNode.Wrapper.Arrange(parentNode.Wrapper.DesiredSize);
                UpdateDocks(parentNode, diagram);
                diagram.DiagramStateHasChanged();
            }
        }
        internal void RemoveBpmnProcesses(Node currentObj, SfDiagramComponent diagram)
        {
            Node element = null;
            if (!string.IsNullOrEmpty(currentObj.ProcessId))
            {
                element = diagram.NameTable[currentObj.ProcessId] as Node;
            }
            BpmnSubProcess subProcess = (currentObj.Shape as BpmnShape)?.Activity.SubProcess;
            if (currentObj.Shape.Type == Shapes.Bpmn && subProcess?.Processes != null &&
                subProcess.Processes.Count > 0)
            {
                DiagramObjectCollection<string> processes = subProcess.Processes;
                for (int j = processes.Count - 1; j >= 0; j--)
                {
                    diagram.Nodes.Remove(diagram.NameTable[processes[j]] as Node);
                }
            }
            if (element != null)
            {
                diagram.RemoveDependentConnector(currentObj);
                this.RemoveChildFromBpmn(element.Wrapper, currentObj.ID, diagram);
            }
        }
        internal void RemoveChildFromBpmn(DiagramContainer wrapper, string name, SfDiagramComponent diagram)
        {
            for (int i = 0; i < wrapper.Children.Count; i++)
            {
                if (wrapper.Children[i].ID == name)
                {
                    wrapper.Children.Remove(wrapper.Children[i]);
                }
                else if (wrapper.Children[i] is DiagramContainer container && container.Children != null)
                {
                    this.RemoveChildFromBpmn((wrapper.Children[i] as DiagramContainer), name, diagram);
                }
            }
            (diagram.NameTable[name] as Node).ProcessId = "";
        }
        private static Canvas RenderBpmnTextAnnotation(Node annotation)
        {
            annotation.ID = string.IsNullOrEmpty(annotation.ID) ? BaseUtil.RandomId() : annotation.ID;
            Canvas annotationsContainer = new Canvas();
            PathElement annotationPath = new PathElement();
            TextElement textObject = new TextElement();

            int margin = 10;

            annotationPath.ID = '_' + annotation.ID + "_path";
            annotationPath.Width = annotation.Width;
            annotationPath.Height = annotation.Height;
            annotationPath.RelativeMode = RelativeMode.Object;
            textObject.ID = annotation.ID + '_' + (annotation.Shape as BpmnShape)?.Annotation.ID + "_text";
            textObject.Content = (annotation.Shape as BpmnShape)?.Annotation.Text;
            if ((annotation as Node).Style is TextStyle)
            {
                if (annotation.Style is TextStyle textStyle)
                    textObject.Style = new TextStyle()
                    {
                        FontSize = textStyle.FontSize,
                        Italic = textStyle.Italic,
                        Bold = textStyle.Bold,
                        TextWrapping = textStyle.TextWrapping,
                        Color = textStyle.Color,
                        FontFamily = textStyle.FontFamily,
                        WhiteSpace = textStyle.WhiteSpace,
                        TextOverflow = TextOverflow.Wrap,

                        TextAlign = textStyle.TextAlign,
                        TextDecoration = textStyle.TextDecoration,
                        Gradient = null,
                        Opacity = textStyle.Opacity,
                        Fill = "white",
                        StrokeColor = "none",
                        StrokeWidth = 0,
                        StrokeDashArray = textStyle.StrokeDashArray
                    };
            }
            textObject.HorizontalAlignment = HorizontalAlignment.Left;
            textObject.VerticalAlignment = VerticalAlignment.Center;
            textObject.RelativeMode = RelativeMode.Object;
            textObject.Margin = new Margin() { Left = 5, Right = 5, Top = 5, Bottom = 5 };

            annotationsContainer.OffsetX = (annotation as Node).OffsetX + ((BpmnShape)(annotation as Node).Shape).Annotation.Length *
                Math.Cos(((BpmnShape)(annotation as Node).Shape).Annotation.Angle * (Math.PI / 180));
            annotationsContainer.OffsetY = (annotation as Node).OffsetY + ((BpmnShape)(annotation as Node).Shape).Annotation.Length *
                Math.Sin(((BpmnShape)(annotation as Node).Shape).Annotation.Angle * (Math.PI / 180));
            annotationsContainer.Float = true;
            annotationsContainer.ID = (annotation as Node).ID + "_textannotation_" + annotation.ID;
            annotationsContainer.Style.StrokeColor = "transparent";
            annotationsContainer.Margin = new Margin() { Left = margin, Right = margin, Top = margin, Bottom = margin };
            annotationsContainer.RelativeMode = RelativeMode.Object;
            annotationsContainer.RotationAngle = 0;
            annotationsContainer.Children = new ObservableCollection<ICommonElement>() { annotationPath, textObject };

            Node annotationObject = new Node()
            {
                ID = annotationsContainer.ID,
                Shape = new BpmnShape() { Type = Shapes.Bpmn, Shape = BpmnShapes.TextAnnotation }
            };
            DiagramObjectCollection<PointPort> port = new DiagramObjectCollection<PointPort>();
            PointPort port1 = new PointPort()
            {
                ID = annotationPath.ID + "_port",
                Shape = PortShapes.Square,
                Offset = new DiagramPoint() { X = 0, Y = 0.5 }
            };
            port.Add(port1);
            annotationObject.Ports = port;
            annotationObject.OffsetX = annotationsContainer.OffsetX;
            annotationObject.OffsetY = annotationsContainer.OffsetY;
            if (annotationObject.Shape is BpmnShape bpmnShape && bpmnShape.Annotation != null)
            {
                bpmnShape.Annotation.Text = bpmnShape.Annotation.Text;
                bpmnShape.Annotation.Angle = bpmnShape.Annotation.Angle;
                bpmnShape.Annotation.Length = bpmnShape.Annotation.Length;
            }
            annotationObject.Width = annotation.Width;
            annotationObject.Height = annotation.Height;
            annotationObject.Wrapper = annotationsContainer;
            if (annotationObject.Ports.Count > 0)
                annotationsContainer.Children.Add(annotationObject.InitPortWrapper(annotationObject.Ports[0] as Port));
            DiagramRect bounds = new DiagramRect(0, 0, 0, 0);
            double width = BaseUtil.GetDoubleValue((annotation as Node).Width);
            double height = BaseUtil.GetDoubleValue((annotation as Node).Height);
            if (width != 0 && height != 0)
            {
                bounds = new DiagramRect((annotation as Node).OffsetX - width / 2, (annotation as Node).OffsetY - height / 2, width, height);
            }
            SetAnnotationPath(
             bounds, annotationsContainer, new DiagramPoint() { X = annotationObject.OffsetX, Y = annotationObject.OffsetY }, annotationObject,
                ((BpmnShape)annotation.Shape).Annotation.Length, annotation.RotationAngle);
            return annotationsContainer;
        }
        private TextElement GetTextAnnotationWrapper(Node node, string id)
        {
            if (node != null && node.Shape.Type == Shapes.Bpmn)
            {
                BpmnShapes shape = ((BpmnShape)node.Shape).Shape;
                if (shape == BpmnShapes.TextAnnotation)
                {
                    return node.Wrapper.Children[1] as TextElement;
                }
                else if (this.annotationObjects[node.ID] != null && this.annotationObjects[node.ID][id] != null)
                {
                    Node annotationNode = this.annotationObjects[node.ID][id]["node"] as Node;
                    return this.GetTextAnnotationWrapper(annotationNode, id);
                }
            }
            return null;
        }
        internal Connector AddAnnotation(Node node, BpmnAnnotation annotation)
        {
            Canvas bpmnShapeContent = node.Wrapper.Children[0] as Canvas;
            BpmnShape shape = node.Shape as BpmnShape;
            (annotation as BpmnAnnotation).NodeId = node.ID;
            BpmnAnnotation annotationObj = new BpmnAnnotation(annotation)
            {
                Parent = shape,
                PropertyName = "annotations"
            };
            shape?.Annotations.Add(annotationObj);
            bpmnShapeContent?.Children.Add(
                this.GetBpmnTextAnnotation(node as Node, annotation, bpmnShapeContent));
            node.Wrapper.Measure(new DiagramSize());
            node.Wrapper.Arrange(node.Wrapper.DesiredSize);
            return this.annotationObjects[node.ID][annotation.ID]["connector"] as Connector;
        }

        private void ClearAnnotations(Node obj, SfDiagramComponent diagram)
        {
            if (obj.Shape is BpmnShape bpmnShape && bpmnShape.Annotations.Count > 0)
            {
                for (int i = bpmnShape.Annotations.Count - 1; i >= 0; i--)
                {
                    BpmnAnnotation annotation = bpmnShape.Annotations[i];
                    this.RemoveAnnotationObjects(obj, annotation, diagram);
                }
            }
            this.annotationObjects.Remove(obj.ID);
        }
        /// <summary>
        /// This method is used to remove annotation.
        /// </summary>
        /// <param name="node">Node</param>
        /// <param name="diagram">SfDiagramComponent</param>
        /// <returns></returns>
        internal bool CheckAndRemoveAnnotations(Node node, SfDiagramComponent diagram)
        {
            //remove connector path
            //remove annotation node wrapper
            //remove from a quad
            if (node != null && diagram != null && node.Shape.Type == Shapes.Bpmn)
            {
                if (((BpmnShape)node.Shape).Shape == BpmnShapes.TextAnnotation)
                {
                    string[] id = node.ID.Split("_");
                    string annotationId = id[^1];
                    string nodeId = !string.IsNullOrEmpty(id[^3]) ? id[^3] : id[0];
                    if (diagram.NameTable[nodeId] is Node parentNode)
                    {
                        if (parentNode.Shape is BpmnShape bpmnShape)
                            foreach (BpmnAnnotation annotation in bpmnShape.Annotations)
                            {
                                if (annotation.ID == annotationId)
                                {
                                    int index = bpmnShape.Annotations.IndexOf(annotation);
                                    if (index != -1)
                                    {
                                        diagram.RemoveDependentConnector(node as Node);
                                        this.RemoveAnnotationObjects(parentNode, annotation, diagram);
                                        return true;
                                    }
                                }
                            }
                    }
                }
                else if ((node.Shape as BpmnShape)?.Annotations != null && ((BpmnShape)node.Shape).Annotations.Count > 0)
                {
                    this.ClearAnnotations(node, diagram);
                }
            }
            return false;
        }

        private void RemoveAnnotationObjects(Node parentNode, BpmnAnnotation annotation, SfDiagramComponent diagram)
        {
            if (parentNode.Shape is BpmnShape bpmnShape)
            {
                int index = bpmnShape.Annotations.IndexOf(annotation);
                if (index != -1)
                {
                    bpmnShape.Annotations.RemoveAt(index);
                    Dictionary<string, Dictionary<string, NodeBase>> entryVal = this.annotationObjects[parentNode.ID];
                    if (entryVal?[annotation.ID] != null)
                    {
                        Node annotationNode = entryVal[annotation.ID]["node"] as Node;
                        Connector annotationConnector = entryVal[annotation.ID]["connector"] as Connector;
                        diagram.UpdateNameTable(new List<NodeBase>() { annotationNode, annotationConnector });
                        entryVal.Remove(annotation.ID);
                    }
                }
            }
        }
        private static void SetAnnotationPath(DiagramRect parentBounds, DiagramElement wrapper, DiagramPoint position, Node node, int length,
               double angle = 0)
        {
            int rotateAngle = GetAnnotationPathAngle(position, parentBounds);
            DiagramElement portElement = new DiagramElement();
            DiagramElement textElement = new DiagramElement();
            if (((Canvas)wrapper).Children.Count > 2)
            {
                portElement = (wrapper as Canvas)?.Children[2] as DiagramElement;
            }
            if (((Canvas)wrapper).Children.Count > 1)
            {
                textElement = ((Canvas)wrapper)?.Children[1] as DiagramElement;
            }

            if ((wrapper as Canvas)?.Children[0] is PathElement pathElement)
            {
                pathElement.HorizontalAlignment = HorizontalAlignment.Stretch;
                pathElement.VerticalAlignment = VerticalAlignment.Stretch;
                if (textElement != null && portElement != null)
                {
                    textElement.Margin.Left = textElement.Margin.Right = 5;
                    textElement.Margin.Top = textElement.Margin.Bottom = 5;
                    Segment segment;
                    string data;
                    switch (rotateAngle)
                    {
                        case 0:
                            data = "M10,20 L0,20 L0,0 L10,0";
                            pathElement.Width = 10;
                            pathElement.HorizontalAlignment = HorizontalAlignment.Left;
                            portElement.SetOffsetWithRespectToBounds(0, 0.5, UnitMode.Fraction);
                            textElement.Margin.Top = textElement.Margin.Bottom = 10;
                            segment = new Segment()
                            {
                                X1 = parentBounds.Right,
                                Y1 = parentBounds.Top,
                                X2 = parentBounds.Right,
                                Y2 = parentBounds.Bottom
                            };
                            break;
                        case 180:
                            data = "M0,0 L10,0 L10,20 L0,20";
                            pathElement.Width = 10;
                            pathElement.HorizontalAlignment = HorizontalAlignment.Right;
                            portElement.SetOffsetWithRespectToBounds(1, 0.5, UnitMode.Fraction);
                            textElement.Margin.Top = textElement.Margin.Bottom = 10;
                            segment = new Segment()
                            {
                                X1 = parentBounds.Left,
                                Y1 = parentBounds.Top,
                                X2 = parentBounds.Left,
                                Y2 = parentBounds.Bottom
                            };
                            break;
                        case 90:
                            data = "M20,10 L20,0 L0,0 L0,10";
                            pathElement.Height = 10;
                            pathElement.VerticalAlignment = VerticalAlignment.Top;
                            portElement.SetOffsetWithRespectToBounds(0.5, 0, UnitMode.Fraction);
                            textElement.Margin.Left = textElement.Margin.Right = 10;
                            segment = new Segment()
                            {
                                X1 = parentBounds.Right,
                                Y1 = parentBounds.Bottom,
                                X2 = parentBounds.Left,
                                Y2 = parentBounds.Bottom
                            };
                            break;
                        default:
                            data = "M0,0 L0,10 L20,10 L20,0";
                            pathElement.Height = 10;
                            pathElement.VerticalAlignment = VerticalAlignment.Bottom;
                            portElement.SetOffsetWithRespectToBounds(0.5, 1, UnitMode.Fraction);
                            textElement.Margin.Left = textElement.Margin.Right = 10;
                            segment = new Segment()
                            {
                                X1 = parentBounds.Right,
                                Y1 = parentBounds.Top,
                                X2 = parentBounds.Left,
                                Y2 = parentBounds.Top,
                            };
                            break;
                    }
                    DiagramPoint center = parentBounds.Center;
                    DiagramPoint endPoint = DiagramPoint.Transform(position, angle, Math.Max(parentBounds.Width, parentBounds.Height));
                    List<DiagramPoint> pts = new List<DiagramPoint>
                    {
                        center,
                        endPoint
                    };
                    DiagramPoint point = ConnectorUtil.GetIntersectionPoints(segment, pts, center);
                    pathElement.Data = data;
                    if (length != 0 && angle != 0)
                    {
                        point = DiagramPoint.Transform(point, angle, length);
                        wrapper.OffsetX = node.OffsetX = point.X;
                        wrapper.OffsetY = node.OffsetY = point.Y;
                    }
                }
            }
        }
        private static int GetAnnotationPathAngle(DiagramPoint point, DiagramRect bounds)
        {
            Direction direction = ConnectorUtil.GetPortDirection(point, bounds, bounds);
            int rotateAngle = direction switch
            {
                Direction.Right => 0,
                Direction.Left => 180,
                Direction.Bottom => 90,
                Direction.Top => 270,
                _ => 0
            };
            return rotateAngle;
        }
        private static Canvas GetBpmnDataObjectShape(Node node)
        {
            Canvas dataObjectShape = new Canvas();
            BpmnDataObject shape = (node.Shape as BpmnShape)?.DataObject;
            //childNode0
            PathElement dataObjNode = new PathElement
            {
                ID = node.ID + "_0_dataobj",
                Data = Dictionary.GetBpmnShapePathData("DataObject")
            };
            DiagramSize size = GetSize(node, dataObjNode);
            dataObjNode.Width = size.Width; dataObjNode.Height = size.Height;
            SetStyle(dataObjNode, node);
            //childNode1
            PathElement dataObjTypeNode = new PathElement
            {
                ID = node.ID + "_1_type",
                Width = 25,
                Height = 20,
                Margin =
                {
                    Left = 5,
                    Top = 5
                },
                Data = Dictionary.GetBpmnShapePathData("Input"),
                Style =
                {
                    //set style - opacity
                    Opacity = node.Style.Opacity
                }
            };
            //childNode2
            PathElement dataObjCollectionNode = new PathElement
            {
                ID = node.ID + "_2_collection",
                Width = 7.5,
                Height = 15,
                Style =
                {
                    Fill = "black",
                    //set style - opacity
                    Opacity = node.Style.Opacity
                },
                Visible = true,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                RelativeMode = RelativeMode.Object,
                Data = Dictionary.GetBpmnShapePathData("Output")
            };

            if (shape != null)
            {
                switch (shape.Type)
                {
                    case BpmnDataObjects.None:
                        dataObjTypeNode.Visible = false;
                        break;
                    case BpmnDataObjects.Input:
                        dataObjTypeNode.Style.Fill = "white";
                        break;
                    case BpmnDataObjects.Output:
                        dataObjTypeNode.Style.Fill = "black";
                        break;
                }

                if (shape.Collection == false)
                {
                    dataObjCollectionNode.Visible = false;
                }
            }

            //append child and set style
            dataObjectShape.Style.Fill = node.Style.Fill;
            dataObjectShape.Style.StrokeColor = "transparent"; dataObjectShape.Style.StrokeWidth = 0;
            dataObjectShape.Children = new ObservableCollection<ICommonElement>() { dataObjNode, dataObjTypeNode, dataObjCollectionNode };
            return dataObjectShape;
        }
        private static Canvas GetBpmnShapes(Node node)
        {
            PathElement bpmnShape = new PathElement();
            //set style
            SetStyle(bpmnShape, node);
            if ((node.Constraints & NodeConstraints.Shadow) != 0)
            {
                bpmnShape.Shadow = node.Shadow;
            }
            string bpmnShapeData = Dictionary.GetBpmnShapePathData((node.Shape as BpmnShape)?.Shape.ToString());
            bpmnShape.Data = bpmnShapeData;
            bpmnShape.ID = node.ID + '_' + ((BpmnShape)node.Shape).Shape;
            if (node.Width != null && node.Height != null)
            {
                bpmnShape.Width = node.Width;
                bpmnShape.Height = node.Height;
            }
            Canvas canvas = new Canvas
            {
                Style =
                {
                    Fill = node.Style.Fill,
                    StrokeColor = "transparent",
                    StrokeWidth = 0
                },
                Children = new ObservableCollection<ICommonElement>() { bpmnShape }
            };
            return canvas;
        }
        private static Canvas GetBpmnEventShape(Node node, BpmnSubEvent subEvent, bool sub = false, string id = "")
        {
            Canvas eventShape = new Canvas();
            BpmnEvents events = BpmnEvents.Start;
            BpmnTriggers trigger = BpmnTriggers.None;
            id = string.IsNullOrEmpty(id) ? node.ID : id;
            string pathData = "M164.1884,84.6909000000001C156.2414,84.6909000000001,149.7764,78.2259000000001,149.7764,70.2769000000001C149.7764,62.3279000000001,156.2414,55.8629000000001,164.1884,55.8629000000001C172.1354,55.8629000000001,178.6024,62.3279000000001,178.6024,70.2769000000001C178.6024,78.2259000000001,172.1354,84.6909000000001,164.1884,84.6909000000001";
            BpmnSubEvent shapeEvent = ((BpmnShape)node.Shape).Events;
            if (((BpmnShape)node.Shape).Shape == BpmnShapes.Event)
            {
                events = shapeEvent.Event;
                trigger = shapeEvent.Trigger;
            }
            int width = Convert.ToInt32(BaseUtil.GetDoubleValue(subEvent.Width));
            int height = Convert.ToInt32(BaseUtil.GetDoubleValue(subEvent.Height));
            if (sub)
            {
                width |= 20;
                height |= 20;
            }
            else if (subEvent.Width == null || subEvent.Height == null)
            {
                PathElement pathElement = new PathElement
                {
                    Data = pathData
                };
                DiagramSize size = GetSize(node, pathElement);
                width = Convert.ToInt32(BaseUtil.GetDoubleValue(size.Width));
                height = Convert.ToInt32(BaseUtil.GetDoubleValue(size.Height));
            }
            if (((BpmnShape)node.Shape).Shape == BpmnShapes.Activity)
            {
                events = subEvent.Event;
                trigger = subEvent.Trigger;
            }
            //childNode0
            PathElement innerEvtNode = new PathElement
            {
                Data = Dictionary.GetBpmnTriggerShapePathData("DefaultEvent"),
                ID = id + "_0_event",
                Width = width,
                Height = height,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                RelativeMode = RelativeMode.Object
            };
            SetStyle(innerEvtNode, node);
            //childNode1
            PathElement outerEvtNode = new PathElement
            {
                Data = Dictionary.GetBpmnTriggerShapePathData("DefaultEvent"),
                ID = id + "_1_event",
                Style =
                {
                    Gradient = node.Style.Gradient,
                    // set style opacity & strokeColor
                    StrokeColor = node.Style.StrokeColor == "transparent" ? "black" : node.Style.StrokeColor,
                    Opacity = node.Style.Opacity
                },
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                RelativeMode = RelativeMode.Object
            };
            //childNode2
            PathElement triggerNode = new PathElement();
            string shapeName = trigger.ToString();
            string triggerNodeData = Dictionary.GetBpmnTriggerShapePathData(shapeName);
            triggerNode.Data = triggerNodeData; triggerNode.ID = id + "_2_trigger";
            triggerNode.HorizontalAlignment = HorizontalAlignment.Center;
            triggerNode.VerticalAlignment = VerticalAlignment.Center;
            triggerNode.RelativeMode = RelativeMode.Object;
            triggerNode.Style.StrokeColor = node.Style.StrokeColor == "transparent" ? "black" : node.Style.StrokeColor;
            triggerNode.Style.Opacity = node.Style.Opacity;

            switch (events)
            {
                case BpmnEvents.Start:
                    outerEvtNode.Visible = false;
                    break;
                case BpmnEvents.NonInterruptingStart:
                    innerEvtNode.Style.StrokeDashArray = "2 3";
                    outerEvtNode.Visible = false;
                    break;
                case BpmnEvents.Intermediate:
                    innerEvtNode.Style.Fill = node.Style.Fill;
                    innerEvtNode.Style.Gradient = null;
                    break;
                case BpmnEvents.NonInterruptingIntermediate:
                    innerEvtNode.Style.Fill = node.Style.Fill;
                    innerEvtNode.Style.Gradient = null;
                    innerEvtNode.Style.StrokeDashArray = "2 3";
                    outerEvtNode.Style.StrokeDashArray = "2 3";
                    break;
                case BpmnEvents.ThrowingIntermediate:
                case BpmnEvents.End:
                    innerEvtNode.Style.Fill = events != BpmnEvents.End ? node.Style.Fill : node.Style.Fill != "white" ? node.Style.Fill : "black";
                    innerEvtNode.Style.Gradient = null;
                    triggerNode.Style.Fill = "black";
                    triggerNode.Style.StrokeColor = "white";
                    break;
            }
            //append child and set style
            eventShape.Style.Fill = node.Style.Fill;
            eventShape.Style.StrokeColor = "transparent"; eventShape.Style.StrokeWidth = 0;
            eventShape.Children = new ObservableCollection<ICommonElement>() { innerEvtNode, outerEvtNode, triggerNode };
            SetSizeForBpmnEvents(shapeEvent, eventShape, width, height);
            return eventShape;
        }
        private static void SetEventVisibility(Node node, ObservableCollection<ICommonElement> canvas)
        {
            BpmnEvents events = ((BpmnShape)node.Shape).Events.Event;
            if (canvas[1] is DiagramElement outerEvtNode && canvas[0] is DiagramElement innerEvtNode)
            {
                switch (events)
                {
                    case BpmnEvents.Start:
                        outerEvtNode.Visible = false;
                        break;
                    case BpmnEvents.NonInterruptingStart:
                        innerEvtNode.Style.StrokeDashArray = "2 3";
                        outerEvtNode.Visible = false;
                        break;
                }
            }
        }
        private static void SetSubProcessVisibility(Node node)
        {
            BpmnSubProcess subProcess = (node.Shape as BpmnShape)?.Activity.SubProcess;
            if (subProcess != null)
            {
                int eventLength = subProcess.Events.Count;
                int index = (((BpmnShape)node.Shape).Activity.SubProcess.Type == BpmnSubProcessTypes.Transaction) ? 2 : 0;

                Canvas elementWrapper = ((Canvas)node.Wrapper.Children[0]).Children[0] as Canvas;
                if (subProcess.Adhoc == false)
                {
                    if (elementWrapper != null) elementWrapper.Children[3 + index + eventLength].Visible = false;
                }
                if (subProcess.Compensation == false)
                {
                    if (elementWrapper != null) elementWrapper.Children[4 + index + eventLength].Visible = false;
                }
                if (eventLength > 0)
                {
                    for (int i = 0; i < eventLength; i++)
                    {
                        if (elementWrapper != null)
                            SetEventVisibility(node, (elementWrapper.Children[2 + i] as DiagramContainer)?.Children);
                    }
                }
            }
        }
        private static Canvas GetBpmnGatewayShape(Node node)
        {
            Canvas gatewayShape = new Canvas();
            //childNode0
            PathElement gatewayNode = new PathElement
            {
                ID = node.ID + "_0_gateway",
                OffsetX = node.OffsetX,
                OffsetY = node.OffsetY,
                Data = Dictionary.GetBpmnGatewayShapePathData("DefaultGateway")
            };
            SetStyle(gatewayNode, node);
            //childNode1
            PathElement gatewayTypeNode = new PathElement
            {
                ID = node.ID + "_1_gatewayType",
                Style =
                {
                    //set style - opacity
                    Opacity = node.Style.Opacity,
                    StrokeColor = node.Style.StrokeColor == "transparent" ? "black" : node.Style.StrokeColor
                },
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                RelativeMode = RelativeMode.Object
            };
            BpmnGateway shapeType = (node.Shape as BpmnShape)?.Gateway;
            if (shapeType != null)
            {
                string shapeName = shapeType.Type.ToString();
                string gatewayTypeNodeShapeData = Dictionary.GetBpmnGatewayShapePathData(shapeName);
                if (shapeType.Type == BpmnGateways.EventBased || shapeType.Type == BpmnGateways.ExclusiveEventBased || shapeType.Type == BpmnGateways.ParallelEventBased)
                {
                    gatewayTypeNode.Style.Fill = "white";
                }
                else { gatewayTypeNode.Style.Fill = "black"; }
                gatewayTypeNode.Data = gatewayTypeNodeShapeData;
            }

            // append child and set style
            gatewayShape.Style.Fill = node.Style.Fill;
            gatewayShape.Style.StrokeColor = "transparent"; gatewayShape.Style.StrokeWidth = 0;
            gatewayShape.Children = new ObservableCollection<ICommonElement>() { gatewayNode, gatewayTypeNode };
            DiagramSize size = GetSize(node, gatewayNode);
            SetSizeForBpmnGateway(gatewayShape, BaseUtil.GetDoubleValue(size.Width), BaseUtil.GetDoubleValue(size.Height));
            return gatewayShape;
        }
        private static Canvas GetBpmnActivityShape(Node node)
        {
            Canvas eventShape = new Canvas();
            DiagramElement content = new DiagramElement();
            BpmnActivity shape = ((BpmnShape)node.Shape).Activity;
            BpmnActivities task = shape.Activity;
            BpmnSubProcess subProcess = shape.SubProcess;
            if (task == BpmnActivities.Task)
            {
                content = GetBpmnTaskShape(node);
            }
            if (task == BpmnActivities.SubProcess && subProcess != null)
            {
                content = GetBpmnSubProcessShape(node);
            }
            content.ID = task + node.ID;
            eventShape.Children = new ObservableCollection<ICommonElement>() { content };
            eventShape.Style.Fill = node.Style.Fill;
            eventShape.Style.StrokeColor = "transparent"; eventShape.Style.StrokeWidth = 0;
            return eventShape;
        }

        internal static Canvas GetBpmnSubProcessShape(Node node)
        {
            Canvas subProcessShapes = new Canvas();
            PathElement subProcessAdhoc = new PathElement();
            BpmnActivity shape = ((BpmnShape)node.Shape).Activity;
            BpmnSubProcess subProcess = shape.SubProcess;
            int subChildCount = GetSubProcessChildCount(node);
            double x;
            DiagramElement subProcessNode = new DiagramElement
            {
                ID = node.ID + "_0_Subprocess",
                Style =
                {
                    Fill = "transparent"
                },
                CornerRadius = 10
            };
            DiagramSize size = GetSize(node, subProcessNode);
            subProcessNode.Width = size.Width; subProcessNode.Height = size.Height;
            subProcessShapes.Children = new ObservableCollection<ICommonElement>() { subProcessNode };
            if (shape.SubProcess.Type == BpmnSubProcessTypes.Transaction)
            {
                GetBpmnSubProcessTransaction(node, (BpmnShape)node.Shape, subProcessShapes);
            }
            double iconSpace = 4;
            double subChildSpace = 12;
            double childSpace = subChildCount * subChildSpace;
            double area = BaseUtil.GetDoubleValue(size.Width / 2);
            if (subChildCount == 1) { x = area - (subChildSpace * 0.5); }
            else
            {
                x = area - (childSpace / 2) - ((subChildCount - 1) * iconSpace) / 2;
            }
            //set style
            SetStyle(subProcessNode, node);
            if ((node.Constraints & NodeConstraints.Shadow) != 0)
            {
                subProcessShapes.Shadow = node.Shadow;
            }
            PathElement collapsedShape = new PathElement
            {
                ID = node.ID + "_0_collapsed",
                Width = 12,
                Height = 12,
                Style =
                {
                    Fill = "black",
                    StrokeColor = node.Style.StrokeColor
                },
                Margin =
                {
                    Bottom = 5
                },
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom
            };
            collapsedShape.SetOffsetWithRespectToBounds(0, 1, UnitMode.Fraction); collapsedShape.RelativeMode = RelativeMode.Point;
            collapsedShape.Data = Dictionary.GetBpmnShapePathData("collapsedShape");
            collapsedShape.Margin.Left = x;
            if (subProcess.Collapsed && subProcess.Processes.Count == 0)
            {
                collapsedShape.Visible = true;
            }
            else { collapsedShape.Visible = false; }
            if (collapsedShape.Visible) { x += 12 + 6; }
            subProcessShapes.Children.Add(collapsedShape);
            if (subProcess.Type == BpmnSubProcessTypes.Event)
            {
                subProcessNode.Style.StrokeWidth = 1; subProcessNode.Style.StrokeDashArray = "2 2";
                List<BpmnSubEvent> events = subProcess.Events;
                for (int i = 0; i < events.Count; i++)
                {
                    BpmnSubEvent subEvent = events[i];
                    GetBpmnSubEvent((BpmnSubEvent)subEvent, node, subProcessShapes);
                }
            }
            // set loop for sub process
            PathElement subProcessLoopShapes = GetBpmnSubProcessLoopShape(node); if (subChildCount == 1) { x = area + 8; }
            subProcessLoopShapes.Margin.Left = x;
            if (subProcessLoopShapes.Visible == true) { x += subChildSpace + iconSpace; }
            subProcessShapes.Children.Add(subProcessLoopShapes);
            // set boundary for sub process
            subProcessNode.ID = node.ID + "_boundary";
            if (subProcess.Boundary == BpmnBoundary.Default)
            {
                subProcessNode.Style.StrokeWidth = 1;
                subProcessNode.Style.StrokeDashArray = "1 0";
            }
            if (subProcess.Boundary == BpmnBoundary.Call)
            {
                subProcessNode.Style.StrokeWidth = 4;
                subProcessNode.Style.StrokeDashArray = "1 0";
            }
            if (subProcess.Boundary == BpmnBoundary.Event)
            {
                subProcessNode.Style.StrokeWidth = 1;
                subProcessNode.Style.StrokeDashArray = "2 2";
            }
            //set adhoc for sub process
            subProcessAdhoc = GetBpmnAdhocShape(node, subProcessAdhoc, subProcess); if (subChildCount == 1) { x = area + 8; }
            subProcessAdhoc.Margin.Left = x; if (subProcessAdhoc.Visible) { x += subChildSpace + iconSpace; }
            subProcessShapes.Children.Add(subProcessAdhoc);
            //set compensation for sub process
            PathElement subProcessComp = new PathElement
            {
                Visible = subProcess.Compensation
            };
            subProcessComp = GetBpmnCompensationShape(node, subProcessComp); if (subChildCount == 1) { x = area + 8; }
            subProcessComp.Margin.Left = x;
            subProcessShapes.Children.Add(subProcessComp);
            //set style for sub process
            subProcessShapes.Style.StrokeColor = "transparent"; subProcessShapes.Style.StrokeWidth = 0;
            subProcessShapes.Style.Fill = "transparent";
            return subProcessShapes;
        }
        private static void GetBpmnSubEvent(BpmnSubEvent subEvent, Node node, DiagramContainer container, string id = "")
        {
            container.Children = container.Children;
            Canvas eventContainer = GetBpmnEventShape(node, subEvent, true, id);
            GetBpmnSubprocessEvent(node, eventContainer, subEvent);
            eventContainer.ID = string.IsNullOrEmpty(id) ? (node.ID + "_subprocessEvents") : id;
            eventContainer.Width = subEvent.Width ?? 20; eventContainer.Height = subEvent.Height ?? 20;
            // set offset for sub events
            eventContainer.SetOffsetWithRespectToBounds(subEvent.Offset.X, subEvent.Offset.Y, UnitMode.Fraction);
            eventContainer.RelativeMode = RelativeMode.Point;
            //set margin for sub events
            eventContainer.Margin = subEvent.Margin;
            //set alignment for sub events
            eventContainer.HorizontalAlignment = subEvent.HorizontalAlignment;
            eventContainer.VerticalAlignment = subEvent.VerticalAlignment;
            // set style for sub event
            eventContainer.Style.Fill = "transparent"; eventContainer.Style.StrokeColor = "transparent";
            eventContainer.Style.StrokeWidth = 0;
            container.Children.Add(eventContainer);
        }
        private static void GetBpmnSubProcessTransaction(Node node, BpmnShape shape, DiagramContainer container)
        {
            double shapeWidth = BaseUtil.GetDoubleValue(container.Children[0].Width);
            double shapeHeight = BaseUtil.GetDoubleValue(container.Children[0].Height);

            DiagramElement innerRect = new DiagramElement
            {
                Margin = new Margin() { Left = 3, Right = 0, Top = 3, Bottom = 0 },
                ID = node.ID + "_0_Subprocess_innnerRect",
                CornerRadius = 10,
                Width = shapeWidth - 6,
                Height = shapeHeight - 6
            };
            container.Children.Add(innerRect);

            Canvas transactionEvents = new Canvas
            {
                ID = node.ID + "_transaction_events",
                Style =
                {
                    Gradient = node.Style.Gradient
                }
            };
            BpmnTransactionSubProcess transaction = shape.Activity.SubProcess.Transaction;

            GetBpmnSubEvent(
                transaction.Success as BpmnSubEvent, node, transactionEvents, node.ID + "_success");

            GetBpmnSubEvent(
                transaction.Cancel as BpmnSubEvent, node, transactionEvents, node.ID + "_cancel");

            GetBpmnSubEvent(
                transaction.Failure as BpmnSubEvent, node, transactionEvents, node.ID + "_failure");

            UpdateDiagramContainerVisibility(transactionEvents.Children[0] as DiagramContainer, transaction.Success.Visible);

            UpdateDiagramContainerVisibility(transactionEvents.Children[1] as DiagramContainer, transaction.Cancel.Visible);

            UpdateDiagramContainerVisibility(transactionEvents.Children[2] as DiagramContainer, transaction.Failure.Visible);

            transactionEvents.Float = true;
            transactionEvents.Width = shapeWidth;
            transactionEvents.Height = shapeHeight;
            transactionEvents.Style.Fill = transactionEvents.Style.StrokeColor = "transparent";
            container.Children.Add(transactionEvents);
        }
        private static PathElement GetBpmnAdhocShape(Node node, PathElement adhocNode, BpmnSubProcess subProcess)
        {
            adhocNode.ID = node.ID + "_0_adhoc";
            adhocNode.Width = 12; adhocNode.Height = 8;
            adhocNode.Style.Fill = "black";
            adhocNode.Style.StrokeColor = node.Style.StrokeColor;
            adhocNode.Margin.Bottom = 5;
            adhocNode.HorizontalAlignment = HorizontalAlignment.Left;
            adhocNode.VerticalAlignment = VerticalAlignment.Bottom;
            adhocNode.RelativeMode = RelativeMode.Object;
            adhocNode.Data = "M 49.832 76.811 v -2.906 c 0 0 0.466 -1.469 1.931 -1.5 c 1.465 -0.031 2.331 1.219 2.897 1.688 s 1.06 0.75 1.526 0.75 c 0.466 0 1.548 -0.521 1.682 -1.208 s 0.083 3.083 0.083 3.083 s -0.76 0.969 -1.859 0.969 c -1.066 0 -1.865 -0.625 -2.464 -1.438 s -1.359 -0.998 -2.064 -0.906 C 50.598 75.467 49.832 76.811 49.832 76.811 Z";
            if (subProcess != null && subProcess.Adhoc == true)
            {
                adhocNode.Visible = true;
            }
            else { adhocNode.Visible = false; }
            return adhocNode;
        }
        private static void GetBpmnSubprocessEvent(Node node, Canvas subProcessEventsShapes, BpmnSubEvent events)
        {
            if (events.Annotations.Count != 0)
            {
                for (int i = 0; i < events.Annotations.Count; i++)
                {
                    Annotation annotation = events.Annotations[i];
                    ICommonElement annotations = node.InitAnnotationWrapper(annotation, false);
                    annotations.Width = events.Width; annotations.Height = events.Height;
                    subProcessEventsShapes.Children.Add(annotations);
                }
            }
            if (events.Ports.Count != 0)
            {
                for (int i = 0; i < events.Ports.Count; i++)
                {
                    Object port = events.Ports[i];
                    DiagramElement ports = node.InitPortWrapper(port as Port);
                    subProcessEventsShapes.Children.Add(ports);
                }
            }
        }
        private static PathElement GetBpmnSubProcessLoopShape(Node node)
        {
            BpmnActivity shape = (node.Shape as BpmnShape)?.Activity;
            PathElement subProcessLoop = new PathElement();
            BpmnSubProcess subProcess = shape?.SubProcess;
            if (subProcess != null)
            {
                string loopType = subProcess.Loop.ToString();
                string subProcessLoopData = Dictionary.GetBpmnLoopShapePathData(loopType);
                if (loopType != "None") { subProcessLoop.Visible = true; } else { subProcessLoop.Visible = false; }
                subProcessLoop.ID = node.ID + "_loop";
                subProcessLoop.Data = subProcessLoopData;
            }

            subProcessLoop.Style.Fill = "black";
            subProcessLoop.Width = 12; subProcessLoop.Height = 12;
            subProcessLoop.HorizontalAlignment = HorizontalAlignment.Left; subProcessLoop.VerticalAlignment = VerticalAlignment.Bottom;
            subProcessLoop.SetOffsetWithRespectToBounds(0, 1, UnitMode.Fraction); subProcessLoop.RelativeMode = RelativeMode.Point;
            subProcessLoop.Margin.Bottom = 5;
            subProcessLoop.Style.Fill = "transparent";
            subProcessLoop.Style.StrokeColor = node.Style.StrokeColor == "transparent" ? "black" : node.Style.StrokeColor;
            return subProcessLoop;
        }
        internal static int GetSubProcessChildCount(Node node)
        {
            int count = 0;
            BpmnActivity shape = ((BpmnShape)node.Shape).Activity;
            BpmnSubProcess subProcess = shape.SubProcess;
            string loopType = ((BpmnSubProcess)subProcess).Loop.ToString();
            if (loopType != null && loopType != "None")
            {
                count++;
            }
            if (((BpmnSubProcess)subProcess).Compensation == true)
            {
                count++;
            }
            if (((BpmnSubProcess)subProcess).Collapsed == true)
            {
                count++;
            }
            if (((BpmnSubProcess)subProcess).Adhoc == true)
            {
                count++;
            }
            return count;
        }
        internal static void UpdateElementVisibility(Node node, bool visible, SfDiagramComponent diagram)
        {
            BpmnSubProcess subProcess = (node.Shape as BpmnShape)?.Activity.SubProcess;
            BpmnShape bpmnShape = node.Shape as BpmnShape;
            if (subProcess != null && subProcess.Processes != null && subProcess.Processes.Count > 0)
            {
                DiagramObjectCollection<string> processes = subProcess.Processes;
                for (int j = processes.Count - 1; j >= 0; j--)
                {
                    Node currentNode = diagram.NameTable[processes[j]] as Node;
                    currentNode.IsVisible = visible;
                    diagram.DiagramContent.UpdateElementVisibility(currentNode.Wrapper, currentNode, visible);
                    if (visible)
                    {
                        if (((BpmnShape)currentNode.Shape).Shape == BpmnShapes.Event)
                        {
                            SetEventVisibility(currentNode, ((DiagramContainer)currentNode.Wrapper.Children[0]).Children);
                        }
                        if ((((BpmnShape)currentNode.Shape).Activity as BpmnActivity).Activity == BpmnActivities.SubProcess)
                        {
                            SetSubProcessVisibility(currentNode);
                        }
                    }
                    List<string> connectors = currentNode.InEdges.Concat(currentNode.OutEdges).ToList();
                    for (int i = connectors.Count - 1; i >= 0; i--)
                    {
                        if (diagram.NameTable[connectors[i]] is Connector connector)
                        {
                            connector.IsVisible = visible;
                            diagram.DiagramContent.UpdateElementVisibility(connector.Wrapper, connector, visible);
                        }
                    }
                }
            }
            if (visible)
            {
                if (bpmnShape != null && bpmnShape.Shape == BpmnShapes.Event)
                {
                    SetEventVisibility(node, ((DiagramContainer)node.Wrapper.Children[0]).Children);
                }
                if (bpmnShape != null && ((BpmnActivity)bpmnShape.Activity).Activity == BpmnActivities.SubProcess)
                {
                    SetSubProcessVisibility(node);
                }
                if (bpmnShape != null && (bpmnShape.Activity as BpmnActivity).Activity == BpmnActivities.Task && (bpmnShape.Shape == BpmnShapes.Activity) && bpmnShape.Activity.SubProcess.Loop == BpmnLoops.None)
                {
                    ((DiagramContainer)((DiagramContainer)node.Wrapper.Children[0]).Children[0]).Children[3].Visible = false;
                }
            }
        }
        private static void UpdateDiagramContainerVisibility(ICommonElement element, bool visible)
        {
            if (element is DiagramContainer container)
            {
                for (int i = 0; i < container.Children.Count; i++)
                {
                    UpdateDiagramContainerVisibility(container.Children[i], visible);
                }
            }
            element.Visible = visible;
        }
        private static Canvas GetBpmnTaskShape(Node node)
        {
            BpmnActivity shape = ((BpmnShape)node.Shape).Activity;
            BpmnTask task = shape.Task;
            Canvas taskShapes = new Canvas();
            //childNode0
            DiagramElement taskNode = new DiagramElement
            {
                CornerRadius = 10
            };
            DiagramSize size = GetSize(node, taskNode);
            taskNode.ID = node.ID + "_0_task";
            taskNode.Width = size.Width;
            taskNode.Height = size.Height;
            SetStyle(taskNode, node);
            // if task as call 
            if (task.Call == true)
            {
                taskNode.Style.StrokeWidth = 4;
            }

            taskShapes.Width = size.Width;
            taskShapes.Height = size.Height;
            double childCount = GetTaskChildCount(node);
            double x;
            double childSpace = childCount * 12;
            double area = BaseUtil.GetDoubleValue(size.Width / 2 - childSpace);
            if (childCount == 1) { x = area + 8; }
            else
            {
                x = area + (childCount - 1) * 8;
            }

            //childNode1
            PathElement taskTypeNode = new PathElement();
            if (task.Type == BpmnTasks.Receive || task.Type == BpmnTasks.Send)
            {
                taskTypeNode.Width = 18; taskTypeNode.Height = 16;
            }
            else
            {
                taskTypeNode.Width = 20; taskTypeNode.Height = 20;
            }
            taskTypeNode.ID = node.ID + "_1_tasktType";
            taskTypeNode.Margin.Left = 5; taskTypeNode.Margin.Top = 5;
            string taskTypeNodeData = Dictionary.GetBpmnTaskShapePathData(task.Type.ToString());
            taskTypeNode.Data = taskTypeNodeData;
            taskTypeNode.Style.Fill = "transparent";
            taskTypeNode.Style.Opacity = node.Style.Opacity;
            // append child and set style
            taskShapes.Style.Fill = "transparent";
            taskShapes.Style.StrokeColor = "transparent";
            taskShapes.Style.StrokeWidth = 0;
            taskShapes.Children = new ObservableCollection<ICommonElement>() { taskNode, taskTypeNode };

            //child node for service
            if (task.Type == BpmnTasks.Service)
            {
                PathElement taskTypeNodeService = new PathElement
                {
                    ID = node.ID + "_1_taskTypeService",
                    Data = taskTypeNodeData,
                    Margin =
                    {
                        Left = taskTypeNode.Margin.Left + 9,
                        Top = taskTypeNode.Margin.Top + 9
                    },
                    Style =
                    {
                        Fill = "white",
                        Opacity = node.Style.Opacity
                    }
                };
                taskShapes.Children.Add(taskTypeNodeService);
            }

            // if task as loop
            string loopType = task.Loop.ToString();
            PathElement taskLoopNode = new PathElement();
            string childNode2data = Dictionary.GetBpmnLoopShapePathData(loopType);
            taskLoopNode.Data = childNode2data; taskLoopNode.Style.Fill = "black";
            if (loopType != "None") { taskLoopNode.Visible = true; } else { taskLoopNode.Visible = false; }
            if (childCount == 1) { x = area + 9; }
            taskLoopNode.Margin.Left = x; if (taskLoopNode.Visible == true) { x += 12 + 8; }
            taskLoopNode.Width = 12; taskLoopNode.Height = 12;
            taskLoopNode.Margin.Bottom = 5; taskLoopNode.ID = node.ID + "_2_loop";
            taskLoopNode.HorizontalAlignment = HorizontalAlignment.Left; taskLoopNode.VerticalAlignment = VerticalAlignment.Bottom;
            taskLoopNode.SetOffsetWithRespectToBounds(0, 1, UnitMode.Fraction); taskLoopNode.RelativeMode = RelativeMode.Point;
            taskLoopNode.Style.Fill = "transparent";
            taskTypeNode.Style.Opacity = node.Style.Opacity;
            taskShapes.Children.Add(taskLoopNode);
            //if task as compensation
            PathElement taskCompNode = new PathElement();
            taskCompNode = GetBpmnCompensationShape(node, taskCompNode);
            taskCompNode.Visible = task.Compensation;
            if (childCount == 1) { x = area + 9; }
            taskCompNode.Margin.Left = x - 3;
            taskShapes.Children.Add(taskCompNode);
            return taskShapes;
        }
        private static PathElement GetBpmnCompensationShape(Node node, PathElement compensationNode)
        {
            compensationNode.ID = node.ID + "_0_compensation";
            compensationNode.Width = 12; compensationNode.Height = 12;
            compensationNode.Margin.Bottom = 5; compensationNode.Style.Fill = "transparent";
            compensationNode.Style.StrokeColor = node.Style.StrokeColor == "transparent" ? "black" : node.Style.StrokeColor;
            compensationNode.HorizontalAlignment = HorizontalAlignment.Left;
            compensationNode.VerticalAlignment = VerticalAlignment.Bottom;
            compensationNode.RelativeMode = RelativeMode.Object;
            compensationNode.Data = "M 22.462 18.754 l -6.79 3.92 l 6.79 3.92 V 22.89 l 6.415 3.705 v -7.841 l -6.415 3.705 V 18.754 Z M 28.331 19.701 v 5.947 l -5.149 -2.973 L 28.331 19.701 Z M 21.916 25.647 l -5.15 -2.973 l 5.15 -2.973 V 25.647 Z M 22.275 12.674 c -5.513 0 -9.999 4.486 -9.999 9.999 c 0 5.514 4.486 10.001 9.999 10.001 c 5.514 0 9.999 -4.486 9.999 -10.001 C 32.274 17.16 27.789 12.674 22.275 12.674 Z M 22.275 32.127  c -5.212 0 -9.453 -4.241 -9.453 -9.454 c 0 -5.212 4.241 -9.453 9.453 -9.453 c 5.212 0 9.453 4.241 9.453 9.453 C 31.728 27.887 27.487 32.127 22.275 32.127 Z";
            return compensationNode;
        }
        private static int GetTaskChildCount(Node node)
        {
            int count = 0;
            BpmnActivity shape = ((BpmnShape)node.Shape).Activity;
            BpmnTask task = ((BpmnActivity)shape).Task;
            if (((BpmnTask)task).Compensation == true)
            {
                count++;
            }
            if (((BpmnTask)task).Loop != BpmnLoops.None)
            {
                count++;
            }

            return count;
        }
        private static DiagramSize GetSize(Node node, DiagramElement content)
        {
            DiagramSize size = new DiagramSize() { Width = node.Width, Height = node.Height };
            if (size.Width == null || size.Height == null)
            {
                if (!(content is PathElement))
                {
                    size.Width ??= 50;
                    size.Height ??= 50;
                }
                if (content.ActualSize.Width != null && content.ActualSize.Height != null)
                {
                    return content.ActualSize;
                }
                else
                {
                    content.Measure(new DiagramSize());
                    size.Width = size.Width == 0 ? content.DesiredSize.Width : size.Width;
                    size.Height = size.Height == 0 ? content.DesiredSize.Height : size.Height;
                }
                if (node.MaxWidth != null)
                {
                    size.Width = Math.Min(Convert.ToByte(BaseUtil.GetDoubleValue(size.Width)), Convert.ToByte(BaseUtil.GetDoubleValue(node.MaxWidth)));
                }
                if (node.MaxHeight != null)
                {
                    size.Height = Math.Min(Convert.ToByte(BaseUtil.GetDoubleValue(size.Height)), Convert.ToByte(BaseUtil.GetDoubleValue(node.MaxHeight)));
                }
                if (node.MinWidth != null)
                {
                    size.Width = Math.Max(Convert.ToByte(BaseUtil.GetDoubleValue(size.Width)), Convert.ToByte(BaseUtil.GetDoubleValue(node.MinWidth)));
                }
                if (node.MinHeight != null)
                {
                    size.Height = Math.Max(Convert.ToByte(BaseUtil.GetDoubleValue(size.Height)), Convert.ToByte(BaseUtil.GetDoubleValue(node.MinHeight)));
                }
            }
            return size;
        }
        private static void SetSizeForBpmnGateway(Canvas wrapper, double width, double height)
        {
            wrapper.Children[0].Width = width;
            wrapper.Children[0].Height = height;
            wrapper.Children[1].Width = width * 0.45;
            wrapper.Children[1].Height = height * 0.45;
        }
        private static void SetStyle(DiagramElement child, Node node)
        {
            child.Style.Fill = node.Style.Fill;
            child.Style.StrokeColor = node.Style.StrokeColor == "transparent" ? "black" : node.Style.StrokeColor;
            child.Style.StrokeWidth = node.Style.StrokeWidth;
            child.Style.StrokeDashArray = node.Style.StrokeDashArray;
            child.Style.Opacity = node.Style.Opacity; child.Style.Gradient = node.Style.Gradient;

            if ((node.Constraints & NodeConstraints.Shadow) != 0)
            {
                child.Shadow = node.Shadow;
            }
        }
        internal static void UpdateTextAnnotationProp(Node actualObject, double oldBpmnOffsetX, double oldBpmnOffsetY)
        {
            if (actualObject.Shape.Type == Shapes.Bpmn)
            {
                DiagramObjectCollection<BpmnAnnotation> annotation = ((BpmnShape)actualObject.Shape).Annotations;
                if (annotation != null && annotation.Count > 0)
                {
                    if (actualObject.Wrapper.Children[0] is Canvas canvas)
                        for (int i = 0; i < canvas.Children.Count; i++)
                        {
                            for (int j = 0; j < annotation.Count; j++)
                            {
                                String[] annotationId = canvas.Children[i].ID.Split('_');
                                string id = annotationId[^1];
                                if (id == annotation[j].ID)
                                {
                                    if ((actualObject.Parent as SfDiagramComponent)?.NameTable[
                                        actualObject.ID + "_" + annotation[j].ID + "_connector"] is Connector connector)
                                    {
                                        _ = ConnectorUtil.GetPortDirection(
                                            connector.TargetPoint, actualObject.Wrapper.Bounds,
                                            actualObject.Wrapper.Bounds);
                                        DiagramPoint position = new DiagramPoint()
                                        {
                                            X = connector.SourcePoint.X + (actualObject.Wrapper.OffsetX -
                                                                           (oldBpmnOffsetX == 0
                                                                               ? actualObject.Wrapper.OffsetX
                                                                               : oldBpmnOffsetX)),
                                            Y = connector.SourcePoint.Y + (actualObject.Wrapper.OffsetY -
                                                                           (oldBpmnOffsetY == 0
                                                                               ? actualObject.Wrapper.OffsetY
                                                                               : oldBpmnOffsetY))
                                        };
                                        position = DiagramPoint.Transform(
                                            position,
                                            (annotation[j] as BpmnAnnotation).Angle,
                                            (annotation[j] as BpmnAnnotation).Length);
                                        if ((actualObject.Parent as SfDiagramComponent)?.NameTable[
                                                actualObject.ID + "_textannotation_" + annotation[j].ID] is Node
                                            annotationNode)
                                        {
                                            canvas.Children[i].OffsetX =
                                                annotationNode.OffsetX = position.X;
                                            canvas.Children[i].OffsetY =
                                                annotationNode.OffsetY = position.Y;
                                        }
                                    }

                                    //diagram.UpdateQuad(annotationNode as Node);
                                }
                            }
                        }
                }
            }

        }
        internal void UpdateBpmn(Node changedProp, Dictionary<string, object> shape, object processesValue)
        {
            foreach (string propertyName in shape.Keys)
            {
                BpmnShape bpmn = changedProp.Shape as BpmnShape;
                object value = shape[propertyName];
                if (propertyName == "Shape")
                {
                    if (shape.ContainsKey("Shape"))
                    {
                        bpmn.Shape = (BpmnShapes)Enum.Parse(typeof(BpmnShapes), ((PropertyChangeValues)shape["Shape"]).NewValue.ToString());
                    }
                }
                else if (propertyName == "Annotations")
                {
                    UpdateBpmnAnnotation(changedProp, value as Dictionary<string, object>);
                }
                else
                {
                    this.UpdateBpmnShapes(changedProp, value as Dictionary<string, object>, processesValue);
                }
                if (bpmn.Shape == BpmnShapes.Message || bpmn.Shape == BpmnShapes.DataSource)
                {
                    changedProp.Wrapper.Children[0] = GetBpmnShapes(changedProp);
                }
                break;
            }

        }
        internal static void UpdateBpmnAnnotation(Node changedProp, Dictionary<string, object> shape)
        {
            foreach (string propertyName in shape.Keys)
            {
                object value = shape[propertyName];
                _ = int.TryParse(propertyName, out int propertyVal);
                UpdateBpmnAnnotations(changedProp, value as Dictionary<string, object>, propertyVal);
            }
        }
        private static void UpdateBpmnAnnotations(Node changedProp, Dictionary<string, object> shape, int index)
        {
            foreach (string propertyName in shape.Keys)
            {
                int newValue = (int)((shape[propertyName] as PropertyChangeValues)?.NewValue);

                ((BpmnShape)changedProp.Shape).annotationId = index;
                if (shape.ContainsKey("Angle"))
                {
                    ((BpmnShape)changedProp.Shape).Annotations[index].Angle = newValue;
                }
                if (shape.ContainsKey("Length"))
                {
                    ((BpmnShape)changedProp.Shape).Annotations[index].Length = newValue;
                }
            }

        }
        internal void UpdateBpmnShapes(Node changedProp, Dictionary<string, object> shape, object processesValue)
        {
            object newValue = null;
            object value = null;
            if (changedProp.Shape is BpmnShape bpmn)
            {
                BpmnShapes type = bpmn.Shape;
                foreach (string propertyName in shape.Keys)
                {
                    if (propertyName == "SubProcess" || propertyName == "Task" || propertyName == "0")
                    {
                        value = shape[propertyName];
                    }
                    else
                    {
                        newValue = ((PropertyChangeValues)shape[propertyName]).NewValue;
                    }

                    switch (type)
                    {
                        case BpmnShapes.Gateway:
                            if (shape.ContainsKey("Type"))
                            {
                                bpmn.Gateway.Type = (BpmnGateways)Enum.Parse(typeof(BpmnGateways), (shape["Type"] as PropertyChangeValues).NewValue.ToString());
                            }
                            changedProp.Wrapper.Children[0] = GetBpmnGatewayShape(changedProp);
                            break;
                        case BpmnShapes.DataObject:
                            if (shape.ContainsKey("Type"))
                            {
                                bpmn.DataObject.Type = (BpmnDataObjects)Enum.Parse(typeof(BpmnDataObjects), (shape["Type"] as PropertyChangeValues).NewValue.ToString());
                            }
                            if (shape.ContainsKey("Collection"))
                            {
                                if (newValue != null)
                                {
                                    bpmn.DataObject.Collection = bool.Parse(newValue.ToString());
                                    ((Canvas)changedProp.Wrapper.Children[0]).Children[2].Visible =
                                        bool.Parse(newValue.ToString());
                                }
                            }
                            changedProp.Wrapper.Children[0] = GetBpmnDataObjectShape(changedProp);
                            break;
                        case BpmnShapes.Activity:
                            if (shape.ContainsKey("Activity"))
                            {
                                bpmn.Activity.Activity = (BpmnActivities)Enum.Parse(typeof(BpmnActivities), ((PropertyChangeValues)shape["Activity"]).NewValue.ToString());
                            }
                            if (shape.ContainsKey("SubProcess"))
                            {
                                UpdateBpmnSubProcess(changedProp, value as Dictionary<string, object>, processesValue);
                            }
                            if (shape.ContainsKey("Task"))
                            {
                                UpdateBpmnTask(changedProp, value as Dictionary<string, object>);
                            }
                            changedProp.Wrapper.Children[0] = GetBpmnActivityShape(changedProp);
                            break;
                        case BpmnShapes.Event:
                            if (shape.ContainsKey("Event"))
                            {
                                bpmn.Events.Event = (BpmnEvents)Enum.Parse(typeof(BpmnEvents), ((PropertyChangeValues)shape["Event"]).NewValue.ToString());
                            }
                            if (shape.ContainsKey("Trigger"))
                            {
                                bpmn.Events.Trigger = (BpmnTriggers)Enum.Parse(typeof(BpmnTriggers), ((PropertyChangeValues)shape["Trigger"]).NewValue.ToString());
                            }
                            changedProp.Wrapper.Children[0] = GetBpmnEventShape(changedProp, bpmn.Events);
                            break;

                    }
                }
            }
        }
        private static void UpdateBpmnProcess(Node changedProp, object value)
        {
            ((BpmnShape)changedProp.Shape).Activity.SubProcess.Processes = value as DiagramObjectCollection<string>;
        }
        internal void UpdateBpmnTask(Node changedProp, Dictionary<string, object> shape)
        {
            if (changedProp.Shape is BpmnShape bpmn)
            {
                BpmnShapes type = bpmn.Shape;
                foreach (string propertyName in shape.Keys)
                {
                    object newValue = ((PropertyChangeValues)shape[propertyName]).NewValue;

                    switch (type)
                    {
                        case BpmnShapes.Activity:
                            if (shape.ContainsKey("Loop"))
                            {
                                ((BpmnShape)changedProp.Shape).Activity.Task.Loop = (BpmnLoops)Enum.Parse(typeof(BpmnLoops), ((PropertyChangeValues)shape["Loop"]).NewValue.ToString());
                            }
                            if (shape.ContainsKey("Type"))
                            {
                                ((BpmnShape)changedProp.Shape).Activity.Task.Type = (BpmnTasks)Enum.Parse(typeof(BpmnTasks), ((PropertyChangeValues)shape["Type"]).NewValue.ToString());
                            }
                            if (shape.ContainsKey("Call"))
                            {
                                ((BpmnShape)changedProp.Shape).Activity.Task.Call = bool.Parse(newValue.ToString());
                            }
                            UpdateBpmnActivity(changedProp);
                            break;
                    }
                }
            }
        }
        internal void UpdateBpmnSubProcess(Node changedProp, Dictionary<string, object> shape, object processessValue)
        {
            object newValue = null;
            if (changedProp.Shape is BpmnShape bpmn)
            {
                BpmnShapes type = bpmn.Shape;
                foreach (string propertyName in shape.Keys)
                {
                    if (propertyName != "Processes")
                    {
                        newValue = ((PropertyChangeValues)shape[propertyName]).NewValue;
                    }

                    switch (type)
                    {
                        case BpmnShapes.Activity:
                            if (shape.ContainsKey("Collapsed"))
                            {
                                if (newValue != null)
                                    ((BpmnShape)changedProp.Shape).Activity.SubProcess.Collapsed =
                                        bool.Parse(newValue.ToString());
                            }
                            if (shape.ContainsKey("Adhoc"))
                            {
                                (changedProp.Shape as BpmnShape).Activity.SubProcess.Adhoc = bool.Parse(newValue.ToString());
                            }
                            if (shape.ContainsKey("Compensation"))
                            {
                                if (newValue != null)
                                    ((BpmnShape)changedProp.Shape).Activity.SubProcess.Compensation =
                                        bool.Parse(newValue.ToString());
                            }
                            if (shape.ContainsKey("Loop"))
                            {
                                ((BpmnShape)changedProp.Shape).Activity.SubProcess.Loop = (BpmnLoops)Enum.Parse(typeof(BpmnLoops), ((PropertyChangeValues)shape[LOOP]).NewValue.ToString());
                            }
                            if (shape.ContainsKey("Boundary"))
                            {
                                ((BpmnShape)changedProp.Shape).Activity.SubProcess.Boundary = (BpmnBoundary)Enum.Parse(typeof(BpmnBoundary), ((PropertyChangeValues)shape[BOUNDARY]).NewValue.ToString());
                            }
                            if (shape.ContainsKey("Type"))
                            {
                                ((BpmnShape)changedProp.Shape).Activity.SubProcess.Type = (BpmnSubProcessTypes)Enum.Parse(typeof(BpmnSubProcessTypes), ((PropertyChangeValues)shape[TYPE]).NewValue.ToString());
                            }
                            if (shape.ContainsKey("Processes"))
                            {
                                UpdateBpmnProcess(changedProp, processessValue);

                            }
                            this.UpdateBpmnActivity(changedProp);
                            break;
                    }
                }
            }
        }
        internal void UpdateBpmnSize(Node newShape)
        {
            if (newShape.Shape is BpmnShape bpmn)
            {
                BpmnShapes type = bpmn.Shape;
                if (type == BpmnShapes.Gateway)
                {
                    UpdateBpmnGateway(newShape);
                }
                else if (type == BpmnShapes.DataObject)
                {
                    UpdateBpmnDataObject(newShape);
                }
                else if (type == BpmnShapes.Activity)
                {
                    this.UpdateBpmnActivity(newShape);

                }
                else if (type == BpmnShapes.Event)
                {
                    UpdateBpmnEvent(newShape);
                }
                else if (type == BpmnShapes.Message || type == BpmnShapes.DataSource)
                {
                    newShape.Wrapper.Children[0] = GetBpmnShapes(newShape);
                }
            }
        }
        private static void UpdateBpmnEvent(Node newObject)
        {
            BpmnTriggers trigger = BpmnTriggers.None;
            Canvas elementWrapper = newObject.Wrapper.Children[0] as Canvas;
            if (newObject.Shape is BpmnShape bpmnShape)
            {
                if (elementWrapper != null)
                {
                    DiagramElement elementWrapperChild0 = elementWrapper.Children[0] as DiagramElement;
                    DiagramElement elementWrapperChild1 = elementWrapper.Children[1] as DiagramElement;
                    DiagramElement elementWrapperChild2 = elementWrapper.Children[2] as DiagramElement;
                    if (newObject.Style != null)
                    {
                        if (newObject.Style.StrokeColor != null)
                        {
                            if (elementWrapperChild1 != null)
                            {
                                elementWrapperChild1.Style.Opacity = newObject.Style.Opacity;
                                elementWrapperChild1.Style.StrokeColor = newObject.Style.StrokeColor == "transparent"
                                    ? "black"
                                    : newObject.Style.StrokeColor;
                            }
                        }
                    }
                    BpmnEvents events = bpmnShape.Events.Event;
                    trigger = bpmnShape.Events.Trigger;
                    GetEvent(newObject, events, elementWrapperChild0, elementWrapperChild1, elementWrapperChild2);
                }

                if (trigger != BpmnTriggers.None)
                {
                    UpdateBpmnEventTrigger(newObject);
                }
            }
            if (newObject.Width != null || newObject.Height != null || trigger != BpmnTriggers.None)
            {
                SetSizeForBpmnEvents(((BpmnShape)newObject.Shape).Events, elementWrapper, Convert.ToInt32(BaseUtil.GetDoubleValue(newObject.Wrapper.Children[0].Width ?? newObject.Width)), Convert.ToInt32(BaseUtil.GetDoubleValue(newObject.Wrapper.Children[0].Height ?? newObject.Height)));
            }
        }
        private static void UpdateBpmnEventTrigger(Node node)
        {
            if (((Canvas)node.Wrapper.Children[0]).Children[2] is DiagramElement elementWrapper)
            {
                ((PathElement)elementWrapper).CanMeasurePath = true;
                if (node.Shape is BpmnShape bpmnShape)
                {
                    string bpmnShapeTriggerData =
                        Dictionary.GetBpmnTriggerShapePathData(bpmnShape.Events.Trigger.ToString());
                    ((PathElement)elementWrapper).Data = bpmnShapeTriggerData;
                }
            }
        }
        internal void UpdateBpmnActivity(Node newObject)
        {
            BpmnShape bpmnShape = newObject.Shape as BpmnShape;
            Canvas elementWrapper = newObject.Wrapper.Children[0] as Canvas;
            DiagramSize size = new DiagramSize();
            if (bpmnShape != null && bpmnShape.Activity.Activity != BpmnActivities.None)
            {
                if (elementWrapper != null)
                    size = GetSize(newObject, ((DiagramContainer)elementWrapper.Children[0]).Children[0] as DiagramElement);
            }
            if (bpmnShape != null && bpmnShape.Shape == BpmnShapes.Activity)
            {
                BpmnActivities actualObjectProp = ((BpmnShape)newObject.Shape).Activity.Activity;
                if (actualObjectProp == BpmnActivities.Task && bpmnShape.Activity.Task != null)
                {
                    UpdateBpmnActivityTask(newObject);
                    int subChildCount = GetTaskChildCount(newObject);
                    int x;
                    int childSpace = subChildCount * 12;

                    int area = Convert.ToInt32(BaseUtil.GetDoubleValue(size.Width / 2 - childSpace));
                    if (subChildCount == 1) { x = area + 8; }
                    else
                    {
                        x = area + (subChildCount - 1) * 8;
                    }
                    if (bpmnShape.Activity.Task.Loop != BpmnLoops.None)
                    {
                        UpdateBpmnActivityTaskLoop(newObject, newObject, x, subChildCount, area, 2);
                    }
                }
                if (actualObjectProp == BpmnActivities.SubProcess && bpmnShape.Activity.SubProcess != null)
                {
                    this.UpdateBpmnActivitySubProcess(newObject, newObject);
                }

                SetSizeForBpmnActivity(((BpmnShape)newObject.Shape).Activity, elementWrapper, Convert.ToInt32(BaseUtil.GetDoubleValue(newObject.Wrapper.Children[0].Width ?? newObject.Width)), Convert.ToInt32(BaseUtil.GetDoubleValue(newObject.Wrapper.Children[0].Height ?? newObject.Height)), newObject);
            }
            if (newObject.Width != null || newObject.Height != null)
            {
                SetSizeForBpmnActivity(
                    ((BpmnShape)newObject.Shape).Activity, elementWrapper, Convert.ToInt32(BaseUtil.GetDoubleValue(newObject.Wrapper.Children[0].Width ?? newObject.Width)), Convert.ToInt32(BaseUtil.GetDoubleValue(newObject.Wrapper.Children[0].Height ?? newObject.Height)), newObject);
            }
        }
        internal void UpdateBpmnActivitySubProcess(Node node, Node newObject)
        {
            BpmnShape bpmnShape = newObject.Shape as BpmnShape;
            DiagramContainer elementWrapper = node.Wrapper.Children[0] as Canvas;
            DiagramSize size = GetSize(node, ((Canvas)elementWrapper?.Children[0])?.Children[0] as DiagramElement);
            BpmnSubProcess subProcess = bpmnShape?.Activity.SubProcess;
            int subChildCount = GetSubProcessChildCount(node);
            int x;
            int childSpace = subChildCount * 12;
            int area = Convert.ToInt32(BaseUtil.GetDoubleValue(size.Width / 2 - childSpace));
            //subProcess.Collapsed = subProcess.Type == BpmnSubProcessTypes.Transaction ? false : subProcess.Collapsed;
            if (subChildCount == 1) { x = area + 8; }
            else
            {
                x = area + (subChildCount - 1) * 8;
            }
            if (subProcess != null && subProcess.Events.Count > 0 && subProcess.Type == BpmnSubProcessTypes.Event)
            {
                UpdateBpmnSubProcessEvent(node, newObject);
            }
            if (subProcess.Adhoc)
            {
                UpdateBpmnSubProcessAdhoc(node, subProcess, x, subChildCount, area);
            }
            if (subProcess.Boundary != BpmnBoundary.Default)
            {
                UpdateBpmnSubProcessBoundary(node, subProcess);
            }
            if (subProcess.Collapsed)
            {
                this.UpdateBpmnSubProcessCollapsed(node, subProcess, x, subChildCount, area);
            }
            if (subProcess.Compensation)
            {
                UpdateBpmnSubProcessCompensation(node, subProcess, x, subChildCount, area);
            }
            if (subProcess.Loop != BpmnLoops.None)
            {
                UpdateBpmnActivityTaskLoop(node, node, x, subChildCount, area, 1);
            }
            if (subProcess.Transaction != null && subProcess.Type == BpmnSubProcessTypes.Transaction)
            {
                UpdateBpmnSubProcessTransaction(node);
                BpmnSubProcess subProcesses = ((BpmnShape)node.Shape).Activity.SubProcess;
                if (subProcesses.Processes != null && subProcesses.Processes.Count > 0)
                {
                    DiagramObjectCollection<string> children = ((BpmnShape)node.Shape).Activity.SubProcess.Processes;
                    foreach (string i in children)
                    {
                        if (((SfDiagramComponent)node.Parent)?.NameTable[i] != null && (string.IsNullOrEmpty(((Node)((SfDiagramComponent)node.Parent).NameTable[i]).ProcessId) || ((Node)((SfDiagramComponent)node.Parent).NameTable[i]).ProcessId == node.ID))
                        {
                            ((Node)((SfDiagramComponent)node.Parent).NameTable[i]).ProcessId = node.ID;
                            if (subProcess.Collapsed)
                            {
                                ((SfDiagramComponent)node.Parent).DiagramContent.UpdateElementVisibility(((Node)((SfDiagramComponent)node.Parent)?.NameTable[i]).Wrapper, ((SfDiagramComponent)node.Parent).NameTable[i] as NodeBase, !subProcess.Collapsed);
                            }
                            node.Wrapper.Children.Add(((Node)((SfDiagramComponent)node.Parent).NameTable[i]).Wrapper as ICommonElement);
                        }
                    }
                }
            }
        }
        private static void UpdateBpmnSubProcessTransaction(Node newObject)
        {
            BpmnTransactionSubProcess transaction = ((BpmnShape)newObject.Shape).Activity.SubProcess.Transaction;
            Canvas eventContainer =
                (((Canvas)((Canvas)newObject.Wrapper).Children[0]).Children[0] as Canvas)?.Children[2] as Canvas;

            BpmnSubEvent actualEvent;
            if (transaction.Success != null && eventContainer != null)
            {
                actualEvent = ((BpmnShape)newObject.Shape).Activity.SubProcess.Transaction.Success;
                UpdateBpmnSubEvent(
                    newObject, transaction.Success, actualEvent, eventContainer.Children[0] as Canvas);
            }

            if (transaction.Cancel != null && eventContainer != null)
            {
                actualEvent = (newObject.Shape as BpmnShape)?.Activity.SubProcess.Transaction.Cancel;
                UpdateBpmnSubEvent(
                    newObject, transaction.Cancel, actualEvent, eventContainer.Children[1] as Canvas);
            }

            if (transaction.Failure != null && eventContainer != null)
            {
                actualEvent = (newObject.Shape as BpmnShape)?.Activity.SubProcess.Transaction.Failure;
                UpdateBpmnSubEvent(
                    newObject, transaction.Failure, actualEvent, eventContainer.Children[2] as Canvas);
            }
        }
        private static void UpdateBpmnSubProcessCompensation(Node node, BpmnSubProcess subProcess, int x, int subChildCount, int area)
        {
            Canvas elementWrapper = ((Canvas)node.Wrapper.Children[0]).Children[0] as Canvas;
            int index = (((BpmnShape)node.Shape).Activity.SubProcess.Type == BpmnSubProcessTypes.Transaction) ? 2 : 0;
            if (subProcess.Compensation == false)
            {
                if (elementWrapper != null) elementWrapper.Children[4 + index].Visible = false;
            }
            else
            {
                if (elementWrapper != null) elementWrapper.Children[4 + index].Visible = true;
            }
            UpdateChildMargin(elementWrapper, subChildCount, area, x, 4 + index);
        }
        internal void UpdateBpmnSubProcessCollapsed(Node node, BpmnSubProcess subProcess, int x, int subChildCount, int area)
        {
            int eventLength = ((BpmnShape)node.Shape).Activity.SubProcess.Events.Count;
            Canvas elementWrapper = ((Canvas)node.Wrapper.Children[0]).Children[0] as Canvas;
            int index = (((BpmnShape)node.Shape).Activity.SubProcess.Type == BpmnSubProcessTypes.Transaction) ? 3 : 1;
            if (subProcess.Collapsed == false)
            {
                UpdateElementVisibility(node, true, this.Parent as SfDiagramComponent);
                if (elementWrapper != null) elementWrapper.Children[index + eventLength].Visible = false;
            }
            else
            {
                UpdateElementVisibility(node, false, node.Parent as SfDiagramComponent);

                if (elementWrapper != null) elementWrapper.Children[index + eventLength].Visible = true;
            }
            UpdateChildMargin(elementWrapper, subChildCount, area, x, 3 + eventLength);
        }
        private static void UpdateBpmnSubProcessBoundary(Node node, BpmnSubProcess subProcess)
        {
            DiagramElement elementWrapper = ((Canvas)((Canvas)node.Wrapper.Children[0]).Children[0]).Children[0] as DiagramElement;
            if (subProcess.Boundary == BpmnBoundary.Default && elementWrapper != null)
            {
                elementWrapper.Style.StrokeWidth = 1;
                elementWrapper.Style.StrokeDashArray = "1 0";
            }
            if (subProcess.Boundary == BpmnBoundary.Call && elementWrapper != null)
            {
                elementWrapper.Style.StrokeWidth = 4;
                elementWrapper.Style.StrokeDashArray = "1 0";
            }
            if (subProcess.Boundary == BpmnBoundary.Event && elementWrapper != null)
            {
                elementWrapper.Style.StrokeWidth = 1;
                elementWrapper.Style.StrokeDashArray = "2 2";
            }
        }
        private static void UpdateBpmnSubProcessAdhoc(Node node, BpmnSubProcess subProcess, int x, int subChildCount, int area)
        {
            Canvas elementWrapper = ((Canvas)node.Wrapper.Children[0]).Children[0] as Canvas;
            int index = (((BpmnShape)node.Shape).Activity.SubProcess.Type == BpmnSubProcessTypes.Transaction) ? 2 : 0;

            if (subProcess.Adhoc == false)
            {
                if (elementWrapper != null) elementWrapper.Children[3 + index].Visible = false;
            }
            else
            {
                if (elementWrapper != null) elementWrapper.Children[3 + index].Visible = true;
            }
            UpdateChildMargin(elementWrapper, subChildCount, area, x, 3 + index);
        }

        private static void UpdateBpmnSubProcessEvent(Node node, Node newObject)
        {
            BpmnShape bpmnShape = newObject.Shape as BpmnShape; DiagramElement elementWrapper = node.Wrapper.Children[0] as DiagramElement;
            Canvas nodeContent = ((Canvas)elementWrapper)?.Children[0] as Canvas;
            BpmnSubProcess subProcess = bpmnShape?.Activity.SubProcess;
            int start = 2;
            if (subProcess != null)
                for (int i = 0; i < subProcess.Events.Count; i++)
                {
                    Canvas eventWrapper = nodeContent?.Children[i + start] as Canvas;
                    BpmnSubEvent actualEvent = ((BpmnShape)node.Shape).Activity.SubProcess.Events[i];
                    UpdateBpmnSubEvent(node, subProcess.Events[i], actualEvent, eventWrapper);
                }
        }
        private static void UpdateBpmnSubEvent(Node newObject, BpmnSubEvent newEvent, BpmnSubEvent actualEvent, Canvas eventWrapper)
        {
            DiagramContainer elementWrapper = newObject.Wrapper.Children[0] as Canvas;
            DiagramElement child0 = eventWrapper.Children[0] as DiagramElement;
            DiagramElement child1 = eventWrapper.Children[1] as DiagramElement;
            DiagramElement child2 = eventWrapper.Children[2] as DiagramElement;

            BpmnEvents eventType = BpmnEvents.Start;
            BpmnTriggers trigger = BpmnTriggers.None;
            if (newObject.Style?.StrokeColor != null)
            {
                ((Canvas)((Canvas)elementWrapper)?.Children[0]).Children[1].Style.StrokeColor = newObject.Style.StrokeColor == "transparent" ? "black" : newObject.Style.StrokeColor;
                ((Canvas)((Canvas)elementWrapper)?.Children[0]).Children[1].Style.Opacity = newObject.Style.Opacity;
            }
            if (newObject.Shape is BpmnShape bpmnShape && bpmnShape.Activity.SubProcess != null)
            {
                eventType = newEvent.Event; trigger = newEvent.Trigger;
            }
            GetEvent(newObject as Node, eventType, child0, child1, child2);
            string bpmnShapeTriggerData = Dictionary.GetBpmnTriggerShapePathData(trigger.ToString());
            ((PathElement)eventWrapper.Children[2]).Data = bpmnShapeTriggerData;
            if (newEvent.Height != null || newEvent.Width != null)
            {
                GetEventSize(newEvent, eventWrapper);
            }
            if (newEvent.ID != null)
            {
                eventWrapper.ID = newEvent.ID;
            }
            if (newEvent.Margin != null)
            {
                eventWrapper.Margin = newEvent.Margin;
            }
            if (newEvent.HorizontalAlignment != HorizontalAlignment.Center)
            {
                eventWrapper.HorizontalAlignment = newEvent.HorizontalAlignment;
            }
            if (newEvent.VerticalAlignment != VerticalAlignment.Center)
            {
                eventWrapper.VerticalAlignment = newEvent.VerticalAlignment;
            }
            if (newEvent.Offset != null)
            {
                eventWrapper.SetOffsetWithRespectToBounds(actualEvent.Offset.X, actualEvent.Offset.Y, UnitMode.Fraction);
                eventWrapper.RelativeMode = RelativeMode.Point;
            }
            if (newEvent.Visible)
            {
                UpdateDiagramContainerVisibility(eventWrapper, newEvent.Visible);
            }
        }
        private static void GetEventSize(BpmnSubEvent events, Canvas wrapperChild)
        {
            if (events.Height != null)
            {
                wrapperChild.Height = events.Height;
                ((PathElement)wrapperChild.Children[0]).Height = events.Height;
                ((PathElement)wrapperChild.Children[1]).Height = events.Height * 0.85;
                ((PathElement)wrapperChild.Children[2]).Height = events.Height * 0.54;
            }
            if (events.Width != null)
            {
                wrapperChild.Width = events.Width;
                ((PathElement)wrapperChild.Children[0]).Width = events.Width;
                ((PathElement)wrapperChild.Children[1]).Width = events.Width * 0.85;
                ((PathElement)wrapperChild.Children[2]).Width = events.Width * 0.54;
            }
        }
        private static void GetEvent(Node oldObject, BpmnEvents events, DiagramElement child0, DiagramElement child1, DiagramElement child2)
        {
            switch (events)
            {
                case BpmnEvents.Start:
                    child1.Visible = false;
                    child0.Style.StrokeDashArray = "";
                    child2.Style.Fill = "white";
                    child2.Style.StrokeColor = "black";
                    child0.Style.Fill = "white";
                    break;
                case BpmnEvents.NonInterruptingStart:
                    child0.Style.StrokeDashArray = "2 3";
                    child2.Style.Fill = "white";
                    child0.Style.Fill = "white";
                    child2.Style.StrokeColor = "black";
                    child1.Visible = false;
                    break;
                case BpmnEvents.Intermediate:
                    child0.Style.StrokeDashArray = "";
                    child0.Style.Fill = "white";
                    child1.Style.StrokeDashArray = "";
                    child0.Style.Gradient = null;
                    child2.Style.Fill = "white";
                    child2.Style.StrokeColor = "black";
                    UpdateEventVisibility(oldObject, child1);
                    break;
                case BpmnEvents.NonInterruptingIntermediate:
                    child0.Style.Fill = "white";
                    child0.Style.Gradient = null;
                    child2.Style.Fill = "white";
                    child2.Style.StrokeColor = "black";
                    child0.Style.StrokeDashArray = "2 3";
                    child1.Style.StrokeDashArray = "2 3";
                    UpdateEventVisibility(oldObject, child1);
                    break;
                case BpmnEvents.ThrowingIntermediate:
                case BpmnEvents.End:
                    child0.Style.Fill = events != BpmnEvents.End ? "white" : "black";
                    child0.Style.StrokeDashArray = "";
                    child1.Style.StrokeDashArray = "";
                    child0.Style.Gradient = null;
                    child2.Style.Fill = "black";
                    UpdateEventVisibility(oldObject, child1);
                    child2.Style.StrokeColor = "white";
                    break;
            }
        }
        private static void UpdateEventVisibility(Node oldObject, DiagramElement child1)
        {
            BpmnActivity activity = ((BpmnShape)oldObject.Shape).Activity;
            BpmnSubEvent bpmnEvent = ((BpmnShape)oldObject.Shape).Events;
            if (activity != null && ((BpmnShape)oldObject.Shape).Activity.SubProcess != null &&
            activity.SubProcess.Events.Count > 0 &&
            activity.SubProcess.Events[0] != null)
            {
                if (activity.SubProcess.Events[0].Event == BpmnEvents.NonInterruptingStart ||
                    activity.SubProcess.Events[0].Event == BpmnEvents.Start)
                {
                    child1.Visible = true;
                }
            }
            else if (bpmnEvent != null)
            {
                if (bpmnEvent.Event == BpmnEvents.NonInterruptingStart ||
                    bpmnEvent.Event == BpmnEvents.Start)
                {
                    if (activity != null) child1.Visible = activity.SubProcess.Transaction.Cancel.Visible;
                }
            }
        }
        private static void UpdateBpmnActivityTaskLoop(Node node, Node newObject, int x, int subChildCount, int area, int start)
        {
            string bpmnShapeLoopData;
            BpmnShape bpmnShape = newObject.Shape as BpmnShape;
            Canvas elementWrapper = (((Canvas)node.Wrapper.Children[0]).Children[0] as Canvas);
            BpmnActivity activity = bpmnShape?.Activity;
            int index = 0;
            if (activity != null && activity.Activity == BpmnActivities.SubProcess && activity.SubProcess != null)
            {
                BpmnSubProcess subProcess = activity.SubProcess;
                index = (activity.SubProcess.Type == BpmnSubProcessTypes.Transaction) ? 2 : 0;
                BpmnLoops loop = subProcess.Loop;
                bpmnShapeLoopData = Dictionary.GetBpmnLoopShapePathData(loop.ToString());
                if (elementWrapper != null)
                {
                    ((PathElement)elementWrapper.Children[2 + index]).Data = bpmnShapeLoopData;
                    elementWrapper.Children[2 + index].Visible = (loop != BpmnLoops.None);
                }
            }
            else if (activity != null && activity.Activity == BpmnActivities.Task && activity.Task != null && activity.Task.Loop != BpmnLoops.None)
            {
                bpmnShapeLoopData = Dictionary.GetBpmnLoopShapePathData(activity.Task.Loop.ToString());
                if (elementWrapper != null)
                {
                    ((PathElement)elementWrapper.Children[2]).Data = bpmnShapeLoopData;
                    elementWrapper.Children[2].Visible = (activity.Task.Loop != BpmnLoops.None);
                }
            }
            UpdateChildMargin(elementWrapper, subChildCount, area, x, start + index);
        }
        private static void UpdateChildMargin(DiagramContainer elementWrapper, int subChildCount, int area, int x, int start)
        {
            if (subChildCount == 1)
            {
                for (int i = start; i < elementWrapper.Children.Count; i++)
                {
                    if (i != 2 && elementWrapper.Children[i].Visible == true)
                    {
                        elementWrapper.Children[i].Margin.Left = x;
                        x = area + 8;
                    }
                }
            }
            else
            {
                x = area + (subChildCount - 1) * 8;
                for (int i = start; i < elementWrapper.Children.Count; i++)
                {
                    if (i != 2 && elementWrapper.Children[i].Visible == true)
                    {
                        elementWrapper.Children[i].Margin.Left = x; x += 12 + 8;
                    }
                }
            }
        }
        internal static void SetSizeForBpmnActivity(BpmnActivity activity, Canvas wrapper, int width, int height, Node node)
        {
            if (wrapper.Children[0] is Canvas canvas)
            {
                canvas.Width = width;
                canvas.Height = height;

                canvas.Children[0].Width = width;
                canvas.Children[0].Height = height;

                if (activity.SubProcess.Type == BpmnSubProcessTypes.Transaction)
                {
                    canvas.Children[1].Width = Math.Max(width - 6, 1);
                    canvas.Children[1].Height = Math.Max(height - 6, 1);
                    canvas.Children[2].Width = width;
                    canvas.Children[2].Height = height;
                }

                DiagramElement taskNode = new DiagramElement();
                int x;
                _ = GetSize(node, taskNode);
                int childCount;
                int iconSpace = 4;
                if (activity.Activity == BpmnActivities.Task)
                {
                    childCount = GetTaskChildCount(node);
                }
                else
                {
                    childCount = GetSubProcessChildCount(node);
                }

                int childSpace = childCount * 12;
                int area = width / 2;
                if (childCount == 1)
                {
                    x = area - 6;
                }
                else
                {
                    x = area - (childSpace / 2) - ((childCount - 1) * iconSpace) / 2;
                }

                for (int i = 0; i < ((Canvas)wrapper.Children[0]).Children.Count; i++)
                {
                    if (canvas.Children[i].Visible &&
                        (canvas.Children[i].ID.IndexOf("_loop", StringComparison.InvariantCulture) > -1 ||
                         canvas.Children[i].ID.IndexOf("_0_compensation", StringComparison.InvariantCulture) > -1 ||
                         canvas.Children[i].ID.IndexOf("_0_adhoc", StringComparison.InvariantCulture) > -1 ||
                         canvas.Children[i].ID.IndexOf("_0_collapsed", StringComparison.InvariantCulture) > -1))
                    {
                        canvas.Children[i].Margin.Left = x;
                        x += Convert.ToInt32(BaseUtil.GetDoubleValue(canvas.Children[i].ActualSize.Width + iconSpace));
                    }
                }
            }
        }
        private static void UpdateBpmnDataObject(Node node)
        {
            Canvas elementWrapper = node.Wrapper.Children[0] as Canvas;
            if (node.Shape is BpmnShape bpmnShape && bpmnShape.Shape == BpmnShapes.DataObject)
            {
                if (elementWrapper != null)
                {
                    DiagramElement elementWrapperChild1 = elementWrapper.Children[1] as DiagramElement;
                    DiagramElement elementWrapperChild2 = elementWrapper.Children[2] as DiagramElement;
                    if (node.Style != null)
                    {
                        if (elementWrapperChild1 != null) elementWrapperChild1.Style.Opacity = node.Style.Opacity;
                        if (elementWrapperChild2 != null) elementWrapperChild2.Style.Opacity = node.Style.Opacity;
                    }

                    switch (bpmnShape.DataObject.Type)
                    {
                        case BpmnDataObjects.None:
                            if (elementWrapperChild1 != null) elementWrapperChild1.Visible = false;
                            break;
                        case BpmnDataObjects.Input:
                            if (elementWrapperChild1 != null) elementWrapperChild1.Style.Fill = "white";
                            break;
                        case BpmnDataObjects.Output:
                            if (elementWrapperChild1 != null) elementWrapperChild1.Style.Fill = "black";
                            break;
                    }
                    if (((BpmnShape)node.Shape).DataObject.Collection)
                    {
                        if (elementWrapperChild2 != null)
                            elementWrapperChild2.Visible = bpmnShape.DataObject.Collection;
                    }
                }
            }
            if (node.Width != null || node.Height != null)
            {
                if (elementWrapper != null)
                {
                    elementWrapper.Children[0].Width = BaseUtil.GetDoubleValue(node.Wrapper.Children[0].Width == null
                        ? node.Width
                        : node.Wrapper.Children[0].Width);
                    elementWrapper.Children[0].Height = BaseUtil.GetDoubleValue(node.Wrapper.Children[0].Height == null
                        ? node.Height
                        : node.Wrapper.Children[0].Height);
                }
            }
        }
        private static void UpdateBpmnActivityTask(Node newObject)
        {
            BpmnShape bpmnShape = newObject.Shape as BpmnShape;
            Canvas elementWrapper = (((Canvas)newObject.Wrapper.Children[0]).Children[0] as Canvas);
            BpmnTask task = bpmnShape?.Activity.Task;
            if (elementWrapper != null)
                for (int i = 0; i < elementWrapper.Children.Count; i++)
                {
                    if (elementWrapper.Children[i].ID == newObject.ID + "_1_taskTypeService")
                    {
                        elementWrapper.Children.Remove(elementWrapper.Children[i]);
                    }
                }

            if (task.Type == BpmnTasks.Receive || task.Type == BpmnTasks.Send)
            {
                elementWrapper.Children[1].Height = 14;
            }
            else
            {
                elementWrapper.Children[1].Height = 20;
            }
            if (task.Type != BpmnTasks.None)
            {
                string bpmnShapeTaskData = Dictionary.GetBpmnTaskShapePathData(task.Type.ToString()); ;
                ((PathElement)elementWrapper.Children[1]).Data = bpmnShapeTaskData;
                if (task.Type == BpmnTasks.Service)
                {
                    for (int i = 0; i < elementWrapper.Children.Count; i++)
                    {
                        if (elementWrapper.Children[i].ID == newObject.ID + "_1_tasktType")
                        {
                            elementWrapper.Children.Remove(elementWrapper.Children[i]);
                        }
                    }
                    PathElement taskTypeNode = new PathElement
                    {
                        ID = newObject.ID + "_1_tasktType",
                        Margin =
                        {
                            Left = 5,
                            Top = 5
                        },
                        Data = bpmnShapeTaskData,
                        Style =
                        {
                            Fill = "transparent",
                            Opacity = newObject.Style.Opacity
                        },
                        Width = 20,
                        Height = 20
                    };
                    elementWrapper.Children.Remove(elementWrapper.Children[1]);
                    elementWrapper.Children.Add(taskTypeNode);
                    PathElement taskTypeNodeService = new PathElement
                    {
                        ID = newObject.ID + "_1_taskTypeService",
                        Data = bpmnShapeTaskData,
                        Margin =
                        {
                            Left = elementWrapper.Children[1].Margin.Left + 9,
                            Top = elementWrapper.Children[1].Margin.Top + 9
                        },
                        Style =
                        {
                            Fill = "white",
                            Opacity = newObject.Style.Opacity
                        }
                    };
                    elementWrapper.Children.Remove(elementWrapper.Children[2]);
                    elementWrapper.Children.Add(taskTypeNodeService);
                }
            }
            if (bpmnShape != null && bpmnShape.Activity.Task.Call)
            {
                if (bpmnShape.Activity.Task.Call != false) { elementWrapper.Children[0].Style.StrokeWidth = 4; }
                else
                {
                    elementWrapper.Children[0].Style.StrokeWidth = 1;
                }
            }
            if (bpmnShape.Activity.Task.Compensation)
            {
                if (bpmnShape.Activity.Task.Compensation == true) { elementWrapper.Children[3].Visible = true; }
                else
                {
                    elementWrapper.Children[3].Visible = false;
                }
            }
        }

        private static void UpdateBpmnGateway(Node node)
        {
            Canvas elementWrapper = node.Wrapper.Children[0] as Canvas;
            if (node.Shape is BpmnShape bpmnShape)
            {
                string bpmnShapeGatewayData = Dictionary.GetBpmnGatewayShapePathData(bpmnShape.Gateway.Type.ToString());
                ((PathElement)((Canvas)elementWrapper)?.Children[1]).Data = bpmnShapeGatewayData;
            }
            ICommonElement canvas = node.Wrapper.Children[0];
            if (node.Wrapper != null && canvas.Width != null || canvas.Height != null)
            {
                SetSizeForBpmnGateway(elementWrapper, BaseUtil.GetDoubleValue(canvas.Width), BaseUtil.GetDoubleValue(canvas.Height));
            }
        }
        private static void SetSizeForBpmnEvents(BpmnSubEvent events, Canvas wrapper, int width, int height)
        {
            wrapper.Children[0].Width = width;
            wrapper.Children[0].Height = height;

            //child node 1 - event node
            if (wrapper.Children[1] is DiagramElement eventNode)
            {
                eventNode.Width = width * 0.85;
                eventNode.Height = height * 0.85;
            }

            DiagramElement triggerNode = wrapper.Children[2] as DiagramElement;
            if (events.Trigger == BpmnTriggers.Message && triggerNode != null)
            {
                triggerNode.Width = width * 0.54;
                triggerNode.Height = height * 0.4;
            }
            else
            {
                if (triggerNode != null)
                {
                    triggerNode.Width = width * 0.5;
                    triggerNode.Height = height * 0.5;
                }
            }
        }
        internal static DiagramRect GetChildrenBound(IDiagramObject node, string excludeChild, SfDiagramComponent diagram)
        {
            DiagramObjectCollection<string> processes = ((BpmnShape)((Node)node).Shape).Activity.SubProcess.Processes;
            DiagramRect bound = null;
            if (processes != null && processes.Count > 0)
            {
                foreach (string i in processes)
                {
                    if (excludeChild != i)
                    {
                        if (bound == null)
                        {
                            bound = ((NodeBase)diagram.NameTable[i]).Wrapper.Bounds;
                        }
                        else
                        {
                            bound = ((NodeBase)diagram.NameTable[i]).Wrapper.Bounds.UniteRect(bound);
                        }
                    }
                }
            }
            return bound ?? ((NodeBase)diagram.NameTable[excludeChild]).Wrapper.Bounds;
        }

        internal static void UpdateSubProcesses(IDiagramObject obj, SfDiagramComponent diagram)
        {
            double diffX = 0; double diffY = 0;
            NodeBase node = diagram.NameTable[((Node)obj).ProcessId] as NodeBase;
            bool right = false; bool bottom = false;
            DiagramPoint pivot = new DiagramPoint() { X = 0.5, Y = 0.5 };
            if ((node.Wrapper.Bounds.Left + ((Node)obj).Margin.Left + ((Node)obj).Width) > (node.Wrapper.Bounds.Right))
            {
                right = true;
            }
            if ((node.Wrapper.Bounds.Top + ((Node)obj).Margin.Top + ((Node)obj).Height) > (node.Wrapper.Bounds.Bottom))
            {
                bottom = true;
            }
            if (right)
            {
                pivot.X = 0;
            }
            if (bottom) { pivot.Y = 0; }
            DiagramSize actualSize = node.Wrapper.ActualSize;
            if (right)
            {
                diffX = (((Node)obj).Wrapper.Margin.Left + ((Node)obj).Wrapper.Bounds.Width) / BaseUtil.GetDoubleValue(actualSize.Width);
            }
            if (bottom)
            {
                diffY = (((Node)obj).Wrapper.Margin.Top + ((Node)obj).Wrapper.Bounds.Height) / BaseUtil.GetDoubleValue(actualSize.Height);
            }
            if (diffX > 0 || diffY > 0)
            {
                diagram.CommandHandler.Scale(diagram.NameTable[((Node)obj).ProcessId] as NodeBase, diffX > 0 ? diffX : 1, diffY > 0 ? diffY : 1, pivot, null);
            }
        }
        /// <summary>
        /// Creates a new BPMN diagram that is a copy of the current BPMN diagram. 
        /// </summary>
        /// <returns>BpmnDiagrams</returns>
        public override object Clone()
        {
            return new BpmnDiagrams(this);
        }

        internal override void Dispose()
        {
            base.Dispose();
            if (TextAnnotationNodes != null)
            {
                for (int i = 0; i < TextAnnotationNodes.Count; i++)
                {
                    TextAnnotationNodes[i].Dispose();
                    TextAnnotationNodes[i] = null;
                }
                TextAnnotationNodes.Clear();
                TextAnnotationNodes = null;
            }
            if (annotationObjects != null)
            {
                annotationObjects.Clear();
                annotationObjects = null;
            }
        }
    }
}
