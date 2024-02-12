
function alert_close(element) {
    element.parentNode.parentNode.style.display = 'none';
    document.getElementById("main-body").style.opacity = 1;
}

function onChange()
//{
//    console.log("hi");
//    var xmlHttp = new XMLHttpRequest();
//    xmlHttp.open("GET", "/patient/onChange("avi@123gmail.com")", false); // false for synchronous request
//    xmlHttp.send(null);
//    console.log(xmlHttp.responseText);
//    //$.get("/patientcontroller/onchange", { parameter: "avi@charusat.edu" }, function (data) {
//    //    console(data);
//    //    });
//    //window.location.href = '@Url.Action("Patient", "OnChnage")';
//}

{
    $.get("/Patient/onChange", { email: 'avi@123gmail.com' }, function (data) {
        alert(data);
    });  

}

