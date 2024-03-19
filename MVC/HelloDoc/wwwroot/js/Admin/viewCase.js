var fname, lanme, birthdate, phone;

function ProfileEdit(temp) {
    if (temp) {
        phone = $("#phone").val();
        fname = $("#firstName").val();
        lname = $("#lastName").val();
        birthdate = $("#birthDate").val();
        $('#edit').css("display", "none");
        $('#submit').css("display", "flex");
        document.getElementById("Fieldset").disabled = false;
    }
    else {
        $("#phone").val(phone);
        $("#firstName").val(fname);
        $("#lastName").val(lname);
        $("#birthDate").val(birthdate);
        $(".validation").css("display", "none");
        $('#edit').css("display", "block");
        $('#submit').css("display", "none");
        document.getElementById("Fieldset").disabled = true;
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
    $(".patientName").text($('#first-name').val() + ' ' + $('#last-name').val());
}

$(document).on("change", "#phone", function () {
    if ($(this).val().length == 0) {
        $("#phoneValidation").css("display", "block");
    }
    else {
        $("#phoneValidation").css("display", "none");
    }
})

$(document).on("change", "#birthDate", function () {
    if ($(this).val().length == 0) {
        $("#birthDateValidation").css("display", "block");
    }
    else {
        $("#birthDateValidation").css("display", "none");
    }
})

$(document).on("change", "#lastName", function () {
    if ($(this).val().length == 0) { 
        $("#lastNameValidation").css("display", "block");
    }
    else {
        $("#lastNameValidation").css("display", "none");
    }
})

$(document).on("change", "#firstName", function () {
    if ($(this).val().length == 0) {
        $("#firstNameValidation").css("display", "block");
    }
    else {
        $("#firstNameValidation").css("display", "none");
    }
})

$(document).on("submit", "#form", function (e) {
    if ($("#phone").val().length == 0 || $("#firstName").val().length == 0 || $("#lastName").val().length == 0 || $("#birthDate").val().length == 0) {
        e.preventDefault();
    }
})