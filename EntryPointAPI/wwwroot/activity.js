"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/activityhub").build();

connection.on("data-feed", function (message) {
    var textArea = document.getElementById("activity-output");
    textArea.value += '\r\n\r\n' + message;
});

connection.start().then(function () {
    console.log('connected to activityhub');
}).catch(function (err) {
    return console.error(err.toString());
});
