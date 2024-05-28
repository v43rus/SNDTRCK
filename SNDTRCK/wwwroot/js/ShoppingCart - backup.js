
//Client cookie CRUD operations

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


//Add product to cart cookie
function AddToCart(productId = 1, quantity = 1) {

    //Gets existing cookie
    var existingUserCart = GetCookie("userCart");

    //Om existingUserCart är sann (alltså innehåller data) fortsätt med JSON.parse, annars skapa tomt js-objekt (: {})
    var cartObj = existingUserCart ? JSON.parse(existingUserCart) : {};

    //Öka kvantiteten om produkten redan finns i korgen, annars lägg till den med kvantiteten 1
    //I ett js-objekt består av nyckel-värde-par, ex i vårt fall: {"1":3, "5":1}
    //cartObj[productId] = (cartObj[productId] || 0) + quantity;
    if (cartObj[productId]) {
        cartObj[productId] += quantity;
    }
    else {
        cartObj[productId] = quantity;
    }

    //Update cart icon quantity indicator
    UpdateCartQuantityIndicatorByAction("raise")

    //Uppdatera quantity-texten på produktraden
    UpdateProductQuantityIndicator(productId, cartObj[productId], "Add");

    //Konventera JS-objektet åter till JSON
    var jsonCart = JSON.stringify(cartObj);
    //var jsonCart = encodeURIComponent(cartObj);
    console.log("Ny cart: " + jsonCart)

    //Uppdatera cookien med nya JSON-strängen, om cookien inte finns skapas den här
    document.cookie = "userCart=" + jsonCart + "; path=/; SameSite=None; Secure";
}

//Remove one quantity of product from cart cookie
function RemoveFromCart(productId = 1, quantity = 1) {
    //Gets existing cookie
    var existingUserCart = GetCookie("userCart");

    //Om existingUserCart är sann (alltså innehåller data) fortsätt med JSON.parse, annars return
    var cartObj = existingUserCart ? JSON.parse(existingUserCart) : null;

    //Minska kvantiteten med 1 eller ta bort produkten helt
    if ((cartObj[productId] - quantity) > 0) {
        cartObj[productId] -= quantity;
    }
    else {
        delete cartObj[productId];
    }

    //Konventera JS-objektet åter till JSON
    var jsonCart = JSON.stringify(cartObj);

    //Uppdatera cookien med nya JSON-strängen
    document.cookie = "userCart=" + jsonCart;

    //Update cart icon quantity indicator
    UpdateCartQuantityIndicatorByAction("lower")

    //Uppdatera quantity-texten på produktraden
    UpdateProductQuantityIndicator(productId, cartObj[productId], "Sub");
}

//Remove product entirely from cart
function DeleteProductFromCart(productId){

    //Delete product from cookie

    //Gets existing cookie
    var existingUserCart = GetCookie("userCart");
    var cartObj = existingUserCart ? JSON.parse(existingUserCart) : null;

    //Delete key and value
    delete cartObj[productId];

    //Konventera JS-objektet åter till JSON
    var jsonCart = JSON.stringify(cartObj);

    //Uppdatera cookien med nya JSON-strängen
    document.cookie = "userCart=" + jsonCart;

    //Remove product's quantity value from cart quantity indicator
        //Get the product row in question
        var id = "row-product-" + productId;
        var row = document.getElementById(id);
        var quantityTextCollection = row.getElementsByClassName("quantity-indicator");
        var quantityText = quantityTextCollection[0];
        //Gets the indicator value as a number
        let quanityIndicatorText = quantityText.innerText;
        let quantityValue = parseInt(quanityIndicatorText);
        for (let i = 0; i < quantityValue; i++) {
            UpdateCartQuantityIndicatorByAction("lower")
        }

    //Get product row in cart and delete it
    var id = "row-product-" + productId;
    var row = document.getElementById(id);
    row.remove();
}

//Each product row's quantity indicator in the cart
function UpdateProductQuantityIndicator(productId, newQuantity) {

    //The user uses the function "AddToCart" but is on a product page where quantity indicator dont exists
    try {
        //Get the product row in question
        var id = "row-product-" + productId;
        var row = document.getElementById(id);
        var quantityTextCollection = row.getElementsByClassName("quantity-indicator");
        var quantityText = quantityTextCollection[0];

        //Remove row if quantity == 0 (undefined)
        if (newQuantity === undefined) {
            DeleteProductFromCart(productId);
        }
        else {
            quantityText.innerText = newQuantity;
        }
    }
    catch {
        //Nothing needed here
    }
}

//The quantity indicator on the cart icon
function UpdateCartQuantityIndicatorByAction(action) {
    console.log("Updated cart quantity by action")
    //Gets the "your cart is empty-text"
    let emptyCartText = document.getElementById("empty-cart-text")

    //Gets the quantity indicator
    let quanityIndicator = document.getElementById("cart-quantity-indicator");
    //Gets total cost container
    let totalCostContainer = document.getElementById("proceed-to-checkout-container");
    //Gets the indicator value as a number
    let quanityIndicatorText = quanityIndicator.innerText;
    let quantityValue = parseInt(quanityIndicatorText);

    if (action === "raise") {
        quanityIndicator.innerText = quantityValue + 1;

        quanityIndicator.style.display = "initial"; //If the indicator was hidden because cart was empty

        try {
            emptyCartText.style.display = "none"; //If the cart was empty
            totalCostContainer.style.display = "flex";
        }
        catch { }
    }
    else if (action === "lower") {

        if (quantityValue - 1 === 0){
            //Hide indicator
            quanityIndicator.style.display = "none";

            //Tries to display "your cart is empty" if the user is on the cart-page
            try {
                emptyCartText.style.display = "initial";
                totalCostContainer.style.display = "none";
            }
            catch {
                //No code needed
            }

            quanityIndicator.innerText = quantityValue - 1;
        }
        else if (quantityValue === 0) {
            //Do nothing
        }
        else {
            quanityIndicator.innerText = quantityValue - 1;
        }
    }
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
    //Uppdatera cookien med nya JSON-strängen
    document.cookie = "userCart=";
    console.log("Cart cleared")
}

//Startup
//Since the page needs to be reloaded for the cookie to update,
//two different methods are needed to upsate the cart quantity indicator.
//One that gets the info from the cookie when the page is loaded and one that
//updates the indicator manually when a button is pressed
UpdateCartQuantityIndicatorByCookie();
