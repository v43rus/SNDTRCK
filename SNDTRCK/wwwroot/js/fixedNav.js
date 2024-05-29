function updateNavPosition() {
    var header = document.getElementById('header');
    var fixedElement = document.getElementById('nav');
        
    // Get the height of the header
    var headerHeight = header.offsetHeight;

    // Calculate the height for the nav to fill the remaining space
    var navHeight = window.innerHeight - headerHeight;

    // Set the height of the nav
    nav.style.top = headerHeight + 'px';
    nav.style.height = navHeight + 'px';
}

// Call the function initially to set the position
updateNavPosition();

// Update the position on window resize
window.addEventListener('resize', updateNavPosition);