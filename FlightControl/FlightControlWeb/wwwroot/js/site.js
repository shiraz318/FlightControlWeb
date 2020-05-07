// Display My-Flights list
// go to the json file and put the information in data

function DisplayFlightDetails() {

    
    // this=the clicked item
    let flightid = $(this).html();
    //console.log(flightid);
    // Display Flight details according to selected flight id
    $.getJSON("/api/FlightPlan/" + flightid, (data) => {
        $("#company-name-definition").html(data.company_name);
        $("#number-of-passangers-definition").html(data.passengers);
        $("#start-location-definition").html(data.initial_location.longitude + ", " + data.initial_location.latitude);   
    });
    $('#my-flights-table tr').each(function (i, item) {
        var result = $(item).find('td').last().text();
        console.log(result);
        if (result == flightid) {
            $(item).css('background-color', 'palegreen');
            console.log("CHANGE");
        } else {
            $(item).css('background-color', 'white');
        }
      
    })
};


function DeleteFlight(id) {
    $.ajax({
        url: '/api/Flights/' + id,
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

$.getJSON("/api/Flights?relative_to=<2014-08-07T14:24:20Z>", (data) => {
    console.log(Date.now());
    // filter=iterates an array, flight is the item itself
    data.filter(flight => {
        let flightDelete = $('<td>').text('X');
        let newflightCompanyName = $('<td>').text(flight.company_name);
        let newflightId = $('<td>').text(flight.flight_id);
        $("<tr class='d-flex'> ").append(
            flightDelete,
            newflightCompanyName,
           newflightId).appendTo('#my-flights-table');
        // create an item in the list with the flight id
        //let newflight = $("<li class='my-flight-table-item'>" + flight.id + "</li>");
        //$("#my-flights-table").append(newflight);
        // when clicked go the function
        newflightId.on("click", DisplayFlightDetails);
        flightDelete.on("click", function () {
            $(this).parents('tr').remove();
            DeleteFlight(flight.id);
        });
    });
});


