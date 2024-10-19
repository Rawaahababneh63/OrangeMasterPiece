// async function getAllCategory()
// {
   
//     let request= await fetch("https://localhost:44332/api/SubCategory/GetALLSubCategory");
//     let data= await request.json();

//     var CategoryId = document.getElementById("SubCategoryId");

//     data.forEach(element => {
        
//     CategoryId.innerHTML += `
//     <option value="${element.id}">${element.subcategoryName}</option>

//     `
//     });

    
// }
// getAllCategory();


// async function addProduct() {
//     event.preventDefault(); // Prevent the form from submitting normally

//     var form = document.getElementById("form");
//     const url = "https://localhost:44381/api/Product/Addproductadmin";
//     var formData = new FormData(form);

//     try {
//         let response = await fetch(url, {
//             method: "POST",
//             body: formData,
//         });

//         if (response.ok) {
//             alert('product added successfully');
          
//         } else {
//             alert('Failed to add product');
//         }
//     } catch (error) {
//         console.error('Error adding product:', error);
//         alert('Error adding product');
//     }
// }


async function getAllCategory() {
    try {
        let request = await fetch(`https://localhost:44332/api/Category`);
        let data = await request.json();

        const categorySelect = document.getElementById("CategoryId");
        data.forEach(element => {
            categorySelect.innerHTML += `
                              <option value="${element.categoryId}">${element.categoryName}</option>
            `;
        });
    } catch (error) {
        console.error('Error fetching categories:', error);
    }
}



// Call this function when the page loads
window.onload = getAllCategory;
async function addProduct(event) {
    debugger
    event.preventDefault(); // Prevent the form from submitting normally

    var form = document.getElementById("form");
    const url = "https://localhost:44332/api/Products/AddProductWith3Images";
    var formData = new FormData(form);

    try {
        let response = await fetch(url, {
            method: "POST",
            body: formData,
        });

        // Log response status and body
        console.log('Response Status:', response.status);
        console.log('Response Body:', await response.text());

        if (response.ok) {
            alert('Product added successfully');
            form.reset(); // إعادة تعيين النموذج
            clearSubcategorySelect(); // إعادة تعيين قائمة الفئات الفرعية
        } else {
            alert('Failed to add product');
        }
    } catch (error) {
        console.error('Error adding product:', error);
        alert('Error adding product');
    }
}

async function getSubcategoriesByCategory() {
    const categoryId = document.getElementById("CategoryId").value;

    if (categoryId === "Select Main Category") {
        clearSubcategorySelect();  // Clear the subcategory select if no category is selected
        return;
    }

    try {
        const response = await fetch(`https://localhost:44332/api/SubCategory/GetSUbCategoryBYCtegoryID/${categoryId}`);
        const subcategories = await response.json();

        console.log('Fetched subcategories:', subcategories); // سجل البيانات

        populateSubcategorySelect(subcategories);

    } catch (error) {
        console.error('Error fetching subcategories:', error);
    }
}

function populateSubcategorySelect(subcategories) {
    const subcategorySelect = document.getElementById("SubCategoryId");
    subcategorySelect.innerHTML = "<option selected>Select Subcategory</option>";  // Reset options

    if (subcategories.length === 0) {
        return;
    }

    subcategories.forEach(subcategory => {
        const option = document.createElement("option");
        option.value = subcategory.subcategoryId; // تأكد من استخدام المفتاح الصحيح هنا
        option.textContent = subcategory.subcategoryName; // تأكد من استخدام المفتاح الصحيح هنا
        subcategorySelect.appendChild(option);
    });
}

function clearSubcategorySelect() {
    const subcategorySelect = document.getElementById("SubCategoryId");
    subcategorySelect.innerHTML = "<option selected>Select Subcategory</option>";  // Reset options
}
getAllCategory();

async function getAllColors() {
    try {
        let request = await fetch(`https://localhost:44332/api/Color/api/Colors`);
        let colors = await request.json();

        const colorSelect = document.getElementById("ClothColorId");
        colors.forEach(color => {
            colorSelect.innerHTML += `
                <option value="${color.colorId}">${color.colorName}</option>
            `;
        });
    } catch (error) {
        console.error('Error fetching colors:', error);
    }
}


getAllColors();