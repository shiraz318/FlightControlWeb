let dropArea = document.getElementById("drop-area");

["dragenter", "dragover", "dragleave", "drop"].forEach(eventName => {
    dropArea.addEventListener(eventName, preventDefaults, false);
});

function preventDefaults(e) {
    e.preventDefault();
    e.stopPropagation();
}

["dragenter", "dragover"].forEach(eventName => {
    dropArea.addEventListener(eventName, highlight, false);
})

    ;["dragleave", "drop"].forEach(eventName => {
        dropArea.addEventListener(eventName, unhighlight, false);
    });


function ShowImage() {
    let img = document.getElementById("myImageId");
    //let dropArea = document.getElementById("drop-area");
   // dropArea.style.backgroundImage = "url(../images/DragandDrop.png)";
    
    //list.style.visibility = "hidden";
    // img.style.visibility = "visible";
   // img.style.opacity = 1.0;
    console.log("Visible");
}
function HideImage() {
    let img = document.getElementById("myImageId");
    //let dropArea = document.getElementById("drop-area");
    //dropArea.style.background = "none";
   // let list = document.getElementById("my-flights-list");
   // list.style.visibility = "visible";
    //img.style.visibility = "hidden";
   // img.style.opacity = 0.0;
}

function highlight(e) {
    dropArea.classList.add("highlight");
    ShowImage();
    
}

function unhighlight(e) {
    HideImage();
    dropArea.classList.remove("highlight");
}
dropArea.addEventListener("drop", handleDrop, false);

function handleDrop(e) {
    let dt = e.dataTransfer;
    let files = dt.files;
    console.log("IN HANDLE DROP");
    handleFiles(files);
}

function handleFiles(files) {
    [...files].forEach(uploadFile);
}
 

function uploadFile(file) {

    var url = "/api/FlightPlan";
    var xhr = new XMLHttpRequest();
    var formData = new FormData();
    let reader = new FileReader();
    reader.readAsText(file);
    reader.onload = function () {
        xhr.open("POST", url, true);
        xhr.setRequestHeader("Content-type", "application/json;charset=UTF-8");
        xhr.addEventListener("readystatechange", function (e) {
            if (xhr.readyState === 4 && xhr.status === 201) {
                //NewRow(reader.result);
                // Done. Inform the user

                console.log("GOOD");
            }
            else if (xhr.readyState === 4 && xhr.status !== 201) {
                // Error. Inform the user
                console.log(xhr.status);
            }
        });

        formData.append("file", file);
        xhr.send(reader.result);
        //console.log(reader.result);
    };

    reader.onerror = function () {
        console.log(reader.error);
    };
}

function previewFile(file) {
    let reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onloadend = function () {
        let img = document.createElement("img");
        img.src = reader.result;
        document.getElementById("gallery").appendChild(img);
    };
}