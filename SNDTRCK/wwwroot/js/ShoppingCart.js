
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

    //Uppdatera quantity-texten på produktraden
    UpdateQuantityIndicator(productId, cartObj[productId], "Add");

    //Konventera JS-objektet åter till JSON
    var jsonCart = JSON.stringify(cartObj);

    //Uppdatera cookien med nya JSON-strängen, om cookien inte finns skapas den här
    document.cookie = "userCart=" + jsonCart;
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

    //Uppdatera quantity-texten på produktraden
    UpdateQuantityIndicator(productId, cartObj[productId], "Sub");
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

    //Get product row in cart and delete it
    var id = "row-product-" + productId;
    var row = document.getElementById(id);
    row.remove();
}

function UpdateQuantityIndicator(productId, newQuantity) {

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
