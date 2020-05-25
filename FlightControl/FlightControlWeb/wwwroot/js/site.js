
setInterval(DisplayFlights,2000);

// Global variables.
let isOnTime = {};
const uninitialize = 0;
const oldFlight = 1;
const newFlight = 2;
const deleted = 3;
const ColorRow = "highlightedRow";

// Sleep for ms mili seconds.
async function Sleep(ms) {
    await new Promise(r => setTimeout(r, ms));
}


// Reset flight details.
function ResetFlightDetails() {
    $("#company-name-definition").html("");
    $("#number-of-passangers-definition").html("");
    $("#start-location-definition").html("");
    $("#end-location-definition").html("");
    $("#start-time-definition").html("");
    $("#end-time-definition").html("");
}

// Calculate the end position of a given flight's segments.
function EndLocation(segments) {
    let loc = 0;
    segments.filter(segment => {
        loc = segment.longitude + ", " + segment.latitude;
    });
    return loc;
}

// Calculate the end time.
function EndTime(startTime, segments) {
    let duration = 0;
    // Sum all the timespan seconds of all the segments.
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

// Calculate the start time using a given time.
function StartTime(time) {
    let d = new Date(time);
    let offset = d.getTimezoneOffset();
    let realOS = offset * -1;
    d.setMinutes(d.getMinutes() + realOS);
    let startTime = d.toISOString();
    return startTime;
}

// Highlight the given id's flight row and unhightlight the other rows.
function Highlighted(id, name) {
    let listItems = document.querySelectorAll(name);
    let j = 0;
    let size = listItems.length;
    for (j = 0; j < size; j++) {
        let id1 = listItems[j].getElementsByTagName("span")[2].innerText;
        if (id1 === id) {
            listItems[j].classList.add(ColorRow);
        } else {
            listItems[j].classList.remove(ColorRow);
        }
    }
}

// Send a get request by a given message and display the returned data.
function FlightPlanDetails(message) {
    $.getJSON(message, (flightPlan) => {
        $("#company-name-definition").html(flightPlan.company_name);
        $("#number-of-passangers-definition").html(flightPlan.passengers);
        $("#start-location-definition").html(flightPlan.initial_location.longitude + ", " + flightPlan.initial_location.latitude);
        $("#end-location-definition").html(EndLocation(flightPlan.segments));
        $("#start-time-definition").html(StartTime(flightPlan.initial_location.date_time));
        $("#end-time-definition").html(EndTime(flightPlan.initial_location.date_time, flightPlan.segments));
    }).fail(function (jqXHR) {
        if (jqXHR.status === 404) {
            Alert("Oops! Something Is Wrong. Couldn't Find The Requested FlightPlan while display detailes. Status: 404 Not Found");
        } else {
            Alert("Oops! Something Is Wrong. Couldn't Get The FlightPlan. Status: " + jqXHR.status);
        }
    }); 
}

// Display the details of a given flight.
function DisplayFlightDetails(id, isExternal) {
    if (isExternal) {
        // Find the server that own the flight with the given id.
        $.getJSON("/api/servers/" + id, (server) => {
            let message = "/api/FlightPlan?id=" + id + "&url=" + server.ServerURL;            
             // Display the details of the given flight.
            FlightPlanDetails(message);
        }).fail(function (jqXHR) {
            if (jqXHR.status === 404) {
                Alert("Oops! Something Is Wrong. Couldn't Find The Requested Server. Status: 404 Not Found");
            } else {
                Alert("Oops! Something Is Wrong. Couldn't Get The Sever With Flight Id = " + id + ". Status: " + jqXHR.status);
            }
        }); 
    }
    else {
        FlightPlanDetails("/api/FlightPlan/" + id);
    }
    // Color the row that is pressed and reset the other rows color.
    Highlighted(id, "#my-flights-list > li");
    Highlighted(id, "#external-flights-list > li");  
}

// Show an error message.
function Alert(message) {

    $('#error_message').html(message);
    $('#error').show();
}

// Handeling X button click.
function DeleteFlight(id, self) {

    // If the given row is highlighted.
    if (self.parent().hasClass(ColorRow)) {
        self.parent().removeClass(ColorRow);
        ResetFlightDetails();
        ResetFlights(); 
    }
        // Delete the flight.
        self.parent().fadeOut(600, function () { $(this).remove(); });
        $.ajax({
            url: "/api/Flights/" + id,
            type: "DELETE",
            success: function (result) {
                RemoveRow(id);
                // Remove the matching marker from the map.
                RemovwMareker(id);
                isOnTime[id] = deleted;
            }
        }).fail(function (jqXHR) {
            if (jqXHR.status === 404) {
                Alert("Oops! Something Is Wrong. Couldn't Delete The Requested Flight. Status: 404 Not Found");
            } else {
                Alert("Oops! Something Is Wrong. Couldn't Delete The Flight With Id = " + id + ". Status: " + jqXHR.status);
            }
        }); 
}

// Check if a given id's flight is in a given list.
function IsFound(id, nameOfList) {
    
    let listItems = document.querySelectorAll(nameOfList);
    let j = 0;
    let size = listItems.length;
    let found = false;
    for (j = 0; j < size; j++) {
         // Get the id of the current flight row.
        let id1 = listItems[j].getElementsByTagName("span")[2].innerText;
        if (id1 === id) {
            found = true;
            break;
        }
    }
    return found;
}

// Reset the isOnTime dictionary.
function ResetDictionaryOnTime() {
    for (let key in isOnTime) {
        if (isOnTime[key] !== deleted) {
            isOnTime[key] = uninitialize;
        }
    }
}

// Get a row in a list by a given id.
function GetRow(id) {

    let listItems = document.querySelectorAll("#my-flights-list > li");
    let j = 0;
    let size = listItems.length;
    for (j = 0; j < size; j++) {
        // Get the id of the current flight row.
        let id1 = listItems[j].getElementsByTagName("span")[2].innerText;
        if (id1 === id) {
            return listItems[j];
        }
    }

    listItems = document.querySelectorAll("#external-flights-list > li");
    j = 0;
    size = listItems.length;
    for (j = 0; j < size; j++) {
         // Get the id of the current flight row.
        let id1 = listItems[j].getElementsByTagName("span")[2].innerText;
        if (id1 === id) {
            return listItems[j];
        }
    }
}

// Remove a row from the list.
function RemoveRow(key) {
    let row = GetRow(key);
    if (row) {
        // If the row of the flight was highlighted - it's details was in the display details.
        if (row.classList.contains(ColorRow)) {
            ResetFlightDetails();
        }
        row.remove();
    }
    isOnTime[key] = deleted;
}

// Remove a deleted flight from the list.
function RemoveFromFlightList() {

    for (let key in isOnTime) {
        // If we did not get this flight in the previous get requset.
        if (isOnTime[key] === uninitialize) {
            RemoveRow(key);
        }
    }
}

// Display a given flight in the external flight list.
function DisplayExternal(flight) {

    isOnTime[flight.flight_id] = newFlight;
    let firstColunm = $("<span class="+ "space"+">").text("X");
    let newflightCompanyName = $("<span class=" + "flight-company" + ">").text(flight.company_name);
    let newflightId = $("<span class=" + "flight-id" + ">").text(flight.flight_id);
    let fakeSpan = $("<span class=" + "space" + ">").text("XXXXXXXXXXX");
    // Add the flight to the list.
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
    // Add a new marker on the map.
     AddMarker(flight);
}

// Display a given flight in the internal flight list.
function DisplayInternal(flight) {

    if (isOnTime[flight.flight_id] === deleted) {
        return;
    }
    isOnTime[flight.flight_id] = newFlight;
    let flightDelete = $("<span class=" + "flight-delete" + ">").text("X");
    let newflightCompanyName = $("<span class=" + "flight-company" + ">").text(flight.company_name);
    let newflightId = $("<span class=" + "flight-id" + ">").text(flight.flight_id);
    let fakeSpan = $("<span class=" + "space" + ">").text("XXXXXXXXXXX");
    // Add the flight to the list.
    $("<li class=" + "d-flex my-flights-list-item" + "> ").append(
        flightDelete,
        newflightCompanyName,
        newflightId,
        fakeSpan).appendTo("#my-flights-list");


    newflightId.on("click", function () {
        DisplayFlightDetails(flight.flight_id, flight.is_external);
        DisplayPath(flight.flight_id);
    });
    newflightCompanyName.on("click", function () {
        DisplayFlightDetails(flight.flight_id, flight.is_external);
        DisplayPath(flight.flight_id);
    });
    flightDelete.on("click", function () {
        DeleteFlight(flight.flight_id, $(this));
    });
    fakeSpan.on("click", function () {
        DisplayFlightDetails(flight.flight_id, flight.is_external);
        DisplayPath(flight.flight_id);
    });
    // Add a new marker on the map.
    AddMarker(flight);
}

// Create a row in a list.
function RowInMyFlightList(flight) {
    let foundInInternal = IsFound(flight.flight_id, "#my-flights-list > li");
    let foundInExternal = IsFound(flight.flight_id, "#external-flights-list > li");

     // The given flight is a new flight.
    if (!(foundInInternal || foundInExternal)) {
        if (flight.is_external) {
            DisplayExternal(flight);
        } else {
           DisplayInternal(flight);
        }        
    }
    // The given flight is already in one of the lists.
    else {
        isOnTime[flight.flight_id] = oldFlight;
    }
    // Set the position of the flight.
    SetNewPosition(flight);   
}

// Display all the flights in the lists.
function DisplayFlights() {
    // Gets the current date in the requierd format.
    let date = new Date().toISOString();
    let curdate = date.split(".")[0] + "Z";

    // Get all active flights.
    $.getJSON("/api/Flights?relative_to=" + curdate + "&sync_all", (data) => {
        ResetDictionaryOnTime();
        // filter = iterates an array, flight is the item itself
        data.filter(flight => {
            // Create a row in one of the lists.
            RowInMyFlightList(flight);
        });
        // Update the lists by removing flights that are not active.
        RemoveFromFlightList();
        // Update the map by removing markers that theirs flights are not active.
        UnDisplayMarkers();

    }).fail(function (jqXHR) {
        if (jqXHR.status === 404) {
            Alert("404 Not Found");
        }else {
            Alert("Oops! Something Is Wrong. Couldn't Get All Flights Properly. Status: " + jqXHR.status);
        }
    });     
}

$('.alert .close').on('click', function (e) {
    $(this).parent().hide();
});

DisplayFlights();

