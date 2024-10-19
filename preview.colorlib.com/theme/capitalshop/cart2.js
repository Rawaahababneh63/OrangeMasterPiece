async function showItemsCart() {
    const itemContainer = document.getElementById("table");
    totalCartPrice = 0;
  
    if (!userId) { // إذا لم يكن هناك مستخدم مسجل دخول.
      const selectedItems = JSON.parse(localStorage.getItem("cartItems")) || [];
  
      selectedItems.forEach((item) => {
        const finalPrice = item.discount ? (item.price - item.discount) : item.price;
        totalCartPrice += finalPrice * item.quantity;
  
        itemContainer.innerHTML += generateCartRowHTML(item.product_id, item.image, item.name, finalPrice, item.quantity);
      });
    } else { // إذا كان هناك مستخدم مسجل دخول.
      try {
        const url = `https://localhost:44332/api/Cart/getUserCartItems/${userId}`;
        const response = await fetch(url);
        const result = await response.json();
  
        result.forEach((item) => {
          const finalPrice = item.product.discount ? item.product.priceWithDiscount : item.product.price;
          totalCartPrice += finalPrice * item.quantity;
  
          itemContainer.innerHTML += generateCartRowHTML(item.cartItemId, item.product.image, item.product.name, finalPrice, item.quantity);
        });
      } catch (error) {
        console.error("خطأ في جلب عناصر السلة:", error);
      }
    }
  
    updateTotalCartPrice(); // تحديث السعر الكلي بعد العرض.
  }
  
  function generateCartRowHTML(id, image, name, price, quantity) {
    return `
      <tr id="item-row-${id}">
        <td class="product_remove" style="cursor: pointer;">
          <i onclick="deleteItem(${id})" class="fa fa-trash-o"></i>
        </td>
        <td class="product_thumb">
          <a href="#"><img src="../../../../Backend/Masterpiece/Masterpiece/Uploads/${image}" alt=""></a>
        </td>
        <td class="product_name"><a href="#">${name}</a></td>
        <td class="product-price">$${price.toFixed(2)}</td>
        <td class="product_quantity">
          <label>Quantity</label>
          <input id="quantity-${id}" min="1" max="100" type="number"
                 value="${quantity}" onchange="changeQuantity(${id}, ${price})">
        </td>
        <td id="total-price-${id}" class="product_total">$${(price * quantity).toFixed(2)}</td>
      </tr>`;
  }
  

  function updateTotalCartPrice() {
    let cartTotalElements = document.getElementById("totalCartPrice");
    cartTotalElements.innerHTML = `$${totalCartPrice.toFixed(2)}`;
  
    let discountAmount = calculateDiscountAmount(); // حساب الخصم.
    let totalAfterDiscount = totalCartPrice - discountAmount;
  
    let totalAfterDiscountText = document.getElementById("totalAfterDisconte");
    totalAfterDiscountText.innerHTML = `$${totalAfterDiscount.toFixed(2)}`;
  
    localStorage.amountForPay = totalAfterDiscount.toFixed(2); // تخزين المبلغ النهائي للدفع.
  }
  async function checkOut() {
    debugger
    const userId = localStorage.getItem("UserId");
  
    if (userId && userId !== "null" && userId !== "") {
        // اجمع العناصر الحالية من localStorage
        const currentCartItems = JSON.parse(localStorage.getItem("cartItems"));
  
        // تخزين السلة الحالية في مفتاح مختلف
        localStorage.setItem("previousCartItems", JSON.stringify(currentCartItems));
  
        const url = `https://localhost:44332/api/Cart/ClearCart/${userId}`;
  
        try {
            const response = await fetch(url, {
                method: "DELETE",
            });
  
            if (response.ok) {
                const result = await response.json();
                localStorage.setItem("cartItems", JSON.stringify(result.cartItems)); // تخزين بيانات العناصر الجديدة في localStorage
                setTimeout(() => {
                    window.location.href = "/CheckOutrawa.html"; // تحويل إلى صفحة الجيك أوت
                }, 100); // تأخير 100 مللي ثانية
            } else {
                alert("حدث خطأ أثناء إفراغ السلة من الـ API.");
            }
        } catch (error) {
            console.error("خطأ أثناء إفراغ السلة:", error);
            alert("حدث خطأ أثناء إفراغ السلة.");
        }
    } else {
        alert("يجب عليك تسجيل الدخول لإستكمال عملية الشراء");
        setTimeout(() => {
            window.location.href = "/capitalshop/login.html"; // تحويل إلى صفحة تسجيل الدخول
        }, 100); // تأخير 100 مللي ثانية
    }
  }
  
  