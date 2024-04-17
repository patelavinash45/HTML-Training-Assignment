var IdList = [];
$(document).on("click","#mainCheckBox",function () {
    if ($(this).is(':checked')) {
        $('.checkBox').prop('checked', true);
        var temp = $('.checkBox');
        IdList = [];
        for (let i = 0; i < temp.length; i++) {
            IdList.push($(temp[i]).attr("id"));
        }
    }
    else {
        $('.checkBox').prop('checked', false);
        IdList = [];
    }
});

$(document).on("click", "#downloadAll", function () {
    for (let i = 0; i < IdList.length; i++) {
        var id = "." + IdList[i];
        $(id)[0].click();
    }
});

$(document).on("click", "#deleteAll", function () {
    if (IdList.length > 0) {
        $.ajax({
            url: "/Admin/DeleteAllFiles",
            type: 'GET',
            contentType: 'application/json',
            data: {
                requestWiseFileIdsList: JSON.stringify(IdList),
            },
            success: function (response) {
                window.location.href = response.redirect;
            }
        })
    }
})

$(document).on("click", "#sendMail", function () {
    if (IdList.length > 0) {
        $.ajax({
            url: "/Admin/SendMail",
            type: "GET",
            contentType: "application/json",
            data: {
                requestWiseFileIdsList: JSON.stringify(IdList),
            },
            success: function (response) {
                window.location.href = response.redirect;
            }
        })
    }
})

function onCheckboxChnage(fileId,totalcount) {
    var id = $(fileId).attr("id");
    if ($(fileId).is(":checked")) {
        IdList.push(id);
    }
    else {
        var index = IdList.indexOf(id);
        IdList.splice(index, 1);
    }
    if (totalcount == IdList.length) {
        $('#mainCheckBox').prop('checked', true);
        $('.checkBox').prop('checked', true);
        var temp = $('.checkBox');
        IdList = [];
        for (let i = 0; i < temp.length; i++) {
            IdList.push($(temp[i]).attr("id"));
        }
    }
    else {
        $('#mainCheckBox').prop('checked', false);
    }
}
