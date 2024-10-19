// Variables to handle pagination
let currentPage = 1;
const itemsPerPage = 5;

async function getProduct() {
    try {
        const response = await fetch('https://localhost:44332/api/Products/GetAllProducts');
        const Products = await response.json();

        
        displayPaginatedCategories(Products, currentPage, itemsPerPage);
        setupPagination(Products, itemsPerPage);
        
    } catch (error) {
        console.error('Error fetching Products:', error);
    }
}

// Function to display paginated categories
function displayPaginatedCategories(Products, page, itemsPerPage) {
    const productContainer = document.getElementById('container');
    productContainer.innerHTML = '';  // Clear the current items

    const start = (page - 1) * itemsPerPage;
    const end = start + itemsPerPage;
    const paginatedItems = Products.slice(start, end);

    // Create table rows for paginated items
    paginatedItems.forEach(product => {
        productContainer.innerHTML += `
            <tr>
                <td>${product.productId}</td>
                <td>${product.subcategoryName}</td> 
                <td>${product.name}</td>
                <td>${product.description}</td>
                <td>${product.price}</td>
                   <td>${product.minPrice}</td> 
                <td>${product.maxPrice}</td> 
                <td>${product.brand}</td> 
                <td><img src="/Backend/Masterpiece/Masterpiece/Uploads/${product.image}" alt="${product.name}" class="img-fluid" style="height: 50px;"></td> 
                <td><img src="/Backend/Masterpiece/Masterpiece/Uploads/${product.image1}" alt="${product.name} Image 1" class="img-fluid" style="height: 50px;"></td> 
                <td><img src="/Backend/Masterpiece/Masterpiece/Uploads/${product.image2}" alt="${product.name} Image 2" class="img-fluid" style="height: 50px;"></td> 
                <td><img src="/Backend/Masterpiece/Masterpiece/Uploads/${product.image3}" alt="${product.name} Image 3" class="img-fluid" style="height: 50px;"></td> 
                <td>${product.clothColor}</td>
                <td>${product.typeProduct}</td> 
                <td>${product.discount ? product.discount : "—"}</td> 
             
                <td>${product.isActive ? "Yes" : "NO"}</td> 
                <td>${product.isDonation ? "Yes" : "NO"}</td> 
                <td>${product.stockQuantity}</td> 
                <td>
                    <button  class="btn btn-sm btn-primary" onclick="getProductId(${product.productId})">Edit</button>
                    <button class="btn btn-danger" onclick="deleteProduct(${product.productId})">Delete</button>
                </td>
            </tr>
        `;
    });
    
}


function setupPagination(items, itemsPerPage) {
    const paginationContainer = document.getElementById('pagination-container');
    paginationContainer.innerHTML = ''; 
    const pageCount = Math.ceil(items.length / itemsPerPage);
    
    for (let i = 1; i <= pageCount; i++) {
        paginationContainer.innerHTML += `<button class="btn btn-secondary mx-1" onclick="changePage(${i})">${i}</button>`;
    }
}


function changePage(pageNumber) {
    currentPage = pageNumber;
    getProduct(); 
}


getProduct();

function getProductId(id) {
    localStorage.setItem('productId', id);
    window.location.href='updateproduct.html';
   
}

async function deleteProduct(id) {
    if (confirm('Are you sure you want to delete this Product?')) {
        try {
            const response = await fetch(`https://localhost:44332/api/Products/Product?id=${id}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                },
            });

            if (response.ok) {
                alert('Product deleted successfully');
                getProduct(); // Re-fetch categories to update the table
            } else {
                alert('Failed to delete Product');
            }
        } catch (error) {
            console.error('Error deleting Product:', error);
            alert('Error deleting Product');
        }
    }
}
