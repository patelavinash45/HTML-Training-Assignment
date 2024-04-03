var IdList = [];

$('#mainCheckBox').click(function () {
    if ($(this).is(':checked')) {
        $('.checkBox').prop('checked', true);
        var temp = $('.checkBox');
        var IdList = [];
        for (let i = 0; i < temp.length; i++) {
            IdList.push($(temp[i]).attr("id"));
        }
    }
    else {
        $('.checkBox').prop('checked', false);
        IdList = [];
    }
});

$('#downloadAll').click(function () {
    for (let i = 0; i < IdList.length; i++) {
        var id = "." + IdList[i];
        $(id)[0].click();
    }
});

$('#deleteAll').click(function () {
    $.ajax({
        url: "/Admin/DeleteAllFiles",
        type: 'post',
        contentType: 'application/json',
        data: JSON.stringify(IdList),
        success: function (response) {
            window.location.href = response.redirect;
        }
    })
})

$("#sendMail").click(function () {
    $.ajax({
        url: "/Admin/SendMail",
        type: "post",
        contentType: "application/json",
        data: JSON.stringify(IdList),
        success: function (response) {
            window.location.href = response.redirect;
        } 
    })
})

function onCheckboxChnage(fileId,totalcount) {
    var id = $(fileId).attr("id");
    if ($(fileId).is(":checked")) {
        IdList.push(id);
    }
    else{
        var index = IdList.indexOf(id);
        IdList.splice(index, 1);
    }
    if (totalcount == IdList.length) {
        $('#mainCheckBox').prop('checked', true);
        $('.checkBox').prop('checked', true);
        var temp = $('.checkBox');
        var IdList = [];
        for (let i = 0; i < temp.length; i++) {
            IdList.push($(temp[i]).attr("id"));
        }
    }
    else {
        $('#mainCheckBox').prop('checked', false);
    }
}
