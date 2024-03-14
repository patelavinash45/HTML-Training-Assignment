$(document).on("submit", "#form", function (e) {
    if ($("#newnotes").val().length == 0) {
        $(".validation").css("display", "block");
        e.preventDefault();
    }
    else {
        $(".validation").css("display", "none");
    }
})