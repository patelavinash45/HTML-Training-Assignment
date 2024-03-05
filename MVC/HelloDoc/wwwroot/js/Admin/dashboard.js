var statusStrings = ["", "New", "Pending", "Active", "Conclude", "Close", "Unpaid"];

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
    $(".searchPatient").val("");
    $(".searchRegion").val("");
    $('.tableRow').css("display", "table-row");
    $('.buttonHr').css("display", "none");
    switch (temp) {
        case 1: $("#new").css("display", "block"); $("#newOption").css('box-shadow', '10px 10px 5px #AAA'); $("#newText").css("display", "block");
                  getTableData("New",1); break;
        case 2: $("#pending").css("display", "block"); $("#pendingOption").css('box-shadow', '10px 10px 5px #AAA'); $("#pendingText").css("display", "block");
                  getTableData("Pending",1); break;
        case 3: $("#active").css("display", "block"); $("#activeOption").css('box-shadow', '10px 10px 5px #AAA'); getTableData("Active",1);
                  $("#activeText").css("display", "block"); break;
        case 4: $("#conclude").css("display", "block"); $("#concludeOption").css('box-shadow', '10px 10px 5px #AAA'); getTableData("Conclude",1);
                  $("#concludeText").css("display", "block"); break;
        case 5: $("#close").css("display", "block"); $("#tocloseOption").css('box-shadow', '10px 10px 5px #AAA'); getTableData("Close",1);
                  $("#closeText").css("display", "block"); break;
        case 6: $("#unpaid").css("display", "block"); $("#unpaidOption").css('box-shadow', '10px 10px 5px #AAA'); getTableData("Unpaid",1);
                  $("#unpaidText").css("display", "block");
    }
}

function getTableData(status,pageNo) {
    $.ajax({
        url: '/Admin/GetTablesData',
        type: 'GET',
        contentType: 'application/json',
        data: {
            pageNo : pageNo,
            status: status,
        },
        success: function (response) {
            setTableData(status, response);
        }
    })
}

function setTableData(status, response) {
    switch (status) {
        case "New": $("#new").html(response); break;
        case "Pending": $("#pending").html(response); break;
        case "Active": $("#active").html(response); break;
        case "Conclude": $("#conclude").html(response); break;
        case "Close": $("#close").html(response); break;
        case "Unpaid": $("#unpaid").html(response); break;
        default: return View();
    }
}

function tableSearch(document) {
    //$('table.myTable').DataTable().search($(document).val()).draw();
}

function regionSearch(document) {
    //$('table.myTable').DataTable().column(0).search($(document).val()).draw();
}

function filterOnButton(status) {
    /*$("body").css("filter", "invert(1)");*/
    $('.tableRows').css("display", "none");
    $('.buttonHr').css("display", "none");
    switch (status) {
        case 1: $('.bg-success').css("display", "table-row"); $('#hr-1').css("display", "block"); break;
        case 2: $('.bg-warning').css("display", "table-row"); $('#hr-2').css("display", "block"); break;
        case 3: $('.bg-danger').css("display", "table-row"); $('#hr-3').css("display", "block"); break;
        case 4: $('.bg-primary').css("display", "table-row"); $('#hr-4').css("display", "block"); break;
        case 5: $('.bg-info').css("display", "table-row"); $('#hr-5').css("display", "block"); break;
        default: $('.tableRow').css("display", "table-row");
    }
}


///

function getNextPageData(currentPageNo, totalRequestCount, status) {
    var totalPages = (totalRequestCount / 10) + 1;
    if (currentPageNo < totalRequestCount) {
        getTableData(statusStrings[status], currentPageNo + 1);
    }
}

function getPreviousPageData(currentPageNo, status) {
    if (currentPageNo > 1) {
        getTableData(statusStrings[status], currentPageNo - 1);
    }
}

function navigateToFirstPage(status) {
    getTableData(statusStrings[status], 1);
}

function navigateToLastPage(status, totalRequestCount) {
    var lastPageNo = parseInt(totalRequestCount / 10, 10) + 1;
    getTableData(statusStrings[status], lastPageNo);
}
