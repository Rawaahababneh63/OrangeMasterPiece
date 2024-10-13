// Variables to handle pagination
let currentPage = 1;
const itemsPerPage = 5;

async function getSubCategory() {
    try {
        const response = await fetch('https://localhost:44332/api/SubCategory/GetALLSubCategory');
        const categories = await response.json();

        
        displayPaginatedCategories(categories, currentPage, itemsPerPage);
        setupPagination(categories, itemsPerPage);
        
    } catch (error) {
        console.error('Error fetching categories:', error);
    }
}

// Function to display paginated categories
function displayPaginatedCategories(categories, page, itemsPerPage) {
    const categoryContainer = document.getElementById('container');
    categoryContainer.innerHTML = '';  // Clear the current items

    const start = (page - 1) * itemsPerPage;
    const end = start + itemsPerPage;
    const paginatedItems = categories.slice(start, end);

    // Create table rows for paginated items
    paginatedItems.forEach(subcategory => {
        categoryContainer.innerHTML += `
            <tr>
                <td>${subcategory.subcategoryId}</td>
                <td>${subcategory.subcategoryName}</td>
                <td><img src="../../../../Backend/Masterpiece/Masterpiece/Uploads/${subcategory.image}" alt="${subcategory.subcategoryName}" class="img-fluid" style="height: 50px;"></td>
                <td>
                    <button class="btn btn-warning" onclick="getSubcategoryId(${subcategory.subcategoryId})">Edit</button>
                    <button class="btn btn-danger" onclick="deleteSubcategory(${subcategory.subcategoryId})">Delete</button>
                </td>
            </tr>
        `;
    });
}

// Function to set up pagination controls
function setupPagination(items, itemsPerPage) {
    const paginationContainer = document.getElementById('pagination-container');
    paginationContainer.innerHTML = '';  // Clear current pagination

    const pageCount = Math.ceil(items.length / itemsPerPage);
    
    for (let i = 1; i <= pageCount; i++) {
        paginationContainer.innerHTML += `<button class="btn btn-secondary mx-1" onclick="changePage(${i})">${i}</button>`;
    }
}

// Function to handle page changes
function changePage(pageNumber) {
    currentPage = pageNumber;
    getSubCategory();  // Re-fetch and display items for the new page
}

// Fetch and display the first set of categories
getSubCategory();

function getCategoryId(categoryid) {
    localStorage.setItem('categoryid', categoryid);
    window.location.href='UpdataCategory.html';
   
}

async function deleteCategory(categoryId) {
    if (confirm('Are you sure you want to delete this category?')) {
        try {
            const response = await fetch(`https://localhost:44332/api/Category/${categoryId}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                },
            });

            if (response.ok) {
                alert('Category deleted successfully');
                getCategory(); // Re-fetch categories to update the table
            } else {
                alert('Failed to delete category');
            }
        } catch (error) {
            console.error('Error deleting category:', error);
            alert('Error deleting category');
        }
    }
}
