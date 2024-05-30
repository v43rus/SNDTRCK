
//The button to "See all" in the search field
let searchSeeAllButton = "";

function ShowSearchSuggestions(query, device) {

    //Send request to controller if query is > 2 characters
    //*Controller builds max three suggestions*
    //If there are a response, render it in the right container (desktop or mobile)
    //Make the container displayed as block

    //Clear the seach suggestion container
    ClearSearchSuggestionContainer(device);

    if (query.length > 0) {
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
                    searchSeeAllButton = document.getElementById("desktop-search-see-all-button");
                }
                else if (device === "mobile" && response !== null) {
                    searchSuggestionContainer = document.getElementById("mobile-search-suggestions-container");
                    searchSeeAllButton = document.getElementById("mobile-search-see-all-button");
                }
                //searchSuggestionContainer.style.display = "block";

                //Displays "See all" button in search container
                if (response.length > 0) {
                    searchSeeAllButton.style.display = "block";
                    searchSuggestionContainer.style.display = "block"
                }
                else {
                    searchSeeAllButton.style.display = "none";
                    searchSuggestionContainer.style.display = "none"
                }

                //Renderar raderna
                response.forEach(function (response) {

                    let suggestionNode = document.createElement("div")
                    suggestionNode.innerHTML = response;
                    searchSuggestionContainer.insertBefore(suggestionNode, searchSeeAllButton);
                });
            }
        };
        xhr.send(JSON.stringify(query));
    }
    else {
        //If the query wasn't long enough the following will be hidden
        document.getElementById("desktop-search-suggestions-container").style.display = "none";
        document.getElementById("mobile-search-suggestions-container").style.display = "none";
        searchSeeAllButton.style.display = "none";
    }
}

function HideSuggestionsContainer(device) {

    //Neccessary to make links clickable in search suggestion before they dissapear
    setTimeout(function () {

        let searchSuggestionsContainer = "";
        let searchBar = "";

        //To determine what container that should be hidden
        if (device === "desktop") {
            searchSuggestionsContainer = document.getElementById("desktop-search-suggestions-container");
            searchBar = document.getElementById("desktop-header-searchbar");
        }
        else if (device === "mobile") {
            searchSuggestionsContainer = document.getElementById("mobile-search-suggestions-container");
            searchBar = document.getElementById("mobile-header-searchbar");
        }

        searchSuggestionsContainer.style.display = "none";

        //Clears user input
        searchBar.value = "";
    }, 100);
}

//Clears search suggestion container
function ClearSearchSuggestionContainer(device) {
    //To determine in what search-field to render suggestions
    let searchSuggestionContainer = "";
    if (device === "desktop") {
        searchSuggestionContainer = document.getElementById("desktop-search-suggestions-container");
        searchSeeAllButton = document.getElementById("desktop-search-see-all-button");
    }
    else if (device === "mobile") {
        searchSuggestionContainer = document.getElementById("mobile-search-suggestions-container");
        searchSeeAllButton = document.getElementById("mobile-search-see-all-button");
    }
    
    var children = searchSuggestionContainer.children;
    var numChildren = children.length;

    // Loopa igenom barnen och ta bort dem förutom det sista
    for (var i = 0; i < numChildren - 1; i++) {
        searchSuggestionContainer.removeChild(children[0]); // Ta bort det första barnet varje gång
    }

    //Hides "See all" button in search container
    searchSeeAllButton.style.display = "none";
}

//Code to detect when the "see all" button in search suggestions is pressed
window.onload = function () {
    document.getElementById('desktop-search-see-all-button').onclick = function () {
        document.getElementById("desktop-searchbar-form").submit();
    };

    document.getElementById('mobile-search-see-all-button').onclick = function () {
        document.getElementById('mobile-header-searchbar-form').submit();
    };
};