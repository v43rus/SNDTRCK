
//To make the desktop navbar dissapear when scrolling down
var prevScrollpos = window.pageYOffset;
var mobileNavHamburger = document.getElementsByClassName("hamburger")[0]
var mobileNavHamburgerDisplay = window.getComputedStyle(mobileNavHamburger)
window.onscroll = function () {
    var currentScrollPos = window.pageYOffset;
    if (prevScrollpos > currentScrollPos) {
        document.getElementsByClassName("nav")[0].style.visibility = "visible";
    }
    else {

        if (mobileNavHamburgerDisplay.display === "none") {
            document.getElementsByClassName("nav")[0].style.visibility = "hidden";
        }

    }
    prevScrollpos = currentScrollPos;
}   


//To disable scrolling while mobile menu is open
var body = document.querySelector("body");
var mobileNavCheckbox = document.getElementById("mobile-menu-checkbox");
function checkIfScroll() {
    if (mobileNavCheckbox.checked) {
        body.style.overflowY = "hidden"
    }
    else {
        body.style.overflowY = "initial"
    }
}
