
// Global variables.
let map;
let allFlightsPath = {};
let allFlightsMarker = {};
const regularImg = "../images/small_black.png";
const clickedImg = "../images/blue_light.png";

// Reset a row color.
function ResetRowsColor(name){
    let listItems = document.querySelectorAll(name);
    let i = 0;
    let size = listItems.length;
    for (i = 0; i < size; i++) {
        listItems[i].classList.remove("highlightedRow");
    }
}

// Reset markers and paths of all the flights.
function ResetFlights() {
    for (let key in allFlightsPath) {
        allFlightsPath[key].setMap(null);
        var icon = { url: regularImg };
        allFlightsMarker[key].setIcon(icon);
    }
}

// Reset the rows color in both lists.
function ResetRowsColorsOnReset() {
    ResetRowsColor("#my-flights-list > li");
    ResetRowsColor("#external-flights-list > li");
}

// Initialize the map element.
function initMap() {
    // Map options.
    let options = {
        zoom: 1,
        center: { lat: 0.00, lng: -71.0589 }
    };
    // New map.
    map = new google.maps.Map(document.getElementById("map"), options);
    google.maps.event.addListener(map, "click", ResetFlights);
    google.maps.event.addListener(map, "click", ResetRowsColorsOnReset);
    google.maps.event.addListener(map, "click", ResetFlightDetails);
}

// Display the path of a flight given it's id.
function DisplayPath(id) {
    // Iterate all the paths
    for (let key in allFlightsPath) {
        let icon;
        if (key === id) {
            allFlightsPath[key].setMap(map);
             icon = { url: clickedImg };
            allFlightsMarker[key].setIcon(icon);
        } else {
            allFlightsPath[key].setMap(null);
            icon = { url: regularImg };
            allFlightsMarker[key].setIcon(icon);
        }
    }   
}

// Set a path.
function SetPath(id, message) {

    console.log("message: ");
    console.log(message);
    // Send a get request that returns a FlightPlan.
    
    $.getJSON(message, (data) => {
        
        console.log("data: " );
        console.log(data);
        let startLoc = new google.maps.LatLng(data.initial_location.latitude, data.initial_location.longitude);
        let flightPlanCoordinates = [startLoc];
        data.segments.filter(segment => {
            let loc = new google.maps.LatLng(segment.latitude, segment.longitude);
            flightPlanCoordinates.push(loc);
        });
        // Create the path.
        let flightPath = new google.maps.Polyline({
            path: flightPlanCoordinates,
            geodesic: true,
            strokeColor: "#FF0000",
            strokeOpacity: 1.0,
            strokeWeight: 2
        });
        allFlightsPath[id] = flightPath;
    });
}

// Create a path for a flight by it's id.
function CreatePath(id, isExternal) {

    if (isExternal) {
        // Get the server who own the flight.
        $.getJSON("/api/servers/" + id, (server) => {
            console.log("ServerURL: ");
            console.log(server.ServerURL);
            let message = "/api/FlightPlan?id=" + id + "&url=" + server.ServerURL;            
            SetPath(id, message);
        });
    }
    else {
        SetPath(id, "/api/FlightPlan/" + id);
    } 
}

// Add a marker on the map for a given flight.
function AddMarker(flight) {  
    // Create a new marker.
    let marker = new google.maps.Marker({
        position: new google.maps.LatLng(flight.latitude, flight.longitude),
        map: map,
        icon: regularImg
    });
    // Add to the markers dictionary.
    allFlightsMarker[flight.flight_id] = marker;

    CreatePath(flight.flight_id, flight.is_external);

    marker.addListener("click", function () {
        DisplayFlightDetails(flight.flight_id, flight.is_external);
        DisplayPath(flight.flight_id);
    });
}

// Set a new position for a given flight.
function SetNewPosition(flight) {
    if (allFlightsMarker[flight.flight_id]) {
        allFlightsMarker[flight.flight_id].setPosition(new google.maps.LatLng(flight.latitude, flight.longitude));
    }
}

// Remove a marker from the map by it's flight id.
function RemovwMareker(id) {
    if (allFlightsMarker[id]) {
        allFlightsMarker[id].setMap(null);
        delete allFlightsMarker[id];
    }
    if (allFlightsPath[id]) {
        allFlightsPath[id].setMap(null);
    }
    delete allFlightsPath[id];
}

// Update the map by removing markers that theirs flights are not active.
function UnDisplayMarkers() {

    for (let key in allFlightsMarker) {
        if (allFlightsMarker[key]) {
            // If the marker exists in either lists we keep it. otherwise - delete it.
            let foundInInternal = IsFound(key, "#my-flights-list > li");
            let foundInExternal = IsFound(key, "#external-flights-list > li");
            // Flight is not active.
            if (!(foundInInternal || foundInExternal)) {
                RemovwMareker(key);
            }
        }
    }
}