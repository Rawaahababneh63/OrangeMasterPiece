
async function applyDiscount() {
    const productId = document.getElementById('id').value;
    const discount = document.getElementById('discount').value;

    if (!productId || !discount) {
        alert("Enter product ID and discount.");
        return;
    }

    try {
        const response = await fetch(`https://localhost:44332/api/Products/${productId}/apply-discount`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(parseFloat(discount))
        });

        if (response.ok) {
            const data = await response.json();
            showSnackbar(`Success: ${data.message}, Discount: ${data.discount}%`);
        } else if (response.status === 400) {
            alert("Error: Invalid discount value.");
        } else if (response.status === 404) {
            alert("Error: Product not found.");
        }
    } catch (error) {
        console.error('Error:', error);
        alert('Failed to apply discount.');
    }
}

function showSnackbar(message) {
    const snackbar = document.getElementById("snackbar");
    snackbar.textContent = message;
    snackbar.className = "snackbar show";
    setTimeout(() => { snackbar.className = snackbar.className.replace("show", ""); }, 3000);
}