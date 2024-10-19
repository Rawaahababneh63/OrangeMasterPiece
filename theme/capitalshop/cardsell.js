async function GetCategories() {
    const url = 'https://localhost:44332/api/Category';
    var response = await fetch(url);
    var result = await response.json();
    var container = document.getElementById('container');
    result.forEach(element => {
        container.innerHTML += `
           <div class="col-xl-3 col-lg-4 col-md-6 col-sm-6">
    <div class="category">
        <a href="form.html" onclick="setCategory(${element.categoryId})">
            <img src="../../../../Backend/Masterpiece/Masterpiece/Uploads/${element.categoryImage}" alt="${element.categoryName}">
        </a>
        <h2>${element.categoryName}</h2>
      
    </div>
</div>
`;
    });
}


GetCategories();
    function setCategory(categoryId) {
        // تخزين معرف الفئة في localStorage
        localStorage.setItem('selectedCategoryId', categoryId);
    }

//////////////////////////////