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

function DisplayFlightDetails(id, self) {

    $.getJSON("/api/FlightPlan/" + id, (data) => {
        $("#company-name-definition").html(data.company_name);
        $("#number-of-passangers-definition").html(data.passengers);
        $("#start-location-definition").html(data.initial_location.longitude + ", " + data.initial_location.latitude);
        $("#end-location-definition").html(EndLocation(data.segments));
        $("#start-time-definition").html(StartTime(data.initial_location.date_time));
        $("#end-time-definition").html(EndTime(data.initial_location.date_time, data.segments));

    });
    Highlighted(id, "#my-flights-list > li");
    Highlighted(id, "#external-flights-list > li");  
}

function DeleteFlight(id, self) {
    if (self.parent().hasClass(ColorRow)) {
        self.parent().removeClass(ColorRow);
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
        //console.log("reset" + isOnTime[key]);
    }
}

function GetRow(id) {
    let listItems = document.querySelectorAll("#my-flights-list > li");
    let j = 0;
    let size = listItems.length;
    for (j = 0; j < size; j++) {
        let id1 = listItems[j].getElementsByTagName("span")[2].innerText;
        if (id1 === id) {
            return listItems[j];
        }
    }

    listItems = document.querySelectorAll("#external-flights-list > li");
    j = 0;
    size = listItems.length;
    for (j = 0; j < size; j++) {
        let id1 = listItems[j].getElementsByTagName("span")[2].innerText;
        if (id1 === id) {
            return listItems[j];
        }
    }
}

function RemoveFromFlightList() {
    //console.log("I am in remove from flight lists");
    //console.log(isOnTime);
    for (let key in isOnTime) {
        if (isOnTime[key] === uninitialize) {
            //console.log("key: " + key);
            let row = GetRow(key);
            //console.log("row: " + row);
            if (row.classList.contains(ColorRow)) {
                ResetFlightDetails();
            }
            row.remove();
            delete isOnTime[key];    
        }
    }
}

function DisplayExternal(flight) {
    isOnTime[flight.flight_id] = newFlight;
    let firstColunm = $("<span>").text("  ");
    let newflightCompanyName = $("<span class=" + "flight-company" + ">").text(flight.company_name);
    let newflightId = $("<span class=" + "flight-id" + ">").text(flight.flight_id);
    $("<li class=" + "d-flex my-flights-list-item" + "> ").append(
        firstColunm,
        newflightCompanyName,
        newflightId).appendTo("#external-flights-list");


    newflightId.on("click", function () {
        DisplayFlightDetails(flight.flight_id, $(this));
        DisplayPath(flight.flight_id);
    });
    newflightCompanyName.on("click", function () {
        DisplayFlightDetails(flight.flight_id, $(this));
        DisplayPath(flight.flight_id);
    });
    firstColunm.on("click", function () {
        DisplayFlightDetails(flight.flight_id, $(this));
        DisplayPath(flight.flight_id);
    });
    console.log(flight.flight_id);
    addMarker(flight);
}

function DisplayInternal(flight) {
    isOnTime[flight.flight_id] = newFlight;
    let flightDelete = $("<span class=" + "flight-delete" + ">").text("X");
    let newflightCompanyName = $("<span class=" + "flight-company" + ">").text(flight.company_name);
    let newflightId = $("<span class=" + "flight-id" + ">").text(flight.flight_id);
    $("<li class=" + "d-flex my-flights-list-item" + "> ").append(
        flightDelete,
        newflightCompanyName,
        newflightId).appendTo("#my-flights-list");


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
    else {
        isOnTime[flight.flight_id] = oldFlight;
    }
    SetNewPosition(flight);   
}

function DisplayFlights() {
    let date = new Date().toISOString();
    let curdate = date.split(".")[0] + "Z";
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



