using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel;
using Syncfusion.Blazor.CircularGauge.Internal;
using Syncfusion.PdfExport;
using Microsoft.JSInterop;
using System.Drawing;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Internal;
using System.Globalization;

namespace Syncfusion.Blazor.CircularGauge
{
    /// <summary>
    /// The circular gauge component is used to visualize the numeric values on the circular scale.
    /// The circular gauge contains labels, ticks, and an axis line to customize its appearance.
    /// </summary>
    public partial class SfCircularGauge
    {
        /// <summary>
        /// This method is invoked when we mouse down on the Circular Gauge.
        /// </summary>
        /// <param name="mouseX">Specifies the x position of the mouse down event.</param>
        /// <param name="mouseY">Specifies the y position of the mouse down event.</param>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exclude />
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task TriggerMouseDownEvent(double mouseX, double mouseY)
        {
            await SfBaseUtils.InvokeEvent<MouseEventArgs>(CircularGaugeEvents?.OnGaugeMouseDown, GetMouseEventArgs(false, mouseX, mouseY));
        }

        /// <summary>
        /// This method is invoked when the mouse is released on the Circular Gauge.
        /// </summary>
        /// <param name="mouseX">Specifies the x position of the mouse up event.</param>
        /// <param name="mouseY">Specifies the y position of the mouse up event.</param>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exclude />
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task TriggerMouseUpEvent(double mouseX, double mouseY)
        {
            await SfBaseUtils.InvokeEvent<MouseEventArgs>(CircularGaugeEvents?.OnGaugeMouseUp, GetMouseEventArgs(false, mouseX, mouseY));
        }

        /// <summary>
        /// This method is invoked when the mouse pointer is moved out of the Circular Gauge.
        /// </summary>
        /// <param name="mouseX">Specifies the x position of the mouse leave event.</param>
        /// <param name="mouseY">Specifies the y position of the mouse leave event.</param>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exclude />
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task TriggerMouseLeaveEvent(double mouseX, double mouseY)
        {
            await SfBaseUtils.InvokeEvent<MouseEventArgs>(CircularGaugeEvents?.OnGaugeMouseLeave, GetMouseEventArgs(false, mouseX, mouseY));
        }

        /// <summary>
        /// This method will be invoked when the window is resized to resize the component.
        /// </summary>
        /// <param name="args">Specifies the argument for the resize event.</param>
        /// <param name="width">Specifies the component width.</param>
        /// <param name="height">Specifies the component height.</param>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exclude />
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task TriggerResizeEvent(ResizeEventArgs args, double width, double height)
        {
            args = new ResizeEventArgs()
            {
                CurrentSize = new Size((int)width, (int)height),
                PreviousSize = new Size((int)AvailableSize.Width, (int)AvailableSize.Height),
                Name = "Resizing",
            };
            AllowRefresh = true;
            AvailableSize = ContainerSize(width, AvailableSize.Height);
            await SfBaseUtils.InvokeEvent<ResizeEventArgs>(CircularGaugeEvents?.Resizing, args);
            StateHasChanged();
        }

        /// <summary>
        /// This method is invoked when the pointer or range element is dragged.
        /// </summary>
        /// <param name="axisIndex">Specifies the index number of the axis in which the pointer is dragged.</param>
        /// <param name="pointerIndex">Specifies the index number of the pointer in which the pointer is dragged.</param>
        /// <param name="rangeIndex">Specifies the index number of the range in which the range is dragged.</param>
        /// <param name="type">Specifies the type of the pointer.</param>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exclude />
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task TriggerDragStart(int axisIndex, int pointerIndex, int rangeIndex, string type)
        {
            string dragType = type == "Pointer" ? "pointerStart" : "rangeStart";
            PointerDragEventArgs args = GetDragEventArguments(axisIndex, pointerIndex, rangeIndex, dragType, "OnDragStart");
            await SfBaseUtils.InvokeEvent<PointerDragEventArgs>(CircularGaugeEvents?.OnDragStart, args);
        }

        /// <summary>
        /// TriggerDragEnd is an async method that is triggered when we drag and leave the pointer or range element.
        /// </summary>
        /// <param name="axisIndex">Specifies the index number of the axis in which the range or pointer drag is stopped.</param>
        /// <param name="pointerIndex">Specifies the index number of the pointer in which the pointer drag is stopped.</param>
        /// <param name="rangeIndex">Specifies the index number of the range in which the range drag is stopped.</param>
        /// <param name="type">Specifies the type of the pointer.</param>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exclude />
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task TriggerDragEnd(int axisIndex, int pointerIndex, int rangeIndex, string type)
        {
            string dragType = type == "Pointer" ? "pointerEnd" : "rangeEnd";
            PointerDragEventArgs args = GetDragEventArguments(axisIndex, pointerIndex, rangeIndex, dragType, "OnDragEnd");
            await SfBaseUtils.InvokeEvent<PointerDragEventArgs>(CircularGaugeEvents?.OnDragEnd, args);
        }

        /// <summary>
        /// This method that is triggered when the tooltip is to be rendered in the Circular Gauge.
        /// </summary>
        /// <param name="x">specifies the x position of the tooltip.</param>
        /// <param name="y">specifies the y position of the tooltip.</param>
        /// <param name="axisIndex">specifies the index of the axis.</param>
        /// <param name="pointerIndex">specifies the index number of the pointer.</param>
        /// <param name="isRange">specifies to show the tooltip for range.</param>
        /// <param name="isPointer">specifies to show the tooltip for pointers.</param>
        /// <param name="isAnnotation">specifies to show the tooltip for annotations.</param>
        /// <param name="elementRectBounds">specifies the bounds of the tooltip elements.</param>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exclude />
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task TriggerTooltipEvent(double x, double y, int axisIndex, int pointerIndex, bool isRange, bool isPointer, bool isAnnotation, BoundingClientRect elementRectBounds)
        {
            if (AxisRenderer.TooltipRenderer == null)
            {
                AxisRenderer.TooltipRenderer = new TooltipRenderer(this);
            }

            PointF location = new PointF
            {
                X = (float)x,
                Y = (float)y,
            };
            if (elementRectBounds != null)
            {
                TooltipRenderEventArgs args = AxisRenderer.TooltipRenderer.RenderTooltip(location, elementRectBounds, axisIndex, pointerIndex, isRange, isPointer, isAnnotation);
                if (args != null)
                {
                    await SfBaseUtils.InvokeEvent(CircularGaugeEvents?.TooltipRendering, args);
                    StateHasChanged();
                }
            }
       }

        /// <summary>
        /// This method is used to animate the pointer element.
        /// </summary>
        /// <param name="axisIndex">specifies the index number of the axis.</param>
        /// <param name="pointerIndex">specifies the index number of the pointer.</param>
        /// <param name="pointerValue">specifies the value of the pointer.</param>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exclude />
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task AnimatePointer(int axisIndex, int pointerIndex, double pointerValue)
        {
            AxisRenderer.PointerRenderer.SetPointerValue(Axes[axisIndex], Axes[axisIndex].Pointers[pointerIndex], pointerValue, AxisRenderer.AxisCollection[axisIndex].PointerCollection[pointerIndex], AxisRenderer.AxisCollection[axisIndex]);
            AnimationCompleteEventArgs args = new AnimationCompleteEventArgs() { Cancel = false };
            await SfBaseUtils.InvokeEvent<AnimationCompleteEventArgs>(CircularGaugeEvents?.AnimationCompleted, args);

            StateHasChanged();
        }

        /// <summary>
        /// TriggerDragEvent is an async method that is triggered when the pointer element is dragged.
        /// </summary>
        /// <param name="x">Specifies the x position of the drag event.</param>
        /// <param name="y">Specifies the y position of the drag event.</param>
        /// <param name="axisIndex">Specifies the index number of the axis.</param>
        /// <param name="pointerIndex">Specifies the index number of the pointer.</param>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exclude />
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task TriggerDragEvent(double x, double y, int axisIndex, int pointerIndex)
        {
            if (EnablePointerDrag)
            {
                PointF location = new PointF()
                {
                    X = (float)x,
                    Y = (float)y,
                };
                double previousValue = Axes[axisIndex].AxisValues.PointerValue[pointerIndex];
                double currentValue = 0;
                if (AxisRenderer.PointerRenderer.PointerDrag(location, axisIndex, pointerIndex))
                {
                    currentValue = Axes[axisIndex].AxisValues.PointerValue[pointerIndex];
                    PointerDragEventArgs args = new PointerDragEventArgs() { AxisIndex = axisIndex, PointerIndex = pointerIndex, RangeIndex = 0, Name = "OnDragMove", Type = "pointerMove", CurrentValue = (float)currentValue, PreviousValue = (float)previousValue };
                    await SfBaseUtils.InvokeEvent<PointerDragEventArgs>(CircularGaugeEvents?.OnDragMove, args);

                    // TODO: The state change has been called since pointer drag didn’t work in IE 11 browser alone. Without this code it works fine in other browsers.
                    if (IsIE)
                    {
                        StateHasChanged();
                    }
                }
            }
        }

        /// <summary>
        /// TriggerRangeDragEvent is an async method that is triggered when the range element is dragged.
        /// </summary>
        /// <param name="x">Specifies the x position of the drag event.</param>
        /// <param name="y">Specifies the y position of the drag event.</param>
        /// <param name="axisIndex">Specifies the index number of the axis.</param>
        /// <param name="rangeIndex">Specifies the index number of the range.</param>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exclude />
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task TriggerRangeDragEvent(double x, double y, int axisIndex, int rangeIndex)
        {
            if (enableRangeDrag)
            {
                CircularGaugeAxis axis = Axes[axisIndex];
                double previousValue = Axes[axisIndex].AxisValues.RangeEnd[rangeIndex];
                double currentValue = 0;
                AxisRenderer.RangeRenderer.RangeDrag(x, y, rangeIndex, axisIndex, axis.AngleStart, axis.AngleEnd, axis.Direction, axis.Maximum != 0 ? axis.Maximum : 100, axis.Minimum, axis);
                currentValue = Axes[axisIndex].AxisValues.RangeEnd[rangeIndex];
                PointerDragEventArgs args = new PointerDragEventArgs() { AxisIndex = axisIndex, PointerIndex = 0, RangeIndex = rangeIndex, Name = "OnDragMove", Type = "rangeMove", CurrentValue = (float)currentValue, PreviousValue = (float)previousValue };
                await SfBaseUtils.InvokeEvent<PointerDragEventArgs>(CircularGaugeEvents?.OnDragMove, args);

                // TODO: The state change has been called since range drag didn’t work in IE 11 browser alone. Without this code it works fine in other browsers.
                if (IsIE)
                {
                    StateHasChanged();
                }
            }
        }

        /// <summary>
        /// The method is invoked when the paging in legend is enabled and clicked on the page element.
        /// </summary>
        /// <param name="currentPage">Specifies the current legend page number.</param>
        /// <param name="current">Specifies the current number of the page.</param>
        /// <exclude />
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void TriggerLegendPageClick(int currentPage, int current)
        {
            if (current >= 1 && current <= AxisRenderer.LegendRenderer.LegendSetting.TotalPages)
            {
                AxisRenderer.LegendRenderer.TranslatePage(currentPage, current);
            }

            StateHasChanged();
        }

        /// <summary>
        /// The method is used to animate the range bar pointer.
        /// </summary>
        /// <param name="midPointX">Specifies the x position of the mid point in the range bar pointer.</param>
        /// <param name="midPointY">Specifies the y position of the mid point in the range bar pointer.</param>
        /// <param name="rangeLinear">Specifies type of the range.</param>
        /// <param name="radius">Specifies the radius of the range.</param>
        /// <param name="innerRadius">Specifies the inner radius of the range.</param>
        /// <param name="minimumAngle">Specifies the minimum angle of the axis.</param>
        /// <param name="axisIndex">Specifies the index of the axis.</param>
        /// <param name="pointerIndex">Specifies the index of the pointer.</param>
        /// <exclude />
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void AnimateRangeBar(double midPointX, double midPointY, double rangeLinear, double radius, double innerRadius, double minimumAngle, double axisIndex, double pointerIndex)
        {
            AxisRenderer.PointerRenderer.RangeBarPathAnimation(GetMidPoint(midPointX, midPointY), rangeLinear, radius, innerRadius, minimumAngle, pointerIndex, axisIndex);
            AllowRefresh = false;
            StateHasChanged();
        }

        /// <summary>
        /// This method is used to animate the range bar pointer with rounded corners.
        /// </summary>
        /// <param name="midPointX">Specifies the x position of mid point of in range bar pointer element.</param>
        /// <param name="midPointY">Specifies the y position of mid point of in range bar pointer element.</param>
        /// <param name="actualStart">Specifies the actual start of the range.</param>
        /// <param name="actualEnd">Specifies the actual end of the range.</param>
        /// <param name="oldStart">Specifies the old start of the range.</param>
        /// <param name="oldEnd">Specifies the old end of the range.</param>
        /// <param name="radius">Specifies the radius of the range.</param>
        /// <param name="pointerWidth">Specifies the width of the pointer.</param>
        /// <param name="pointerIndex">Specifies the index of the pointer.</param>
        /// <param name="axisIndex">Specifies the index of the axis.</param>
        /// <param name="roundRadius">Specifies the range with rounded radius.</param>
        /// <exclude />
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void AnimateRoundedRangeBar(double midPointX, double midPointY, double actualStart, double actualEnd, double oldStart, double oldEnd, double radius, double pointerWidth, double pointerIndex, double axisIndex, double roundRadius)
        {
            AxisRenderer.PointerRenderer.RoundedRangeBarPathAnimation(GetMidPoint(midPointX, midPointY), actualStart, actualEnd, oldStart, oldEnd, radius, pointerWidth, pointerIndex, axisIndex, roundRadius);
            StateHasChanged();
        }

        /// <summary>
        /// The method is used to set the pointer value dynamically for circular gauge.
        /// </summary>
        /// <param name="axisIndex">Specifies the index number of the axis in which the pointer value is to be changed.</param>
        /// <param name="pointerIndex">Specifies the index number of the pointer in which the value is to be changed.</param>
        /// <param name="pointerValue">Specifies the value of the pointer to be updated in it.</param>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async void SetPointerValue(int axisIndex, int pointerIndex, double pointerValue)
        {
            await SetPointerValueAsync(axisIndex, pointerIndex, pointerValue);
        }

        /// <summary>
        /// The method is used to set the pointer value dynamically for circular gauge.
        /// </summary>
        /// <param name="axisIndex">Specifies the index number of the axis in which the pointer value is to be changed.</param>
        /// <param name="pointerIndex">Specifies the index number of the pointer in which the value is to be changed.</param>
        /// <param name="pointerValue">Specifies the value of the pointer to be updated in it.</param>
        /// <returns><see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task SetPointerValueAsync(int axisIndex, int pointerIndex, double pointerValue)
        {
            GetAxisAndPointer(axisIndex, pointerIndex, pointerValue);
            await InvokeAsync(() => StateHasChanged());
        }

        /// <summary>
        /// The method is used to set the range values dynamically for circular gauge.
        /// </summary>
        /// <param name="axisIndex">Specifies the index number of the axis in which the range value is to be changed.</param>
        /// <param name="rangeIndex">Specifies the index number of the range in which the value is to be changed.</param>
        /// <param name="start">Specifies the start value of the range to be updated in it.</param>
        /// <param name="end">Specifies the end value of the range to be updated in it.</param>
        public void SetRangeValue(int axisIndex, int rangeIndex, double start, double end)
        {
            GetAxisAndRange(axisIndex, rangeIndex, start, end);
        }

        /// <summary>
        /// This method is used to set the annotation content dynamically for circular gauge.
        /// </summary>
        /// <param name="axisIndex">Specifies the index number of the axis in which the annotation content is to be changed.</param>
        /// <param name="annotationIndex">Specifies the index number of the annotation in which the content is to be changed.</param>
        /// <param name="content">Specifies the content of the annotation to be updated in it.</param>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async void SetAnnotationValue(int axisIndex, int annotationIndex, string content)
        {
            await SetAnnotationValueAsync(axisIndex, annotationIndex, content);
        }

        /// <summary>
        /// This method is used to set the annotation content dynamically for circular gauge.
        /// </summary>
        /// <param name="axisIndex">Specifies the index number of the axis in which the annotation content is to be changed.</param>
        /// <param name="annotationIndex">Specifies the index number of the annotation in which the content is to be changed.</param>
        /// <param name="content">Specifies the content of the annotation to be updated in it.</param>
        /// <returns><see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task SetAnnotationValueAsync(int axisIndex, int annotationIndex, string content)
        {
            if (AxisRenderer != null)
            {
                List<Internal.Annotation> annotationCollection = AxisRenderer.AxisCollection[axisIndex].Annotations;
                if (annotationCollection.Count > 0 && annotationCollection[annotationIndex] != null)
                {
                    annotationCollection[annotationIndex].ContentTemplate = content;
                    Axes[axisIndex].AxisValues.AnnotationContent[annotationIndex] = content;
                    AllowRefresh = true;
                    await InvokeAsync(() => StateHasChanged());
                }
            }
        }

        /// <summary>
        /// This method renders the circular gauge component again.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async void Refresh()
        {
            await RefreshAsync();
        }

        /// <summary>
        /// This method renders the circular gauge component again.
        /// </summary>
        /// <returns><see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task RefreshAsync()
        {
            SizeD size = await InvokeMethod<SizeD>("sfBlazor.CircularGauge.getContainerSize", false, new object[] { ID, DotnetObjectReference });
            AvailableSize = ContainerSize(size.Width, size.Height);
            AllowRefresh = true;
            await InvokeAsync(() => StateHasChanged());
        }

        /// <summary>
        /// This method is invoked when the legend items are clicked.
        /// </summary>
        /// <param name="axisIndex">Specifies the index number of the axis.</param>
        /// <param name="legendIndex">Specifies the index number of the legend.</param>
        /// <exclude />
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void TriggerLegendClick(int axisIndex, int legendIndex)
        {
            if (LegendSettings != null && LegendSettings.Visible && LegendSettings.ToggleVisibility)
            {
                int rangeIndex = GetRangeIndex(axisIndex, legendIndex);
                Legend legendOptions = AxisRenderer.LegendRenderer.LegendSetting.LegendItemCollections[legendIndex];
                LegendIndex index = new LegendIndex()
                {
                    AxisIndex = axisIndex,
                    RangeIndex = legendIndex,
                    IsToggled = !legendOptions.LegendRender,
                };
                if (toggledIndexes == null && toggledIndexes.Count == 0)
                {
                    toggledIndexes.Add(index);
                }
                else
                {
                    for (int i = 0; i < toggledIndexes.Count; i++)
                    {
                        if (toggledIndexes[i].AxisIndex == index.AxisIndex &&
                            toggledIndexes[i].RangeIndex == index.RangeIndex)
                        {
                            toggledIndex = i;
                            break;
                        }
                        else
                        {
                            toggledIndex = -1;
                        }
                    }

                    if (toggledIndex == -1)
                    {
                        toggledIndexes.Add(index);
                    }
                    else
                    {
                        toggledIndexes[toggledIndex].IsToggled = toggledIndexes[toggledIndex].IsToggled;
                    }
                }

                for (int i = 0; i < toggledIndexes.Count; i++)
                {
                    if (toggledIndexes[i].AxisIndex == axisIndex && toggledIndexes[i].RangeIndex == legendIndex && !toggledIndexes[i].IsToggled)
                    {
                        toggledIndexes[i].IsToggled = true;
                        AxisRenderer.AxisCollection[axisIndex].RangeCollection[rangeIndex].Visibility = "visibility:hidden";
                        AxisRenderer.LegendRenderer.LegendSetting.LegendItemCollections[toggledIndexes[i].RangeIndex].LegendToggleFill = "#D3D3D3";
                        AxisRenderer.LegendRenderer.LegendSetting.LegendColors[toggledIndexes[i].RangeIndex].Fill = "#D3D3D3";
                    }
                    else if (toggledIndexes[i].AxisIndex == axisIndex && toggledIndexes[i].RangeIndex == legendIndex && toggledIndexes[i].IsToggled)
                    {
                        toggledIndexes[i].IsToggled = false;
                        AxisRenderer.AxisCollection[axisIndex].RangeCollection[rangeIndex].Visibility = "visibility:visible";
                        AxisRenderer.LegendRenderer.LegendSetting.LegendItemCollections[toggledIndexes[i].RangeIndex].LegendToggleFill = LegendSettings.TextStyle != null &&
                            !string.IsNullOrEmpty(LegendSettings.TextStyle.Color) ? LegendSettings.TextStyle.Color : "black";
                        AxisRenderer.LegendRenderer.LegendSetting.LegendColors[toggledIndexes[i].RangeIndex].Fill = AxisRenderer.AxisCollection[toggledIndexes[i].AxisIndex].RangeCollection[rangeIndex].RangeFillColor;
                    }
                }

                StateHasChanged();
            }
        }

        /// <summary>
        /// The method is used to perform the export functionality for the circular gauge.
        /// </summary>
        /// <param name="type">Specifies the format of the file to export the circular gauge.</param>
        /// <param name="fileName">Specifies the name of the file for the exported circular gauge.</param>
        /// <param name="orientation">Specifies the orientation of the exported PDF document when the type parameter is PDF. </param>
        /// <param name="allowDownload">Specifies whether the exported file is to be downloaded or not.</param>
        /// <returns>Returns base64 string of the exported circular gauge when allowDownload parameter is false.</returns>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<string> Export(ExportType type, string fileName, PdfPageOrientation? orientation = null, bool allowDownload = true)
        {
            return await ExportAsync(type, fileName, orientation, allowDownload);
        }

        /// <summary>
        /// The method is used to perform the export functionality for the circular gauge.
        /// </summary>
        /// <param name="type">Specifies the format of the file to export the circular gauge.</param>
        /// <param name="fileName">Specifies the name of the file for the exported circular gauge.</param>
        /// <param name="orientation">Specifies the orientation of the exported PDF document when the type parameter is PDF. </param>
        /// <param name="allowDownload">Specifies whether the exported file is to be downloaded or not.</param>
        /// <returns>Returns base64 string of the exported circular gauge when allowDownload parameter is false.</returns>
        public async Task<string> ExportAsync(ExportType type, string fileName, PdfPageOrientation? orientation = null, bool allowDownload = true)
        {
            if ((type != ExportType.PDF) && AllowImageExport)
            {
                return await InvokeMethod<string>("sfExport.exportToImage", false, new object[] { type.ToString(), fileName, ID + "_svg", allowDownload });
            }
            else if ((type == ExportType.PDF) && AllowPdfExport)
            {
                if (orientation == null)
                {
                    orientation = PdfPageOrientation.Portrait;
                }

                return await ExportToPdf(fileName, (PdfPageOrientation)orientation, allowDownload);
            }

            return null;
        }

        /// <summary>
        /// The method is used to print the rendered circular gauge.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Print()
        {
            await PrintAsync();
        }

        /// <summary>
        /// The method is used to print the rendered circular gauge.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task PrintAsync()
        {
            PrintEventArgs args = new PrintEventArgs() { Cancel = false };
            await SfBaseUtils.InvokeEvent<PrintEventArgs>(CircularGaugeEvents?.OnPrint, args);
            if (AllowPrint)
            {
                await InvokeMethod("sfExport.print", new object[] { Element });
            }
        }

        /// <summary>
        /// Calculate the conversion operation from string to number to the size of the circular gauge.
        /// </summary>
        /// <param name="numberString">represents the number value as string.</param>
        /// <param name="size">represents the size.</param>
        /// <returns>value.</returns>
        internal static double StringToNumber(string numberString, double size)
        {
            if (!string.IsNullOrEmpty(numberString))
            {
                bool isNumber = double.TryParse(numberString, out double numberValue);
                return numberString.IndexOf("%", StringComparison.InvariantCulture) != -1 ? (size / 100) * double.Parse(numberString.Replace("%", string.Empty, StringComparison.InvariantCulture), CultureInfo.InvariantCulture) :
                    numberString.IndexOf("px", StringComparison.InvariantCulture) != -1 ? double.Parse(numberString.Replace("px", string.Empty, StringComparison.InvariantCulture), CultureInfo.InvariantCulture) : (isNumber ? numberValue : 0);
            }

            return double.NaN;
        }

        /// <summary>
        /// The method is used to trigger when the values of the properties change.
        /// </summary>
        internal void PropertyChangeHandler()
        {
            if (AxisRenderer.AxisCollection.Count > 0)
            {
                AllowRefresh = true;
                StateHasChanged();
            }
        }

        /// <summary>
        /// The method is used to update legend toggle visibility in the client script.
        /// </summary>
        internal async Task UpdateToggleVisibility()
        {
            if (IsRendered)
            {
                await InvokeMethod("sfBlazor.CircularGauge.setLegendToggle", new object[] { Element, LegendSettings.ToggleVisibility });
            }
        }

        /// <summary>
        /// The method is used to trigger when the values of the properties change.
        /// </summary>
        internal async Task PropertyChangesHandle()
        {
            await ComponentRender();
            AllowRefresh = true;
            isPropertyChanged = true;
        }

        /// <summary>
        /// Defines the styles for the base div element of the circular gauge component.
        /// </summary>
        /// <returns>return the as in string.</returns>
        internal string GetStyle()
        {
            if (!string.IsNullOrEmpty(Width) || !string.IsNullOrEmpty(Height))
            {
                SetSvgSize();
                string height = !string.IsNullOrEmpty(Height) ? "height:" + Height + ";" : string.Empty;
                string width = !string.IsNullOrEmpty(Width) ? "width:" + Width + ";" : string.Empty;
                if (isInitialRender)
                {
                    isInitialRender = false;
                    return string.Empty;
                }

                return height + width + "position:relative;";
            }
            else
            {
                if (AvailableSize != null)
                {
                    SetSvgSize();
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Calculates the size of the circular gauge component.
        /// </summary>
        /// <param name="width">represents the width of the container.</param>
        /// <param name="height">represents the height of the container.</param>
        /// <returns>return the size.</returns>
        internal SizeD ContainerSize(double width, double height)
        {
            SizeD domElementAvailableSize = new SizeD()
            {
                Width = width != 0 ? width : 600,
                Height = height != 0 ? string.IsNullOrEmpty(Height) && height < 450 ? 450 : height : 450,
            };
            return domElementAvailableSize;
        }

        /// <summary>
        /// Disposes the property values during the destroy of the component that is hold up for the execution of the component.
        /// </summary>
        internal override void ComponentDispose()
        {
            AxisRenderer?.ComponentDispose();
#pragma warning disable CS4014
            UnWireEvents();
#pragma warning restore CS4014
            AvailableSize = null;
            toggledIndexes = null;
            AxisRenderer = null;
            Border = null;
            CircularGaugeEvents = null;
            LegendSettings = null;
            Margin = null;
            ThemeStyles = null;
            TitleStyle = null;
            Tooltip = null;
            Axes = null;
            Tooltip = null;
        }

        /// <summary>
        /// The method is used to set the pointer value dynamically for circular gauge.
        /// </summary>
        /// <param name="axisIndex">represents the index of the axis.</param>
        /// <param name="pointerIndex">represents the index of the pointer.</param>
        /// <param name="currentValue">represents the current value of the pointer.</param>
        internal void GetAxisAndPointer(int axisIndex, int pointerIndex, double currentValue)
        {
            if (AxisRenderer != null && AxisRenderer.PointerRenderer != null && AxisRenderer.AxisCollection.Count > 0 && AxisRenderer.AxisCollection[axisIndex].PointerCollection.Count > 0)
            {
                CircularGaugeAxis axis = Axes[axisIndex];
                AxisRenderer.PointerRenderer.SetPointerValue(axis, axis.Pointers[pointerIndex], currentValue, AxisRenderer.AxisCollection[axisIndex].PointerCollection[pointerIndex], AxisRenderer.AxisCollection[axisIndex]);
                axis.AxisValues.PointerValue[pointerIndex] = currentValue;
            }
        }

        /// <summary>
        /// The method is used to set the range value dynamically for circular gauge.
        /// </summary>
        /// <param name="axisIndex">represents the index of the axis.</param>
        /// <param name="rangeIndex">represents the index of the range.</param>
        /// <param name="start">represents the value of the start.</param>
        /// <param name="end">represents the value of the end.</param>
        internal void GetAxisAndRange(int axisIndex, int rangeIndex, double start, double end)
        {
            CircularGaugeAxis axis = Axes[axisIndex];
            Internal.Range range = AxisRenderer.AxisCollection[axisIndex].RangeCollection[rangeIndex];
            AxisRenderer.RangeRenderer.SetRangeValue(axis, axis.Ranges[rangeIndex], start, end, range);
            axis.AxisValues.RangeStart[rangeIndex] = start;
            axis.AxisValues.RangeEnd[rangeIndex] = end;
            if (axis.Ranges[rangeIndex].RoundedCornerRadius > 0)
            {
                range.RangeRoundedPath = AxisRenderer.RangeRenderer.RangeSetting.RangeRoundedPath;
            }
            else
            {
                range.RangePath = AxisRenderer.RangeRenderer.RangeSetting.RangePath;
            }

            StateHasChanged();
        }

        /// <summary>
        /// The method is used to animate the pointer value of the circular gauge.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        internal async Task PointerAnimation()
        {
            if (Axes != null)
            {
                for (int i = 0; i < (Axes != null ? Axes.Count : 0); i++)
                {
                    int pointerNeedleIndex = 0;
                    int rangeBarIndex = 0;
                    if (AxisRenderer.AxisCollection[i].PointerCollection.Count != 0)
                    {
                        for (int j = 0; j < (Axes != null ? Axes[i].Pointers.Count : 0); j++)
                        {
                            CircularGaugePointer pointer = Axes[i].Pointers[j];
#pragma warning disable CA1508
                            if (pointer.Animation == null || (pointer.Animation != null && pointer.Animation.Enable))
#pragma warning restore CA1508
                            {
                                string animationID = ID + "_Axis_" + i + "_Pointers_" + j;
                                if (pointer.Type == PointerType.RangeBar)
                                {
                                    await InvokeMethod("sfBlazor.CircularGauge.animationProcess", new object[] { Element, GetRangeAnimationInstance(AxisRenderer.AxisCollection[i].RangeAnimate[rangeBarIndex]), animationID });
                                    rangeBarIndex++;
                                }
                                else
                                {
                                    await InvokeMethod("sfBlazor.CircularGauge.animationProcess", new object[] { Element, GetPointerAnimationInstance(AxisRenderer.AxisCollection[i].PointerAnimate[pointerNeedleIndex]), animationID });
                                    pointerNeedleIndex++;
                                }
                            }
                        }
                    }
                }
            }
        }

        private static Dictionary<string, object> GetRangeAnimationInstance(AnimationOptions rangeAnimate)
        {
            Dictionary<string, object> circularGaugePointer = new Dictionary<string, object>();
            circularGaugePointer.Add("start", rangeAnimate.Start);
            circularGaugePointer.Add("duration", rangeAnimate.Duration);
            circularGaugePointer.Add("end", rangeAnimate.End);
            circularGaugePointer.Add("startAngle", rangeAnimate.StartAngle);
            circularGaugePointer.Add("endAngle", rangeAnimate.EndAngle);
            circularGaugePointer.Add("midPointX", rangeAnimate.MidPoint.X);
            circularGaugePointer.Add("midPointY", rangeAnimate.MidPoint.Y);
            circularGaugePointer.Add("isClockWise", rangeAnimate.IsClockWise);
            circularGaugePointer.Add("radius", rangeAnimate.Radius);
            circularGaugePointer.Add("innerRadius", rangeAnimate.InnerRadius);
            circularGaugePointer.Add("minimumAngle", rangeAnimate.MinimumAngle);
            circularGaugePointer.Add("oldStart", rangeAnimate.OldStart);
            circularGaugePointer.Add("pointerWidth", rangeAnimate.PointerWidth);
            circularGaugePointer.Add("roundRadius", rangeAnimate.RoundRadius);
            circularGaugePointer.Add("pointerType", "RangeBar");
            return circularGaugePointer;
        }

        private static MouseEventArgs GetMouseEventArgs(bool cancel, double x, double y)
        {
            return new MouseEventArgs() { Cancel = cancel, X = x, Y = y };
        }

        private static PointF GetMidPoint(double x, double y)
        {
            return new PointF { X = (float)x, Y = (float)y };
        }

        private static Dictionary<string, object> GetPointerAnimationInstance(AnimationOptions pointerAnimate)
        {
            Dictionary<string, object> circularGaugePointer = new Dictionary<string, object>();
            circularGaugePointer.Add("start", pointerAnimate.Start);
            circularGaugePointer.Add("duration", pointerAnimate.Duration);
            circularGaugePointer.Add("end", pointerAnimate.End);
            circularGaugePointer.Add("startAngle", pointerAnimate.StartAngle);
            circularGaugePointer.Add("endAngle", pointerAnimate.EndAngle);
            circularGaugePointer.Add("midPointX", pointerAnimate.MidPoint.X);
            circularGaugePointer.Add("midPointY", pointerAnimate.MidPoint.Y);
            circularGaugePointer.Add("isClockWise", pointerAnimate.IsClockWise);
            circularGaugePointer.Add("radius", pointerAnimate.Radius);
            circularGaugePointer.Add("innerRadius", pointerAnimate.InnerRadius);
            circularGaugePointer.Add("pointerType", "Needle");
            return circularGaugePointer;
        }

        private async Task<string> ExportToPdf(string fileName, PdfPageOrientation orientation, bool allowDownload)
        {
            string base64String = await JSRuntimeExtensions.InvokeAsync<string>(JSRuntime, "sfExport.exportToImage", new object[] { "PNG", fileName, ID + "_svg", false });
            string encodedString = base64String.Split("base64,")[1];
            byte[] data = Convert.FromBase64String(encodedString);
            using (MemoryStream initialStream = new MemoryStream(data))
            {
                Stream stream = initialStream as Stream;
                using (PdfDocument document = new PdfDocument())
                {
                    document.PageSettings.Orientation = orientation;
                    PdfPage page = document.Pages.Add();
                    PdfGraphics graphics = page.Graphics;
                    using (PdfBitmap image = new PdfBitmap(stream))
                    {
                        graphics.DrawImage(image, 0, 0);
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            document.Save(memoryStream);
                            memoryStream.Position = 0;
                            base64String = Convert.ToBase64String(memoryStream.ToArray());
                            if (allowDownload)
                            {
                                await InvokeMethod("sfExport.downloadPdf", new object[] { base64String, fileName });
                                base64String = string.Empty;
                            }
                            else
                            {
                                base64String = "data:application/pdf;base64," + base64String;
                            }

                            document.Dispose();
                        }
                    }
                }
            }

            return base64String;
        }

        private Dictionary<string, object> GetInstance()
        {
            Dictionary<string, object> circularGauge = new Dictionary<string, object>();
            circularGauge.Add("enablePointerDrag", EnablePointerDrag);
            circularGauge.Add("enableRangeDrag", EnableRangeDrag);
            circularGauge.Add("enableTooltip", Tooltip != null ? Tooltip.Enable : false);
            circularGauge.Add("tooltipType", Tooltip != null && Tooltip.Enable ? Tooltip.Type : null);
            circularGauge.Add("showPointerTooltipAtMousePosition", Tooltip != null ? Tooltip.ShowAtMousePosition : false);
            circularGauge.Add("showRangeTooltipAtMousePosition", Tooltip != null && Tooltip.RangeSettings != null ? Tooltip.RangeSettings.ShowAtMousePosition : false);
            circularGauge.Add("legendToggleVisibility", LegendSettings != null ? LegendSettings.ToggleVisibility : false);
            circularGauge.Add("height", Height);
            circularGauge.Add("width", Width);
            return circularGauge;
        }

        private void SetSvgSize()
        {
            svgHeight = AvailableSize != null ? AvailableSize.Height : 0;
            svgWidth = AvailableSize != null ? AvailableSize.Width : 0;
        }

        private async Task MouseMoveHandler(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
           await SfBaseUtils.InvokeEvent<MouseEventArgs>(CircularGaugeEvents?.OnGaugeMouseMove, GetMouseEventArgs(false, args.ClientX, args.ClientY));
        }

        private PointerDragEventArgs GetDragEventArguments(int axisIndex, int pointerIndex, int rangeIndex, string type, string name)
        {
            double dragValue = 0;
            if (type == "Range")
            {
                dragValue = Axes[axisIndex].AxisValues.RangeEnd[rangeIndex];
            }
            else
            {
                dragValue = Axes[axisIndex].AxisValues.PointerValue[pointerIndex];
            }

            return new PointerDragEventArgs() { AxisIndex = axisIndex, PointerIndex = pointerIndex, RangeIndex = rangeIndex, Name = name, Type = type, CurrentValue = (float)dragValue, PreviousValue = (float)dragValue };
        }

        private async Task AnnotationRerenderLocation()
        {
            if (Axes != null)
            {
                for (int j = 0; j < (Axes != null ? Axes.Count : 0); j++)
                {
                    for (int i = 0; i < (AxisRenderer != null ? AxisRenderer.AxisCollection[j].Annotations.Count : 0); i++)
                    {
                        Internal.Annotation annotation = AxisRenderer.AxisCollection[j].Annotations[i];
                        if (!annotation.IsTemplate)
                        {
                            BoundingClientRect annotationElement = await InvokeMethod<BoundingClientRect>("sfBlazor.CircularGauge.getElementBounds", false, new object[] { annotation.AnnotationID });
                            annotation.LeftPosition = (annotation.AnnotationLocation.X - (annotationElement.Width / 2)).ToString(culture);
                            annotation.TopPosition = (annotation.AnnotationLocation.Y - (annotationElement.Height / 2)).ToString(culture);
                            annotation.AnnotationPosition = "left:" + annotation.LeftPosition + "px" + ";" + "top:" + annotation.TopPosition + "px";
                            annotation.Visibility = "visibility:visible";
                        }
                    }
                }

                StateHasChanged();
            }
        }

        private int GetRangeIndex(int axisIndex, int legendIndex)
        {
            int rangeIndex = 0;
            for (int i = 0; i <= axisIndex; i++)
            {
                if (legendIndex < Axes[0].Ranges.Count)
                {
                    rangeIndex = legendIndex;
                    break;
                }
                else
                {
                    rangeIndex += Axes[i].Ranges.Count;
                    if (rangeIndex > legendIndex)
                    {
                        rangeIndex -= Axes[i].Ranges.Count;
                        rangeIndex = legendIndex - rangeIndex;
                    }
                }
            }

            return rangeIndex;
        }

        private async Task UnWireEvents()
        {
            await InvokeMethod("sfBlazor.CircularGauge.dispose", new object[] { Element });
        }
    }
}
