function roleValidation() {
    if ($("#roleName").val().length == 0) {
        $("#roleNameValidation").text("this Role Name Filed is required");
    }
    else {
        $("#roleNameValidation").text("");
    }
}

$(document).on("change", "#roleName", function () {
    roleValidation();
})

function typeValidation() {
    if ($("#type").val().length == 0) {
        $("#accountTypeValidation").text("this Account Type Filed is required");
    }
    else {
        $("#accountTypeValidation").text("");
    }
}

$(document).on("change", "#type", function () {
    typeValidation();
    selectedMenus = [];
    if ($(this).val().length != 0) {
        $.ajax({
            url: "/Admin/ChangeMenusByRole",
            type: "Get",
            contentType: "application/json",
            data: {
                roleId: $(this).val(),
            },
            success: function (response) {
                $("#menuValidation").text("");
                $(".checkBoxs").html(response);
            }
        })
    }
})

var selectedMenus = [];

function changeCheckBox(doc) {
    if ($(doc).is(":checked")) {
        selectedMenus.push($(doc).attr("value"));
        $("#menuValidation").text("");
    }
    else {
        var index = selectedMenus.indexOf($(doc).attr("value"));
        if (index > -1) {
            selectedMenus.splice(index, 1);
        }
    } 
}

$(document).on("click", "#saveButton", function () {
    if ((selectedMenus.length != 0 || $("#type").val() == 1) && $("#roleName").val().length != 0 && $("#type").val().length != 0) {
        var data = JSON.stringify({
            RoleName: $("#roleName").val(),
            SlectedAccountType: $("#type").val(),
            SelectedMenus: selectedMenus,
        });
        $.ajax({
            url: "/Admin/CreateRoles",
            type: "Get",
            contentType: "application/json",
            data: {
                data: data,
            },
            success: function (response) {
                window.location.href = response.redirect;
            }
        })
    }
    else if (selectedMenus.length == 0 && $("#type").val() != 1) {
        $("#menuValidation").text("Select Any One Filed");
    }
    roleValidation();
    typeValidation();
})