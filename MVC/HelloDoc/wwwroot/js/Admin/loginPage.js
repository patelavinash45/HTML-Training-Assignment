var temp = true;
function changeIcon() {
    if (temp) {
        document.getElementById("show-password").style.display = "none";
        document.getElementById("hide-password").style.display = "inline";
        document.getElementById("password").type = "text";
        temp = false;
    } else {
        document.getElementById("hide-password").style.display = "none";
        document.getElementById("show-password").style.display = "inline";
        document.getElementById("password").type = "password";
        temp = true;
    }
}