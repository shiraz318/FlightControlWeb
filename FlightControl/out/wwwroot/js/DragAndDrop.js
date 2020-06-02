/* global Alert */

let dropArea = document.getElementById("drop-area");

["dragenter", "dragover", "dragleave", "drop"].forEach(eventName => {
    dropArea.addEventListener(eventName, preventDefaults, false);
});

function preventDefaults(e) {
    e.preventDefault();
    e.stopPropagation();
}

["dragenter", "dragover"].forEach(eventName => {
    dropArea.addEventListener(eventName, Highlight, false);
})

    ;["dragleave", "drop"].forEach(eventName => {
        dropArea.addEventListener(eventName, Unhighlight, false);
    });


function Highlight() {
    dropArea.classList.add("highlight");
    let listItems = document.querySelectorAll("#my-flights-list > li");
    let i = 0;
    let size = listItems.length;
    for (i = 0; i < size; i++) {
        let spanX = listItems[i].getElementsByTagName("span")[0];
        spanX.classList.add("disappear");
        listItems[i].classList.add("disappear");
    }
}


function Unhighlight() {
    dropArea.classList.remove("highlight");
    let listItems = document.querySelectorAll("#my-flights-list > li");
    let i = 0;
    let size = listItems.length;
    for (i = 0; i < size; i++) {
        let spanX = listItems[i].getElementsByTagName("span")[0];
        spanX.classList.remove("disappear");
        listItems[i].classList.remove("disappear");
    }
}
dropArea.addEventListener("drop", HandleDrop, false);

// Drop file event.
function HandleDrop(e) {
    let dt = e.dataTransfer;
    let files = dt.files;
    HandleFiles(files);
}


function HandleFiles(files) {
    [...files].forEach(UploadFile);
}


// Upload a file to the server.
function UploadFile(file) {

    var url = "/api/FlightPlan";
    var xhr = new XMLHttpRequest();
    var formData = new FormData();
    let reader = new FileReader();
    reader.readAsText(file);
    reader.onload = function () {
        xhr.open("POST", url, true);
        xhr.setRequestHeader("Content-type", "application/json;charset=UTF-8");
        xhr.addEventListener("readystatechange", function () {
            if (xhr.readyState === 4 && xhr.status !== 201 && xhr.status !== 200) {
                // Error. Inform the user
                let message = xhr.response;
                if (message.includes("Oops!")) {
                    Alert(message);
                } else {
                    Alert("Oops! Somethis Is Wrong. Json File Is Not Valid");
                }
            }
        });
        formData.append("file", file);
        xhr.send(reader.result);

    };
    reader.onerror = function () {
        Alert("Oops! Somethis Is Wrong. " + reader.error);
    };
}
