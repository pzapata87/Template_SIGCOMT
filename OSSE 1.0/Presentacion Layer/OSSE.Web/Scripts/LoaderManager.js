var idAnimacion;
var loader;

function IniciarAnimacion(container, width, height) {

    if (width == null)
        width = 50;

    if (height == null)
        height = 50;

    $(container).children("#topLoader").remove();
    $(container).append('<div id="topLoader" style="display:none; width: 80px;position: absolute;left:45%;top:40%;"></div>');

    loader = $('#topLoader').percentageLoader({
        width: width,
        height: height,
        controllable: true,
        progress: 0,
        value: ''
    });

    loader.setProgress(0);

    var kb = 0;
    var totalKb = 100;
    loader.show();

    var animateFunc = function () {
        kb += 1;
        loader.setProgress(kb / totalKb);

        if (kb < totalKb) {
            idAnimacion = setTimeout(animateFunc, 50);
        } else {
            kb = 0;
            idAnimacion = setTimeout(animateFunc, 50);
        }
    };

    var detenerFunc = function() {
        loader.setProgress(0);
        loader.remove();
        clearInterval(idAnimacion);
    };

    idAnimacion = setTimeout(animateFunc, 25);

    return detenerFunc;
}
