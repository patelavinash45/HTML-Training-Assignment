const partialViewNames = ["", "_dayWiseScheduling", "_weekWiseScheduling", "_monthWiseScheduling"]
const weekDays = ["sun", "mon", "tue", "wed", "thu", "fir", "sat"]
var currentType = 1;
var dayWise, weekWise, monthWise;

async function chnagetab(type) {
    $(".tabButtons").removeClass("active");
    $("#displayWeek").text("");
    currentType = type;
    await getData();
    switch (type) {
        case 1: $("#dayButton").addClass("active");
                dayWise = new Date();
                setDate();
                break;
        case 2: $("#weekButton").addClass("active");
                weekWise = new Date();
                setWeek();
                break;
        case 3: $("#monthButton").addClass("active");
                monthWise = new Date();
                monthWise.setDate(1);
                setMonth();
                break;
    }
}

async function getData() {
    await $.ajax({
            url: "/Admin/ChangeTab",
            type: "Get",
            contentType: "application/json",
            async: true,
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
    dayWise = new Date;
    setDate(dayWise);
})

$(document).on("click", "#previous", async function () {
    await getData();
    switch (currentType) {
        case 1: dayWise.setDate(dayWise.getDate() - 1);
                setDate();
                break;
        case 2: weekWise.setDate(weekWise.getDate() - 7);
                setWeek();
                break;
        case 3: monthWise.setMonth(monthWise.getMonth() - 1);
                setMonth();
    }
})

$(document).on("click", "#next", async function () {
    await getData();
    switch (currentType) {
        case 1: dayWise.setDate(dayWise.getDate() + 1);
                setDate();
                break;
        case 2: weekWise.setDate(weekWise.getDate() + 7);
                setWeek();
                break;
        case 3: monthWise.setMonth(monthWise.getMonth() + 1);
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
    $("#display").text(formatter.format(date.setDate(date.getDate() - date.getDay())));
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
