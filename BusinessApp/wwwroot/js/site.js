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
        resultContainer.innerHTML = ""; // Clear previous results

        if (data.success) {
            const resultsList = document.createElement("ul");
            data.results.forEach(result => {
                const listItem = document.createElement("li");
                listItem.textContent = `Payment ID: ${result.paymentId}, Status: ${result.status}`;
                resultsList.appendChild(listItem);
            });
            resultContainer.appendChild(resultsList);
        } else {
            resultContainer.textContent = "No results found or an error occurred.";
        }
    }
});