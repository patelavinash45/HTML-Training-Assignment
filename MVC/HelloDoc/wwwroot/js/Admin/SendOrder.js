function changeSelect() {
    $.ajax({
        url: "/Admin/GetBussinesses",
        type: "Get",
        contentType: "application/json",
        data: {
            professionId: $('#selectedProfession').val(),
        },
        success: function (response) {
            $("#selectedBusiness").children().remove();
            $("#selectedBusiness").append('<option disabled selected value="">Business</option>');
            $.each(response, function (index, item) {
                var option = "<option value=" + index + ">" + item + "</option>";
                $("#selectedBusiness").append(option);
            });
            $(".contact").val("").change();
            $(".email").val("").change();
            $(".faxNumber").val("").change();
        }
    });
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
                $(".contact").val(valus[15]).change();
                $(".email").val(valus[14]).change();
                $(".faxNumber").val(valus[3]).change();
            }
        })
    }
    else {
        $(".contact").val("").change();
        $(".email").val("").change();
        $(".faxNumber").val("").change();
    }
}