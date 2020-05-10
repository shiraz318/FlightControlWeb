setInterval(DisplayFlights, 1000);
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
    let d = new Date(startTime);
    d.setSeconds(d.getSeconds() + duration);
    let offset = d.getTimezoneOffset();
    let realOS = offset * -1; 
    d.setMinutes(d.getMinutes() + realOS);
    let endTime = d.toISOString();
    return endTime;
}

function StartTime(time) {
    let d = new Date(time);
    let offset = d.getTimezoneOffset();
    let realOS = offset * -1;
    d.setMinutes(d.getMinutes() + realOS);
    let startTime = d.toISOString();
    return startTime;
}

function DisplayFlightDetails(id, self) {

    $.getJSON("/api/FlightPlan/" + id, (data) => {
        $("#company-name-definition").html(data.company_name);
        $("#number-of-passangers-definition").html(data.passengers);
        $("#start-location-definition").html(data.initial_location.longitude + ", " + data.initial_location.latitude);
        $("#end-location-definition").html(EndLocation(data.segments));
        $("#start-time-definition").html(StartTime(data.initial_location.date_time));
        $("#end-time-definition").html(EndTime(data.initial_location.date_time, data.segments));

    });
    self.parent().toggleClass("highlighted");
    self.parent().siblings().removeClass("highlighted");
  
};



function DeleteFlight(id, self) {
    if (self.parent().hasClass("highlighted")) {
        console.log(1);
        self.parent().removeClass("highlighted");
    } else {
        self.parent().fadeOut(600, function () { $(this).remove(); });
        $.ajax({
            url: '/api/Flights/' + id,
            type: 'DELETE',
            success: function (result) {
                console.log("DELETED");
            }
        });
    }
}

function RowInMyFlightList(flight) {
    var listItems = document.querySelectorAll('#my-flights-table > li');

    let j = 0;
    let size = listItems.length;
    let found = false;
    for (j = 0; j < size; j++) {
        let id = listItems[j].getElementsByTagName("span")[2].innerText;
        if (id == flight.flight_id) {
            found = true;
            break;
        }
    }
    if (!found) {

        let flightDelete = $('<span class="flight-delete">').text('X');
        let newflightCompanyName = $('<span class="flight-company">').text(flight.company_name);
        let newflightId = $('<span class="flight-id">').text(flight.flight_id);
        $("<li class='d-flex my-flights-list-item'> ").append(
            flightDelete,
            newflightCompanyName,
            newflightId).appendTo('#my-flights-table');

        newflightId.on("click", function () {
            DisplayFlightDetails(flight.flight_id, $(this));
        });
        newflightCompanyName.on("click", function () {
            DisplayFlightDetails(flight.flight_id, $(this));
        });
        flightDelete.on("click", function () {

            DeleteFlight(flight.flight_id, $(this));
        });
        addMarker(flight, $(this));
    }
}

function DisplayFlights() {
    let date = new Date().toISOString();
    //console.log(date)
    $.getJSON("/api/Flights?relative_to=<2014-08-07T17:24:20Z>", (data) => {
        // filter=iterates an array, flight is the item itself
        data.filter(flight => {
            RowInMyFlightList(flight);
        });
    });
}
DisplayFlights();


