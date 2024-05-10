

function ShowSearchSuggestions(query, device) {

    //Send request to controller if query is > 2 characters
    //*Controller builds max three suggestions*
    //If there are a response, render it in the right container (desktop or mobile)
    //Make the container displayed as block

    if (query.length > 2) {
        let xhr = new XMLHttpRequest();
        xhr.open("POST", "/Home/BuildSearchSuggestions", false);
        xhr.setRequestHeader("Content-Type", "application/json");

        xhr.onreadystatechange = function () {
            if (xhr.readyState === 4 && xhr.status === 200) {
                var response = JSON.parse(xhr.responseText); //När svaret från servern tas emot tolkas det som JSON-data med JSON-parse

                //To determine in what search-field to render suggestions
                let searchSuggestionContainer = "";
                if (device === "desktop" && response !== null) {
                    searchSuggestionContainer = document.getElementById("desktop-search-suggestions-container");
                    document.getElementById("desktop-search-suggestions-container").style.display = "block";
                }
                else if (device === "mobile" && response !== null) {
                    searchSuggestionContainer = document.getElementById("mobile-search-suggestions-container");
                    document.getElementById("mobile-search-suggestions-container").style.display = "block";
                }

                //Renderar raderna
                response.forEach(function (suggestion) {
                    searchSuggestionContainer.innerHTML += suggestion;
                });
            }
        };
        xhr.send(JSON.stringify(query));
    }
}

function HideSuggestionsContainer(device) {

    //To determine what container that should be hidden
    if (device === "desktop") {
        document.getElementById("desktop-search-suggestions-container").style.display = "none"
    }
    else if (device === "mobile") {
        document.getElementById("mobile-search-suggestions-container").style.display = "none"
    }
}