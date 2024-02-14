
function alert_close(element) {
    element.parentNode.parentNode.style.display = 'none';
    document.getElementById("main-body").style.opacity = 1;
}

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


