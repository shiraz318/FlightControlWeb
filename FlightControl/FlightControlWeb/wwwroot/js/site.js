setInterval(DisplayFlights, 1000);
let isOnTime = {};
const uninitialize = 0;
const oldFlight = 1;
const newFlight = 2;
const ColorRow = "highlightedRow";
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

function Highlighted(id, name) {
    console.log(name);
    let listItems = document.querySelectorAll(name);
    let j = 0;
    let size = listItems.length;
    for (j = 0; j < size; j++) {
        let id1 = listItems[j].getElementsByTagName("span")[2].innerText;
        if (id1 === id) {
            console.log("be green");
            listItems[j].classList.add(ColorRow);
            //listItems[j].classList.add("shiraz");

        } else {
            listItems[j].classList.remove(ColorRow);
        }
    }
}



function FlightPlanDetails(message) {
    $.getJSON(message, (data) => {
        console.log(data);
        $("#company-name-definition").html(data.company_name);
        $("#number-of-passangers-definition").html(data.passengers);
        $("#start-location-definition").html(data.initial_location.longitude + ", " + data.initial_location.latitude);
        $("#end-location-definition").html(EndLocation(data.segments));
        $("#start-time-definition").html(StartTime(data.initial_location.date_time));
        $("#end-time-definition").html(EndTime(data.initial_location.date_time, data.segments));
    });

}

function DisplayFlightDetails(id, isExternal) {
    console.log("in displayflight detail");
    if (isExternal) {
        // find the server that own the flight with the given id.
        $.getJSON("/api/servers/" + id, (server) => {
            let message = "/api/FlightPlan?id=" + id + "&url=" + server.ServerURL;            
            FlightPlanDetails(message);
        });
    }
    else {
        FlightPlanDetails("/api/FlightPlan/" + id);
    }
    // Color the row that is pressed and reset the other rows color.
    Highlighted(id, "#my-flights-list > li");
    Highlighted(id, "#external-flights-list > li");  
}

function DeleteFlight(id, self) {
    if (self.parent().hasClass(ColorRow)) {
        self.parent().removeClass(ColorRow);
        ResetFlightDetails();
    } else {
        self.parent().fadeOut(600, function () { $(this).remove(); });
        $.ajax({
            url: "/api/Flights/" + id,
            type: "DELETE",
            success: function (result) {
                console.log("DELETED");
            }
        });
        RemovwMareker(id);
    }
}

function IsFound(id, nameOfList) {
    
    let listItems = document.querySelectorAll(nameOfList);
    let j = 0;
    let size = listItems.length;
    let found = false;
    for (j = 0; j < size; j++) {
        let id1 = listItems[j].getElementsByTagName("span")[2].innerText;
        if (id1 === id) {
            found = true;
            break;
        }
    }
    return found;
}

function ResetDictionaryOnTime() {
    for (let key in isOnTime) {
        isOnTime[key] = uninitialize;
    }
}

function GetRow(id) {
    let listItems = document.querySelectorAll("#my-flights-list > li");
    let j = 0;
    let size = listItems.length;
    for (j = 0; j < size; j++) {
        let id1 = listItems[j].getElementsByTagName("span")[2].innerText;
        console.log(id1);
        console.log("need to find: ");
        console.log(id);
        if (id1 === id) {
            console.log("return row1");
            return listItems[j];
        }
    }

    listItems = document.querySelectorAll("#external-flights-list > li");
    j = 0;
    size = listItems.length;
    for (j = 0; j < size; j++) {
        let id1 = listItems[j].getElementsByTagName("span")[2].innerText;
        if (id1 === id) {
            console.log("return row2");
            return listItems[j];
        }
    }
}

function RemoveFromFlightList() {

    for (let key in isOnTime) {
        // If we did not get this flight in the previous get requset.
        if (isOnTime[key] === uninitialize) {
            let row = GetRow(key);
            if (row) {
                // If the row of the flight was highlighted - it's details was in the display details.
                if (row.classList.contains(ColorRow)) {
                    ResetFlightDetails();
                }
                row.remove();
                delete isOnTime[key];
            }
        }
    }
}

function DisplayExternal(flight) {

    isOnTime[flight.flight_id] = newFlight;
    let firstColunm = $("<span class="+ "space"+">").text("X");
    let newflightCompanyName = $("<span class=" + "flight-company" + ">").text(flight.company_name);
    let newflightId = $("<span class=" + "flight-id" + ">").text(flight.flight_id);
    let fakeSpan = $("<span class=" + "space" + ">").text("XXXXXXXXXXX");
    $("<li class=" + "d-flex my-flights-list-item" + "> ").append(
        firstColunm,
        newflightCompanyName,
        newflightId,
        fakeSpan).appendTo("#external-flights-list");


    newflightId.on("click", function () {
        DisplayFlightDetails(flight.flight_id, flight.is_external);
        DisplayPath(flight.flight_id);
    });
    newflightCompanyName.on("click", function () {
        DisplayFlightDetails(flight.flight_id, flight.is_external);
        DisplayPath(flight.flight_id);
    });
    firstColunm.on("click", function () {
        DisplayFlightDetails(flight.flight_id, flight.is_external);
        DisplayPath(flight.flight_id);
    });
    fakeSpan.on("click", function () {
        DisplayFlightDetails(flight.flight_id, flight.is_external);
        DisplayPath(flight.flight_id);
    });
    addMarker(flight);
}

function DisplayInternal(flight) {

    isOnTime[flight.flight_id] = newFlight;
    let flightDelete = $("<span class=" + "flight-delete" + ">").text("X");
    let newflightCompanyName = $("<span class=" + "flight-company" + ">").text(flight.company_name);
    let newflightId = $("<span class=" + "flight-id" + ">").text(flight.flight_id);
    let fakeSpan = $("<span class=" + "space" + ">").text("XXXXXXXXXXX");
    $("<li class=" + "d-flex my-flights-list-item" + "> ").append(
        flightDelete,
        newflightCompanyName,
        newflightId,
        fakeSpan).appendTo("#my-flights-list");


    newflightId.on("click", function () {
        console.log("click");
        DisplayFlightDetails(flight.flight_id, flight.is_external);
        DisplayPath(flight.flight_id);
    });
    newflightCompanyName.on("click", function () {
        console.log("click");
        DisplayFlightDetails(flight.flight_id, flight.is_external);
        DisplayPath(flight.flight_id);
    });
    flightDelete.on("click", function () {
        DeleteFlight(flight.flight_id, $(this));
        ResetFlights();   
        
    });
    fakeSpan.on("click", function () {
        DisplayFlightDetails(flight.flight_id, flight.is_external);
        DisplayPath(flight.flight_id);
    });
    addMarker(flight);
}

function RowInMyFlightList(flight) {
    let foundInInternal = IsFound(flight.flight_id, "#my-flights-list > li");
    let foundInExternal = IsFound(flight.flight_id, "#external-flights-list > li");
    
    if (!(foundInInternal || foundInExternal)) {
        console.log(flight.is_external);

        if (flight.is_external) {
            DisplayExternal(flight);
        } else {
            DisplayInternal(flight);
        }        
    }
    // The id is not found in external and in internal.
    else {
        isOnTime[flight.flight_id] = oldFlight;
    }
    // Set the position of the flight.
    SetNewPosition(flight);   
}

function DisplayFlights() {
    let date = new Date().toISOString();
    let curdate = date.split(".")[0] + "Z";
    //$.getJSON("/api/Flights?relative_to=" + curdate, (data) => {
    $.getJSON("/api/Flights?relative_to=" + curdate + "&sync_all", (data) => {
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



