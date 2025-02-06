// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $("#StartDate, #EndDate").change(function () {
        var startDate = new Date($("#StartDate").val());
        var endDate = new Date($("#EndDate").val());

        if (startDate && endDate && endDate < startDate) {
            alert("Slutdatum kan inte vara före startdatum!");
            $("#EndDate").val(""); // Rensar felaktigt datum
        }
    });
});
function clearSession() {
    fetch('/Home/Logout', { method: 'POST' })
        .then(response => window.location.reload()) // Refresh page after clearing session
        .catch(error => console.error('Error:', error));
}