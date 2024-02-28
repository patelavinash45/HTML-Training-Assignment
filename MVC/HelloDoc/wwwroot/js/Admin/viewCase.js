function ProfileEdit(temp) {
    if (temp) {
        $('#edit').css("display", "none");
        $('#submit').css("display", "flex");
        document.getElementById("Fieldset").disabled = false;
    }
    else {
        document.getElementById("Fieldset").disabled = true;
        $('#edit').css("display", "block");
        $('#submit').css("display", "none");
    }
}

function sidebar() {
    var temp = document.getElementById("side-bar").style.display;
    if (temp == "none") {
        document.getElementById("side-bar").style.display = "block";
    } else {
        document.getElementById("side-bar").style.display = "none";
    }
}

function cancelPopUp(id) {
    $("#patientRequestId").val(id);
    $("#patientName").text($('#first-name').val() + ' ' + $('#last-name').val());
}