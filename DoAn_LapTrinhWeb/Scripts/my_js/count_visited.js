// Declare a proxy to reference the hub.
var counter = $.connection.hitCounter;

// register online user at the very begining of the page
$.connection.hub.start().done(function () {
    // Call the Send method on the hub.
    counter.server.sendCounter();
});

// Create a function that the hub can call to recalculate online users.
counter.client.recalculateOnlineUsers = function (count) {
    // Add the message to the page.
    $('#counvisited').text(count);
};
