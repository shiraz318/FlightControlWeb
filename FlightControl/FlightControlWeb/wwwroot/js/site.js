﻿// Display My-Flights list
// go to the json file and put the information in data

function DisplayFlightDetails() {
    // this=the clicked item
    let flightid = $(this).html();
    console.log(flightid);
    // Display Flight details according to selected flight id
    $.getJSON("/api/FlightPlan/" + flightid, (data) => {
        $("#company-name-definition").html(data.company_name);
        $("#number-of-passangers-definition").html(data.passengers);
        $("#start-location-definition").html(data.initial_location.longitude + ", " + data.initial_location.latitude);   
    });
};

function DeleteFlight() {
    $.ajax({
        url: '/api/Flights/CT54263cM',
        type: 'DELETE',
        success: function (result) {
            console.log("DELETED");
        }
    });
}
//$.getJSON("/api/FlightPlan", (data) => {
//    // filter=iterates an array, flight is the item itself
//   data.filter(flight => {
//    // create an item in the list with the flight id
//    let newflight = $("<li class='my-flight-list-item'>" + flight.id + "</li>");
//    $("#my-flights-list").append(newflight);
//    // when clicked go the function
//    newflight.on("click", DisplayFlightDetails);
//    });
//});


$.getJSON("/api/FlightPlan", (data) => {
    // filter=iterates an array, flight is the item itself
    data.filter(flight => {
        let flightDelete = $('<td>').text('X');
        let newflightId = $('<td>').text(flight.id);
        let newflightCompanyName = $('<td>').text(flight.company_name);
        $('<tr>').append(
            flightDelete,
            newflightId,
            newflightCompanyName).appendTo('#my-flights-table');
        // create an item in the list with the flight id
        //let newflight = $("<li class='my-flight-table-item'>" + flight.id + "</li>");
        //$("#my-flights-table").append(newflight);
        // when clicked go the function
        //newflightId.on("click", DisplayFlightDetails);
        flightDelete.on("click", DeleteFlight);
    });
});


