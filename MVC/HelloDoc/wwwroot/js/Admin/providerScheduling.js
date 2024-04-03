const partialViewNames = ["", "_dayWiseScheduling", "_weekWiseScheduling", "_monthWiseScheduling"]
const weekDays = ["sun", "mon", "tue", "wed", "thu", "fir", "sat"]
var currentType = 1;
var dayWise, weekWise, monthWise;

async function chnagetab(type) {
    $(".tabButtons").removeClass("active");
    $("#displayWeek").text("");
    currentType = type;
    switch (type) {
        case 1: $("#dayButton").addClass("active");
                dayWise = new Date();
                await getData();
                setDate();
                break;
        case 2: $("#weekButton").addClass("active");
                weekWise = new Date();
                weekWise.setDate(weekWise.getDate() - weekWise.getDay())
                await getData();
                setWeek();
                break;
        case 3: $("#monthButton").addClass("active");
                monthWise = new Date();
                monthWise.setDate(1);
                await getData();
                setMonth();
                break;
    }
}

async function getData() {
    const formatter = new Intl.DateTimeFormat('en-US', {
        weekday: 'long',
        month: 'short',
        day: 'numeric',
        year: 'numeric'
    });
    var time;
    switch (currentType) {
        case 1: time = dayWise; break;
        case 2: time = weekWise; break;
        case 3: time = monthWise;
    }
    time = formatter.format(time);
    await $.ajax({
            url: "/Admin/ChangeTab",
            type: "Get",
            contentType: "application/json",
            async: true,
            data: {
                name: partialViewNames[currentType],
                regionId: $("#regionsList").val(),
                type: currentType,
                time: time,
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
    dayWise = new Date;
    setDate(dayWise);
})

$(document).on("click", "#previous", async function () {
    switch (currentType) {
        case 1: dayWise.setDate(dayWise.getDate() - 1);
                await getData();
                setDate();
                break;
        case 2: weekWise.setDate(weekWise.getDate() - 7);
                await getData();
                setWeek();
                break;
        case 3: monthWise.setMonth(monthWise.getMonth() - 1);
                await getData();
                setMonth();
    }
})

$(document).on("click", "#next", async function () {
    switch (currentType) {
        case 1: dayWise.setDate(dayWise.getDate() + 1);
                await getData();
                setDate();
                break;
        case 2: weekWise.setDate(weekWise.getDate() + 7);
                await getData();
                setWeek();
                break;
        case 3: monthWise.setMonth(monthWise.getMonth() + 1);
                await getData();
                setMonth();
    }
})

function setDate() {
    const formatter = new Intl.DateTimeFormat('en-US', {
        weekday: 'long',
        month: 'short',
        day: 'numeric',
        year: 'numeric'
    });
    $("#display").text(formatter.format(dayWise));
}

function setWeek() {
    var date = new Date(weekWise);
    var formatter = new Intl.DateTimeFormat('en-US', {
        month: 'short',
        day: 'numeric',
        year: 'numeric'
    });
    $("#display").text(formatter.format(date));
    $("#displayWeek").text(" - " + formatter.format(date.setDate((6 - date.getDay()) + date.getDate())));
    for (i = 6; i >= 0; i--) {
        $(`#${weekDays[i]}`).text(date.getDate());
        date.setDate(date.getDate() - 1);
    }
}

function setMonth() {
    var date = new Date(monthWise);
    var formatter = new Intl.DateTimeFormat('en-US', {
        month: 'short',
        year: 'numeric'
    });
    $("#display").text(formatter.format(date));
    var totalDays = new Date(date.getFullYear(), date.getMonth() + 1, 0).getDate();
    var temp = 0;
    for (i = 0; i < totalDays; i++)
    {
        var x = date.getDay();
        if (x == 0 && i!=0) {
            temp++;
        }
        var y = weekDays[x] + "-" + temp;
        $(`#${y}`).text(i + 1);
        $(`#${y}`).removeClass("bg-white");
        date.setDate(date.getDate() + 1);
    }
    if (temp != 5) {
        $(`.row-5`).css("display", "none");
    }
}


/////   popup - create shift

$(document).on("change", "#isRepeat", function () {
    if ($(this).is(":checked")) {
        $("#repeatFields").prop("disabled", false);
    }
    else {
        $("#repeatEnd").val("");
        $(".weekDay").prop("checked",false);
        $("#repeatFields").prop("disabled", true);
        $("#repeatEndValidation").text("");
    }
});


$(document).on("change", "#regions", function () {
    if ($(this).val().length != 0) {
        $.ajax({
            url: "/Admin/GetPhysicians",
            type: "Get",
            contentType: "application/json",
            data: {
                regionId: $(this).val(),
            },
            success: function (response) {
                $("#physician").html('<option disabled selected value="">Physicians</option>');
                $.each(response, function (index, item) {
                    var option = "<option value=" + index + ">" + item + "</option>";
                    $("#physician").append(option);
                });
            }
        });
    }
})


$(document).on("change", ".weekDay", function () {
    if (!$(this).is(":checked")) {
        $("#repeatValidation").text("Week Day is required");
    }
    else {
        $("#repeatValidation").text("");
    }
})

$(document).on("submit", "#createShiftForm", function (e)
{
    if ($("#regions").val().length == 0 || $("#endTime").val().length == 0 || $("#startTime").val().length == 0 || $("#shiftDate").val().length == 0
        || $("#physician").val().length == 0 ) {
        e.preventDefault();
    }
    if ($("#isRepeat").is(":checked")) {
        if (!$(".weekDay").is(":checked") || $("#repeatEnd").val().length == 0) {
            e.preventDefault();
            if (!$(".weekDay").is(":checked")) {
                $("#repeatValidation").text("Week Day is required");
            }
            else {
                $("#repeatValidation").text("");
            }
        }
    }
})
