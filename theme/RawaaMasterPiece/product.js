async function fetchProductDetails() {
    const productId = localStorage.getItem('productId'); // جلب productId من localStorage
    if (productId) {
        try {
            // جلب تفاصيل المنتج من API
            const response = await fetch(`https://localhost:44332/api/Products/GetProductByIdNew/${productId}`);
            const product = await response.json();

                // جلب التقييم الخاص بالمنتج
                const ratingResponse = await fetch(`https://localhost:44332/api/Comment/GetProductRating/${productId}`);
                const averageRating = await ratingResponse.json(); // افتراض أن الـ API يعيد متوسط التقييم للمنتج
                          
            // حساب السعر الحالي والسعر القديم
            const currentPrice = product.priceWithDiscount ? 
            (product.price - (product.price * (product.priceWithDiscount / 100))).toFixed(2) : 
            product.price.toFixed(2);
        const oldPrice = product.priceWithDiscount ? product.price.toFixed(2) : null; // إذا لم يكن هناك خصم، اجعل السعر القديم null
            // تحديث محتويات الصفحة باستخدام innerHTML
            document.querySelector('.product_right_sidebar').innerHTML = `
    <div class="row" style="direction: rtl; text-align: right;">  <!-- إضافة اتجاه الكتابة من اليمين لليسار -->
    <div class="col-lg-6 col-md-6">
        <div class="product-details-tab">
            <div id="img-1" class="zoomWrapper single-zoom">
                <a href="#">
                    <img id="zoom1" src="../../../../Backend/Masterpiece/Masterpiece/Uploads/${product.image}" 
                        data-zoom-image="../../../../Backend/Masterpiece/Masterpiece/Uploads/${product.image}" alt="${product.name}">
                </a>
            </div>
       <div class="single-zoom-thumb">
    <ul class="s-tab-zoom owl-carousel single-product-active" id="gallery_01">
        <li>
            <a href="#" class="elevatezoom-gallery active" data-update="" data-image="../../../../Backend/Masterpiece/Masterpiece/Uploads/${product.image}" data-zoom-image="../../../../Backend/Masterpiece/Masterpiece/Uploads/${product.image}">
                <img id="thumb1" src="../../../../Backend/Masterpiece/Masterpiece/Uploads/${product.image}" alt="zo-th-1"/>
            </a>
        </li>
        <li>
            <a href="#" class="elevatezoom-gallery active" data-update="" data-image="../../../../Backend/Masterpiece/Masterpiece/Uploads/${product.image}" data-zoom-image="../../../../Backend/Masterpiece/Masterpiece/Uploads/${product.image}">
                <img id="thumb2" src="../../../../Backend/Masterpiece/Masterpiece/Uploads/${product.image}" alt="zo-th-2"/>
            </a>
        </li>
    </ul>
</div>
        </div>
    </div>
    <div class="col-lg-6 col-md-6">
        <div class="product_d_right">
            <form action="#">
                <h1 style="text-align: right;"><a id="productName" href="#">${product.name}</a></h1>
                <div class="product_nav"">
                    <ul  style="position:relative;right:360px">
                        <li class="prev"><a href="#"><i class="fa fa-angle-right"></i></a></li> <!-- تعديل الأيقونة -->
                        <li class="next"><a href="#"><i class="fa fa-angle-left"></i></a></li> <!-- تعديل الأيقونة -->
                    </ul>
                </div>
                <div class="product_ratting">
                    <ul style="text-align: right;">
                        ${generateStars(averageRating)} <!-- عرض النجوم بناءً على التقييم -->
                        <li class="review"><a href="#"> (${averageRating} تقييم العملاء) </a></li>
                    </ul>
                </div>
               <div class="price_box">
                        <span id="currentPrice" class="current_price">$${currentPrice}</span>
                        ${oldPrice ? `<span id="oldPrice" class="old_price" style="text-decoration: line-through;">$${oldPrice}</span>` : ''}
                    </div>
                <div class="product_desc">
                    <p id="productDescription">${product.description}</p>
                </div>
                <div class="product_variant color">
                    <h3>الخيارات المتاحة</h3>
                    <label>اللون</label>
                    <ul style="text-align: right;">
                        <li class="color1"><a href="#"></a></li>
                        <li class="color2"><a href="#"></a></li>
                        <li class="color3"><a href="#"></a></li>
                        <li class="color4"><a href="#"></a></li>
                    </ul>
                </div>
                <div class="product_variant quantity">
                    <label>الكمية</label>
          <input id="inputQutity" type="number" value="1" min="1">
                    <label for="offerInput">السعر المطلوب </label>
<input type="text" id="offerInput" >
                </div>
                <div class="button_group" style="display: flex; gap: 10px; margin-top: 15px;">
                <button class="btn btn-dark" class="button" href="cart.html" style="color: white; background-color: black;" onclick="addToCart(${product.productId}, '${product.name}', ${(product.price - (product.price * (product.priceWithDiscount / 100))).toFixed(2)}, '${product.image}')""addToCart()">اضافة للسلة</button> 
          
                  <button class="btn btn-dark" type="submit" style="color: white; background-color: black;" onclick="negotiatePrice()">مفاوضة على السعر</button>
                </div>
                <div class="product_d_action">
                    <ul style="text-align: right;">
                        <li><a href="#" title="أضف إلى المفضلة">+ أضف إلى المفضلة</a></li>
                        <li><a href="#" title="قارن">+ قارن</a></li>
                    </ul>
                </div>
            </form>
            <div class="priduct_social" style="text-align: right;">
                <ul>
                    <li><a class="facebook" href="#" title="facebook"><i class="fa fa-facebook"></i> أعجبني</a></li>           
                    <li><a class="twitter" href="#" title="twitter"><i class="fa fa-twitter"></i> غرد</a></li>           
                    <li><a class="pinterest" href="#" title="pinterest"><i class="fa fa-pinterest"></i> احفظ</a></li>           
                    <li><a class="google-plus" href="#" title="google +"><i class="fa fa-google-plus"></i> شارك</a></li>        
                    <li><a class="linkedin" href="#" title="linkedin"><i class="fa fa-linkedin"></i> شارك</a></li>        
                </ul>      
            </div>
        </div>
    </div>
</div>
            `;

            // تفعيل الزووم بعد تحميل الصور
            $('#zoom1').elevateZoom({
                zoomType: "inner",
                cursor: "crosshair"
            });

            // إعادة تفعيل الـ Owl Carousel بعد إضافة الصور
            $(".owl-carousel").owlCarousel({
                items: 3,  // عدد الصور الصغيرة المراد عرضها
                loop: true,
                margin: 10,
                nav: true,
                dots: false,
                autoplay: true,
                rtl: true 
            });

        } catch (error) {
            console.error('Error fetching product details:', error);
        }
    } else {
        console.error('Product ID not found in localStorage');
    }
}

// استدعاء الدالة لجلب التفاصيل عند تحميل الصفحة
window.onload = fetchProductDetails;
// //////////////////////////
let selectedRating = 0;
const productId = localStorage.getItem('productId'); // افتراض أن معرف المنتج موجود في localStorage
const UserId =localStorage.getItem("UserId"); // مثال، يجب استبداله بمعرف المستخدم الصحيح

// تفعيل اختيار النجوم
document.querySelectorAll('#star-rating a').forEach(star => {
    star.addEventListener('click', function(event) {
        event.preventDefault();
        selectedRating = this.getAttribute('data-value');
        highlightStars(selectedRating);
    });
});

function highlightStars(rating) {
    document.querySelectorAll('#star-rating a').forEach(star => {
        if (parseInt(star.getAttribute('data-value')) <= rating) {
            star.style.color = 'gold'; // نجوم ذهبية
        } else {
            star.style.color = 'grey'; // نجوم رمادية
        }
    });
}

// إرسال التعليق مع التقييم
document.getElementById('commentForm').addEventListener('submit', async function(e) {
    e.preventDefault();
    
    // التحقق من وجود تقييم سابق للمستخدم
    const hasPreviousRating = await checkPreviousRating(productId, UserId);
    if (hasPreviousRating) {
        alert("لقد قمت بالفعل بتقييم هذا المنتج ولا يمكنك تقييمه مرة أخرى.");
        return; // منع إرسال التقييم مرة ثانية
    }

    const author = document.getElementById("author").value;
    const email = document.getElementById("email").value;
    const commentText = document.getElementById("review_comment").value;
    
    const comment = {
        UserId: UserId,
        ProductId: productId,
        Comment1: commentText,
        Rating: selectedRating,
        Status: "pending" // الافتراض أن التعليقات تمر عبر موافقة الإدارة
    };

    try {
        const response = await fetch("https://localhost:44332/api/Comment/AddComment", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(comment),
        });
        if (response.ok) {
            alert("تم إرسال التقييم بنجاح. سيتم نشره بمجرد الموافقة عليه.");
            loadComments(productID); // إعادة تحميل التعليقات بعد الإرسال
        }
    } catch (error) {
        console.error("Error submitting comment:", error);
    }
});

// دالة للتحقق من وجود تقييم سابق للمستخدم
async function checkPreviousRating(productId, userId) {
    try {
        const response = await fetch(`https://localhost:44332/api/Comment/GetComments/${productId}`);
        const comments = await response.json();
        
        // التحقق مما إذا كان المستخدم قد قام بتقييم المنتج مسبقًا
        return comments.some(comment => comment.UserId === userId);
    } catch (error) {
        console.error("Error checking previous rating:", error);
        return false; // افتراض أنه لا يوجد تقييم في حال حدوث خطأ
    }
}

// تحميل التعليقات وعرضها
async function loadComments(productId) {
    try {
        const response = await fetch(`https://localhost:44332/api/Comment/GetComments/${productId}`);
        const comments = await response.json();

        const reviewsWrapper = document.querySelector(".reviews_wrapper");
        reviewsWrapper.innerHTML = ''; // تفريغ الحاوية قبل إعادة تعبئتها

        comments.forEach((comment) => {
            const commentHtml = `
                <div class="reviews_comment_box">
                    <div class="comment_thmb">
                        <img src="/htmldemo.net/dorno/dorno/assets/img/blog/comment2.jpg" alt="User Image">
                    </div>
                    <div class="comment_text">
                        <div class="reviews_meta">
                            <div class="star_rating">
                                <ul>
                                    ${generateStars(comment.rating)}
                                </ul>
                            </div>
                            <p><strong>${comment.userName}</strong> - ${comment.date}</p>
                            <span>${comment.comment1}</span>
                        </div>
                    </div>
                </div>`;
            reviewsWrapper.innerHTML += commentHtml;
        });
    } catch (error) {
        console.error("Error loading comments:", error);
    }
}

// دالة لتوليد النجوم بناءً على التقييم
function generateStars(rating) {
    let starsHtml = '';
    for (let i = 1; i <= 5; i++) {
        if (i <= rating) {
            starsHtml += '<li><i class="icon icon-Star" style="color: gold;"></i></li>';
        } else {
            starsHtml += '<li><i class="icon icon-Star" style="color: grey;"></i></li>';
        }
    }
    return starsHtml;
}

// استدعاء دالة تحميل التعليقات عند تحميل الصفحة
document.addEventListener("DOMContentLoaded", function() {
    loadComments(productId);
});


//////////////////////////////////////
async function negotiatePrice() {
    debugger
    const productId = localStorage.getItem('productId');
    const offerInput = document.getElementById("offerInput").value;

    if (!offerInput) {
        alert("من فضلك، أدخل السعر الذي ترغب في تقديمه في هذا المنتج");
        return;
    }

    const offer = parseFloat(offerInput);

    try {
        const response = await fetch('https://localhost:44332/api/Products/CheckPrice', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                ProductId: productId,
                Offer: offer,
            }),
        });

        const data = await response.json();


        const originalPrice = data.finalPrice; // السعر الأصلي
        const discountPercentage = data.priceWithDiscount; // نسبة الخصم

        // حساب السعر بعد الخصم
        const adjustedPrice = discountPercentage > 0 ? 
            (originalPrice - (originalPrice * (discountPercentage / 100))).toFixed(2) : 
            originalPrice.toFixed(2);

        // تحديد الحدود للمفاوضة
        const minimumAcceptablePrice = discountPercentage > 0 ? 
            (adjustedPrice - 1.5).toFixed(2) : 
            (originalPrice - 1.5).toFixed(2);

        const maximumAcceptablePrice = discountPercentage > 0 ? 
            adjustedPrice : 
            originalPrice;

        // تحقق مما إذا كان السعر الذي أدخله المستخدم ضمن النطاق
        if (offer >= minimumAcceptablePrice && offer <= maximumAcceptablePrice) {
            localStorage.setItem('negotiatedPrice', offer.toFixed(2));
            alert(`تم قبول العرض بسعر: $${offer.toFixed(2)}`);
        } else {
            alert("عذراً، العرض غير مقبول.");
        }

    } catch (error) {
        console.error('Error:', error);
        alert("حدث خطأ أثناء المفاوضة على السعر.");
    }
}
//////////////////////////////////////////////////////
async function addToCart(productId, name, originalPrice, image) {
    const negotiatedPrice = localStorage.getItem('negotiatedPrice');
    const priceToUse = negotiatedPrice ? parseFloat(negotiatedPrice) : originalPrice;

    // جلب الكمية من حقل الإدخال
    const quantityInput = document.getElementById("inputQutity").value;
    const quantity = parseInt(quantityInput) || 1; // استخدام 1 كقيمة افتراضية إذا كانت الكمية غير صحيحة

    if (UserId != null) {
        const url = `https://localhost:44332/api/Cart/AddCartItem/${UserId}`;

        const data = {
            ProductId: productId,
            Quantity: quantity,
            Price: priceToUse
        };

        let response = await fetch(url, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(data),
        });

        if (response.ok) {
            alert("تمت إضافة المنتج بنجاح إلى السلة!");
            window.location.href = "cart.html"; // استبدل هذا بالرابط الصحيح
        } else {
            let error = await response.text();
            console.error("خطأ:", error);
        }
    } else {
        const cartItem = {
            product_id: productId,
            quantity: quantity,
            name: name,
            price: priceToUse,
            image: image,
        };

        let cartItems = JSON.parse(localStorage.getItem("cartItems")) || [];
        let existingItem = cartItems.find((item) => item.product_id === productId);

        if (existingItem) {
            // إذا كان المنتج موجودًا بالفعل في السلة، قم بتحديث الكمية
            existingItem.quantity += quantity;
        } else {
            // إذا كان منتجًا جديدًا، أضفه إلى مصفوفة السلة
            cartItems.push(cartItem);
        }

        localStorage.setItem("cartItems", JSON.stringify(cartItems));
        alert(`تمت إضافة المنتج بنجاح إلى السلة!\nالاسم: ${name}\nالسعر: $${priceToUse.toFixed(2)}\nالكمية: ${cartItem.quantity}`);
        
        window.location.href = "cart.html"; // استبدل هذا بالرابط الصحيح
    }
}

/////////////////////////////////////

async function loadTopFiveAbayas() {
    try {
        // استدعاء الـ API
        let response = await fetch('https://localhost:44332/api/Products/GetTopFiveAbayas');
        let data = await response.json();
  
        // طباعة البيانات في الكونسول للتحقق منها
        console.log("Response data:", data);
  
        let container = document.getElementById('abayas-products-container');
        container.innerHTML = ''; // تنظيف المحتوى السابق
  
        // التأكد من وجود قائمة المنتجات
        if (data && data.products && data.products.length > 0) {
            // عرض المنتجات
            data.products.forEach(product => {
                container.innerHTML += `
                <div class="properties pb-30">
                    <div class="properties-card">
                        <div class="properties-img">
                            <a href="productdetalis1">
                                <img src="../../../../Backend/Masterpiece/Masterpiece/Uploads/${product.image}" alt="${product.name}">
                            </a>




                            <div class="socal_icon">
                                <a href="cart.html" onclick="addToCart(${product.productId}, '${product.name}', ${product.price}, '${product.image}')"><i class="ti-shopping-cart"></i></a>
                                <a href="#"><i class="ti-heart"></i></a>
                                <a href="#" onClick="storeProductId(${product.productId})"><i class="ti-zoom-in"></i></a>
                            </div>
                        </div>
                        <div class="properties-caption properties-caption2">
                            <h3><a href="productdetalis1.html">${product.name}</a></h3>
                            <div class="properties-footer">
                                <div class="price">
                                    <span>${product.price} JD<span class="old-price">15 JD</span></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>











                `;
            });
        } else {
            console.error('No products found or products is undefined.');
        }
    } catch (error) {
        console.error('Error fetching abayas:', error);
    }
  }

// استدعاء الدالة لعرض المنتجات عند تحميل الصفحة
loadTopFiveAbayas();



// دالة لتخزين productId في localStorage وإعادة التوجيه إلى صفحة التفاصيل
function storeProductId(productId) {
    localStorage.setItem('productId', productId); // تخزين productId في localStorage
    window.location.href = 'productdetalis1.html'; // إعادة التوجيه إلى صفحة التفاصيل
}
