function EndLocation(segments) {
    let loc = 3;
    segments.filter(segment => {
       loc = segment.longitude + ", " + segment.latitude;
    });
    return loc;
}

function EndTime(startTime, segments) {
    let duration = 0;
    segments.filter(segment => {
        duration += segment.timespan_seconds;
    });
    console.log(duration);
    let d = new Date(startTime);
    console.log(d);
    d.setSeconds(d.getSeconds() + duration);
    let endTime = d.toISOString();
    return endTime;
}

function StartTime(time) {
    let d = new Date(time);
    let startTime = d.toISOString();
    return startTime;
}

function DisplayFlightDetails(id) {

    $.getJSON("/api/FlightPlan/" + id, (data) => {
        $("#company-name-definition").html(data.company_name);
        $("#number-of-passangers-definition").html(data.passengers);
        $("#start-location-definition").html(data.initial_location.longitude + ", " + data.initial_location.latitude);
        $("#end-location-definition").html(EndLocation(data.segments));
        $("#start-time-definition").html(StartTime(data.initial_location.date_time));
        $("#end-time-definition").html(EndTime(data.initial_location.date_time, data.segments));
        
    });
    $('#my-flights-table tr').each(function (i, item) {
        var result = $(item).find('td').last().text();
        console.log(result);
        if (result == id) {
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

function DisplayFlights() {
    //let d = new Date();
    let date = new Date().toISOString();
    console.log(date)
    $.getJSON("/api/Flights?relative_to=<2014-08-07T14:24:20Z>", (data) => {
        // filter=iterates an array, flight is the item itself
        data.filter(flight => {
            let flightDelete = $('<td>').text('X');
            let newflightCompanyName = $('<td>').text(flight.company_name);
            let newflightId = $('<td>').text(flight.flight_id);
            $("<tr class='d-flex'> ").append(
                flightDelete,
                newflightCompanyName,
                newflightId).appendTo('#my-flights-table');

            newflightId.on("click", function () {
                DisplayFlightDetails(flight.flight_id);
            });
            newflightCompanyName.on("click", function () {
                DisplayFlightDetails(flight.flight_id);
            });
            flightDelete.on("click", function () {
                $(this).parents('tr').remove();
                DeleteFlight(flight.flight_id);
            });
        });
    });
}
DisplayFlights();


