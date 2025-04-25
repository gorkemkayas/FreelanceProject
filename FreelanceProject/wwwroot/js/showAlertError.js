function showAlertError(alertId) {
    var alertBox = document.getElementById(alertId);
    if (alertBox) {
        alertBox.classList.add("show-alert");
        setTimeout(() => {
            alertBox.classList.remove("show-alert");
        }, 3000); // 3 saniye sonra kaybolur
    }
}