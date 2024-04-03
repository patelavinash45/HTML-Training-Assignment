
$(document).ready(function () {
    if ($("#popup").length) {
        $("#popup").modal('show');
    }
});

function checkEmail(element) {
    var email = $(element).val();
    if (email.length > 0) {
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
}

function checkLocation() {
    var link = "http://maps.google.com/?q=" + $("#address").val();
    link = link.replace(/\s+/g, "");
    window.open(link, "_blank");
}





