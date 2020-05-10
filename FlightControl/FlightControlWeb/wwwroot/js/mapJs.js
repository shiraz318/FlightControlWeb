let map;
let allFlightsPath = {};
let allFlightsMarker = {};
const regularImg = '../images/small_black.png'
const clickedImg = '../images/airplane.png'
function ResetFlightDetails() {
    $("#company-name-definition").html("");
    $("#number-of-passangers-definition").html("");
    $("#start-location-definition").html("");
    $("#end-location-definition").html("");
    $("#start-time-definition").html("");
    $("#end-time-definition").html("");
}

function Reset() {
    for (let key in allFlightsPath) {
        allFlightsPath[key].setMap(null);
        var icon = { url: regularImg };
        allFlightsMarker[key].setIcon(icon);
    }
    let listItems = document.querySelectorAll('#my-flights-table > li');
    let i = 0;
    let size = listItems.length;
    for (i = 0; i < size; i++) {
        listItems[i].classList.remove("highlighted");
    }
    ResetFlightDetails();
}

function initMap() { 
    // Map options.
    let options = {
        zoom: 1,
        center: {lat:42.3601, lng:-71.0589}
    }
    // New map.
    map = new google.maps.Map(document.getElementById('map'), options);
    google.maps.event.addListener(map, 'click', Reset);
}

function DisplayPath(id) {

    for (let key in allFlightsPath) {
        if (key == id) {
            allFlightsPath[key].setMap(map);
            var icon = { url: clickedImg };
            allFlightsMarker[key].setIcon(icon);
        } else {
            allFlightsPath[key].setMap(null);
            var icon = { url: regularImg };
            allFlightsMarker[key].setIcon(icon);
        }
    }
   
}

function CreatePath(id) {
    $.getJSON("/api/FlightPlan/" + id, (data) => {
        let startLoc = new google.maps.LatLng(data.initial_location.latitude, data.initial_location.longitude);
        let flightPlanCoordinates = [startLoc];
        data.segments.filter(segment => {
            let loc = new google.maps.LatLng(segment.latitude, segment.longitude);
            flightPlanCoordinates.push(loc);
        });
        let flightPath = new google.maps.Polyline({
            path: flightPlanCoordinates,
            geodesic: true,
            strokeColor: '#FF0000',
            strokeOpacity: 1.0,
            strokeWeight: 2
        });
       
        allFlightsPath[id] = flightPath;
    });
}

function addMarker(flight) {  
    let marker = new google.maps.Marker({
        position: new google.maps.LatLng(flight.latitude, flight.longitude),
        map: map,
        icon: regularImg
    });
    allFlightsMarker[flight.flight_id] = marker;
    let infoWindow = new google.maps.InfoWindow({
        content: '<h3>' + flight.flight_id + '</h3>'
    });

    CreatePath(flight.flight_id);

    marker.addListener('click', function () {
        //console.log($(this));
       
        infoWindow.open(map, marker);
        DisplayFlightDetails(flight.flight_id);
        DisplayPath(flight.flight_id);
    });

}
function SetNewPosition(flight) {
    if (allFlightsMarker[flight.flight_id]) {
        allFlightsMarker[flight.flight_id].setPosition(new google.maps.LatLng(flight.latitude, flight.longitude));
    }
}

