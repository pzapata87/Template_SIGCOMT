jQuery(document).ready(function() {
    $('#frmLogin').on('submit', function(e) {

        if (!$(this).valid()) {
            e.preventDefault();
        }

        return true;
    });
});