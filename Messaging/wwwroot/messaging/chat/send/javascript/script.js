document.getElementById("messageForm").addEventListener("submit", async function (event) {
    event.preventDefault();
    const formData = new FormData(this);
    const alertsContainer = document.getElementById("alertsContainer");
    const spinner = createSpinner();
    alertsContainer.prepend(spinner);
    try {
        const response = await fetch("/messaging/chat/api/send", {
            method: "POST",
            body: formData
        });
        const alert = response.ok 
            ? createAlert("Success:", await response.text(), "check-circle-fill") 
            : createAlert("Error:", await response.text(), "exclamation-triangle-fill");
        spinner.replaceWith(alert);
    } catch (error) {
        const errorAlert = createAlert("Error:", `${error.name}: ${error.message}`, "exclamation-triangle-fill");
        spinner.replaceWith(errorAlert);
    }
});

document.getElementById("clearAlerts").addEventListener("click", function () {
    document.getElementById("alertsContainer").innerHTML = "";
});