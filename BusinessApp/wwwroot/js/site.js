document.addEventListener("DOMContentLoaded", function() {
    const uploadForm = document.getElementById("uploadForm");
    const resultContainer = document.getElementById("resultContainer");

    uploadForm.addEventListener("submit", function(event) {
        event.preventDefault();
        const formData = new FormData(uploadForm);
        
        fetch("/Upload/Process", {
            method: "POST",
            body: formData
        })
        .then(response => response.json())
        .then(data => {
            displayResults(data);
        })
        .catch(error => {
            console.error("Error:", error);
        });
    });

    function displayResults(data) {
        resultContainer.innerHTML = "";
        if (data.validPayments.length > 0) {
            const validList = document.createElement("ul");
            data.validPayments.forEach(payment => {
                const listItem = document.createElement("li");
                listItem.textContent = `Valid Payment: ${payment.narration} on ${payment.date}`;
                validList.appendChild(listItem);
            });
            resultContainer.appendChild(validList);
        }

        if (data.invalidPayments.length > 0) {
            const invalidList = document.createElement("ul");
            data.invalidPayments.forEach(payment => {
                const listItem = document.createElement("li");
                listItem.textContent = `Invalid Payment: ${payment.narration} on ${payment.date}`;
                invalidList.appendChild(listItem);
            });
            resultContainer.appendChild(invalidList);
        }
    }
});