//function DisplayFlightDetails() {
//    // this=the clicked item
//    let flightid = $(this).html();
//    console.log(flightid);
//    // Display Flight details according to selected flight id
//    $.getJSON("/js/flightDetails.json", (data) => {
//        data.filter(flight => {
//            if (flight.flight_id == flightid) {
//                console.log(flight.company_name);
//                $("#company-name-definition").html(flight.company_name);
//                $("#number-of-passangers-definition").html(flight.passengers);
//                $("#start-location-definition").html(flight.longitude + ", " + flight.latitude);
//            }
//        });
//    });
//};



// Display My-Flights list
// go to the json file and put the information in data

function DisplayFlightDetails() {
    // this=the clicked item
    let flightid = $(this).html();
    console.log(flightid);
    // Display Flight details according to selected flight id
    $.getJSON("/api/FlightPlan/" + flightid, (data) => {
       
                console.log(data.company_name);
                $("#company-name-definition").html(data.company_name);
                $("#number-of-passangers-definition").html(data.passengers);
        $("#start-location-definition").html(data.initial_location.longitude + ", " + data.initial_location.latitude);
          
    });
};
$.getJSON("/api/FlightPlan", (data) => {
    // filter=iterates an array, flight is the item itself
   data.filter(flight => {
    // create an item in the list with the flight id
    let newflight = $("<li class='my-flight-list-item'>" + flight.id + "</li>");
        $("#my-flights-list").append(newflight);
        // when clicked go the function
        newflight.on("click", DisplayFlightDetails);
    });
});


