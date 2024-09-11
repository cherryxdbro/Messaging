document.addEventListener("DOMContentLoaded", function () {
    const themeToggleButton = document.getElementById("themeButton");
    const themeIcon = document.getElementById("themeIcon");
    const themeIconUseElement = themeIcon.querySelector("use");
    const savedTheme = localStorage.getItem("theme") || "light";
    document.documentElement.setAttribute("data-bs-theme", savedTheme);
    updateThemeIcon(savedTheme);
    themeToggleButton.addEventListener("click", function () {
        themeToggleButton.setAttribute("disabled", true);
        const currentTheme = document.documentElement.getAttribute("data-bs-theme");
        const newTheme = currentTheme === "dark" ? "light" : "dark";
        document.documentElement.setAttribute("data-bs-theme", newTheme);
        localStorage.setItem("theme", newTheme);
        updateThemeIcon(newTheme);
        themeToggleButton.classList.add("pulse");
        setTimeout(function () {
            themeToggleButton.classList.remove("pulse");
            themeToggleButton.removeAttribute("disabled");
        }, 1000);
    });
    function updateThemeIcon(theme) {
        const iconPath = theme === "dark" ? "/shared/scalable-vector-graphics/themes.svg#moon-stars-fill" : "/shared/scalable-vector-graphics/themes.svg#sun-fill";
        themeIconUseElement.setAttribute("xlink:href", iconPath);
    }
});

function createAlert(label, message, icon) {
    const alert = document.createElement("div");
    alert.className = "alert alert-dismissible alert-primary align-items-center d-flex message";
    alert.role = "alert";
    alert.innerHTML = `
        <svg aria-label="${label}" class="bi me-2" role="img">
            <use xlink:href="/shared/scalable-vector-graphics/alerts.svg#${icon}"></use>
        </svg>
        <div>${message}</div>
        <button aria-label="Close" class="btn-close" data-bs-dismiss="alert" type="button"></button>
    `;
    return alert;
}

function createCardElement(className, textContent, textClass) {
    const element = document.createElement("div");
    element.className = className;
    if (textClass) {
        const text = document.createElement("p");
        text.className = textClass;
        text.textContent = textContent;
        element.append(text);
    } else {
        element.textContent = textContent;
    }
    return element;
}

function createMessageCard(message) {
    const messageCard = document.createElement("div");
    messageCard.className = "card message text-start";
    const header = createCardElement("card-header", message.name);
    const body = createCardElement("card-body", message.text, "card-text");
    const footer = createCardElement("card-footer text-end", new Date(message.timestamp).toLocaleTimeString());
    messageCard.append(header, body, footer);
    return messageCard;
}

function createSpinner() {
    const spinner = document.createElement("div");
    spinner.className = "my-3 spinner-border text-primary";
    spinner.role = "status";
    spinner.innerHTML = "<span class='visually-hidden'>Loading...</span>";
    return spinner;
}