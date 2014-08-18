
//Funcion que permite validar que se haya seleccionado como mínimo la cantidad de items respectiva.
//válida únicamente para elementos multiselect
function CargarValidacionCantidadMinimaSeleccionados(selector) {
    if ($.validator && $.validator.unobtrusive) {
        $.validator.unobtrusive.adapters.addSingleVal("countelements", "minelements");
        $.validator.addMethod("countelements", function(value, element, minElements) {
            if ($(selector).multiselect("widget").find("input:checked").length < minElements)
                return false;
            return true;
        });
    }
}