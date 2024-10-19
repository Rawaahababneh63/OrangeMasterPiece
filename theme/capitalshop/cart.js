
/** @format */



var UserId = localStorage.getItem("UserId");
debugger
async function addToCart(productId, name, price, image) {

    if (UserId != null) {
        var url = `https://localhost:44332/api/Cart/AddCartItem/${UserId}`;
    
        var data = {
            ProductId: productId,
            Quantity: 1,
           
        };

        let response = await fetch(url, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(data),
        });

        if (response.ok) {
            alert("Product added successfully to the cart!");
            // Redirect to the cart page
            window.location.href = "cart.html"; // استبدل هذا بالرابط الصحيح
        } else {
            let error = await response.text();
            console.error("Error:", error);
        }
    } else {
        const cartItem = {
            product_id: productId,
            quantity: 1,
            name: name,
            price: price,
            image: image,
        };

        // Check if there is already a cart in localStorage
        let cartItems = JSON.parse(localStorage.getItem("cartItems")) || [];

        // Check if the product is already in the cart
        let existingItem = cartItems.find((item) => item.product_id === productId);

        if (existingItem) {
            // If the product is already in the cart, update the quantity
            existingItem.quantity = parseInt(existingItem.quantity) + 1; // Increment quantity by 1
        } else {
            // If it's a new product, add it to the cart array
            cartItems.push(cartItem);
        }

        // Save the updated cart back to localStorage
        localStorage.setItem("cartItems", JSON.stringify(cartItems));

        // تأكد من عرض تفاصيل المنتج
        alert(`Product added successfully to the cart!\nName: ${name}\nPrice: $${price}\nQuantity: ${cartItem.quantity}`);
        
        // Redirect to the cart page
        window.location.href = "cart.html"; // استبدل هذا بالرابط الصحيح
    }
}
///////////////////////////////////////////
var token = localStorage.getItem("Token");
  var userId = localStorage.getItem("UserId");
  var totalCartPrice = 0;
  var disconteAmount = 0;

  var errorMessage = document.getElementById("errorMessage");


  async function checkVoucher() {    debugger
  if (userId == null) {
    alert("يجب عليك تسجيل الدخول قبل استخدام القسيمة!");
  } else {

    var voucher = document.getElementById("inputVoucher").value;

    const url = `https://localhost:44332/api/Cart/ApplyVoucher/${voucher}`;

    try {
      var response = await fetch(url);
      var data = await response.json();

      if (!response.ok) {
        errorMessage.innerHTML = data.message;
      } else {
        disconteAmount = parseFloat(data.discount);

        var discount = (disconteAmount / 100) * totalCartPrice;
        var totalAfterDisconte = totalCartPrice - discount;

        var disconteAmountText = document.getElementById("disconteAmount");
        disconteAmountText.textContent = `$${discount.toFixed(2)}`;

        var totalAfterDisconteText =
          document.getElementById("totalAfterDisconte");
        totalAfterDisconteText.innerHTML = `$${totalAfterDisconte.toFixed(2)}`;

        localStorage.amountForPay = totalAfterDisconte.toFixed(2);

        errorMessage.innerHTML = "";

        alert("تم تطبيق القسيمة بنجاح!");
      }
    } catch (error) {
      console.error("حدث خطأ أثناء تطبيق القسيمة.", error);
      errorMessage.innerHTML = "An error occurred while applying the voucher.";
    }
  }
}

disconteAmountText = document.getElementById("disconteAmount");
disconteAmountText.textContent = `$${disconteAmount.toFixed(2)}`;



////////////هاد كود القسيمة

//   async function checkVoucher() {
//     if (userId == null) {
//       alert("You must be logged in before using a voucher!");
//     } else {
//       var voucher = document.getElementById("inputVoucher").value;
//       const url = `https://localhost:44362/api/Cart/ApplyVoucher/${voucher}`;

//       try {
//         var response = await fetch(url);
//         var data = await response.json();

//         if (!response.ok) {
//           errorMessage.innerHTML = data.message;
//         } else {
//           disconteAmount = parseFloat(data.discount);
//           var discount = (disconteAmount / 100) * totalCartPrice;
//           var totalAfterDisconte = totalCartPrice - discount;

//           document.getElementById("disconteAmount").textContent = `$${discount.toFixed(2)}`;
//           document.getElementById("totalAfterDisconte").innerHTML = `$${totalAfterDisconte.toFixed(2)}`;

//           localStorage.amountForPay = totalAfterDisconte.toFixed(2);
//           errorMessage.innerHTML = "";
//           alert("Voucher applied successfully!");
//         }
//       } catch (error) {
//         console.error("Error applying voucher:", error);
//         errorMessage.innerHTML = "An error occurred while applying the voucher.";
//       }_i
//     }
//   }




async function showItemsCart() {
    if (userId == null) {
      debugger;
      var selectedItems = JSON.parse(localStorage.getItem("cartItems"));
  
      let itemContainer = document.getElementById("table");
  
      selectedItems.forEach((item) => {
        totalCartPrice += item.price * item.quantity;
  
        // Calculate final price considering discount
        const finalPrice = item.discount ? (item.price - item.discount) : item.price;
  
        itemContainer.innerHTML += `
          <tr id="item-row-${item.product_id}">
            <td class="product_remove" style="cursor: pointer;">
              <i onclick="deleteItem1(${item.product_id})" class="fa fa-trash-o"></i>
            </td>
            <td class="product_thumb">
              <a href="#"><img src="../../../../Backend/Masterpiece/Masterpiece/Uploads/${item.image}" alt=""></a>
            </td>
            <td class="product_name">
              <a href="#">${item.name}</a>
            </td>
            <td class="product-price">$${finalPrice.toFixed(2)}</td>
            <td class="product_quantity">
              <label>Quantity</label>
              <input id="quantity-${item.product_id}" min="1" max="100"
                value="${item.quantity}" type="number"
                onchange="changeQuantity1(${item.product_id}, ${finalPrice})">
            </td>
            <td id="total-price-${item.product_id}" class="product_total">
              $${(finalPrice * item.quantity).toFixed(2)}
            </td>
          </tr>
        `;
      });
  
      updateTotalCartPrice();
    } else {
      debugger;
      let url2 = `https://localhost:44332/api/Cart/getUserCartItems/${userId}`;
  
      let request = await fetch(url2);
      let result = await request.json();
      let itemContainer = document.getElementById("table");
  
      result.forEach((item) => {
        totalCartPrice += item.product.priceWithDiscount * item.quantity;
  
        // Calculate final price considering discount
        const finalPrice = item.product.discount ? (item.product.priceWithDiscount - item.product.discount) : item.product.priceWithDiscount;
  
        itemContainer.innerHTML += `
          <tr id="item-row-${item.cartItemId}">
            <td class="product_remove" style="cursor: pointer;">
              <i onclick="deleteItem(${item.cartItemId})" class="fa fa-trash-o"></i>
            </td>
            <td class="product_thumb">
              <a href="#"><img src="../../../../Backend/Masterpiece/Masterpiece/Uploads/${item.product.image}" alt=""></a>
            </td>
            <td class="product_name">
              <a href="#">${item.product.name}</a>
            </td>
            <td class="product-price">$${finalPrice.toFixed(2)}</td>
            <td class="product_quantity">
              <label>Quantity</label>
              <input id="quantity-${item.cartItemId}" min="1" max="100"
                onchange="changeQuantity(${item.cartItemId}, ${finalPrice})"
                value="${item.quantity}" type="number">
            </td>
            <td id="total-price-${item.cartItemId}" class="product_total">
              $${(finalPrice * item.quantity).toFixed(2)}
            </td>
          </tr>
        `;
      });
  
      updateTotalCartPrice();
    }
  }
  showItemsCart();
  
  // Function to change the quantity of the cart item in local storage
  function changeQuantity1(cartItemId, priceWithDiscount) {
    const quantityInput = document.getElementById(`quantity-${cartItemId}`);
    const newQuantity = parseInt(quantityInput.value);
  
    let cartItems = JSON.parse(localStorage.getItem("cartItems"));
  
    cartItems = cartItems.map((item) => {
      if (item.product_id == cartItemId) {
        item.quantity = newQuantity;
      }
      return item;
    });
  
    localStorage.setItem("cartItems", JSON.stringify(cartItems));
  
    const totalPriceElement = document.getElementById(`total-price-${cartItemId}`);
    const updatedTotalPrice = (priceWithDiscount * newQuantity).toFixed(2);
    totalPriceElement.textContent = `$${updatedTotalPrice}`;
  
    updateTotalCartPrice(); // Update the total price without reloading the page
  }
  
  // Function to recalculate and update the total cart price
  function updateTotalCartPrice() {
    let totalCartPrice = 0;
    let cartItems = JSON.parse(localStorage.getItem("cartItems"));
  
    cartItems.forEach((item) => {
      const finalPrice = item.discount ? (item.price - item.discount) : item.price;
      totalCartPrice += parseFloat(finalPrice) * item.quantity;
    });
  
    document.getElementById("totalCartPrice").textContent = `$${totalCartPrice.toFixed(2)}`;
  
    let totalAfterDisconte = totalCartPrice - disconteAmount;
    document.getElementById("totalAfterDisconte").textContent = `$${totalAfterDisconte.toFixed(2)}`;
  
    localStorage.amountForPay = totalAfterDisconte.toFixed(2);
  }
  
// Function to change the quantity of the cart item in local storage
function changeQuantity1(cartItemId, priceWithDiscount) {
  const quantityInput = document.getElementById(`quantity-${cartItemId}`);
  const newQuantity = parseInt(quantityInput.value);

  let cartItems = JSON.parse(localStorage.getItem("cartItems"));

  cartItems = cartItems.map((item) => {
    if (item.product_id == cartItemId) {
      item.quantity = newQuantity;
    }
    return item;
  });

  localStorage.setItem("cartItems", JSON.stringify(cartItems));

  const totalPriceElement = document.getElementById(`total-price-${cartItemId}`);
  const updatedTotalPrice = (priceWithDiscount * newQuantity).toFixed(2);
  totalPriceElement.textContent = `$${updatedTotalPrice}`;

  updateTotalCartPrice(); // Update the total price without reloading the page
}

// Function to recalculate and update the total cart price
function updateTotalCartPrice() {
  let totalCartPrice = 0;
  let cartItems = JSON.parse(localStorage.getItem("cartItems"));

  cartItems.forEach((item) => {
    totalCartPrice += parseFloat(item.price) * item.quantity;
  });

  document.getElementById(
    "totalCartPrice"
  ).textContent = `$${totalCartPrice.toFixed(2)}`;

  let totalAfterDisconte = totalCartPrice - disconteAmount;
  document.getElementById("totalAfterDisconte").textContent = `$${totalAfterDisconte.toFixed(2)}`;

  localStorage.amountForPay = totalAfterDisconte.toFixed(2);
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////

function deleteItem1(productId) {
  let storedCartItems = JSON.parse(localStorage.getItem("cartItems"));

  const updatedCartItems = storedCartItems.filter(
    (item) => item.product_id != productId.toString()
  );

  localStorage.setItem("cartItems", JSON.stringify(updatedCartItems));

  const itemRow = document.getElementById(`item-row-${productId}`);
  if (itemRow) {
    itemRow.remove(); // Remove the item row without reloading the page
  }

  updateTotalCartPrice(); // Update the total price without reloading the page
}
//////////////////////////////////////////////////////////////////////////////////////////////////////////////
async function deleteItem(cartItemId) {
  let url = `https://localhost:44332/api/Cart/deleteItemById/${cartItemId}`;

  fetch(url, {
    method: "DELETE",
  }).then((response) => {
    if (response.ok) {
      const itemRow = document.getElementById(`item-row-${cartItemId}`);
      if (itemRow) {
        itemRow.remove(); // Remove the item row without reloading the page
      }
      alert("Item was deleted!");
      updateTotalCartPrice(); // Update the total price without reloading the page
    }
  });
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////

async function changeQuantity(cartItemId, productPrice) {
  const Quantity = document.getElementById(`quantity-${cartItemId}`);
  const newQuantity = parseInt(Quantity.value);

  const url = `https://localhost:44362/api/Cart/changeQuantity`;

  const response = await fetch(url, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      cartItemId: cartItemId,
      quantity: newQuantity,
    }),
  });

  const newTotalPrice = productPrice * newQuantity;

  const oldTotalPriceElement = document.getElementById(
    `total-price-${cartItemId}`
  );
  oldTotalPriceElement.innerHTML = `$${newTotalPrice.toFixed(2)}`;

  updateTotalCartPrice(); // Update the total price without reloading the page
}




///////////////////////////////////////////////////////////////////////////////////////////
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
