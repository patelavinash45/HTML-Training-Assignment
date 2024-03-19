var statusStrings = ["", "New", "Pending", "Active", "Conclude", "Close", "Unpaid"];
var statusTableStrings = ["", "_NewTable", "_PendingTable", "_ActiveTable", "_ConcludeTable", "_CloseTable", "_UnpaidTable"];
var currentStatus = 1;         /// for which state is current
var currentDataType = 1;       /// 1 - all data , 2 - patient search , 3 - region search , 4 - requester type search

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
    $('.buttonHr').css("display", "none");
    currentDataType = 1;
    switch (temp) {
        case 1: $("#new").css("display", "block");
            $("#newOption").css('box-shadow', '10px 10px 5px #AAA');
            $("#statusText").text("(New)");
            currentStatus = 1;
            getTableData(1);
            break;
        case 2: $("#pending").css("display", "block");
            $("#pendingOption").css('box-shadow', '10px 10px 5px #AAA');
            $("#statusText").text("(Pending)");
            currentStatus = 2;
            getTableData(1);
            break;
        case 3: $("#active").css("display", "block");
            $("#activeOption").css('box-shadow', '10px 10px 5px #AAA');
            $("#statusText").text("(Active)");
            currentStatus = 3;
            getTableData(1);
            break;
        case 4: $("#conclude").css("display", "block");
            $("#concludeOption").css('box-shadow', '10px 10px 5px #AAA');
            $("#statusText").text("(Conclude)");
            currentStatus = 4;
            getTableData(1);
            break;
        case 5: $("#close").css("display", "block");
            $("#tocloseOption").css('box-shadow', '10px 10px 5px #AAA');
            $("#statusText").text("(Close)");
            currentStatus = 5;
            getTableData(1);
            break;
        case 6: $("#unpaid").css("display", "block");
            $("#unpaidOption").css('box-shadow', '10px 10px 5px #AAA');
            $("#statusText").text("(UnPaid)");
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

requesterType = 0;
function requesterTypeSearch(_requesterType, pageNo) {     /// search based on requester type
    requesterType = _requesterType;
    $('.buttonHr').css("display", "none");
    currentDataType = 4;
    $(".searchRegion").val("");
    $(".searchPatient").val("");
    switch (_requesterType) {
        case 1: $('#hr-1').css("display", "block"); getSearchData(_requesterType, pageNo); break;
        case 2: $('#hr-2').css("display", "block"); getSearchData(_requesterType, pageNo); break;
        case 3: $('#hr-3').css("display", "block"); getSearchData(_requesterType, pageNo); break;
        case 4: $('#hr-4').css("display", "block"); getSearchData(_requesterType, pageNo); break;
        case 5: $('#hr-5').css("display", "block"); getSearchData(_requesterType, pageNo); break;
        default: changeTable(currentStatus);
    }
}


//////

function getLastPageNo(totalRequestCount) {      /// count total pages
    return totalRequestCount % 10 != 0 ? parseInt(totalRequestCount / 10, 10) + 1 : parseInt(totalRequestCount / 10, 10);
}

function getNextPageData(currentPageNo, totalRequestCount) {   /// for next page in pagination
    var lastPageNo = getLastPageNo(totalRequestCount);
    if (currentPageNo < lastPageNo) {
        switch (currentDataType) {
            case 1: getTableData(currentPageNo + 1); break;
            case 2: patientSearch(currentPageNo + 1); break;
            case 3: regionSearch(currentPageNo + 1); break;
            case 4: requesterTypeSearch(requesterType, currentPageNo + 1); 
        }
    }
}

function getPreviousPageData(currentPageNo) {   /// for previous page in pagination
    if (currentPageNo > 1) {
        switch (currentDataType) {
            case 1: getTableData(currentPageNo - 1); break;
            case 2: patientSearch(currentPageNo - 1); break;
            case 3: regionSearch(currentPageNo - 1); break;
            case 4: requesterTypeSearch(requesterType, currentPageNo - 1); 
        }
    }
}

function navigateToFirstPage() {    /// for First page in pagination
    switch (currentDataType) {
        case 1: getTableData(1); break;
        case 2: patientSearch(1); break;
        case 3: regionSearch(1); break;
        case 4: requesterTypeSearch(requesterType, 1); 
    }
}

function navigateToLastPage(totalRequestCount) {       /// for last page in pagination
    var lastPageNo = getLastPageNo(totalRequestCount);
    switch (currentDataType) {
        case 1: getTableData(lastPageNo); break;
        case 2: patientSearch(lastPageNo); break;
        case 3: regionSearch(lastPageNo); break;
        case 4: requesterTypeSearch(requesterType, lastPageNo); 
    }
}

function patientSearch(pageNo) {    
    $(".searchRegion").val("");
    $('.buttonHr').css("display", "none");
    var patientName = $(".searchPatient").val();
    if (patientName.length !== 0) {
        currentDataType = 2;
        getSearchData(patientName, pageNo);
    }
    else { 
        changeTable(currentStatus);
    }
}

function getSearchData(searchElement, pageNo) { 
    $.ajax({
        url: '/Admin/Search',
        type: 'GET',
        contentType: 'application/json',
        data: {
            type: currentDataType,
            searchElement: searchElement,
            status: statusStrings[currentStatus],
            partialViewName: statusTableStrings[currentStatus],
            pageNo: pageNo,
        },
        success: function (response) {
            setTableData(response);
        }
    })
}

function regionSearch(pageNo) {
    $(".searchPatient").val("");
    $('.buttonHr').css("display", "none");
    var regionId = $(".searchRegion").val();
    if (regionId) {
        currentDataType = 3;
        getSearchData(regionId, pageNo);
    }
    else {
        changeTable(currentStatus);
    }
}


///


$(document).on("click", "#exportData", function () {
    var data;
    var status = ["", "new", "pending", "active", "conclude", "close", "unpaid"];
    var id = "#" + status[currentStatus];
    var temp = $(id).children()[2];
    var pageNo = $(temp).find(".currentPage").text();
    var searchElement;
    switch (currentDataType) {
        case 2: searchElement = $(".searchPatient").val(); break;
        case 3: searchElement = $(".searchRegion").val(); break;
        case 4: searchElement = requesterType; break;
    }
    $.ajax({
        url: '/Admin/ExportData',
        type: 'GET',
        xhrFields: {
            responseType: 'arraybuffer'
        },
        data: {
            pageNo: pageNo,
            status: statusStrings[currentStatus],
            type: currentDataType,
            searchElement: searchElement,
        },
        success: function (response) {
            const a = document.createElement('a');
            var unit8array = new Uint8Array(response);
            a.href = window.URL.createObjectURL(new Blob([unit8array], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' }));
            a.download = 'Data.xlsx';
            a.click();
        }
    })
})