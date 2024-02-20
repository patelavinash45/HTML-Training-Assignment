var temp = true;
function Password() {
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


// function darkmoad(){
//   // data-bs-theme="dark"
//   var html=document.querySelector("html");
//   console.log(html);
//   if(html.getAttribute("data-bs-theme")!="dark"){
//     html.setAttribute("data-bs-theme");
//   }
//   else
//   {
//     html.setAttribute("data-bs-theme")="auto";
//   }
// }
