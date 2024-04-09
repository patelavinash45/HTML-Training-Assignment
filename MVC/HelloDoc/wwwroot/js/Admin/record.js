
///  For Record Page

$(document).on("click", ".searchButton", function () {
    getRecordData();
})

$(document).on("click", ".resetRecords", function () {
    $("#filterForm").trigger("reset");
    getRecordData();
})

function getRecordData() {
    $.ajax({
        url: "/Admin/GetRecordsTableDate",
        type: "Post",
        data: $("#filterForm").serializeArray(),
        success: function (response) {
            $(".tableData").html(response);
        }
    })
}


// for Email log Page

$(document).on("click", ".searchButtonEmailLogs", function () {
    getEmailLogs();
})

$(document).on("click", ".resetEmaillog", function () {
    $("#filterFormEmailLogs").trigger("reset");
    getEmailLogs();
})

function getEmailLogs() {
    $.ajax({
        url: "/Admin/GetEmailLogsTableDate",
        type: "Post",
        data: $("#filterFormEmailLogs").serializeArray(),
        success: function (response) {
            $(".tableData").html(response);
        }
    })
}


/// for Sms log Page

$(document).on("click", ".searchButtonSMSLogs", function () {
    getEmailLogs();
})

$(document).on("click", ".resetSMSLogs", function () {
    $("#filterFormSMSLogs").trigger("reset");
    getEmailLogs();
})

function getEmailLogs() {
    $.ajax({
        url: "/Admin/GetSMSLogsTableDate",
        type: "Post",
        data: $("#filterFormSMSLogs").serializeArray(),
        success: function (response) {
            $(".tableData").html(response);
        }
    })
}