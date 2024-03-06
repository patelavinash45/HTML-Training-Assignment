var IdList = [];

$('#mainCheckBox').click(function () {
    if ($(this).is(':checked')) {
        $('.checkBox').prop('checked', true);
        var temp = $('.checkBox');
        var List = [];
        for (let i = 0; i < temp.length; i++) {
            List.push($(temp[i]).attr("id"));
        }
        IdList = List;
    }
    else {
        $('.checkBox').prop('checked', false);
        var List = [];
        IdList = List;
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
        var List = [];
        for (let i = 0; i < temp.length; i++) {
            List.push($(temp[i]).attr("id"));
        }
        IdList = List;
    }
    else {
        $('#mainCheckBox').prop('checked', false);
    }
}