


//GetCookie
function GetCookie(name) {

    //Olika cookies separeras med "; ", dessa delas på här och lagras i en array
    //["user=eyJpZCI6MzEsIm20ifQ==", "settings=eyJtZXNzYWdl9ybGQifQ=="]
    //Varje cookie slutar på "==" "bara för att", det har med encoding att göra
    var cookieArray = document.cookie.split("; ")

    for (var i = 0; i < cookieArray.length; i++) {
        //Varje cookiedel delas vid likhetstecken för att få nyckeln och värdet
        //[  ["user", "eyJpZCI6MzEsIm20ifQ=="], ["settings", "eyJtZXNzYWdl9ybGQifQ=="]  ]
        var cookiePair = cookieArray[i].split("=");

        //Kontrollerar namnet på varje cookie och ser om det matchar
        if (cookiePair[0] === name) {

            //dekodar värdet från Base64 (som cookie data lagras i) till JSON
            console.log("GetCookie: " + decodeURIComponent(cookiePair[1]));
            return decodeURIComponent(cookiePair[1]);
            //"{"1":1,"45":2,"22":5}"
        }
    }
    return null;
}

//The quantity indicator on the cart icon
function UpdateCartQuantityIndicatorByCookie() {

    //Calculates how many products there are in the cart;
    let jsonObj = JSON.parse(GetCookie("userCart"));
    let totalProducts = 0; //Stores the number of products in cart
    for (var key in jsonObj) {
        if (jsonObj.hasOwnProperty(key)) {
            totalProducts += jsonObj[key];
        }
    }

    console.log(jsonObj)

    //Gets the quantity indicator
    let quanityIndicator = document.getElementById("cart-quantity-indicator");
    //Gets total cost container
    let totalCostContainer = document.getElementById("proceed-to-checkout-container")
    //Gets the indicator value as a number
    let quanityIndicatorText = quanityIndicator.innerText;
    let quantityValue = parseInt(quanityIndicatorText);

    //Gets the "your cart is empty-text"
    let emptyCartText = document.getElementById("empty-cart-text");

    if (totalProducts > 0) {
        quanityIndicator.style.display = "initial"; //If the indicator was hidden because cart was empty
        quanityIndicator.innerText = totalProducts;
        try {
            totalCostContainer.style.display = "flex";
        }
        catch { }
    }
    else if (totalProducts === 0) {

        //hide indicator, try to display empty cart
        quanityIndicator.style.display = "none";
        quanityIndicator.innerText = totalProducts;

        //Tries to display "your cart is empty" if the user is on the cart-page
        try {
            emptyCartText.style.display = "initial";
        }
        catch {
            //No code needed
        }
    }
}

//Called from console in browser for testing purposes
function ClearCart() {

    var jsonCart = JSON.stringify({});
    var uriCart = encodeURIComponent(jsonCart)
    //Uppdatera cookien med nya JSON-strängen, om cookien inte finns skapas den här
    document.cookie = "userCart=" + uriCart + "; path=/; SameSite=None; Secure; max-age= 26000000";
    console.log("userCart cleared, new cart: " + jsonCart)
}

//Startup
//Since the page needs to be reloaded for the cookie to update,
//two different methods are needed to upsate the cart quantity indicator.
//One that gets the info from the cookie when the page is loaded and one that
//updates the indicator manually when a button is pressed
console.log("Hello")
ClearCart();
UpdateCartQuantityIndicatorByCookie();
