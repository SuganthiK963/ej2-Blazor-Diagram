window.sfBlazor = window.sfBlazor || {};

window.sfBlazor.import = function (url) {
    if (typeof url === 'string' && url.startsWith('./')) {
        url = document.baseURI + url.substr(2);
    }
    return import(/* webpackIgnore: true */ url);
}
