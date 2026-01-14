(() => {
    const form = document.getElementById('appSettingForm');
    const modalEl = document.getElementById('confirmSaveModal');
    const confirmBtn = document.getElementById('appSettingConfirmBtn');
    const bsModal = new bootstrap.Modal(modalEl);
    let confirmed = false;

    // Intercept all submits (button click & Enter)
    form.addEventListener('submit', function (e) {
        if (confirmed) return; // already confirmed -> allow submit

        // If using HTML5 validation, block and show errors first
        if (typeof form.checkValidity === 'function' && !form.checkValidity()) {
            e.preventDefault();
        e.stopPropagation();
        form.classList.add('was-validated');
        form.reportValidity && form.reportValidity();
        return;
                    }

        // Show confirm modal instead of submitting immediately
        e.preventDefault();
        e.stopPropagation();
        bsModal.show();
    });

    // On confirm: close modal and submit for real
    confirmBtn.addEventListener('click', function () {
        confirmed = true;
        bsModal.hide();
        if (typeof form.requestSubmit === 'function') {
            form.requestSubmit();
        } else {
            form.submit();
        }
    });
    }
)();
