// رابط API لجلب الأخبار من الباك إند
const apiUrl = "https://localhost:44332/api/News";  // استبدل هذا بالرابط الصحيح

// تحديد العنصر الذي ستتم إضافة الكاردات إليه
const newsContainer = document.getElementById('News');

// إنشاء دالة لجلب البيانات باستخدام async/await
async function fetchNews() {
    try {
        // جلب البيانات من API
        const response = await fetch(apiUrl);
        
        // التأكد من نجاح الطلب
        if (!response.ok) {
            throw new Error('Failed to fetch news');
        }
        
        // تحويل البيانات إلى JSON
        const newsData = await response.json();

        // تفريغ الـ HTML
        let newsHTML = '';

        // إنشاء الكاردات باستخدام البيانات المستلمة
        newsData.forEach(newsItem => {
            newsHTML += `
                <div class="col-lg-4 col-md-6 col-sm-6">
                    <div class="single-blogs mb-30">
                        <div class="blog-img">
                            <a href="${newsItem.link}"><img src="../../../../Backend/Masterpiece/Masterpiece/Uploads/${newsItem.imageUrl}" alt="${newsItem.title}"></a>
                        </div>
                        <div class="blogs-cap">
                            <h5><a href="${newsItem.link}">${newsItem.title}</a></h5>
                            <p>${newsItem.shortDescription}</p>
                            <a href="${newsItem.link}" class="red-btn">اقرأ المزيد</a>
                        </div>
                    </div>
                </div>`;
        });

        // إضافة الكاردات إلى الـ DOM باستخدام innerHTML
        newsContainer.innerHTML = newsHTML;

    } catch (error) {
        console.error('Error fetching news:', error);
    }
}

// استدعاء الدالة لجلب الأخبار عند تحميل الصفحة
fetchNews();
