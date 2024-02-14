

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
        $('#checkBox').prop('checked', true);
    }
    else {
        $('#checkBox').prop('checked', false);
    }
});

$('#downloadText').click(function () {
    $('#download')[0].click();
});