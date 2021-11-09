window.sfBlazor = window.sfBlazor || {};
window.sfBlazor.Diagram = (function () {
'use strict';

/**
 * Rect defines and processes rectangular regions
 */
var Rect = /** @class */ (function () {
    function Rect(x, y, width, height) {
        /**
         * Sets the x-coordinate of the starting point of a rectangular region
         *
         * @default 0
         */
        this.x = Number.MAX_VALUE;
        /**
         * Sets the y-coordinate of the starting point of a rectangular region
         *
         * @default 0
         */
        this.y = Number.MAX_VALUE;
        /**
         * Sets the width of a rectangular region
         *
         * @default 0
         */
        this.width = 0;
        /**
         * Sets the height of a rectangular region
         *
         * @default 0
         */
        this.height = 0;
        if (x === undefined || y === undefined) {
            x = y = Number.MAX_VALUE;
            width = height = 0;
        }
        else {
            if (width === undefined) {
                width = 0;
            }
            if (height === undefined) {
                height = 0;
            }
        }
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
    }
    /**   @private  */
    Rect.empty = new Rect(Number.MAX_VALUE, Number.MIN_VALUE, 0, 0);
    return Rect;
}());

/**
 * Size defines and processes the size(width/height) of the objects
 */
var Size = /** @class */ (function () {
    function Size(width, height) {
        this.width = width;
        this.height = height;
    }
    return Size;
}());

/** @private */
function applyStyleAgainstCsp(svg, attributes) {
    var keys = attributes.split(';');
    for (var i = 0; i < keys.length; i++) {
        var attribute = keys[i].split(':');
        if (attribute.length === 2) {
            svg.style[attribute[0].trim()] = attribute[1].trim();
        }
    }
}
/** @private */
function setAttributeHtml(element, attributes) {
    var keys = Object.keys(attributes);
    for (var i = 0; i < keys.length; i++) {
        if (keys[i] !== 'style') {
            element.setAttribute(keys[i], attributes[keys[i]]);
        }
        else {
            applyStyleAgainstCsp(element, attributes[keys[i]]);
        }
    }
}
/** @hidden */
function parentsUntil(elem, selector, isID) {
    var parent = elem;
    while (parent) {
        if (isID ? parent.id === selector : hasClass(parent, selector)) {
            break;
        }
        parent = parent.parentNode;
    }
    return parent;
}
/** @hidden */
function hasClass(element, className) {
    var eClassName = (typeof element.className === 'object') ? element.className.animVal : element.className;
    return ((' ' + eClassName + ' ').indexOf(' ' + className + ' ') > -1) ? true : false;
}
/**
 * Init Draggable
 */
var InitDraggable = /** @class */ (function () {
    function InitDraggable(parent, symbolPaletteInstance) {
        var _this = this;
        this.over = function (e) {
            var previewElementValue = "clonedNode";
            var symbolPaletteDragEnter = "SymbolPaletteDragEnter";
            var previewElement = document.getElementById(previewElementValue);
            var component;
            for (var i = _this.symbolPaletteInstance.length - 1; i >= 0; i--) {
                if (e.dragData.draggable.id === _this.symbolPaletteInstance[i].id) {
                    component = _this.symbolPaletteInstance[i].componentInstance;
                    break;
                }
            }
            component.invokeMethodAsync(symbolPaletteDragEnter, e.target.id.split("_")[0]);
            if (previewElement) {
                sf.base.remove(previewElement);
                var cloneNode = document.getElementsByClassName("e-cloneproperties e-draganddrop e-dragclone");
                cloneNode[0].style.width = "1px";
                cloneNode[0].style.height = "1px";
            }
        };
        this.drop = function (e) {
            var component;
            var symbolPaletteDrop = "SymbolPaletteDrop";
            var diagramClass = "e-diagram";
            for (var i = _this.symbolPaletteInstance.length - 1; i >= 0; i--) {
                if (e.dragData.draggable.id === _this.symbolPaletteInstance[i].id) {
                    component = _this.symbolPaletteInstance[i].componentInstance;
                    break;
                }
            }
            var ParentElement = parentsUntil(e.target, diagramClass);
            if (ParentElement && (e.event.changedTouches && e.event.changedTouches.length > 0)) {
                var palettecomponent = void 0;
                for (var i = _this.symbolPaletteInstance.length - 1; i >= 0; i--) {
                    palettecomponent = _this.symbolPaletteInstance[i].componentInstance;
                    break;
                }
                if (palettecomponent) {
                    window["sfBlazor"].Diagram.invokeDiagramEvents(e.event, palettecomponent, undefined, e);
                }
                component.invokeMethodAsync(symbolPaletteDrop, ParentElement.id, true);
            }
            else {
                component.invokeMethodAsync(symbolPaletteDrop, ParentElement.id, false);
            }
            sf.base.remove(e.droppedElement);
        };
        this.out = function (e) {
            var component;
            var symbolPaletteDragLeave = "SymbolPaletteDragLeave";
            for (var i = _this.symbolPaletteInstance.length - 1; i >= 0; i--) {
                if (e.target.children[0].id === _this.symbolPaletteInstance[i].id || e.target.parentNode.parentNode.parentElement.id === _this.symbolPaletteInstance[i].id
                    || (_this.draggable["target"] && _this.draggable["target"].id == _this.symbolPaletteInstance[i].id)) {
                    component = _this.symbolPaletteInstance[i].componentInstance;
                    break;
                }
            }
            if (component) {
                if (e.evt.changedTouches && e.evt.changedTouches.length > 0) {
                    window["sfBlazor"].Diagram.invokeDiagramEvents(e.evt, component, undefined, e);
                }
                component.invokeMethodAsync(symbolPaletteDragLeave);
            }
        };
        this.helper = function (e) {
            var previewID = "previewID";
            var accordianControl = "e-control e-accordion";
            var helperElement = "helperElement";
            var PaletteControl = document.getElementsByClassName(accordianControl)[0];
            var visualElement = sf.base.createElement('div', {
                className: 'e-cloneproperties e-draganddrop e-dragclone',
                styles: 'color:"transparent" height:"auto",  width:' + PaletteControl.offsetWidth
            });
            var previewElement = document.getElementById(previewID);
            if (previewElement === null) {
                previewElement = e.sender.target;
            }
            if (previewElement) {
                visualElement.setAttribute("id", helperElement);
                document.body.appendChild(visualElement);
                return visualElement;
            }
            return null;
        };
        this.dragStart = function (e) {
            e.bindEvents(e.dragElement);
        };
        this.drag = function (e) {
            var diagramClass = "e-diagram";
            if (!parentsUntil(e.target, diagramClass)) {
                var helperElement = "helperElement";
                var previewID = "previewID";
                var previewElement = document.getElementById(previewID);
                var canAllowDrag = void 0;
                for (var i = _this.symbolPaletteInstance.length - 1; i >= 0; i--) {
                    if (e.element.id === _this.symbolPaletteInstance[i].id) {
                        canAllowDrag = _this.symbolPaletteInstance[i].allowDrag;
                        break;
                    }
                }
                if (previewElement) {
                    previewElement.style.visibility = "";
                    var cloneNode = previewElement.cloneNode(true);
                    cloneNode.style.display = "Block";
                    cloneNode.style.visibility = true;
                    cloneNode.setAttribute("class", "e-cloneproperties e-draganddrop e-dragclone");
                    cloneNode.setAttribute("id", "clonedNode");
                    var dragHelperElement = document.getElementById(helperElement);
                    if (dragHelperElement && dragHelperElement.children[0]) {
                        dragHelperElement.removeChild(dragHelperElement.children[0]);
                    }
                    if (!canAllowDrag) {
                        dragHelperElement.style.opacity = "0";
                    }
                    dragHelperElement.appendChild(cloneNode);
                }
            }
            else {
                if (e.event.touches && e.event.touches.length > 0) {
                    var component = void 0;
                    for (var i = _this.symbolPaletteInstance.length - 1; i >= 0; i--) {
                        if (e.element.id === _this.symbolPaletteInstance[i].id) {
                            component = _this.symbolPaletteInstance[i].componentInstance;
                            break;
                        }
                    }
                    if (component) {
                        window["sfBlazor"].Diagram.invokeDiagramEvents(e.event, component, undefined, e);
                    }
                }
            }
        };
        this.dragStop = function (e) {
            var diagramClass = "e-diagram";
            var helperNode = document.getElementsByClassName("e-cloneproperties e-draganddrop e-dragclone");
            if (helperNode.length > 0) {
                helperNode[0].style.width = "1px";
                helperNode[0].style.height = "1px";
            }
            if (helperNode.length > 1) {
                for (var k = helperNode.length - 1; k > 0; k--) {
                    if (helperNode[k].id == "helperElement")
                        helperNode[k].remove();
                }
            }
            if (!parentsUntil(e.target, diagramClass)) {
                var helperElements = "helperElement";
                var helperElement = document.getElementById(helperElements);
                if (helperElement)
                    helperElement.remove();
                var component = void 0;
                var elementDropToOutSideDiagram = "ElementDropToOutSideDiagram";
                for (var i = _this.symbolPaletteInstance.length - 1; i >= 0; i--) {
                    if ((e.element.id === _this.symbolPaletteInstance[i].id)) {
                        component = _this.symbolPaletteInstance[i].componentInstance;
                        break;
                    }
                }
                if (component) {
                    component.invokeMethodAsync(elementDropToOutSideDiagram);
                }
            }
        };
        this.symbolPaletteInstance = symbolPaletteInstance;
        if (parent) {
            this.initializeDrag(parent);
        }
    }
    
    InitDraggable.prototype.initializeDrag = function (parent) {
        var element = parent.children[0];
        this.draggable = new sf.base.Draggable(element, {
            dragTarget: '.e-symbol-draggable',
            helper: this.helper,
            dragStart: this.dragStart,
            drag: this.drag,
            dragStop: this.dragStop,
            preventDefault: false
        });
        var droppableElements = document.getElementsByClassName("e-control e-diagram e-lib e-droppable e-tooltip");
        for (var i_1 = 0; i_1 < droppableElements.length; i_1++) {
            this.droppable = new sf.base.Droppable(droppableElements[i_1], {
                accept: '.e-dragclone',
                drop: this.drop,
                over: this.over,
                out: this.out
            });
        }
        var headerContent = document.getElementsByClassName("e-acrdn-header-content");
        for (var i = 0; i < headerContent.length; i++) {
            headerContent[i].style.textDecoration = "inherit";
        }
    };
    
    /**
     * To destroy the drag
     * @return {void}
     * @hidden
     */
    InitDraggable.prototype.destroy = function () {
        this.draggable.destroy();
    };
    
    return InitDraggable;
}());

var __awaiter = (undefined && undefined.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (undefined && undefined.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
var eventStarted = false;
var inAction = false;
var action = false;
var isMouseWheel = false;
var storeFormat;
var isZoomPan = false;
var scrollDiagramID = "";
var pageSettings;
var diagramId;
var defaultTextStyle = {
    bold: false, italic: false, fontFamily: 'Arial', color: 'black',
    textAlign: "Center", fontSize: 12.0, textDecoration: "None", textOverflow: "Wrap",
    textWrapping: "WrapWithOverflow", whiteSpace: "CollapseSpace"
};
var diagram = {
    createHtmlElement: function (elementType, attribute) {
        var element = sf.base.createElement(elementType);
        if (attribute) {
            this.setAttribute(element, attribute);
        }
        return element;
    },
    /**
 * setAttributeSvg method   \
 *
 * @returns {void} setAttributeSvg method .\
 * @param { SVGElement } svg - provide the svg  value.
 * @param { Object } attributes - provide the boolean  value.
 * @private
 */
    setAttributeSvg: function (svg, attributes) {
        var keys = Object.keys(attributes);
        for (var i = 0; i < keys.length; i++) {
            if (keys[i] !== 'style') {
                svg.setAttribute(keys[i], attributes[keys[i]]);
            }
            else {
                this.applyStyleAgainstCsp(svg, attributes[keys[i]]);
            }
        }
    },
    /**
     * applyStyleAgainstCsp method   \
     *
     * @returns {void} applyStyleAgainstCsp method .\
     * @param { SVGElement } svg - provide the svg  value.
     * @param { string } attributes - provide the boolean  value.
     * @private
     */
    applyStyleAgainstCsp: function (svg, attributes) {
        var keys = attributes.split(';');
        for (var i = 0; i < keys.length; i++) {
            var attribute = keys[i].split(':');
            if (attribute.length === 2) {
                svg.style[attribute[0].trim()] = attribute[1].trim();
            }
        }
    },
    setCanvasSize: function (canvas, width, height) {
        if (canvas) {
            canvas.setAttribute('width', width.toString());
            canvas.setAttribute('height', height.toString());
        }
    },
    getHTMLLayer: function (diagramId) {
        var htmlLayer = null;
        if (!window[diagramId + '_htmlLayer']) {
            var element = this.getDiagramElement(diagramId);
            // eslint-disable-next-line @typescript-eslint/no-explicit-any
            var elementcoll = element.getElementsByClassName('e-html-layer');
            htmlLayer = elementcoll[0];
            window[diagramId + '_htmlLayer'] = htmlLayer;
        }
        else {
            htmlLayer = window[diagramId + '_htmlLayer'];
        }
        return htmlLayer;
    },
    getDiagramElement: function (elementId, contentId) {
        var diagramElement;
        var element;
        if (contentId) {
            element = document.getElementById(contentId);
        }
        if (sf.base.Browser.info.name === 'msie' || sf.base.Browser.info.name === 'edge') {
            diagramElement = (element) ? element.querySelector('#' + elementId) : document.getElementById(elementId);
        }
        else {
            diagramElement = (element) ? element.querySelector('#' + CSS.escape(elementId)) : document.getElementById(elementId);
        }
        return diagramElement;
    },
    createCanvas: function (id, width, height) {
        var canvasObj = this.createHtmlElement('canvas', { 'id': id });
        this.setCanvasSize(canvasObj, width, height);
        return canvasObj;
    },
    setAttribute: function (element, attributes) {
        var keys = Object.keys(attributes);
        for (var i = 0; i < keys.length; i++) {
            element.setAttribute(keys[i], attributes[keys[i]]);
        }
    },
    createMeasureElements: function (isZoomValue, layerList, width, height, elementId, component, transform, patternSize, gridLinePathData, dotsGrid) {
        this.updateZoomPanTool(isZoomValue);
        this.updateInnerLayerSize(layerList, width, height, undefined, transform, patternSize, gridLinePathData, dotsGrid);
        if (elementId && component) {
            this.onAddWireEvents(elementId, component);
        }
        var measureWindowElement = 'measureElement';
        if (!window[measureWindowElement]) {
            var divElement = this.createHtmlElement('div', {
                id: 'measureElement',
                style: 'visibility:hidden ; height: 0px ; width: 0px; overflow: hidden;'
            });
            var text = this.createHtmlElement('span', { 'style': 'display:inline-block ; line-height: normal' });
            divElement.appendChild(text);
            var imageElement = void 0;
            imageElement = this.createHtmlElement('img', {});
            divElement.appendChild(imageElement);
            var svg = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
            svg.setAttribute('xlink', 'http://www.w3.org/1999/xlink');
            divElement.appendChild(svg);
            var element = document.createElementNS('http://www.w3.org/2000/svg', 'path');
            svg.appendChild(element);
            var tSpan = document.createElementNS('http://www.w3.org/2000/svg', 'text');
            tSpan.setAttributeNS('http://www.w3.org/XML/1998/namespace', 'xml:space', 'preserve');
            svg.appendChild(tSpan);
            window[measureWindowElement] = divElement;
            window[measureWindowElement].usageCount = 1;
            document.body.appendChild(divElement);
            var measureElementCount = 'measureElementCount';
            if (!window[measureElementCount]) {
                window[measureElementCount] = 1;
            }
            else {
                window[measureElementCount]++;
            }
        }
        else {
            window[measureWindowElement].usageCount += 1;
        }
    },
    updateZoomPanTool: function (val) {
        isZoomPan = val;
    },
    updateGridlines: function updateGridlines(patternId, patternSize, gridLinePathData, dotsGrid) {
        var layer = document.getElementById(patternId);
        if (layer) {
            layer.setAttribute("width", patternSize.width.toString());
            layer.setAttribute("height", patternSize.height.toString());
            if (layer.children.length > 0) {
                for (var k = 0; k < layer.children.length; k++) {
                    if (gridLinePathData && gridLinePathData.length) {
                        layer.children[k].setAttribute("d", gridLinePathData[k]);
                    }
                    else if (dotsGrid && dotsGrid.length) {
                        layer.children[k].setAttribute("cx", dotsGrid[k].x.toString());
                        layer.children[k].setAttribute("cy", dotsGrid[k].y.toString());
                    }
                }
            }
        }
    },
    updateInnerLayerSize: function (layerList, width, height, scrollValues, transform, patternSize, gridLinePathData, dotsGrid, attributeList, id) {
        var bounds;
        if (id) {
            bounds = this.measureScrollValues(id);
            width = bounds.width;
            height = bounds.height;
        }
        if (layerList != undefined && width != undefined && height != undefined && layerList.length > 0) {
            var layer = void 0;
            for (var i = 0; i < layerList.length - 1; i++) {
                layer = document.getElementById(layerList[i]);
                if (i >= layerList.length - 4) {
                    if (layerList[i].indexOf("_pattern") != -1) {
                        // Update the gridline's pattern while zooming
                        this.updateGridlines(layerList[i], patternSize, gridLinePathData, dotsGrid);
                    }
                    else if (layerList[i].indexOf("gridline") != -1 && transform && layer) {
                        var scale = transform.scale;
                        var x = -(transform.tx * scale);
                        var y = -(transform.ty * scale);
                        var gridLineTransform = "translate(" + (transform.tx * scale) + "," + (transform.ty * scale) + ")";
                        // Update the transform in gridline's element.
                        layer.setAttribute("transform", gridLineTransform);
                        layer.children[0].setAttribute("x", x.toString());
                        layer.children[0].setAttribute("y", y.toString());
                    }
                    else if (transform && layer) {
                        var scale = transform.scale;
                        var gTransform = "translate(" + (transform.tx * scale) + "," + (transform.ty * scale) + "),scale(" + scale + ")";
                        // Update the diagramLayer's transform while zooming and panning
                        layer.setAttribute("transform", gTransform);
                        // Update the html layer's transform while zooming and panning
                        var htmlLayerDiv = document.getElementById(layerList[i].split("_")[0] + "_htmlLayer_div");
                        var style = "position: absolute; top: 0px; left: 0px; pointer-events: all; transform: translate(" + (transform.tx * scale) + "px," + (transform.ty * scale) + "px) scale(" + scale + ")";
                        htmlLayerDiv.setAttribute("style", style);
                        // Update the selector while zooming or panning the diagram.
                        var selectorlayer = document.getElementById(layerList[i].split("_")[0] + "_SelectorElement");
                        if (selectorlayer.children.length > 0 && selectorlayer.children.length == attributeList.length) {
                            for(var selectorChildrenCount =0; selectorChildrenCount<selectorlayer.children.length; selectorChildrenCount++)
                            {
                                if (selectorlayer.children[selectorChildrenCount].tagName == "line") {
                                    var lineAttributes = attributeList[selectorChildrenCount];
                                    var x1 = (lineAttributes.startPoint.x + lineAttributes.x);
                                    var y1 = (lineAttributes.startPoint.y + lineAttributes.y);
                                    var x2 = (lineAttributes.endPoint.x + lineAttributes.x);
                                    var y2 = (lineAttributes.endPoint.y + lineAttributes.y);
                                    selectorlayer.children[selectorChildrenCount].setAttribute("x1", x1.toString());
                                    selectorlayer.children[selectorChildrenCount].setAttribute("y1", y1.toString());
                                    selectorlayer.children[selectorChildrenCount].setAttribute("x2", x2.toString());
                                    selectorlayer.children[selectorChildrenCount].setAttribute("y2", y2.toString());
                                    selectorlayer.children[selectorChildrenCount].setAttribute("transform", "rotate(" + lineAttributes.angle + " " + (lineAttributes.x + lineAttributes.width * lineAttributes.pivotX) + " " + (lineAttributes.y + lineAttributes.height * lineAttributes.pivotY) + ")");
                                    selectorChildrenCount += 1;
                                }
                                else if (selectorlayer.children[selectorChildrenCount].tagName == "path") {
                                    var pathAttributes = attributeList[selectorChildrenCount];
                                    selectorlayer.children[selectorChildrenCount].setAttribute("transform", "rotate(" + pathAttributes.angle + "," + (pathAttributes.x + pathAttributes.width * pathAttributes.pivotX) + "," + (pathAttributes.y + pathAttributes.height * pathAttributes.pivotY) + ")" + "translate(" + pathAttributes.x + "," + pathAttributes.y + ")");
                                    selectorChildrenCount += 1;
                                }
                                else if (selectorlayer.children[selectorChildrenCount].tagName == "rect") {
                                    var rectAttributes = attributeList[selectorChildrenCount];
                                    selectorlayer.children[selectorChildrenCount].setAttribute("x", rectAttributes.x);
                                    selectorlayer.children[selectorChildrenCount].setAttribute("y", rectAttributes.y);
                                    selectorlayer.children[selectorChildrenCount].setAttribute("width", rectAttributes.width);
                                    selectorlayer.children[selectorChildrenCount].setAttribute("height", rectAttributes.height);
                                    selectorlayer.children[selectorChildrenCount].setAttribute("transform", "rotate(" + rectAttributes.angle + "," + (rectAttributes.x + rectAttributes.width * rectAttributes.pivotX) + "," + (rectAttributes.y + rectAttributes.height * rectAttributes.pivotY) + ")");
                                    selectorChildrenCount += 1;
                                }
                                else if (selectorlayer.children[selectorChildrenCount].tagName == "circle") {
                                    selectorlayer.children[selectorChildrenCount].setAttribute("cx", attributeList[selectorChildrenCount].cx);
                                    selectorlayer.children[selectorChildrenCount].setAttribute("cy", attributeList[selectorChildrenCount].cy);
                                }
                            }
                        }
                    }
                }
                else if (layer) {
                    layer.style.width = width;
                    layer.style.height = height;
                }
            }
        }
        if (isMouseWheel) {
            isMouseWheel = false;
        }
        if (!id && layerList != undefined && layerList.length > 0) {
            var element = document.getElementById(layerList[layerList.length - 1]);
            return this.onChangeScrollValues(element, scrollValues);
        }
        return bounds;
    },
    onChangeScrollValues: function (element, scrollValues) {
        if (eventStarted) {
            eventStarted = false;
        }
        if (element != undefined && scrollValues != undefined && scrollValues.x != undefined && scrollValues.y != undefined) {
            element.scrollLeft = scrollValues.x;
            element.scrollTop = scrollValues.y;
            return this.measureScrollValues(scrollDiagramID);
        }
        return null;
    },
    measureScrollValues: function (diagramId) {
        var element = document.getElementById(diagramId);
        var point = new Rect(element.scrollLeft, element.scrollTop, element.scrollWidth, element.scrollHeight);
        return point;
    },
    measurePath: function (data) {
        if (data) {
            var measureWindowElement = 'measureElement';
            window[measureWindowElement].style.visibility = 'visible';
            var svg = window[measureWindowElement].children[2];
            var element = this.getChildNode(svg)[0];
            element.setAttribute('d', data);
            var bounds = element.getBBox();
            var svgBounds = new Rect(bounds.x, bounds.y, bounds.width, bounds.height);
            window[measureWindowElement].style.visibility = 'hidden';
            return svgBounds;
        }
        return new Rect(0, 0, 0, 0);
    },
    openUrl: function (url) {
        return __awaiter(this, void 0, void 0, function () {
            return __generator(this, function (_a) {
                window.open(url, '_blank');
                return [2 /*return*/];
            });
        });
    },
    textEdit: function (annotation, centerPoint, nodeBounds, transform, canZoomTextEdit, annottaionId) {
        var _this = this;
        var textBoxDiv = "diagram_editTextBoxDiv";
        var editBoxDiv = "diagram_editBox";
        var textEditing = document.getElementById(textBoxDiv);
        var x;
        var y;
        annotation.id = annottaionId;
        var attributes;
        var textArea = document.getElementById(editBoxDiv);
        var text = annotation.content;
        if (!textEditing && !textArea) {
            textEditing = this.createHtmlElement('div', {});
            textArea = this.createHtmlElement('textarea', {});
            var diagramDiv = document.getElementsByClassName("e-control e-diagram e-lib e-droppable e-tooltip");
            var diagramCanvas = diagramDiv[0].children[0];
            diagramCanvas.appendChild(textEditing);
            textEditing.appendChild(textArea);
            textArea.appendChild(document.createTextNode(text));
            this[textBoxDiv] = textEditing;
            this[editBoxDiv] = textArea;
        }
        var minWidth = 90;
        var maxWidth = annotation.bounds.width < nodeBounds.width ? annotation.bounds.width : nodeBounds.width;
        maxWidth = minWidth > maxWidth ? minWidth : maxWidth;
        var bounds = this.measureHtmlText(annotation.style, text, undefined, undefined, maxWidth);
        if (bounds.width == 0 && bounds.height == 0) {
            bounds.width = 50;
            bounds.height = 12;
        }
        var scale = canZoomTextEdit ? transform.scale : 1;
        bounds.width = Math.max(bounds.width, 50);
        x = ((((centerPoint.x + transform.tx) * transform.scale) - (bounds.width / 2) * scale) - 2.5);
        y = ((((centerPoint.y + transform.ty) * transform.scale) - (bounds.height / 2) * scale) - 3);
        attributes = {
            'id': 'diagram' + '_editTextBoxDiv', 'style': 'position: absolute' + ';left:' + x + 'px;top:' +
                y + 'px;width:' + ((bounds.width + 1) * scale) + 'px;height:' + (bounds.height * scale) +
                'px; containerName:' + 'temp' + ';'
        };
        setAttributeHtml(textEditing, attributes);
        var style = (annotation.style);
        attributes = {
            'id': 'diagram' + '_editBox', 'style': 'width:' + ((bounds.width + 1) * scale) +
                'px;height:' + (bounds.height * scale) + 'px;resize: none;outline: none;overflow: hidden;' +
                ';font-family:' + style.fontFamily +
                ';font-size:' + (style.fontSize * scale) + 'px;text-align:' +
                (annotation.style.textAlign.toLocaleLowerCase()) + ';', 'class': 'e-diagram-text-edit'
        };
        setAttributeHtml(textArea, attributes);
        textArea.style.fontWeight = (style.bold) ? 'bold' : '';
        textArea.style.fontStyle = (style.italic) ? 'italic' : '';
        textArea.style.lineHeight = (style.fontSize * 1.2 + 'px;').toString();
        var nodeTextbox = document.getElementById(annotation.id + "_text");
        if (nodeTextbox) {
            nodeTextbox.setAttribute("visibility", "hidden");
        }
        textArea.style.textDecoration = (style.textDecoration) ? style.textDecoration : '';
        textArea.addEventListener('input', function (e) { _this.inputChange(e, annotation, nodeBounds, centerPoint, transform, canZoomTextEdit); });
        textArea.select();
        window['annotation'] = annotation;
    },
    inputChange: function (e, annotation, nodeBounds, centerPoint, transform, canZoomTextEdit) {
        var minWidth = 90;
        var textBoxDiv = "diagram_editTextBoxDiv";
        var editBoxDiv = "diagram_editBox";
        var maxWidth;
        var minHeight = 12;
        var fontsize;
        var height;
        var width;
        var textBounds;
        var editTextBox = this[editBoxDiv];
        var editTextBoxDiv = this[textBoxDiv];
        var text = (editTextBox.value);
        var line = text.split('\n');
        maxWidth = nodeBounds.width < annotation.bounds.width ? nodeBounds.width : annotation.bounds.width;
        maxWidth = maxWidth > minWidth ? maxWidth : minWidth;
        textBounds = this.measureHtmlText(annotation.style, text, undefined, undefined, maxWidth);
        fontsize = Number((editTextBox.style.fontSize).split('px')[0]);
        if (line.length > 1 && line[line.length - 1] === '') {
            textBounds.height = textBounds.height + fontsize;
        }
        var scale = canZoomTextEdit ? transform.scale : 1;
        width = textBounds.width;
        width = ((minWidth > width) ? minWidth : width) * scale;
        height = ((minHeight > textBounds.height) ? minHeight : textBounds.height) * scale;
        editTextBoxDiv.style.left = ((((centerPoint.x + transform.tx) * transform.scale) - width / 2) - 2.5) + 'px';
        editTextBoxDiv.style.top = ((((centerPoint.y + transform.ty) * transform.scale) - height / 2) - 3) + 'px';
        editTextBoxDiv.style.width = width + 'px';
        editTextBoxDiv.style.height = height + 'px';
        editTextBox.style.minHeight = minHeight + 'px';
        editTextBox.style.minWidth = minWidth + 'px';
        editTextBox.style.width = width + 'px';
        editTextBox.style.height = height + 'px';
    },
    measureHtmlText: function (style, content, width, height, maxWidth) {
        var bounds = {};
        var text = this.createHtmlElement('span', { 'style': 'display:inline-block; line-height: normal' });
        if (style.bold) {
            text.style.fontWeight = 'bold';
        }
        if (style.italic) {
            text.style.fontStyle = 'italic';
        }
        if (width !== undefined) {
            text.style.width = width.toString() + 'px';
        }
        if (height !== undefined) {
            text.style.height = height.toString() + 'px';
        }
        if (maxWidth !== undefined) {
            text.style.maxWidth = maxWidth.toString() + 'px';
        }
        text.style.fontFamily = style.fontFamily;
        text.style.fontSize = style.fontSize + 'px';
        text.style.color = style.color;
        text.textContent = content;
        text.style.whiteSpace = this.whiteSpaceToString(style.whiteSpace, style.textWrapping);
        if (maxWidth !== undefined) {
            text.style.wordBreak = 'break-word';
        }
        else {
            text.style.wordBreak = this.wordBreakToString(style.textWrapping);
        }
        document.body.appendChild(text);
        bounds.width = text.offsetWidth;
        bounds.height = text.offsetHeight;
        document.body.removeChild(text);
        return bounds;
    },
    getChildNode: function (node) {
        var child;
        var collection = [];
        if (sf.base.Browser.info.name === 'msie' || sf.base.Browser.info.name === 'edge') {
            for (var i = 0; i < node.childNodes.length; i++) {
                child = node.childNodes[i];
                if (child.nodeType === 1) {
                    collection.push(child);
                }
            }
        }
        else {
            collection = node.children;
        }
        return collection;
    },
    measureBounds: function (pathobj, textObj, imageObj, nativeObj) {
        return __awaiter(this, void 0, void 0, function () {
            var previousValue, accordianPanel, k, finalResult, pathResult, textResult, imageResult, nativeResult, measureWindowElement, result, svg, element, i, data, bounds, svgBounds, dom, bounds, result, i, data, content, style, size, nodeSz, images, value, result, result, results, nativeSize, i, nativeId, nativeBounds, svgBounds, k;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        if (nativeObj != null) {
                            accordianPanel = document.getElementsByClassName("e-acrdn-panel e-content-hide");
                            previousValue = [];
                            for (k = 0; k < accordianPanel.length; k++) {
                                previousValue[k] = accordianPanel[k].style.display;
                                accordianPanel[k].style.display = "block";
                            }
                        }
                        finalResult = {};
                        pathResult = {};
                        textResult = {};
                        imageResult = {};
                        nativeResult = {};
                        measureWindowElement = 'measureElement';
                        if (pathobj) {
                            result = Object.keys(pathobj).map(function (key) { return [pathobj[key], key]; });
                            window[measureWindowElement].style.visibility = 'visible';
                            svg = window[measureWindowElement].children[2];
                            element = this.getChildNode(svg)[0];
                            for (i = 0; i < result.length; i++) {
                                if (result[i][0] == "Path") {
                                    data = result[i][1];
                                    element.setAttribute('d', data);
                                    bounds = element.getBBox();
                                    svgBounds = { x: bounds.x, y: bounds.y, width: bounds.width, height: bounds.height };
                                    pathResult[data] = svgBounds;
                                }
                                else if (result[i][0].indexOf("GetBoundingClientRect") != -1) {
                                    dom = document.getElementById(result[i][1]);
                                    if (dom) {
                                        bounds = dom.getBoundingClientRect();
                                        pathResult[result[i][0]] = { x: bounds.x, y: bounds.y, width: bounds.width, height: bounds.height };
                                    }
                                    if (result[i][0] == "GetBoundingClientRect") {
                                        scrollDiagramID = result[i][1];
                                        pathResult["GetScrollerBounds"] = this.measureScrollValues(result[i][1]);
                                    }
                                }
                            }
                        }
                        if (textObj) {
                            result = Object.keys(textObj).map(function (key) { return [textObj[key], key]; });
                            for (i = 0; i < result.length; i++) {
                                data = result[i][1];
                                content = textObj[data].content;
                                style = textObj[data].style;
                                size = textObj[data].bounds;
                                nodeSz = textObj[data].nodeSize;
                                size.width = size.width == null ? undefined : size.width;
                                size.height = size.height == null ? undefined : size.height;
                                nodeSz.width = nodeSz.width == null ? undefined : nodeSz.width;
                                nodeSz.height = nodeSz.height == null ? undefined : nodeSz.height;
                                textResult[data] = this.measureText(size, style, content, (size.width || nodeSz.width));
                            }
                        }
                        if (!imageObj) return [3 /*break*/, 2];
                        images = Object.keys(imageObj).map(function (key) { return [imageObj[key], key]; });
                        if (!(images.length > 0)) return [3 /*break*/, 2];
                        value = 0;
                        result = {};
                        return [4 /*yield*/, this.loadImage(images, value, result)];
                    case 1:
                        _a.sent();
                        imageResult = result;
                        _a.label = 2;
                    case 2:
                        if (nativeObj) {
                            result = Object.keys(nativeObj).map(function (key) { return [nativeObj[key], key]; });
                            if (result.length > 0) {
                                results = {};
                                nativeSize = {};
                                for (i = 0; i < result.length; i++) {
                                    nativeId = result[i][0];
                                    nativeBounds = document.getElementById(nativeId);
                                    svgBounds = nativeBounds.getBoundingClientRect();
                                    nativeSize = { width: svgBounds.width, height: svgBounds.height };
                                    results[result[i][1]] = nativeSize;
                                }
                                nativeResult = results;
                            }
                        }
                        pathResult["GetScrollerWidth"] = this.getScrollerWidth();
                        finalResult["Path"] = pathResult;
                        finalResult["Text"] = textResult;
                        finalResult["Image"] = imageResult;
                        finalResult["Native"] = nativeResult;
                        if (previousValue != null) {
                            for (k = 0; k < accordianPanel.length; k++) {
                                accordianPanel[k].style.display = previousValue[k].toString();
                            }
                        }
                        window[measureWindowElement].style.visibility = 'hidden';
                        return [2 /*return*/, finalResult];
                }
            });
        });
    },
    measureText: function (size, style, content, maxWidth, textValue) {
        var finalResult = {};
        var bounds = { width: 0, height: 0 };
        var childNodes;
        var wrapBounds;
        var options = this.getTextOptions(content, size, style, maxWidth);
        childNodes = this.wrapSvgText(options, textValue, maxWidth);
        wrapBounds = this.wrapSvgTextAlign(options, childNodes);
        bounds.width = wrapBounds.width;
        if (wrapBounds.width >= maxWidth && options.textOverflow !== 'Wrap') {
            bounds.width = maxWidth;
        }
        bounds.height = childNodes.length * options.fontSize * 1.2;
        if (wrapBounds.width > options.width && options.textOverflow !== 'Wrap' && options.textWrapping === 'NoWrap') {
            childNodes[0].text = this.overFlow(options.content, options);
        }
        finalResult["Bounds"] = bounds;
        finalResult["WrapBounds"] = wrapBounds;
        finalResult["ChildNodes"] = childNodes;
        return finalResult;
    },
    getTextOptions: function (content, size, style, maxWidth) {
        var options = {
            fill: style.fill, stroke: style.strokeColor,
            strokeWidth: style.strokeWidth,
            dashArray: style.strokeDashArray, opacity: style.opacity,
            gradient: style.gradient,
            width: maxWidth || size.width, height: size.height,
        };
        options.fontSize = style.fontSize || defaultTextStyle.fontSize;
        options.fontFamily = style.fontFamily || defaultTextStyle.fontFamily;
        options.textOverflow = style.textOverflow || defaultTextStyle.textOverflow;
        options.textDecoration = style.textDecoration || defaultTextStyle.textDecoration;
        options.doWrap = style.doWrap;
        options.whiteSpace = this.whiteSpaceToString(style.whiteSpace || defaultTextStyle.whiteSpace, style.textWrapping || defaultTextStyle.textWrapping);
        options.content = content;
        options.textWrapping = style.textWrapping || defaultTextStyle.textWrapping;
        options.breakWord = this.wordBreakToString(style.textWrapping || defaultTextStyle.textWrapping);
        options.textAlign = this.textAlignToString(style.textAlign || defaultTextStyle.textAlign);
        options.color = style.color || defaultTextStyle.color;
        options.italic = style.italic || defaultTextStyle.italic;
        options.bold = style.bold || defaultTextStyle.bold;
        options.dashArray = '';
        options.strokeWidth = 0;
        options.fill = '';
        return options;
    },
    whiteSpaceToString: function (value, wrap) {
        if (wrap === 'NoWrap' && value === 'PreserveAll') {
            return 'pre';
        }
        var state = '';
        switch (value) {
            case 'CollapseAll':
                state = 'nowrap';
                break;
            case 'CollapseSpace':
                state = 'pre-line';
                break;
            case 'PreserveAll':
                state = 'pre-wrap';
                break;
        }
        return state;
    },
    wordBreakToString: function (value) {
        var state = '';
        switch (value) {
            case 'Wrap':
                state = 'breakall';
                break;
            case 'NoWrap':
                state = 'keepall';
                break;
            case 'WrapWithOverflow':
                state = 'normal';
                break;
            case 'LineThrough':
                state = 'line-through';
                break;
        }
        return state;
    },
    textAlignToString: function (value) {
        var state = '';
        switch (value) {
            case 'Center':
                state = 'center';
                break;
            case 'Left':
                state = 'left';
                break;
            case 'Right':
                state = 'right';
                break;
        }
        return state;
    },
    wrapSvgText: function (text, textValue, laneWidth) {
        var childNodes = [];
        var k = 0;
        var txtValue;
        var bounds1;
        var content = textValue || text.content;
        if (text.whiteSpace !== 'nowrap' && text.whiteSpace !== 'pre') {
            if (text.breakWord === 'breakall') {
                txtValue = '';
                txtValue += content[0];
                for (k = 0; k < content.length; k++) {
                    bounds1 = this.bBoxText(txtValue, text);
                    if (bounds1 >= text.width && txtValue.length > 0) {
                        childNodes[childNodes.length] = { text: txtValue, x: 0, dy: 0, width: bounds1 };
                        txtValue = '';
                    }
                    else {
                        txtValue = txtValue + (content[k + 1] || '');
                        if (txtValue.indexOf('\n') > -1) {
                            childNodes[childNodes.length] = { text: txtValue, x: 0, dy: 0, width: this.bBoxText(txtValue, text) };
                            txtValue = '';
                        }
                        var width = this.bBoxText(txtValue, text);
                        if (Math.ceil(width) + 2 >= text.width && txtValue.length > 0) {
                            childNodes[childNodes.length] = { text: txtValue, x: 0, dy: 0, width: width };
                            txtValue = '';
                        }
                        if (k === content.length - 1 && txtValue.length > 0) {
                            childNodes[childNodes.length] = { text: txtValue, x: 0, dy: 0, width: width };
                            txtValue = '';
                        }
                    }
                }
            }
            else {
                childNodes = this.wordWrapping(text, textValue, laneWidth);
            }
        }
        else {
            childNodes[childNodes.length] = { text: content, x: 0, dy: 0, width: this.bBoxText(content, text) };
        }
        return childNodes;
    },
    wordWrapping: function (text, textValue, laneWidth) {
        var childNodes = [];
        var txtValue = '';
        var j = 0;
        var i = 0;
        var wrap = text.whiteSpace !== 'nowrap' ? true : false;
        var content = textValue || text.content;
        var eachLine = content.split('\n');
        var words;
        var newText;
        var existingWidth;
        var existingText;
        for (j = 0; j < eachLine.length; j++) {
            words = text.textWrapping !== 'NoWrap' ? eachLine[j].split(' ') : (text.textWrapping === 'NoWrap') ? [eachLine[j]] : eachLine;
            for (i = 0; i < words.length; i++) {
                txtValue += (((i !== 0 || words.length === 1) && wrap && txtValue.length > 0) ? ' ' : '') + words[i];
                newText = txtValue + ' ' + (words[i + 1] || '');
                var width = this.bBoxText(newText, text);
                if (Math.floor(width) > (laneWidth || text.width) - 2 && txtValue.length > 0) {
                    childNodes[childNodes.length] = {
                        text: txtValue, x: 0, dy: 0,
                        width: newText === txtValue ? width : (txtValue === existingText) ? existingWidth : this.bBoxText(txtValue, text)
                    };
                    txtValue = '';
                }
                else {
                    if (i === words.length - 1) {
                        childNodes[childNodes.length] = { text: txtValue, x: 0, dy: 0, width: width };
                        txtValue = '';
                    }
                }
                existingText = newText;
                existingWidth = width;
            }
        }
        return childNodes;
    },
    wrapSvgTextAlign: function (text, childNodes) {
        var wrapBounds = { x: 0, width: 0 };
        var k = 0;
        var txtWidth;
        var width;
        for (k = 0; k < childNodes.length; k++) {
            txtWidth = childNodes[k].width;
            width = txtWidth;
            if (text.textAlign === 'left') {
                txtWidth = 0;
            }
            else if (text.textAlign === 'center') {
                if (txtWidth > text.width && (text.textOverflow === 'Ellipsis' || text.textOverflow === 'Clip')) {
                    txtWidth = 0;
                }
                else {
                    txtWidth = -txtWidth / 2;
                }
            }
            else if (text.textAlign === 'right') {
                txtWidth = -txtWidth;
            }
            else {
                txtWidth = childNodes.length > 1 ? 0 : -txtWidth / 2;
            }
            childNodes[k].dy = text.fontSize * 1.2;
            childNodes[k].x = txtWidth;
            if (!wrapBounds) {
                wrapBounds = {
                    x: txtWidth,
                    width: width
                };
            }
            else {
                wrapBounds.x = Math.min(wrapBounds.x, txtWidth);
                wrapBounds.width = Math.max(wrapBounds.width, width);
            }
        }
        return wrapBounds;
    },
    overFlow: function (text, options) {
        var i = 0;
        var j = 0;
        var middle = 0;
        var bounds = 0;
        var temp = '';
        j = text.length;
        var t = 0;
        do {
            if (bounds > 0) {
                i = middle;
            }
            middle = Math.floor(this.middleElement(i, j));
            temp += text.substr(i, middle);
            bounds = this.bBoxText(temp, options);
        } while (bounds <= options.width);
        temp = temp.substr(0, i);
        for (t = i; t < j; t++) {
            temp += text[t];
            bounds = this.bBoxText(temp, options);
            if (bounds >= options.width) {
                text = text.substr(0, temp.length - 1);
                break;
            }
        }
        if (options.textOverflow === 'Ellipsis') {
            text = text.substr(0, text.length - 3);
            text += '...';
        }
        else {
            text = text.substr(0, text.length);
        }
        return text;
    },
    middleElement: function (i, j) {
        var m = 0;
        m = (i + j) / 2;
        return m;
    },
    getScrollerWidth: function () {
        var outer = this.createHtmlElement('div', { 'style': 'visibility:hidden; width: 100px' });
        document.body.appendChild(outer);
        var widthNoScroll = outer.getBoundingClientRect().width;
        outer.style.overflow = 'scroll';
        var inner = this.createHtmlElement('div', { 'style': 'width:100%' });
        outer.appendChild(inner);
        var widthWithScroll = inner.getBoundingClientRect().width;
        outer.parentNode.removeChild(outer);
        var svgBounds = { x: 0, y: 0, width: widthNoScroll - widthWithScroll, height: 0 };
        return svgBounds;
    },
    pathPoints: function (pathPointsObj) {
        return __awaiter(this, void 0, void 0, function () {
            var pathPoints, result, i, data;
            return __generator(this, function (_a) {
                pathPoints = {};
                if (pathPointsObj) {
                    result = Object.keys(pathPointsObj).map(function (key) { return [pathPointsObj[key], key]; });
                    for (i = 0; i < result.length; i++) {
                        if (result.length > 0) {
                            data = result[i][1];
                            pathPoints[result[i][0]] = this.findSegmentPoints(data);
                        }
                    }
                }
                return [2 /*return*/, pathPoints];
            });
        });
    },
    findSegmentPoints: function (pathData) {
        var pts = [];
        var sample;
        var sampleLength;
        var measureWindowElement = 'measureElement';
        window[measureWindowElement].style.visibility = 'visible';
        var svg = window[measureWindowElement].children[2];
        var pathNode = this.getChildNode(svg)[0];
        pathNode.setAttributeNS(null, 'd', pathData);
        var pathLength = pathNode.getTotalLength();
        for (sampleLength = 0; sampleLength <= pathLength; sampleLength += 10) {
            sample = pathNode.getPointAtLength(sampleLength);
            pts.push({ x: sample.x, y: sample.y });
        }
        window[measureWindowElement].style.visibility = 'hidden';
        return pts;
    },
    loadImage: function (images, value, result) {
        return __awaiter(this, void 0, void 0, function () {
            var promise, _a, _b;
            return __generator(this, function (_c) {
                switch (_c.label) {
                    case 0:
                        promise = new Promise(function (resolve, reject) {
                            var imageSize = {};
                            var measureWindowElement = 'measureElement';
                            window[measureWindowElement].style.visibility = 'visible';
                            var imageElement = window[measureWindowElement].children[1];
                            imageElement.setAttribute('src', images[value][0]);
                            window[measureWindowElement].style.visibility = 'hidden';
                            var element = document.createElement('img');
                            element.setAttribute('src', images[value][0]);
                            setAttributeHtml(element, { id: "imagesf" + value + "imageNode", style: 'display: none;' });
                            document.body.appendChild(element);
                            element.onload = function (event) {
                                var loadedImage = event.currentTarget;
                                imageSize = { width: loadedImage.width, height: loadedImage.height };
                                resolve(imageSize);
                            };
                        });
                        _a = result;
                        _b = images[value][1];
                        return [4 /*yield*/, promise];
                    case 1:
                        _a[_b] = _c.sent();
                        if (!(value == (images.length - 1))) return [3 /*break*/, 2];
                        return [2 /*return*/, result];
                    case 2:
                        value++;
                        return [4 /*yield*/, this.loadImage(images, value, result)];
                    case 3:
                        _c.sent();
                        _c.label = 4;
                    case 4: return [2 /*return*/];
                }
            });
        });
    },
    bBoxText: function (textContent, options) {
        var measureWindowElement = 'measureElement';
        window[measureWindowElement].style.visibility = 'visible';
        var svg = window[measureWindowElement].children[2];
        var text = this.getChildNode(svg)[1];
        text.textContent = textContent;
        applyStyleAgainstCsp(text, 'font-size:' + options.fontSize + 'px; font-family:'
            + options.fontFamily + ';font-weight:' + (options.bold ? 'bold' : 'normal'));
        var bBox = text.getBBox().width;
        window[measureWindowElement].style.visibility = 'hidden';
        return bBox;
    },
    endEdit: function (e) {
        setTimeout(function () {
            var nodeTextbox = document.getElementById(annotation.id + "_text");
            if (nodeTextbox) {
                nodeTextbox.setAttribute("visibility", "visible");
            }
        }, 100);
        var textBoxDiv = "diagram_editTextBoxDiv";
        var editBoxDiv = "diagram_editBox";
        var textArea = this[editBoxDiv];
        var annotation = window['annotation'];
        var content = textArea.value;
        var style = annotation.style;
        var size = annotation.bounds;
        var nodeSize = annotation.nodeSize;
        nodeSize.width = nodeSize.width == null ? undefined : nodeSize.width;
        nodeSize.height = nodeSize.height == null ? undefined : nodeSize.height;
        var annotationValue = {};
        var finalResult = {};
        annotationValue[annotation.id] = this.measureText(size, style, content, (size.width || nodeSize.width));
        annotationValue[annotation.id].Content = content;
        finalResult["Text"] = annotationValue;
        var textBoxEditElement = this[textBoxDiv];
        textBoxEditElement.remove();
        return finalResult;
    },
    onAddWireEvents: function (id, component) {
        var _this = this;
        var element = document.getElementById(id);
        element.addEventListener('mousedown', function (e) { _this.invokeDiagramEvents(e, component); });
        window.addEventListener('resize', function (e) { _this.invokeDiagramEvents(e, component, id); });
        element.addEventListener('mousemove', function (e) { _this.invokeDiagramEvents(e, component); });
        element.addEventListener('mouseup', function (e) { _this.invokeDiagramEvents(e, component); });
        element.addEventListener('mouseleave', function (e) { _this.invokeDiagramEvents(e, component); });
        // Invoke the touch events for the diagram content
        element.addEventListener('touchstart', function (e) { _this.invokeDiagramEvents(e, component); });
        element.addEventListener('touchmove', function (e) { _this.invokeDiagramEvents(e, component); });
        element.addEventListener('touchend', function (e) { _this.invokeDiagramEvents(e, component); });
        element.addEventListener('scroll', function (e) { _this.invokeDiagramEvents(e, component); });
        element.addEventListener('mousewheel', function (e) { _this.invokeDiagramEvents(e, component); });
        element.addEventListener('keydown', function (e) { _this.invokeDiagramEvents(e, component); });
        element.addEventListener('keyup', function (e) { _this.invokeDiagramEvents(e, component); });
    },
    invokeDiagramEvents: function (e, component, id, paletteDragAndDrop) {
        var finalResult;
        var textBoxDiv = "diagram_editTextBoxDiv";
        var editTextBoxDiv = this[textBoxDiv];
        var args = {};
        if (e.type == "mousedown" || e.type == "touchstart") {
            action = true;
        }
        if (e.type == "mouseup" || e.type == "touchend" || e.type == "mouseleave") {
            action = false;
        }
        if ((e.type == "mouseleave" || e.key == "Escape" || ((e.type == "mousedown" || e.type == "touchstart") && e.target.id != "diagram_editBox")) && editTextBoxDiv) {
            window['isEscape'] = e.key == "Escape" ? true : false;
            finalResult = this.endEdit(e, this);
            this[textBoxDiv] = editTextBoxDiv = document.getElementById("diagram_editTextBoxDiv");
            e.currentTarget.focus();
        }
        args = this.getMouseEvents(e, id, paletteDragAndDrop);
        if (e.key == "Escape") {
            var helperElements = "helperElement";
            var helperElement = document.getElementById(helperElements);
            if (helperElement) {
                sf.base.remove(helperElement);
                helperElement.remove();
            }
        }
        // eventStarted value is used to whether the diagram is pan or not in mouse move, So if you change the if condition need to check the Zoom and pan functionality in manually.
        if (!editTextBoxDiv && (((e.type == "mousemove" || e.type == "touchmove") && !eventStarted) || (e.type != "mousemove" && e.type != "touchmove") || !inAction)) {
            // if we zoom the diagram using touch, this if condition (e.type == "touchmove" && !isMouseWheel) should return false.
            if ((e.type == "mousemove" || (e.type == "touchmove" && !isMouseWheel)) && action && isZoomPan) {
                // if we start the panning the diagram eventStarted value is set as true then after the return the c# to Js for changing the scroll values the "eventStarted" is changed to false.
                eventStarted = true;
                args.isPan = true;
            }
            if (paletteDragAndDrop) {
                component.invokeMethodAsync('InvokeDiagramEvents', args);
            }
            else if (e.type == "keydown" || e.type == "keyup" || e.type != "scroll" || (e.type == "scroll" && !isMouseWheel)) {
                component.invokeMethodAsync('InvokeDiagramEvents', args, finalResult);
            }
        }
        if ((e.type == "mouseup" || e.type == "mousemove" || e.type == "touchend" || e.type == "touchmove") && e.currentTarget && e.currentTarget.id && !this[textBoxDiv]) {
            e.currentTarget.focus();
        }
    },
    isForeignObject: function (target, isTextBox) {
        var foreignobject = target;
        if (foreignobject) {
            while (foreignobject.parentNode !== null) {
                if (typeof foreignobject.className === 'string' &&
                    ((!isTextBox && foreignobject.className.indexOf('foreign-object') !== -1) ||
                        (isTextBox && foreignobject.className.indexOf('e-diagram-text-edit') !== -1))) {
                    return foreignobject;
                }
                else {
                    foreignobject = foreignobject.parentNode;
                }
            }
        }
        return null;
    },
    getTouches: function (evt) {
        var touches = [];
        if (evt && evt.touches && evt.touches.length > 0) {
            for (var i = 0; i < evt.touches.length; i++) {
                touches.push({
                    pageX: evt.touches[i].pageX,
                    pageY: evt.touches[i].pageY,
                    screenX: evt.touches[i].screenX,
                    screenY: evt.touches[i].screenY,
                    clientX: evt.touches[i].clientX,
                    clientY: evt.touches[i].clientY
                });
            }
        }
        return touches;
    },
    getMouseEvents: function (evt, id, paletteDragAndDrop) {
        var mouseEventArgs = {};
        if (evt.type.indexOf("touch") != -1) {
            mouseEventArgs = {
                altKey: evt.altKey,
                shiftKey: evt.shiftKey,
                ctrlKey: evt.ctrlKey,
                detail: evt.detail,
                metaKey: evt.metaKey,
                type: evt.type,
            };
            mouseEventArgs.touches = this.getTouches(evt);
            if (evt.changedTouches && evt.changedTouches[0]) {
                mouseEventArgs.offsetX = evt.changedTouches[0].clientX;
                mouseEventArgs.offsetY = evt.changedTouches[0].clientY;
            }
            if (paletteDragAndDrop && paletteDragAndDrop.name == "out") {
                mouseEventArgs.type = "mouseleave";
            }
            if (evt.type == "touchmove") {
                evt.preventDefault();
            }
        }
        else {
            mouseEventArgs = {
                altKey: evt.altKey, shiftKey: evt.shiftKey, ctrlKey: evt.ctrlKey, detail: evt.detail,
                metaKey: evt.metaKey, screenX: evt.screenX, screenY: evt.screenY,
                clientX: evt.clientX, clientY: evt.clientY,
                offsetX: evt.offsetX, offsetY: evt.offsetY, type: evt.type, key: evt.key, keyCode: evt.keyCode,
                button: evt.button
            };
        }
        if (id && evt.type == 'resize') {
            var bounds = this.measureScrollValues(id);
            var element = document.getElementById(id);
            mouseEventArgs.diagramCanvasScrollBounds = bounds;
            mouseEventArgs.diagramGetBoundingClientRect = element.getBoundingClientRect();
        }
        else if (paletteDragAndDrop) {
            if (paletteDragAndDrop.target.id && paletteDragAndDrop.target.id != "") {
                var diagramID = parentsUntil(paletteDragAndDrop.target, "e-diagram").id;
                var bounds = this.measureScrollValues(diagramID);
                mouseEventArgs.diagramCanvasScrollBounds = bounds;
                mouseEventArgs.diagramGetBoundingClientRect = document.getElementById(diagramID + "_content").getBoundingClientRect();
            }
        }
        else {
            if (evt.currentTarget && evt.currentTarget.id) {
                var bounds = this.measureScrollValues(evt.currentTarget.id);
                mouseEventArgs.diagramCanvasScrollBounds = bounds;
                mouseEventArgs.diagramGetBoundingClientRect = evt.currentTarget.getBoundingClientRect();
            }
            else if (evt.target) {
                var bounds = this.measureScrollValues(evt.target.id);
                mouseEventArgs.diagramCanvasScrollBounds = bounds;
                mouseEventArgs.diagramGetBoundingClientRect = evt.target.getBoundingClientRect();
            }
        }
        if (evt.type == "mousewheel") {
            evt.preventDefault();
            evt.currentTarget.focus();
            mouseEventArgs.wheelDelta = evt.wheelDelta;
            isMouseWheel = true;
        }
        if (evt.type == "touchmove" && evt.touches.length > 1) {
            isMouseWheel = true;
        }
        if (evt.type == "mousedown" || evt.type == "touchstart") {
            inAction = true;
        }
        if (evt.type == "mouseup" || evt.type == "touchend") {
            inAction = false;
        }
        if (!id && !this.isForeignObject(evt.target, true) && (evt.type == "mouseleave" || evt.type == "mousmove" || evt.type == "mousedown" || evt.type == "mouseup" || evt.type == "keydown" || evt.type == "keyup")) {
            evt.preventDefault();
        }
        return mouseEventArgs;
    },
    //Symbol palette Snippet Starts here 
    initialiseModule: function (element, component, allowDrag) {
        return __awaiter(this, void 0, void 0, function () {
            var symbolPaletteInstance, object, object;
            var _this = this;
            return __generator(this, function (_a) {
                symbolPaletteInstance = 'symbolPaletteInstance';
                if (window[symbolPaletteInstance]) {
                    object = { id: element.children[0].id, componentInstance: component, allowDrag: allowDrag };
                    window[symbolPaletteInstance].push(object);
                }
                else {
                    object = { id: element.children[0].id, componentInstance: component, allowDrag: allowDrag };
                    window[symbolPaletteInstance] = [];
                    window[symbolPaletteInstance].push(object);
                }
                element.addEventListener('mousedown', function (e) { _this.invokePaletteEvents(e, component); });
                element.addEventListener('mousemove', function (e) { _this.invokePaletteEvents(e, component); });
                element.addEventListener('mouseup', function (e) { _this.invokePaletteEvents(e, component); });
                element.addEventListener('mouseleave', function (e) { _this.invokePaletteEvents(e, component); });
                element.addEventListener('touchstart', function (e) { _this.invokePaletteEvents(e, component); });
                element.addEventListener('touchmove', function (e) { _this.invokePaletteEvents(e, component); });
                element.addEventListener('touchend', function (e) { _this.invokePaletteEvents(e, component); });
                setTimeout(function () {
                    this.symbolPaletteDragAndDropModule = new InitDraggable(element, window[symbolPaletteInstance]);
                }, 100);
                return [2 /*return*/];
            });
        });
    },
    invokePaletteEvents: function (e, component) {
        var invokePaletteEvents = "InvokePaletteEvents";
        var symbolDraggableClass = "e-symbol-draggable";
        var symbolhoverClass = "e-symbol-hover";
        var symbolId;
        e.preventDefault();
        var args = this.palettegetMouseEvents(e);
        if (e.target.id.split('_').length === 2) {
            symbolId = e.target.id.split('_')[0];
        }
        if ((e.type == "mousemove" && !eventStarted) || e.type != "mousemove" || !inAction) {
            if (e && e.target && e.type) {
                var element = document.getElementById(e.target.id);
                var container = void 0;
                if (element) {
                    for (var k = 0; k < element.classList.length; k++) {
                        if (element.classList[k] == symbolDraggableClass) {
                            container = e.target;
                            container.classList.add(symbolhoverClass);
                            break;
                        }
                    }
                    var hoverElementCount = document.getElementsByClassName(symbolhoverClass);
                    if (hoverElementCount && hoverElementCount.length > 0) {
                        for (var a = 0; a < hoverElementCount.length; a++) {
                            var oldcontainer = hoverElementCount[a];
                            if (container && (container != oldcontainer) || container == undefined) {
                                oldcontainer.classList.remove(symbolhoverClass);
                            }
                        }
                    }
                }
            }
            component.invokeMethodAsync(invokePaletteEvents, args, symbolId);
        }
    },
    palettegetMouseEvents: function (evt) {
        var mouseEventArgs = {};
        if (evt.type.indexOf("touch") != -1) {
            mouseEventArgs = {
                altKey: evt.altKey,
                shiftKey: evt.shiftKey,
                ctrlKey: evt.ctrlKey,
                detail: evt.detail,
                metaKey: evt.metaKey,
                type: evt.type
            };
            mouseEventArgs.touches = this.getTouches(evt);
            if (evt.changedTouches && evt.changedTouches[0]) {
                mouseEventArgs.offsetX = evt.changedTouches[0].clientX;
                mouseEventArgs.offsetY = evt.changedTouches[0].clientY;
            }
            if (evt.type == "touchmove") {
                evt.preventDefault();
            }
        }
        else {
            mouseEventArgs = {
                altKey: evt.altKey, shiftKey: evt.shiftKey, ctrlKey: evt.ctrlKey, detail: evt.detail,
                metaKey: evt.metaKey, screenX: evt.screenX, screenY: evt.screenY,
                clientX: evt.clientX, clientY: evt.clientY,
                offsetX: evt.offsetX, offsetY: evt.offsetY, type: evt.type
            };
        }
        return mouseEventArgs;
    },
    /**
     * To Export the diagram
     *
     * @private
     */
    exportDiagram: function (options, diagramId, pbounds, pageSettings, canPrint, name, fileformat, isbase64) {
        return __awaiter(this, void 0, void 0, function () {
            var instance, promise;
            var _this = this;
            return __generator(this, function (_a) {
                instance = this;
                promise = new Promise(function (resolve, reject) { return __awaiter(_this, void 0, void 0, function () {
                    var data, buffers, margin, bounds, pbbounds, clipbund, fileName, attr, img;
                    return __generator(this, function (_a) {
                        switch (_a.label) {
                            case 0:
                                data = 'data';
                                diagramId = diagramId;
                                pageSettings = pageSettings;
                                
                                margin = options.margin || {};
                                options.region = options && options.region == 0 ? 'PageSettings' : options.region == 1 ? 'Content' : 'ClipBounds';
                                options.fileName = name;
                                options.mode = isbase64 ? 'Download' : 'Data';
                                options.format = fileformat && fileformat === 0 ? 'JPEG' :
                                    (fileformat === 1) ? 'PNG' : fileformat === 2 ? 'SVG' : fileformat;
                                options.orientation = options && options.orientation === 0 ? 'Landscape' : 'Portrait';
                                bounds = pbounds;
                                pbbounds = new Rect(pbounds.x, pbounds.y, pbounds.width, pbounds.height);
                                clipbund = options.clipBounds;
                                if (clipbund) {
                                    bounds.x = clipbund.x ? clipbund.x : bounds.x;
                                    bounds.y = clipbund.y ? clipbund.y : bounds.y;
                                    bounds.width = clipbund.width || bounds.width;
                                    bounds.height = clipbund.height || bounds.height;
                                }
                                margin = {
                                    top: margin.top,
                                    bottom: margin.bottom,
                                    right: margin.right,
                                    left: margin.left
                                };
                                if (options.region !== 'PageSettings') {
                                    bounds.x -= margin.left;
                                    bounds.y -= margin.top;
                                    bounds.width += margin.left + margin.right;
                                    bounds.height += margin.top + margin.bottom;
                                }
                                fileName = options.fileName || 'diagram';
                                return [4 /*yield*/, instance.setCanvas(options, bounds, pbbounds, margin, fileName, pageSettings, diagramId)];
                            case 1:
                                data = _a.sent();
                                if (data !== null) {
                                    if (canPrint) {
                                        resolve(data);
                                    }
                                    else {
                                        attr = {
                                            'id': diagramId + '_ExportImage',
                                            'src': data
                                        };
                                        options.margin = { top: 0, bottom: 0, right: 0, left: 0 };
                                        img = instance.createHtmlElement('img', attr);
                                        img.onload = function () {
                                            var div = instance.getMultipleImage(img, options, diagramId, true, pageSettings);
                                            resolve(JSON.stringify(div));
                                        };
                                    }
                                }
                                return [2 /*return*/];
                        }
                    });
                }); });
                return [2 /*return*/, promise];
            });
        });
    },
    setCanvas: function (options, bounds, pbounds, margin, fileName, pageSettings, diagramId) {
        return __awaiter(this, void 0, void 0, function () {
            var content, pagebounds, scaleX, scaleY, scaleOffsetX, scaleOffsetY, updatedbounds, canvas, image;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        options.clipBounds = bounds;
                        pagebounds = new Rect(pbounds.x, pbounds.y, pbounds.width, pbounds.height);
                        scaleX = 'scaleX';
                        scaleY = 'scaleY';
                        scaleOffsetX = 'scaleOffsetX';
                        scaleOffsetY = 'scaleOffsetY';
                        updatedbounds = new Rect(bounds.x, bounds.y, bounds.width, bounds.height);
                        this.setScaleValueforCanvas(options, bounds, pageSettings);
                        return [4 /*yield*/, this.exportAsImage({
                                clipBounds: bounds, margin: margin, region: options.region, scaleX: options[scaleX],
                                scaleY: options[scaleY], scaleOffsetX: options[scaleOffsetX], scaleOffsetY: options[scaleOffsetY]
                            }, fileName, diagramId, options, pagebounds, updatedbounds, pageSettings, options.mode === 'Download')];
                    case 1:
                        canvas = _a.sent();
                        image = content = storeFormat = (options.format === 'JPEG') ? canvas.toDataURL('image/jpeg') :
                            (options.format === 'PNG') ? canvas.toDataURL('image/png') : (options.format === 'BMP') ? canvas.toDataURL('image/bmp') : canvas.toDataURL();
                        if (options.mode === 'Data') {
                            if (options.format === 'SVG') {
                                return [2 /*return*/, canvas];
                            }
                            return [2 /*return*/, content];
                        }
                        this.canvasMultiplePage(options, canvas, margin, image, fileName);
                        return [2 /*return*/, null];
                }
            });
        });
    },
    exportAsImage: function (options, fileName, diagramId, updatedoptions, pageBounds, updatedbounds, pageSettings, isReturnBase64) {
        return __awaiter(this, void 0, void 0, function () {
            var returnValue;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, this.imageExport(options, fileName, diagramId, updatedoptions, pageBounds, updatedbounds, pageSettings, isReturnBase64)];
                    case 1:
                        returnValue = _a.sent();
                        if (returnValue instanceof Promise) {
                            returnValue.then(function (data) {
                                return data;
                            });
                            return [2 /*return*/, returnValue];
                        }
                        else {
                            return [2 /*return*/, returnValue];
                        }
                        return [2 /*return*/];
                }
            });
        });
    },
    imageExport: function (options, fileName, elementId, updatedoptions, pageBounds, updatedbounds, pageSettings, isReturnBase64) {
        return __awaiter(this, void 0, void 0, function () {
            var instance, element, promise;
            return __generator(this, function (_a) {
                instance = this;
                element = document.getElementById(elementId);
                promise = new Promise(function (resolve, reject) {
                    var region = options.clipBounds;
                    var margin = options.margin;
                    var left = pageBounds.x;
                    var top = pageBounds.y;
                    var width = pageBounds.width;
                    var height = pageBounds.height;
                    var svgData = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
                    var attr = {
                        'x': String(updatedbounds.x),
                        'y': String(updatedbounds.y),
                        'width': String(updatedbounds.width), 'height': String(updatedbounds.height)
                    };
                    instance.setAttributeSvg(svgData, attr);
                    attr = {
                        'x': String(left),
                        'y': String(top), 'width': String(width + margin.left + margin.right), 'height': String(height + margin.top + margin.bottom)
                    };
                    var backgroudImage = document.getElementById(elementId + '_backgroundLayer_svg').cloneNode(true);
                    var diagramLayer = document.getElementById(elementId + '_diagramLayer' + '_svg').cloneNode(true);
                    if (backgroudImage) {
                        // removing pagebreak element.
                        for (var i = 0; i < backgroudImage.childNodes.length; i++) {
                            var element = backgroudImage.childNodes[i];
                            if (element.id === elementId + '_backgroundLayer') {
                                for (var j = element.childNodes.length - 1; j >= 0; j--) {
                                    var innerElement = element.childNodes[j];
                                    if (innerElement.id !== elementId + '_backgroundLayerrect') {
                                        element.removeChild(innerElement);
                                    }
                                }
                            }
                        }
                        if (updatedoptions.region !== 'PageSettings')
                            instance.setAttributeSvg(backgroudImage, attr);
                        svgData.appendChild(backgroudImage);
                        if (updatedoptions.region !== 'PageSettings') {
                            instance.setTransform(backgroudImage, updatedbounds, margin);
                            for (var i = 0; i < backgroudImage.childNodes.length; i++) {
                                var element = backgroudImage.childNodes[i];
                                if (element.id === elementId + '_backgroundImageLayer' || element.id === elementId + '_backgroundLayer') {
                                    instance.setTransform(element, updatedbounds, margin);
                                }
                            }
                        }
                    }
                    svgData.appendChild(diagramLayer);
                    if (updatedoptions.region !== 'PageSettings') {
                        instance.setTransform(diagramLayer, updatedbounds, margin);
                        for (var i = 0; i < diagramLayer.childNodes.length; i++) {
                            var element = diagramLayer.childNodes[i];
                            if (element.id === elementId + '_diagramlayer') {
                                instance.setTransform(element, updatedbounds, margin);
                            }
                        }
                    }
                    var serializer = 'XMLSerializer';
                    var url = window.URL.createObjectURL(new Blob([new window[serializer]().serializeToString(svgData)], { type: 'image/svg+xml' }));
                    if (updatedoptions.format === 'SVG') {
                        var svg;
                        if (updatedoptions.mode === 'Data') {
                            if (updatedoptions.format === 'SVG') {
                                svg = new window[serializer]().serializeToString(svgData);
                                resolve(svg);
                            }
                        }
                        var buffers = [];
                        var buffer = new window[serializer]().serializeToString(svgData);
                        buffers.push(buffer);
                        var b = 0, blob;
                        if (updatedoptions.mode === 'Download') {
                            for (b = 0; b < buffers.length; b++) {
                                blob = new Blob([buffers[b]], { type: 'application/octet-stream' });
                                if (sf.base.Browser.info.name === 'msie') {
                                    window.navigator.msSaveOrOpenBlob(blob, fileName + '.' + 'svg');
                                }
                                else {
                                    instance.triggerDownload('SVG', fileName, url);
                                }
                            }
                        }
                    }
                    else {
                        var canvasElement_1 = document.createElement('canvas');
                        canvasElement_1.height = options.clipBounds.height;
                        canvasElement_1.width = options.clipBounds.width;
                        var context = canvasElement_1.getContext('2d');
                        if (updatedoptions.region === 'PageSettings') {
                            context.translate(-region.x, -region.y);
                            context.save();
                            context.fillStyle = (pageSettings.background.background === 'transparent') ? 'white' :
                                pageSettings.background.background;
                            region = options.region === 'Content' ? pageBounds : region;
                            context.fillRect(region.x, region.y, region.width, region.height);
                        }
                        else {
                            context.fillStyle = (pageSettings.background.background === 'transparent') ? 'white' : pageSettings.background.background;
                            context.fillRect(0, 0, options.clipBounds.width, options.clipBounds.height);
                        }
                        var image_1 = new Image();
                        image_1.onload = function () {
                            var w = pageBounds.width;
                            var h = pageBounds.height;
                            var width;
                            var height;
                            var xx = 0;
                            var yy = 0;
                            if (canvasElement_1.width < w) {
                                xx = w / canvasElement_1.width;
                            }
                            else {
                                xx = canvasElement_1.width / w;
                            }
                            if (canvasElement_1.height < h) {
                                yy = h / canvasElement_1.height;
                            }
                            else {
                                yy = canvasElement_1.height / h;
                            }
                            if (xx > yy) {
                                if (canvasElement_1.width < w) {
                                    width = w / xx;
                                    height = h / xx;
                                }
                                else {
                                    width = w;
                                    height = h;
                                }
                            }
                            else {
                                if (canvasElement_1.height < h) {
                                    width = w / yy;
                                    height = h / yy;
                                }
                                else {
                                    width = w;
                                    height = h;
                                }
                            }
                            context.drawImage(image_1, 0, 0, width, height);
                            window.URL.revokeObjectURL(url);
                            var base64String = canvasElement_1;
                            resolve(base64String);
                        };
                        image_1.src = url;
                    }
                });
                return [2 /*return*/, promise];
            });
        });
    },
    triggerDownload: function (type, fileName, url) {
        var anchorElement = document.createElement('a');
        anchorElement.download = fileName + '.' + type.toLocaleLowerCase();
        anchorElement.href = url;
        anchorElement.click();
    },
    canvasMultiplePage: function (options, canvas, margin, image, fileName) {
        var _this = this;
        var images = [];
        var imageData = image.substring(image.indexOf(":") + 1, image.indexOf(";"));
        var imageFormat = imageData.substring(imageData.indexOf("/") + 1);
        if (imageFormat === 'jpeg') {
            imageFormat = undefined;
        }
        else {
            imageFormat = imageFormat.toUpperCase();
        }
        var fileType = imageFormat || 'JPG';
        if (options.fitToPage) {
            options.pageHeight = options.pageHeight ? options.pageHeight : pageSettings.height;
            options.pageWidth = options.pageWidth ? options.pageWidth : pageSettings.width;
            options.pageHeight = options.pageHeight ? options.pageHeight : canvas.width;
            options.pageWidth = options.pageWidth ? options.pageWidth : canvas.height;
            margin = options.margin || {};
            if (options.orientation) {
                if ((options.orientation === 'Landscape' && options.pageHeight > options.pageWidth) ||
                    options.orientation === 'Portrait' && options.pageWidth > options.pageHeight) {
                    var temp = options.pageWidth;
                    options.pageWidth = options.pageHeight;
                    options.pageHeight = temp;
                }
            }
            options.margin = {
                top: !isNaN(margin.top) ? margin.top : 0,
                bottom: !isNaN(margin.bottom) ? margin.bottom : 0,
                left: !isNaN(margin.left) ? margin.left : 0,
                right: !isNaN(margin.right) ? margin.right : 0
            };
            var attr = {
                'id': diagramId + '_printImage',
                'src': image
            };
            var img_1 = this.createHtmlElement('img', attr);
            img_1.onload = function () {
                images = _this.getMultipleImage(img_1, options, true);
                _this.exportImage(images, fileName, fileType, image);
            };
        }
        else {
            images = [image];
            this.exportImage(images, fileName, fileType, image);
        }
    },
    exportImage: function (images, fileName, fileType, image) {
        var buffers = [];
        var length = (!(images instanceof HTMLElement)) ? images.length : 0;
        for (var g = 0; g < length; g++) {
            image = images[g];
            image = image.replace(/^data:[a-z]*;,/, '');
            var image1 = image.split(',');
            var byteString = atob(image1[1]);
            var buffer = new ArrayBuffer(byteString.length);
            var intArray = new Uint8Array(buffer);
            for (var i = 0; i < byteString.length; i++) {
                intArray[i] = byteString.charCodeAt(i);
            }
            buffers.push(buffer);
        }
        for (var j = 0; j < buffers.length; j++) {
            var b = new Blob([buffers[j]], { type: 'application/octet-stream' });
            if (sf.base.Browser.info.name === 'msie') {
                window.navigator.msSaveOrOpenBlob(b, fileName + '.' + fileType);
            }
            else {
                var urlLink = window.URL.createObjectURL(b);
                var anchorElement = document.createElement('a');
                anchorElement.download = fileName + '.' + fileType.toLocaleLowerCase();
                anchorElement.href = urlLink;
                anchorElement.click();
            }
        }
    },
    setScaleValueforCanvas: function (options, bounds, pageSettings) {
        var scaleX = 'scaleX';
        var scaleY = 'scaleY';
        var scaleOffsetX = 'scaleOffsetX';
        var scaleOffsetY = 'scaleOffsetY';
        options[scaleX] = 1;
        options[scaleY] = 1;
        options[scaleOffsetX] = 0;
        options[scaleOffsetY] = 0;
        options.pageHeight = options.pageHeight || pageSettings.height;
        options.pageWidth = options.pageWidth || pageSettings.width;
        var pageOrientation = options.orientation || pageSettings.orientation;
        if (!pageOrientation) {
            pageOrientation = 'Portrait';
        }
        if (pageOrientation === 'Portrait') {
            if (options.pageWidth > options.pageHeight) {
                var temp = options.pageHeight;
                options.pageHeight = options.pageWidth;
                options.pageWidth = temp;
            }
        }
        else {
            if (options.pageHeight > options.pageWidth) {
                var temp = options.pageWidth;
                options.pageWidth = options.pageHeight;
                options.pageHeight = temp;
            }
        }
        if (options.pageWidth && options.pageHeight && !options.fitToPage) {
            options.stretch = 'Meet';
        }
        var height = options.pageHeight || bounds.height;
        var width = options.pageWidth || bounds.width;
        if (options.stretch === 'Stretch' || options.stretch === 'Meet' || options.stretch === 'Slice') {
            options[scaleX] = width / bounds.width;
            options[scaleY] = height / bounds.height;
            if (options.stretch === 'Meet') {
                options[scaleX] = options[scaleY] = Math.min(options[scaleX], options[scaleY]);
                options[scaleOffsetY] = (options.pageHeight - bounds.height * options[scaleX]) / 2;
                options[scaleOffsetX] = (options.pageWidth - bounds.width * options[scaleX]) / 2;
            }
            else if (options.stretch === 'Slice') {
                options[scaleX] = options[scaleY] = Math.max(options[scaleX], options[scaleY]);
            }
            bounds.width = width;
            bounds.height = height;
        }
        bounds.x *= options[scaleX];
        bounds.y *= options[scaleY];
    },
    setTransform: function (element, bounds, margin) {
        element.setAttribute('transform', 'translate(' + (-bounds.x + margin.left) + ', ' +
            (-bounds.y + margin.top) + ')');
    },
    getPrintCanvasStyle: function (img, options) {
        var width = 0;
        var height = 0;
        var size = new Size();
        width = img.width;
        height = img.height;
        if (options.pageHeight || options.pageWidth) {
            height = options.pageHeight ? options.pageHeight : height;
            width = options.pageWidth ? options.pageWidth : width;
        }
        if (options.orientation) {
            if ((options.orientation === 'Landscape' && height > width) || options.orientation === 'Portrait' && width > height) {
                var temp = width;
                width = height;
                height = temp;
            }
        }
        size.height = height;
        size.width = width;
        return size;
    },
    getMultipleImage: function (img, options, isExport) {
        var imageArray = [];
        var div = this.createHtmlElement('div', {});
        var pageSize = this.getPrintCanvasStyle(img, options);
        //let pageWidth: number;
        //let pageHeight: number;
        var margin = options.margin;
        var mLeft = margin.left;
        var mTop = margin.top;
        var mRight = margin.right;
        var mBottom = margin.bottom;
        var x = 0;
        var y = 0;
        var pageWidth = pageSize.width + x;
        var pageHeight = pageSize.height + y;
        var drawnX = 0;
        var drawnY = 0;
        if (options && options.fitToPage) {
            div.style.height = 'auto';
            div.style.width = 'auto';
            var imgHeight = img.height;
            var imgWidth = img.width;
            //if (img) {
            var i = 0;
            var j = 0;
            var url = void 0;
            var clipWidth = 0;
            var clipHeight = 0;
            var ctx = void 0;
            var canvas = void 0;
            do {
                do {
                    clipWidth = pageSize.width;
                    clipHeight = pageSize.height;
                    if ((drawnX + pageSize.width) >= imgWidth) {
                        clipWidth = (imgWidth - drawnX);
                    }
                    if ((drawnY + pageSize.height) >= imgHeight) {
                        clipHeight = (imgHeight - drawnY);
                    }
                    canvas = this.createCanvas(diagramId + '_multiplePrint', pageSize.width, pageSize.height);
                    ctx = canvas.getContext('2d');
                    ctx.drawImage(img, x + drawnX + mLeft, y + drawnY + mTop, clipWidth - mRight - mLeft, clipHeight - mBottom - mTop, 0 + mLeft, 0 + mTop, clipWidth - mRight - mLeft, clipHeight - mBottom - mTop);
                    if ((drawnX + pageSize.width) >= imgWidth) {
                        drawnX -= (drawnX - imgWidth);
                    }
                    url = options.format === 'PNG' ? canvas.toDataURL('image/png') : options.format === 'JPEG' ? canvas.toDataURL('image/jpeg') : canvas.toDataURL();
                    ctx.restore();
                    drawnX += pageWidth;
                    if (isExport) {
                        imageArray.push(url);
                    }
                    else {
                        this.printImage(div, url, i + '' + j, pageWidth + 'px;', pageHeight + 'px;');
                    }
                    i++;
                } while (drawnX < imgWidth);
                j++;
                i = x = drawnX = 0;
                if ((drawnY + pageSize.height) >= imgHeight) {
                    drawnY -= (drawnY - imgHeight);
                }
                drawnY += pageHeight;
            } while (drawnY < imgHeight);
            //}
        }
        else {
            var x_1 = 0;
            var y_1 = 0;
            var pageSize_1 = this.getPrintCanvasStyle(img, options);
            var pageWidth_1 = pageSize_1.width;
            var pageHeight_1 = pageSize_1.height;
            var canvas = this.createCanvas(diagramId + '_diagram', pageWidth_1, pageHeight_1);
            var ctx = canvas.getContext('2d');
            ctx.drawImage(img, x_1 + mLeft, y_1 + mTop, img.width - (mRight + mLeft), img.height - (mTop + mBottom), 0 + mLeft, 0 + mTop, pageWidth_1 - (mRight + mLeft), pageHeight_1 - (mTop + mBottom));
            var url = options.format === 'PNG' ? canvas.toDataURL('image/png') : options.format === 'JPEG' ? canvas.toDataURL('image/jpeg') : canvas.toDataURL();
            ctx.restore();
            if (isExport) {
                imageArray.push(url);
            }
            else {
                this.printImage(div, url, 0);
            }
        }
        if (isExport) {
            return imageArray;
        }
        else {
            return div;
        }
    },
    printImage: function (div, url, i, pageWidth, pageHeight) {
        var attr = { 'class': 'e-diagram-print-page', 'style': 'width:' + pageWidth + 'height:' + pageHeight };
        var img = this.createHtmlElement('img', attr);
        var innerDiv = this.createHtmlElement('div', attr);
        attr = { 'id': diagramId + '_multiplePrint_img' + i, 'style': 'float:left', 'src': url };
        setAttributeHtml(img, attr);
        innerDiv.appendChild(img);
        div.appendChild(innerDiv);
    },
    /**
    * To print the image
    *
    * @private
    */
    print: function (options, diagramId, bounds, pageSettings) {
        return __awaiter(this, void 0, void 0, function () {
            var url;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        options.mode = 'Data';
                        pageSettings = pageSettings;
                        diagramId = diagramId;
                        return [4 /*yield*/, this.exportDiagram(options, diagramId, bounds, pageSettings, true, undefined, undefined, false)];
                    case 1:
                        url = _a.sent();
                        this.printImages(url, options);
                        return [2 /*return*/, url];
                }
            });
        });
    },
    printImages: function (url, options) {
        var _this = this;
        var attr = {
            'id': diagramId + '_printImage',
            'src': url
        };
        options.margin = { top: 0, bottom: 0, right: 0, left: 0 };
        var img = this.createHtmlElement('img', attr);
        img.onload = function () {
            var div = _this.getMultipleImage(img, options);
            // specify window parameters
            var printWind = window.open('');
            if (printWind != null) {
                if ((div instanceof HTMLElement)) {
                    printWind.document.write('<html><head><style> body{margin:0px;}  @media print { .e-diagram-print-page' +
                        '{page-break-after: left; }.e-diagram-print-page:last-child {page-break-after: avoid;}}' +
                        '</style><title></title></head>');
                    printWind.addEventListener('load', function (event) {
                        setTimeout(function () {
                            printWind.window.print();
                        }, 3000);
                    });
                    printWind.document.write('<center>' + div.innerHTML + '</center>');
                    printWind.document.close();
                }
            }
        };
    },
    /** @private */
    exportImages: function (image, options) {
        var _this = this;
        var region = options && options.region ? options.region : 'Content';
        var margin = options.margin || {};
        margin = {
            top: !isNaN(margin.top) ? margin.top : 0,
            bottom: !isNaN(margin.bottom) ? margin.bottom : 0,
            left: !isNaN(margin.left) ? margin.left : 0,
            right: !isNaN(margin.right) ? margin.right : 0
        };
        var bounds = this.getDiagramBounds(region, {});
        if (options.clipBounds) {
            bounds.x = (!isNaN(options.clipBounds.x) ? options.clipBounds.x : bounds.x);
            bounds.y = (!isNaN(options.clipBounds.y) ? options.clipBounds.y : bounds.y);
            bounds.width = (options.clipBounds.width || bounds.width);
            bounds.height = (options.clipBounds.height || bounds.height);
        }
        var img = document.createElement('img');
        var attr = {
            'src': image
        };
        setAttributeHtml(img, attr);
        var context = this;
        img.onload = function () {
            var canvas = _this.createCanvas(context.Diagram.id + 'innerImage', bounds.width + (margin.left + margin.right), bounds.height + (margin.top + margin.bottom));
            var ctx = canvas.getContext('2d');
            ctx.fillStyle = context.diagram.pageSettings.background.color;
            ctx.fillRect(0, 0, bounds.width + (margin.left + margin.right), bounds.height + (margin.top + margin.bottom));
            ctx.drawImage(img, 0, 0, bounds.width, bounds.height, margin.left, margin.top, bounds.width, bounds.height);
            image = canvas.toDataURL();
            if (options.printOptions) {
                context.printImages(image, options);
                return;
            }
            ctx.restore();
            var fileName = options.fileName || 'diagram';
            _this.canvasMultiplePage(options, canvas, margin, image, fileName);
        };
    }
};

return diagram;

}());
