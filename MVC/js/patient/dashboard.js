function openTab(element) {
    var temp;
    temp = document.getElementsByClassName("tab-content");
    for (i = 0; i < temp.length; i++) {
        temp[i].style.display = 'none';
    }
    element.style.display = 'block';
}