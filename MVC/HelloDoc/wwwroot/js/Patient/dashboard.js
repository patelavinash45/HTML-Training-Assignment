function changeTab(element1, element2) {
    var temp;
    temp = document.getElementsByClassName("tab-content");
    for (i = 0; i < temp.length; i++) {
        temp[i].style.display = "none";
    }
    // display.getElementsByClassName("element").style.fontweight="bold";
    element1.style.display = "inline";
    if (element2) {
        document.getElementById("dashboard").style.backgroundColor = "#dcdbdb";
        document.getElementById("dashboard").style.color = "cadetblue";
        document.getElementById("profile").style.backgroundColor = "white";
        document.getElementById("profile").style.color = "black";
    } else {
        document.getElementById("profile").style.backgroundColor = "#dcdbdb";
        document.getElementById("profile").style.color = "cadetblue";
        document.getElementById("dashboard").style.backgroundColor = "white";
        document.getElementById("dashboard").style.color = "black";
    }
    document.getElementById("side-bar").style.display = "none";
}

function sidebar() {
    var temp = document.getElementById("side-bar").style.display;
    console.log(document.getElementById("side-bar").style);
    if (temp == "none") {
        document.getElementById("side-bar").style.display = "inline";
    } else {
        document.getElementById("side-bar").style.display = "none";
    }
}
