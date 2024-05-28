
//Render shopping cart rows in view shopping cart, with ajax

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
            //"{1:1,45:2,22:5}"
        }
    }
    return null;
}

//On load, Read cookie and send request to controller for product-cart-rows
window.addEventListener("load", (event) => {

    //Lagrar en JSON sträng som denna: "{1:1,45:2,22:5}", först siffran är productId och den andra är antal
    cartData = GetCookie("userCart");

    console.log("cartData:" + cartData.length)

    if (cartData.length > 2) { //En tom korg består av tecknen: {}
        var xhr = new XMLHttpRequest(); //nytt XMLHttpRequest-objekt skapas, vilket är en inbyggd webbläsarobjekt som används för att skicka HTTP-förfrågningar och ta emot svar från en server utan att ladda om hela sidan.
        xhr.open("POST", "/Home/BuildShoppingCartRows", false); //asynkron POST-förfrågan till en viss URL (/Controller/Action) öppnas. Detta är den URL där servern förväntas ta emot förfrågan för att behandla varukorgen.
        xhr.setRequestHeader("Content-Type", "application/json"); //ställer in HTTP-headers för förfrågan. Här specificeras att den skickade datan är i JSON-format.

        //händelselyssnare som lyssnar på förändringar i xhr-objektets status
        //När statusen ändras, kontrollerar koden om förfrågan är klar (status 4) och om svaret från servern är OK (status 200).
        xhr.onreadystatechange = function () { 
            if (xhr.readyState === 4 && xhr.status === 200) {
                var response = JSON.parse(xhr.responseText); //När svaret från servern tas emot tolkas det som JSON-data med JSON-parse

                //Hämtar container för produktraderna i varukorgen
                var productContainer = document.getElementById("Shoppingcart-product-container")

                //Hides loader 
                let loader = document.getElementById("loader");
                loader.style.display = "none";

                //Renderar raderna
                response.forEach(function (productRow) {
                    productContainer.innerHTML += productRow;
                });
            }
        };
        xhr.send(JSON.stringify(cartData)); //Här skickas den faktiska cart-datan till servern, först så säkerställs det att det är i JSON-format, även fast GetCookie() returnerar JSON
        console.log("cartData sent:");
        console.log(JSON.stringify(cartData));
    }
});

//Checks if the loader should be displayed or not 
//For some reason this didn't work in the window.load function
function DisplayLoader() {
    cartData = GetCookie("userCart");

    if (cartData.length > 2) {
        //Displays loader 
        let loader = document.getElementById("loader");
        loader.style.display = "block";
    }
}

//Startup
DisplayLoader();