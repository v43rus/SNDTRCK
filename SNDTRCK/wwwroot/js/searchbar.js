﻿

function ShowSearchSuggestions(query, device) {

    //Send request to controller if query is > 2 characters
    //*Controller builds max three suggestions*
    //If there are a response, render it in the right container (desktop or mobile)
    //Make the container displayed as block

    //Clear the seach suggestion container
    ClearSearchSuggestionContainer(device);

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

                //Displays "See all" button in search container
                if (response !== null) {
                    document.getElementById("search-see-all-button").style.display = "block";
                }

                //Renderar raderna
                response.forEach(function (response) {

                    let suggestionNode = document.createElement("div")
                    suggestionNode.innerHTML = response;
                    //searchSuggestionContainer.innerHTML += suggestion;
                    let seeAllButton = document.getElementById("search-see-all-button");
                    searchSuggestionContainer.insertBefore(suggestionNode, seeAllButton);
                });
            }
        };
        xhr.send(JSON.stringify(query));
    }
    else {
        document.getElementById("desktop-search-suggestions-container").style.display = "none"
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

//Clears search suggestion container
function ClearSearchSuggestionContainer(device) {
    //To determine in what search-field to render suggestions
    let searchSuggestionContainer = "";
    if (device === "desktop") {
        searchSuggestionContainer = document.getElementById("desktop-search-suggestions-container");
    }
    else if (device === "mobile") {
        searchSuggestionContainer = document.getElementById("mobile-search-suggestions-container");
    }
    
    var children = searchSuggestionContainer.children;
    var numChildren = children.length;

    // Loopa igenom barnen och ta bort dem förutom det sista
    for (var i = 0; i < numChildren - 1; i++) {
        searchSuggestionContainer.removeChild(children[0]); // Ta bort det första barnet varje gång
    }

    //Hides "See all" button in search container
    document.getElementById("search-see-all-button").style.display = "none";
}