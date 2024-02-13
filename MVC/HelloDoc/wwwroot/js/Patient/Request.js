
function alert_close(element) {
    element.parentNode.parentNode.style.display = 'none';
    document.getElementById("main-body").style.opacity = 1;
}
/*<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.7.1/jquery.min.js"></script>*/
$('#email2').focusout(function () {
    console.log("hi");
    var email = $(this).val();
    $.ajax({
        url: '/Patient/CheckEmailExists',
        type: 'GET',
        contentType: 'application/json',
        data: { email: email },
        success: function (response) {
            if (response.emailExists) {
                $('#input-password').hide();
            } else {
                $('#input-password').show();
            }
        }
    });
});


