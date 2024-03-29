const partialViewNames = ["", "_dayWiseScheduling", "_weekWiseScheduling", "_monthWiseScheduling"]
var currentType = 1;
function chnagetab(type) {
    $(".tabButtons").css("background-color", "white");
    $(".tabButtons").css("color", "cadetblue");
    $("#display").text("");
    $("#displayWeek").text("");
    currentType = type;
    switch (type) {
        case 1: $("#dayButton").css("background-color", "cadetblue");
                $("#dayButton").css("color", "white");
                setDate(new Date());
                break;
        case 2: $("#weekButton").css("background-color", "cadetblue");
                $("#weekButton").css("color", "white");
                setWeek(new Date(),true);
                break;
        case 3: $("#monthButton").css("background-color", "cadetblue");
                $("#monthButton").css("color", "white");
                setMonth(new Date());
                break;
    }
    getData();
}

function getData() {
    $.ajax({
        url: "/Admin/ChangeTab",
        type: "Get",
        contentType: "application/json",
        data: {
            name: partialViewNames[currentType],
            regionId: $("#regionsList").val(),
        },
        success: function (response) {
            $(".tab").html(response);
        }
    })
}

$(document).on("change", "#regionsList", function () {
    getData();
})

$(document).ready(function () {
    var date = new Date;
    setDate(date);
})

$(document).on("click", "#previous", function () {
    switch (currentType) {
        case 1: var date = new Date(Date.parse($("#display").text()));
                date.setDate(date.getDate() - 1);
                setDate(date);
                break;
        case 2: var date = new Date(Date.parse($("#display").text()));
                date.setDate(date.getDate() - 7);
                setWeek(date,false);
                break;
        case 3: var date = new Date(Date.parse($("#display").text()));
                date.setDate(1);
                date.setMonth(date.getMonth() - 1);
                setMonth(date);
    }
})

$(document).on("click", "#next", function () {
    switch (currentType) {
        case 1: var date = new Date(Date.parse($("#display").text()));
                date.setDate(date.getDate() + 1);
                setDate(date);
                break;
        case 2: var date = new Date(Date.parse($("#display").text()));
                date.setDate(date.getDate() + 7);
                setWeek(date, false);
                break;
        case 3: var date = new Date(Date.parse($("#display").text()));
                date.setDate(1);
                date.setMonth(date.getMonth() + 1);
                setMonth(date);
    }
})

function setDate(date) {
    const formatter = new Intl.DateTimeFormat('en-US', {
        weekday: 'long',
        month: 'short',
        day: 'numeric',
        year: 'numeric'
    });
    $("#display").text(formatter.format(date));
}

function setWeek(date, temp) {
    var formatter = new Intl.DateTimeFormat('en-US', {
        month: 'short',
        day: 'numeric',
        year: 'numeric'
    });
    if (temp) {
        $("#display").text(formatter.format(date.setDate(date.getDate() - date.getDay())));
        $("#displayWeek").text(" - " + formatter.format(date.setDate((6 - date.getDay()) + date.getDate())));
    }
    else {
        $("#display").text(formatter.format(date));
        $("#displayWeek").text(" - " + formatter.format(date.setDate(date.getDate() + 6)));
    }
}

function setMonth(date) {
    var formatter = new Intl.DateTimeFormat('en-US', {
        month: 'short',
        year: 'numeric'
    });
    $("#display").text(formatter.format(date));
}
