function changeSelect() {
    var id = ".item-" + $('#selectedProfession').val();
    $('.bussinessOptions').css("display", "none");
    $(id).css("display", "block");
    $('#selectedBusiness').val("").change();
}

function changeBussiness() {
    var venderId = $('#selectedBusiness').val();
    if (venderId != null) {
        $.ajax({
            url: "/Admin/GetBussinessData",
            type: 'Get',
            contentType: 'appliaction/json',
            data: {
                venderId: venderId,
            },
            success: function (response) {
                var valus = Object.values(response);
                $("#contact").val(valus[15]).change();
                $("#email").val(valus[14]).change();
                $("#faxNumber").val(valus[3]).change();
            }
        })
    }
    else {
        $("#contact").val("").change();
        $("#email").val("").change();
        $("#faxNumber").val("").change();
    }
}