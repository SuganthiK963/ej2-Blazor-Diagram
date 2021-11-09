window.sfBlazor = window.sfBlazor || {};

window.sfBlazor.import = function (url) {
    if (typeof url === 'string' && url.startsWith('./')) {
        url = document.baseURI + url.substr(2);
    }
    new Promise(function (resolve, reject) {
        var script = document.createElement('script');
        script.src = url;
        script.async = true;
        script.onload = function () {
            resolve(script);
        }
        script.onerror = function () {
            reject(script);
        }
        document.body.appendChild(script);
    });
}
