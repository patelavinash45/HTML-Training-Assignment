var requesterTextColor = [ "", "text-danger", "text-success", "text-warning", "text-primary" ];
var requester = ["", "Business", "Patient", "Family", "Concierge"];

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
            $(".physician").append('<option disabled selected value="">Physicians*</option>');
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

function displaySendAgreementPopUp(id) {
    $(".patientRequestId").val(id);
    $.ajax({
        url: "/Admin/GetEmailAndMobileNumber",
        type: "Get",
        async: false,
        contentType: "application/json",
        data: {
            requestId: id,
        },
        success: function (response) {
            $("#patientEmail").val(response.item1);
            $("#patientNumber").val(response.item2);
            $("#requesterIcon").addClass(requesterTextColor[response.item3]);
            $("#requesterName").html(requester[response.item3]);
        }
    })
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

$("#assignForm").submit(function (e) {
    if ($("#assignRequestRegion").val() == null || $("#assignRequestPhysician").val() == null) {
        e.preventDefault();
        $("#assignValidation").css("display", "block");
    }
    else {
        $("#assignValidation").css("display", "none");
    }
})

$("#transferForm").submit(function (e) {
    if ($("#transferRequestRegion").val() == null || $("#transferRequestPhysician").val() == null) {
        e.preventDefault();
        $("#transferValidation").css("display", "block");
    }
    else {
        $("#transferValidation").css("display", "none");
    }
})

$("#cacelForm").submit(function (e) {
    if ($("#cancelRequestRegions").val() == null) {
        e.preventDefault();
        $("#cancelValidation").css("display", "block");
    }
    else {
        $("#cancelValidation").css("display", "none");
    }
})

$("#sendAgreementForm").submit(function (e) {
    if ($("#patientEmail").val().length == 0 || $("#patientNumber").val().length == 0) {
        e.preventDefault();
        $("#sendAgreementValidation").css("display", "block");
    }
    else {
        $("#sendAgreementValidation").css("display", "none");
    }
})

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

function navigatToEncounterForm(requestId) {
    navigation("EncounterForm", requestId);
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