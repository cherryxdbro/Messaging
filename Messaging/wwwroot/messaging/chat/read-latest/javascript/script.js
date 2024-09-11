document.getElementById("loadMessagesButton").addEventListener("click", async function () {
    this.disabled = true;
    const messagesContainer = document.getElementById("messagesContainer");
    const spinner = createSpinner();
    messagesContainer.innerHTML = "";
    messagesContainer.append(spinner);
    try {
        const response = await fetch("/messaging/chat/api/read-latest");
        const result = await response.text();
        if (response.ok) {
            if (result === "No messages found in the last 10 minutes.") {
                displayAlert(messagesContainer, "Information:", result, "info-circle-fill");
            } else {
                const messages = JSON.parse(result);
                displayMessages(messagesContainer, messages);
            }
        } else {
            displayAlert(messagesContainer, "Error:", result, "exclamation-triangle-fill");
        }
    } catch (error) {
        displayAlert(messagesContainer, "Error:", `${error.name}: ${error.message}`, "exclamation-triangle-fill");
    } finally {
        this.disabled = false;
    }
});

function displayAlert(container, title, message, iconClass) {
    const alert = createAlert(title, message, iconClass);
    container.innerHTML = "";
    container.append(alert);
}

function displayMessages(container, messages) {
    const fragment = document.createDocumentFragment();
    for (const message of messages) {
        const messageCard = createMessageCard(message);
        fragment.append(messageCard);
    }
    container.innerHTML = "";
    container.append(fragment);
}