
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
function AddToCart(productId = "1", quantity = 2) {

    //Gets existing cookie
    var existingUserCart = getCookie("userCart");

    //Om existingUserCart är sann (alltså innehåller data) fortsätt med JSON.parse, annars skapa tomt js-objekt (: {})
    var cartObj = existingUserCart ? JSON.parse(existingUserCart) : {};

    //Öka kvantiteten om produkten redan finns i korgen, annars lägg till den med kvantiteten 1
    //I ett js-objekt består av nyckel-värde-par, ex i vårt fall: {"1":3, "5":1}
    cartObj[productId] = (cartObj[productId] || 0) + quantity;

    //Konventera JS-objektet åter till JSON
    var jsonCart = JSON.stringify(cartObj);

    //Uppdatera cookien med nya JSON-strängen, om cookien inte finns skapas den här
    document.cookie = "userCart=" + jsonCart;
}

//On load, Read cookie and send request to controller for product-cart-lines
window.addEventListener("load", (event) => {

    //Lagrar en JSON sträng som denna: "{"1":1,"45":2,"22":5}", först siffran är productId och den andra är antal
    cartData = GetCookie(userCart);

    if (cart !== null) {
        var xhr = new XMLHttpRequest(); //nytt XMLHttpRequest-objekt skapas, vilket är en inbyggd webbläsarobjekt som används för att skicka HTTP-förfrågningar och ta emot svar från en server utan att ladda om hela sidan.
        xhr.open("POST", "/Controller/Action", true); //asynkron POST-förfrågan till en viss URL (/Controller/Action) öppnas. Detta är den URL där servern förväntas ta emot förfrågan för att behandla varukorgen.
        xhr.setRequestHeader("Content-Type", "application/json"); //ställer in HTTP-headers för förfrågan. Här specificeras att den skickade datan är i JSON-format.

        //händelselyssnare som lyssnar på förändringar i xhr-objektets status
        //När statusen ändras, kontrollerar koden om förfrågan är klar (status 4) och om svaret från servern är OK (status 200).
        xhr.onreadystatechange = function () { 
            if (xhr.readyState === 4 && xhr.status === 200) {
                var response = JSON.parse(xhr.responseText); //När svaret från servern tas emot tolkas det som JSON-data med JSON-parse

                console.log(response);
            }
        };
        xhr.send(JSON.stringify(cartData)); //Här skickas den faktiska cart-datan till servern, först så säkerställs det att det är i JSON-format, även fast GetCookie() returnerar JSON
    }
    else {
        //Display "Your shopping cart is empty"
    }

});

//AJAX requests to controller to get product data



//Render items in shoppingcart



//User CRUD operations for items in shopping cart (Add, update quantity, delete)


//OnPageLoad
function OnPageLoad() {

}