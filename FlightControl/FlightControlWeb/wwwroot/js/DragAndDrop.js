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


function Highlight(e) {
    dropArea.classList.add("highlight");
}


function Unhighlight(e) {
    dropArea.classList.remove("highlight");
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
        xhr.addEventListener("readystatechange", function (e) {
             if (xhr.readyState === 4 && xhr.status !== 201 && xhr.status !== 200) {
                // Error. Inform the user
                Alert("Oops! Somethis Is Wrong. Json File Is Not Valid");
            }
        });

        formData.append("file", file);
        xhr.send(reader.result);
    };

    reader.onerror = function () {
        Alert("Oops! Somethis Is Wrong. " + reader.error);
    };
}

////
//function PreviewFile(file) {
//    let reader = new FileReader();
//    reader.readAsDataURL(file);
//    reader.onloadend = function () {
//        let img = document.createElement("img");
//        img.src = reader.result;
//        document.getElementById("gallery").appendChild(img);
//    };
//}