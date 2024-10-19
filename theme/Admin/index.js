async function fetchCategoryCount() {
    try {
        const response = await fetch('https://localhost:44332/api/Category/CountAllCategory');
        const count = await response.json();

       
        const categoryCountElement = document.querySelector('.widget-data .weight-700');
        
        // Update the count in the element
        categoryCountElement.textContent = count;

    } catch (error) {
        console.error('Error fetching category count:', error);
    }
}


fetchCategoryCount();


async function fetchSubCategoryCount() {
    try {
        const response = await fetch('https://localhost:44332/api/SubCategory/CountAllSubCategory');
        const count = await response.json();

       
        const categoryCountElement = document.querySelector('.subcategory');
        
        // Update the count in the element
        categoryCountElement.textContent = count;

    } catch (error) {
        console.error('Error fetching category count:', error);
    }
}

fetchSubCategoryCount();

    async function fetchProductCount() {
        try {
            const response = await fetch('https://localhost:44332/api/Products/CountOfAllProduct');
            const count = await response.json();

            // Find the element where the product count will be displayed
            const productCountElement = document.querySelector('.widget-data .product-count');

            // Update the count in the element
            productCountElement.textContent = count;

        } catch (error) {
            console.error('Error fetching product count:', error);
        }
    }

    // Call the function to fetch and display the product count
    fetchProductCount();

