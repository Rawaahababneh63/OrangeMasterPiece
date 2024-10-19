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
                    <button class="btn btn-sm btn-primary"  onclick="getSubcategoryId(${subcategory.subcategoryId})">Edit</button>
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
    localStorage.setItem('Subcategoryid', Subcategoryid);
    window.location.href='UpdateSubCategory.html';
   
}





async function getSubcategoriesByCategory() {
    const categoryId = document.getElementById("CategoryId").value;

    if (categoryId === "Select Category") {
        clearSubcategoryTable();  // Clear the table if no category is selected
        return;
    }

    try {
        const response = await fetch(`https://localhost:44332/api/SubCategory/GetSUbCategoryBYCtegoryID/${categoryId}`);
        const subcategories = await response.json();

        console.log('Fetched subcategories:', subcategories); // سجل البيانات

        displaySubcategories(subcategories);

    } catch (error) {
        console.error('Error fetching subcategories:', error);
    }
}

function displaySubcategories(subcategories) {
    const tableBody = document.querySelector("#subcategoryTable tbody");
    tableBody.innerHTML = "";  // Clear any existing rows

    if (subcategories.length === 0) {
        tableBody.innerHTML = "<tr><td colspan='4'>لا توجد فئات فرعية متاحة</td></tr>";
        return;
    }

    subcategories.forEach(subcategory => {
        tableBody.innerHTML += `
            <tr>
                <td>${subcategory.subcategoryId}</td>
                <td>${subcategory.subcategoryName}</td>
                <td><img src="../../../../Backend/Masterpiece/Masterpiece/Uploads/${subcategory.image}" alt="${subcategory.subcategoryName}" class="img-fluid" style="height: 50px;"></td>
                <td>
                    <button class="btn btn-sm btn-primary"  onclick="getSubcategoryId(${subcategory.subcategoryId})">Edit</button>
                    <button class="btn btn-danger" onclick="deleteSubcategory(${subcategory.subcategoryId})">Delete</button>
                </td>
            </tr>
        `;
    });
}

function clearSubcategoryTable() {
    const tableBody = document.querySelector("#subcategoryTable tbody");
    tableBody.innerHTML = "";  // Clear the table body
}

function getSubcategoryId(subcategoryId) {
    window.location.href='UpdateSubCategory.html';
    localStorage.setItem('SubcategoryID', subcategoryId);
    console.log('Selected Subcategory ID:', subcategoryId);
}

async function deleteSubcategory(subcategoryId) {
    if (confirm('Are you sure you want to delete this subcategory?')) {
        try {
            const response = await fetch(`https://localhost:44332/api/SubCategory/${subcategoryId}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                },
            });

            if (response.ok) {
                alert('Subcategory deleted successfully');
                const categoryId = document.getElementById("CategoryId").value;
                getSubcategoriesByCategory();  // Reload subcategories
            } else {
                alert('Failed to delete subcategory');
            }
        } catch (error) {
            console.error('Error deleting subcategory:', error);
            alert('Error deleting subcategory');
        }
    }
}

getAllCategory();