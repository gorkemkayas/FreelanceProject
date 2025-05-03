function validateForms(inputId, errorId, buttonId) {
    const inputElement = document.getElementById(inputId);
    const errorElement = document.getElementById(errorId);
    const buttonElement = document.getElementById(buttonId);

    if (!inputElement || !errorElement || !buttonElement) {
        console.error(`Element not found: ${inputId} or ${errorId}`);
        return;
    }

    inputElement.addEventListener('input', function () {
        const inputValue = this.value.trim();

        if (inputValue.length === 0) {
            errorElement.style.display = 'block';
            this.classList.add('is-invalid');
            buttonElement.disabled = true; // butonu pasif yap
        } else {
            errorElement.style.display = 'none';
            this.classList.remove('is-invalid');
            buttonElement.disabled = false; // butonu aktif yap
        }
    });
}