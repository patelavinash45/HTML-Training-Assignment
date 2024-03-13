var statusStrings = ["", "New", "Pending", "Active", "Conclude", "Close", "Unpaid"];
var statusTableStrings = ["", "_NewTable", "_PendingTable", "_ActiveTable", "_ConcludeTable", "_CloseTable", "_UnpaidTable"];
var currentStatus = 1;
var isSearch = false;

function sidebar() {  /// for mobile view silebar
    var temp = document.getElementById("side-bar").style.display;
    if (temp == "none") {
        document.getElementById("side-bar").style.display = "block";
    } else {
        document.getElementById("side-bar").style.display = "none";
    }
}

function changeTable(temp) {      /// change view according to status
    $(".tables").css("display", "none");
    $(".optionButton").css('box-shadow', 'none');
    $(".searchPatient").val("");
    $(".searchRegion").val("");
    $('.tableRow').css("display", "table-row");
    $('.buttonHr').css("display", "none");
    switch (temp) {
        case 1: $("#new").css("display", "block");
                $("#newOption").css('box-shadow', '10px 10px 5px #AAA');
                $("#newText").css("display", "block");
                currentStatus = 1;
                getTableData(1);
                break;
        case 2: $("#pending").css("display", "block");
                $("#pendingOption").css('box-shadow', '10px 10px 5px #AAA');
                $("#pendingText").css("display", "block");
                currentStatus = 2;
                getTableData(1);
                break;
        case 3: $("#active").css("display", "block");
                $("#activeOption").css('box-shadow', '10px 10px 5px #AAA');
                $("#activeText").css("display", "block"); 
                currentStatus = 3;
                getTableData(1);
                break;
        case 4: $("#conclude").css("display", "block");
                $("#concludeOption").css('box-shadow', '10px 10px 5px #AAA');
                $("#concludeText").css("display", "block");
                currentStatus = 4;
                getTableData(1);
                break;
        case 5: $("#close").css("display", "block");
                $("#tocloseOption").css('box-shadow', '10px 10px 5px #AAA');
                $("#closeText").css("display", "block"); 
                currentStatus = 5;
                getTableData(1);
                break;
        case 6: $("#unpaid").css("display", "block");
                $("#unpaidOption").css('box-shadow', '10px 10px 5px #AAA');
                $("#unpaidText").css("display", "block"); 
                currentStatus = 6;
                getTableData(1);
    }
}

function getTableData(pageNo) { ///get table data 
    $.ajax({
        url: '/Admin/GetTablesData',
        type: 'GET',
        contentType: 'application/json',
        data: {
            pageNo: pageNo,
            status: statusStrings[currentStatus],
            partialViewName: statusTableStrings[currentStatus],
        },
        success: function (response) {
            setTableData(response);
        }
    })
}

function setTableData(response) {     /// set table date in par
    switch (statusStrings[currentStatus]) {
        case "New": $("#new").html(response); break;
        case "Pending": $("#pending").html(response); break;
        case "Active": $("#active").html(response); break;
        case "Conclude": $("#conclude").html(response); break;
        case "Close": $("#close").html(response); break;
        case "Unpaid": $("#unpaid").html(response); break;
    }
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

function getLastPageNo(totalRequestCount) {      /// count total pages
    return totalRequestCount % 10 != 0 ? parseInt(totalRequestCount / 10, 10) + 1 : parseInt(totalRequestCount / 10, 10);
}

function getNextPageData(currentPageNo, totalRequestCount) {
    var lastPageNo = getLastPageNo(totalRequestCount);
    if (currentPageNo < lastPageNo) {
        if (isSearch) {
            patientSearch(currentPageNo + 1);
        }
        else {
            getTableData(currentPageNo + 1);
        }
    }
}

function getPreviousPageData(currentPageNo) {
    if (currentPageNo > 1) {
        if (isSearch) {
            searchPatient(currentPageNo - 1);
        } else {
            getTableData(currentPageNo - 1);
        }
    }
}

function navigateToFirstPage() {
    if (isSearch) {
        patientSearch(1);
    } else {
        getTableData(1);
    }
}

function navigateToLastPage(totalRequestCount) {
    var lastPageNo = getLastPageNo(totalRequestCount);
    if (isSearch) {
        patientSearch(lastPageNo);
    } else {
        getTableData(lastPageNo);
    }
}

function patientSearch(pageNo) {
    var patientName = $(".searchPatient").val();
    if (patientName.length !== 0) {
        isSearch = true;
        getSearchData(patientName, pageNo, 1);
    }
    else {
        isSearch = false;
        changeTable(currentStatus);
    }
}

function getSearchData(patientName,pageNo,type) {
    $.ajax({
        url: '/Admin/Search',
        type: 'GET',
        contentType: 'application/json',
        data: {
            type: type,
            patientName: patientName,
            status: statusStrings[currentStatus],
            partialViewName: statusTableStrings[currentStatus],
            pageNo: pageNo,
            regionId: 1,
        },
        success: function (response) {
            setTableData(response);
        }
    })
}

function regionSearch(pageNo) {
    var regionId = $(".searchRegion").val();
    console.log(regionId);
    if (regionId) {
        isSearch = true;
        getSearchData(regionId, pageNo, 2);
    }
    else {
        isSearch = false;
        changeTable(currentStatus);
    }
}


