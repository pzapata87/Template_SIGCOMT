jQuery(document).ready(function () {
    $('#frmLogin').on('submit', function () {
        if (!$(this).valid()) {
            return false;
        }
       
        return true;
    });
});