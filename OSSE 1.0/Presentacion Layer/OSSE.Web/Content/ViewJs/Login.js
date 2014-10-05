jQuery(document).ready(function () {
    //jQuery("#btnLogOn").on("click", function (evt) {
    //    evt.preventDefault();
        
    //    var formulario = $("#frmLogOn");

    //    if (formulario.validate().form()) {
    //        formulario.submit();
    //    } else {
    //        alert("error");
    //    }
    //});
    
    jQuery("#frmLogOn").validate({
        errorLabelContainer: jQuery("#frmLogOn div.valSumModel")
    });
});