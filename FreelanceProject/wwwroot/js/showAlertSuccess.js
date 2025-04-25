function showAlertSuccess(alertId) {
    var alertBox = document.getElementById(alertId);
    if (alertBox) {
        alertBox.classList.add("show-alert");
        setTimeout(() => {
            alertBox.classList.remove("show-alert");

            // Yönlendirme işlemi
            window.location.href = '/user/signin';

        }, 3000); // 3 saniye sonra kaybolur
    }
}