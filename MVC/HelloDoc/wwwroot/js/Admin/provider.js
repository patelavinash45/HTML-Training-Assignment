function onnotificationClick(doc) {
    $.ajax({
        url: "/Admin/EditProviderNotification",
        type: "Get",
        contentType: "application/json",
        data: {
            physicanId : $(doc).attr("id"),
            isNotification : $(doc).is(":checked"),
        },
    })
}

$(document).on("change", "#regionFilter", function () {
    $.ajax({
        url: "/Admin/RegionFilter",
        type: "Get",
        contentType: "application/json",
        data: {
            regionId: $(this).val(),
        },
        success: function (response) {
            $(".tableData").html(response);
        }
    })
})