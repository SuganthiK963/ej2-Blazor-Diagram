window.sfBlazor = window.sfBlazor || {};
window.sfBlazor.Maps = (function () {
'use strict';

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
/* eslint-disable no-case-declarations */
/* eslint-disable radix */
/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable camelcase */
/* eslint-disable @typescript-eslint/no-this-alias */
/* eslint-disable max-len */
/**
 * Maps Blazor introp module
 */
var rect = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
var MapLocation = /** @class */ (function () {
    function MapLocation(x, y) {
        this.x = x;
        this.y = y;
    }
    return MapLocation;
}());
/**
 *
 * @param {string} id Specifies the string
 * @returns {Element} Returns the element
 */
function createGroup(id) {
    var group = document.createElementNS('http://www.w3.org/2000/svg', 'g');
    group.setAttribute('id', id);
    return group;
}
/**
 *
 * @param {any} shape Specifies the shape
 * @param {Size} size Specifies the size
 * @param {MapLocation} location Specifies the location
 * @returns {string} Returns the string
 */
function calculateShape(shape, size, location) {
    var path = '';
    switch (shape) {
        case 'Balloon':
            path = 'M15,0C8.8,0,3.8,5,3.8,11.2C3.8,17.5,9.4,24.4,15,30c5.6-5.6,11.2-12.5,11.2-18.8C26.2,5,21.2,0,15,0z M15,16' +
                'c-2.8,0-5-2.2-5-5s2.2-5,5-5s5,2.2,5,5S17.8,16,15,16z';
            break;
        case 'Cross':
            path = 'M ' + location.x + ' ' + (location.y - size.height / 2) + ' L ' + location.x + ' ' + (location.y + size.height
                / 2) + ' M ' + (location.x - size.width / 2) + ' ' + location.y + ' L ' + (location.x + size.width / 2) + ' ' + location.y;
            break;
        case 'Diamond':
            path = 'M ' + location.x + ' ' + (location.y - size.height / 2) + ' L ' + (location.x + size.width / 2) + ' '
                + location.y + ' L ' + location.x + ' ' + (location.y + size.height / 2) + ' L ' + (location.x - size.width / 2)
                + ' ' + location.y + ' Z';
            break;
        case 'Star':
            path = 'M ' + (location.x + size.width / 3) + ' ' + (location.y - size.height / 2) + ' L ' + (location.x - size.width / 2)
                + ' ' + (location.y + size.height / 6) + ' L ' + (location.x + size.width / 2) + ' ' + (location.y + size.height / 6)
                + ' L ' + (location.x - size.width / 3) + ' ' + (location.y - size.height / 2) + ' L ' + location.x + ' ' +
                (location.y + size.height / 2) + ' L ' + (location.x + size.width / 3) + ' ' + (location.y - size.height / 2) + ' Z';
            break;
        case 'Triangle':
            path = 'M ' + location.x + ' ' + (location.y - size.height / 2) + ' L ' + (location.x + size.width / 2) + ' ' +
                (location.y + size.height / 2) + ' L ' + (location.x - size.width / 2) + ' ' + (location.y + size.height / 2) + ' Z';
            break;
        case 'HorizontalLine':
            path = ' M ' + (location.x - size.width / 2) + ' ' + location.y + ' L ' + (location.x + size.width / 2) + ' '
                + location.y;
            break;
        case 'VerticalLine':
            path = 'M ' + location.x + ' ' + (location.y - size.height / 2) + ' L ' + location.x + ' ' +
                (location.y + size.height / 2);
            break;
        case 'InvertedTriangle':
            path = 'M ' + (location.x - size.width / 2) + ' ' + (location.y - size.height / 2) +
                ' L ' + (location.x + size.width / 2) + ' ' +
                (location.y - size.height / 2) + ' L ' + (location.x) + ' ' + (location.y + size.height / 2) + ' Z';
            break;
        case 'Pentagon':
            var factor = 72;
            var xValue = void 0;
            var yValue = void 0;
            for (var i = 0; i < 5; i++) {
                xValue = (size.width / 2) * Math.cos((Math.PI / 180) * (i * factor));
                yValue = (size.height / 2) * Math.sin((Math.PI / 180) * (i * factor));
                path += (i === 0 ? 'M ' : 'L ') + (location.x + xValue) + ' ' + (location.y + yValue);
            }
            path += ' Z';
            break;
    }
    return path;
}
var SfMaps = /** @class */ (function () {
    function SfMaps(id, element, options, dotnetRef) {
        this.previousMarkerId = '';
        this.previousId = '';
        this.enableSelectionZoom = false;
        this.allowPanning = false;
        this.isPanning = false;
        this.isPan = true;
        this.zoomClick = false;
        this.mouseClick = false;
        this.position = null;
        this.height = 0;
        this.width = 0;
        this.mapsBorderWidth = 0;
        this.toolbarButtonOpacity = 1;
        this.toolbarShapeOpacity = 1;
        this.markerClusterExpandCheck = false;
        this.svgCreated = false;
        this.zoomIn = 1;
        this.zoomOut = 1;
        this.scaleFactor = 1;
        this.factorCount = 0;
        this.svg = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
        this.lastScale = 1;
        this.clusterLocation = [];
        this.isTouch = false;
        this.touchMoveList = [];
        this.touchStartList = [];
        this.newTiles = [];
        this.tileTranslatePoint = null;
        this.isToolbarTooltip = false;
        this.marginTop = 0;
        this.marginLeft = 0;
        this.isPinchZoomed = false;
        this.panComplete = false;
        this.pinchComplete = false;
        this.id = id;
        this.element = element;
        this.dotNetRef = dotnetRef;
        this.layerCount = options.layerCount;
        this.clusterSettings = options.markerCluster ? JSON.parse(options.markerCluster) : null;
        this.dataSource = options.dataSource ? JSON.parse(options.dataSource) : null;
        this.navigation = options.navigationSettings ? JSON.parse(options.navigationSettings) : null;
        this.shapeTranslatePoint = options.shapeTranslatePoint ? options.shapeTranslatePoint : null;
        this.shapeBorderWidth = options.shapeBorderWidth;
        this.projectionType = options.projectionType;
        this.options = options;
        this.options.layerHighlightSettings = JSON.parse(this.options.layerHighlightSettings);
        this.options.markerHighlightSettings = JSON.parse(this.options.markerHighlightSettings);
        this.options.bubbleHighlightSettings = JSON.parse(this.options.bubbleHighlightSettings);
        this.options.navigationHighlightSettings = JSON.parse(this.options.navigationHighlightSettings);
        this.options.selectionSettings = JSON.parse(this.options.selectionSettings);
        this.element.blazor__instance = this;
        this.marginLeft = 0;
        this.urlTemplate = options.urlTemplate ? options.urlTemplate : null;
        this.newTiles = [];
        this.isToolbarTooltip = false;
        this.isTouch = false;
        this.pinchFactor = 1;
        this.lastScale = 1;
        this.touchStartList = [];
        this.touchMoveList = [];
    }
    SfMaps.prototype.wireEvents = function () {
        sf.base.EventHandler.add(this.element, 'wheel', this.mapMouseWheel.bind(this), this);
        sf.base.EventHandler.add(this.element, 'touchmove mousemove', this.mouseMove.bind(this), this);
        sf.base.EventHandler.add(this.element, 'touchend mouseup mouseleave', this.mouseUp.bind(this), this);
        sf.base.EventHandler.add(this.element, 'touchstart mousedown', this.mouseDown.bind(this), this);
        sf.base.EventHandler.add(this.element, 'click', this.click.bind(this), this);
        sf.base.EventHandler.add(this.element, 'dblclick', this.doubleClick.bind(this), this);
        sf.base.EventHandler.add(this.element, 'contextmenu', this.rightClick.bind(this), this);
        window.addEventListener('resize', this.reSize.bind(this));
    };
    SfMaps.prototype.rightClick = function (event) {
        var id = event.target['id'];
        var clientValue = this.getMousePosition(event.pageX, event.pageY);
        this.dotNetRef.invokeMethodAsync('TriggerMouseClick', clientValue.x, clientValue.y, document.getElementById(id));
    };
    SfMaps.prototype.reSize = function () {
        var width;
        var height;
        if (this.element !== null) {
            width = this.element.getBoundingClientRect().width;
            height = this.element.getBoundingClientRect().height;
        }
        if (this.previousHeight !== height || this.previousWidth !== width) {
            this.previousHeight = height;
            this.previousWidth = width;
            this.dotNetRef.invokeMethodAsync('TriggerResize', width, height);
        }
    };
    SfMaps.prototype.doubleClick = function (event) {
        if (event.detail === 2 && this.options.enableZoom && this.options.doubleClickZoom) {
            var parentElement = document.getElementById(this.id).getBoundingClientRect();
            var factor = this.scaleFactor + 1;
            if (factor >= 1) {
                this.scaleFactor = factor;
                this.removeCluster();
                this.dotNetRef.invokeMethodAsync('MouseWheelZoom', event.pageX - parentElement['x'], event.pageY - parentElement['y'], event.which, 'doubleClick');
            }
        }
        var id = event.target['id'];
        var clientValue = this.getMousePosition(event.pageX, event.pageY);
        if (this.options.tooltipDisplayMode === 'DoubleClick') {
            var layerIndex = parseInt(id.split('_LayerIndex_')[1].split('_')[0], 10);
            if (id.indexOf('shapeIndex') > -1) {
                var shapeIndex = parseInt(id.split('_shapeIndex_')[1].split('_')[0], 10);
                this.dotNetRef.invokeMethodAsync('TriggerShapeTooltip', id, clientValue.x, clientValue.y, layerIndex, shapeIndex, 'Shape', 0);
            }
            else if (id.indexOf('_MarkerIndex_') > -1 && id.indexOf('_cluster_') === -1) {
                var markerIndex = parseInt(id.split('_MarkerIndex_')[1].split('_')[0], 10);
                var dataIndex = parseInt(id.split('_dataIndex_')[1].split('_')[0], 10);
                this.dotNetRef.invokeMethodAsync('TriggerShapeTooltip', id, clientValue.x, clientValue.y, layerIndex, dataIndex, 'Marker', markerIndex);
            }
            else if (id.indexOf('_BubbleIndex_') > -1) {
                var markerIndex = parseInt(id.split('_BubbleIndex_')[1].split('_')[0], 10);
                var dataIndex = parseInt(id.split('_dataIndex_')[1].split('_')[0], 10);
                this.dotNetRef.invokeMethodAsync('TriggerShapeTooltip', id, clientValue.x, clientValue.y, layerIndex, dataIndex, 'Bubble', markerIndex);
            }
            else if (id.indexOf('_shapeIndex_') === -1 && id.indexOf('_MarkerIndex_') === -1 && id.indexOf('_BubbleIndex_') === -1) {
                this.dotNetRef.invokeMethodAsync('TriggerShapeTooltip', '', clientValue.x, clientValue.y, 0, 0, '', 0);
            }
        }
        this.dotNetRef.invokeMethodAsync('TriggerDoubleClick', clientValue.x, clientValue.y, document.getElementById(id));
    };
    SfMaps.prototype.mapMouseWheel = function (event) {
        event.preventDefault();
        var parentElement = document.getElementById(this.element.id).getBoundingClientRect();
        var delta = !sf.base.isNullOrUndefined(event['wheelDelta']) ? event['wheelDelta'] : -event['deltaY'];
        var direction = (delta / 120) > 0 ? 'ZoomIn' : 'ZoomOut';
        var factor = direction === 'ZoomIn' ? this.scaleFactor + 1 : this.scaleFactor - 1;
        var position = this.getMousePosition(event.pageX, event.pageY);
        if (this.options.enableZoom && this.options.enableMouseWheelZoom && factor >= 1) {
            this.scaleFactor = factor;
            this.removeCluster();
            this.dotNetRef.invokeMethodAsync('MouseWheelZoom', position.x, position.y, delta, 'wheelZoom');
        }
    };
    SfMaps.prototype.mouseUp = function (event) {
        var id = event.target['id'];
        var layerX;
        var layerY;
        var translatePoint = this.isTileMap ? this.tileTranslatePoint : this.shapeTranslatePoint;
        var touchCheck = false;
        this.lastScale = 1;
        if (event.type === 'touchend') {
            event.preventDefault();
            layerX = 0;
            layerY = 0;
            touchCheck = true;
            this.isTouch = false;
            this.touchMoveList = [];
            this.touchStartList = [];
        }
        else {
            this.moveClientX = event.pageX;
            this.moveClientY = event.pageY;
            layerX = event['layerX'];
            layerY = event['layerY'];
        }
        var clientValue = this.getMousePosition(this.moveClientX, this.moveClientY);
        this.moveClientX = clientValue.x;
        this.moveClientY = clientValue.y;
        var parentId = id.split('_')[0];
        if (!(id.indexOf('_Zooming_') > -1) && this.options.enableZoom && this.options.zoomOnClick && !this.isPanning && event.type !== 'mouseleave') {
            var factor = this.scaleFactor + 1;
            if (factor >= 1) {
                this.scaleFactor = factor;
                this.removeCluster();
                var eventValues = this.getMousePosition(event.pageX, event.pageY);
                this.dotNetRef.invokeMethodAsync('MouseWheelZoom', eventValues.x, eventValues.y, event.which, 'click');
            }
        }
        if (this.allowPanning && this.isPanning && this.options['enablePanning']) {
            if (this.isTileMap) {
                for (var i = 0; i < this.newTiles.length; i++) {
                    var divElement = document.getElementById(this.element.id + 'tile' + this.newTiles[i]);
                    divElement.parentNode.removeChild(divElement);
                }
            }
            this.newTiles = [];
            this.dotNetRef.invokeMethodAsync('UpdateTranslatePoint', this.isTileMap ? this.translatePoint : this.shapeTranslatePoint, this.tileTranslatePoint, this.scaleFactor);
            if (this.panComplete) {
                this.dotNetRef.invokeMethodAsync('TriggerPanningComplete', this.previousPoint.x, this.previousPoint.y, translatePoint.x, translatePoint.y, this.scaleFactor);
            }
        }
        if (this.isPinchZoomed) {
            this.isPinchZoomed = false;
            this.dotNetRef.invokeMethodAsync('UpdateTranslatePoint', this.shapeTranslatePoint, this.tileTranslatePoint, this.scaleFactor);
            if (this.pinchComplete) {
                this.dotNetRef.invokeMethodAsync('TriggerOnZoomComplete', translatePoint.x, translatePoint.y);
            }
        }
        this.isPanning = false;
        this.allowPanning = false;
        if (this.options.enableZoom && this.options.enableSelectionZooming && (this.mouseClick && this.zoomClick)) {
            this.svgCreated = false;
            this.mouseClick = false;
            var rectElement = document.getElementById(this.element.id + 'drawRect');
            if (rectElement) {
                rectElement.remove();
                this.removeCluster();
                var zoomFactor = 0;
                if (this.height > 0 && this.width > 0) {
                    var size = this.element.getBoundingClientRect();
                    if (!this.isTileMap) {
                        zoomFactor = this.scaleFactor + Math.round(((size.width / this.width) + (size.height / this.height)) / 2);
                    }
                    else {
                        zoomFactor = this.scaleFactor + Math.round(this.scaleFactor + (((size.width / this.width) +
                            (size.height / this.height)) / 2));
                        zoomFactor -= 2;
                    }
                    this.scaleFactor = zoomFactor;
                }
                if (zoomFactor >= 1) {
                    this.dotNetRef.invokeMethodAsync('TriggerZoomSelection', this.position.x, this.position.y, this.height, this.width);
                }
            }
        }
        if (id.indexOf('_LayerIndex_') > -1) {
            var shapeIndex = void 0;
            var dataIndex = void 0;
            var bubbleIndex = void 0;
            var navigationIndex = void 0;
            var layerIndex = parseInt(id.split('_LayerIndex_')[1].split('_')[0], 10);
            if (id.indexOf('shapeIndex') > -1) {
                shapeIndex = parseInt(id.split('_shapeIndex_')[1].split('_')[0], 10);
                dataIndex = id.indexOf('_dataIndex_') !== -1 ? parseInt(id.split('_dataIndex_')[1].split('_')[0], 10) : null;
                if (this.options.selectionSettings && this.options.selectionSettings[layerIndex] &&
                    this.options.selectionSettings[layerIndex]['Enable']) {
                    this.removeAllHighlight();
                }
                this.dotNetRef.invokeMethodAsync('SelectMap', id, layerIndex, shapeIndex, 'Shape', dataIndex);
            }
            if (id.indexOf('_BubbleIndex_') > -1) {
                bubbleIndex = parseInt(id.split('_BubbleIndex_')[1].split('_')[0], 10);
                dataIndex = id.indexOf('_dataIndex_') !== -1 ? parseInt(id.split('_dataIndex_')[1].split('_')[0], 10) : null;
                this.removeAllHighlight();
                this.dotNetRef.invokeMethodAsync('SelectMap', id, layerIndex, dataIndex, 'Bubble', bubbleIndex);
            }
            if (id.indexOf('_NavigationIndex_') > -1) {
                navigationIndex = parseInt(id.split('_NavigationIndex_')[1].split('_')[0], 10);
                dataIndex = id.indexOf('_Line_') !== -1 ? parseInt(id.split('_Line_')[1].split('_')[0], 10) : null;
                this.removeAllHighlight();
                this.dotNetRef.invokeMethodAsync('SelectMap', id, layerIndex, dataIndex, 'NavigationLine', navigationIndex);
            }
            if (touchCheck && id.indexOf('MarkerIndex') > -1 && !(id.indexOf('cluster') > -1)) {
                var layerIndex_1 = parseInt(id.split('_LayerIndex_')[1].split('_')[0], 10);
                var markerIndex = parseInt(id.split('_MarkerIndex_')[1].split('_')[0], 10);
                dataIndex = parseInt(id.split('_dataIndex_')[1].split('_')[0], 10);
                this.dotNetRef.invokeMethodAsync('TriggerMouseLeave', layerIndex_1, markerIndex, dataIndex, clientValue.x, clientValue.y, touchCheck);
            }
        }
        if (!(id.indexOf('_Zooming_') > -1) && event.type !== 'mouseleave') {
            this.dotNetRef.invokeMethodAsync('TriggerClickEventArgs', false, layerX, layerY, document.getElementById(id), this.isTileMap);
        }
    };
    SfMaps.prototype.targetTouches = function (touches) {
        var targetTouches = [];
        for (var i = 0; i < touches.length; i++) {
            targetTouches.push({ pageX: touches[i].pageX, pageY: touches[i].pageY });
        }
        return targetTouches;
    };
    SfMaps.prototype.mouseMove = function (event) {
        var id = event.target['id'];
        this.moveClientX = event.pageX;
        this.moveClientY = event.pageY;
        var layerX;
        var layerY;
        var translatePoint = this.isTileMap ? this.tileTranslatePoint : this.shapeTranslatePoint;
        var x;
        var y;
        var touchCheck = false;
        if (event.type === 'touchmove') {
            this.moveClientX = x = event['touches'][0].clientX;
            this.moveClientY = y = event['touches'][0].clientY;
            if (event['touches'].length === 2) {
                this.touchMoveList = this.targetTouches(event['touches']);
            }
            event.preventDefault();
            layerX = 0;
            layerY = 0;
            touchCheck = true;
        }
        else {
            this.moveClientX = event.pageX;
            this.moveClientY = event.pageY;
            x = event.clientX;
            y = event.clientY;
            layerX = event['layerX'];
            layerY = event['layerY'];
            if (id.indexOf('_Zooming') > -1) {
                document.getElementById(this.element.id + '_Zooming_ToolBar').setAttribute('opacity', '1');
            }
            else {
                var toolbar_1 = document.getElementById(this.element.id + '_Zooming_ToolBar');
                if (!sf.base.isNullOrUndefined(toolbar_1)) {
                    toolbar_1.setAttribute('opacity', '0.3');
                }
            }
        }
        if (id.indexOf("_Zooming_ToolBar_") > -1) {
            var elementGroup = document.getElementById(this.element.id + '_Zooming_KitCollection');
            for (var i = 0; i < elementGroup.childElementCount; i++) {
                elementGroup.children[i].setAttribute('class', ' e-maps-toolbar');
            }
            if (elementGroup) {
                if (!this.options.enableSelectionZooming && id.indexOf('_Zoom_') > -1) {
                    document.getElementById(this.element.id + '_Zooming_ToolBar_Zoom_Group').setAttribute('class', '');
                }
                if (!this.options.enablePanning && id.indexOf('_Pan_') > -1) {
                    document.getElementById(this.element.id + '_Zooming_ToolBar_Pan_Group').setAttribute('class', '');
                }
                if (this.options.factor === 1) {
                    if (this.isTileMap) {
                        this.removeZoomOpacity((this.options.enableSelectionZooming ? this.toolbarShapeOpacity : 0.3), (this.options.enableSelectionZooming ? this.toolbarButtonOpacity : 0.3), this.toolbarShapeOpacity, this.toolbarButtonOpacity, 0.3, 0.3, (this.options.enablePanning ? this.toolbarShapeOpacity : 0.3), (this.options.enablePanning ? this.toolbarButtonOpacity : 0.3), 0.3, 0.3);
                        if (!this.enableSelectionZoom && (this.isPan && this.options.enablePanning)) {
                            this.removePanColor(this.toolbarSelection);
                        }
                    }
                    else {
                        if (this.options.enablePanning && id.indexOf('_Pan_') > -1) {
                            document.getElementById(this.element.id + '_Zooming_ToolBar_Pan_Group').setAttribute('class', '');
                        }
                        this.removeZoomOpacity((this.options.enableSelectionZooming ? this.toolbarShapeOpacity : 0.3), (this.options.enableSelectionZooming ? this.toolbarButtonOpacity : 0.3), this.toolbarShapeOpacity, this.toolbarButtonOpacity, 0.3, 0.3, 0.3, 0.3, 0.3, 0.3);
                    }
                    if (id.indexOf('_ZoomOut_') > -1) {
                        document.getElementById(this.element.id + '_Zooming_ToolBar_ZoomOut_Group').setAttribute('class', '');
                    }
                    else if (id.indexOf('_Pan_') > -1 && !this.isPan) {
                        document.getElementById(this.element.id + '_Zooming_ToolBar_Pan_Group').setAttribute('class', '');
                    }
                    else if (id.indexOf('_Reset_') > -1) {
                        document.getElementById(this.element.id + '_Zooming_ToolBar_Reset_Group').setAttribute('class', '');
                    }
                }
                else if (this.options.maxZoom === this.options.factor) {
                    this.removeZoomOpacity(0.3, 0.3, 0.3, 0.3, this.toolbarShapeOpacity, this.toolbarButtonOpacity, (this.options.enablePanning ? this.toolbarShapeOpacity : 0.3), (this.options.enablePanning ? this.toolbarButtonOpacity : 0.3), this.toolbarShapeOpacity, this.toolbarButtonOpacity);
                    if (id.indexOf('_ZoomIn_') > -1) {
                        document.getElementById(this.element.id + '_Zooming_ToolBar_ZoomIn_Group').setAttribute('class', '');
                    }
                    else if (id.indexOf('_Zoom_') > -1) {
                        document.getElementById(this.element.id + '_Zooming_ToolBar_Zoom_Group').setAttribute('class', '');
                    }
                }
                else if (this.options.factor > 1) {
                    this.removeZoomOpacity((this.options.enableSelectionZooming ? this.toolbarShapeOpacity : 0.3), (this.options.enableSelectionZooming ? this.toolbarButtonOpacity : 0.3), this.toolbarShapeOpacity, this.toolbarButtonOpacity, this.toolbarShapeOpacity, this.toolbarButtonOpacity, (this.options.enablePanning ? this.toolbarShapeOpacity : 0.3), (this.options.enablePanning ? this.toolbarButtonOpacity : 0.3), this.toolbarShapeOpacity, this.toolbarButtonOpacity);
                    if (this.enableSelectionZoom && this.options.enableSelectionZooming) {
                        this.removeZoomColor(this.toolbarSelection);
                    }
                    if (this.options.minZoom === this.options.factor) {
                        this.minZoomSelection(id);
                    }
                }
            }
            this.previousId = id;
        }
        if (this.previousId.indexOf("_Zooming_ToolBar_") > -1 && id.indexOf("_Zooming_ToolBar_") === -1) {
            this.removeZoomOpacity(this.toolbarShapeOpacity, this.toolbarButtonOpacity, this.toolbarShapeOpacity, this.toolbarButtonOpacity, this.toolbarShapeOpacity, this.toolbarButtonOpacity, this.toolbarShapeOpacity, this.toolbarButtonOpacity, this.toolbarShapeOpacity, this.toolbarButtonOpacity);
            this.previousId = '';
        }
        if (this.legendType === 'Interactive' && id.indexOf('_MapAreaBorder') > -1) {
            var legendElement = document.getElementById(this.element.id + '_Interactive_Legend');
            if (legendElement !== null) {
                this.dotNetRef.invokeMethodAsync('MapsAreaInteractive');
            }
        }
        if (this.legendType === 'Interactive' && id.indexOf('_LayerIndex') > -1) {
            var elementId = document.getElementById(event.target['id']);
            var fill = elementId.getAttribute('fill');
            this.dotNetRef.invokeMethodAsync('RenderInteractive', fill);
        }
        var clientValue = this.getMousePosition(this.moveClientX, this.moveClientY);
        this.moveClientX = clientValue.x;
        this.moveClientY = clientValue.y;
        var parentId = id.split('_')[0];
        this.highlightMap(event);
        if (this.options.enableZoom && this.options.enableSelectionZooming && (this.mouseClick && this.zoomClick) && this.options.maxZoom > this.options.factor
            && this.enableSelectionZoom) {
            event.preventDefault();
            if (!this.svgCreated) {
                this.startValue = this.svgPoint(this.svg, x, y);
                this.svgCreated = true;
            }
            this.position = this.svgPoint(this.svg, x, y);
            var svgElement = document.getElementById(this.element.id).getBoundingClientRect();
            this.width = Math.abs(this.position.x - this.startValue.x);
            this.height = Math.abs(this.position.y - this.startValue.y);
            if (this.position.x > this.startValue.x) {
                this.position.x = this.startValue.x;
            }
            if (this.position.y > this.startValue.y) {
                this.position.y = this.startValue.y;
            }
            this.position.x = this.position.x - svgElement['x'];
            this.position.y = this.position.y - svgElement['y'];
            rect.setAttributeNS(null, 'x', this.position.x.toString());
            rect.setAttributeNS(null, 'y', this.position.y.toString());
            rect.setAttributeNS(null, 'id', this.element.id + 'drawRect');
            rect.setAttributeNS(null, 'width', this.width.toString());
            rect.setAttributeNS(null, 'height', this.height.toString());
            rect.setAttributeNS(null, 'fill', '#d3d3d3');
            rect.setAttributeNS(null, 'stroke-dasharray', '3');
            rect.setAttributeNS(null, 'opacity', '0.5');
            rect.setAttributeNS(null, 'stroke', '#009900');
            if (document.getElementById(this.element.id + '_tile_parent') !== null) {
                document.getElementById(this.element.id + '_LayerCollections').parentNode.appendChild(rect);
            }
            else {
                document.getElementById(this.element.id + '_svg').appendChild(rect);
            }
        }
        if (this.allowPanning && this.options.enablePanning && (this.clientX !== this.moveClientX || this.clientY !== this.moveClientY)) {
            this.isPanning = true;
            if (!this.isTileMap) {
                var element = document.getElementById(this.element.id + '_LayerCollections');
                for (var i = 0; i < element.children.length; i++) {
                    for (var j = 0; j < element.children[i].childElementCount; j++) {
                        if (element.children[i].children[j].id.indexOf('Polygon_Group') > -1) {
                            this.scaleFactor = element.children[i].children[j]['transform']['baseVal'][0]['matrix']['a'];
                            break;
                        }
                    }
                }
                if (this.clientX !== this.moveClientX && this.clientY !== this.moveClientY) {
                    var xDifference = this.clientX - this.moveClientX;
                    var yDifference = this.clientY - this.moveClientY;
                    var x_1 = translatePoint.x - xDifference / this.scaleFactor;
                    var y_1 = translatePoint.y - yDifference / this.scaleFactor;
                    if (!this.panComplete && !this.pinchComplete) {
                        this.previousPoint = this.isTileMap ? this.tileTranslatePoint : this.shapeTranslatePoint;
                        this.dotNetRef.invokeMethodAsync('TriggerPanning', translatePoint.x, translatePoint.y, x_1, y_1, this.scaleFactor);
                    }
                    this.panning(xDifference, yDifference, layerX, layerY, this.scaleFactor);
                    this.panComplete = this.isTouch && this.touchMoveList.length === 2 && this.touchStartList.length === 2 ? false : true;
                    this.clientX = this.moveClientX;
                    this.clientY = this.moveClientY;
                }
            }
            if ((id.indexOf(parentId) > -1)) {
                var element = document.getElementById(event.currentTarget['id'] + '_animated_tiles');
                if (element !== null) {
                    this.isTileMap = true;
                    this.scaleFactor = parseInt(element['className'], 10);
                    var xDifference = this.clientX - this.moveClientX;
                    var yDifference = this.clientY - this.moveClientY;
                    var location_1 = this.getTileGeoLocation(layerX, layerY);
                    if (!this.panComplete && this.touchMoveList.length !== 2 && this.touchStartList.length !== 2) {
                        this.previousPoint = this.isTileMap ? this.tileTranslatePoint : this.shapeTranslatePoint;
                        this.dotNetRef.invokeMethodAsync('TriggerTilePanning', this.previousPoint.x, this.previousPoint.y, translatePoint.x, translatePoint.y, this.scaleFactor, location_1.latitude, location_1.longitude);
                    }
                    this.panning(xDifference, yDifference, layerX, layerY, this.scaleFactor);
                    this.panComplete = this.isTouch && this.touchMoveList.length === 2 && this.touchStartList.length === 2 ? false : true;
                    this.clientX = this.moveClientX;
                    this.clientY = this.moveClientY;
                }
            }
        }
        if (this.options.enableZoom && this.options.enablePinchZooming && this.touchMoveList.length === 2 && this.touchStartList.length === 2) {
            this.isPinchZoomed = true;
            if (!this.pinchComplete && !this.panComplete) {
                this.dotNetRef.invokeMethodAsync('TriggerOnZoom', translatePoint.x, translatePoint.y);
            }
            this.pinchZooming(event);
            this.pinchComplete = true;
        }
        if (!this.allowPanning && !(this.mouseClick && this.zoomClick)) {
            if (id.indexOf('shapeIndex') > -1 && this.options.tooltipDisplayMode === 'MouseMove') {
                var layerIndex = parseInt(id.split('_LayerIndex_')[1].split('_')[0], 10);
                var shapeIndex = parseInt(id.split('_shapeIndex_')[1].split('_')[0], 10);
                this.dotNetRef.invokeMethodAsync('TriggerShapeTooltip', id, clientValue.x, clientValue.y, layerIndex, shapeIndex, 'Shape', 0);
            }
        }
        if (id.indexOf('_shapeIndex_') === -1 && id.indexOf('_MarkerIndex_') === -1 && id.indexOf('_BubbleIndex_') === -1 &&
            this.options.tooltipDisplayMode === 'MouseMove' ||
            (this.isToolbarTooltip && id.indexOf('_Zooming_ToolBar_') === -1 && this.options.tooltipDisplayMode !== 'MouseMove')) {
            this.dotNetRef.invokeMethodAsync('TriggerShapeTooltip', '', clientValue.x, clientValue.y, 0, 0, '', 0);
        }
        this.isToolbarTooltip = false;
        if (id.indexOf('_Zooming_ToolBar_') > -1) {
            this.isToolbarTooltip = true;
            var text = id.split('_Zooming_ToolBar_')[1].split('_')[0];
            this.dotNetRef.invokeMethodAsync('TriggerShapeTooltip', id, clientValue.x, clientValue.y, 0, 0, text, 0);
        }
        if ((id.indexOf('_LayerIndex_') > -1 && id.indexOf('_MarkerIndex_') > -1) || (touchCheck && id.indexOf('MarkerIndex') > -1 && !(id.indexOf('cluster') > -1))) {
            var layerIndex = parseInt(id.split('_LayerIndex_')[1].split('_')[0], 10);
            var markerIndex = parseInt(id.split('_MarkerIndex_')[1].split('_')[0], 10);
            var dataIndex = parseInt(id.split('_dataIndex_')[1].split('_')[0], 10);
            this.dotNetRef.invokeMethodAsync('TriggerMouseMove', layerIndex, markerIndex, dataIndex, id, clientValue.x, clientValue.y, id.indexOf('cluster') > -1 ? 'cluster' : 'marker', touchCheck);
        }
        if (id.indexOf('_LayerIndex_') > -1 && id.indexOf('_BubbleIndex_') > -1) {
            var layerIndex = parseInt(id.split('_LayerIndex_')[1].split('_')[0], 10);
            var markerIndex = parseInt(id.split('_BubbleIndex_')[1].split('_')[0], 10);
            var dataIndex = parseInt(id.split('_dataIndex_')[1].split('_')[0], 10);
            this.dotNetRef.invokeMethodAsync('TriggerBubbleMouseMove', layerIndex, markerIndex, dataIndex, id, clientValue.x, clientValue.y);
        }
        if (this.previousMarkerId.indexOf('_MarkerIndex_') > -1 && this.previousMarkerId != id) {
            var layerIndex = parseInt(this.previousMarkerId.split('_LayerIndex_')[1].split('_')[0], 10);
            var markerIndex = parseInt(this.previousMarkerId.split('_MarkerIndex_')[1].split('_')[0], 10);
            var dataIndex = parseInt(this.previousMarkerId.split('_dataIndex_')[1].split('_')[0], 10);
            this.dotNetRef.invokeMethodAsync('TriggerMouseLeave', layerIndex, markerIndex, dataIndex, clientValue.x, clientValue.y, touchCheck);
        }
        this.previousMarkerId = (id.indexOf('MarkerIndex') > -1 && !(id.indexOf('cluster') > -1)) ? id : '';
    };
    SfMaps.prototype.mouseDown = function (event) {
        var id = event.target['id'];
        var finalFactor = this.options.factor + (id.indexOf('_ZoomIn_') > 1 && document.getElementById(this.element.id + '_Zooming_ToolBar_ZoomIn_Rect').getAttribute('stroke-opacity') !== '0.3' ? 1 : 0) + (id.indexOf('_ZoomOut_') > 1 && (document.getElementById(this.element.id + '_Zooming_ToolBar_ZoomOut_Rect').getAttribute('stroke-opacity')) !== '0.3' ? -1 : 0);
        finalFactor = id.indexOf('_Reset_') > 1 ? this.options.minZoom : finalFactor;
        this.isPinchZoomed = false;
        this.isTouch = false;
        this.panComplete = false;
        this.pinchComplete = false;
        if (event.type === 'touchstart') {
            event.preventDefault();
            this.clientX = event['touches'][0].clientX;
            this.clientY = event['touches'][0].clientY;
            if (event['touches'].length === 2) {
                this.touchStartList = this.targetTouches(event['touches']);
            }
            this.isTouch = true;
        }
        else {
            this.clientX = event.pageX;
            this.clientY = event.pageY;
        }
        var clientValue = this.getMousePosition(this.clientX, this.clientY);
        this.clientX = clientValue.x;
        this.clientY = clientValue.y;
        if (this.options.enableZoom && this.options.enableSelectionZooming && this.zoomClick) {
            this.mouseClick = true;
            event.preventDefault();
        }
        if (!this.mouseClick && id.indexOf('ToolBar_Pan') === -1) {
            if (id.indexOf('_MapAreaBorder') > -1 && this.options.enablePanning) {
                this.allowPanning = true;
            }
            if (id.indexOf('') > -1 && !(id.indexOf('Zooming') > -1) && this.options.enablePanning) {
                this.allowPanning = true;
            }
        }
        if (this.isTouch && id.indexOf('MarkerIndex') > -1 && !(id.indexOf('cluster') > -1)) {
            var layerIndex = parseInt(id.split('_LayerIndex_')[1].split('_')[0], 10);
            var markerIndex = parseInt(id.split('_MarkerIndex_')[1].split('_')[0], 10);
            var dataIndex = parseInt(id.split('_dataIndex_')[1].split('_')[0], 10);
            this.dotNetRef.invokeMethodAsync('TriggerMarkerClick', layerIndex, markerIndex, dataIndex, id, clientValue.x, clientValue.y, null, 'marker', this.isTouch);
        }
        if (document.getElementById(this.element.id + '_Zooming_KitCollection')) {
            if (finalFactor === 1) {
                if (document.getElementById(this.element.id + '_Zooming_ToolBar_Reset_Rect')) {
                    document.getElementById(this.element.id + '_Zooming_ToolBar_Reset_Group').setAttribute('class', '');
                }
                if (document.getElementById(this.element.id + '_Zooming_ToolBar_ZoomOut_Rect')) {
                    document.getElementById(this.element.id + '_Zooming_ToolBar_ZoomOut_Group').setAttribute('class', '');
                }
                this.removePanColor(this.toolbarColor);
                if (this.isTileMap && (!this.enableSelectionZoom || this.previousId.indexOf("_Reset_") > -1)) {
                    if (document.getElementById(this.element.id + '_Zooming_ToolBar_Pan_Rect') && document.getElementById(this.element.id + '_Zooming_ToolBar_Pan_Rect').getAttribute('stroke-opacity') !== '0.3') {
                        this.removePanColor(this.toolbarSelection);
                    }
                    if (this.allowPanning) {
                        this.removeZoomOpacity((this.options.enableSelectionZooming ? this.toolbarShapeOpacity : 0.3), (this.options.enableSelectionZooming ? this.toolbarButtonOpacity : 0.3), this.toolbarShapeOpacity, this.toolbarButtonOpacity, this.toolbarShapeOpacity, this.toolbarButtonOpacity, (this.options.enablePanning ? this.toolbarShapeOpacity : 0.3), (this.options.enablePanning ? this.toolbarButtonOpacity : 0.3), this.toolbarShapeOpacity, this.toolbarButtonOpacity);
                    }
                    else {
                        this.removeZoomOpacity((this.options.enableSelectionZooming ? this.toolbarShapeOpacity : 0.3), (this.options.enableSelectionZooming ? this.toolbarButtonOpacity : 0.3), this.toolbarShapeOpacity, this.toolbarButtonOpacity, 0.3, 0.3, (this.options.enablePanning ? this.toolbarShapeOpacity : 0.3), (this.options.enablePanning ? this.toolbarButtonOpacity : 0.3), 0.3, 0.3);
                    }
                    if (document.getElementById(this.element.id + '_Zooming_ToolBar_Pan_Rect') && !this.enableSelectionZoom && (this.isPan && this.options.enablePanning)) {
                        this.removePanColor(this.toolbarSelection);
                    }
                }
                else {
                    this.removeZoomOpacity((this.options.enableSelectionZooming ? this.toolbarShapeOpacity : 0.3), (this.options.enableSelectionZooming ? this.toolbarButtonOpacity : 0.3), this.toolbarShapeOpacity, this.toolbarButtonOpacity, 0.3, 0.3, 0.3, 0.3, 0.3, 0.3);
                }
            }
            else if ((this.options.maxZoom === finalFactor || this.options.maxZoom < finalFactor) && !this.allowPanning) {
                if (document.getElementById(this.element.id + '_Zooming_ToolBar_ZoomIn_Rect')) {
                    document.getElementById(this.element.id + '_Zooming_ToolBar_ZoomIn_Group').setAttribute('class', '');
                }
                if (document.getElementById(this.element.id + '_Zooming_ToolBar_Zoom_Rect')) {
                    document.getElementById(this.element.id + '_Zooming_ToolBar_Zoom_Group').setAttribute('class', '');
                }
                if (this.previousId != '') {
                    this.removeZoomOpacity(0.3, 0.3, 0.3, 0.3, this.toolbarShapeOpacity, this.toolbarButtonOpacity, (this.options.enablePanning ? this.toolbarShapeOpacity : 0.3), (this.options.enablePanning ? this.toolbarButtonOpacity : 0.3), this.toolbarShapeOpacity, this.toolbarButtonOpacity);
                }
                if (this.options.maxZoom === finalFactor && this.enableSelectionZoom) {
                    if (document.getElementById(this.element.id + '_Zooming_ToolBar_Zoom_Rect')) {
                        document.getElementById(this.element.id + '_Zooming_ToolBar_Zoom_Path').setAttribute('fill', this.toolbarColor);
                        document.getElementById(this.element.id + '_Zooming_ToolBar_Zoom_Path').setAttribute('stroke', this.toolbarColor);
                        this.isPan = false;
                    }
                }
                if (this.options.maxZoom >= finalFactor && finalFactor != 1 && !this.enableSelectionZoom && (this.isPan && this.options.enablePanning)) {
                    this.removePanColor(this.toolbarSelection);
                }
                if (this.options.minZoom === finalFactor) {
                    this.minZoomSelection(id);
                }
            }
            else if (finalFactor > 1) {
                this.removeZoomOpacity((this.options.enableSelectionZooming ? this.toolbarShapeOpacity : 0.3), (this.options.enableSelectionZooming ? this.toolbarButtonOpacity : 0.3), this.toolbarShapeOpacity, this.toolbarButtonOpacity, this.toolbarShapeOpacity, this.toolbarButtonOpacity, (this.options.enablePanning ? this.toolbarShapeOpacity : 0.3), (this.options.enablePanning ? this.toolbarButtonOpacity : 0.3), this.toolbarShapeOpacity, this.toolbarButtonOpacity);
                if (this.options.maxZoom >= finalFactor && finalFactor != 1) {
                    if (!this.enableSelectionZoom && (this.isPan && this.options.enablePanning)) {
                        this.removePanColor(this.toolbarSelection);
                    }
                    if (this.enableSelectionZoom && this.isTileMap) {
                        this.removePanColor(this.toolbarColor);
                    }
                }
                if (this.enableSelectionZoom && this.options.enableSelectionZooming) {
                    this.removeZoomColor(this.toolbarSelection);
                }
                if (this.options.minZoom === finalFactor) {
                    this.minZoomSelection(id);
                }
            }
        }
    };
    SfMaps.prototype.pinchZooming = function (event) {
        this.pinchFactor = this.scaleFactor;
        var prevLevel = this.scaleFactor;
        var availSize = this.areaRect;
        this.previousScale = this.scaleFactor;
        this.previousPoint = this.shapeTranslatePoint;
        var prevTilePoint = this.tileTranslatePoint;
        var scale = this.calculateScale(this.touchStartList, this.touchMoveList);
        var touchCenter = this.getTouchCenter(this.touchMoveList);
        var newScale = scale / this.lastScale;
        this.lastScale = scale;
        this.pinchFactor *= newScale;
        this.pinchFactor = Math.min(this.options.maxZoom, Math.max(this.options.minZoom, this.pinchFactor));
        if (!this.isTileMap) {
            var mapTotalWidth = Math.abs(this.baseMapBounds.minBounds.x - this.baseMapBounds.maxBounds.x);
            var mapTotalHeight = Math.abs(this.baseMapBounds.minBounds.y - this.baseMapBounds.maxBounds.y);
            var currentHeight = Math.abs(this.baseMapBounds.maxBounds.y - this.baseMapBounds.minBounds.y) * this.pinchFactor;
            var translatePointX = this.shapeTranslatePoint.x - (((availSize.width / this.scaleFactor) - (availSize.width / this.pinchFactor)) / (availSize.width / touchCenter.x));
            var translatePointY = this.shapeTranslatePoint.y - (((availSize.height / this.scaleFactor) - (availSize.height / this.pinchFactor)) / (availSize.height / touchCenter.y));
            translatePointX = (currentHeight < this.areaRect.height) ? (availSize.x + ((-(this.baseMapBounds.minBounds.x)) + ((availSize.width / 2) - (mapTotalWidth / 2)))) : translatePointX;
            translatePointY = (currentHeight < this.areaRect.height) ? (availSize.y + ((-(this.baseMapBounds.minBounds.y)) + ((availSize.height / 2) - (mapTotalHeight / 2)))) : translatePointY;
            this.scaleFactor = this.pinchFactor;
            this.shapeTranslatePoint = { x: translatePointX, y: translatePointY };
            this.applyTransform();
        }
        else {
            this.getTileTranslatePosition(prevLevel, this.pinchFactor, { x: touchCenter.x, y: touchCenter.y }, null);
            this.tileZoomLevel = this.pinchFactor;
            this.scaleFactor = this.pinchFactor;
            this.translatePoint.x = (this.tileTranslatePoint.x - (0.01 * this.scaleFactor)) / Math.pow(2, this.scaleFactor - 1);
            this.translatePoint.y = (this.tileTranslatePoint.y - (0.01 * this.scaleFactor)) / Math.pow(2, this.scaleFactor - 1);
            this.generateTiles();
            this.renderMarkers();
        }
    };
    SfMaps.prototype.minZoomSelection = function (id) {
        this.removeZoomOpacity((this.options.enableSelectionZooming ? this.toolbarShapeOpacity : 0.3), (this.options.enableSelectionZooming ? this.toolbarButtonOpacity : 0.3), this.toolbarShapeOpacity, this.toolbarButtonOpacity, 0.3, 0.3, (this.options.enablePanning ? this.toolbarShapeOpacity : 0.3), (this.options.enablePanning ? this.toolbarButtonOpacity : 0.3), 0.3, 0.3);
        if (id.indexOf('_ZoomOut_') > -1) {
            document.getElementById(this.element.id + '_Zooming_ToolBar_ZoomOut_Group').setAttribute('class', '');
        }
        if (id.indexOf('_Reset_') > -1) {
            document.getElementById(this.element.id + '_Zooming_ToolBar_Reset_Group').setAttribute('class', '');
        }
    };
    SfMaps.prototype.removeZoomOpacity = function (zoomOpacity, zoomStrokeOpacity, zoomInOpacity, zoomInStrokeOpacity, zoomOutOpacity, zoomOutStrokeOpacity, panOpacity, panStrokeOpacity, resetOpacity, resetStrokeOpacity) {
        if (document.getElementById(this.element.id + '_Zooming_ToolBar_Zoom_Rect')) {
            document.getElementById(this.element.id + '_Zooming_ToolBar_Zoom_Rect').setAttribute('stroke-opacity', zoomStrokeOpacity.toString());
            document.getElementById(this.element.id + '_Zooming_ToolBar_Zoom_Path').setAttribute('opacity', zoomOpacity.toString());
        }
        if (document.getElementById(this.element.id + '_Zooming_ToolBar_ZoomIn_Rect')) {
            document.getElementById(this.element.id + '_Zooming_ToolBar_ZoomIn_Rect').setAttribute('stroke-opacity', zoomInStrokeOpacity.toString());
            document.getElementById(this.element.id + '_Zooming_ToolBar_ZoomIn_Path').setAttribute('opacity', zoomInOpacity.toString());
        }
        if (document.getElementById(this.element.id + '_Zooming_ToolBar_ZoomOut_Rect')) {
            document.getElementById(this.element.id + '_Zooming_ToolBar_ZoomOut_Rect').setAttribute('stroke-opacity', zoomOutStrokeOpacity.toString());
            document.getElementById(this.element.id + '_Zooming_ToolBar_ZoomOut_Path').setAttribute('opacity', zoomOutOpacity.toString());
        }
        if (document.getElementById(this.element.id + '_Zooming_ToolBar_Pan_Rect')) {
            document.getElementById(this.element.id + '_Zooming_ToolBar_Pan_Rect').setAttribute('stroke-opacity', panStrokeOpacity.toString());
            document.getElementById(this.element.id + '_Zooming_ToolBar_Pan_Path').setAttribute('opacity', panOpacity.toString());
        }
        if (document.getElementById(this.element.id + '_Zooming_ToolBar_Reset_Rect')) {
            document.getElementById(this.element.id + '_Zooming_ToolBar_Reset_Rect').setAttribute('stroke-opacity', resetStrokeOpacity.toString());
            document.getElementById(this.element.id + '_Zooming_ToolBar_Reset_Path').setAttribute('opacity', resetOpacity.toString());
        }
    };
    SfMaps.prototype.removePanColor = function (selectionColor) {
        if (document.getElementById(this.element.id + '_Zooming_ToolBar_Pan_Rect')) {
            document.getElementById(this.element.id + '_Zooming_ToolBar_Pan_Path').setAttribute('fill', selectionColor);
            document.getElementById(this.element.id + '_Zooming_ToolBar_Pan_Path').setAttribute('stroke', selectionColor);
        }
    };
    SfMaps.prototype.removeZoomColor = function (selectionColor) {
        if (document.getElementById(this.element.id + '_Zooming_ToolBar_Zoom_Rect')) {
            document.getElementById(this.element.id + '_Zooming_ToolBar_Zoom_Path').setAttribute('fill', selectionColor);
            document.getElementById(this.element.id + '_Zooming_ToolBar_Zoom_Path').setAttribute('stroke', selectionColor);
        }
    };
    SfMaps.prototype.getTileTranslatePosition = function (prevLevel, currentLevel, position, type) {
        var tileDefaultSize = 256;
        var padding = type === 'ZoomOut' ? 10 : (type === 'Reset' && currentLevel > 1) ? 0 : 10;
        var prevSize = Math.pow(2, prevLevel) * 256;
        var totalSize = Math.pow(2, currentLevel) * 256;
        var x = ((position.x - this.tileTranslatePoint.x) / prevSize) * 100;
        var y = ((position.y - this.tileTranslatePoint.y) / prevSize) * 100;
        var bounds = this.baseMapBounds.availableSize;
        this.tileTranslatePoint.x = (currentLevel === 1) ? (bounds.width / 2) - ((tileDefaultSize * 2) / 2) :
            position.x - ((x * totalSize) / 100);
        this.tileTranslatePoint.y = (currentLevel === 1) ? ((bounds.height / 2) - ((tileDefaultSize * 2) / 2) + (padding * 2)) :
            position.y - ((y * totalSize) / 100);
    };
    SfMaps.prototype.calculateScale = function (startTouches, endTouches) {
        var startDistance = this.getDistance(startTouches[0], startTouches[1]);
        var endDistance = this.getDistance(endTouches[0], endTouches[1]);
        return (endDistance / startDistance);
    };
    SfMaps.prototype.getDistance = function (a, b) {
        var x = a.pageX - b.pageX;
        var y = a.pageY - b.pageY;
        return Math.sqrt(x * x + y * y);
    };
    SfMaps.prototype.sum = function (a, b) {
        return a + b;
    };
    SfMaps.prototype.getTouchCenter = function (touches) {
        return {
            x: touches.map(function (e) { return e['pageX']; }).reduce(this.sum) / touches.length,
            y: touches.map(function (e) { return e['pageY']; }).reduce(this.sum) / touches.length
        };
    };
    SfMaps.prototype.highlightMap = function (event) {
        var targetElement = event.target;
        var id = targetElement['id'];
        this.removeAllHighlight();
        this.targetElementPath = targetElement;
        if (id.indexOf('_LayerIndex_') > -1) {
            var layerIndex = parseInt(id.split('_LayerIndex_')[1].split('_')[0], 10);
            var dataIndex = id.indexOf('_dataIndex_') > -1 ? parseInt(id.split('_dataIndex_')[1].split('_')[0], 10) : 0;
            if (id.indexOf('shapeIndex') > -1 && this.options.layerHighlightSettings[layerIndex] &&
                this.options.layerHighlightSettings[layerIndex].Enable) {
                if (!targetElement.classList.contains('mapShapeSelection')) {
                    var shapeIndex = parseInt(id.split('_shapeIndex_')[1].split('_')[0], 10);
                    this.dotNetRef.invokeMethodAsync('TriggerShapeHighlight', layerIndex, shapeIndex, id);
                }
            }
            else if (id.indexOf('_BubbleIndex_') > -1) {
                var bubbleIndex = parseInt(id.split('_BubbleIndex_')[1].split('_')[0], 10);
                if (this.options.bubbleHighlightSettings[layerIndex][bubbleIndex] &&
                    this.options.bubbleHighlightSettings[layerIndex][bubbleIndex].Enable
                    && !targetElement.classList.contains('mapBubbleSelection')) {
                    this.dotNetRef.invokeMethodAsync('TriggerItemHighlight', 'Bubble', layerIndex, bubbleIndex, dataIndex, id);
                }
            }
            else if (id.indexOf('_NavigationIndex_') > -1) {
                var navigationIndex = parseInt(id.split('_NavigationIndex_')[1].split('_')[0], 10);
                if (this.options.navigationHighlightSettings[layerIndex][navigationIndex] &&
                    this.options.navigationHighlightSettings[layerIndex][navigationIndex].Enable
                    && !targetElement.classList.contains('mapNavigationSelection')) {
                    this.dotNetRef.invokeMethodAsync('TriggerItemHighlight', 'Navigation', layerIndex, navigationIndex, 0, id);
                }
            }
            else if (id.indexOf('_MarkerIndex_') > -1 && id.indexOf('_cluster_') === -1) {
                var markerIndex = parseInt(id.split('_MarkerIndex_')[1].split('_')[0], 10);
                if (this.options.markerHighlightSettings[layerIndex][markerIndex] &&
                    this.options.markerHighlightSettings[layerIndex][markerIndex].Enable
                    && !targetElement.classList.contains('mapMarkerSelection')) {
                    this.dotNetRef.invokeMethodAsync('TriggerItemHighlight', 'Marker', layerIndex, markerIndex, dataIndex, id);
                }
            }
        }
        else if (id.indexOf('_Legend_Shape_') > -1 && this.options.layerHighlightSettings[0] &&
            this.options.layerHighlightSettings[0].Enable) {
            if (!targetElement.classList.contains('mapShapeSelection')) {
                targetElement.classList.add('highlightShape');
                var elements = document.querySelectorAll('path[fill*="' + targetElement.getAttribute("fill") + '"]');
                if (elements && elements.length > 0) {
                    for (var k = 0; k < elements.length; k++) {
                        this.handleHighlight(elements[k], 'Shape', this.options.layerHighlightSettings[0]);
                    }
                }
            }
        }
    };
    SfMaps.prototype.removeAllHighlight = function () {
        this.removeHighlight('Shape');
        this.removeHighlight('Marker');
        this.removeHighlight('Bubble');
        this.removeHighlight('Navigation');
    };
    SfMaps.prototype.handleHighlight = function (element, type, settings) {
        if (type === 'Shape') {
            var elements = document.querySelectorAll('rect[id*="' + this.element.id + '_Legend_Shape_Index"]');
            if (elements && elements.length > 0) {
                for (var k = 0; k < elements.length; k++) {
                    if (elements[k].getAttribute('fill') === element.getAttribute('fill')) {
                        elements[k].classList.add('highlight' + type);
                    }
                }
            }
        }
        if (type !== 'Arrow') {
            element.classList.add('highlight' + type);
            if (document.querySelector('#highlight' + type + 'Style') !== null &&
                document.querySelector('#highlight' + type + 'Style')['length'] !== 0) {
                this.customizeStyle('highlight' + type + 'Style', 'highlight' + type, settings, element['id']);
            }
            else {
                this.createStyle('highlight' + type + 'Style', 'highlight' + type, settings, element['id']);
            }
        }
        else {
            element.setAttribute('fill', settings.Fill);
            element.setAttribute('fill-opacity', settings.opacity);
            element.setAttribute('stroke-opacity', settings['BorderOpacity']);
        }
    };
    SfMaps.prototype.removeHighlight = function (type) {
        var highlights = document.querySelectorAll('.highlight' + type);
        if (highlights && highlights.length > 0) {
            for (var i = 0; i < highlights.length; i++) {
                highlights[i].classList.remove('highlight' + type);
                if (type === 'Navigation' && (highlights[i].getAttribute('marker-start') !== '' || highlights[i].getAttribute('marker-end') !== '')) {
                    var highlightNavigate = document.getElementById(highlights[i].id + '_triangle');
                    var layerIndex = parseInt(highlights[i].id.split('_LayerIndex_')[1].split('_')[0], 10);
                    var lineIndex = parseInt(highlights[i].id.split('_NavigationIndex_')[1].split('_')[0], 10);
                    highlightNavigate.setAttribute('fill', this.navigation[layerIndex][lineIndex].ArrowColor);
                    highlightNavigate.setAttribute('opacity', '1');
                }
            }
        }
    };
    SfMaps.prototype.createStyle = function (id, className, setting, elementId) {
        var style = document.createElement('style');
        style.id = id;
        this.highlightStyle(style, className, setting, elementId);
        document.body.appendChild(style);
    };
    SfMaps.prototype.highlightStyle = function (style, className, setting, elementId) {
        if (elementId.indexOf('NavigationIndex') > -1) {
            var layerIndex = parseInt(elementId.split('_LayerIndex_')[1].split('_')[0], 10);
            var lineIndex = parseInt(elementId.split('_NavigationIndex_')[1].split('_')[0], 10);
            if (this.navigation[layerIndex][lineIndex].Angle === 0) {
                style.innerHTML = '.' + className + ' {fill:' + setting['Fill'] + ';fill-opacity:' +
                    setting['Opacity'] + ';stroke:' + setting['BorderColor']
                    + ';stroke-width:' + setting['BorderWidth'] + ';stroke-opacity:' + setting['BorderOpacity'];
            }
            else {
                style.innerHTML = '.' + className + ' {fill: none;fill-opacity:' +
                    setting['Opacity'] + ';stroke:' + setting['Fill']
                    + ';stroke-width:' + setting['BorderWidth'] + ';stroke-opacity:' + setting['BorderOpacity'];
            }
        }
        else {
            style.innerHTML = '.' + className + ' {fill:' + setting['Fill'] + ';fill-opacity:' +
                setting['Opacity'] + ';stroke:' + setting['BorderColor']
                + ';stroke-width:' + setting['BorderWidth'] + ';stroke-opacity:' + setting['BorderOpacity'];
        }
    };
    SfMaps.prototype.customizeStyle = function (id, className, setting, elementId) {
        var style = document.getElementById(id);
        this.highlightStyle(style, className, setting, elementId);
    };
    SfMaps.prototype.renderMarkers = function () {
        var layerCollection = document.getElementById(this.element.id + '_LayerCollections');
        for (var k = 0; k < this.layerCount; k++) {
            var templateCollection = document.getElementById(this.element.id + '_LayerIndex_' + k + '_Markers_Template_Group');
            this.markerTemplateTranslate(templateCollection, 0, this.translatePoint.x, this.translatePoint.y);
            var labelCollection = document.getElementById(this.element.id + '_LayerIndex_' + k + '_DataLabel_Group');
            this.labelTemplateTranslate(labelCollection, this.translatePoint.x, this.translatePoint.y);
        }
        for (var i = 0; i < layerCollection.childElementCount; i++) {
            var layerElement = layerCollection.childNodes[i];
            if (layerElement.tagName === 'g') {
                var index = layerElement.id.indexOf('_LayerIndex_') > -1 &&
                    parseFloat(layerElement.id.split('_LayerIndex_')[1].split('_')[0]);
                for (var j = 0; j < layerElement.childNodes.length; j++) {
                    var currentEle = layerElement.childNodes[j];
                    if (currentEle.COMMENT_NODE === currentEle.nodeType) {
                        continue;
                    }
                    if (currentEle.id.indexOf('Polygon') > -1) {
                        currentEle.setAttribute('transform', 'scale(' + Math.pow(2, this.scaleFactor - 1) + ') translate( ' +
                            this.translatePoint.x + ' ' + this.translatePoint.y + ' )');
                    }
                    else if (currentEle.id.indexOf('_MarkerGroup') > -1) {
                        for (var k = 0; k < currentEle.childElementCount; k++) {
                            this.markerTranslate(currentEle.childNodes[k], 0, this.translatePoint.x, this.translatePoint.y, this.scaleFactor, false);
                        }
                    }
                    else if (currentEle.id.indexOf('_line_Group') > -1) {
                        if (!this.isTileMap) {
                            currentEle.setAttribute('transform', 'scale(' + (this.scaleFactor) + ') translate( ' +
                                this.translatePoint.x + ' ' + this.translatePoint.y + ' )');
                        }
                        else {
                            for (var m = 0; m < currentEle.childElementCount; m++) {
                                var currentGroup = document.getElementById(currentEle.children[m]['id']);
                                for (var k = 0; k < currentGroup.childElementCount; k++) {
                                    if (currentGroup.childNodes[k]['id'] !== '') {
                                        var layerIndex = parseInt(currentGroup.childNodes[k].id.split('_LayerIndex_')[1].split('_')[0], 10);
                                        var lineIndex = parseInt(currentGroup.childNodes[k].id.split('_NavigationIndex_')[1].split('_')[0], 10);
                                        this.navigationTranslate(currentGroup.children[k], 0, layerIndex, lineIndex);
                                    }
                                }
                            }
                        }
                    }
                    else if (currentEle.id.indexOf('_dataLableIndex_Group') > -1) {
                        var factor = Math.pow(2, this.scaleFactor - 1);
                        for (var k = 0; k < currentEle.childNodes.length; k++) {
                            this.dataLabelTranslate(currentEle.childNodes[k], 0, this.translatePoint.x, this.translatePoint.y, factor, 'DataLabel');
                        }
                    }
                }
            }
        }
    };
    SfMaps.prototype.setMarkerTranslate = function (element, layerIndex, markerIndex, dataIndex, point) {
        if ((this.dataSource[layerIndex][markerIndex][dataIndex][this.markerSettings[layerIndex][markerIndex].ShapeValuePath] === "Balloon" || this.markerSettings[layerIndex][markerIndex].Type == "Balloon") && !(element.id.indexOf('cluster') > -1)) {
            element.setAttribute('transform', 'translate( ' + (point.x - (this.markerSettings[layerIndex][markerIndex].Width / 2)) + ' ' + (point.y - this.markerSettings[layerIndex][markerIndex].Height) + ' )' + 'scale(' + this.markerSettings[layerIndex][markerIndex].Width / 30 + ' ' + this.markerSettings[layerIndex][markerIndex].Height / 30 + ' ' + ')');
        }
        else if ((this.dataSource[layerIndex][markerIndex][dataIndex][this.markerSettings[layerIndex][markerIndex].ShapeValuePath] === "Rectangle" || this.markerSettings[layerIndex][markerIndex].Type == "Rectangle") && !(element.id.indexOf('cluster') > -1)) {
            element.setAttribute('transform', 'translate( ' + (point.x - (this.markerSettings[layerIndex][markerIndex].Width / 2)) + ' ' + (point.y - (this.markerSettings[layerIndex][markerIndex].Height / 2)) + ' )');
        }
        else {
            element.setAttribute('transform', 'translate( ' + point.x + ' ' + point.y + ' )');
        }
    };
    SfMaps.prototype.markerTranslate = function (element, factor, x, y, scale, template) {
        var layerIndex = parseInt(element.id.split('_LayerIndex_')[1].split('_')[0], 10);
        var markerIndex = parseInt(element.id.split('_MarkerIndex_')[1].split('_')[0], 10);
        var dataIndex = parseInt(element.id.split('_dataIndex_')[1].split('_')[0], 10);
        var longitude = (this.dataSource[layerIndex]['length'] !== 0 ?
            (this.dataSource[layerIndex][markerIndex].length !== 0 ?
                this.dataSource[layerIndex][markerIndex][dataIndex]['Longitude'] : null) : null);
        var latitude = (this.dataSource[layerIndex]['length'] !== 0 ? (this.dataSource[layerIndex][markerIndex].length !== 0 ?
            this.dataSource[layerIndex][markerIndex][dataIndex]['Latitude'] : null) : null);
        if (latitude && longitude) {
            if (this.isTileMap) {
                var point = this.convertTileLatLongToPoint({ x: longitude, y: latitude }, this.tileZoomLevel, this.tileTranslatePoint, true);
                point['x'] = point['x'] + this.markerSettings[layerIndex][markerIndex]['X'];
                point['y'] = point['y'] + this.markerSettings[layerIndex][markerIndex]['Y'];
                if (!template) {
                    this.setMarkerTranslate(element, layerIndex, markerIndex, dataIndex, point);
                }
                else {
                    element['style']['left'] = point.x + 'px';
                    element['style']['top'] = point.y + 'px';
                }
            }
            else {
                var location_2 = this.convertGeoToPoint(latitude, longitude, factor);
                location_2.x = ((location_2.x + x) * scale) + this.markerSettings[layerIndex][markerIndex]['X'];
                location_2.y = ((location_2.y + y) * scale) + this.markerSettings[layerIndex][markerIndex]['Y'];
                if (!template) {
                    this.setMarkerTranslate(element, layerIndex, markerIndex, dataIndex, location_2);
                }
                else {
                    element['style']['left'] = (location_2.x - this.areaRect.x) + 'px';
                    element['style']['top'] = (location_2.y - this.areaRect.y) + 'px';
                }
            }
        }
    };
    SfMaps.prototype.navigationTranslate = function (element, factor, layerIndex, lineIndex) {
        if (!sf.base.isNullOrUndefined(this.navigation[layerIndex])) {
            var angle = this.navigation[layerIndex][lineIndex].Angle;
            var direction = void 0;
            var d = void 0;
            var point = [];
            if (this.navigation[layerIndex][lineIndex].Latitude.length === this.navigation[layerIndex][lineIndex].Longitude.length) {
                for (var i = 0; i < this.navigation[layerIndex][lineIndex].Latitude.length; i++) {
                    var location_3 = this.isTileMap ? this.convertTileLatLongToPoint({ x: this.navigation[layerIndex][lineIndex].Longitude[i], y: this.navigation[layerIndex][lineIndex].Latitude[i] }, this.tileZoomLevel, this.tileTranslatePoint, true) :
                        this.convertGeoToPoint(this.navigation[layerIndex][lineIndex].Latitude[i], this.navigation[layerIndex][lineIndex].Longitude[i], factor);
                    point.push(location_3);
                }
            }
            for (var j = 0; j < point['length'] - 1; j++) {
                angle = (-1 > angle) ? -1 : angle;
                angle = (1 < angle) ? 1 : angle;
                var radius1 = this.convertRadius(point[j], point[j + 1]);
                if (angle <= 1 && angle > 0) {
                    direction = 0;
                    if (point[j]['x'] > point[j + 1]['x']) {
                        direction = 1;
                    }
                }
                if (angle >= -1 && angle < 0) {
                    direction = 1;
                    if (point[j]['x'] > point[j + 1]['x']) {
                        direction = 0;
                    }
                }
                if (point[j]['x'] !== point[j + 1]['x']) {
                    angle = Math.abs(angle);
                    d = (angle === 0) ? 'M ' + point[j]['x'] + ',' + point[j]['y'] + 'L ' + point[j + 1]['x']
                        + ',' + point[j + 1]['y'] + ' ' :
                        'M ' + point[j]['x'] + ',' + point[j]['y'] + ' A ' + (radius1 / 2 + (1 - angle) * radius1 / (angle * 10)) +
                            ' ' + (radius1 / 2 + (1 - angle) * radius1 / (angle * 10)) + ' ' + 0 + ',' + 0 + ','
                            + direction + ' , ' + point[j + 1]['x'] + ',' + point[j + 1]['y'] + ' ';
                }
                element.setAttribute('d', d);
            }
        }
    };
    SfMaps.prototype.dataLabelTranslate = function (element, factor, x, y, scale, type) {
        var layerIndex = parseInt(element.id.split('_LayerIndex_')[1].split('_')[0], 10);
        var labelIndex = parseFloat(element.id.split('_shapeIndex_')[1].split('_')[0]);
        var labelX;
        var labelY;
        if (type !== 'Template') {
            var currentLabel = this.labelCollection[layerIndex][labelIndex];
            labelX = currentLabel['LocationX'];
            labelY = currentLabel['LocationY'];
            var padding = 5;
            labelX = ((labelX + x) * scale) - (currentLabel['BorderSize']['Height'] / 4);
            labelY = ((labelY + y) * scale) + (currentLabel['BorderSize']['Height'] / 2);
            if (element.id.indexOf('_RectIndex_') > -1) {
                element.setAttribute('x', (labelX - (parseInt(element.getAttribute('width')) / 2)).toString());
                element.setAttribute('y', (labelY - padding - (parseInt(element.getAttribute('height')) / 2)).toString());
            }
            else {
                element.setAttribute('transform', 'translate( ' + labelX + ' ' + labelY + ' )');
            }
        }
        else {
            var currentLabel = this.dataLabelTemplateCollection[layerIndex][labelIndex];
            labelX = (currentLabel['LocationX'] + x) * scale;
            labelY = (currentLabel['LocationY'] + y) * scale;
            element['style']['left'] = labelX + 'px';
            element['style']['top'] = labelY + 'px';
        }
    };
    SfMaps.prototype.convertTileLatLongToPoint = function (center, zoomLevel, tileTranslatePoint, isMapCoordinates) {
        var size = Math.pow(2, zoomLevel) * 256;
        var x = (center.x + 180) / 360;
        var sinLatitude = Math.sin(center.y * Math.PI / 180);
        var y = 0.5 - Math.log((1 + sinLatitude) / (1 - sinLatitude)) / (4 * Math.PI);
        var pixelX = center.x;
        var pixelY = center.y;
        if (isMapCoordinates) {
            pixelX = (x * size + 0.5) + tileTranslatePoint.x;
            pixelY = (y * size + 0.5) + tileTranslatePoint.y;
        }
        return { x: pixelX, y: pixelY };
    };
    SfMaps.prototype.panning = function (xDifference, yDifference, layerX, layerY, scaleFactor) {
        if (!this.isTileMap) {
            var previousPoint = this.shapeTranslatePoint;
            var x = this.shapeTranslatePoint.x - xDifference / scaleFactor;
            var y = this.shapeTranslatePoint.y - yDifference / scaleFactor;
            var layerRect = document.getElementById(this.element.id + '_LayerCollections').getBoundingClientRect();
            var elementRect = document.getElementById(this.element.id + '_svg').getBoundingClientRect();
            var panningXDirection = ((xDifference < 0 ? layerRect.left <= (elementRect.left + this.areaRect.x) :
                ((layerRect.left + layerRect.width) >= (elementRect.left + elementRect.width) + this.areaRect.x + this.marginLeft)));
            var panningYDirection = ((yDifference < 0 ? layerRect.top <= (elementRect.top + this.areaRect.y) :
                ((layerRect.top + layerRect.height + this.marginTop) >= (elementRect.top + elementRect.height))));
            if (panningXDirection && panningYDirection) {
                this.shapeTranslatePoint = { x: x, y: y };
                this.applyTransform();
            }
            else if (panningXDirection) {
                this.shapeTranslatePoint = { x: x, y: this.shapeTranslatePoint.y };
                this.applyTransform();
            }
            else if (panningYDirection) {
                this.shapeTranslatePoint = { x: this.shapeTranslatePoint.x, y: y };
                this.applyTransform();
            }
        }
        else {
            var x = this.tileTranslatePoint.x - xDifference;
            var y = this.tileTranslatePoint.y - yDifference;
            this.tileTranslatePoint.x = x;
            this.tileTranslatePoint.y = y;
            if ((this.tileTranslatePoint.y > -10 && yDifference < 0) ||
                ((this.tileTranslatePoint.y < -((Math.pow(2, scaleFactor) - 2) * 256) && yDifference > 0))) {
                this.tileTranslatePoint.x = x + xDifference;
                this.tileTranslatePoint.y = y + yDifference;
            }
            this.translatePoint.x = (this.tileTranslatePoint.x - xDifference) / Math.pow(2, scaleFactor - 1);
            this.translatePoint.y = (this.tileTranslatePoint.y - yDifference) / Math.pow(2, scaleFactor - 1);
            this.generateTiles();
            this.renderMarkers();
        }
    };
    SfMaps.prototype.applyTransform = function () {
        var x = !this.isTileMap ? this.shapeTranslatePoint.x : this.translatePoint.x;
        var y = !this.isTileMap ? this.shapeTranslatePoint.y : this.translatePoint.y;
        var layerCollection = document.getElementById(this.element.id + '_LayerCollections');
        var factor = this.calculateFactor();
        for (var i = 0; i < layerCollection.childNodes.length; i++) {
            var layerElement = layerCollection.childNodes[i];
            if (layerElement.COMMENT_NODE === layerElement.nodeType) {
                continue;
            }
            if (layerElement.tagName === 'g') {
                var index = layerElement.id.indexOf('_LayerIndex_') > -1 &&
                    parseFloat(layerElement.id.split('_LayerIndex_')[1].split('_')[0]);
                for (var j = 0; j < layerElement.childNodes.length; j++) {
                    var currentEle = layerElement.childNodes[j];
                    if (currentEle.COMMENT_NODE === currentEle.nodeType) {
                        continue;
                    }
                    if (!sf.base.isNullOrUndefined(currentEle.id)) {
                        if (currentEle.id.indexOf('Polygon') > -1 || currentEle.id.indexOf('_line_Group') > -1) {
                            currentEle.setAttribute('transform', 'scale(' + this.scaleFactor + ') translate( ' + x + ' ' + y + ' )');
                        }
                        else if (currentEle.id.indexOf('_MarkerGroup') > -1) {
                            for (var k = 0; k < currentEle.childNodes.length; k++) {
                                if (!sf.base.isNullOrUndefined(currentEle.childNodes[k].id)) {
                                    this.markerTranslate(currentEle.childNodes[k], factor, x, y, this.scaleFactor, false);
                                }
                            }
                        }
                        else if (currentEle.id.indexOf('_bubble_Group_') > -1) {
                            for (var k = 0; k < currentEle.childNodes.length; k++) {
                                var childElement = currentEle.childNodes[k];
                                if (!sf.base.isNullOrUndefined(childElement.id)) {
                                    var layerIndex = parseFloat(childElement.id.split('_LayerIndex_')[1].split('_')[0]);
                                    var bubleIndex = parseFloat(childElement.id.split('_BubbleIndex_')[1].split('_')[0]);
                                    var dataIndex = parseFloat(childElement.id.split('_BubbleIndex_')[1].split('_')[2]);
                                    if (this.bubbleCollection[layerIndex] && this.bubbleCollection[layerIndex][bubleIndex]) {
                                        var currentBubble = this.bubbleCollection[layerIndex][bubleIndex][dataIndex];
                                        var centerX = currentBubble['ZoomTranslatePoint']['X'];
                                        var centerY = currentBubble['ZoomTranslatePoint']['Y'];
                                        var currentX = ((centerX + x) * this.scaleFactor);
                                        var currentY = ((centerY + y) * this.scaleFactor);
                                        childElement.setAttribute('transform', 'translate( ' + currentX + ' ' + currentY + ' )');
                                    }
                                }
                            }
                        }
                        else if (currentEle.id.indexOf('_dataLableIndex_Group') > -1) {
                            for (var k = 0; k < currentEle.childNodes.length; k++) {
                                this.dataLabelTranslate(currentEle.childNodes[k], factor, x, y, this.scaleFactor, 'DataLabel');
                            }
                        }
                    }
                }
            }
        }
        for (var m = 0; m < this.layerCount; m++) {
            var templateCollection = document.getElementById(this.element.id + '_LayerIndex_' + m + '_Markers_Template_Group');
            this.markerTemplateTranslate(templateCollection, factor, x, y);
            var labelCollection = document.getElementById(this.element.id + '_LayerIndex_' + m + '_DataLabel_Group');
            this.labelTemplateTranslate(labelCollection, x, y);
        }
    };
    SfMaps.prototype.markerTemplateTranslate = function (templateCollection, factor, x, y) {
        if (templateCollection) {
            for (var k = 0; k < templateCollection.childElementCount; k++) {
                this.markerTranslate(templateCollection.childNodes[k], factor, x, y, this.scaleFactor, true);
            }
        }
    };
    SfMaps.prototype.labelTemplateTranslate = function (labelTemplateCollection, x, y) {
        if (labelTemplateCollection) {
            var factor = this.isTileMap ? Math.pow(2, this.scaleFactor - 1) : this.scaleFactor;
            for (var k = 0; k < labelTemplateCollection.childElementCount; k++) {
                this.dataLabelTranslate(labelTemplateCollection.childNodes[k], 0, x, y, factor, 'Template');
            }
        }
    };
    SfMaps.prototype.calculateFactor = function () {
        var horFactor;
        var verFactor = 1;
        var divide = 10;
        var exp = 'e+1';
        var bounds = this.baseMapBounds;
        var mapSize = { width: this.areaRect.width, height: this.areaRect.height };
        var mapHeight;
        var mapWidth;
        if (bounds) {
            var start = this.convertGeoToPoint(bounds.latitudeMin, bounds.longitudeMin, null);
            var end = this.convertGeoToPoint(bounds.latitudeMax, bounds.longitudeMax, null);
            mapHeight = end.y - start.y;
            mapWidth = end.x - start.x;
        }
        else {
            mapHeight = mapWidth = 500;
        }
        if (mapHeight < mapSize.height) {
            horFactor = parseFloat(Math.abs(Number(mapSize.height / Number(mapHeight.toString() + exp)) * 100).toString().split('.')[0])
                / divide;
        }
        else {
            horFactor = mapSize.height / mapHeight;
        }
        if (mapWidth < mapSize.width) {
            verFactor = parseFloat(Math.abs(Number(mapSize.width / Number(mapWidth.toString() + exp)) * 100).toString().split('.')[0])
                / divide;
        }
        else {
            verFactor = mapSize.width / mapWidth;
        }
        return (Math.min(verFactor, horFactor));
    };
    SfMaps.prototype.convertRadius = function (point1, point2) {
        var value1 = point2['x'] - point1['x'];
        var value2 = point2['y'] - point1['y'];
        var value = Math.sqrt((Math.pow(value1, 2) + Math.pow(value2, 2)));
        return value;
    };
    SfMaps.prototype.convertGeoToPoint = function (latitude, longitude, factor) {
        var mapSize = { width: this.areaRect.width, height: this.areaRect.height };
        var x;
        var y;
        var value;
        var lat;
        var lng;
        var temp;
        var latRadian = this.degreesToRadians(latitude);
        var lngRadian = this.degreesToRadians(longitude);
        var size = factor === null || factor === undefined ? Math.min(mapSize.width, mapSize.height) :
            Math.min(mapSize.width, mapSize.height) * factor;
        var type = this.projectionType;
        switch (type) {
            case 'Mercator':
                var pixelOrigin = { x: size / 2, y: size / 2 };
                x = pixelOrigin.x + longitude * (size / 360);
                var sinY = this.calculateBound(Math.sin(this.degreesToRadians(latitude)), -0.9999, 0.9999);
                y = pixelOrigin.y + 0.5 * (Math.log((1 + sinY) / (1 - sinY))) * (-(size / (2 * Math.PI)));
                break;
            case 'Winkel3':
                value = this.aitoff(lngRadian, latRadian);
                lng = (value.x + lngRadian / (Math.PI / 2)) / 2;
                lat = (value.y + latRadian) / 2;
                break;
            case 'Miller':
                lng = lngRadian;
                lat = (1.25 * Math.log(Math.tan((Math.PI / 4) + (.4 * latRadian))));
                break;
            case 'Eckert3':
                temp = Math.sqrt(Math.PI * (4 + Math.PI));
                lng = 2 / temp * lngRadian * (1 + Math.sqrt(1 - 4 * latRadian * latRadian / (Math.PI * Math.PI)));
                lat = 4 / temp * latRadian;
                break;
            case 'AitOff':
                value = this.aitoff(lngRadian, latRadian);
                lng = value.x;
                lat = value.y;
                break;
            case 'Eckert5':
                lng = lngRadian * (1 + Math.cos(latRadian)) / Math.sqrt(2 + Math.PI);
                lat = 2 * latRadian / Math.sqrt(2 + Math.PI);
                break;
            case 'Equirectangular':
                lng = lngRadian;
                lat = latRadian;
                break;
            case 'Eckert6':
                var epsilon = 1e-6;
                temp = (1 + (Math.PI / 2)) * Math.sin(latRadian);
                var delta = Infinity;
                for (var i = 0; i < 10 && Math.abs(delta) > epsilon; i++) {
                    delta = (latRadian + (Math.sin(latRadian)) - temp) / (1 + Math.cos(latRadian));
                    latRadian = latRadian - delta;
                }
                temp = Math.sqrt(2 + Math.PI);
                lng = lngRadian * (1 + Math.cos(latRadian)) / temp;
                lat = 2 * latRadian / temp;
                break;
        }
        return { x: x, y: y };
    };
    SfMaps.prototype.aitoff = function (x, y) {
        var cosy = Math.cos(y);
        var sincia = Math.sin(Math.cos(cosy * Math.cos(x /= 2)));
        return { x: 2 * cosy * Math.sin(x) * sincia, y: Math.sin(y) * sincia };
    };
    SfMaps.prototype.degreesToRadians = function (deg) {
        return deg * (Math.PI / 180);
    };
    SfMaps.prototype.calculateBound = function (value, min, max) {
        if (!sf.base.isNullOrUndefined(min)) {
            value = Math.max(value, min);
        }
        if (!sf.base.isNullOrUndefined(max)) {
            value = Math.min(value, max);
        }
        return value;
    };
    SfMaps.prototype.getTileGeoLocation = function (layerX, layerY) {
        var container = this.getBound(this.element.id);
        var element = this.getBound(this.element.id + '_tile_parent');
        return this.pointToLatLong(layerX + this.areaRect.x - (element.x - container.x), layerY +
            this.areaRect.y - (element.y - container.y));
    };
    SfMaps.prototype.pointToLatLong = function (pageX, pageY) {
        var mapSize = 256 * Math.pow(2, this.scaleFactor);
        var x1 = (this.clip(pageX - (this.translatePoint.x * this.scaleFactor), 0, mapSize - 1) / mapSize) - 0.5;
        var y1 = 0.5 - (this.clip(pageY - (this.translatePoint.y * this.scaleFactor), 0, mapSize - 1) / mapSize);
        var lat = 90 - 360 * Math.atan(Math.exp(-y1 * 2 * Math.PI)) / Math.PI;
        var lng = 360 * x1;
        return { latitude: lat, longitude: lng };
    };
    SfMaps.prototype.generateTiles = function () {
        var size = { width: this.baseMapBounds.availableSize.width, height: this.baseMapBounds.availableSize.height };
        var xCount = 0;
        var yCount = 0;
        var xLeft = 0;
        var xRight = 0;
        xCount = yCount = Math.pow(2, this.tileZoomLevel);
        var tiles = [];
        if ((this.tileTranslatePoint.x + (xCount * 256)) < size.width) {
            xLeft = this.tileTranslatePoint.x > 0 ? Math.ceil(this.tileTranslatePoint.x / 256) : 0;
            xRight = ((this.tileTranslatePoint.x + xCount * 256) < size.width) ?
                Math.ceil((size.width - (this.tileTranslatePoint.x + xCount * 256)) / 256) : 0;
        }
        xCount += xLeft + xRight;
        xCount = (this.horizontalPanXCount >= xCount) ? this.horizontalPanXCount : xCount;
        this.horizontalPan = false;
        var endY = Math.min(yCount, ((-this.tileTranslatePoint.y + size.height) / 256) + 1);
        var endX = Math.min(xCount, ((-this.tileTranslatePoint.x + size.width + (xRight * 256)) / 256) + 1);
        var startX = (-((this.tileTranslatePoint.x + (xLeft * 256)) + 256) / 256);
        var startY = (-(this.tileTranslatePoint.y + 256) / 256);
        for (var i = Math.round(startX); i < Math.round(endX); i++) {
            for (var j = Math.round(startY); j < Math.round(endY); j++) {
                var x = 256 * i + this.tileTranslatePoint.x;
                var y = 256 * j + this.tileTranslatePoint.y;
                if (x > -256 && x <= size.width && y > -256 && y < size.height) {
                    if (j >= 0) {
                        var tileI = i;
                        if (i < 0) {
                            tileI = (tileI % yCount) + yCount;
                        }
                        var tile = { x: tileI % yCount, y: j, left: x, top: y, height: 256, width: 256, src: null };
                        if (this.urlTemplate.indexOf('virtualearth') === -1) {
                            tile.src = this.urlTemplate.replace('level', this.scaleFactor.toString()).replace('tileX', tile.x.toString()).replace('tileY', tile.y.toString());
                        }
                        else {
                            tile.src = this.getBingMap(tile, this.urlTemplate);
                        }
                        tiles.push(tile);
                    }
                }
            }
        }
        var animatedElement = document.getElementById(this.element.id + '_animated_tiles');
        for (var j = 0; j < tiles.length; j++) {
            var tileElement = document.getElementById(this.element.id + 'tile' + j);
            var imgElement = null;
            var isNewTile = false;
            if (!tileElement) {
                tileElement = document.createElement('div');
                tileElement.id = this.element.id + 'tile' + j;
                tileElement.style.userSelect = 'none';
                imgElement = document.createElement('img');
                if (this.newTiles.indexOf(j) === -1) {
                    this.newTiles.push(j);
                }
                isNewTile = true;
            }
            else {
                imgElement = tileElement.childNodes[0];
            }
            if (imgElement.src !== tiles[j].src) {
                imgElement.setAttribute('src', tiles[j].src);
            }
            tileElement.style.position = 'absolute';
            tileElement.style.left = tiles[j].left + 'px';
            tileElement.style.top = tiles[j].top + 'px';
            tileElement.style.height = tiles[j].height + 'px';
            tileElement.style.width = tiles[j].width + 'px';
            if (isNewTile) {
                tileElement.appendChild(imgElement);
                animatedElement.appendChild(tileElement);
            }
        }
    };
    SfMaps.prototype.getBingMap = function (tile, imageUrl) {
        var quadKey = '';
        // eslint-disable-next-line prefer-const
        var subDomains = ['t0', 't1', 't2', 't3'];
        var maxZoom = Math.min(this.tileZoomLevel, this.options.maxZoom);
        for (var i = maxZoom; i > 0; i--) {
            var digit = 0;
            var mask = 1 << (i - 1);
            if ((tile.x & mask) !== 0) {
                digit++;
            }
            if ((tile.y & mask) !== 0) {
                digit += 2;
            }
            quadKey = quadKey + '' + digit;
        }
        var subDomain = subDomains[Math.min(parseInt(quadKey.substr(quadKey.length - 1, 1), 10), subDomains.length)];
        imageUrl = imageUrl.replace('{quadkey}', quadKey).replace('{subdomain}', subDomain);
        return imageUrl += '&mkt=' + 'en-US' + '&ur=IN&Key=' + this.key;
    };
    SfMaps.prototype.clip = function (value, minValue, maxValue) {
        return Math.min(Math.max(value, minValue), maxValue);
    };
    SfMaps.prototype.svgPoint = function (elem, x, y) {
        var p = this.svg.createSVGPoint();
        p.x = x;
        p.y = y;
        return p.matrixTransform(elem.getScreenCTM().inverse());
    };
    SfMaps.prototype.clusterExpand = function (target, options, dotnetRef) {
        var datasource = JSON.parse(options.dataSource);
        var layerIndex = parseInt(target[0].split('_LayerIndex_')[1].split('_')[0], 10);
        var clusterSetting = this.markerClusterCollection[layerIndex];
        var data;
        var id = target[0].split('_LayerIndex_');
        var index = parseInt(id[1].split('_')[0], 10);
        var markCollection = [];
        var clusterCollection = [];
        if (target[0].indexOf('_MarkerIndex_') > -1) {
            var markerIndex = parseInt(id[1].split('_MarkerIndex_')[1].split('_')[0], 10);
            var dataIndex = parseInt(id[1].split('_dataIndex_')[1].split('_')[0], 10);
            if (!isNaN(markerIndex)) {
                data = datasource[dataIndex];
                var collection1 = [];
                if (target[0].indexOf('_cluster_') > -1 && clusterSetting.AllowClustering) {
                    for (var i = 0; i < datasource.length; i++) {
                        if (datasource[i]['Latitude'] === data['Latitude'] && datasource[i]['Longitude'] === data['Longitude']) {
                            collection1.push({ data: data, index: index });
                        }
                    }
                }
                if ((target[0].indexOf('_cluster_') > -1)) {
                    var isClusterSame = false;
                    var clusterElement = document.getElementById(target[0].indexOf('_datalabel_') > -1 ?
                        target[0].split('_datalabel_')[0] : target[0]);
                    var indexes = void 0;
                    if (this.markerClusterCollection[layerIndex].Shape !== 'Balloon' && !sf.base.isNullOrUndefined(clusterElement)) {
                        indexes = clusterElement.innerHTML.split(',').map(Number);
                    }
                    else if (!sf.base.isNullOrUndefined(clusterElement.firstElementChild)) {
                        indexes = clusterElement.firstElementChild.innerHTML.split(',').map(Number);
                    }
                    collection1 = [];
                    for (var k = 0, collectionIndex = indexes; k < collectionIndex.length; k++) {
                        var i = collectionIndex[k];
                        collection1.push({ data: datasource[i], index: i });
                        datasource[i]['text'] = '';
                        markCollection.push(datasource[i]);
                    }
                    isClusterSame = false;
                    clusterCollection.push({
                        data: collection1, layerIndex: index, markerIndex: markerIndex, dataIndex: dataIndex,
                        targetClusterIndex: +(target[0].split('_cluster_')[1].indexOf('_datalabel_') > -1 ?
                            target[0].split('_cluster_')[1].split('_datalabel_')[0] : target[0].split('_cluster_')[1]),
                        isClusterSame: isClusterSame
                    });
                }
            }
        }
        if (clusterCollection.length > 0) {
            this.sameMarkerData = clusterCollection;
            var markerGroup = document.getElementById(id[0] + '_LayerIndex_' + layerIndex + '_MarkerGroup');
            if (this.markerClusterExpandCheck) {
                this.mergeSeparateCluster(this.sameMarkerData, id[0]);
                this.markerClusterExpandCheck = false;
            }
            else {
                this.clusterSeparate(this.sameMarkerData, markerGroup, true, id[0], clusterSetting);
                this.markerClusterExpandCheck = true;
            }
        }
    };
    SfMaps.prototype.mergeSeparateCluster = function (sameMarkerData, id) {
        var layerIndex = sameMarkerData[0].layerIndex;
        var clusterIndex = sameMarkerData[0].targetClusterIndex;
        var markerIndex = sameMarkerData[0].markerIndex;
        var dataIndex = sameMarkerData[0].dataIndex;
        var markerId = id + '_LayerIndex_' + layerIndex + '_MarkerIndex_' + markerIndex;
        var clusterId = markerId + '_dataIndex_' + dataIndex + '_cluster_' + clusterIndex;
        var clusterEle = this.getElementId(clusterId);
        var clusterEleLabel = this.getElementId(clusterId + '_datalabel_' + clusterIndex);
        clusterEle.setAttribute('visibility', 'visible');
        clusterEleLabel['style']['visibility'] = 'visible';
        var markerEle;
        var markerDataLength = sameMarkerData[0].data.length;
        if (this.clusterLocation.length != 0) {
            for (var i = 0; i < markerDataLength; i++) {
                markerEle = this.getElementId(markerId + '_dataIndex_' + sameMarkerData[0].data[i]['index']);
                markerEle.setAttribute('transform', 'translate( ' + this.clusterLocation[i]['x'] + ' ' + this.clusterLocation[i]['y'] + ')');
                markerEle.setAttribute('style', 'visibility:hidden; content:marker');
            }
        }
        this.clusterLocation = [];
        this.removeElement(id + '_LayerIndex_' + layerIndex + '_MarkerIndex_' + markerIndex + '_markerClusterConnectorLine');
    };
    SfMaps.prototype.removeElement = function (id) {
        var element = document.getElementById(id);
        if (!sf.base.isNullOrUndefined(element)) {
            element.remove();
        }
    };
    SfMaps.prototype.drawPath = function (options) {
        var path = document.getElementById(options.id);
        if (sf.base.isNullOrUndefined(path)) {
            path = document.createElementNS('http://www.w3.org/2000/svg', 'path');
        }
        path.setAttribute('id', options.id);
        path.setAttribute('d', options.d);
        path.setAttribute('opacity', options.opacity.toString());
        path.setAttribute('stroke', options.stroke);
        path.setAttribute('stroke-width', options.strokeWidth.toString());
        return path;
    };
    SfMaps.prototype.clusterSeparate = function (sameMarkerData, markerElement, isDom, id, connectorLine) {
        var layerIndex = sameMarkerData[0].layerIndex;
        var markerIndex = sameMarkerData[0].markerIndex;
        var clusterIndex = sameMarkerData[0].targetClusterIndex;
        var dataIndex = sameMarkerData[0].dataIndex;
        var container = document.getElementById(id);
        var containerBound = container.getBoundingClientRect();
        var getElementFunction = isDom ? this.getElementId : markerElement.querySelector.bind(markerElement);
        var getQueryConnect = isDom ? '' : '#';
        var markerId = id + '_LayerIndex_' + layerIndex + '_MarkerIndex_' + markerIndex;
        var clusterId = markerId + '_dataIndex_' + dataIndex + '_cluster_' + clusterIndex;
        var clusterEle = getElementFunction(getQueryConnect + '' + clusterId);
        var clusterEleLabel = getElementFunction(getQueryConnect + '' + clusterId + '_datalabel_' + clusterIndex);
        clusterEle.setAttribute('visibility', 'hidden');
        clusterEleLabel.setAttribute('visibility', 'hidden');
        var markerEle = getElementFunction(getQueryConnect + '' + markerId + '_dataIndex_' + dataIndex);
        var height = 25;
        var width = 25;
        var centerX = +clusterEle.getAttribute('transform').split('translate(')[1].trim().split(' ')[0];
        var centerY = +clusterEle.getAttribute('transform').split('translate(')[1].trim().split(' ')[1].split(')')[0].trim();
        var radius = width + 5;
        var area = 2 * 3.14 * radius;
        var totalMarker = 0;
        var numberOfMarker = Math.round(area / width);
        totalMarker += numberOfMarker;
        var markerDataLength = sameMarkerData[0].data.length;
        var percent = Math.round((height / area) * 100);
        percent = markerDataLength < numberOfMarker ? 100 / markerDataLength : percent;
        var angle = (percent / 100) * 360;
        var newAngle = markerDataLength < numberOfMarker ? 45 : 0;
        var count = 1;
        var start = 'M ' + centerX + ' ' + centerY + ' ';
        var path = '';
        for (var i = 0; i < markerDataLength; i++) {
            if (totalMarker === i || Math.round(newAngle) >= 360) {
                count++;
                radius = (width + 5) * count;
                newAngle = 0;
                area = 2 * 3.14 * radius;
                numberOfMarker = Math.round(area / height);
                percent = Math.round((height / area) * 100);
                while (percent * numberOfMarker < 100) {
                    numberOfMarker++;
                }
                angle = ((percent / 100) * 360);
                totalMarker += numberOfMarker;
            }
            var x1 = centerX + radius * Math.sin((Math.PI * 2 * newAngle) / 360);
            var y1 = centerY + radius * Math.cos((Math.PI * 2 * newAngle) / 360);
            path += start + 'L ' + (x1) + ' ' + y1 + ' ';
            markerEle = getElementFunction(getQueryConnect + '' + markerId + '_dataIndex_' + sameMarkerData[0].data[i]['index']);
            var bounds = markerEle.getBoundingClientRect();
            this.clusterLocation.push({ x: bounds['left'] - containerBound['left'], y: bounds['top'] - containerBound['top'] });
            var markerInfo = this.markerSettings[layerIndex][markerIndex];
            if (markerInfo.Type === 'Balloon') {
                markerEle.setAttribute('transform', 'translate( ' + (x1 - (markerInfo.Width / 2)) + ' ' + (y1 - markerInfo.Height) + '  ) ' + 'scale( ' + markerInfo.Width / 30 + ' ' + markerInfo.Height / 30 + ' ) ');
            }
            else {
                markerEle.setAttribute('transform', 'translate( ' + x1 + ' ' + y1 + ')');
            }
            markerEle['style']['visibility'] = 'visible';
            clusterEleLabel['style']['visibility'] = 'hidden';
            newAngle += angle;
        }
        var options = {
            d: path, id: id + '_LayerIndex_' + layerIndex + '_MarkerIndex_' + markerIndex + '_dataIndex_' +
                dataIndex + '_markerClusterConnectorLine', stroke: sf.base.isNullOrUndefined(connectorLine.LineColor) ? '#000000' :
                connectorLine.LineColor, opacity: connectorLine.LineOpacity, strokeWidth: connectorLine.LineWidth
        };
        markerElement = isDom ? getElementFunction(id + '_LayerIndex_' + layerIndex + '_MarkerGroup') : markerElement;
        var groupEle = createGroup(id + '_LayerIndex_' + layerIndex + '_MarkerIndex_' + markerIndex +
            '_markerClusterConnectorLine');
        groupEle.appendChild(this.drawPath(options));
        markerElement.insertBefore(groupEle, markerElement.querySelector('#' + markerId + '_dataIndex_0'));
    };
    SfMaps.prototype.getElementId = function (id) {
        return document.getElementById(id);
    };
    SfMaps.prototype.shapeHighlightHandle = function (fill, opacity, layerIndex) {
        var highlighProperty = this.options.layerHighlightSettings[layerIndex];
        this.handleHighlight(this.targetElementPath, 'Shape', { BorderColor: highlighProperty['BorderColor'], BorderWidth: highlighProperty['BorderWidth'], BorderOpacity: highlighProperty['BorderOpacity'], Enable: highlighProperty['Enable'], EnableMultiSelect: highlighProperty['EnableMultiSelect'], Fill: fill, Opacity: opacity });
    };
    SfMaps.prototype.itemHighlightHandle = function (fill, opacity, layerIndex, settingIndex, type) {
        if (type === 'Bubble') {
            var bubbleHighlighProperty = this.options.bubbleHighlightSettings[layerIndex][settingIndex];
            this.handleHighlight(this.targetElementPath, 'Bubble', { BorderColor: bubbleHighlighProperty['BorderColor'], BorderWidth: bubbleHighlighProperty['BorderWidth'], BorderOpacity: bubbleHighlighProperty['BorderOpacity'], Enable: bubbleHighlighProperty['Enable'], EnableMultiSelect: bubbleHighlighProperty['EnableMultiSelect'], Fill: fill, Opacity: opacity });
        }
        if (type === 'Marker') {
            var markerHighlighProperty = this.options.markerHighlightSettings[layerIndex][settingIndex];
            this.handleHighlight(this.targetElementPath, 'Marker', { BorderColor: markerHighlighProperty['BorderColor'], BorderWidth: markerHighlighProperty['BorderWidth'], BorderOpacity: markerHighlighProperty['BorderOpacity'], Enable: markerHighlighProperty['Enable'], EnableMultiSelect: markerHighlighProperty['EnableMultiSelect'], Fill: fill, Opacity: opacity });
        }
        if (type === 'Navigation') {
            var navigationHighlighProperty = this.options.navigationHighlightSettings[layerIndex][settingIndex];
            this.handleHighlight(this.targetElementPath, 'Navigation', { BorderColor: navigationHighlighProperty['BorderColor'], BorderWidth: navigationHighlighProperty['BorderWidth'], BorderOpacity: navigationHighlighProperty['BorderOpacity'], Enable: navigationHighlighProperty['Enable'], EnableMultiSelect: navigationHighlighProperty['EnableMultiSelect'], Fill: fill, Opacity: opacity });
            if (this.targetElementPath.getAttribute('marker-start') !== '' || this.targetElementPath.getAttribute('marker-end') !== '') {
                var highlightNavigate = document.getElementById(this.targetElementPath['id'] + '_triangle');
                this.handleHighlight(highlightNavigate, 'Arrow', { BorderColor: highlightNavigate['BorderColor'], BorderWidth: highlightNavigate['BorderWidth'], BorderOpacity: highlightNavigate['BorderOpacity'], Enable: highlightNavigate['Enable'], EnableMultiSelect: highlightNavigate['EnableMultiSelect'], Fill: fill, Opacity: opacity });
            }
        }
    };
    SfMaps.prototype.elementAnimate = function (element, delay, duration, x, y, ele, radius, dotNetRef, type, shape, width, markerHeight) {
        var _this = this;
        if (sf.base.isNullOrUndefined(radius)) {
            radius = 0;
        }
        var centerX = x;
        var centerY = y;
        var height = 0;
        new sf.base.Animation({}).animate(element, {
            duration: duration,
            delay: delay,
            progress: function (args) {
                if (args.timeStamp > args.delay) {
                    height = ((args.timeStamp - args.delay) / args.duration);
                    element.style.visibility = 'visible';
                    element.setAttribute('transform', 'translate( ' + (centerX - (radius * height)) + ' ' + (centerY - (radius * height)) +
                        ' ) scale(' + height + ')');
                }
            },
            end: function (model) {
                element.setAttribute('transform', '');
                if (type === 'Balloon' || shape === 'Balloon') {
                    element.setAttribute('transform', 'translate( ' + x + ' ' + y + '  ) ' + 'scale( ' + (width / 30) + ' ' + (markerHeight / 30) + ' ) ');
                }
                else {
                    element.setAttribute('transform', 'translate( ' + x + ' ' + y + '  )');
                }
                _this.dotNetRef.invokeMethodAsync('TriggerAnimation', element);
                if (!ele) {
                    return;
                }
            }
        });
    };
    SfMaps.prototype.markerAnimation = function (id, marker, dotNetRef) {
        var markers = JSON.parse(marker.markerData);
        for (var i = 0; i < markers.length; i++) {
            for (var j = 0; j < markers[i].MarkerAnimation.length; j++) {
                if (markers[i].MarkerAnimation[j].IsMarkerShape) {
                    for (var k = 0; k < markers[i].MarkerAnimation[j].DataSourceLength; k++) {
                        var markerChild = document.getElementById(id + '_LayerIndex_' + i + '_MarkerIndex_' + j + '_dataIndex_' + k);
                        var shapePath = !sf.base.isNullOrUndefined(this.dataSource[markers[i].MarkerAnimation[j].Layer][j][k][markers[i].MarkerAnimation[j].ShapeValuePath]) ? this.dataSource[markers[i].MarkerAnimation[j].Layer][j][k][markers[i].MarkerAnimation[j].ShapeValuePath] : ' ';
                        this.elementAnimate(markerChild, markers[i].MarkerAnimation[j].AnimationDelay, markers[i].MarkerAnimation[j].AnimationDuration, markerChild['transform']['baseVal'][0]['matrix']['e'], markerChild['transform']['baseVal'][0]['matrix']['f'], null, 0, dotNetRef, markers[i].MarkerAnimation[j].MarkerType, shapePath, markers[i].MarkerAnimation[j].Width, markers[i].MarkerAnimation[j].Height);
                    }
                }
            }
        }
    };
    SfMaps.prototype.bubbleAnimation = function (id, bubble, dotNetRef) {
        var bubbles = JSON.parse(bubble.bubbleData);
        for (var i = 0; i < bubbles.length; i++) {
            for (var j = 0; j < bubbles[i].MarkerAnimation.length; j++) {
                for (var k = 0; k < bubbles[i].MarkerAnimation[j].DataSourceLength; k++) {
                    var bubbleChild = document.getElementById(id + '_LayerIndex_' + i + '_BubbleIndex_' + j + '_dataIndex_' + k);
                    this.elementAnimate(bubbleChild, bubbles[i].MarkerAnimation[j].AnimationDelay, bubbles[i].MarkerAnimation[j].AnimationDuration, bubbleChild['transform']['baseVal'][0]['matrix']['e'], bubbleChild['transform']['baseVal'][0]['matrix']['f'], null, 0, dotNetRef, ' ', ' ', 0, 0);
                }
            }
        }
    };
    SfMaps.prototype.toolbarOpacity = function (toolbarButtonOpacity, toolbarShapeOpacity) {
        this.removeZoomOpacity(toolbarShapeOpacity, toolbarButtonOpacity, toolbarShapeOpacity, toolbarButtonOpacity, toolbarShapeOpacity, toolbarButtonOpacity, toolbarShapeOpacity, toolbarButtonOpacity, toolbarShapeOpacity, toolbarButtonOpacity);
    };
    SfMaps.prototype.clusterMarkers = function (layerIndex) {
        var markerCluster = this.markerClusterCollection[layerIndex];
        if (markerCluster.AllowClustering) {
            var bounds1 = void 0;
            var bounds2 = void 0;
            var id = this.id;
            var indexCollection = [];
            var colloideBounds = [];
            var tempX = 0;
            var tempY = 0;
            var clusterGroup = createGroup(id + '_LayerIndex_' + layerIndex + '_cluster');
            var postionY = (15 / 4);
            var m = 0;
            var markerCollection = void 0;
            var tempElement = void 0;
            var balloonGroup = void 0;
            var clusterColloideBounds = [];
            var markerTemplateCheck = void 0;
            var containerBound = document.getElementById(id);
            var markerTemplate = document.getElementById(id + '_LayerIndex_' + layerIndex + '_MarkerGroup');
            if (!sf.base.isNullOrUndefined(markerTemplate) && markerTemplate.childElementCount <= 0) {
                if (markerTemplate.childElementCount <= 0) {
                    markerTemplate = document.getElementById(id + '_LayerIndex_' + layerIndex + '_Markers_Template_Group');
                    markerTemplateCheck = true;
                }
            }
            markerTemplate = !sf.base.isNullOrUndefined(markerTemplate) ? markerTemplate : document.getElementById(id + '_LayerIndex_' +
                layerIndex + '_Markers_Template_Group');
            if (markerTemplate) {
                for (var o = 0; o < markerTemplate.childElementCount; o++) {
                    if (!(markerTemplate.children[o]['id'].indexOf('markerClusterConnectorLine') > -1)) {
                        indexCollection = [];
                        var markerIndex = parseInt(markerTemplate.children[o]['id'].split('_MarkerIndex_')[1].split('_')[0], 10);
                        if (markerTemplate.children[o]['style']['visibility'] !== 'hidden' && markerIndex === 0) {
                            tempElement = markerTemplate.children[o];
                            var bounds1_1 = tempElement.getBoundingClientRect();
                            indexCollection.push(o);
                            if (bounds1_1 !== null) {
                                for (var p = o + 1; p < markerTemplate.childElementCount; p++) {
                                    var secondMarkerIndex = parseInt(markerTemplate.children[p]['id'].split('_MarkerIndex_')[1].split('_')[0], 10);
                                    if (markerTemplate.children[p]['style']['visibility'] !== 'hidden' && secondMarkerIndex === 0) {
                                        tempElement = markerTemplate.children[p];
                                        var bounds2_1 = tempElement.getBoundingClientRect();
                                        if (bounds2_1 !== null) {
                                            if (!(bounds1_1.left > bounds2_1.right || bounds1_1.right < bounds2_1.left
                                                || bounds1_1.top > bounds2_1.bottom || bounds1_1.bottom < bounds2_1.top)) {
                                                colloideBounds.push(bounds2_1);
                                                markerTemplate.children[p].setAttribute('style', 'visibility:hidden; content:marker');
                                                indexCollection.push(p);
                                            }
                                        }
                                    }
                                }
                                tempX = bounds1_1.left + bounds1_1.width / 2;
                                tempY = bounds1_1.top + bounds1_1.height;
                                if (colloideBounds.length > 0) {
                                    var container = containerBound.getBoundingClientRect();
                                    tempX = this.isTileMap ? (tempX - container['left'] - this.areaRect.x) : tempX - container['left'];
                                    tempY = this.isTileMap ? (tempY - container['top'] - this.areaRect.y) : tempY - container['top'];
                                    var dataIndex = parseInt(markerTemplate.children[o]['id'].split('_dataIndex_')[1].split('_')[0], 10);
                                    var layerIndex_2 = parseInt(markerTemplate.children[o]['id'].split('_LayerIndex_')[1].split('_')[0], 10);
                                    var transform = markerTemplate.children[o].getAttribute('transform');
                                    if (transform) {
                                        tempX += (this.isTileMap ? 0 : markerTemplateCheck ? this.areaRect.x : 0);
                                        tempY += (this.isTileMap ? 0 : markerTemplateCheck ? this.areaRect.y : 0);
                                    }
                                    else {
                                        tempX = parseFloat(markerTemplate.children[o]['style']['left'].replace('px', '')) + (this.isTileMap ? 0 : markerTemplateCheck ? this.areaRect.x : 0);
                                        tempY = parseFloat(markerTemplate.children[o]['style']['top'].replace('px', '')) + (this.isTileMap ? 0 : markerTemplateCheck ? this.areaRect.y : 0);
                                    }
                                    var clusterID = id + '_LayerIndex_' + layerIndex_2 + '_MarkerIndex_' + markerIndex +
                                        '_dataIndex_' + dataIndex + '_cluster_' + (m);
                                    var labelID = id + '_LayerIndex_' + layerIndex_2 + '_MarkerIndex_' + markerIndex +
                                        '_dataIndex_' + dataIndex + '_cluster_' + (m) + '_datalabel_' + m;
                                    m++;
                                    var shape = void 0;
                                    switch (markerCluster.Shape) {
                                        case 'Circle':
                                            shape = this.createClusterShape('circle', clusterID, markerCluster.Fill, markerCluster.Opacity, 'translate( ' + tempX + ' ' + tempY + ' )', true, indexCollection, markerCluster);
                                            shape.setAttribute('r', ((markerCluster.Height + markerCluster.Width) / 4));
                                            break;
                                        case 'Rectangle':
                                            shape = this.createClusterShape('rect', clusterID, markerCluster.Fill, markerCluster.Opacity, 'translate( ' + tempX + ' ' + tempY + ' )', true, indexCollection, markerCluster);
                                            shape.setAttribute('x', -(markerCluster.Width / 2));
                                            shape.setAttribute('y', -(markerCluster.Height / 2));
                                            shape.setAttribute('height', (markerCluster.Height));
                                            shape.setAttribute('width', (markerCluster.Width));
                                            break;
                                        case 'Balloon':
                                            balloonGroup = createGroup(clusterID);
                                            var balloonPath = 'M15,0C8.8,0,3.8,5,3.8,11.2C3.8,17.5,9.4,24.4,15,30c5.6-5.6,11.2-12.5,11.2-18.8C26.2,5,21.2,0,15,0z M15,16' +
                                                'c-2.8,0-5-2.2-5-5s2.2-5,5-5s5,2.2,5,5S17.8,16,15,16z';
                                            shape = this.createClusterShape('path', clusterID, markerCluster.Fill, markerCluster.Opacity, 'translate( ' + (-(markerCluster.Width / 2)) + ', ' + (-(markerCluster.Height)) +
                                                ' ) scale( ' + (markerCluster.Width / 30) + ', ' + (markerCluster.Height / 30) + ' )', true, indexCollection, markerCluster);
                                            shape.setAttribute('d', balloonPath);
                                            balloonGroup.appendChild(shape);
                                            balloonGroup.setAttribute('style', 'visibility:visible');
                                            balloonGroup.setAttribute('style', 'cursor: pointer');
                                            balloonGroup.setAttribute('class', 'clusterGroup');
                                            balloonGroup.setAttribute('transform', 'translate( ' + tempX + ' ' + tempY + ' )');
                                            break;
                                        case 'Image':
                                            var locationX = 0;
                                            var locationY = 0;
                                            var x = -(locationX + (markerCluster.Width / 2));
                                            var y = -(locationY + (markerCluster.Height / 2));
                                            shape = this.createClusterShape('image', clusterID, null, null, 'translate( ' + tempX + ' ' + tempY + ' )', true, indexCollection, markerCluster);
                                            shape.setAttributeNS(null, 'height', markerCluster.Height);
                                            shape.setAttributeNS(null, 'width', markerCluster.Width);
                                            shape.setAttributeNS('http://www.w3.org/1999/xlink', 'href', markerCluster.ImageUrl);
                                            shape.setAttributeNS(null, 'x', x);
                                            shape.setAttributeNS(null, 'y', y);
                                            break;
                                        case 'Cross':
                                        case 'Diamond':
                                        case 'Star':
                                        case 'Triangle':
                                        case 'HorizontalLine':
                                        case 'VerticalLine':
                                        case 'InvertedTriangle':
                                        case 'Pentagon':
                                            var path = '';
                                            path = calculateShape(markerCluster.Shape, { height: markerCluster.Height, width: markerCluster.Width }, { x: 0, y: 0 });
                                            shape = this.createClusterShape('path', clusterID, markerCluster.Fill, markerCluster.Opacity, 'translate( ' + tempX + ' ' + tempY + ' )', false, indexCollection, markerCluster);
                                            shape.setAttribute('d', path);
                                            if (markerCluster.Shape === 'Cross' || markerCluster.Shape === 'HorizontalLine' || markerCluster.Shape === 'VerticalLine') {
                                                shape.setAttribute('stroke', markerCluster.Fill);
                                                shape.setAttribute('stroke-width', ((markerCluster.Width + markerCluster.Height) / 4).toString());
                                            }
                                            break;
                                    }
                                    shape.setAttribute('class', 'clusterGroup');
                                    var textElement = document.createElementNS('http://www.w3.org/2000/svg', 'text');
                                    textElement.setAttribute('id', labelID);
                                    textElement.setAttribute('x', '0');
                                    textElement.setAttribute('y', postionY.toString());
                                    textElement.setAttribute('fill', markerCluster.LabelColor);
                                    textElement.setAttribute('font-size', markerCluster.LabelSize);
                                    textElement.setAttribute('font-style', markerCluster.LabelFontStyle);
                                    textElement.setAttribute('font-family', markerCluster.LabelFontFamily);
                                    textElement.setAttribute('font-weight', markerCluster.LabelFontWeight);
                                    textElement.setAttribute('text-anchor', 'middle');
                                    textElement.setAttribute('transform', 'translate( ' + tempX + ' ' + tempY + ' )');
                                    textElement.setAttribute('opacity', markerCluster.LabelOpacity);
                                    textElement.setAttribute('style', 'cursor: pointer');
                                    textElement.setAttribute('style', 'visibility: visible');
                                    textElement.setAttribute('class', 'clusterGroup');
                                    textElement.innerHTML = (colloideBounds.length + 1).toString();
                                    clusterGroup.appendChild(textElement);
                                    clusterGroup.setAttribute('class', 'clusterGroup');
                                    if (markerCluster.Shape !== 'Balloon') {
                                        clusterGroup.appendChild(shape);
                                    }
                                    else {
                                        clusterGroup.appendChild(balloonGroup);
                                    }
                                    markerTemplate.children[o].setAttribute('style', 'visibility:hidden; content:marker');
                                }
                                colloideBounds = [];
                            }
                        }
                    }
                }
            }
            if (clusterGroup.childElementCount > 0) {
                var layerElement = document.getElementById(id + '_LayerIndex_' + layerIndex);
                layerElement.appendChild(clusterGroup);
                markerCollection = document.getElementById(id + '_LayerIndex_' + layerIndex + '_MarkerGroup');
                for (var o = 0; o < clusterGroup.childElementCount; o++) {
                    if (clusterGroup.childNodes[o]['style']['visibility'] !== 'hidden') {
                        tempElement = clusterGroup.childNodes[o];
                        bounds1 = tempElement.getBoundingClientRect();
                        if (bounds1 !== null && !(tempElement.id.indexOf('_datalabel_') > -1)) {
                            for (var p = o + 1; p < clusterGroup.childElementCount; p++) {
                                if (clusterGroup.childNodes[p]['style']['visibility'] !== 'hidden') {
                                    var tempElement1 = clusterGroup.children[p];
                                    bounds2 = tempElement1.getBoundingClientRect();
                                    if (bounds2 !== null && !(tempElement1.id.indexOf('_datalabel_') > -1)) {
                                        if (!(bounds1.left > bounds2.right || bounds1.right < bounds2.left
                                            || bounds1.top > bounds2.bottom || bounds1.bottom < bounds2.top)) {
                                            clusterColloideBounds.push(tempElement1);
                                            clusterColloideBounds.push(clusterGroup.childNodes[p - 1]);
                                            clusterGroup.childNodes[p]['style']['visibility'] = 'hidden';
                                            clusterGroup.childNodes[p - 1]['style']['visibility'] = 'hidden';
                                            indexCollection.push(p);
                                        }
                                    }
                                }
                            }
                            if (clusterColloideBounds.length > 0) {
                                tempElement = clusterGroup.childNodes[o];
                                for (var i = 0; i < clusterColloideBounds.length; i++) {
                                    if (tempElement.tagName === 'g') {
                                        tempElement.childNodes[0].textContent = tempElement.childNodes[0].textContent + ',' +
                                            clusterColloideBounds[i].textContent;
                                    }
                                    else {
                                        tempElement.textContent = tempElement.textContent + ',' + clusterColloideBounds[i].textContent;
                                    }
                                    clusterGroup.childNodes[o - 1].textContent = ((+(clusterGroup.childNodes[o - 1].textContent)) +
                                        (+(clusterColloideBounds[i + 1].textContent))).toString();
                                    i++;
                                }
                            }
                            clusterColloideBounds = [];
                        }
                    }
                }
                while (0 < clusterGroup.childNodes.length) {
                    markerCollection.insertBefore(clusterGroup.childNodes[0], markerCollection.firstChild);
                }
                layerElement = document.getElementById(id + '_LayerIndex_' + layerIndex);
                layerElement.appendChild(markerCollection);
                layerElement.removeChild(clusterGroup);
            }
        }
    };
    SfMaps.prototype.createClusterShape = function (shapeType, id, fill, opacity, transform, isClass, indexCollection, border) {
        var shape = document.createElementNS('http://www.w3.org/2000/svg', shapeType);
        shape.setAttribute('id', id);
        shape.setAttribute('fill', fill);
        shape.setAttribute('stroke', border['BorderColor']);
        shape.setAttribute('stroke-width', border['BorderWidth']);
        shape.setAttribute('stroke-dasharray', border['DashArray']);
        shape.setAttribute('fill-opacity', opacity);
        shape.setAttribute('stroke-opacity', border['BorderOpacity']);
        shape.setAttribute('transform', transform);
        shape.setAttribute('style', 'visibility:visible');
        shape.setAttribute('style', 'cursor: pointer');
        if (!isClass) {
            shape.setAttribute(null, 'class', 'clusterGroup');
        }
        else {
            shape.setAttribute('class', 'clusterGroup');
        }
        shape.innerHTML = indexCollection.toString();
        return shape;
    };
    SfMaps.prototype.click = function (event) {
        var id = event.target['id'];
        var clientValue = this.getMousePosition(event.pageX, event.pageY);
        var parentId = id.split('_')[0];
        if (id.indexOf('_Left_Page_Rect') > -1 || id.indexOf('_Right_Page_Rect') > -1) {
            this.dotNetRef.invokeMethodAsync('TriggerLegendPaging', id.indexOf('_Left_Page_Rect') > -1 ? 1 : 0);
        }
        if ((id.indexOf('shapeIndex') > -1 || id.indexOf('Tile') > -1 || this.markerClusterExpandCheck) && this.sameMarkerData) {
            this.mergeSeparateCluster(this.sameMarkerData, this.id);
            this.markerClusterExpandCheck = false;
        }
        if (id.indexOf('_Zooming_') > -1 && this.options.enableZoom) {
            var factor = 1;
            if (id.indexOf('_ZoomIn_') > -1) {
                var zoomType = id.split('_Zooming_ToolBar_')[1].split('_')[0];
                var eventType = event.type;
                factor = this.factorCount === 0 ? this.options.factor + 1 : this.scaleFactor + 1;
                this.factorCount++;
                if (factor >= 1) {
                    this.scaleFactor = factor;
                    this.removeCluster();
                    this.dotNetRef.invokeMethodAsync('TriggerZoom', zoomType, eventType);
                }
            }
            if (id.indexOf('_ZoomOut_') > -1) {
                var zoomType = id.split('_Zooming_ToolBar_')[1].split('_')[0];
                var eventType = event.type;
                factor = this.factorCount === 0 ? this.options.factor - 1 : this.scaleFactor - 1;
                this.factorCount++;
                if (factor >= 1) {
                    this.scaleFactor = factor;
                    this.removeCluster();
                    this.dotNetRef.invokeMethodAsync('TriggerZoom', zoomType, eventType);
                }
            }
            if (id.indexOf('_Reset_') > -1) {
                this.zoomClick = false;
                this.handleToolbarSelection('_Zooming_ToolBar_Pan_Path', '_Zooming_ToolBar_Zoom_Path');
                var zoomType = id.split('_Zooming_ToolBar_')[1].split('_')[0];
                var eventType = event.type;
                this.removeCluster();
                this.dotNetRef.invokeMethodAsync('TriggerZoom', zoomType, eventType);
            }
            if (id.indexOf('_Zoom_') > -1) {
                if (document.getElementById(this.id + '_Zooming_ToolBar_Zoom_Rect').getAttribute('stroke-opacity') !== '0.3') {
                    this.zoomClick = true;
                    this.handleToolbarSelection('_Zooming_ToolBar_Zoom_Path', '_Zooming_ToolBar_Pan_Path');
                    this.allowPanning = false;
                    this.enableSelectionZoom = true;
                }
            }
            if (id.indexOf('_Pan_') > -1) {
                if (document.getElementById(this.id + '_Zooming_ToolBar_Pan_Rect').getAttribute('stroke-opacity') !== '0.3') {
                    this.zoomClick = false;
                    this.handleToolbarSelection('_Zooming_ToolBar_Pan_Path', '_Zooming_ToolBar_Zoom_Path');
                    this.allowPanning = false;
                    this.enableSelectionZoom = false;
                    this.isPan = true;
                }
            }
        }
        if (id.indexOf('_LayerIndex_') > -1 && id.indexOf('_MarkerIndex_') > -1) {
            var layerIndex = parseInt(id.split('_LayerIndex_')[1].split('_')[0], 10);
            var markerIndex = parseInt(id.split('_MarkerIndex_')[1].split('_')[0], 10);
            var dataIndex = parseInt(id.split('_dataIndex_')[1].split('_')[0], 10);
            var element = document.getElementById(id);
            var dataElement = void 0;
            if (this.markerClusterCollection[layerIndex].Shape === 'Balloon') {
                if (!sf.base.isNullOrUndefined(element.firstElementChild) && element.firstElementChild.innerHTML.length > 0) {
                    dataElement = element.firstElementChild.innerHTML.split(',');
                }
            }
            else {
                if (!sf.base.isNullOrUndefined(element) && element.innerHTML.length > 0) {
                    dataElement = element.innerHTML.split(',');
                }
            }
            this.removeAllHighlight();
            this.dotNetRef.invokeMethodAsync('TriggerMarkerClick', layerIndex, markerIndex, dataIndex, id, clientValue.x, clientValue.y, dataElement, id.indexOf('cluster') > -1 ? 'cluster' : 'marker', this.isTouch);
        }
        if (id.indexOf('shapeIndex') > -1 && this.options.tooltipDisplayMode === 'Click') {
            var layerIndex = parseInt(id.split('_LayerIndex_')[1].split('_')[0], 10);
            var shapeIndex = parseInt(id.split('_shapeIndex_')[1].split('_')[0], 10);
            this.dotNetRef.invokeMethodAsync('TriggerShapeTooltip', id, clientValue.x, clientValue.y, layerIndex, shapeIndex, 'Shape', 0);
        }
        if (id.indexOf('_LayerIndex_') > -1 && id.indexOf('BubbleIndex') > -1) {
            var layerIndex = parseInt(id.split('_LayerIndex_')[1].split('_')[0], 10);
            var markerIndex = parseInt(id.split('_BubbleIndex_')[1].split('_')[0], 10);
            var dataIndex = parseInt(id.split('_dataIndex_')[1].split('_')[0], 10);
            var element = document.getElementById(id);
            this.dotNetRef.invokeMethodAsync('TriggerBubbleClick', layerIndex, markerIndex, dataIndex, id, clientValue.x, clientValue.y);
        }
        if (id.indexOf('_Legend_') > -1) {
            this.removeAllHighlight();
            this.dotNetRef.invokeMethodAsync('TriggerLegendClick', parseInt(id.split('_Index_')[1].split('_')[0], 10));
        }
        if (id.indexOf('_shapeIndex_') === -1 && id.indexOf('_MarkerIndex_') === -1 && id.indexOf('_BubbleIndex_') === -1 &&
            this.options.tooltipDisplayMode === 'Click') {
            this.dotNetRef.invokeMethodAsync('TriggerShapeTooltip', '', clientValue.x, clientValue.y, 0, 0, '', 0);
        }
    };
    SfMaps.prototype.handleToolbarSelection = function (selectId, deSelectId) {
        var selectionElement = document.getElementById(this.element.id + selectId);
        if (selectionElement) {
            selectionElement.setAttribute('stroke', this.toolbarSelection);
            selectionElement.setAttribute('fill', this.toolbarSelection);
        }
        var deselectionElement = document.getElementById(this.element.id + deSelectId);
        if (deselectionElement) {
            deselectionElement.setAttribute('stroke', this.toolbarColor);
            deselectionElement.setAttribute('fill', this.toolbarColor);
        }
    };
    SfMaps.prototype.removeCluster = function () {
        var samecluster = document.querySelectorAll('[class="clusterGroup"]');
        if (samecluster.length > 0) {
            for (var i = 0; i < samecluster.length; i++) {
                document.getElementById(samecluster[i].id).remove();
            }
        }
        var visibleMarker = document.querySelectorAll('[style="visibility:hidden; content:marker"]');
        for (var i = 0; i < visibleMarker.length; i++) {
            var visibilityChange = document.getElementById(visibleMarker[i].id);
            visibilityChange.setAttribute('style', 'visibility: visible;');
        }
    };
    SfMaps.prototype.createImageUrl = function (element, type) {
        var svgData = '<svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink">' +
            element.outerHTML + '</svg>';
        return window.URL.createObjectURL(new Blob(type === 'SVG' ? [svgData] : [(new XMLSerializer()).serializeToString(element)], { type: 'image/svg+xml' }));
    };
    SfMaps.prototype.imageExport = function (type, fileName, allowDownload) {
        return __awaiter(this, void 0, void 0, function () {
            var proxy;
            return __generator(this, function (_a) {
                proxy = this;
                return [2 /*return*/, new Promise(function (resolve, reject) {
                        var imageCanvasElement = document.createElement('canvas');
                        var elementBound = proxy.element.getBoundingClientRect();
                        imageCanvasElement.width = elementBound.width;
                        imageCanvasElement.height = elementBound.height;
                        var element = document.getElementById(proxy.element.id + '_svg');
                        var url = proxy.createImageUrl(element, type);
                        var tileElementCount = document.getElementById(proxy.element.id + '_animated_tiles').childNodes.length;
                        var context = imageCanvasElement.getContext('2d');
                        var titleElement = document.getElementById(proxy.element.id + '_Map_title');
                        var borderElement = document.getElementById(proxy.element.id + '_MapBorder');
                        if (!sf.base.isNullOrUndefined(borderElement)) {
                            context.fillStyle = document.getElementById(proxy.element.id + '_MapBorder').getAttribute('fill');
                            context.fillStyle = context.fillStyle === 'transparent' ? '#ffffff' : context.fillStyle;
                            context.fillStyle = proxy.options.background ? proxy.options.background : '#ffffff';
                        }
                        if (!sf.base.isNullOrUndefined(titleElement)) {
                            context.font = titleElement.getAttribute('font-size') + ' Arial';
                            context.fillStyle = titleElement.getAttribute('fill');
                            context.fillText(titleElement.textContent, parseFloat(titleElement.getAttribute('x')), parseFloat(titleElement.getAttribute('y')));
                        }
                        context.save();
                        var svgParent = document.getElementById(proxy.element.id + '_Tile_SVG_Parent');
                        context.rect(parseFloat(svgParent.style.left), parseFloat(svgParent.style.top), parseFloat(svgParent.style.width), parseFloat(svgParent.style.height));
                        context.clip();
                        context.fillRect(0, 0, elementBound.width, elementBound.height);
                        proxy.exportedCount = 0;
                        var promises = [];
                        for (var i = 0; i < tileElementCount; i++) {
                            var tile = document.getElementById(proxy.element.id + 'tile' + i);
                            var exportTileImg = new Image();
                            exportTileImg.crossOrigin = 'Anonymous';
                            promises.push(proxy.renderImages(imageCanvasElement, tile, context, exportTileImg, tileElementCount, type, fileName, url, proxy, allowDownload));
                            exportTileImg.src = tile.children[0].getAttribute('src');
                        }
                        Promise.all(promises).then(function (values) {
                            for (var i = 0; i < values.length; i++) {
                                if (values[i] != null) {
                                    resolve(values[i]);
                                    break;
                                }
                            }
                        });
                    })];
            });
        });
    };
    SfMaps.prototype.renderImages = function (canvasElement, tile, context, exportTileImg, tileElementCount, type, fileName, url, proxy, allowDownload) {
        return new Promise(function (resolve, reject) {
            exportTileImg.onload = function () {
                proxy.exportedCount++;
                context.setTransform(1, 0, 0, 1, parseFloat(tile.style.left) + 10, parseFloat(tile.style.top) +
                    (parseFloat(document.getElementById(proxy.element.id + '_tile_parent').style.top)));
                context.drawImage(exportTileImg, 0, 0);
                if (proxy.exportedCount === tileElementCount) {
                    var svgParent_1 = document.getElementById(proxy.element.id + '_Tile_SVG_Parent');
                    url = proxy.createImageUrl(svgParent_1.childNodes[0], type);
                    var image_1 = new Image();
                    image_1.onload = function () {
                        context.setTransform(1, 0, 0, 1, parseFloat(svgParent_1.style.left), parseFloat(svgParent_1.style.top));
                        context.drawImage(image_1, 0, 0);
                        if (allowDownload) {
                            proxy.triggerDownload(type, fileName, canvasElement.toDataURL('image/png').replace('image/png', 'image/octet-stream'));
                            resolve(null);
                        }
                        else {
                            var base64String = (type === 'JPEG') ? canvasElement.toDataURL('image/jpeg') : (type === 'PNG') ? canvasElement.toDataURL('image/png') : '';
                            resolve(base64String);
                        }
                    };
                    image_1.src = url;
                }
                else {
                    resolve(null);
                }
            };
        });
    };
    SfMaps.prototype.triggerDownload = function (type, fileName, url) {
        var anchorElement = document.createElement('a');
        anchorElement.download = fileName + '.' + type.toLocaleLowerCase();
        anchorElement.href = url;
        anchorElement.click();
    };
    SfMaps.prototype.getMousePosition = function (pageX, pageY) {
        var elementRect = this.element.getBoundingClientRect();
        var pageXOffset = this.element.ownerDocument.defaultView.pageXOffset;
        var pageYOffset = this.element.ownerDocument.defaultView.pageYOffset;
        var clientTop = this.element.ownerDocument.documentElement.clientTop;
        var clientLeft = this.element.ownerDocument.documentElement.clientLeft;
        var positionX = elementRect.left + pageXOffset - clientLeft;
        var positionY = elementRect.top + pageYOffset - clientTop;
        return new MapLocation((pageX - positionX), (pageY - positionY));
    };
    SfMaps.prototype.getBound = function (id) {
        var element = document.getElementById(id);
        return { x: element.offsetLeft, y: element.offsetTop };
    };
    SfMaps.prototype.createToolbarStyle = function (color, parentToolbar) {
        var zoomPath = document.getElementById(this.element.id + '_Zooming_ToolBar_Zoom_Path');
        if (!sf.base.isNullOrUndefined(zoomPath)) {
            zoomPath.setAttribute('stroke', this.toolbarColor);
        }
        this.zoomClick = false;
        var zoomStyle = document.getElementById(this.element + '_zoomToolbar_style');
        if (zoomStyle) {
            zoomStyle.parentNode.removeChild(zoomStyle);
        }
        var css = '.e-maps-toolbar:hover { opacity:1; } .e-maps-toolbar:hover > circle { stroke:' + color +
            '; } .e-maps-toolbar:hover > path { fill: ' + color + ' ;  stroke: ' + color + '; }' +
            '.e-maps-toolbar:hover { cursor: pointer; } .e-maps-cursor-disable:hover { cursor: not-allowed; } .e-maps-panning:hover { cursor: pointer; } ' +
            '.e-maps-popup-close { display: block; opacity: 0; }';
        var style = document.createElement('style');
        style.id = this.element + '_zoomToolbar_style';
        style.appendChild(document.createTextNode(css));
        parentToolbar.appendChild(style);
    };
    SfMaps.prototype.render = function () {
        this.wireEvents();
    };
    return SfMaps;
}());
var Maps = {
    getMarker: function (target, options, dotnetRef, element) {
        if (element && element.blazor__instance) {
            element.blazor__instance.clusterExpand(target, options, dotnetRef);
        }
    },
    markerCluster: function (element) {
        if (element && element.blazor__instance) {
            var isClusterCleared = false;
            for (var i = 0; i < element.blazor__instance.layerCount; i++) {
                if (!isClusterCleared) {
                    element.blazor__instance.removeCluster();
                    isClusterCleared = true;
                }
                element.blazor__instance.clusterMarkers(i);
            }
        }
    },
    itemHighlight: function (fill, opacity, layerIndex, settingIndex, type, element) {
        if (element && element.blazor__instance) {
            element.blazor__instance.itemHighlightHandle(fill, opacity, layerIndex, settingIndex, type);
        }
    },
    shapeHighlight: function (fill, opacity, layerIndex, element) {
        if (element && element.blazor__instance) {
            element.blazor__instance.shapeHighlightHandle(fill, opacity, layerIndex);
        }
    },
    panDirection: function (xDiff, yDiff, x, y, scale, element) {
        if (element && element.blazor__instance) {
            element.blazor__instance.panning(xDiff, yDiff, x, y, scale);
        }
    },
    initialize: function (element, options, dotnetRef) {
        var instance = new SfMaps(element.id, element, options, dotnetRef);
        instance.render();
        return this.getElementSize(element);
    },
    getElementSize: function (element) {
        var elementWidth;
        var elementHeight;
        var x;
        var y;
        if (element !== null) {
            var elementRect = element.getBoundingClientRect();
            elementWidth = elementRect.width;
            elementHeight = elementRect.height;
            x = elementRect['x'];
            y = elementRect['y'];
        }
        return { width: elementWidth, height: elementHeight, isIE: sf.base.Browser.isIE, x: x, y: y };
    },
    getElementBounds: function (id) {
        var htmlElement = document.getElementById(id);
        if (htmlElement) {
            var bounds = htmlElement.getBoundingClientRect();
            return {
                width: bounds.width, height: bounds.height, top: bounds.top, bottom: bounds.bottom,
                left: bounds.left, right: bounds.right
            };
        }
        return null;
    },
    getBound: function (id, dotNetRef) {
        var element = document.getElementById(id);
        return { x: element.offsetLeft, y: element.offsetTop };
    },
    getBoundData: function (id) {
        var element = document.getElementById(id);
        return { x: element.getBoundingClientRect()['x'], y: element.getBoundingClientRect()['y'] };
    },
    getTileZoom: function (id, left, top, url, height, width, index, dotNetRef) {
        return __awaiter(this, void 0, void 0, function () {
            var element, scale, image, layerElement, i, j, k, layerIndex, markerIndex, dataIndex, point;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        element = document.getElementById(id + '_animated_tiles');
                        scale = parseInt(element['className'], 10);
                        element.children[index].setAttribute('style', 'top:' + '' + top + '; height:' + '' + height + ';width:' + '' + width + '; left:' + '' + left + '; position:absolute;');
                        element.children[index].innerHTML = '';
                        image = document.createElement('img');
                        image.src = url;
                        element.children[index].appendChild(image);
                        layerElement = document.getElementById(id + '_LayerCollections');
                        i = 0;
                        _a.label = 1;
                    case 1:
                        if (!(i < layerElement.children.length)) return [3 /*break*/, 8];
                        j = 0;
                        _a.label = 2;
                    case 2:
                        if (!(j < layerElement.children[i].childElementCount)) return [3 /*break*/, 7];
                        if (!(layerElement.children[i].children[j].id.indexOf('MarkerGroup') > -1)) return [3 /*break*/, 6];
                        k = 0;
                        _a.label = 3;
                    case 3:
                        if (!(k < layerElement.children[i].children[j].childElementCount)) return [3 /*break*/, 6];
                        layerIndex = parseInt(layerElement.children[i].children[j].children[k].id.split('_LayerIndex_')[1].split('_')[0], 10);
                        markerIndex = parseInt(layerElement.children[i].children[j].children[k].id.split('_MarkerIndex_')[1].split('_')[0], 10);
                        dataIndex = parseInt(layerElement.children[i].children[j].children[k].id.split('_dataIndex_')[1].split('_')[0], 10);
                        return [4 /*yield*/, dotNetRef.invokeMethodAsync('ZoomMarker', layerIndex, markerIndex, dataIndex, 0, 0, scale, dotNetRef)];
                    case 4:
                        point = _a.sent();
                        layerElement.children[i].children[j].children[k].setAttribute('transform', 'translate(' + point[0] + ',' + point[1] + ')');
                        _a.label = 5;
                    case 5:
                        k++;
                        return [3 /*break*/, 3];
                    case 6:
                        j++;
                        return [3 /*break*/, 2];
                    case 7:
                        i++;
                        return [3 /*break*/, 1];
                    case 8: return [2 /*return*/, index];
                }
            });
        });
    },
    getLayer: function (id, dotNetRef) {
        var element = document.getElementById(id + '_LayerCollections');
        var bound = element.getBoundingClientRect();
        return { Bottom: bound.bottom, Height: bound.height, Left: bound.left, Right: bound.right, Top: bound.top, Width: bound.width, X: bound['x'], Y: bound['y'] };
    },
    getElement: function (id, dotNetRef) {
        var element = document.getElementById(id);
        var bound = element.getBoundingClientRect();
        return { Bottom: bound.bottom, Height: bound.height, Left: bound.left, Right: bound.right, Top: bound.top, Width: bound.width, X: bound['x'], Y: bound['y'] };
    },
    getSvg: function (id, dotNetRef) {
        var element = document.getElementById(id + '_svg');
        var bound = element.getBoundingClientRect();
        return { Bottom: bound.bottom, Height: bound.height, Left: bound.left, Right: bound.right, Top: bound.top, Width: bound.width, X: bound['x'], Y: bound['y'] };
    },
    updateTileTranslatePoint: function (tileTranslatePoint, translatePoint, areaRectX, areaRectY, areaRectWidth, areaRectHeight, marginLeft, marginTop, tileZoomLevel, element) {
        if (element && element.blazor__instance) {
            element.blazor__instance.tileTranslatePoint = { x: tileTranslatePoint.x, y: tileTranslatePoint.y };
            element.blazor__instance.translatePoint = { x: translatePoint.x, y: translatePoint.y };
            element.blazor__instance.areaRect = { x: areaRectX, y: areaRectY, width: areaRectWidth, height: areaRectHeight };
            element.blazor__instance.marginLeft = marginLeft;
            element.blazor__instance.marginTop = marginTop;
            element.blazor__instance.tileZoomLevel = element.blazor__instance.options.factor = element.blazor__instance.scaleFactor = tileZoomLevel;
            if ((tileZoomLevel === element.blazor__instance.options.maxZoom || tileZoomLevel > 1 || tileZoomLevel === 1) && element.blazor__instance.previousId === "") {
                element.blazor__instance.toolbarOpacity(element.blazor__instance.toolbarButtonOpacity, element.blazor__instance.toolbarShapeOpacity);
                if (tileZoomLevel === element.blazor__instance.options.maxZoom) {
                    if (document.getElementById(element.blazor__instance.id + '_Zooming_ToolBar_Zoom_Rect')) {
                        document.getElementById(element.blazor__instance.id + '_Zooming_ToolBar_Zoom_Path').setAttribute('fill', element.blazor__instance.toolbarColor);
                        document.getElementById(element.blazor__instance.id + '_Zooming_ToolBar_Zoom_Path').setAttribute('stroke', element.blazor__instance.toolbarColor);
                        element.blazor__instance.isPan = false;
                    }
                }
            }
            if (element.blazor__instance.isTileMap && element.blazor__instance.previousId.indexOf("_Reset_") > -1) {
                if (!element.blazor__instance.options.enablePanning) {
                    if (document.getElementById(element.blazor__instance.id + '_Zooming_ToolBar_Pan_Rect')) {
                        document.getElementById(element.blazor__instance.id + '_Zooming_ToolBar_Pan_Path').setAttribute('fill', element.blazor__instance.toolbarColor);
                        document.getElementById(element.blazor__instance.id + '_Zooming_ToolBar_Pan_Path').setAttribute('stroke', element.blazor__instance.toolbarColor);
                    }
                }
                element.blazor__instance.enableSelectionZoom = element.blazor__instance.enableSelectionZoom ? false : element.blazor__instance.enableSelectionZoom;
            }
            element.blazor__instance.isPan = element.blazor__instance.previousId.indexOf("_Reset_") > -1 && element.blazor__instance.options.enablePanning ? true : element.blazor__instance.isPan;
        }
    },
    updateTranslatePoint: function (translatePoint, areaRectX, areaRectY, areaRectWidth, areaRectHeight, marginLeft, marginTop, zoomFactor, element) {
        if (element && element.blazor__instance) {
            element.blazor__instance.shapeTranslatePoint = translatePoint;
            element.blazor__instance.areaRect = { x: areaRectX, y: areaRectY, width: areaRectWidth, height: areaRectHeight };
            element.blazor__instance.marginLeft = marginLeft;
            element.blazor__instance.marginTop = marginTop;
            element.blazor__instance.options.factor = element.blazor__instance.scaleFactor = zoomFactor;
            if (element.blazor__instance.previousId.indexOf("_Reset_") > -1) {
                if (document.getElementById(element.blazor__instance.id + '_Zooming_ToolBar_Pan_Rect')) {
                    if (element.blazor__instance.options.minZoom === 1) {
                        document.getElementById(element.blazor__instance.id + '_Zooming_ToolBar_Pan_Path').setAttribute('fill', element.blazor__instance.toolbarColor);
                        document.getElementById(element.blazor__instance.id + '_Zooming_ToolBar_Pan_Path').setAttribute('stroke', element.blazor__instance.toolbarColor);
                    }
                    element.blazor__instance.enableSelectionZoom = false;
                    element.blazor__instance.isPan = true;
                }
            }
            if (zoomFactor === element.blazor__instance.options.maxZoom && element.blazor__instance.enableSelectionZoom) {
                element.blazor__instance.isPan = false;
            }
            if ((zoomFactor === element.blazor__instance.options.maxZoom || zoomFactor > 1 || zoomFactor === 1) && element.blazor__instance.previousId === "") {
                element.blazor__instance.toolbarOpacity(element.blazor__instance.toolbarButtonOpacity, element.blazor__instance.toolbarShapeOpacity);
                if (zoomFactor === element.blazor__instance.options.maxZoom) {
                    if (document.getElementById(element.blazor__instance.id + '_Zooming_ToolBar_Zoom_Rect')) {
                        document.getElementById(element.blazor__instance.id + '_Zooming_ToolBar_Zoom_Path').setAttribute('fill', element.blazor__instance.toolbarColor);
                        document.getElementById(element.blazor__instance.id + '_Zooming_ToolBar_Zoom_Path').setAttribute('stroke', element.blazor__instance.toolbarColor);
                    }
                }
            }
        }
    },
    updateMapBound: function (element, mapBound) {
        if (element && element.blazor__instance) {
            element.blazor__instance.baseMapBounds = mapBound;
        }
    },
    updateZoomSettings: function (element, zoomSettings) {
        if (element && element.blazor__instance) {
            element.blazor__instance.options.enableZoom = zoomSettings.enable;
            element.blazor__instance.options.zoomOnClick = zoomSettings.zoomOnClick;
            element.blazor__instance.options.doubleClickZoom = zoomSettings.doubleClickZoom;
            element.blazor__instance.options.enablePanning = zoomSettings.enablePanning;
            element.blazor__instance.options.enablePinchZooming = zoomSettings.pinchZooming;
            element.blazor__instance.options.enableSelectionZooming = zoomSettings.enableSelectionZooming;
            element.blazor__instance.options.enableMouseWheelZoom = zoomSettings.mouseWheelZoom;
        }
    },
    updateCollection: function (element, collection) {
        if (element && element.blazor__instance) {
            element.blazor__instance.bubbleCollection = JSON.parse(collection.bubble);
            element.blazor__instance.labelCollection = JSON.parse(collection.dataLabel);
            element.blazor__instance.dataLabelTemplateCollection = JSON.parse(collection.dataLabelTemplate);
            element.blazor__instance.markerSettings = JSON.parse(collection.markerSettings);
            element.blazor__instance.markerClusterCollection = JSON.parse(collection.markerCluster);
            element.blazor__instance.baseMapBounds = {
                latitudeMax: collection.latitudeMax, latitudeMin: collection.latitudeMin, longitudeMax: collection.longitudeMax, longitudeMin: collection.longitudeMin,
                minBounds: collection.minBounds, maxBounds: collection.maxBounds, availableSize: collection.availableSize
            };
            element.blazor__instance.isTileMap = collection.isTileMap;
            element.blazor__instance.legendType = collection.legendSettings;
            element.blazor__instance.urlTemplate = collection.urlTemplate;
            element.blazor__instance.key = collection.key;
            element.blazor__instance.dataSource = JSON.parse(collection.markerDataSource);
            element.blazor__instance.toolbarSelection = collection.toolbarSelection;
            element.blazor__instance.toolbarColor = collection.toolbarColor;
            element.blazor__instance.mapsBorderWidth = collection.mapsBorder;
            element.blazor__instance.toolbarButtonOpacity = collection.toolbarButtonOpacity;
            element.blazor__instance.toolbarShapeOpacity = collection.toolbarShapeOpacity;
        }
    },
    updateLayerHighlight: function (index, setting, isBorder, element) {
        if (element && element.blazor__instance) {
            if (!isBorder) {
                element.blazor__instance.options.layerHighlightSettings[index].Fill = setting.fill;
                element.blazor__instance.options.layerHighlightSettings[index].Opacity = setting.opacity;
                element.blazor__instance.options.layerHighlightSettings[index].Enable = setting.enable;
            }
            else {
                element.blazor__instance.options.layerHighlightSettings[index].BorderColor = setting.borderColor;
                element.blazor__instance.options.layerHighlightSettings[index].BorderWidth = setting.borderWidth;
                element.blazor__instance.options.layerHighlightSettings[index].BorderOpacity = setting.borderOpacity;
            }
        }
    },
    updateMarkerHighlight: function (layerIndex, index, setting, isBorder, element) {
        if (element && element.blazor__instance) {
            if (!isBorder) {
                element.blazor__instance.options.markerHighlightSettings[layerIndex][index].Fill = setting.fill;
                element.blazor__instance.options.markerHighlightSettings[layerIndex][index].Opacity = setting.opacity;
                element.blazor__instance.options.markerHighlightSettings[layerIndex][index].Enable = setting.enable;
            }
            else {
                element.blazor__instance.options.markerHighlightSettings[layerIndex][index].BorderColor = setting.borderColor;
                element.blazor__instance.options.markerHighlightSettings[layerIndex][index].BorderWidth = setting.borderWidth;
                element.blazor__instance.options.markerHighlightSettings[layerIndex][index].BorderOpacity = setting.borderOpacity;
            }
        }
    },
    updateBubbleHighlight: function (layerIndex, index, setting, isBorder, element) {
        if (element && element.blazor__instance) {
            if (!isBorder) {
                element.blazor__instance.options.bubbleHighlightSettings[layerIndex][index].Fill = setting.fill;
                element.blazor__instance.options.bubbleHighlightSettings[layerIndex][index].Opacity = setting.opacity;
                element.blazor__instance.options.bubbleHighlightSettings[layerIndex][index].Enable = setting.enable;
            }
            else {
                element.blazor__instance.options.bubbleHighlightSettings[layerIndex][index].BorderColor = setting.borderColor;
                element.blazor__instance.options.bubbleHighlightSettings[layerIndex][index].BorderWidth = setting.borderWidth;
                element.blazor__instance.options.bubbleHighlightSettings[layerIndex][index].BorderOpacity = setting.borderOpacity;
            }
        }
    },
    updateTooltipDisplayMode: function (element, displayMode) {
        if (element && element.blazor__instance) {
            element.blazor__instance.options.tooltipDisplayMode = displayMode;
        }
    },
    updateNavigationHighlight: function (layerIndex, index, setting, isBorder, element) {
        if (element && element.blazor__instance) {
            if (!isBorder) {
                element.blazor__instance.options.navigationHighlightSettings[layerIndex][index].Fill = setting.fill;
                element.blazor__instance.options.navigationHighlightSettings[layerIndex][index].Opacity = setting.opacity;
                element.blazor__instance.options.navigationHighlightSettings[layerIndex][index].Enable = setting.enable;
            }
            else {
                element.blazor__instance.options.navigationHighlightSettings[layerIndex][index].BorderColor = setting.borderColor;
                element.blazor__instance.options.navigationHighlightSettings[layerIndex][index].BorderWidth = setting.borderWidth;
                element.blazor__instance.options.navigationHighlightSettings[layerIndex][index].BorderOpacity = setting.borderOpacity;
            }
        }
    },
    exportToImage: function (type, fileName, allowDownload, element) {
        return __awaiter(this, void 0, void 0, function () {
            var returnValue;
            var _this = this;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        if (!(element && element.blazor__instance)) return [3 /*break*/, 2];
                        return [4 /*yield*/, element.blazor__instance.imageExport(type, fileName, allowDownload)];
                    case 1:
                        returnValue = _a.sent();
                        _a.label = 2;
                    case 2:
                        if (!(returnValue instanceof Promise)) return [3 /*break*/, 4];
                        return [4 /*yield*/, returnValue.then(function (data) { return __awaiter(_this, void 0, void 0, function () {
                                return __generator(this, function (_a) {
                                    return [2 /*return*/, data];
                                });
                            }); })];
                    case 3:
                        _a.sent();
                        return [3 /*break*/, 5];
                    case 4: return [2 /*return*/, returnValue];
                    case 5: return [2 /*return*/];
                }
            });
        });
    },
    getLegendRect: function (id, top) {
        var legendId = document.getElementById(id);
        var legendRect = legendId.getBoundingClientRect();
        var mapId = id.split('_Legend_')[0];
        var svgId = document.getElementById(mapId);
        var svgRect = svgId.getBoundingClientRect();
        return { x: legendRect.left - svgRect.left, y: legendRect.top - svgRect.top, width: legendRect.width, height: legendRect.height };
    },
    markerAnimation: function (id, marker, dotNetRef, element) {
        if (element && element.blazor__instance) {
            element.blazor__instance.markerAnimation(id, marker, dotNetRef);
        }
    },
    layerAnimation: function (id, point, scale, currentPoint, currentScale, dotNetRef, element) {
        if (element && element.blazor__instance && id !== element.id) {
            var layerGroup = document.getElementById(id + '_LayerIndex_0');
            for (var i = 0; i < layerGroup.children.length; i++) {
                if (layerGroup.children[i].getAttribute('transform') !== null) {
                    this.zoomAnimation(layerGroup.children[i], 0, 1000, currentPoint, currentScale, point, scale, dotNetRef);
                }
            }
        }
    },
    slope: function (previousLocation, point) {
        if (previousLocation.x === point.x) {
            return null;
        }
        return (point.y - previousLocation.y) / (point.x - previousLocation.x);
    },
    interception: function (point, slopeValue) {
        if (slopeValue === null) {
            return point.x;
        }
        return point.y - slopeValue * point.x;
    },
    zoomAnimation: function (element, delay, duration, currentPoint, scale, previousLocation, preScale, dotNetRef) {
        var _this = this;
        var delta = 0;
        var point = { x: currentPoint.x, y: currentPoint.y };
        var diffScale = scale - preScale;
        var currentLocation = { x: 0, y: 0 };
        var currentScale = 1;
        if (scale === preScale) {
            element.setAttribute('transform', 'scale( ' + (scale) + ' ) translate( ' + point.x + ' ' + point.y + ' )');
            return;
        }
        var slopeFactor = this.slope(previousLocation, point);
        var slopeIntersection = this.interception(previousLocation, slopeFactor);
        var horizontalDifference = point.x - previousLocation.x;
        var verticalDifference = point.y - previousLocation.y;
        new sf.base.Animation({}).animate(element, {
            duration: duration,
            delay: delay,
            progress: function (args) {
                if (args.timeStamp > args.delay) {
                    element.style.visibility = 'visible';
                    delta = ((args.timeStamp - args.delay) / args.duration);
                    currentScale = preScale + (delta * diffScale);
                    currentLocation.x = previousLocation.x + (delta * horizontalDifference) / (currentScale / scale);
                    if (!sf.base.isNullOrUndefined(slopeFactor)) {
                        currentLocation.y = previousLocation.y + (delta * verticalDifference);
                    }
                    else {
                        currentLocation.y = ((slopeFactor * currentLocation.x) + slopeIntersection);
                    }
                    element.setAttribute('transform', 'scale( ' + currentScale + ' ) ' +
                        'translate( ' + currentLocation.x + ' ' + currentLocation.y + ' )');
                }
            },
            end: function (model) {
                element.setAttribute('transform', 'scale( ' + (scale) + ' ) translate( ' + point.x + ' ' + point.y + ' )');
                _this.dotNetRef.invokeMethodAsync('TriggerAnimation', element);
            }
        });
    },
    bubbleAnimation: function (id, bubble, dotNetRef, element) {
        if (element && element.blazor__instance) {
            element.blazor__instance.bubbleAnimation(id, bubble, dotNetRef);
        }
    },
    getToolbarAlign: function (id, data, element) {
        var verticalAlignment = data.verticalAlignment;
        var horizontalAlignment = data.horizontalAlignment;
        var zoomToolbar = document.getElementById(id + '_Zooming_KitCollection');
        var toolBarSize = zoomToolbar.getBoundingClientRect();
        var x = 0;
        var y = 0;
        switch (verticalAlignment) {
            case 0:
                y = data.y;
                break;
            case 1:
                y = (data.height / 2 - toolBarSize.height / 2) + data.mapsBorder;
                break;
            case 2:
                y = data.height - toolBarSize.height + data.mapsBorder - (2 * data.buttonPadding);
                break;
        }
        switch (horizontalAlignment) {
            case 0:
                x = data.x;
                break;
            case 1:
                x = (data.width / 2 - toolBarSize.width / 2) + data.mapsBorder;
                break;
            case 2:
                x = data.width - toolBarSize.width - (2 * data.buttonPadding) + data.mapsBorder;
                break;
        }
        var parentToolbar = document.getElementById(id + '_ToolBar');
        parentToolbar.style.left = x + 'px';
        parentToolbar.style.top = y + 'px';
        parentToolbar.style.visibility = 'visible';
        var color = data.color;
        if (element && element.blazor__instance) {
            element.blazor__instance.createToolbarStyle(color, parentToolbar);
        }
    },
    getTemplate: function (data, template, top, left, id, index, templateCheck, parentId) {
        var dataValue = data[0];
        var properties = Object.keys(dataValue);
        var templateHtml = template;
        for (var i = 0; i < properties.length; i++) {
            if (properties[i].toLowerCase() !== 'latitude' && properties[i].toLowerCase() !== 'longitude') {
                templateHtml = templateHtml.replace(new RegExp('{{:' + properties[i] + '}}', 'g'), dataValue[properties[i].toString()]);
            }
        }
        var markerTemplate = document.createElementNS('http://www.w3.org/2000/svg', 'div');
        markerTemplate.setAttribute('id', id);
        markerTemplate.style.position = 'absolute';
        markerTemplate.style.transform = 'translate(-50 %, -50 %)';
        markerTemplate.style.left = left.toString();
        markerTemplate.style.top = top.toString();
        markerTemplate.innerHTML = templateHtml.toString();
        var secondTemplateGroup = document.getElementById(parentId + '_LayerIndex_0_Markers_Template_Group');
        secondTemplateGroup.appendChild(markerTemplate);
        var markerTemplateGroup = document.getElementById(parentId + '_Secondary_Element');
        markerTemplateGroup.appendChild(secondTemplateGroup);
        var templateId = document.getElementById(parentId);
        templateId.appendChild(markerTemplateGroup);
        return templateHtml;
    }
};

return Maps;

}());
