namespace Syncfusion.Blazor.Diagram.Internal
{
    internal static class ConstraintsUtil
    {
        internal static int CheckPortRestriction(Port port, PortVisibility portVisibility)
        {
            var visibility = port.Visibility;
            visibility &= portVisibility;
            return (int)visibility;
        }
        internal static bool CheckPortConstraints(Port port, PortConstraints portConstraints)
        {
            return port.Constraints.HasFlag(portConstraints);
        }

        internal static bool CanOutConnect(Node node)
        {
            return node.Constraints.HasFlag(NodeConstraints.OutConnect);
        }

        internal static bool CanInConnect(Node node)
        {
            return node.Constraints.HasFlag(NodeConstraints.InConnect);
        }


        internal static bool CanPortOutConnect(PointPort port)
        {
            return (port.Constraints != PortConstraints.None) && port.Constraints.HasFlag(PortConstraints.OutConnect);
        }
        internal static bool CanPortInConnect(PointPort port)
        {
            return (port.Constraints != PortConstraints.None) && port.Constraints.HasFlag(PortConstraints.InConnect);
        }
        internal static bool CanShadow(Node node)
        {
            return node.Constraints.HasFlag(NodeConstraints.Shadow);
        }
        //internal static bool CanVirtualize(SfDiagramComponent diagram)
        //{
        //    return false;// diagram.Constraints.HasFlag(DiagramConstraints.Virtualization);
        //}
        internal static bool CanZoom(SfDiagramComponent diagram)
        {
            return diagram.Constraints.HasFlag(DiagramConstraints.Zoom);
        }
        internal static bool CanZoomTextEdit(SfDiagramComponent diagram)
        {
            return diagram.Constraints.HasFlag(DiagramConstraints.ZoomTextEdit);
        }
        internal static bool CanPan(SfDiagramComponent diagram)
        {
            return CanPanX(diagram) || CanPanY(diagram);
        }
        internal static bool CanMultiSelect(SfDiagramComponent diagram)
        {
            return diagram.InteractionController.HasFlag(InteractionController.MultipleSelect);
        }
        internal static bool CanSingleSelect(SfDiagramComponent diagram)
        {
            return diagram.InteractionController.HasFlag(InteractionController.SingleSelect);
        }
        internal static bool CanZoomPan(SfDiagramComponent diagram)
        {
            return diagram.InteractionController.HasFlag(InteractionController.ZoomPan);
        }
        internal static bool CanPanX(SfDiagramComponent diagram)
        {
            return diagram.Constraints.HasFlag(DiagramConstraints.PanX);
        }
        internal static bool CanPanY(SfDiagramComponent diagram)
        {
            return diagram.Constraints.HasFlag(DiagramConstraints.PanY);
        }
        internal static bool CanUserInteract(SfDiagramComponent diagram)
        {
            return diagram.Constraints.HasFlag(DiagramConstraints.UserInteraction);
        }
        internal static bool DefaultTool(SfDiagramComponent diagram)
        {
            return diagram.InteractionController.HasFlag(InteractionController.SingleSelect) || diagram.InteractionController.HasFlag(InteractionController.MultipleSelect);
        }

        internal static bool CanSelect(IDiagramObject obj)
        {
            if (obj != null)
            {
                if (obj is Connector connector)
                {
                    return connector.Constraints.HasFlag(ConnectorConstraints.Select);
                }
                else
                {
                    return ((Node)obj).Constraints.HasFlag(NodeConstraints.Select);
                };
            }
            return false;
        }
        internal static bool CanRotate(IDiagramObject obj)
        {
            if (obj != null)
            {
                return ((Node)obj).Constraints.HasFlag(NodeConstraints.Rotate);
            }
            return false;
        }
        internal static bool CanMove(IDiagramObject obj)
        {
            if (obj != null)
            {
                if (obj is Connector connector)
                {
                    return connector.Constraints.HasFlag(ConnectorConstraints.Drag);
                }
                else if (obj is DiagramSelectionSettings)
                {
                    return true;
                }
                else
                {
                    return ((Node)obj).Constraints.HasFlag(NodeConstraints.Drag);
                }
            }
            return true;
        }
        internal static bool CanDelete(IDiagramObject obj)
        {
            if (obj != null)
            {
                if (obj is Connector connector)
                {
                    return connector.Constraints.HasFlag(ConnectorConstraints.Delete);
                }
                else
                {
                    return ((Node)obj).Constraints.HasFlag(NodeConstraints.Delete);
                }
            }
            return true;
        }

        internal static bool CanShowCorner(SelectorConstraints selectorConstraints, SelectorConstraints constraints)
        {
            return selectorConstraints.HasFlag(constraints);
        }

        internal static bool HasSingleConnection(SfDiagramComponent diagram)
        {
            if (diagram.SelectionSettings.Connectors.Count == 1 && diagram.SelectionSettings.Nodes.Count == 0)
            {
                return true;
            }
            return false;
        }

        internal static bool CanDragSourceEnd(Connector connector)
        {
            return connector.Constraints.HasFlag(ConnectorConstraints.DragSourceEnd);
        }
        internal static bool CanDrawOnce(SfDiagramComponent model)
        {
            return model.InteractionController.HasFlag(InteractionController.DrawOnce);
        }
        internal static bool CanContinuous(SfDiagramComponent model)
        {
            return model.InteractionController.HasFlag(InteractionController.ContinuousDraw);
        }

        internal static bool CanDragTargetEnd(Connector connector)
        {
            return connector.Constraints.HasFlag(ConnectorConstraints.DragTargetEnd);
        }

        internal static bool CanDragSegmentThumb(Connector connector)
        {
            return connector.Constraints.HasFlag(ConnectorConstraints.DragSegmentThumb);
        }

        internal static bool CanEnablePointerEvents(NodeBase obj)
        {
            if (obj is Connector connector)
            {
                return connector.Constraints.HasFlag(ConnectorConstraints.PointerEvents);
            }
            else
            {
                return ((Node)obj).Constraints.HasFlag(NodeConstraints.PointerEvents);
            }
        }

        internal static bool EnableReadOnly(IDiagramObject obj, Annotation text)
        {
            if (obj is Connector && text is PathAnnotation annotation)
            {
                if (CanEditText(obj) && annotation.Constraints.HasFlag(AnnotationConstraints.InheritReadOnly))
                {
                    return true;
                }
                else if (annotation.Constraints.HasFlag(AnnotationConstraints.ReadOnly))
                {
                    return true;
                }
            }
            else if (obj is Node && text is ShapeAnnotation shapeAnnotation)
            {
                if (CanEditText(obj) && shapeAnnotation.Constraints.HasFlag(AnnotationConstraints.InheritReadOnly))
                {
                    return true;
                }
                else if (shapeAnnotation.Constraints.HasFlag(AnnotationConstraints.ReadOnly))
                {
                    return true;
                }
            }
            return false;
        }

        internal static bool CanEditText(IDiagramObject obj)
        {
            if (obj is Connector connector)
            {
                return connector.Constraints.HasFlag(ConnectorConstraints.ReadOnly);
            }
            else if (obj is Node node)
            {
                return node.Constraints.HasFlag(NodeConstraints.ReadOnly);
            }
            return false;
        }
        internal static bool CanAllowDrop(NodeBase obj)
        {
            if (obj is Connector connector)
            {
                return connector.Constraints.HasFlag(ConnectorConstraints.AllowDrop);
            }
            else if (obj is Node node)
            {
                return node.Constraints.HasFlag(NodeConstraints.AllowDrop);
            }
            return false;
        }
        internal static bool CheckConnect(PointPort target, Actions endPoint)
        {
            if (CanPortInConnect(target) && endPoint == Actions.ConnectorTargetEnd)
            {
                return true;
            }
            else if (CanPortOutConnect(target) && endPoint == Actions.ConnectorSourceEnd)
            {
                return true;
            }
            else if ((target.Constraints != PortConstraints.None) && !CanPortInConnect(target) && !CanPortOutConnect(target))
            {
                return true;
            }
            return false;
        }

        internal static bool CanBridge(Connector connector, SfDiagramComponent diagram)
        {
            if (connector.Constraints.HasFlag(ConnectorConstraints.Bridging))
            {
                return true;
            }
            else if (diagram.Constraints.HasFlag(DiagramConstraints.Bridging) && connector.Constraints.HasFlag(ConnectorConstraints.InheritBridging))
            {
                return true;
            }
            return false;
        }
        internal static bool CanPageEditable(SfDiagramComponent model)
        {
            return CanApiInteract(model) || (model.DiagramAction.HasFlag(DiagramAction.Interactions) && CanUserInteract(model));
        }
        internal static bool CanApiInteract(SfDiagramComponent model)
        {
            return model.Constraints.HasFlag(DiagramConstraints.ApiUpdate);
        }
        internal static bool CanDraw(Port port)
        {
            return port.Constraints.HasFlag(PortConstraints.Draw);
        }
    }
}