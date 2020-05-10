function initMap() {
 
    // Map options.
    let options = {
        zoom: 8,
        center: {lat:42.3601, lng:-71.0589}
    }

    // New map.
    let map = new google.maps.Map(document.getElementById('map'), options);
    return map;
    // Add marker
    /*let image = "../images/blue_airplane.jpg";
    let marker = new google.maps.Marker({
        position: { lat: 42.4668, lng: -70.9495 },
        map: map,
        //icon: "../images/red_airplane.jpg"
    });

    let infoWindow = new google.maps.InfoWindow({
        content: '<h3>Shiraz Berger</h3>'
    });

    marker.addListener('click', function () {
        infoWindow.open(map, marker);
    });
    */
    //addMarker({
    //    coords: { lat: 42.4668, lng: -70.9495 },
    //    iconImage: ''
    //});
    //addMarker({ coords: { lat: 42.8584, lng: -70.9300 } });

    // Add marker function.
    

    flightPath.setMap(map);
}
function addMarker(flight, self) {
    let map = initMap();
    console.log(flight.latitude);
    let marker = new google.maps.Marker({
        position: { lat: 42.8584, lng: -70.9300 },//{ lat:  + flight.latitude + , lng: + flight.longitude + },
        map: map
        //icon: "../images/red_airplane.jpg"
    });
    let infoWindow = new google.maps.InfoWindow({
        content: '<h3>' + flight.flight_id + '</h3>'
    });

    marker.addListener('click', function () {
        infoWindow.open(map, marker);
        DisplayFlightDetails(flight_id, self);
    });
}
//var flightPlanCoordinates = [
//    { lat: 37.772, lng: -122.214 },
//    { lat: 21.291, lng: -157.821 },
//    { lat: -18.142, lng: 178.431 },
//    { lat: -27.467, lng: 153.027 }
//];
//var flightPath = new google.maps.Polyline({
//    path: flightPlanCoordinates,
//    geodesic: true,
//    strokeColor: '#FF0000',
//    strokeOpacity: 1.0,
//    strokeWeight: 2
//});


