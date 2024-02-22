function sidebar() {
    var temp = document.getElementById("side-bar").style.display;
    if (temp == "none") {
        document.getElementById("side-bar").style.display = "block";
    } else {
        document.getElementById("side-bar").style.display = "none";
    }
}

function changeTable(temp) {
    $(".tables").css("display", "none");
    $(".optionButton").css('box-shadow', 'none');
    switch (temp) {
        case 1: $("#new").css("display", "block"); $("#newOption").css('box-shadow', '10px 10px 5px #AAA'); $("#newText").css("display", "block");
                  getTableData("New"); break;
        case 2: $("#pending").css("display", "block"); $("#pendingOption").css('box-shadow', '10px 10px 5px #AAA'); $("#pendingText").css("display", "block");
                  getTableData("Pending"); break;
        case 3: $("#active").css("display", "block"); $("#activeOption").css('box-shadow', '10px 10px 5px #AAA'); getTableData("Active");
                  $("#activeText").css("display", "block"); break;
        case 4: $("#conclude").css("display", "block"); $("#concludeOption").css('box-shadow', '10px 10px 5px #AAA'); getTableData("Conclude");
                  $("#concludeText").css("display", "block"); break;
        case 5: $("#close").css("display", "block"); $("#tocloseOption").css('box-shadow', '10px 10px 5px #AAA'); getTableData("Close");
                  $("#closeText").css("display", "block"); break;
        case 6: $("#unpaid").css("display", "block"); $("#unpaidOption").css('box-shadow', '10px 10px 5px #AAA'); getTableData("Unpaid");
                  $("#unpaidText").css("display", "block");
    }
}

function getTableData(temp) {

    $.ajax({
        url: '/Admin/GetTablesData',
        type: 'GET',
        contentType: 'application/json',
        data: { status: temp },
        success: function (response) {
            setTableData(temp, response);
        }
    })
}

function setTableData(temp, response) {
    switch (temp) {
        case "New": $("#new").html(response); break;
        case "Pending": $("#pending").html(response); break;
        case "Active": $("#active").html(response); break;
        case "Conclude": $("#conclude").html(response); break;
        case "Close": $("#close").html(response); break;
        case "Unpaid": $("#unpaid").html(response); break;
        default: return View();
    }
}