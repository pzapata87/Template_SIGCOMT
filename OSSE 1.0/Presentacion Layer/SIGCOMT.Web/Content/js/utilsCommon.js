Utils = {
    Grilla: function (opciones) {
        if (opciones.dom == null)
            opciones.dom = '<"left"f><"right"l>rtip';
        
        if (opciones.responsive == null)
            opciones.responsive = true;
        
        if (opciones.processing == null)
            opciones.processing = true;
        
        if (opciones.serverSide == null)
            opciones.serverSide = true;
        
        if (opciones.order == null)
            opciones.order = [[1, "desc"]];
        
        if (opciones.lengthMenu == null)
            opciones.lengthMenu = [[5, 25, 50, -1], [5, 25, 50, "Todos"]];

        var grilla = jQuery(opciones.grilla).dataTable({
            "dom": opciones.dom,
            "responsive": opciones.responsive,
            "processing": opciones.processing,
            "serverSide": opciones.serverSide,
            "language": {
                "url": opciones.languageUrl
            },
            "order": opciones.order,
            "lengthMenu": opciones.lengthMenu,
            "ajax": {
                "url": opciones.url,
                "type": "POST"
            },
            "columns": opciones.columns,
            "fnInitComplete": function() {
                // Permite reemplazar el evento de filtro por defecto del "keypress" por "enter"
                grilla.fnFilterOnReturn();
            }
        });
    }
}