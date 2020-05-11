setInterval(DisplayFlights, 1000);
let isOnTime = {};
const uninitialize = 0;
const oldFlight = 1;
const newFlight = 2;

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

    let listItems = document.querySelectorAll('#my-flights-table > li');
    let j = 0;
    let size = listItems.length;
    let found = false;
    let row = listItems[0];
    for (j = 0; j < size; j++) {
        let id1 = listItems[j].getElementsByTagName("span")[2].innerText;
        if (id1 == id) {
            listItems[j].classList.add("highlighted");

        } else {
            listItems[j].classList.remove("highlighted");
        }
    }
  
};



function DeleteFlight(id, self) {
    if (self.parent().hasClass("highlighted")) {
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
        RemovwMareker(id);
    }
}

function IsFound(id) {
    let listItems = document.querySelectorAll('#my-flights-table > li');
    let j = 0;
    let size = listItems.length;
    let found = false;
    for (j = 0; j < size; j++) {
        let id1 = listItems[j].getElementsByTagName("span")[2].innerText;
        if (id1 == id) {
            found = true;
            break;
        }
    }
    return found;
}

function ResetDictionaryOnTime() {
    for (let key in isOnTime) {
        isOnTime[key] = uninitialize;
        //console.log("reset" + isOnTime[key]);
    }
}

function GetRow(id) {
    let listItems = document.querySelectorAll('#my-flights-table > li');
    let j = 0;
    let size = listItems.length;
    for (j = 0; j < size; j++) {
        let id1 = listItems[j].getElementsByTagName("span")[2].innerText;
        if (id1 == id) {
            return listItems[j];
        }
    }
}


function RemoveFromFlightList() {
    //console.log("I am in remove from flight lists");
    //console.log(isOnTime);
    for (let key in isOnTime) {
        if ((isOnTime[key] == uninitialize)) {
            //console.log("key: " + key);
            let row = GetRow(key);
            //console.log("row: " + row);
            row.remove();
            delete isOnTime[key];

            
        }
    }
}

function RowInMyFlightList(flight) {
  
    let found = IsFound(flight.flight_id);
    
    if (!found) {
        isOnTime[flight.flight_id] = newFlight;
        let flightDelete = $('<span class="flight-delete">').text('X');
        let newflightCompanyName = $('<span class="flight-company">').text(flight.company_name);
        let newflightId = $('<span class="flight-id">').text(flight.flight_id);
        $("<li class='d-flex my-flights-list-item'> ").append(
            flightDelete,
            newflightCompanyName,
            newflightId).appendTo('#my-flights-table');


        newflightId.on("click", function () {
            DisplayFlightDetails(flight.flight_id, $(this));
            DisplayPath(flight.flight_id);
        });
        newflightCompanyName.on("click", function () {
            DisplayFlightDetails(flight.flight_id, $(this));
            DisplayPath(flight.flight_id);
        });
        flightDelete.on("click", function () {
            DeleteFlight(flight.flight_id, $(this));
            Reset();
        });
        console.log(flight.flight_id);
        addMarker(flight);
    }
    else {
        isOnTime[flight.flight_id] = oldFlight;
    }
    SetNewPosition(flight);   
}


function DisplayFlights() {
    let date = new Date().toISOString();
    let curdate = date.split(".")[0] + "Z";
    $.getJSON("/api/Flights?relative_to=" + curdate, (data) => {
        // filter=iterates an array, flight is the item itself
        ResetDictionaryOnTime();
        data.filter(flight => {
            RowInMyFlightList(flight);
        });
        RemoveFromFlightList();
        UnDisplayMarkers();
    }); 
   
}
DisplayFlights();



