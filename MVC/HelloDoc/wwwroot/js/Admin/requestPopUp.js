function changeSelect(document) {
    $.ajax({
        url: "/Admin/GetPhysicians",
        type: "Get",
        contentType: "application/json",
        data: {
            regionId: $(document).val(),
        },
        success: function (response) {
            $(".physician").children().remove();
            $(".physician").append('<option disabled selected value="">Physicians</option>');
            $.each(response, function (index, item) {
                var option = "<option value=" + index + ">"+item+"</option>";
                $(".physician").append(option);
            });
        }
    });
}

function displayPopUp(id) {
    var idName = "#name-" + id;
    $(".patientRequestId").val(id);
    $(".patientName").text($(idName).text());
}

function clearCase() {
    $.ajax({
        url: "/Admin/ClearPopUp",
        type: "Get",
        async: false,
        contentType: "application/json",
        data: {
            requestId: $(".patientRequestId").val(),
        },
        success: function (response) {
            window.location.href = response.redirect;
        }
    })
}


////

function navigatToViewCase(requestId) {
    navigation("ViewCase", requestId);
}

function navigatToViewNotes(requestId) {
    navigation("ViewNotes", requestId);
}

function navigatToViewDocuments(requestId) {
    navigation("ViewDocument", requestId);
}

function navigatToSendOrder(requestId) {
    navigation("SendOrder", requestId);
}


function navigation(actionName, requestId) {
    $.ajax({
        url: "/Admin/SetRequestId",
        type: "Get",
        contentType: "application/json",
        data: {
            requestId: requestId,
            actionName: actionName,
        },
        success: function (response) {
            window.location.href = response.redirect;
        }
    })
}