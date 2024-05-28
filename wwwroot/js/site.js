// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener('DOMContentLoaded', function () {
    var toastEl = document.getElementById('liveToast');
    var toast = new bootstrap.Toast(toastEl);
    toast.show();
});


function fetchReminders() {
    fetch('/Reminders/CheckReminders')
        .then(response => response.json())
        .then(data => {
            data.forEach(reminder => {
                showToast(reminder.title, reminder.reminderDateTime);
            });
        });
}

function showToast(title, dateTime) {
    var toastHTML = `
    <div class="toast" role="alert" aria-live="assertive" aria-atomic="true">
        <div class="toast-header">
            <strong class="me-auto">${title}</strong>
            <small>${new Date(dateTime).toLocaleString()}</small>
            <button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Close"></button>
        </div>
        <div class="toast-body">
            Reminder: ${title} at ${new Date(dateTime).toLocaleString()}
        </div>
    </div>`;
    document.body.insertAdjacentHTML('beforeend', toastHTML);
    $('.toast').toast('show');
}

document.addEventListener('DOMContentLoaded', function () {
    setInterval(fetchReminders, 60000); // Check every minute
});

