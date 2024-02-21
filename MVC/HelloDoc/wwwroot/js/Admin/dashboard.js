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
        case 1: $("#new").css("display", "block"); $("#newOption").css('box-shadow', '10px 10px 5px #AAA'); break;
        case 2: $("#pending").css("display", "block"); $("#pendingOption").css('box-shadow', '10px 10px 5px #AAA'); getTableData("Pending"); break;
        case 3: $("#active").css("display", "block"); $("#activeOption").css('box-shadow', '10px 10px 5px #AAA'); break;
        case 4: $("#conclude").css("display", "block"); $("#concludeOption").css('box-shadow', '10px 10px 5px #AAA'); break;
        case 5: $("#close").css("display", "block"); $("#tocloseOption").css('box-shadow', '10px 10px 5px #AAA'); break;
        case 6: $("#unpaid").css("display", "block"); $("#unpaidOption").css('box-shadow', '10px 10px 5px #AAA'); break;
    }
}

function getTableData(temp) {

    //$.ajax({
    //    url: '/Admin/GetTablesData',
    //    type: 'GET',
    //    contentType: 'application/json',
    //    data: { status: temp },
    //    success: function (response) {
    //        console.log("hi");
    //    }
    //})


}