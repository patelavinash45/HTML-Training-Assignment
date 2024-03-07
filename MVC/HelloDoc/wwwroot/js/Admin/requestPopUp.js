function changeSelect() {
    var id = ".region-" + $('#selectRegion').val();
    $('.physicianOptions').css("display", "none");
    $(id).css("display", "block");
    $('.physician').val("").change();
}

function displayPopUp(id) {
    var idName = "#name-" + id;
    $(".patientRequestId").val(id);
    $(".patientName").text($(idName).text());
}