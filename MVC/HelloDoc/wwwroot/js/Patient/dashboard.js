function changeTab(element1, element2) {
    var temp;
    temp = document.getElementsByClassName("tab-content");
    for (i = 0; i < temp.length; i++) {
        temp[i].style.display = "none";
    }
    // display.getElementsByClassName("element").style.fontweight="bold";
    element1.style.display = "Block";
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
    if (temp == "none") {
        document.getElementById("side-bar").style.display = "block";
    } else {
        document.getElementById("side-bar").style.display = "none";
    }
}

function openDocumnet(path) {
    console.log("hi");
    Page.ClientScript.RegisterStartupScript(
        this.GetType(), "OpenWindow", "window.open(path,'_newtab');", true);
}
