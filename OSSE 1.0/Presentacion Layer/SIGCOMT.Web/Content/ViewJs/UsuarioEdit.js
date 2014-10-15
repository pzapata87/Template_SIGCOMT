jQuery(document).ready(function() {
    $('#frmUsuario').on('submit', function (e) {

        if (!$(this).valid()) {
            e.preventDefault();
            return;
        }

        var form = Utils.GetForm(this);

        $.ajax({
            url: form.url,
            type: form.type,
            data: form.data,
            success: function (response) {
                if (response.Success == true) {
                    jQuery.gritter.add({
                        title: 'USUARIO',
                        text: response.Message,
                        class_name: 'growl-success',
                        //image: 'images/screen.png',
                        sticky: false,
                        time: ''
                    });

                    //setTimeout(function() {
                    //    window.location = "";
                    //}, 3000);

                } else {
                    jQuery.gritter.add({
                        title: 'USUARIO',
                        text: response.Message,
                        class_name: 'growl-danger',
                        //image: 'images/screen.png',
                        sticky: false,
                        time: ''
                    });
                }
            },
            error: function (x, xh, xhr) {
                $('#loading').modal("hide");
                console.error(xh);
            }
        });

        e.preventDefault();
    });
});