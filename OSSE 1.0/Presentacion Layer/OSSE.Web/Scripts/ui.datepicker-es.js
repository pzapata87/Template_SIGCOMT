jQuery(function($) {
    $.datepicker.regional['es'] =
    {
        clearText: 'Borra',
        clearStatus: 'Borra fecha actual',
        closeText: 'Cerrar',
        closeStatus: 'Cerrar sin guardar',
        prevStatus: 'Mostrar mes anterior',
        prevBigStatus: 'Mostrar año anterior',
        nextStatus: 'Mostrar mes siguiente',
        nextBigStatus: 'Mostrar año siguiente',
        currentText: 'Hoy',
        currentStatus: 'Mostrar mes actual',
        monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
        monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
        monthStatus: 'Seleccionar otro mes',
        yearStatus: 'Seleccionar otro año',
        weekHeader: 'Sm',
        weekStatus: 'Semana del año',
        dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
        dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Jue', 'Vie', 'Sáb'],
        dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sá'],
        dayStatus: 'Set DD as first week day',
        dateStatus: 'Select D, M d',
        dateFormat: 'dd/mm/yy',
        firstDay: 1,
        initStatus: 'Seleccionar fecha',
        isRTL: false
    };

    $.timepicker.regional['es'] = {
        currentText: 'Hoy',
        closeText: 'Seleccionar',
        isRTL: false,
        timeOnlyTitle: 'Hora',
        timeText: 'Hora',
        hourText: 'Hora',
        minuteText: 'Minuto',
        secondText: 'Segundo',
        millisecText: 'Milisegundo',
        timezoneText: 'Zona Horaria',
        timeFormat: 'HH:mm',
        amNames: ['AM', 'A'],
        pmNames: ['PM', 'P']
    };

    $.datepicker.setDefaults($.datepicker.regional['es']);
    $.timepicker.setDefaults($.timepicker.regional['es']);
});