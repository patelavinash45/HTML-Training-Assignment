function checkEmail(element) {
    var email = $(element).val();
    $.ajax({
        url: '/Patient/CheckEmailExists',
        type: 'GET',
        contentType: 'application/json',
        data: { email: email },
        success: function (response) {
            if (response.emailExists) {
                $('#input-password').hide();
            } else {
                $('#input-password').css("display", "flex");
            }
        }
    });
}

$(document).ready(function () {
    $("#popup").modal('show');
});


