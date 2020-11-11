"use strict"

function enableUrl() {
    document.getElementById("urlInputField").disabled = false;
    document.getElementById("imageUploadInput").disabled = true;
}

function enableUpload() {
    document.getElementById("urlInputField").disabled = true;
    document.getElementById("imageUploadInput").disabled = false;
}