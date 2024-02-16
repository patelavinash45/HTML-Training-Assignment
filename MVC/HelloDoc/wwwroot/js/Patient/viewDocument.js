

function uploadFile(id) {
    var file = document.getElementById("file-picker").value;
    $.ajax({
        url: '/Patient/UploadFile',
        type: 'POST',
        contentType: 'application/json',
        data: { file: file },
        success: function (response) {
            if (response) {
                console.log(response)
            } else {
                console.log(response)
            }
        }
    });
    //$.post("/Patient/UploadFile",
    //    {
    //        file: file,
    //    },
    //    function (data, status) {
    //        alert("Data: ");
    //    });
}

$('#mainCheckBox').click(function () {
    if ($(this).is(':checked')) {
        console.log($('.checkBox'));
        $('.checkBox').prop('checked', true);
    }
    else {
        $('.checkBox').prop('checked', false);
    }
});

$('#downloadText').click(function () {
    //var temp = $('.download');
    //for (let i = 0; i < temp.length / 2; i++) {
    //    temp[i].click();
    //}
    $.ajax({
        url: '/Patient/DownloadAll',
        type: 'GET',
        contentType: 'application/json',
        data: { id: 40 },
        success: function (response) {
            if (response.emailExists) {
                $('#input-password').hide();
            } else {
                $('#input-password').css("display", "flex");
            }
        }
    });
});