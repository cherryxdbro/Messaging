const connection = new signalR.HubConnectionBuilder()
    .withUrl("/messaging/chat/websocket")
    .build();

connection.start()
    .then(() => console.log("Connected to the chat hub."))
    .catch(error => console.error(`${error.name}: ${error.message}`));

connection.on("ReceiveMessage", function (message) {
    const messageCard = createMessageCard(message);
    const messagesContainer = document.getElementById("messagesContainer");
    messagesContainer.prepend(messageCard);
});