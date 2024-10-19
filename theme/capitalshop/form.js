async function getAllCategory() {
    debugger
    try {
        // جلب الفئة المخزنة من localStorage
        const storedCategory = localStorage.getItem('selectedCategoryId');
        
        // طلب الفئات من الـ API
        let request = await fetch(`https://localhost:44332/api/Category`);
        let data = await request.json();

        const categorySelect = document.getElementById("CategoryId");

        // إيجاد الفئة المطابقة للفئة المخزنة
        const matchedCategory = data.find(element => element.categoryId == storedCategory);
        
        // التحقق من وجود الفئة المخزنة في البيانات المجلوبة
        if (matchedCategory) {
            categorySelect.innerHTML = `
                <option id="Subcategory" value="${matchedCategory.categoryId}" selected>${matchedCategory.categoryName}</option>
            `;
        } else {
            // عرض رسالة أو خيار افتراضي في حال لم تكن الفئة المخزنة موجودة في البيانات
            categorySelect.innerHTML = `
                <option value="" disabled selected>الفئة غير موجودة</option>
            `;
        }
    } catch (error) {
        console.error('Error fetching categories:', error);
    }
}

// استدعاء الدالة عند تحميل الصفحة
getAllCategory();





async function loadSubcategories() {
    try {
        const categoryId = localStorage.getItem("selectedCategoryId");
        let request = await fetch(`https://localhost:44332/api/SubCategory/GetSUbCategoryBYCtegoryID/${categoryId}`);
        let data = await request.json();

        const categorySelect = document.getElementById("subcategory");
        data.forEach(element => {
            categorySelect.innerHTML += `
                              <option value="${element.subcategoryId}">${element.subcategoryName}</option>
            `;
        });
    } catch (error) {
        console.error('Error fetching categories:', error);
    }
}




loadSubcategories();



// استدعاء الدالة عند تحميل الصفحة
window.onload = async function() {
    await getAllCategories();
    await loadSubcategories(); // جلب الفئات الفرعية عند تحميل الصفحة
};

// إضافة حدث تغيير لاختيار الفئة الرئيسية
document.getElementById("CategoryId").addEventListener("change", function() {
    localStorage.setItem("selectedCategoryId", this.value); // تخزين القيمة المختارة
    loadSubcategories(); // استدعاء الدالة لتحميل الفئات الفرعية
});


///////////////////////////
async function submitForm(event) {
    debugger
    event.preventDefault(); // منع إعادة تحميل الصفحة
    const formData = new FormData(event.target); // استخدام FormData للتعامل مع الصور والبيانات

    const userId = localStorage.getItem('UserId') || 0; // الحصول على userId من localStorage
    formData.append('userId', userId); 

    const mainCategoryId = 2; // حدد قيمة صحيحة لـ mainCategoryId حسب حاجتك
    formData.append('mainCategoryId', mainCategoryId); 

    // تحقق من البيانات
    for (let pair of formData.entries()) {
        console.log(pair[0]+ ', ' + pair[1]); 
    }

    try {
        const response = await fetch('https://localhost:44332/api/SaleRequests/submit-sale-request', {
            method: 'POST',
            body: formData // إرسال البيانات كـ FormData
        });

        if (!response.ok) {
            const errorData = await response.json(); // احصل على البيانات في حالة الخطأ
            console.error('Error:', errorData);
            throw new Error(errorData.message || 'حدث خطأ أثناء الاتصال بالسيرفر');
        }

        const data = await response.json();
        alert(data.message || 'تم إرسال الطلب بنجاح!');
    } catch (error) {
        console.error('There was a problem with your fetch operation:', error);
        alert('حدث خطأ أثناء إرسال الطلب.');
    }
}

// دالة لمعاينة الصورة عند رفعها
function previewImage(event) {
    const reader = new FileReader();
    reader.onload = function() {
        const preview = document.getElementById('preview');
        preview.src = reader.result; // تعيين مصدر الصورة المعاينة
    };
    reader.readAsDataURL(event.target.files[0]); // قراءة الصورة
}

